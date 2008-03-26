Public Class Main
    Private axe As MTPAxe

    Private originalPlaylists As Collection

    'keeps track of what column was clicked last in order to enable 
    'sorting ascending or descending if the column header is clicked more than once
    Dim playlistListView_lastColumnClicked As Short = -1

#Region "Application Menu"
    Private Sub QuitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuitToolStripMenuItem.Click
        Application.Exit()
    End Sub
    Private Sub ShowDebugWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowDebugWindowToolStripMenuItem.Click
        If Me.ShowDebugWindowToolStripMenuItem.Checked Then
            TraceOutput.Visible = True
        Else
            TraceOutput.Visible = False
        End If
    End Sub
    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("WalkmanMTP by Dr. Zoidberg" & vbCr & "v" & Application.ProductVersion, MsgBoxStyle.Information, "About")
    End Sub

#End Region


    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not axe Is Nothing Then
            If Not axe.stopAxe Then
                MsgBox("Could not close MTPAxe")
            End If
            axe = Nothing
        End If
    End Sub
    Private Sub Main_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If Me.Visible Then
            'create the custom treeview
            Me.tvPlaylistsFilesOnDevice.Name = "sdf"
            Dim multiselectTv As New MultiSelectTreeview.MultiSelectTreeview
            With multiselectTv
                .Name = "tvPlaylistsFilesOnDevice"
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
                .Location = New Point(-3, 16)
                .Size = New Size(353, 418)
            End With
            Me.initAndRefreshApp()

            TraceOutput.Visible = False
            TraceOutput.Owner = Me

            Me.Activate()

        End If
    End Sub
    Private Sub btnRefreshDevices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshDevices.Click
        initAndRefreshApp()
    End Sub
    Private Sub btnSync_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSync.Click
        If MsgBox("All non empty playlists created, modified or deleted will be sync'ed." & vbCrLf & _
                  "All albums created, modified or deleted will be updated" & vbCrLf & _
                  "Any other selected files will be uploaded or deleted", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, "Sync device?") = MsgBoxResult.Cancel Then
            Exit Sub
        End If
        Splash.setText("Syncing")
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        t.Start()

        syncPlaylists()

        'check what tab is open when this button is clicked
        'that way, we can only refresh the necessary things instead of the whole app
        If Me.tabMain.SelectedTab.Text = "Playlists" Then
            Me.refreshPlaylistsList()
        End If

        t.Abort()
    End Sub
    Private Sub cmbDevices_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDevices.SelectedIndexChanged
        Me.initSelectedDevice(Me.cmbDevices.SelectedItem.ToString)
    End Sub

    Private Sub initAndRefreshApp()
        Splash.setText("Initializing and Reading Devices")
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        t.Start()

        Me.cmbDevices.Items.Clear()

        'stop MTP axe if it is running
        If Not axe Is Nothing Then
            axe.stopAxe()
            axe = Nothing
        End If

        axe = New MTPAxe
        If Not axe.startAxe Then
            Trace.WriteLine("refreshDevices: could not start MTPAXE")
            MsgBox("refreshDevices: could not start MTPAXE")
            axe = Nothing
            Exit Sub
        End If



        'enumerate devices 
        Dim devarr() As String
        Dim ret As String
        ret = axe.enumerateDevices
        If ret = "-1" Then
            Trace.WriteLine("refreshDevices: no devices found")
            MsgBox("no devices found")
            Exit Sub
        End If

        devarr = ret.Split(":"c)

        Me.cmbDevices.Items.AddRange(devarr)

        'select the walkman device automatically
        Me.cmbDevices.SelectedItem = "WALKMAN"

        t.Abort()

    End Sub
    Private Sub initSelectedDevice(ByVal devName As String)
        If axe Is Nothing Then
            Trace.WriteLine("initSelectedDevice: MTPAxe is not initialized")
            MsgBox("initSelectedDevice: MTPAxe is not initialized")
            Exit Sub
        End If

        Cursor.Current = Cursors.WaitCursor

        Try
            'IMPORTANT: set the active device
            If axe.setCurrentDevice(devName) = "-1" Then
                Trace.WriteLine("initSelectedDevice: error setting " & devName & " to the current device")
                Exit Sub
            End If

            refreshPlaylistDeviceFiles()
            refreshFileTransfersDeviceFiles()
            refreshPlaylistsList()

            Me.lblManufacturer.Text = axe.getDeviceManufacturer()

            'get capacity
            Dim ret() As String
            ret = axe.getDeviceCapacity().Split(":"c)
            If ret(0) = "-1" Then
                Trace.WriteLine("initSelectedDevice: error getting device capacity")
                Exit Sub
            End If
            Dim freeSpace, capacity As Long
            capacity = Long.Parse(ret(0)) / 1024 / 1024 'to MB
            freeSpace = Long.Parse(ret(1)) / 1024 / 1024
            'free space doesnt seem to work...
            'Me.lblCapacity.Text = freeSpace & "MB of " & capacity & " MB"
            Me.lblCapacity.Text = capacity & " MB"

            'get the icon
            Dim iconPath, retstr As String, mtpIcon As Icon
            iconPath = System.IO.Path.Combine(System.IO.Path.GetTempPath, "DevIcon.fil")
            retstr = axe.getDeviceIcon(iconPath.Replace("\"c, "\\"))
            If Not retstr = "-1" Then
                mtpIcon = New Icon(iconPath, New Size(500, 500))
                Me.pboxDevIcon.Image = mtpIcon.ToBitmap
            End If
        Catch ex As Exception
            Trace.WriteLine("Error initializing '" & devName & "'" & vbCrLf & ex.Message)
            MsgBox("Error initializing '" & devName & "'" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try

        Cursor.Current = Cursors.Default

    End Sub

    Private Sub syncPlaylists()
        'the way playlist syncing works is the following (in order of execution):
        '0.any playlist ending with * is a new or modified playlist
        '1.any playlist ending with * is deleted from the player
        '2.any non empty playlist ending with * is then re-created on the player


        Dim tpage As TabPage

        'first search for playlists to delete and then delte then
        For Each tpage In originalPlaylists
            'check the remaining tabs to see if there's a match. if there
            'is no match, the playlist was deleted, renamed, or changed
            'so we must delete this playlist from the player
            Dim matchfound As Boolean = False
            For Each playlist As TabPage In Me.tabPlaylists.TabPages
                'remember the tag holds the unique identifier for the playlist
                If tpage.Text = playlist.Text And tpage.Tag = playlist.Tag Then
                    'if there is a match, there is no need to delete it
                    matchfound = True
                    Exit For
                End If
            Next

            If Not matchfound Then
                'delete it
                'note: MTPAxe only deletes the first playlist with matching name
                'so the wrong one could still be deleted
                If axe.deletePlaylist(tpage.Text) = "-1" Then
                    Trace.WriteLine("Sync: error deleting playlist " & tpage.Text)
                    MsgBox("Sync: error deleting playlist " & tpage.Text, MsgBoxStyle.Critical)
                    Exit Sub
                End If
            End If
        Next

        'any playlist ending in '*' that is not empty needs to be added to the player
        Dim lv As ListView
        Dim attribs() As String
        Dim nodeLevel As Short
        Dim nodeType As Integer
        Dim nodeParent As String
        Dim str As String
        For Each tpage In Me.tabPlaylists.TabPages
            If tpage.Text.EndsWith("*") Then
                lv = tpage.Controls("lvPl" & tpage.Text)

                'only add non-empty playlists
                If lv.Items.Count > 0 Then
                    str = ""
                    For Each lvitem As ListViewItem In lv.Items
                        'get the item attributes
                        attribs = lvitem.Tag.Split(","c)
                        nodeLevel = attribs(0)
                        nodeType = attribs(1)
                        nodeParent = attribs(2)
                        'add each item to the string
                        str = str & "<" & nodeLevel & "," & nodeType & "," & nodeParent & ">" & lvitem.Text & ":"
                    Next
                    'remove the trailing ':'
                    str = str.Remove(str.Length - 1, 1)

                    If axe.createPlaylist(tpage.Text.Remove(tpage.Text.Length - 1, 1), str) = "-1" Then
                        Trace.WriteLine("Sync: error creating playlist " & tpage.Text)
                        MsgBox("Sync: error creating playlist " & tpage.Text, MsgBoxStyle.Critical)
                        Exit Sub
                    End If

                End If 'lv.items.count
            End If 'tpage.text.endswith
        Next

    End Sub

    

#Region "Playlists"
    Private Sub btnPlaylistsFilesOnDeviceRefresh_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnPlaylistsFilesOnDeviceRefresh.LinkClicked
        Cursor.Current = Cursors.WaitCursor
        refreshPlaylistDeviceFiles()
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub btnDeleteAllLists_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnDeleteAllLists.LinkClicked
        If MsgBox("Are you sure?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Really?") = MsgBoxResult.No Then
            Exit Sub
        End If

        Me.tabPlaylists.TabPages.Clear()

    End Sub
    Private Sub btnDelPlaylist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelPlaylist.Click
        deleteActivePlaylist()
    End Sub
    Private Sub btnRenamePlaylist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRenamePlaylist.Click
        Dim newname As String
        newname = InputBox("Enter new name: ", "Rename Playlist")
        renameActivePlaylist(newname)
    End Sub
    Private Sub btnAddPlaylist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPlaylist.Click
        Dim newname As String
        newname = InputBox("Enter playlist name: ", "Create Playlist")

        'check to see if the playlist name already exists
        For Each tpage As TabPage In Me.tabPlaylists.TabPages
            If String.Equals(newname & "*", tpage.Text, StringComparison.CurrentCultureIgnoreCase) Or String.Equals(newname, tpage.Text, StringComparison.CurrentCultureIgnoreCase) Then
                If MsgBox("Warning: there is already a playlist with the same name. Unpredictable behaviour may occur. Continue anyways?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Duplicate name") = MsgBoxResult.No Then
                    Exit Sub
                Else
                    Exit For
                End If
            End If
        Next

        Me.tabPlaylists.SelectTab(createNewPlaylist(newname, False))
    End Sub


    Private Sub tvPlaylistsFilesOnDevice_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles tvPlaylistsFilesOnDevice.ItemDrag
        ' Move the dragged node when the left mouse button is used.
        If e.Button = MouseButtons.Left Then
            DoDragDrop(Me.tvPlaylistsFilesOnDevice.SelectedNodes, DragDropEffects.Move)
        End If

    End Sub
    Private Sub playlistListView_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs)
        Dim lv As ListView = CType(sender, ListView)
        If e.Column <> playlistListView_lastColumnClicked Then
            lv.Sorting = SortOrder.Ascending
            lv.Columns(e.Column).Text = lv.Columns(e.Column).Text & " ^"
            If Not playlistListView_lastColumnClicked = -1 Then
                'clean up the title of the previously clicked column
                lv.Columns(playlistListView_lastColumnClicked).Text = lv.Columns(playlistListView_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            End If
            playlistListView_lastColumnClicked = e.Column
        Else
            ' Determine what the last sort order was and change it.
            If lv.Sorting = SortOrder.None Then
                lv.Sorting = SortOrder.Ascending
                lv.Columns(e.Column).Text = lv.Columns(e.Column).Text & " ^"
            ElseIf lv.Sorting = SortOrder.Ascending Then
                lv.Sorting = SortOrder.Descending
                lv.Columns(e.Column).Text = lv.Columns(e.Column).Text.Replace(" ^", " v")
            Else
                lv.Sorting = SortOrder.None
                lv.Columns(e.Column).Text = lv.Columns(e.Column).Text.Replace(" v", "")
            End If

        End If

        If lv.Sorting = SortOrder.None Then
            lv.ListViewItemSorter = Nothing
        Else
            lv.ListViewItemSorter = New PlaylistListViewItemComparer(e.Column, lv.Sorting)
        End If



    End Sub
    Private Sub playlistListView_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        'if DEL is pressed, delete selected items
        If e.KeyCode = Keys.Delete Then
            Dim lv As ListView = CType(sender, ListView)

            For Each lvItem As ListViewItem In lv.SelectedItems
                lv.Items.Remove(lvItem)
            Next

        End If
    End Sub
    Private Sub playlistListView_dragenter(ByVal sender As Object, ByVal e As DragEventArgs)
        e.Effect = e.AllowedEffect
    End Sub
    Private Sub playlistListView_dragdrop(ByVal sender As Object, ByVal e As DragEventArgs)
        Dim draggedNodes As List(Of TreeNode) = CType(e.Data.GetData(GetType(List(Of TreeNode))), List(Of TreeNode))
        Dim lv As ListView = CType(sender, ListView)

        lv.SmallImageList = Me.tvPlaylistsFilesOnDevice.ImageList

        For Each draggedNode In draggedNodes
            playlistListView_dragdrop_helper(lv, draggedNode)
        Next
    End Sub
    Private Sub playlistListView_dragdrop_helper(ByRef lv As ListView, ByVal root As TreeNode)
        'lv is the listview to add the nodes to. root is the node to add (if its a file) or to
        'traverse (if its a folder)
        Dim attribs() As String
        Dim nodeLevel As Short
        Dim nodeType As Integer
        Dim nodeParent As String
        Dim lvItem As ListViewItem

        'check to see if the root is a file, if it is add it to the list view
        'if it's not, then it's a folder and we must recurse it
        attribs = root.Tag.Split(","c)
        nodeLevel = attribs(0)
        nodeType = attribs(1)
        nodeParent = attribs(2)
        If (nodeType And MTPAxe.WMDM_FILE_ATTR_FILE) = MTPAxe.WMDM_FILE_ATTR_FILE Then
            'the root node is a file
            lvItem = New ListViewItem
            lvItem.Text = root.Text
            lvItem.ImageKey = root.ImageKey
            lvItem.Tag = root.Tag
            lv.Items.Add(lvItem)
        Else
            'the root is a folder and we must traverse it and any subfolders
            For Each node As TreeNode In root.Nodes
                playlistListView_dragdrop_helper(lv, node)
            Next
        End If


    End Sub

    Private Sub refreshPlaylistDeviceFiles()
        Dim tv As TreeView

        Me.tvPlaylistsFilesOnDevice.Nodes.Clear()

        tv = axe.getTreeViewByName("MUSIC")
        Me.tvPlaylistsFilesOnDevice.ImageList = tv.ImageList

        'enumerate storage on the device (necessary for all other device related functions to work)
        'can use this enumeration to fill the directory tree
        For Each node As TreeNode In tv.Nodes
            Me.tvPlaylistsFilesOnDevice.Nodes.Add(node.Clone)
        Next
        Me.tvPlaylistsFilesOnDevice.ExpandAll()
    End Sub
    Private Sub refreshPlaylistsList()
        Dim tv, tv2 As TreeView

        originalPlaylists = New Collection

        tv = axe.getTreeViewByName("Playlists")
        Me.tvPlaylistsFilesOnDevice.ImageList = tv.ImageList


        Me.tabPlaylists.TabPages.Clear()

        'create a blacnk playlist by default
        createNewPlaylist("New Playlist", False)

        'enumerate storage on the device (necessary for all other device related functions to work)
        'can use this enumeration to fill the directory tree

        Dim lv As ListView
        Dim tpage As TabPage
        Dim lvItem As ListViewItem
        For Each node As TreeNode In tv.Nodes(0).Nodes
            tv2 = axe.getPlaylistContentsAsTreeview(node.Text)

            'create a new playlist tab and listview based on the plalist name
            tpage = createNewPlaylist(node.Text, True)
            lv = tpage.Controls("lvPl" & node.Text)
            lv.SmallImageList = tv.ImageList
            For Each plItem As TreeNode In tv2.Nodes
                'fill the contents of the treeview with the playlist contents
                lvItem = New ListViewItem

                lvItem.Text = (plItem.Text)
                lvItem.ImageKey = IO.Path.GetExtension(lvItem.Text)
                lvItem.Tag = plItem.Tag

                lv.Items.Add(lvItem)
            Next

            originalPlaylists.Add(tpage)
        Next
    End Sub


    Private Sub deleteActivePlaylist()
        Dim tpage As TabPage, selTabIndex As Integer
        tpage = Me.tabPlaylists.SelectedTab
        selTabIndex = Me.tabPlaylists.SelectedIndex

        'select the previous tab, then remove the page
        If selTabIndex > 0 Then
            Me.tabPlaylists.SelectedIndex = selTabIndex - 1
        End If
        Me.tabPlaylists.TabPages.Remove(tpage)
        If Me.tabPlaylists.TabPages.Count = 0 Then
            createNewPlaylist("New Playlist", False)
        End If
    End Sub
    Private Function createNewPlaylist(ByVal name As String, ByVal isFromPlayer As Boolean) As TabPage
        If name = "" Then
            Return Nothing
        End If

        If Not isFromPlayer Then
            name = name & "*"
        End If

        Dim tpage As TabPage
        Dim lv As ListView

        tpage = New TabPage(name)
        tpage.Name = "tpPl" & name
        lv = New ListView
        With lv
            .Dock = DockStyle.Fill
            .GridLines = True
            .ShowItemToolTips = True
            .View = View.Details
            .AllowDrop = True
            .FullRowSelect = True
            .Columns.Add("File Name", 120, HorizontalAlignment.Left)
            .Columns.Add("Title", 80, HorizontalAlignment.Left)
            .Columns.Add("Artist", 80, HorizontalAlignment.Left)
            .Columns.Add("Album", 80, HorizontalAlignment.Left)
            .Columns.Add("Year", 80, HorizontalAlignment.Left)
            .Name = "lvPl" & name
            AddHandler .DragEnter, AddressOf playlistListView_dragenter
            AddHandler .DragDrop, AddressOf playlistListView_dragdrop
            AddHandler .KeyDown, AddressOf playlistListView_KeyDown
            AddHandler .ColumnClick, AddressOf playlistListView_ColumnClick
        End With
        'use the tag of teh TabPage to keep track of the playlists (in case they have the same name
        'we need a way to differentiate them)
        tpage.Tag = Now.Ticks
        tpage.Controls.Add(lv)
        Me.tabPlaylists.TabPages.Add(tpage)

        Return tpage
    End Function
    Private Sub renameActivePlaylist(ByVal newName As String)
        If newName = "" Then
            Exit Sub
        End If

        Dim tpage As TabPage

        tpage = Me.tabPlaylists.SelectedTab

        If newName.EndsWith("*") Then
            tpage.Text = newName
        Else
            tpage.Text = newName & "*"
        End If
    End Sub
#End Region

#Region "FileTransfers"

    Private Sub refreshFileTransfersDeviceFiles()
        Dim tv As TreeView

        tv = axe.getFullTreeView
        Me.TreeView1.ImageList = tv.ImageList
        Me.TreeView1.Nodes.Clear()

        'enumerate storage on the device (necessary for all other device related functions to work)
        'can use this enumeration to fill the directory tree
        For Each node As TreeNode In tv.Nodes
            Me.TreeView1.Nodes.Add(node.Clone)
        Next
        Me.TreeView1.ExpandAll()
    End Sub
#End Region


    'comparer for playlistitems listview sorting
    Private Class PlaylistListViewItemComparer
        Implements IComparer
        'the column to sort by
        Private whichColumn As Integer
        Private whichSortOrder As SortOrder

        Public Sub New()
            whichColumn = 0
        End Sub

        Public Sub New(ByVal selectedColumnIndex As Integer, ByVal order As SortOrder)
            whichColumn = selectedColumnIndex
            whichSortOrder = order
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            'called by listview.sort
            'if x<y return -1
            'if x=y return 0
            'if x>y return 1

            Dim ret As Integer = -1
            Dim subitem_x As ListViewItem.ListViewSubItem
            Dim subitem_y As ListViewItem.ListViewSubItem

            'don't allow any other column other than 0 for now
            'since the subitems are not yet implemented
            If whichColumn <> 0 Then
                Return 0
            End If

            'subitem 0 is the main item
            subitem_x = CType(x, ListViewItem).SubItems(whichColumn)
            subitem_y = CType(y, ListViewItem).SubItems(whichColumn)
            ret = String.Compare(subitem_x.Text, subitem_y.Text, True)

            If whichSortOrder = SortOrder.Descending Then
                ' Invert the value returned by String.Compare.
                ret *= -1
            End If


            Return ret
        End Function
    End Class


End Class