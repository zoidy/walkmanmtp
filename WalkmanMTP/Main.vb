Imports WalkmanMTP.ListViewDnD
Public Class Main
    Private axe As MTPAxe

    'used for keeping track of the complete file listing on the device
    Private fullFileListing As TreeView

    'keeps track of what the original playlists were as of the last time
    ' the playlists list was refreshed. this is to be able to
    'distinguish which lists have to be deleted and added
    Private originalPlaylists As Collection

    'keeps track of what column was clicked last in order to enable 
    'sorting ascending or descending if the column header is clicked more than once
    Dim playlistListView_lastColumnClicked As Short = -1
    Dim lvFileManagementDeviceFilesInFolder_lastColumnClicked As Short = -1

#Region "Application Menu"
    Private Sub QuitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuitToolStripMenuItem.Click
        Application.Exit()
    End Sub
    Private Sub ShowDebugWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowDebugWindowToolStripMenuItem.Click
        If Me.ShowDebugWindowToolStripMenuItem.Checked Then
            TraceOutput.Visible = True
            TraceOutput.WindowState = FormWindowState.Normal
        Else
            TraceOutput.Visible = False
        End If
    End Sub
    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("WalkmanMTP by Dr. Zoidberg v" & Application.ProductVersion & vbCrLf & _
               "Taglib# v" & System.Reflection.Assembly.GetAssembly(GetType(TagLib.File)).GetName.Version.ToString, MsgBoxStyle.Information, "About")

    End Sub
    Private Sub InformationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InformationToolStripMenuItem.Click
        Dim readme As String = IO.Path.Combine(Application.StartupPath, "readme.txt")
        Try
            Process.Start(readme)
        Catch ex As Exception
            MsgBox("Couldn't open " & readme)
        End Try
    End Sub
    Private Sub SyncDeviceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SyncDeviceToolStripMenuItem.Click
        btnSync_Click(Nothing, Nothing)
    End Sub
#End Region


    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not axe Is Nothing Then
            If Not axe.stopAxe Then
                MsgBox("Could not close MTPAxe")
                Trace.WriteLine("Main form Closing: could not close MTPAxe")
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

            TraceOutput.Show(Me)
            TraceOutput.Visible = False

            Trace.WriteLine("Application Starting...")
            Me.initAndRefreshApp()
            Me.Activate()
            Trace.WriteLine("Application Starting...Completed")
        End If
    End Sub
    Private Sub btnDeviceDetails_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnDeviceDetails.LinkClicked
        MsgBox("To Do")
    End Sub
    Private Sub btnRefreshDevices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshDevices.Click
        initAndRefreshApp()
    End Sub
    Private Sub btnSync_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSync.Click
        If MsgBox("All non empty playlists created, modified or deleted will be sync'ed." & vbCrLf & _
                  "All albums created, modified or deleted will be updated", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Sync device?") = MsgBoxResult.No Then
            Exit Sub
        End If
        
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        If Not Splash.Visible Then t.Start()
        Splash.setText("Syncing")
        Splash.setTitle("Syncing...")

        Trace.WriteLine("Start sync operation...")
        syncPlaylists()
        Trace.WriteLine("Start sync operation...Completed")

        refreshFullDirectoryTree()
        refreshPlaylistsList()
        refreshFileTransfersDeviceFiles()
        refreshPlaylistDeviceFiles()

        t.Abort()
    End Sub
    Private Sub cmbDevices_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDevices.SelectedIndexChanged
        Me.initSelectedDevice(Me.cmbDevices.SelectedItem.ToString)
    End Sub

    Private Sub initAndRefreshApp()

        Splash.Close()
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        t.Start()
        Splash.setText("Initializing and Reading Devices")
        Splash.setTitle("Initializing...")

        Me.cmbDevices.Items.Clear()

        'stop MTP axe if it is running
        If Not axe Is Nothing Then
            axe.stopAxe()
            axe = Nothing
        End If

        Trace.WriteLine("initAndRefresh: opening MTPAxe")
        axe = New MTPAxe
        If Not axe.startAxe Then
            Splash.Close()
            Trace.WriteLine("initAndRefresh: could not start MTPAXE")
            MsgBox("initAndRefresh: could not start MTPAXE", MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
            axe = Nothing
            Exit Sub
        End If

        If Not Me.originalPlaylists Is Nothing Then
            Me.originalPlaylists.Clear()
            Me.originalPlaylists = Nothing
        End If
        deleteAllPlaylists()
        Me.tvFileManagementDeviceFolders.Nodes.Clear()
        Me.tvPlaylistsFilesOnDevice.Nodes.Clear()


        'enumerate devices 
        Trace.WriteLine("initAndRefresh: enumerating devices")
        Dim devarr() As String
        Dim ret As String
        ret = axe.enumerateDevices
        If ret = "-1" Then
            Splash.Close()
            Trace.WriteLine("initAndRefresh: no devices found")
            MsgBox("no devices found", MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
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
            MsgBox("initSelectedDevice: MTPAxe is not initialized", MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
            Exit Sub
        End If

        'let the user know we're busy
        Cursor.Current = Cursors.WaitCursor
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        If Not Splash.Visible Then t.Start()
        Splash.setTitle("Initializing " & devName)

        Try
            'IMPORTANT: set the active device
            Trace.WriteLine("initSelectedDevice: initializing device " & devName)
            If axe.setCurrentDevice(devName) = "-1" Then
                Trace.WriteLine("initSelectedDevice: error setting " & devName & " to the current device")
                Exit Sub
            End If

            'first refresh the file listing. the list needs to be up to date
            'so the other refresh functions can use it
            refreshFullDirectoryTree()
            'refresh the various lists and trees
            refreshPlaylistDeviceFiles()
            refreshFileTransfersDeviceFiles()
            refreshPlaylistsList()

            Splash.setText("Reading device info")
            Me.lblManufacturer.Text = axe.getDeviceManufacturer()

            'get capacity
            Trace.WriteLine("initSelectedDevice: getting device capacity")
            Dim ret() As String
            ret = axe.getDeviceCapacity().Split(":"c)
            If ret(0) = "-1" Then
                Trace.WriteLine("initSelectedDevice: error getting device capacity")
                Exit Sub
            End If
            Dim freeSpace, capacity As Long
            capacity = Long.Parse(ret(0)) / 1024 / 1024 'to MB
            freeSpace = Long.Parse(ret(1)) / 1024 / 1024

            Me.lblCapacity.Text = freeSpace & " of " & capacity & " MB"

            'get the icon
            Trace.WriteLine("initSelectedDevice: getting device icon")
            Splash.setText("Reading device icon")
            Dim iconPath, retstr As String, mtpIcon As Icon
            iconPath = System.IO.Path.Combine(System.IO.Path.GetTempPath, "DevIcon.fil")
            retstr = axe.getDeviceIcon(iconPath.Replace("\"c, "\\"))
            If Not retstr = "-1" Then
                mtpIcon = New Icon(iconPath, New System.Drawing.Size(48, 48))
                Me.pboxDevIcon.Image = mtpIcon.ToBitmap
            Else
                Trace.WriteLine("initSelectedDevice: device icon not found")
            End If
        Catch ex As Exception
            Trace.WriteLine("initSelectedDevice: Error initializing '" & devName & "'" & ":" & ex.Message & " in " & ex.Source)
            MsgBox("Error initializing '" & devName & "'" & vbCrLf & ex.Message & " in " & ex.Source, MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
        End Try

        Cursor.Current = Cursors.Default
        t.Abort()

    End Sub

    



#Region "Playlists"
    Private Sub syncPlaylists()
        'the way playlist syncing works is the following (in order of execution):
        '0.any playlist ending with * is a new or modified playlist
        '1.any playlist ending with * is deleted from the player
        '2.any non empty playlist ending with * is then re-created on the player

        Trace.WriteLine("Syncing playlists...Deleting old playlists")

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
                    MsgBox("Sync: error deleting playlist " & tpage.Text, MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
                    Exit Sub
                End If
            End If
        Next

        Trace.WriteLine("Syncing playlists...adding new playlists")

        'any playlist ending in '*' that is not empty needs to be added to the player
        Dim lv As ListView
        Dim item As StorageItem
        Dim str As String
        For Each tpage In Me.tabPlaylists.TabPages
            If tpage.Text.EndsWith("*") Then
                lv = tpage.Controls("lvPl" & tpage.Text)

                'only add non-empty playlists
                If lv.Items.Count > 0 Then
                    str = ""
                    For Each lvitem As ListViewItem In lv.Items
                        'get the item attributes
                        item = CType(lvitem.Tag, StorageItem)
                        'add each item to the string
                        str = str & item.ID & ":"c
                    Next
                    'remove the trailing ':'
                    str = str.Remove(str.Length - 1, 1)

                    If axe.createPlaylist(tpage.Text.Remove(tpage.Text.Length - 1, 1), str) = "-1" Then
                        Trace.WriteLine("Sync: error creating playlist " & tpage.Text)
                        MsgBox("Sync: error creating playlist " & tpage.Text, MsgBoxStyle.Critical Or MsgBoxStyle.Critical)
                        Exit Sub
                    End If

                End If 'lv.items.count
            End If 'tpage.text.endswith
        Next

        Trace.WriteLine("Syncing playlists...Done")
    End Sub

    Private Sub btnPlaylistsFilesOnDeviceRefresh_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnPlaylistsFilesOnDeviceRefresh.LinkClicked
        Cursor.Current = Cursors.WaitCursor
        refreshFullDirectoryTree()
        refreshPlaylistDeviceFiles()

        Dim lv As ListView
        For Each tpage In tabPlaylists.TabPages
            're-add the image list since the icons may have been previously freed
            lv = tpage.Controls(tpage.Name.Replace("tpPl", "lvPl"))
            If Not lv Is Nothing Then
                lv.SmallImageList = Me.tvPlaylistsFilesOnDevice.ImageList
            End If
        Next
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub btnDeleteAllLists_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnDeleteAllLists.LinkClicked
        If MsgBox("Are you sure?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Really?") = MsgBoxResult.No Then
            Exit Sub
        End If

        deleteAllPlaylists()
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
        If newname = "" Then Exit Sub

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
        Dim lv As ListViewEx = CType(sender, ListView)
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
        Dim lv As ListView = CType(sender, ListViewEx)

        'if DEL is pressed, delete selected items
        If e.KeyCode = Keys.Delete Then
            For Each lvItem As ListViewItem In lv.SelectedItems
                lv.Items.Remove(lvItem)
            Next

            markPlaylistChanged(lv)
        End If

    End Sub
    Private Sub tvPlaylistsFilesOnDevice_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvPlaylistsFilesOnDevice.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            'get the node under the right click
            Dim targetNode As TreeNode = tvFileManagementDeviceFolders.GetNodeAt(tvFileManagementDeviceFolders.PointToClient(New Point(e.X, e.Y)))
            targetNode = tvFileManagementDeviceFolders.SelectedNode
            If targetNode Is Nothing Then
                Exit Sub
            Else
                mnuTvPlaylistFilesRightClick.Show(CType(sender, TreeView).PointToScreen(e.Location))
            End If
        End If
    End Sub
    Private Sub CollapseChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollapseChildrenToolStripMenuItem.Click
        Me.tvPlaylistsFilesOnDevice.BeginUpdate()
        For Each node As TreeNode In Me.tvPlaylistsFilesOnDevice.SelectedNodes
            node.Collapse(False)
        Next
        Me.tvPlaylistsFilesOnDevice.EndUpdate()
    End Sub
    Private Sub ExpandChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpandChildrenToolStripMenuItem.Click
        Me.tvPlaylistsFilesOnDevice.BeginUpdate()
        For Each node As TreeNode In Me.tvPlaylistsFilesOnDevice.SelectedNodes
            node.ExpandAll()
        Next
        Me.tvPlaylistsFilesOnDevice.EndUpdate()
    End Sub

    Private Sub playlistListView_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        'show the right click menu on mouse click
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim lv As ListViewEx = CType(sender, ListViewEx)

            'get the node under the right click
            Dim selNode As ListViewItem = lv.SelectedItems(0)
            If selNode IsNot Nothing Then
                mnuLvPlaylistContentsRightClick.Show(lv.PointToScreen(e.Location))
            End If
        End If
    End Sub
    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        'get the current playlist
        Dim tpage As TabPage = Me.tabPlaylists.SelectedTab
        Dim lv As ListViewEx = tpage.Controls(tpage.Name.Replace("tpPl", "lvPl"))
        If lv Is Nothing Then Exit Sub

        For Each lvItem As ListViewItem In lv.SelectedItems
            lv.Items.Remove(lvItem)
        Next

        markPlaylistChanged(lv)
    End Sub
    Private Sub SelctionSortAscendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelctionSortAscendingToolStripMenuItem.Click
        'get the current playlist
        Dim tpage As TabPage = Me.tabPlaylists.SelectedTab
        Dim lv As ListViewEx = tpage.Controls(tpage.Name.Replace("tpPl", "lvPl"))
        If lv Is Nothing Then Exit Sub

        'disable the sorting on the listview so we can manually sort stuff without the listview automatically resorting
        If Not playlistListView_lastColumnClicked = -1 Then
            'clean up the title of the previously clicked column
            lv.Columns(playlistListView_lastColumnClicked).Text = lv.Columns(playlistListView_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            playlistListView_lastColumnClicked = -1
        End If
        lv.ListViewItemSorter = Nothing
        lv.Sorting = SortOrder.None

        listviewItemSortSelected(lv, SortOrder.Descending)

        markPlaylistChanged(lv)
    End Sub
    Private Sub SelectionSortDescendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectionSortDescendingToolStripMenuItem.Click
        'get the current playlist
        Dim tpage As TabPage = Me.tabPlaylists.SelectedTab
        Dim lv As ListViewEx = tpage.Controls(tpage.Name.Replace("tpPl", "lvPl"))
        If lv Is Nothing Then Exit Sub

        'disable the sorting on the listview so we can manually sort stuff without the listview automatically resorting
        If Not playlistListView_lastColumnClicked = -1 Then
            'clean up the title of the previously clicked column
            lv.Columns(playlistListView_lastColumnClicked).Text = lv.Columns(playlistListView_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            playlistListView_lastColumnClicked = -1
        End If
        lv.ListViewItemSorter = Nothing
        lv.Sorting = SortOrder.None

        listviewItemSortSelected(lv, SortOrder.Ascending)

        markPlaylistChanged(lv)
    End Sub

    Private Sub playlistListView_dragenter(ByVal sender As Object, ByVal e As DragEventArgs)
        e.Effect = e.AllowedEffect
    End Sub
    Private Sub playlistListView_dragdrop(ByVal sender As Object, ByVal e As DragEventArgs)
        Dim draggedNodes As List(Of TreeNode)
        Dim lv As ListViewEx

        draggedNodes = CType(e.Data.GetData(GetType(List(Of TreeNode))), List(Of TreeNode))
        lv = CType(sender, ListViewEx)

        'check to see if the dragged data comes from our treeview
        If Not draggedNodes Is Nothing Then
            For Each draggedNode In draggedNodes
                playlistListView_dragdrop_helper(lv, draggedNode)
            Next
            markPlaylistChanged(lv)
        Else
            'if it's not from the treeview, check to see if it's from the ListViewEx itself
            Dim draggedListviewItem As Object
            draggedListviewItem = e.Data.GetData(GetType(ListViewEx.DragItemData))
            If Not draggedListviewItem Is Nothing Then
                'clean up the columheaders from sorting indicator and disable sorting
                lv.Sorting = SortOrder.None
                lv.ListViewItemSorter = Nothing
                If playlistListView_lastColumnClicked <> -1 Then
                    lv.Columns(playlistListView_lastColumnClicked).Text = lv.Columns(playlistListView_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
                End If
                markPlaylistChanged(lv)
            End If
        End If

    End Sub
    Private Sub playlistListView_dragdrop_helper(ByRef lv As ListViewEx, ByVal root As TreeNode)
        'lv is the listview to add the nodes to. root is the node to add (if its a file) or to
        'traverse (if its a folder)
        Dim item As StorageItem
        Dim lvItem As ListViewItem

        'check to see if the root is a file, if it is add it to the list view
        'if it's not, then it's a folder and we must recurse it
        item = CType(root.Tag, StorageItem)
        If (item.StorageType And MTPAxe.WMDM_FILE_ATTR_FILE) = MTPAxe.WMDM_FILE_ATTR_FILE Then
            'the root node is a file. add only .mp3,wma,mp4,m4a,3gp,wav audio files
            'tip: can add video files to playlists too as long as the extension is mp4, but the player will only play the audio.
            If root.ImageKey = ".mp3" Or root.ImageKey = ".wma" Or root.ImageKey = ".mp4" Or root.ImageKey = ".m4a" Or root.ImageKey = ".3gp" Or root.ImageKey = ".wav" Then
                lvItem = New ListViewItem
                lvItem.Text = root.Text
                lvItem.SubItems.Add(item.Title)
                lvItem.SubItems.Add(item.AlbumArtist)
                lvItem.SubItems.Add(item.AlbumTitle)
                lvItem.SubItems.Add(item.Year)
                lvItem.SubItems.Add(item.TrackNum)
                lvItem.SubItems.Add(item.Genre)
                lvItem.ImageKey = root.ImageKey
                lvItem.Tag = root.Tag
                lv.Items.Add(lvItem)
            End If
        Else
            'the root is a folder and we must traverse it and any subfolders
            For Each node As TreeNode In root.Nodes
                playlistListView_dragdrop_helper(lv, node)
            Next
        End If


    End Sub

    Private Sub refreshPlaylistDeviceFiles()
        Dim musicFldr As TreeNode

        Trace.WriteLine("Getting music files...")

        'let the user know were busy
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        If Not Splash.Visible Then
            t.Start()
            Splash.setTitle(Me.cmbDevices.Text)
        End If
        Splash.setText("Getting music files")

        Me.tvPlaylistsFilesOnDevice.BeginUpdate()

        Me.tvPlaylistsFilesOnDevice.Nodes.Clear()

        musicFldr = findTreeNodeByName(fullFileListing.Nodes(0), "MUSIC")

        Me.tvPlaylistsFilesOnDevice.Nodes.Add(musicFldr.Clone)

        Me.tvPlaylistsFilesOnDevice.Sort()
        Me.tvPlaylistsFilesOnDevice.ExpandAll()

        Me.tvPlaylistsFilesOnDevice.EndUpdate()

        t.Abort()
        Trace.WriteLine("Getting music files...Complete")
    End Sub
    Private Sub refreshPlaylistsList()
        'note: this function only uses the fullFileListing tree for finding the playlists
        'not for finding the playlist contents.  This is because the fullFileListing tree
        'does not contain the reference information contained inside of the playlists.
        'As a result it is still necessary to call some enumeration functions on the device

        Trace.WriteLine("Refreshing playlists...")

        'let the user know were busy
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        If Not Splash.Visible Then
            t.Start()
            Splash.setTitle(Me.cmbDevices.Text)
        End If

        Splash.setText("Getting playlists")

        originalPlaylists = New Collection

        'create a blacnk playlist by default
        deleteAllPlaylists()
        createNewPlaylist("New Playlist", False)

        Dim plFldr As TreeNode
        plFldr = findTreeNodeByName(fullFileListing.Nodes(0), "Playlists")
        If plFldr Is Nothing Then
            Trace.WriteLine("Error refreshing playlists - plyalists folder not found")
            Exit Sub
        End If
        If plFldr.Nodes.Count = 0 Then
            Trace.WriteLine("Error refreshing playlists - no playlists found")
            Exit Sub
        End If

        'enumerate storage on the device (necessary for all other device related functions to work)
        'can use this enumeration to fill the directory tree

        Dim lv As ListViewEx
        Dim tpage, originalTabPage As TabPage
        Dim lvItem As ListViewItem
        Dim plItem As TreeNode
        Dim item As StorageItem
        Dim strIDs() As String
        For Each node As TreeNode In plFldr.Nodes
            strIDs = axe.getPlaylistContentsIDs(node.Text)
            If strIDs IsNot Nothing Then
                'create a new playlist tab and listview based on the playlist name
                tpage = createNewPlaylist(node.Text, True)
                lv = tpage.Controls("lvPl" & node.Text)

                For Each id As String In strIDs
                    If fullFileListing IsNot Nothing AndAlso fullFileListing.Nodes.Count > 0 Then
                        'search for the id in the fulltree
                        plItem = findTreeNodeByID(fullFileListing.Nodes(0), id)
                        If plItem IsNot Nothing Then
                            item = CType(plItem.Tag, StorageItem)

                            'fill the contents of the treeview with the playlist contents
                            lvItem = New ListViewItem

                            lvItem.Text = item.FileName
                            lvItem.SubItems.Add(item.Title)
                            lvItem.SubItems.Add(item.AlbumArtist)
                            lvItem.SubItems.Add(item.AlbumTitle)
                            lvItem.SubItems.Add(item.Year)
                            lvItem.SubItems.Add(item.TrackNum)
                            lvItem.SubItems.Add(item.Genre)
                            'lvItem.ImageKey = IO.Path.GetExtension(lvItem.Text)
                            lvItem.ImageKey = plItem.ImageKey
                            lvItem.Tag = plItem.Tag

                            lv.Items.Add(lvItem)
                        Else
                            'there was an item in the playlist that was not found in the tree
                            Trace.WriteLine("Refresh Playlists: an item that was returned was not found in the tree. ItemID=" & id)
                            MsgBox("Refresh Playlists: an item that was returned was not found in the tree. ItemID=" & id)
                        End If
                    End If
                Next

                'partially clone the tabpage to add to the originalplaylists list
                'this is because the values in this list will change if the tabpage
                'is modified (we don' want that)
                originalTabPage = New TabPage
                originalTabPage.Name = tpage.Name
                originalTabPage.Text = tpage.Text
                originalTabPage.Tag = tpage.Tag
                originalPlaylists.Add(originalTabPage)
            End If
            strIDs = Nothing
        Next

        t.Abort()
        Trace.WriteLine("Refreshing playlists...complete")
    End Sub


    Private Sub deleteActivePlaylist()
        'deletes the active playlist tab and frees resources.
        'don't free the icons since the icons in the listview point to the ones from the tree
        Dim tpage As TabPage, selTabIndex As Integer, lv As ListView

        tpage = Me.tabPlaylists.SelectedTab
        selTabIndex = Me.tabPlaylists.SelectedIndex
        'get the listview as well
        lv = tpage.Controls(tpage.Name.Replace("tpPl", "lvPl"))

        'select the previous tab, then remove the page
        If selTabIndex > 0 Then
            Me.tabPlaylists.SelectedIndex = selTabIndex - 1
        End If

        Me.tabPlaylists.TabPages.Remove(tpage)
        If Me.tabPlaylists.TabPages.Count = 0 Then
            createNewPlaylist("New Playlist", False)
        End If
    End Sub
    Private Sub deleteAllPlaylists()
        Me.tabPlaylists.TabPages.Clear()
    End Sub
    Private Function createNewPlaylist(ByVal name As String, ByVal isFromPlayer As Boolean) As TabPage
        If name = "" Then
            Return Nothing
        End If

        If Not isFromPlayer Then
            name = name & "*"
        End If

        'don't allow :,<,>,/ in the name since that's what we use as a separator
        name = name.Replace(":"c, "_").Replace("<"c, "").Replace(">"c, "").Replace("/"c, "")

        Dim tpage As TabPage
        Dim lv As ListViewEx

        tpage = New TabPage(name)
        tpage.Name = "tpPl" & name
        lv = New ListViewEx
        With lv
            .Dock = DockStyle.Fill
            .GridLines = True
            .ShowItemToolTips = True
            .View = View.Details
            .AllowDrop = True
            .FullRowSelect = True
            .Columns.Add("File Name", 260, HorizontalAlignment.Left)
            .Columns.Add("Title", 120, HorizontalAlignment.Left)
            .Columns.Add("Artist", 100, HorizontalAlignment.Left)
            .Columns.Add("Album", 100, HorizontalAlignment.Left)
            .Columns.Add("Year", 50, HorizontalAlignment.Center)
            .Columns.Add("Track#", 30, HorizontalAlignment.Center)
            .Columns.Add("Genre", 80, HorizontalAlignment.Left)
            .Name = "lvPl" & name
            .SmallImageList = fullFileListing.ImageList
            AddHandler .DragEnter, AddressOf playlistListView_dragenter
            AddHandler .DragDrop, AddressOf playlistListView_dragdrop
            AddHandler .KeyDown, AddressOf playlistListView_KeyDown
            AddHandler .ColumnClick, AddressOf playlistListView_ColumnClick
            AddHandler .MouseClick, AddressOf playlistListView_MouseClick
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
        Dim lv As ListView

        tpage = Me.tabPlaylists.SelectedTab

        'search through playlists for duplicate names
        For Each otherPage As TabPage In Me.tabPlaylists.TabPages
            If newName = otherPage.Text.Replace("*", "") Then
                If MsgBox("Warning: Duplicate playlist name '" & newName & "'. This may cause problems", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkCancel, "Warining") = MsgBoxResult.Cancel Then
                    Exit Sub
                End If
            End If
        Next

        'get the listview as well
        lv = tpage.Controls(tpage.Name.Replace("tpPl", "lvPl"))

        tpage.Text = newName & "*"
        tpage.Name = "tpPl" & newName & "*"
        lv.Name = "lvPl" & newName & "*"



    End Sub
    Private Sub markPlaylistChanged(ByVal lv As ListView)
        'lv is the listview where the change occureed

        'mark this list for update
        Dim tpage As TabPage
        tpage = Me.tabPlaylists.TabPages(lv.Name.Replace("lvPl", "tpPl"))
        If Not tpage Is Nothing Then
            If Not tpage.Text.EndsWith("*") Then
                lv.Name = lv.Name & "*"
                tpage.Name = tpage.Name & "*"
                tpage.Text = tpage.Text & "*"
            End If
        End If
    End Sub
#End Region

#Region "FileManagement"

    'required delegates for updating the treeview from the uploading worker thread (used for drag n drop from explorer)
    Private Delegate Sub updateTreeviewDelegate(ByVal parentnode As TreeNode, ByVal childnode As TreeNode)
    Private Sub refreshFileTransfersDeviceFiles()

        Trace.WriteLine("Refreshing file list...")

        'let the user know were busy
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        If Not Splash.Visible Then
            t.Start()
            Splash.setTitle(Me.cmbDevices.Text)
        End If

        Splash.setText("Getting all media files")

        Me.tvFileManagementDeviceFolders.Nodes.Clear()
        Me.tvFileManagementDeviceFolders.BeginUpdate()

        'add the folders directly without the root 'storage media' node
        For Each node As TreeNode In fullFileListing.Nodes(0).Nodes
            If node.Text = "MUSIC" Or node.Text = "PICTURES" Or node.Text = "VIDEO" Then
                Me.tvFileManagementDeviceFolders.Nodes.Add(node.Clone)
            End If
        Next


        'show only folders to the treeview
        For Each node As TreeNode In Me.tvFileManagementDeviceFolders.Nodes
            Me.refreshFileTransfersDeviceFiles_helper(node)
        Next

        Me.tvFileManagementDeviceFolders.Sort()
        Me.tvFileManagementDeviceFolders.ExpandAll()
        Me.tvFileManagementDeviceFolders.SelectedNode = Me.tvFileManagementDeviceFolders.Nodes(0)

        Me.tvFileManagementDeviceFolders.EndUpdate()

        t.Abort()
        Trace.WriteLine("Refreshing file list...Complete")
    End Sub
    Private Sub refreshFileTransfersDeviceFiles_helper(ByRef root As TreeNode)
        Dim i As Integer = 0
        Dim node As TreeNode
        Dim item As StorageItem

        'show only folders. cant use for..each here because calling node.remove
        'modifies the collection count and that screws up the enumerator
        While i < root.Nodes.Count
            node = root.Nodes(i)
            i += 1
            item = CType(node.Tag, StorageItem)
            If (item.StorageType And MTPAxe.WMDM_FILE_ATTR_FOLDER) <> MTPAxe.WMDM_FILE_ATTR_FOLDER Then
                If node.Text <> "MUSIC" And node.Text <> "VIDEOS" And node.Text <> "PICTURES" Then
                    node.Remove()
                    i = 0 'if we remove a node, reset the counter so we will look at the updated nodes list
                End If
            Else
                Me.refreshFileTransfersDeviceFiles_helper(node)
            End If
        End While


    End Sub
    
    Private Sub tvFileManagementDeviceFolders_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvFileManagementDeviceFolders.AfterSelect
        Dim theNode As TreeNode = Nothing
        If fullFileListing IsNot Nothing AndAlso fullFileListing.Nodes.Count > 0 Then
            theNode = findTreeNodeByID(fullFileListing.Nodes(0), CType(e.Node.Tag, StorageItem).ID)
        End If


        If theNode IsNot Nothing Then
            Me.lvFileManagementDeviceFilesInFolder.BeginUpdate()

            'save the selected node into list view so we can easily get the currently displayed
            'folder
            Me.lvFileManagementDeviceFilesInFolder.Tag = Me.tvFileManagementDeviceFolders.SelectedNode
            Me.lblFileManagementSelectedFolder.Text = theNode.FullPath

            'get all files in this folder (except other subfolders)
            Me.lvFileManagementDeviceFilesInFolder.Items.Clear()

            Dim lvItem As ListViewItem
            Dim item As StorageItem
            For Each node As TreeNode In theNode.Nodes
                lvItem = New ListViewItem
                lvItem.Tag = node.Tag
                lvItem.Text = node.Text
                lvItem.ImageKey = node.ImageKey

                item = CType(lvItem.Tag, StorageItem)

                lvItem.SubItems.Add(Math.Ceiling((item.Size / 1024)).ToString("N0") & " KB")
                lvItem.SubItems.Add(item.Title)
                lvItem.SubItems.Add(item.AlbumArtist)
                lvItem.SubItems.Add(item.AlbumTitle)
                lvItem.SubItems.Add(item.Year)
                lvItem.SubItems.Add(item.TrackNum)
                lvItem.SubItems.Add(item.Genre)

                lvFileManagementDeviceFilesInFolder.Items.Add(lvItem)
            Next

            If lvFileManagementDeviceFilesInFolder.Items.Count = 0 Then
                lvFileManagementDeviceFilesInFolder.Items.Add("No files found")
            End If

            'sort the listview
            listviewItemSortAll(lvFileManagementDeviceFilesInFolder, SortOrder.Descending)

            Me.lvFileManagementDeviceFilesInFolder.EndUpdate()

        Else
            MsgBox("Error finding " & e.Node.Text)
            Trace.WriteLine("Error finding " & e.Node.Text)
        End If
    End Sub
    Private Sub tvFileManagementDeviceFolders_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvFileManagementDeviceFolders.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            'get the node under the right click
            Dim targetNode As TreeNode = tvFileManagementDeviceFolders.GetNodeAt(tvFileManagementDeviceFolders.PointToClient(New Point(e.X, e.Y)))
            targetNode = tvFileManagementDeviceFolders.SelectedNode
            If targetNode Is Nothing Then
                Exit Sub
            Else
                mnuTvFileManagementRightClick.Show(CType(sender, TreeView).PointToScreen(e.Location))
            End If
        End If
    End Sub
    Private Sub CollapseChildrenToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CollapseChildrenToolStripMenuItem1.Click
        Me.tvFileManagementDeviceFolders.BeginUpdate()
        Me.tvFileManagementDeviceFolders.SelectedNode.Collapse(False)
        Me.tvFileManagementDeviceFolders.EndUpdate()
    End Sub
    Private Sub ExpandChildrenToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExpandChildrenToolStripMenuItem1.Click
        Me.tvFileManagementDeviceFolders.BeginUpdate()
        Me.tvFileManagementDeviceFolders.SelectedNode.ExpandAll()
        Me.tvFileManagementDeviceFolders.EndUpdate()
    End Sub

    Private Sub updateTreeview(ByVal parentnode As TreeNode, ByVal childnode As TreeNode)
        'add the child node to the parent
        parentnode.Nodes.Add(childnode)

        'add it to the playlists file list too. first find the right node to add it to
        Dim plFileListNode As TreeNode = Nothing
        If Me.tvPlaylistsFilesOnDevice.Nodes.Count > 0 Then
            For Each node As TreeNode In Me.tvPlaylistsFilesOnDevice.Nodes
                plFileListNode = findTreeNodeByID(node, CType(parentnode.Tag, StorageItem).ID)
                If plFileListNode IsNot Nothing Then
                    'parent node found
                    Exit For
                End If
            Next
        End If
        If plFileListNode IsNot Nothing Then
            plFileListNode.Nodes.Add(childnode.Clone)
        End If

    End Sub

    Private Sub lvFileManagementDeviceFilesInFolder_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvFileManagementDeviceFilesInFolder.DragDrop
        'check to see if files and folders are being dragged from explorer
        Dim draggedFiles() As String
        draggedFiles = e.Data.GetData(DataFormats.FileDrop)
        If draggedFiles IsNot Nothing Then
            'worker threat to do the actual uploading. this prevents blocking of explorer
            'due to the dragdrop operation
            Dim tWork As New Threading.Thread(AddressOf lvFileManagementDeviceFilesInFolder_DragDrop_helper_starter)
            tWork.Start(draggedFiles)
        Else
            'if it's not from the treeview, check to see if it's from the ListViewEx itself
            Dim draggedListviewItem As Object
            draggedListviewItem = e.Data.GetData(GetType(ListViewEx.DragItemData))
            If Not draggedListviewItem Is Nothing Then
                'clean up the columheaders from sorting indicator and disable sorting
                lvFileManagementDeviceFilesInFolder.Sorting = SortOrder.None
                lvFileManagementDeviceFilesInFolder.ListViewItemSorter = Nothing
                If lvFileManagementDeviceFilesInFolder_lastColumnClicked <> -1 Then
                    lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text = lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
                End If
            End If
        End If
    End Sub
    Private Sub lvFileManagementDeviceFilesInFolder_DragDrop_helper_starter(ByVal draggedfiles As Object)
        'this is the worker thread for uploading files


        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        If Not Splash.Visible Then t.Start()
        Splash.setText("...")
        Splash.setTitle("Uploading...")

        'get the total number of files to transfer
        Dim totalNumFiles As Integer = 0
        For Each draggedfile As String In draggedfiles
            totalNumFiles += countAllFiles(draggedfile)
        Next
        Splash.initProgBar(totalNumFiles)

        For Each draggedfile As String In draggedfiles
            lvFileManagementDeviceFilesInFolder_DragDrop_helper(draggedfile, CType(Me.lvFileManagementDeviceFilesInFolder.Tag, TreeNode))
        Next

        Me.lvFileManagementDeviceFilesInFolder.Tag.expandall()

        Splash.resetProgBar()
        t.Abort()
        Me.Activate()
    End Sub
    Private Sub lvFileManagementDeviceFilesInFolder_DragDrop_helper(ByVal path As String, ByVal parentNode As TreeNode)
        'adds files specified by path to the folder  specified by parentNode

        'get the attributes of the parent node (will need them to add new storage items to the tree)
        Dim item As StorageItem
        item = CType(parentNode.Tag, StorageItem)

        If IO.Directory.Exists(path) Then
            'draggedfile was a directory. now add it and all it's children

            'first create the folder on the device
            Dim folderName = path.Split("\")(path.Split("\").Length - 1)

            Splash.setText("Creating directory '" & folderName & "'")
            Dim createdItemID As String = axe.uploadFile(folderName, item.ID, 1)
            If createdItemID = "-1" Then
                Trace.WriteLine("fileManagement: error creating folder '" & folderName & "'")
                MsgBox("fileManagement: error creating folder '" & folderName & "'. Maybe device is full?", MsgBoxStyle.Critical, "error")
                Exit Sub
            End If

            'since this is a folder, we add it to the treeview as well as the listview
            'add it to the listview
            If lvFileManagementDeviceFilesInFolder.Items(0) IsNot Nothing Then
                If lvFileManagementDeviceFilesInFolder.Items(0).Text = "No files found" Then
                    lvFileManagementDeviceFilesInFolder.Items.Clear()
                End If
            End If
            If Me.lvFileManagementDeviceFilesInFolder.Tag Is parentNode Then
                lvFileManagementDeviceFilesInFolder.Items.Add(folderName, "*").SubItems.Add("0 KB")
            End If

            'add it to the treeview
            Dim newNode As New TreeNode
            Dim newItem As New StorageItem
            newItem.DirectoryDepth = item.DirectoryDepth + 1
            newItem.FileName = folderName
            newItem.ParentFileName = item.FileName
            newItem.FileName = folderName
            newItem.StorageType = 295176
            newItem.ID = createdItemID

            newNode.Tag = newItem
            newNode.Text = folderName
            newNode.ImageKey = "*"

            'invoke the delegate to update the treeview from the main thread
            Me.Invoke(New updateTreeviewDelegate(AddressOf updateTreeview), New TreeNode() {parentNode, newNode})

            'now that the folder has been successfully created, add it to the fullFileListing tree
            parentNode = findTreeNodeByID(fullFileListing.Nodes(0), CType(parentNode.Tag, StorageItem).ID)
            parentNode.Nodes.Add(newNode.Clone)

            'now that the directory has been created, recursively add all it's children
            For Each filesystementry As String In IO.Directory.GetFileSystemEntries(path)
                lvFileManagementDeviceFilesInFolder_DragDrop_helper(filesystementry, newNode)
            Next
        Else
            'if were here, file was really just a file
            Dim ext As String = IO.Path.GetExtension(path)
            Dim fname As String = IO.Path.GetFileName(path)
            Dim itemSize As Long = FileLen(path)

            Splash.setText("Uploading '" & fname & "'")

            'get the file metadata
            Dim newItem As New StorageItem
            newItem.DirectoryDepth = item.DirectoryDepth + 1
            newItem.StorageType = 295200
            newItem.FileName = fname
            newItem.ParentFileName = item.FileName
            newItem.Size = itemSize
            'read metadata tags with taglib-sharp. if there is an exception
            'the default values (i.e. "" ) will be used
            Try
                'note the file extension must be correct or taglib won't properly recognize the file
                Dim tagReader As TagLib.File = TagLib.File.Create(path)
                newItem.Title = tagReader.Tag.Title
                newItem.AlbumTitle = tagReader.Tag.Album
                newItem.AlbumArtist = tagReader.Tag.FirstArtist
                If newItem.AlbumArtist = "" Then
                    newItem.AlbumArtist = tagReader.Tag.FirstAlbumArtist
                    If newItem.AlbumArtist = "" Then
                        newItem.AlbumArtist = tagReader.Tag.FirstPerformer
                        If newItem.AlbumArtist = "" Then
                            newItem.AlbumArtist = tagReader.Tag.FirstComposer
                        End If
                    End If
                End If
                newItem.Genre = tagReader.Tag.FirstGenre
                newItem.TrackNum = tagReader.Tag.Track
                newItem.Year = tagReader.Tag.Year
                tagReader = Nothing
            Catch ex As Exception
                Trace.WriteLine("taglib-sharp error (wrong file extension maybe?): " & ex.Message & " for file=" & path)
            End Try

            Dim createdItemID As String = axe.uploadFile(path, item.ID, 0, newItem)
            If createdItemID = "-1" Then
                Trace.WriteLine("fileManagement: error uploading file '" & path & "'")
                MsgBox("fileManagement: error uploading file '" & path & "'. Maybe device is full?", MsgBoxStyle.Critical, "error")
                Exit Sub
            End If
            newItem.ID = createdItemID

            If lvFileManagementDeviceFilesInFolder.Items.Count > 0 AndAlso lvFileManagementDeviceFilesInFolder.Items(0) IsNot Nothing Then
                If lvFileManagementDeviceFilesInFolder.Items(0).Text = "No files found" Then
                    lvFileManagementDeviceFilesInFolder.Items.Clear()
                End If
            End If

            'get the parentNode attribs, so we can add the new node in the correct spot
            'we also need to define the type attribute so as to have a complete picture of the node
            'type seems to be 295176 for normal folders and 295200 for normal files
            Dim newNode As New TreeNode

            newNode.Text = fname
            newNode.Tag = newItem
            newNode.ImageKey = ext

            'add this item to the fulltree so it will appear in the listview if the parent node
            'is selected again. could also enumerate the storage again, but this would be too slow
            parentNode = findTreeNodeByID(fullFileListing.Nodes(0), CType(parentNode.Tag, StorageItem).ID)
            If parentNode Is Nothing Then
                Trace.WriteLine("fileManagement: couldnt find parent folder '" & item.FileName & "'")
                MsgBox("fileManagement: File upload succeeded but coulnd't add it to the tree node '" & item.FileName & "'")
                Exit Sub
            End If

            'invoke the delegate to update the treeview on the playlists tab from the main thread. note this alos
            'adds newnode to parentNode, which in this case, a delegate is not required (since parentNode does not reside on a control)
            'but it's handy to reuse this function
            Me.Invoke(New updateTreeviewDelegate(AddressOf updateTreeview), New TreeNode() {parentNode, newNode})

            'add it to the listview only if the file is actually contained in the selected folder 
            'without this, all files are added to the listview, even ones in subfolders
            If CType(Me.lvFileManagementDeviceFilesInFolder.Tag, TreeNode).Tag.Equals(parentNode.Tag) Then
                Dim lvItem As New ListViewItem
                lvItem.Tag = newNode.Tag
                lvItem.Text = newNode.Text
                lvItem.ImageKey = newNode.ImageKey

                lvItem.SubItems.Add(Math.Ceiling((newItem.Size / 1024)).ToString("N0") & " KB")
                lvItem.SubItems.Add(newItem.Title)
                lvItem.SubItems.Add(newItem.AlbumArtist)
                lvItem.SubItems.Add(newItem.AlbumTitle)
                lvItem.SubItems.Add(newItem.Year)
                lvItem.SubItems.Add(newItem.TrackNum)
                lvItem.SubItems.Add(newItem.Genre)
                Me.lvFileManagementDeviceFilesInFolder.Items.Add(lvItem)
            End If

            Splash.incProgBar()
            End If
    End Sub


    Private Sub lvFileManagementDeviceFilesInFolder_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvFileManagementDeviceFilesInFolder.DragEnter
        Me.Activate()
        e.Effect = e.AllowedEffect
    End Sub
    Private Sub lvFileManagementDeviceFilesInFolder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvFileManagementDeviceFilesInFolder.KeyDown
        If e.KeyValue = Keys.Delete Then
            MsgBox("Deleting items is not yet supported. use explorer to delete files")
        End If
    End Sub
    Private Sub lvFileManagementDeviceFilesInFolder_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvFileManagementDeviceFilesInFolder.MouseClick
        'show the right click menu on mouse click
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim lv As ListViewEx = CType(sender, ListViewEx)

            'get the node under the right click
            Dim selNode As ListViewItem = lv.SelectedItems(0) 'tvTree.GetNodeAt(tvTree.PointToClient(New Point(e.X, e.Y)))
            If selNode IsNot Nothing Then
                mnuLvFileManagementRightClick.Show(lv.PointToScreen(e.Location))
            End If
        End If
    End Sub
    Private Sub SortAscendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortAscendingToolStripMenuItem.Click
        'disable the sorting on the listview so we can manually sort stuff without the listview automatically resorting
        If Not lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1 Then
            'clean up the title of the previously clicked column
            Me.lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text = Me.lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1
        End If

        listviewItemSortSelected(Me.lvFileManagementDeviceFilesInFolder, SortOrder.Descending)
    End Sub
    Private Sub SortDescendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SortDescendingToolStripMenuItem.Click
        'disable the sorting on the listview so we can manually sort stuff without the listview automatically resorting
        If Not lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1 Then
            'clean up the title of the previously clicked column
            Me.lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text = Me.lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1
        End If

        listviewItemSortSelected(Me.lvFileManagementDeviceFilesInFolder, SortOrder.Ascending)
    End Sub

    Private Sub lvFileManagementDeviceFilesInFolder_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvFileManagementDeviceFilesInFolder.ColumnClick
        Dim lv As ListViewEx = CType(sender, ListView)
        If e.Column <> lvFileManagementDeviceFilesInFolder_lastColumnClicked Then
            lv.Sorting = SortOrder.Ascending
            lv.Columns(e.Column).Text = lv.Columns(e.Column).Text & " ^"
            If Not lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1 Then
                'clean up the title of the previously clicked column
                lv.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text = lv.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            End If
            lvFileManagementDeviceFilesInFolder_lastColumnClicked = e.Column
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

    Private Sub btnFileManagementRefresh_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnFileManagementRefresh.LinkClicked
        Application.DoEvents()
        refreshFullDirectoryTree()
        refreshFileTransfersDeviceFiles()
    End Sub
#End Region


#Region "misc. helpers"
    Private Function countAllFiles(ByVal path As String) As Integer
        'counts how many files there are in a given path. subdirectories are recursed
        Dim count As Integer = 0

        If IO.Directory.Exists(path) Then
            'path is a folder

            For Each filesystementry As String In IO.Directory.GetFileSystemEntries(path)
                count += countAllFiles(filesystementry)
            Next
        Else
            'its a file
            count += 1
        End If

        Return count
    End Function
    Private Function xfindTreeNode(ByRef root As TreeNode, ByRef theNode As TreeNode) As TreeNode
        'search for the specified node in the child nodes of root and 
        'return it. if not found, nothing is returned
        'root is the node to search, theNode is the node we're looking for
        Dim ret As TreeNode = Nothing

        If root.Tag.Equals(theNode.Tag) Then
            Return root
        End If

        For Each node As TreeNode In root.Nodes
            If node.Tag.Equals(theNode.Tag) Then
                ret = node 'check if the node is the folder we're looking for
            ElseIf node.Nodes.Count > 0 Then
                'else check the child nodes
                ret = xfindTreeNode(node, theNode)
            End If

            'if we found it, return it
            If ret IsNot Nothing Then
                Return ret
            End If
        Next

        Return ret
    End Function
    Private Function findTreeNodeByName(ByRef root As TreeNode, ByVal name As String) As TreeNode
        'searches for the first matching treenode with the given name
        Dim ret As TreeNode = Nothing

        If root.Text = name Then
            Return root
        End If

        For Each node As TreeNode In root.Nodes
            If node.Text = name Then
                ret = node
            ElseIf node.Nodes.Count > 0 Then
                'else check the child nodes
                ret = findTreeNodeByName(node, name)
            End If

            'if we found it, return it
            If ret IsNot Nothing Then
                Return ret
            End If
        Next

        Return ret
    End Function
    Private Function findTreeNodeByID(ByRef root As TreeNode, ByVal ID As String) As TreeNode
        'searches for the first matching treenode with the given PersistentUniqueID
        Dim ret As TreeNode = Nothing
        Dim item As StorageItem

        item = CType(root.Tag, StorageItem)
        If item.ID = ID Then
            Return root
        End If

        For Each node As TreeNode In root.Nodes
            item = CType(node.Tag, StorageItem)
            If item.ID = ID Then
                ret = node
            ElseIf node.Nodes.Count > 0 Then
                'else check the child nodes
                ret = findTreeNodeByID(node, ID)
            End If

            'if we found it, return it
            If ret IsNot Nothing Then
                Return ret
            End If
        Next

        Return ret
    End Function
    Private Sub freeImageListHandles(ByRef imgList As ImageList)
        'frees all icon handles of the specified image list. use this whenever refreshing
        'a treeview or listview to avoid GDI+ or System.Drawing out of memory errors

        'free the image list of the previous tree, if it exists.
        Dim result As Boolean
        Try
            If Not imgList Is Nothing Then
                For Each img As Image In imgList.Images
                    If TypeOf img Is Bitmap Then
                        result = DestroyIcon(CType(img, Bitmap).GetHicon)
                    End If
                Next
                imgList.Images.Clear()
            End If
        Catch ex As Exception
            Trace.WriteLine("freeImageListHandles(): error freeing handles" & ex.Message & ", " & ex.Source)
        End Try
    End Sub
    Private Sub listviewItemSortSelected(ByRef lv As ListViewDnD.ListViewEx, ByVal order As SortOrder)
        'sorts the selected items of the specified listviewex

        If lv.SelectedItems.Count = 0 Then Exit Sub

        lv.BeginUpdate()

        lv.ListViewItemSorter = Nothing
        lv.Sorting = SortOrder.None

        'save the index of where the items go
        Dim listPos As Integer = lv.SelectedItems(0).Index

        'add the items to an array
        Dim tmpArr(lv.SelectedItems.Count - 1) As ListViewItem
        lv.SelectedItems.CopyTo(tmpArr, 0)

        'create a new listviewitemcomprar
        Dim comparer As New PlaylistListViewItemComparer(0, order)
        Array.Sort(tmpArr, comparer)

        'if the sort was successful (no exception was thrown) delete the old item
        'and add the new sorted items   

        For Each lvItem As ListViewItem In lv.SelectedItems
            lv.Items.Remove(lvItem)
        Next
        For Each lvItem As ListViewItem In tmpArr
            lv.Items.Insert(listPos, lvItem)
        Next

        lv.EndUpdate()
    End Sub
    Private Sub listviewItemSortAll(ByRef lv As ListViewDnD.ListViewEx, ByVal order As SortOrder)
        'sorts all items of the specified listviewex

        If lv.Items.Count = 0 Then Exit Sub

        lv.BeginUpdate()

        lv.ListViewItemSorter = Nothing
        lv.Sorting = SortOrder.None

        'add the items to an array
        Dim tmpArr(lv.Items.Count - 1) As ListViewItem
        lv.Items.CopyTo(tmpArr, 0)

        'create a new listviewitemcomprar
        Dim comparer As New PlaylistListViewItemComparer(0, order)
        Array.Sort(tmpArr, comparer)

        'if the sort was successful (no exception was thrown) delete the old item
        'and add the new sorted items   
        'For Each lvItem As ListViewItem In lv.Items
        '    lv.Items.Remove(lvItem)
        'Next
        lv.Items.Clear()
        For Each lvItem As ListViewItem In tmpArr
            lv.Items.Insert(0, lvItem)
        Next

        lv.EndUpdate()
    End Sub
    Private Sub refreshFullDirectoryTree()
        Trace.WriteLine("Refreshing full directory tree...")

        'let the user know were busy
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        If Not Splash.Visible Then
            t.Start()
            Splash.setTitle(Me.cmbDevices.Text)
        End If

        Splash.setText("Refreshing device file list")

        'refreshes the global variable that stores the full listing of all the files on the
        'device
        If Not fullFileListing Is Nothing Then
            freeImageListHandles(fullFileListing.ImageList)
        End If

        'get the stuff on the device (keep it in a global variable
        'so we don't have to read the device whenever a folder is opened
        fullFileListing = axe.getFullTreeView
        If fullFileListing.Nodes.Count = 0 Then
            fullFileListing.Nodes.Add("Empty Tree").Tag = New StorageItem
        End If

        'rebuild icons
        Me.tvPlaylistsFilesOnDevice.ImageList = fullFileListing.ImageList
        Me.tvFileManagementDeviceFolders.ImageList = fullFileListing.ImageList
        Me.lvFileManagementDeviceFilesInFolder.SmallImageList = fullFileListing.ImageList
        Dim lv As ListViewEx
        For Each tpage As TabPage In Me.tabPlaylists.TabPages
            For Each c As Control In tpage.Controls
                lv = CType(c, ListViewEx)
                If lv IsNot Nothing Then
                    lv.SmallImageList = fullFileListing.ImageList
                End If
            Next
        Next


        Trace.WriteLine("Refreshing full directory tree...done")
        t.Abort()
    End Sub


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

            If x Is Nothing And y Is Nothing Then
                Return 0
            End If
            If x Is Nothing Then
                Return -1
            End If
            If y Is Nothing Then
                Return 1
            End If

            Dim ret As Integer = -1
            Dim item_x As ListViewItem = CType(x, ListViewItem)
            Dim item_y As ListViewItem = CType(y, ListViewItem)
            Dim subitem_x As ListViewItem.ListViewSubItem
            Dim subitem_y As ListViewItem.ListViewSubItem

            'don't allow any other column other than 0 for now
            'since the subitems are not yet implemented
            If whichColumn <> 0 Then
                Return 0
            End If

            'check to see if we're comparing two folders, a file and a folder, or two files
            If (item_x.ImageKey = "*" And item_y.ImageKey = "*") Or (item_x.ImageKey <> "*" And item_y.ImageKey <> "*") Then
                'two folders or two files are being compared. 
                'look at the text to determine which one is greater
                'subitem 0 is the main item
                subitem_x = item_x.SubItems(whichColumn)
                subitem_y = item_y.SubItems(whichColumn)
                ret = String.Compare(subitem_x.Text, subitem_y.Text, True)
            ElseIf item_x.ImageKey = "*" And item_y.ImageKey <> "*" Then
                'item_x is a folder. folders have priority over files
                Return 1
            Else
                'item_x is a file and item_y is a folder => item_y is bigger than item_x
                Return -1
            End If

            If whichSortOrder = SortOrder.Descending Then
                ' Invert the value returned by String.Compare.
                ret *= -1
            End If


            Return ret
        End Function
    End Class

#End Region

 
End Class
