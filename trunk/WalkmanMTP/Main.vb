''Copyright 2008 Dr. Zoidberg
' 
'Licensed under the Apache License, Version 2.0 (the "License");
'you may not use this file except in compliance with the License.
'You may obtain a copy of the License at
'
'	http://www.apache.org/licenses/LICENSE-2.0 
'
'Unless required by applicable law or agreed to in writing, software 
'distributed under the License is distributed on an "AS IS" 
'BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'See the License for the specific language governing permissions 
'and limitations under the License. 

Imports WalkmanMTP.ListViewDnD
Public Class Main
    Public axe As MTPAxe

    'application settings
    Private theSettings As New Settings

    'used for keeping track of the complete file listing on the device
    Private fullFileListing As TreeView

    'keeps track of what the original playlists were as of the last time
    ' the playlists list was refreshed. this is to be able to
    'distinguish which lists have to be deleted and added
    Private originalPlaylists As Collection
    'keeps track of what albums (and their contents) were originally on the player
    'before any changes are done to albums, originalAlbums and modified albums will be identical
    Private originalAlbums As New TreeView
    Private modifiedAlbums As New TreeView

    'keeps track of what column was clicked last in order to enable 
    'sorting ascending or descending if the column header is clicked more than once
    Dim playlistListView_lastColumnClicked As Short = -1
    Dim lvFileManagementDeviceFilesInFolder_lastColumnClicked As Short = -1
    Dim lvAlbumItems_lastColumnClicked As Short = -1

    'keeps track if a device is connected and selected
    'TODO: enable device removal detection and update this value
    Private DeviceConnected As Boolean = False

    'keeps track of the free space on the device. this is updated un updateDeviceFreeSpace
    Private freeSpace As Double = -1
    Private capacity As Double = -1

#Region "Application Menu"
    Private Sub QuitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileQuitToolStripMenuItem.Click
        Application.Exit()
    End Sub
    Private Sub ShowDebugWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOptionsShowDebugWindowToolStripMenuItem.Click
        If Me.mnuOptionsShowDebugWindowToolStripMenuItem.Checked Then
            TraceOutput.Visible = True
            TraceOutput.WindowState = FormWindowState.Normal
        Else
            TraceOutput.Visible = False
        End If
    End Sub
    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpAboutToolStripMenuItem.Click
        Dim mtpAxeVer As String = ""
        If axe IsNot Nothing Then
            mtpAxeVer = axe.getMTPAxeVersion
        End If
        MsgBox("WalkmanMTP by Dr. Zoidberg v" & Application.ProductVersion & vbCrLf & mtpAxeVer & vbCrLf & vbCrLf & _
               "Taglib# by Brian Nickel v" & System.Reflection.Assembly.GetAssembly(GetType(TagLib.File)).GetName.Version.ToString & vbCrLf & _
               "Icons by Mark James (www.famfamfam.com/lab/icons/silk) and poggos", MsgBoxStyle.Information, "About")

    End Sub
    Private Sub InformationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpInformationToolStripMenuItem.Click
        Dim readme As String = IO.Path.Combine(Application.StartupPath, "readme.txt")
        Try
            Process.Start(readme)
        Catch ex As Exception
            MsgBox("Couldn't open " & readme)
        End Try
    End Sub
    Private Sub SyncDeviceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSyncDeviceToolStripMenuItem.Click
        btnSync_Click(Nothing, Nothing)
    End Sub
    Private Sub ShowDeviceIconToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOptionsShowDeviceIconToolStripMenuItem.Click
        mnuOptionsShowDeviceIconToolStripMenuItem.Checked = Not mnuOptionsShowDeviceIconToolStripMenuItem.Checked
        pboxDevIcon.Visible = mnuOptionsShowDeviceIconToolStripMenuItem.Checked
        theSettings.ShowDeviceIcon = mnuOptionsShowDeviceIconToolStripMenuItem.Checked
    End Sub
#End Region

    Private Sub Main_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'delete temporary albumart icons
        removeAllAlbumsFromAlbumsList()
        cleanUpAxeTmpFiles()

        If Not axe Is Nothing Then
            If Not axe.stopAxe Then
                MsgBox("Could not close MTPAxe")
                Trace.WriteLine("Main form Closing: could not close MTPAxe")
            End If
            axe = Nothing
        End If

        'save a form attributes
        theSettings.MainFormWindowState = Me.WindowState
        theSettings.MainFormWindowHeight = Me.Height
        theSettings.MainFormWindowWidth = Me.Width
        theSettings.AlbumPanelSplitterDistance = Me.SplitContainer3.SplitterDistance
        theSettings.PlaylistsPanelSplitterDistance = Me.SplitContainer1.SplitterDistance
        theSettings.FileManagementPanelSplitterDistance = Me.SplitContainer2.SplitterDistance
        theSettings.save()
    End Sub
 
    Private Sub Main_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.btnAddPlaylist.Image = My.Resources.Playlist_add
        Me.btnDelPlaylist.Image = My.Resources.Playlist_delete
        Me.btnRenamePlaylist.Image = My.Resources.Playlist_rename
        Me.btnSync.Image = My.Resources.SyncDevice
        Me.btnRefreshDevices.Image = My.Resources.DevicesList_refresh
        Me.pboxDevIcon.Image = My.Resources.NoDeviceIcon
        Me.lvFileManagementDeleteToolStripMenuItem.Image = My.Resources.Delete
        Me.mnulvFileManagementSortAscendingToolStripMenuItem.Image = My.Resources.SortAscending
        Me.mnulvFileManagementSortDescendingToolStripMenuItem.Image = My.Resources.SortDescending
        Me.lvPlaylistContentsDeleteToolStripMenuItem.Image = My.Resources.Delete
        Me.lvPlaylistContentsSelctionSortAscendingToolStripMenuItem.Image = My.Resources.SortAscending
        Me.lvPlaylistContentsSelectionSortDescendingToolStripMenuItem.Image = My.Resources.SortDescending
        Me.mnutvPlaylistFilesCollapseChildrenToolStripMenuItem.Image = My.Resources.CollapseChild
        Me.mnutvPlaylistFilesExpandChildrenToolStripMenuItem.Image = My.Resources.ExpandChild
        Me.mnuFileQuitToolStripMenuItem.Image = My.Resources.Quit
        Me.mnuFileSyncDeviceToolStripMenuItem.Image = My.Resources.SyncDevice
        Me.mnuHelpAboutToolStripMenuItem.Image = My.Resources.About
        Me.mnuHelpInformationToolStripMenuItem.Image = My.Resources.Information
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.Image = My.Resources.Log
        Me.mnuOptionsShowDeviceIconToolStripMenuItem.Image = My.Resources.DeviceIcon
        Me.mnuTVFileManagementCollapseChildren.Image = My.Resources.CollapseChild
        Me.mnuTVFileManagementExpandChildren.Image = My.Resources.ExpandChild
        Me.btnDeleteAlbum.Image = My.Resources.Album_delete
        Me.pbAlbumArt.Image = My.Resources.NoDeviceIcon
        Me.pbAlbumArt.AllowDrop = True

        'set the form state from the settings
        Try
            Me.WindowState = theSettings.MainFormWindowState
            Me.Width = theSettings.MainFormWindowWidth
            Me.Height = theSettings.MainFormWindowHeight
            Me.SplitContainer3.SplitterDistance = theSettings.AlbumPanelSplitterDistance
            Me.SplitContainer1.SplitterDistance = theSettings.PlaylistsPanelSplitterDistance
            Me.SplitContainer2.SplitterDistance = theSettings.FileManagementPanelSplitterDistance

            Me.mnuOptionsShowDeviceIconToolStripMenuItem.Checked = theSettings.ShowDeviceIcon
            Me.chkDeleteSongsOnAlbumDelete.Checked = theSettings.DeleteAlbumSongsOnAlbumDelete
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Main_Shown(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles Me.Shown

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
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        DeviceDetails.Show()
    End Sub
    Private Sub btnRefreshDevices_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshDevices.Click
        initAndRefreshApp()
    End Sub
    Private Sub btnSync_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSync.Click
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

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
        syncAlbums()
        Trace.WriteLine("Start sync operation...Completed")

        refreshFullDirectoryTree()
        refreshPlaylistsList()
        refreshFileTransfersDeviceFiles()
        refreshPlaylistDeviceFiles()
        refreshAlbumsList()

        updateDeviceFreeSpace()

        t.Abort()
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

        devarr = ret.Split(";"c)

        Me.cmbDevices.Items.AddRange(devarr)

        'select the last selected device automatically
        If Me.cmbDevices.Items.Contains(theSettings.SelectedDevice) Then
            Me.cmbDevices.SelectedItem = theSettings.SelectedDevice
        End If

        t.Abort()

    End Sub
    Private Sub initSelectedDevice(ByVal devName As String)
        DeviceConnected = False

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

            'make sure the selected device is a playback device
            If Not Me.checkDeviceSupported Then
                Trace.WriteLine("Device '" & Me.cmbDevices.SelectedItem & "' is unsupported")
                If MsgBox("Selected device doesn't seem to be a media player. Continue anyways?", MsgBoxStyle.YesNo Or MsgBoxStyle.SystemModal Or MsgBoxStyle.Exclamation, "Device not supported") = Windows.Forms.DialogResult.No Then
                    Me.cmbDevices.SelectedIndex = -1
                    Exit Sub
                Else
                    Trace.WriteLine("User decided to continue with unsupported device")
                End If
            End If

            'first refresh the file listing. the list needs to be up to date
            'so the other refresh functions can use it
            refreshFullDirectoryTree()
            'refresh the various lists and trees
            refreshPlaylistDeviceFiles()
            refreshFileTransfersDeviceFiles()
            refreshPlaylistsList()
            refreshAlbumsList()

            Splash.setText("Reading device info")
            Me.lblManufacturer.Text = axe.getDeviceManufacturer()

            'get capacity
            Me.updateDeviceFreeSpace()

            'get the icon
            If theSettings.ShowDeviceIcon Then
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
            End If
            DeviceConnected = True
        Catch ex As Exception
            Trace.WriteLine("initSelectedDevice: Error initializing '" & devName & "'" & ":" & ex.Message & " in " & ex.Source)
            MsgBox("Error initializing '" & devName & "'" & vbCrLf & ex.Message & " in " & ex.Source, MsgBoxStyle.Critical Or MsgBoxStyle.SystemModal)
        End Try

        Cursor.Current = Cursors.Default
        t.Abort()

        If DeviceConnected = False Then
            If MsgBox("There was an error initializing " & devName & ". Continue at your own risk", MsgBoxStyle.Critical Or MsgBoxStyle.YesNo Or MsgBoxStyle.SystemModal) = MsgBoxResult.Yes Then
                DeviceConnected = True
            Else
                Application.Exit()
            End If
        End If

    End Sub
    Private Sub cmbDevices_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDevices.SelectedIndexChanged
        theSettings.SelectedDevice = Me.cmbDevices.SelectedItem
        If Me.cmbDevices.SelectedIndex <> -1 Then
            Me.initSelectedDevice(Me.cmbDevices.SelectedItem.ToString)
        End If
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
                If axe.deleteFile(tpage.Tag) = "-1" Then
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
                        'don't exit on error, keep processing subsequent lists
                    End If

                End If 'lv.items.count
            End If 'tpage.text.endswith
        Next

        Trace.WriteLine("Syncing playlists...Done")
    End Sub

    Private Sub btnPlaylistsFilesOnDeviceRefresh_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnPlaylistsFilesOnDeviceRefresh.LinkClicked
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

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
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        If MsgBox("Are you sure?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Really?") = MsgBoxResult.No Then
            Exit Sub
        End If

        deleteAllPlaylists()
    End Sub
    Private Sub btnDelPlaylist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelPlaylist.Click
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        deleteActivePlaylist()
    End Sub
    Private Sub btnRenamePlaylist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRenamePlaylist.Click
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Dim newname As String
        newname = InputBox("Enter new name: ", "Rename Playlist", Me.tabPlaylists.SelectedTab.Text.Replace("*", ""))
        renameActivePlaylist(newname)
    End Sub
    Private Sub btnAddPlaylist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPlaylist.Click
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

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

    Private Sub CollapseChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnutvPlaylistFilesCollapseChildrenToolStripMenuItem.Click
        Me.tvPlaylistsFilesOnDevice.BeginUpdate()
        For Each node As TreeNode In Me.tvPlaylistsFilesOnDevice.SelectedNodes
            node.Collapse(False)
        Next
        Me.tvPlaylistsFilesOnDevice.EndUpdate()
    End Sub
    Private Sub ExpandChildrenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnutvPlaylistFilesExpandChildrenToolStripMenuItem.Click
        Me.tvPlaylistsFilesOnDevice.BeginUpdate()
        For Each node As TreeNode In Me.tvPlaylistsFilesOnDevice.SelectedNodes
            node.ExpandAll()
        Next
        Me.tvPlaylistsFilesOnDevice.EndUpdate()
    End Sub


    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvPlaylistContentsDeleteToolStripMenuItem.Click
        'get the current playlist
        Dim tpage As TabPage = Me.tabPlaylists.SelectedTab
        Dim lv As ListViewEx = tpage.Controls(tpage.Name.Replace("tpPl", "lvPl"))
        If lv Is Nothing Then Exit Sub

        For Each lvItem As ListViewItem In lv.SelectedItems
            lv.Items.Remove(lvItem)
        Next

        markPlaylistChanged(lv)
    End Sub
    Private Sub SelctionSortAscendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvPlaylistContentsSelctionSortAscendingToolStripMenuItem.Click
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
    Private Sub SelectionSortDescendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvPlaylistContentsSelectionSortDescendingToolStripMenuItem.Click
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

        Me.tvPlaylistsFilesOnDevice.EndUpdate()
        Me.tvPlaylistsFilesOnDevice.BeginUpdate()

        Me.tvPlaylistsFilesOnDevice.Nodes.Clear()

        musicFldr = findTreeNodeByName(fullFileListing.Nodes(0), "MUSIC")
        If musicFldr Is Nothing Then Exit Sub

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

        Dim plFldr As TreeNode
        plFldr = findTreeNodeByName(fullFileListing.Nodes(0), "Playlists")
        If plFldr Is Nothing Then
            Trace.WriteLine("Error refreshing playlists - plyalists folder not found")
            Exit Sub
        End If
        If plFldr.Nodes.Count = 0 Then
            Trace.WriteLine("no playlists found")
            createNewPlaylist("New Playlist", False)
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
        Dim nameNewPlaylist As String = "New Playlist"
        Dim i As Integer = 0
        For Each node As TreeNode In plFldr.Nodes
            'create a new playlist tab and listview based on the playlist name and the playlist's
            'PersistentUniqueID
            tpage = createNewPlaylist(node.Text, True)
            lv = tpage.Controls("lvPl" & node.Text)
            'since this playlist is one that exists on the player, it has a PersistentUniqueID
            'use this ID instead of the automatically generated ID to identify this playlist tab.
            'If the playlist is modified later, we can use this ID to delete it. Don't pass the ID
            'into the createNewPlaylist function since if the user creates a playlist, it won't have
            'a PersistentUniqueID yet, in which case the default identifier is used.
            tpage.Tag = CType(node.Tag, StorageItem).ID
            'partially clone the tabpage to add to the originalplaylists list
            'this is because the values in this list will change if the tabpage
            'is modified (we don' want that)
            originalTabPage = New TabPage
            originalTabPage.Name = tpage.Name
            originalTabPage.Text = tpage.Text
            originalTabPage.Tag = tpage.Tag
            originalPlaylists.Add(originalTabPage)


            While node.Text = nameNewPlaylist
                nameNewPlaylist = "New Playlist " & i + 1
                i += 1
            End While


            strIDs = axe.getPlaylistContentsIDs(CType(node.Tag, StorageItem).ID)
            If strIDs IsNot Nothing Then

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
            End If 'if srtIds is nothing
            strIDs = Nothing
        Next

     
        createNewPlaylist(nameNewPlaylist, False)


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
        'name = name.Replace(":"c, "_").Replace("<"c, "").Replace(">"c, "").Replace("/"c, "")

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
        tpage.Tag = Now.Ticks.ToString
        tpage.Controls.Add(lv)
        Me.tabPlaylists.TabPages.Insert(0, tpage)
        Me.tabPlaylists.SelectedTab = tpage

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

#Region "Albums"
    Private Sub syncAlbums()
        'keeps track of whether an error occured for a given album
        Dim albumSyncError As Boolean = False
        'keeps track of whether an error occured for ANY album (so we can show the
        'warning dialog at the end)
        Dim albumSyncErrorOccurred As Boolean = False

        'save any changes to the current album (only if one is selected, and has a valid title)
        If Me.lvAlbumItems.Tag IsNot Nothing And Me.txtAlbumTitle.Text <> "" Then saveAlbumMetadata(Me.lvAlbumItems.Tag)

        Trace.WriteLine("Syncing albums...")


        'find the MUSIC folder
        If fullFileListing Is Nothing OrElse fullFileListing.Nodes.Count < 1 Then
            Trace.WriteLine("Could not sync albums. tree was not initialized")
            Exit Sub
        End If
        Dim musicFolder As TreeNode = findTreeNodeByName(fullFileListing.Nodes(0), "MUSIC")
        If musicFolder Is Nothing Then
            Trace.WriteLine("could not sync albums. MUSIC folde not found")
            Exit Sub
        End If

        Trace.WriteLine("Deleting old albums")
        'find which albums on the player have been deleted by comparing the origninal list
        'with the current list
        For Each originalAlbum As TreeNode In originalAlbums.Nodes
            Dim originalMetadata As StorageItem = originalAlbum.Tag
            Dim originalAlbumWasDeleted As Boolean = True

            For Each modifiedAlbum As TreeNode In modifiedAlbums.Nodes
                Dim modifiedMetadata As StorageItem = modifiedAlbum.Tag

                'if the original node exists in the the modified list, it
                'wasn't deleted. else it was
                If originalMetadata.ID = modifiedMetadata.ID Then
                    originalAlbumWasDeleted = False
                    Exit For
                End If
            Next
            If originalAlbumWasDeleted Then
                Trace.WriteLine("Deleting album " & originalMetadata.FileName)
                If axe.deleteFile(originalMetadata.ID) = "-1" Then
                    albumSyncError = True
                    Trace.WriteLine("Error deleting album " & originalMetadata.FileName)
                End If

                'delete songs too, if the checkbox was checked
                If Me.chkDeleteSongsOnAlbumDelete.Checked Then
                    For Each song As TreeNode In originalAlbum.Nodes
                        Trace.WriteLine("Deleting song " & song.Tag.filename)
                        If axe.deleteFile(song.Tag.id) = "-1" Then
                            albumSyncError = True
                            Trace.WriteLine("Error deleting song " & song.Tag.filename)
                        End If
                    Next
                End If
            End If
        Next
        Trace.WriteLine("Deleting old albums Done")

        Trace.WriteLine("Uploading new albums")

        'upload any new files or update modified albums
        For Each modifiedAlbum As TreeNode In modifiedAlbums.Nodes
            If isAlbumModified(modifiedAlbum) Then
                Dim modifiedMetadata As StorageItem = modifiedAlbum.Tag

                'keeps track of whether songs were uploaded (i.e. the album has been modified and we need to re-create it)
                Dim songsUploaded As Boolean = False
                'used to keep track of the id's of files that were uploaded so we can add them to the album
                Dim uploadedFilesIDs As String = ""

                Trace.WriteLine("Uploading new albums - uploading songs for album " & modifiedMetadata.AlbumTitle)
                For Each song As TreeNode In modifiedAlbum.Nodes
                    Dim songMetadata As StorageItem
                    songMetadata = CType(song.Tag, StorageItem)

                    If songMetadata.ID = "" Then
                        'songs with id "" are new songs(see buildAlbumsListFromPaths)

                        Dim folderToUploadToName As String
                        Dim folderToUploadTo As TreeNode
                        Dim ret As String

                        'try to find the folder to upload this song to
                        folderToUploadToName = modifiedMetadata.AlbumArtist & " - [" & modifiedMetadata.Year & "] " & modifiedMetadata.AlbumTitle
                        folderToUploadTo = findTreeNodeByName(fullFileListing.Nodes(0), folderToUploadToName)

                        If folderToUploadTo Is Nothing Then
                            'the folder doesn't exist, so create it
                            folderToUploadTo = New TreeNode
                            Dim tmp As New StorageItem
                            folderToUploadTo.Tag = tmp
                            Splash.setText("Creating folder " & folderToUploadToName)
                            Trace.WriteLine("Creating folder " & folderToUploadToName)
                            ret = axe.uploadFile(folderToUploadToName, musicFolder.Tag.id, 1)
                            If ret = "-1" Then
                                Trace.WriteLine("Sync album error: Couldn't create folder " & folderToUploadToName & " in folder MUSIC")
                                albumSyncError = True
                                folderToUploadTo = Nothing
                            Else
                                tmp.ID = ret
                                tmp.FileName = folderToUploadToName
                                tmp.StorageType = 295176
                                tmp.DirectoryDepth = musicFolder.Tag.directorydepth + 1
                                tmp.ParentFileName = musicFolder.Tag.filename
                                tmp.ParentID = musicFolder.Tag.id
                                folderToUploadTo.Tag = tmp
                                'now that the folder has been successfully created, add it to the fullFileListing tree
                                musicFolder.Nodes.Add(folderToUploadTo)
                            End If
                        End If

                        If folderToUploadTo IsNot Nothing Then
                            'we now have a valid folder to put the song in so now we can upload

                            Trace.WriteLine("Uploading " & songMetadata.FilePath)
                            Splash.setText("Uploading " & songMetadata.FileName)
                            ret = axe.uploadFile(songMetadata.FilePath, folderToUploadTo.Tag.id, 0, songMetadata)
                            If ret = "-1" Then
                                Trace.WriteLine("error syncing album. couldn't upload file " & songMetadata.FilePath & " Out of space maybe?")
                                albumSyncError = True
                            Else
                                uploadedFilesIDs = uploadedFilesIDs & ret & ":"
                            End If

                            songsUploaded = True
                        End If

                    End If 'if song is new
                Next
                Trace.WriteLine("Uploading new albums - uploading songs for album " & modifiedMetadata.AlbumTitle & " Done")

                'if the album has been modified by adding songs or changing the 
                'album art, need to delete it and re-create it
                If (songsUploaded Or Not modifiedMetadata.AlbumArtIsFromPlayer) And Not albumSyncError Then
                    If modifiedMetadata.ID.StartsWith("{") Then
                        'if were here, it means songs were uploaded to an existing album
                        'need to make a list of items already in the album so as to
                        're-add them to the album after it has been deleted and recreated
                        For Each song In modifiedAlbum.Nodes
                            If song.tag.id <> "" Then
                                're-create the list
                                uploadedFilesIDs = uploadedFilesIDs & song.tag.id & ":"
                            End If
                        Next

                        'delete the modified album
                        Trace.WriteLine("Creating albums - deleting modified album " & modifiedMetadata.AlbumTitle)
                        If axe.deleteFile(modifiedMetadata.ID) = "-1" Then
                            Trace.WriteLine("Error deleting album " & modifiedMetadata.AlbumTitle)
                            Exit Sub
                        End If
                        modifiedAlbum.Remove()
                        modifiedAlbum = Nothing
                    End If
                    'chop off the final delimiter
                    uploadedFilesIDs = Mid(uploadedFilesIDs, 1, uploadedFilesIDs.Length - 1)

                    'upload the album, if it is new or has been modified
                    Trace.WriteLine("Creating albums - creating " & modifiedMetadata.AlbumTitle)
                    Dim ret As String = axe.createAlbum(modifiedMetadata.AlbumTitle, uploadedFilesIDs, modifiedMetadata)
                    If ret = "-1" Then
                        albumSyncError = True
                    End If

                End If
            End If 'if album is modified

            If albumSyncError Then
                albumSyncErrorOccurred = True
            End If
            albumSyncError = False
        Next
        Trace.WriteLine("Uploading albums Done")

        Trace.WriteLine("Syncing albums...done")

        If albumSyncErrorOccurred Then
            MsgBox("There were errors syncing albums. Check the log", MsgBoxStyle.Exclamation Or MsgBoxStyle.SystemModal)
        End If

        updateDeviceFreeSpace()
    End Sub

    Private Sub refreshAlbumsList()
        'note: this function only uses the fullFileListing tree for finding the playlists
        'not for finding the playlist contents.  This is because the fullFileListing tree
        'does not contain the reference information contained inside of the playlists.
        'As a result it is still necessary to call some enumeration functions on the device

        Trace.WriteLine("Refreshing albums list...")

        'let the user know were busy
        Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
        t.SetApartmentState(Threading.ApartmentState.MTA)
        If Not Splash.Visible Then
            t.Start()
            Splash.setTitle(Me.cmbDevices.Text)
        End If

        Splash.setText("Getting albums")

        clearAlbumDetailsView()
        removeAllAlbumsFromAlbumsList()
        removeAllAlbumsFromAlbumsList()
        cleanUpAxeTmpFiles()

        'set the image list
        If Me.lvAlbumsList.SmallImageList Is Nothing Then
            Dim imglist As New ImageList
            imglist.ColorDepth = ColorDepth.Depth32Bit
            imglist.Images.Add("+", My.Resources.Album_entryWithArt)
            imglist.Images.Add("-", My.Resources.Album_entryWithoutArt)
            Me.lvAlbumsList.SmallImageList = imglist
        End If

        originalAlbums = New TreeView
        modifiedAlbums = New TreeView

        Dim plFldr As TreeNode
        plFldr = findTreeNodeByName(fullFileListing.Nodes(0), "Albums")
        If plFldr Is Nothing Then
            Trace.WriteLine("Error refreshing albums - Albums folder not found")
            Exit Sub
        End If
        If plFldr.Nodes.Count = 0 Then
            Trace.WriteLine("Error refreshing albums - no albums found")
            Exit Sub
        End If

        For Each node As TreeNode In plFldr.Nodes
            Dim albumMetadata As StorageItem = CType(node.Tag, StorageItem)
            If albumMetadata IsNot Nothing Then

                'clone the original node (we don't want to modify the version in the fullFileListing
                node = node.Clone

                'now get the album contents
                Dim strIDs() As String = axe.getPlaylistContentsIDs(albumMetadata.ID)
                Dim albumSong As TreeNode
                If strIDs IsNot Nothing Then

                    For Each strid In strIDs
                        If fullFileListing IsNot Nothing AndAlso fullFileListing.Nodes.Count > 0 Then
                            'serach the tree for the referenced storage item
                            albumSong = findTreeNodeByID(fullFileListing.Nodes(0), strid)
                            If albumSong IsNot Nothing Then

                                'add the song to the album tree (note this node is the cloned version
                                'of the orignal album node since we don't want to modify the original
                                node.Nodes.Add(albumSong.Clone)

                                'add to the album size, the size of this song
                                node.Tag.size += albumSong.Tag.size
                            Else
                                Trace.WriteLine("The storage item " & strid & " referenced in album '" & albumMetadata.FileName & "' was not found")
                            End If
                        End If
                        albumSong = Nothing
                    Next

                End If 'if strids isnot nothing

                'now that we have the album and all it's files add them to the internal
                'list that keeps track of the albums
                originalAlbums.Nodes.Add(node)
                'clone the node since we don't want to modify the node in the originalAlbums list
                'the cloned node corresponding to the album 
                'will be stored in the tag of the listview item to avoid  having to search for the node later
                node = node.Clone
                modifiedAlbums.Nodes.Add(node)

                'add the album to teh albums list listview
                Dim albumItem As New ListViewItem
                If IO.Path.GetExtension(albumMetadata.FileName) = ".alb" Then
                    albumMetadata.AlbumTitle = albumMetadata.Title
                Else
                    'the album title is contained in the filename
                    albumMetadata.AlbumTitle = albumMetadata.FileName
                    'the album artist is contained in the genre
                    albumMetadata.AlbumArtist = albumMetadata.Genre
                    'the genre and year are contained in the title
                    Dim genreYear() As String = albumMetadata.Title.Split(":")
                    If genreYear.Length >= 2 Then
                        albumMetadata.Year = genreYear(1)
                        albumMetadata.Genre = genreYear(0)
                    End If
                End If


                If albumMetadata.AlbumTitle = "" Then albumMetadata.AlbumTitle = "Unknown"
                If albumMetadata.Year = "" Then albumMetadata.Year = "0"
                If albumMetadata.AlbumArtist = "" Then albumMetadata.AlbumArtist = "Unknown"
                If albumMetadata.Genre = "" Then albumMetadata.Genre = "Unknown"
                albumMetadata.AlbumArtIsFromPlayer = True
                albumItem.Text = albumMetadata.AlbumTitle
                albumItem.Name = albumMetadata.AlbumTitle
                albumItem.SubItems.Add(albumMetadata.AlbumArtist)
                albumItem.SubItems.Add(albumMetadata.Year)
                albumItem.SubItems.Add(albumMetadata.Genre)
                albumItem.ImageKey = IIf(albumMetadata.AlbumArtPath = "", "-", "+")
                albumItem.Tag = node
                Me.lvAlbumsList.Items.Add(albumItem)

            End If

        Next

        t.Abort()
        Trace.WriteLine("Refreshing albums...complete")

        Me.cmbAlbumListGroupBy.SelectedIndex = -1
        Me.cmbAlbumListGroupBy.SelectedIndex = 0
    End Sub
    Private Sub removeAlbumFromAlbumsList(ByRef album As ListViewItem, Optional ByVal reallyDelete As Boolean = False)
        'removes an album from the list. album is alistview item from the
        'lvAlbumList listview

        clearAlbumDetailsView()

        If album Is Nothing Then
            Trace.WriteLine("Can't deelte album. No album selected")
            Exit Sub
        End If

        If album.Tag Is Nothing Then
            Trace.WriteLine("Can't delete album " & album.Text & " no node was associated with it")
            Exit Sub
        End If

        Dim item As StorageItem

        'remember that the listviewitem tag has the treenode
        item = CType(album.Tag.tag, StorageItem)
        If item IsNot Nothing Then
            'delete the cover art image if it was temporary
            deleteTmpFile(item.AlbumArtPath)

            'update the free space before we delete the tag
            updateDeviceFreeSpace(item.Size, False)

            'remove the album from the modified albums list
            album.Tag.remove()
            album.Tag = Nothing

            'dont actually delete the item. it causes too many problems
            'with the ItemSelectionChanged event
            album.Font = New Font("Tahoma", 8, FontStyle.Strikeout)
            'DANGER DANGER! try to avoid using this
            If reallyDelete Then
                album.Remove()
                album = Nothing
            End If

        Else
            Trace.WriteLine("remove album: could not cleanly remove " & album.Text & ". The associated metadata was not found")
        End If

    End Sub
    Private Sub removeAllAlbumsFromAlbumsList(Optional ByVal clearListView As Boolean = True)
        'remove the handler to prevent the album info being loaded when an item is deleted
        '(the selection changes when the items is deleted)
        clearAlbumDetailsView()
        Me.lvAlbumsList.BeginUpdate()


        Dim metadata As StorageItem = Nothing
        For Each Album As ListViewItem In Me.lvAlbumsList.Items
            removeAlbumFromAlbumsList(Album)
        Next

        If clearListView Then
            Me.lvAlbumsList.Groups.Clear()
            Me.lvAlbumsList.Items.Clear()
        End If


        Me.lvAlbumsList.EndUpdate()
    End Sub
    Private Sub setNewSongOrder(ByRef album As ListViewItem, ByVal songs As ListView.ListViewItemCollection)
        'reorders the songs in the specified album. Album is a listviewitem from the AlbumsList listview
        'the song will be added to the internal treeview that keeps track of modified albums
        'songs are the items of the album in the desired order

        'note most of this is here for completeness, as the player ignores the album order.
        'additionally, since the player does it's own album grouping, manualy adding
        'songs to an album has been disabled
        'this was done by setting the Allow dragdrop property of the listview)
        'This code is still used when an album item is deleted though

        Trace.WriteLine("Setting new song order...")

        If songs Is Nothing Then
            MsgBox("Could not reorder songs: no song was specified", MsgBoxStyle.Critical)
            Trace.WriteLine("Could not reorder songs: no song was specified")
            Exit Sub
        End If

        If album Is Nothing Then
            MsgBox("Could not reorder songs: no album was selected", MsgBoxStyle.Critical)
            Trace.WriteLine("Could not reorder songs: no album was selected")
            Exit Sub
        End If

        'get the storageitem of the selected album
        Dim metadata As StorageItem = CType(album.Tag.tag, StorageItem)
        If metadata Is Nothing Then
            MsgBox("Could not reorder songs in album " & album.Text & "as changed: no metadata available", MsgBoxStyle.Critical)
            Trace.WriteLine("Could not reorder songs in album " & album.Text & "as changed: no metadata available")
            Exit Sub
        End If

        Dim foundAlbum As TreeNode = album.Tag

        If foundAlbum Is Nothing Then
            Trace.WriteLine("setNewSongOrder: no album is selected")
            MsgBox("no album is selected")
            Exit Sub
        End If

        'we now have the node in the modified albums tree corresponding to the specified album

        foundAlbum.Nodes.Clear()
        Dim newSongNode As TreeNode
        Dim newSongMetadata As StorageItem
        For Each newsong As ListViewItem In songs
            newSongNode = New TreeNode

            newSongMetadata = CType(newsong.Tag, StorageItem)
            newSongNode.Text = newSongMetadata.FileName
            newSongNode.Tag = newSongMetadata

            foundAlbum.Nodes.Add(newSongNode)

        Next

        markAlbumChanged(album)

        Trace.WriteLine("Setting new song order...Done. Reordered album " & metadata.AlbumTitle)
    End Sub
    Private Sub saveAlbumMetadata(ByRef album As ListViewItem)
        'saves the values in the album textboxes (title etc) to the current album if any of the values
        'changed.
        'Note a lot of this code isn't being used. see the note near the txtAlbumTitle_KeyDown sub.
        'this sub is only being used for when the album art image changes

        If album IsNot Nothing Then

            If Not validateAlbumTextBoxes() Then Exit Sub

            Trace.WriteLine("Saving album metadata...")

            'remember that the lvAlbumItems tag has the listview item of the currently selected
            'album.  The tag of the listview item contains the node associated with the album
            'and finally, the node tag contains the album metadata
            Dim albumMetaData As StorageItem = CType(album.Tag.tag, StorageItem)
            If albumMetaData IsNot Nothing Then
                albumMetaData.AlbumTitle = Me.txtAlbumTitle.Text
                albumMetaData.AlbumArtist = Me.txtAlbumArtist.Text
                albumMetaData.Genre = Me.txtAlbumGenre.Text
                albumMetaData.Year = Me.txtAlbumYear.Text
                albumMetaData.AlbumArtPath = IIf(pbAlbumArt.Tag Is Nothing, "", pbAlbumArt.Tag)


                'update the albums list
                Dim albumLVItem As ListViewItem = album
                albumLVItem.Text = albumMetaData.AlbumTitle
                albumLVItem.SubItems(1).Text = albumMetaData.AlbumArtist
                albumLVItem.SubItems(2).Text = albumMetaData.Year
                albumLVItem.SubItems(3).Text = albumMetaData.Genre
                If albumMetaData.AlbumArtPath <> "" Then
                    albumLVItem.ImageKey = "+"
                End If

                'update the treenode
                album.Tag.tag = albumMetaData

                markAlbumChanged(albumLVItem)

                Me.txtAlbumArtist.Modified = False
                Me.txtAlbumGenre.Modified = False
                Me.txtAlbumTitle.Modified = False
                Me.txtAlbumYear.Modified = False

                Trace.WriteLine("Saving album metadata...Done")
            End If

        Else
            Trace.WriteLine("Saving album metadata...no album selected")
        End If
    End Sub
    Private Sub markAlbumChanged(ByRef album As ListViewItem)
        'change the state of the album list item so we can tell it's been modified
        If album Is Nothing Then
            MsgBox("Could not mark the album as changed: no album was selected", MsgBoxStyle.Critical)
            Trace.WriteLine("Could not mark the album as changed: no album was selected")
            Exit Sub
        End If

        If Not album.Checked Then
            album.Checked = True

            album.Tag.checked = True
            'change the album listviewitem text to reflect the fact that it's been modified
            If Not album.Text.EndsWith("*"c) Then
                album.Text = album.Text & "*"
            End If
        End If
    End Sub
    Private Function isAlbumModified(ByRef album As ListViewItem) As Boolean
        'checks whether the specified album has been modified
        If album Is Nothing Then
            Return False
        End If

        Return album.Checked
    End Function
    Private Function isAlbumModified(ByRef album As TreeNode) As Boolean
        'checks whether the specified album has been modified
        If album Is Nothing Then
            Return False
        End If

        Return album.Checked
    End Function


    Private Sub clearAlbumDetailsView()
        'clears all textboxes and fields in the album details view
        If Me.pbAlbumArt.Image IsNot Nothing Then
            Me.pbAlbumArt.Image.Dispose()
        End If
        Me.pbAlbumArt.Image = My.Resources.NoDeviceIcon
        Me.pbAlbumArt.Tag = ""
        Me.lvAlbumItems.Items.Clear()
        Me.txtAlbumTitle.Clear()
        Me.txtAlbumArtist.Clear()
        Me.txtAlbumGenre.Clear()
        Me.txtAlbumYear.Clear()
        Me.lblAlbumArtDimensions.Text = ""
        Me.lblAlbumArtFileSize.Text = ""
        Me.lblAlbumNumberOfTracks.Text = ""
        Me.lblAlbumTotalSize.Text = ""

        'reset the modified status of the textboxes
        Me.txtAlbumArtist.Modified = False
        Me.txtAlbumGenre.Modified = False
        Me.txtAlbumTitle.Modified = False
        Me.txtAlbumYear.Modified = False

        Me.lvAlbumItems.Tag = Nothing

        'clean up the columheaders from sorting indicator and disable sorting
        Me.lvAlbumItems.Sorting = SortOrder.None
        Me.lvAlbumItems.ListViewItemSorter = Nothing
        If lvAlbumItems_lastColumnClicked <> -1 Then
            Me.lvAlbumItems.Columns(lvAlbumItems_lastColumnClicked).Text = Me.lvAlbumItems.Columns(lvAlbumItems_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
        End If
    End Sub

    Private Sub btnClearAllAlbums_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles btnClearAllAlbums.LinkClicked
        If MsgBox("Are you sure?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        removeAllAlbumsFromAlbumsList(False)
    End Sub
    Private Sub chkDeleteSongsOnAlbumDelete_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDeleteSongsOnAlbumDelete.CheckedChanged
        Me.theSettings.DeleteAlbumSongsOnAlbumDelete = Me.chkDeleteSongsOnAlbumDelete.Checked
    End Sub
    Private Sub cmbAlbumListGroupBy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAlbumListGroupBy.SelectedIndexChanged
        If Me.cmbAlbumListGroupBy.SelectedIndex <> -1 Then
            createGroupsFromColumn(Me.lvAlbumsList, Me.cmbAlbumListGroupBy.SelectedIndex + 1)
            GC.Collect()
        End If
    End Sub

    Private Sub lvAlbumsList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvAlbumsList.KeyDown
        If e.KeyCode = Keys.Delete Then
            For Each Album As ListViewItem In Me.lvAlbumsList.SelectedItems
                removeAlbumFromAlbumsList(Album)
            Next
        End If
    End Sub
    Private Sub lvAlbumsList_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvAlbumsList.ItemSelectionChanged

        clearAlbumDetailsView()

        If fullFileListing IsNot Nothing Then
            Me.lvAlbumItems.SmallImageList = fullFileListing.ImageList
        End If

        'if there is only 1 item selected, show it's details
        If Me.lvAlbumsList.SelectedItems.Count = 1 And e.IsSelected Then
            Dim selectedItem As ListViewItem = Me.lvAlbumsList.SelectedItems(0)
            Dim foundNode As TreeNode = selectedItem.Tag
            If foundNode Is Nothing Then Exit Sub

            'remember the tag of listview item in the albums list contains the reference to the
            'associated album node. the tag of the album node contains the metadata. this avoids
            'having to do a search for the node to update it.
            Dim metadata As StorageItem = CType(foundNode.Tag, StorageItem)
            If metadata IsNot Nothing Then

                'once we have the album node, add all of the items to the list
                Me.lvAlbumItems.Items.Clear()
                Dim lvitem As ListViewItem
                Dim songMetadata As StorageItem
                'Dim totalalbumsize As Integer = 0
                Dim allSongsHaveTrackNumTag As Boolean = True
                For Each node In foundNode.Nodes
                    songMetadata = CType(node.tag, StorageItem)
                    If songMetadata IsNot Nothing Then
                        lvitem = New ListViewItem
                        lvitem.Text = songMetadata.FileName
                        lvitem.SubItems.Add(songMetadata.TrackNum)
                        lvitem.SubItems.Add(songMetadata.Title)
                        lvitem.SubItems.Add(songMetadata.AlbumArtist)
                        lvitem.SubItems.Add(songMetadata.Year)
                        lvitem.SubItems.Add(songMetadata.Genre)
                        lvitem.SubItems.Add(Math.Ceiling(songMetadata.Size / 1024).ToString("N0") & "KB")

                        'totalalbumsize += songMetadata.Size

                        lvitem.Tag = songMetadata
                        lvitem.ImageKey = IO.Path.GetExtension(songMetadata.FileName)

                        If Not IsNumeric(songMetadata.TrackNum) Then
                            allSongsHaveTrackNumTag = False
                        End If

                        Me.lvAlbumItems.Items.Add(lvitem)
                    End If
                Next
                'metadata.Size = totalalbumsize

                'once all the items have been added to the list, sort them the way the player would:
                'If all the tracks have a tracknum tag, sort asceding by tracknum, else, sort
                'asending by title. of course this sort won't work for files where the player
                'doesn't store the metadata, like aac files so just sort by filename in this case
                If allSongsHaveTrackNumTag Then
                    lvAlbumItems_ColumnClick(Me.lvAlbumItems, New ColumnClickEventArgs(1))
                Else
                    lvAlbumItems_ColumnClick(Me.lvAlbumItems, New ColumnClickEventArgs(0))
                End If

                'add album information

                'get the album art from the player. get it here to avoid readin
                'the album art of all the files on refresh. that way, images are only read as needed
                'only change the albumart path if there is no currently valid albumart
                If Not IO.File.Exists(metadata.AlbumArtPath) Then
                    Dim art As String = axe.getAlbumArt(metadata.ID)
                    If art <> "-1" Then
                        metadata.AlbumArtPath = art
                        metadata.AlbumArtIsFromPlayer = True
                    End If
                End If


                Me.txtAlbumArtist.Text = metadata.AlbumArtist
                Me.txtAlbumGenre.Text = metadata.Genre
                Me.txtAlbumTitle.Text = metadata.AlbumTitle
                Me.txtAlbumYear.Text = metadata.Year
                Me.lblAlbumNumberOfTracks.Text = Format(foundNode.Nodes.Count, "00")
                Me.lblAlbumTotalSize.Text = Math.Round(metadata.Size / 1024 / 1024, 2).ToString("N1") & "MB"
                Try
                    Me.pbAlbumArt.Image = New Bitmap(metadata.AlbumArtPath)
                    Me.lblAlbumArtFileSize.Text = Math.Round((New IO.FileInfo(metadata.AlbumArtPath)).Length / 1024, 2).ToString("N1") & " KB"
                    Me.lblAlbumArtDimensions.Text = Me.pbAlbumArt.Image.Width.ToString & " x " & Me.pbAlbumArt.Image.Height.ToString
                    Me.pbAlbumArt.Tag = metadata.AlbumArtPath
                    selectedItem.ImageKey = "+"
                Catch ex As Exception
                    Trace.WriteLine("Album Activate: error displaying album art - " & metadata.AlbumArtPath & ". " & ex.Message & "," & ex.Source)
                    Me.pbAlbumArt.Image = My.Resources.NoDeviceIcon
                    Me.pbAlbumArt.Tag = ""
                    selectedItem.ImageKey = "-"
                End Try

                'save the selected album into the tag of the album items listview for easy retrieval 
                Me.lvAlbumItems.Tag = selectedItem

            End If 'if metadata isnot nothing

        End If


    End Sub

    Private Sub lvAlbumsList_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvAlbumsList.DragEnter
        Me.Activate()
        e.Effect = e.AllowedEffect
    End Sub
    Private Sub lvAlbumsList_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvAlbumsList.DragDrop
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        'check to see if files and folders are being dragged from explorer
        Dim draggedFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        If draggedFiles IsNot Nothing Then
            Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
            t.SetApartmentState(Threading.ApartmentState.MTA)
            If Not Splash.Visible Then t.Start()
            Splash.setText("Building album list")
            Splash.setTitle("Building album list..")

            lvAlbumslist_DragDrop_helper(draggedFiles)

            Splash.Close()
            t.Abort()
        End If


    End Sub
    Private Sub lvAlbumslist_DragDrop_helper(ByVal draggedFiles() As String)
        Dim albums As TreeView
        Trace.WriteLine("Building albums list...")
        Array.Sort(draggedFiles)
        albums = buildAlbumsListFromPaths(draggedFiles)
        If albums Is Nothing Then
            MsgBox("No albums found")
            Trace.WriteLine("Building albums list...Done - no albums found")
            Exit Sub
        End If

        'add the returned albums to the listview
        If Me.lvAlbumsList.SmallImageList Is Nothing Then
            Dim imglist As New ImageList
            imglist.ColorDepth = ColorDepth.Depth32Bit
            imglist.Images.Add("+", My.Resources.Album_entryWithArt)
            imglist.Images.Add("-", My.Resources.Album_entryWithoutArt)
            Me.lvAlbumsList.SmallImageList = imglist
        End If

        For Each node As TreeNode In albums.Nodes
            'check to see if the album exists, if it does
            'ask to overwrite
            Dim metadata As StorageItem = CType(node.Tag, StorageItem)

            Dim overwriteAlbumIfExists As Boolean = True
            If Me.lvAlbumsList.Items.ContainsKey(metadata.AlbumTitle) Then
                If MsgBox("Album ' " & metadata.AlbumTitle & "' already exists. Overwrite it?" & vbCrLf & _
                       "The album's music files won't be deleted unless 'Delete Songs on Album Delete' is checked" & vbCrLf & _
                       "and the device is then synced", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo Or MsgBoxStyle.SystemModal) = MsgBoxResult.No Then
                    overwriteAlbumIfExists = False
                Else
                    removeAlbumFromAlbumsList(Me.lvAlbumsList.Items(metadata.AlbumTitle), True)
                    overwriteAlbumIfExists = True
                End If
            End If

            If overwriteAlbumIfExists Then
                node = node.Clone

                Dim lvitem As New ListViewItem

                lvitem.Text = metadata.AlbumTitle
                lvitem.Name = metadata.AlbumTitle
                lvitem.SubItems.Add(metadata.AlbumArtist)
                lvitem.SubItems.Add(metadata.Year)
                lvitem.SubItems.Add(metadata.Genre)
                lvitem.Tag = node
                If metadata.AlbumArtPath = "" Then
                    lvitem.ImageKey = "-"
                Else
                    lvitem.ImageKey = "+"
                End If

                markAlbumChanged(lvitem)

                Me.lvAlbumsList.Items.Add(lvitem)

                'add the new items to the modified albums tree
                If modifiedAlbums IsNot Nothing Then
                    modifiedAlbums.Nodes.Add(node)
                End If

                'update the free space(the free space decreases)
                Dim songMetadata As StorageItem
                For Each song As TreeNode In node.Nodes
                    songMetadata = CType(song.Tag, StorageItem)
                    updateDeviceFreeSpace(-songMetadata.Size, False)
                Next
            End If

        Next

        'now that we've added all the items, create the groups. don't do this inside of the loop
        'to add items since it's not possible to sort items within groups. they appear
        'in the order they were added to the group
        Me.cmbAlbumListGroupBy.SelectedIndex = -1 'deselect any currently selected item
        Me.cmbAlbumListGroupBy.SelectedIndex = 0

        'can safely clear these nodes since we cloned them in the for loop
        albums.Nodes.Clear()
        albums = Nothing

        Trace.WriteLine("Building albums list...Done")
    End Sub


    Private Sub btnDeleteAlbum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAlbum.Click
        Dim album As ListViewItem = Me.lvAlbumItems.Tag
        If album Is Nothing Then
            MsgBox("CAn't delete the current album: no album is selected", MsgBoxStyle.Exclamation)
            Trace.WriteLine("CAn't delete the current album: no album is selected")
            Exit Sub
        End If

        removeAlbumFromAlbumsList(album)
    End Sub

    '*******************************************************************
    '*The code to update the album information is here for completeness
    '*since the player ignores it(it reads everything from the tags)
    '*this code isn't currently being used
    '********************************************************************
    Private Sub txtAlbumTitle_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAlbumTitle.KeyDown
        'FUNCTION DISABLED
        Exit Sub

        If e.KeyCode = Keys.Enter Then
            Me.txtAlbumArtist.Focus()
        End If
    End Sub
    Private Sub txtAlbumTitle_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlbumTitle.LostFocus
        'FUNCTION DISABLED
        Exit Sub

        saveAlbumMetadata(Me.lvAlbumItems.Tag)
    End Sub
    Private Sub txtAlbumArtist_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAlbumArtist.KeyDown
        'FUNCTION DISABLED
        Exit Sub

        If e.KeyCode = Keys.Enter Then
            Me.txtAlbumGenre.Focus()
        End If
    End Sub
    Private Sub txtAlbumArtist_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlbumArtist.LostFocus
        'FUNCTION DISABLED
        Exit Sub

        saveAlbumMetadata(Me.lvAlbumItems.Tag)
    End Sub
    Private Sub txtAlbumGenre_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAlbumGenre.KeyDown
        'FUNCTION DISABLED
        Exit Sub

        If e.KeyCode = Keys.Enter Then
            Me.txtAlbumYear.Focus()
        End If
    End Sub
    Private Sub txtAlbumGenre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlbumGenre.LostFocus
        'FUNCTION DISABLED
        Exit Sub

        saveAlbumMetadata(Me.lvAlbumItems.Tag)
    End Sub
    Private Sub txtAlbumYear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAlbumYear.KeyDown
        'FUNCTION DISABLED
        Exit Sub

        If e.KeyCode = Keys.Enter Then
            Me.txtAlbumTitle.Focus()
        End If
    End Sub
    Private Sub txtAlbumYear_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAlbumYear.LostFocus
        'FUNCTION DISABLED
        Exit Sub

        saveAlbumMetadata(Me.lvAlbumItems.Tag)
    End Sub
    Private Sub addSongToAlbum(ByRef Album As ListViewItem, ByVal songMetadata As StorageItem)
        'adds a song to the specified album. Album is a listviewitem from the AlbumsList listview
        'the song will be added to the internal treeview that keeps track of modified albums


        'note this is here for completeness, as the player ignores the album order.
        'additionally, since the player does it's own album grouping, manualy adding
        'songs to an album has been disabled
        'this was done by setting the Allow dragdrop property of the listview)
        'This function was used in the dragdrop event of the lvAlbumItems therefore
        'this function will no longer be called...
        Trace.WriteLine("Adding song...")

        If songMetadata Is Nothing Then
            MsgBox("Could not add a song to the album: no song was specified", MsgBoxStyle.Critical)
            Trace.WriteLine("Could not add a song to the album: no song was specified")
            Exit Sub
        End If

        If Album Is Nothing Then
            MsgBox("Could not add a song to the album: no album was selected", MsgBoxStyle.Critical)
            Trace.WriteLine("Could not add a song to the album: no album was selected")
            Exit Sub
        End If

        'get the storageitem of the selected album
        Dim metadata As StorageItem = CType(Album.Tag, StorageItem)
        If metadata Is Nothing Then
            MsgBox("Could not add song to album " & Album.Text & ": no metadata available", MsgBoxStyle.Critical)
            Trace.WriteLine("Could not add song to album " & Album.Text & ": no metadata available")
            Exit Sub
        End If

        'find the album in the modified albums list
        Dim foundAlbum As TreeNode = Nothing
        For Each node As TreeNode In modifiedAlbums.Nodes
            foundAlbum = findTreeNodeByID(node, metadata.ID)
            If foundAlbum IsNot Nothing Then
                Exit For
            End If
        Next
        If foundAlbum Is Nothing Then
            MsgBox("Couldn't add song " & songMetadata.FileName & " to album " & metadata.AlbumTitle & ". The album was not found in the tree")
            Trace.WriteLine("Couldn't add song " & songMetadata.FileName & " to album " & metadata.AlbumTitle & ". The album was not found in the tree")
            Exit Sub
        End If

        'add it to the listview                       
        Dim lvitem As New ListViewItem
        lvitem.Text = songMetadata.FileName
        lvitem.SubItems.Add(songMetadata.TrackNum)
        lvitem.SubItems.Add(songMetadata.Title)
        lvitem.SubItems.Add(songMetadata.AlbumArtist)
        lvitem.SubItems.Add(songMetadata.Year)
        lvitem.SubItems.Add(songMetadata.Genre)
        lvitem.SubItems.Add(Math.Ceiling(songMetadata.Size / 1024).ToString("N0") & "KB")

        lvitem.Tag = songMetadata
        lvitem.ImageKey = IO.Path.GetExtension(songMetadata.FileName)

        Me.lvAlbumItems.Items.Add(lvitem)

        'now that we have the album in the modified albums list, add the new song to it
        Dim newSongNode As New TreeNode
        newSongNode.Text = songMetadata.FileName
        newSongNode.Tag = songMetadata
        foundAlbum.Nodes.Add(newSongNode)

        'save the album changes 
        markAlbumChanged(Album)

        Trace.WriteLine("Adding song...Done. Added " & songMetadata.FileName & " to album " & metadata.AlbumTitle)
    End Sub
    Private Function validateAlbumTextBoxes() As Boolean
        Dim result As Boolean = True
        If Me.txtAlbumTitle.Text = "" Then
            'for some reason, using a msgbox here causes the lostfocus event to fire twice
            Trace.WriteLine("Album title cannot be blank")
            Me.txtAlbumTitle.Text = "Album"
            Me.txtAlbumTitle.Modified = True
            result = False
        End If

        Return result
    End Function
    Private Sub lvAlbumItems_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvAlbumItems.DragEnter
        Me.Activate()
        e.Effect = e.AllowedEffect
    End Sub
    Private Sub lvAlbumItems_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles lvAlbumItems.DragDrop

        If Me.lvAlbumItems.Tag Is Nothing Then
            MsgBox("Can't add songs. No album is currently selected. If this is a new album. Click on the 'Add new album' button first before adding songs.")
            Trace.WriteLine("AlbumItems DragDrop:Can't add songs. No album is currently selected")
            Exit Sub
        End If

        'check to see if files and folders are being dragged from explorer
        Dim draggedFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        If draggedFiles IsNot Nothing Then
            Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
            t.SetApartmentState(Threading.ApartmentState.MTA)
            If Not Splash.Visible Then t.Start()
            Splash.setText("Building album list")
            Splash.setTitle("Adding new items to album ")

            Dim albums As TreeView
            Array.Sort(draggedFiles)
            albums = buildAlbumsListFromPaths(draggedFiles)
            If albums IsNot Nothing Then
                'extract all of the songs and add them to the current album
                For Each node As TreeNode In albums.Nodes
                    For Each song As TreeNode In node.Nodes

                        Dim songMetadata As StorageItem = CType(song.Tag, StorageItem)
                        If songMetadata Is Nothing Then
                            songMetadata = New StorageItem
                        End If

                        'add it to the album
                        addSongToAlbum(Me.lvAlbumItems.Tag, songMetadata)

                    Next
                Next
            End If
            t.Abort()
        Else
            'check to see if theyre from the albumitems list
            Dim draggedListviewItem As Object = e.Data.GetData(GetType(ListViewEx.DragItemData))
            If Not draggedListviewItem Is Nothing Then
                'clean up the columheaders from sorting indicator and disable sorting
                Me.lvAlbumItems.Sorting = SortOrder.None
                Me.lvAlbumItems.ListViewItemSorter = Nothing
                If playlistListView_lastColumnClicked <> -1 Then
                    Me.lvAlbumItems.Columns(lvAlbumItems_lastColumnClicked).Text = Me.lvAlbumItems.Columns(lvAlbumItems_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
                End If

                'save the song order by deleting all of the nodes currently in the album and
                're-adding them in the order specified by the playlist
                setNewSongOrder(Me.lvAlbumItems.Tag, Me.lvAlbumItems.Items)

            End If
        End If

    End Sub
    Private Sub btnAddNewAlbum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '***DISABLED****
        'not much use in adding a blank album, since the player builds it's own album list anyways

        'creates a new empty album and adds it to the albums list

        Dim lvitem As New ListViewItem
        Dim metadata As New StorageItem
        Dim newAlbumNode As New TreeNode

        clearAlbumDetailsView()

        Me.txtAlbumTitle.Text = "Album"

        metadata.AlbumTitle = Me.txtAlbumTitle.Text
        metadata.AlbumArtist = Me.txtAlbumArtist.Text
        metadata.Genre = Me.txtAlbumGenre.Text
        metadata.Year = Me.txtAlbumYear.Text
        metadata.AlbumArtPath = IIf(pbAlbumArt.Tag Is Nothing, "", pbAlbumArt.Tag)
        'since this album is not yet on the player, we need a way to identify it
        'so we can search the tree for it later
        metadata.ID = Now.Ticks.ToString & metadata.AlbumTitle

        lvitem.Text = metadata.AlbumTitle & "*" 'mark the album as modified because it's new
        lvitem.SubItems.Add(metadata.AlbumArtist)
        lvitem.SubItems.Add(metadata.Year)
        lvitem.SubItems.Add(metadata.Genre)
        lvitem.Tag = metadata

        If metadata.AlbumArtPath = "" Then
            lvitem.ImageKey = "-"
        Else
            lvitem.ImageKey = "+"
        End If
        Me.lvAlbumsList.Items.Add(lvitem)

        newAlbumNode.Tag = metadata
        newAlbumNode.Text = metadata.AlbumTitle

        'add the new items to the modified albums tree
        If modifiedAlbums IsNot Nothing Then
            modifiedAlbums.Nodes.Add(newAlbumNode)
        End If

        For Each item In Me.lvAlbumsList.Items
            item.selected = False
        Next
        lvitem.Selected = True
        lvitem.EnsureVisible()
    End Sub
    Private Sub btnClearAlbumItems_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        '***DISABLED*** 
        'not much use for this actually. the player shows album art only if the first song displayed in
        'the players albumlisting is contained in the album. If you delete the first song, then no art will appear
        If MsgBox("Delete all songs in this album?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            Exit Sub
        End If
        Me.lvAlbumItems.BeginUpdate()
        For Each item As ListViewItem In Me.lvAlbumItems.Items
            item.Selected = True
        Next
        lvAlbumItems_KeyDown(Nothing, New KeyEventArgs(Keys.Delete))
        Me.lvAlbumItems.EndUpdate()
    End Sub

    Private Sub lvAlbumItems_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvAlbumItems.ColumnClick
        'don't bother resorting items, the player makes it's own albums list anyways
        Exit Sub

        Dim lv As ListViewEx = CType(sender, ListView)
        If e.Column <> lvAlbumItems_lastColumnClicked Then
            lv.Sorting = SortOrder.Ascending
            lv.Columns(e.Column).Text = lv.Columns(e.Column).Text & " ^"
            If Not lvAlbumItems_lastColumnClicked = -1 Then
                'clean up the title of the previously clicked column
                lv.Columns(lvAlbumItems_lastColumnClicked).Text = lv.Columns(lvAlbumItems_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            End If
            lvAlbumItems_lastColumnClicked = e.Column
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

        'save the song order **DISABLED*** player ignores song order
        'setNewSongOrder(Me.lvAlbumItems.Tag, Me.lvAlbumItems.Items)
    End Sub
    Private Sub lvAlbumItems_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvAlbumItems.KeyDown
        'don't bother deleting items in the album. the player makes its own list anyways
        Exit Sub

        If e.KeyCode = Keys.Delete Then
            For Each song As ListViewItem In Me.lvAlbumItems.SelectedItems
                song.Remove()
                song = Nothing
            Next

            setNewSongOrder(Me.lvAlbumItems.Tag, Me.lvAlbumItems.Items)
        End If
    End Sub
    '*******************************************************************

    Private Sub pbAlbumArt_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles pbAlbumArt.DragDrop
        'check to see if files and folders are being dragged are from explorer
        Dim draggedFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        If draggedFiles IsNot Nothing Then
            If draggedFiles.Length <> 1 Then
                MsgBox("Album art must be a jpg file")
                Exit Sub
            End If

            If Me.lvAlbumItems.Tag Is Nothing Then
                MsgBox("Select an album first")
                Exit Sub
            End If

            If IO.File.Exists(draggedFiles(0)) Then
                Dim ext As String = IO.Path.GetExtension(draggedFiles(0))

                Try
                    Dim artfile As String = convertToJpeg(draggedFiles(0))
                    If artfile = "" Then
                        Throw New Exception("Error converting " & draggedFiles(0) & " to jpeg")
                    End If
                    Me.pbAlbumArt.Image = New Bitmap(artfile)
                    Me.lblAlbumArtFileSize.Text = Math.Round((New IO.FileInfo(artfile)).Length / 1024, 2).ToString("N1") & " KB"
                    Me.lblAlbumArtDimensions.Text = Me.pbAlbumArt.Image.Width.ToString & " x " & Me.pbAlbumArt.Image.Height.ToString
                    Me.pbAlbumArt.Tag = artfile
                Catch ex As Exception
                    MsgBox("Album art must be a jpg file" & vbCrLf & ex.Message & "," & ex.Source)
                    Trace.WriteLine("AlbumArt_DragDrop: error displaying album art - " & draggedFiles(0) & ". " & ex.Message & "," & ex.Source)
                    Me.pbAlbumArt.Image = My.Resources.NoDeviceIcon
                    Me.pbAlbumArt.Tag = ""
                    Exit Sub
                End Try

            Else
                MsgBox("Album art must be an image file")
                Exit Sub
            End If

            'set a textbox to modified so savealbummetadata will save the new cover image
            Me.lvAlbumItems.Tag.tag.tag.AlbumArtIsFromPlayer = False
            Me.lvAlbumItems.Tag.tag.tag.AlbumArtIsFromEmbedded = False
            saveAlbumMetadata(Me.lvAlbumItems.Tag)
        End If
    End Sub
    Private Sub pbAlbumArt_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles pbAlbumArt.DragEnter
        Me.Activate()
        e.Effect = e.AllowedEffect
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
        Me.tvFileManagementDeviceFolders.EndUpdate()
        Me.tvFileManagementDeviceFolders.BeginUpdate()

        'add the folders directly without the root 'storage media' node
        For Each node As TreeNode In fullFileListing.Nodes(0).Nodes
            If node.Text = "MUSIC" Or node.Text = "PICTURES" Or node.Text = "VIDEO" Then
                Me.tvFileManagementDeviceFolders.Nodes.Add(node.Clone)
            End If
        Next

        'if none of the folders were found, exit
        If Me.tvFileManagementDeviceFolders.Nodes.Count = 0 Then Exit Sub

        'show only folders to the treeview
        For Each node As TreeNode In Me.tvFileManagementDeviceFolders.Nodes
            Me.refreshFileTransfersDeviceFiles_helper(node)
        Next

        Me.tvFileManagementDeviceFolders.Sort()
        Me.tvFileManagementDeviceFolders.ExpandAll()
        Me.tvFileManagementDeviceFolders.SelectedNode = Me.tvFileManagementDeviceFolders.Nodes(0)

        Me.tvFileManagementDeviceFolders.EndUpdate()

        updateDeviceFreeSpace()

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
            Me.lvFileManagementDeviceFilesInFolder.EndUpdate()
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
    Private Sub CollapseChildrenToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuTVFileManagementCollapseChildren.Click
        Me.tvFileManagementDeviceFolders.BeginUpdate()
        Me.tvFileManagementDeviceFolders.SelectedNode.Collapse(False)
        Me.tvFileManagementDeviceFolders.EndUpdate()
    End Sub
    Private Sub ExpandChildrenToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuTVFileManagementExpandChildren.Click
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
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        'check to see if files and folders are being dragged from explorer
        Dim draggedFiles() As String
        draggedFiles = e.Data.GetData(DataFormats.FileDrop)
        If draggedFiles IsNot Nothing Then
            'worker thread to do the actual uploading. this prevents blocking of explorer
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

        'expand any subfolders that were uploaded
        Me.lvFileManagementDeviceFilesInFolder.Tag.expandall()

        updateDeviceFreeSpace()

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
            Dim filesystemEntries() As String = IO.Directory.GetFileSystemEntries(path)
            Array.Sort(filesystemEntries)
            For Each filesystementry As String In filesystemEntries
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
    Private Sub SortAscendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnulvFileManagementSortAscendingToolStripMenuItem.Click
        'disable the sorting on the listview so we can manually sort stuff without the listview automatically resorting
        If Not lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1 Then
            'clean up the title of the previously clicked column
            Me.lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text = Me.lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1
        End If

        listviewItemSortSelected(Me.lvFileManagementDeviceFilesInFolder, SortOrder.Descending)
    End Sub
    Private Sub SortDescendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnulvFileManagementSortDescendingToolStripMenuItem.Click
        'disable the sorting on the listview so we can manually sort stuff without the listview automatically resorting
        If Not lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1 Then
            'clean up the title of the previously clicked column
            Me.lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text = Me.lvFileManagementDeviceFilesInFolder.Columns(lvFileManagementDeviceFilesInFolder_lastColumnClicked).Text.Replace(" ^", "").Replace(" v", "")
            lvFileManagementDeviceFilesInFolder_lastColumnClicked = -1
        End If

        listviewItemSortSelected(Me.lvFileManagementDeviceFilesInFolder, SortOrder.Ascending)
    End Sub
    Private Sub DeleteToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvFileManagementDeleteToolStripMenuItem.Click
        lvFileManagementDeviceFilesInFolder_KeyDown(Nothing, New KeyEventArgs(Keys.Delete))
    End Sub
    Private Sub lvFileManagementDeviceFilesInFolder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvFileManagementDeviceFilesInFolder.KeyDown
        If e.KeyData = Keys.Delete Then

            If MsgBox("Are you sure? This will delete files recursively", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Really delete?") = MsgBoxResult.No Then Exit Sub

            Dim t As New Threading.Thread(New Threading.ThreadStart(AddressOf Splash.ShowDialog))
            t.SetApartmentState(Threading.ApartmentState.MTA)
            If Not Splash.Visible Then t.Start()
            Splash.setText("Deleting")
            Splash.setTitle("Deleting...")

            Trace.WriteLine("Deleting files...")

            'delete the selected item(s)
            Dim item As StorageItem
            For Each lvitem As ListViewItem In Me.lvFileManagementDeviceFilesInFolder.SelectedItems
                item = CType(lvitem.Tag, StorageItem)
                If item IsNot Nothing Then
                    Trace.WriteLine("Deleting " & lvitem.Text & "-" & item.ID & "...")
                    Splash.setText("Deleting " & lvitem.Text)
                    If Not axe.deleteFile(item.ID) = "-1" Then
                        'remove from tree if successful
                        Dim node As TreeNode
                        node = findTreeNodeByID(fullFileListing.Nodes(0), item.ID)
                        If node IsNot Nothing Then
                            node.Remove()
                            node = Nothing
                        End If
                    End If
                    Trace.WriteLine("Deleting " & lvitem.Text & "...Done")
                End If
                lvitem.Remove()
                lvitem = Nothing
            Next

            Trace.WriteLine("Deleting files...Done")

            t.Abort()

            refreshFileTransfersDeviceFiles()
            refreshPlaylistDeviceFiles()
        End If
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
        If Not DeviceConnected Then
            MessageBox.Show("No device selected. Please select a device from the list and then retry.", "Walkman MTP", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
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
    Private Function findTreeNodeByName(ByRef root As TreeNode, ByVal FileName As String) As TreeNode
        'searches for the first matching treenode with the given name
        Dim ret As TreeNode = Nothing
        Dim item As StorageItem

        item = CType(root.Tag, StorageItem)
        If item.FileName = FileName Then
            Return root
        End If

        For Each node As TreeNode In root.Nodes
            If node.Tag.FileName = FileName Then
                ret = node
            ElseIf node.Nodes.Count > 0 Then
                'else check the child nodes
                ret = findTreeNodeByName(node, FileName)
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
        If fullFileListing Is Nothing Then
            Trace.WriteLine("Refreshing full directory tree...error")

        End If

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
    Private Function getAlbumArtFromFile(ByVal filepath As String) As String
        'try to get the embedded album art. returns the path to a temp file containig the image
        '"" on error
        Try
            Trace.WriteLine("Getting embedded album art for " & filepath)
            Dim tagreader As TagLib.File = TagLib.File.Create(filepath)
            Dim tmpfile As String = ""
            If tagreader.Tag.Pictures.Length > 0 Then
                Try
                    'only non progressive jpg images are valid cover art
                    tmpfile = IO.Path.GetTempFileName
                    Dim file As New IO.FileStream(tmpfile, IO.FileMode.Truncate)
                    file.Write(tagreader.Tag.Pictures(0).Data.Data, 0, tagreader.Tag.Pictures(0).Data.Data.Length)
                    file.Close()
                    Return tmpfile
                Catch ex As Exception
                    Trace.WriteLine("File " & filepath & " had embedded album art but it was unable to be saved to " & tmpfile & " - " & ex.Message & "," & ex.Source)
                End Try
            Else
                Trace.WriteLine("No embedded album art in " & filepath)
            End If
        Catch ex As Exception
            Trace.WriteLine("taglib sharp error (wrong file type?):" & filepath & " Message=" & ex.Message)
        End Try

        Return ""
    End Function
    Private Function buildAlbumsListFromPaths(ByVal fileSystemEntries() As String) As TreeView
        'builds a flat list of albums starting from the possibly nested list of files or directories 
        'in the array fileSystemEntries()
        'the returned treeview will have 1 node for each album and each of those nodes will contain N 
        'subnodes which are the files themselves.  The treeview will have a directory depth of 1 (starting
        'from 0 for the root). empty directories will not be added

        
        Trace.WriteLine("buildAlbumsListFromDirectory...")

        Dim tv As New TreeView

        For Each path In fileSystemEntries
            Try
                If IO.Directory.Exists(path) Then
                    'path is a folder
                    Dim itemsInFolder() As String = IO.Directory.GetFileSystemEntries(path)
                    Array.Sort(itemsInFolder)

                    Dim subTV As TreeView
                    For Each itemInFolder As String In itemsInFolder
                        subTV = buildAlbumsListFromPaths(New String() {itemInFolder})
                        If subTV IsNot Nothing Then

                            Dim nodeToMergeMetadata As StorageItem
                            Dim nodeToMerge As TreeNode
                            For Each node As TreeNode In subTV.Nodes
                                nodeToMerge = node.Clone
                                nodeToMergeMetadata = CType(node.Tag, StorageItem)

                                'check if the album we're trying to merge already exists
                                Dim foundalbum As TreeNode = Nothing
                                Dim foundAlbumMetadata As StorageItem
                                For Each Album As TreeNode In tv.Nodes
                                    foundalbum = Album
                                    foundAlbumMetadata = CType(foundalbum.Tag, StorageItem)
                                    If foundAlbumMetadata.AlbumTitle = nodeToMergeMetadata.AlbumTitle Then
                                        'if the node we're trying to merge exits, compare the rest
                                        'of the tags to see if they need changing
                                        If foundAlbumMetadata.AlbumArtist <> nodeToMergeMetadata.AlbumArtist Then
                                            foundAlbumMetadata.AlbumArtist = "Various"
                                        End If
                                        If foundAlbumMetadata.Genre <> nodeToMergeMetadata.Genre Then
                                            foundAlbumMetadata.Genre = "Various"
                                        End If
                                        If foundAlbumMetadata.Year <> nodeToMergeMetadata.Year Then
                                            foundAlbumMetadata.Year = "Various"
                                        End If


                                        'determine if we need to change overwrite the existing album art
                                        'first check to see if the node that is already in the main tree has album art
                                        If foundAlbumMetadata.AlbumArtPath <> "" Then
                                            'if the already present album has album art, check if it's from an embedded image
                                            'if it is, we can't overwrite it. if it's not, then we can overwrite it
                                            'if the nodeToMerge has album art from an embedded source
                                            If foundAlbumMetadata.AlbumArtIsFromEmbedded Then
                                                'it's from an embedded source, can't touch it. delete the album art we're trying
                                                'to merge
                                                deleteTmpFile(nodeToMergeMetadata.AlbumArtPath)
                                                nodeToMergeMetadata.AlbumArtPath = ""
                                            Else
                                                If nodeToMergeMetadata.AlbumArtIsFromEmbedded Then
                                                    'the node to merge has embedded metadata but the original one doesn't therefore
                                                    'we must overwrite it
                                                    deleteTmpFile(foundAlbumMetadata.AlbumArtPath)
                                                    foundAlbumMetadata.AlbumArtPath = nodeToMergeMetadata.AlbumArtPath
                                                    foundAlbumMetadata.AlbumArtIsFromEmbedded = True
                                                End If
                                            End If
                                        Else
                                            'if it has no album art, use the album art of the node we're trying to merge (even if has none)
                                            foundAlbumMetadata.AlbumArtPath = nodeToMergeMetadata.AlbumArtPath
                                            foundAlbumMetadata.AlbumArtIsFromEmbedded = False
                                        End If

                                        'update the album size
                                        foundAlbumMetadata.Size += nodeToMergeMetadata.Size

                                        foundalbum.Tag = foundAlbumMetadata
                                        nodeToMerge = foundalbum.Clone

                                        'remove the original album from the tv tree
                                        foundalbum.Remove()
                                        foundalbum = Nothing

                                        'we can just copy over the items
                                        'contained in the node we want to merge
                                        For Each song As TreeNode In node.Nodes
                                            nodeToMerge.Nodes.Add(song)
                                        Next
                                    End If ' if album already exists
                                Next

                                tv.Nodes.Add(nodeToMerge)
                            Next
                            subTV.Nodes.Clear()
                            subTV = Nothing
                        End If
                    Next
                Else
                    'path is a file. will potentially be added to an album

                    'first get the file metadata
                    Dim fileMetadata As New StorageItem
                    Dim albumToInsertSongInto As TreeNode = Nothing
                    Dim albumMetadata As StorageItem = Nothing
                    Try
                        'note the file extension must be correct or taglib won't properly recognize the file
                        Dim tagReader As TagLib.File = TagLib.File.Create(path)
                        fileMetadata.Title = tagReader.Tag.Title
                        fileMetadata.AlbumTitle = tagReader.Tag.Album
                        fileMetadata.AlbumArtist = tagReader.Tag.FirstArtist
                        If fileMetadata.AlbumArtist = "" Then
                            fileMetadata.AlbumArtist = tagReader.Tag.FirstAlbumArtist
                            If fileMetadata.AlbumArtist = "" Then
                                fileMetadata.AlbumArtist = tagReader.Tag.FirstPerformer
                                If fileMetadata.AlbumArtist = "" Then
                                    fileMetadata.AlbumArtist = tagReader.Tag.FirstComposer
                                End If
                            End If
                        End If
                        fileMetadata.FileName = IO.Path.GetFileName(path)
                        fileMetadata.FilePath = path
                        fileMetadata.Genre = tagReader.Tag.FirstGenre
                        fileMetadata.TrackNum = tagReader.Tag.Track
                        fileMetadata.Year = tagReader.Tag.Year
                        fileMetadata.Size = (New IO.FileInfo(path)).Length

                        If fileMetadata.AlbumTitle = "" Then fileMetadata.AlbumTitle = "Unknown"
                        If fileMetadata.AlbumArtist = "" Then fileMetadata.AlbumArtist = "Unknown"
                        If fileMetadata.Year = "" Then fileMetadata.Year = "0"
                        If fileMetadata.Genre = "" Then fileMetadata.Genre = "Unknown"

                        'dont try to get the album art until we've checked if we actually need it

                        tagReader = Nothing
                    Catch ex As Exception
                        Throw New Exception("error getting tags (wrong file extension maybe?) " & ex.Message & "," & ex.Source)
                    End Try
                    'if we got past the above Try..Catch, the file was a valid song since taglib threw no exceptions

                    'try to find the correct album to put this track in
                    For Each node As TreeNode In tv.Nodes
                        albumMetadata = CType(node.Tag, StorageItem)
                        If albumMetadata IsNot Nothing Then
                            If albumMetadata.AlbumTitle = fileMetadata.AlbumTitle Then
                                'we've found the album
                                albumToInsertSongInto = node
                                Exit For
                            End If
                        End If
                    Next

                    'if we found the album, then insert the song
                    'if we didnt; then create the album and insert the song
                    If albumToInsertSongInto IsNot Nothing Then
                        'check the album tags. if they're different than the song tags, set the appropriate tags to "various"
                        If albumMetadata IsNot Nothing Then
                            If albumMetadata.AlbumArtist <> fileMetadata.AlbumArtist Then
                                albumMetadata.AlbumArtist = "Various"
                            End If
                            If albumMetadata.Genre <> fileMetadata.Genre Then
                                albumMetadata.Genre = "Various"
                            End If
                            If albumMetadata.Year <> fileMetadata.Year Then
                                albumMetadata.Year = "Various"
                            End If
                            albumToInsertSongInto.Tag = albumMetadata

                            'determine if we need to change overwrite the existing album art
                            'if there is no album art, then for sure use the one from this file, if available
                            If albumMetadata.AlbumArtPath = "" Then
                                albumMetadata.AlbumArtPath = convertToJpeg(getAlbumArtFromFile(path))
                                albumMetadata.AlbumArtIsFromEmbedded = True
                            Else
                                'if there is existing art, determine whether it was obtained from an embedded image.

                                If albumMetadata.AlbumArtIsFromEmbedded Then
                                    'the album art was obtained from an embedded imgage. nothing can overwrite it
                                Else
                                    'if the album art is not from an embedded file, it can be overwritten if this
                                    'file has embedded art.
                                    Dim tmppath = convertToJpeg(getAlbumArtFromFile(path))
                                    If tmppath <> "" Then
                                        'embedded album art was found. overwrite existing non-embedded art
                                        deleteTmpFile(albumMetadata.AlbumArtPath)
                                        albumMetadata.AlbumArtPath = tmppath
                                        albumMetadata.AlbumArtIsFromEmbedded = True
                                    End If
                                End If

                            End If
                        End If

                    Else
                        'album was not found so create a new album
                        albumToInsertSongInto = New TreeNode
                        albumMetadata = New StorageItem

                        albumMetadata.AlbumArtist = fileMetadata.AlbumArtist
                        albumMetadata.AlbumTitle = fileMetadata.AlbumTitle
                        albumMetadata.Genre = fileMetadata.Genre
                        albumMetadata.Year = fileMetadata.Year
                        'since this album is not yet on the player, we need a way to identify it
                        'so we can search the tree for it later
                        albumMetadata.ID = Now.Ticks.ToString & albumMetadata.AlbumTitle


                        'don't forget to delete this file afterwards when we no longer need it.
                        albumMetadata.AlbumArtPath = convertToJpeg(getAlbumArtFromFile(path))
                        'if there was no embedded album art, use the first jpg file in the same
                        'directory of the song (if there is one)
                        If albumMetadata.AlbumArtPath = "" Then
                            Dim parentDirectoryFileList() As String = IO.Directory.GetFiles(IO.Path.GetDirectoryName(path))
                            Array.Sort(parentDirectoryFileList)
                            For Each file In parentDirectoryFileList
                                Dim ext As String = IO.Path.GetExtension(file)
                                'BMP, GIF, EXIG, JPG, PNG And TIFF
                                If ext = ".jpg" Or ext = ".JPG" Or ext = ".JPEG" Or ext = ".jpeg" Or ext = ".jpe" Or ext = ".JPE" _
                                   Or ext = ".bmp" Or ext = ".BMP" Or ext = ".png" Or ext = ".PNG" Or ext = ".tif" Or ext = ".TIF" Or ext = ".TIFF" Or ext = ".tiff" Then
                                    albumMetadata.AlbumArtPath = convertToJpeg(file)
                                    albumMetadata.AlbumArtIsFromEmbedded = False
                                End If
                            Next
                        Else
                            albumMetadata.AlbumArtIsFromEmbedded = True
                        End If

                        albumToInsertSongInto.Tag = albumMetadata
                        albumToInsertSongInto.Text = albumMetadata.AlbumTitle

                        'add the album to the master list
                        tv.Nodes.Add(albumToInsertSongInto)
                    End If


                    'add the song to the album
                    Dim song As New TreeNode
                    song.Tag = fileMetadata
                    song.Text = fileMetadata.Title
                    albumToInsertSongInto.Tag.size += fileMetadata.Size
                    albumToInsertSongInto.Nodes.Add(song)


                End If 'path is a file or folder
            Catch ex As Exception
                Trace.WriteLine("buildAlbumsListFromDirectory error - " & ex.Message & "," & ex.Source)
            End Try
        Next

        Trace.WriteLine("buildAlbumsListFromDirectory...Done")

        Return tv
    End Function
    Private Sub deleteTmpFile(ByVal filename As String)
        If filename = "" Then Exit Sub
        Try
            If IO.Path.GetExtension(filename) = ".tmp" Then
                IO.File.Delete(filename)
            End If
        Catch ex As Exception
            Trace.WriteLine("delete file: could not delete file '" & filename)
        End Try
    End Sub
    Private Sub cleanUpAxeTmpFiles()
        For Each file As String In IO.Directory.GetFiles(IO.Path.GetTempPath)
            If IO.Path.GetFileName(file).StartsWith("Axe") Then
                deleteTmpFile(file)
            End If
        Next
    End Sub
    Private Sub createGroupsFromColumn(ByRef lv As ListView, ByVal column As Integer)
        'groups the items in the specified listview by the specified column
        'the groups will always be sorted ascending. the albums will be sorted ascending by year

        lv.BeginUpdate()

        'copy the items into an array
        Dim itemsArr(lv.Items.Count - 1)
        lv.Items.CopyTo(itemsArr, 0)

        'clear the listview once we've copied everything
        lv.Items.Clear()
        lv.Groups.Clear()

        'sort the listviewitems by album year, ascending
        Dim lvc As New ListViewItemComparer(2, SortOrder.Ascending)
        Array.Sort(itemsArr, lvc)

        'get a list of the groups in the listview
        Dim groups As New Collection
        For Each item As ListViewItem In itemsArr
            'if the group doesn't exist create it

            'first, check if the album hasn't been deleted from the listview
            '(the tag will be null in this case)
            If item.Tag IsNot Nothing Then
                Dim theGroup As ListViewGroup = Nothing
                Dim metadata As StorageItem = CType(item.Tag.tag, StorageItem)
                Try
                    theGroup = groups(item.SubItems(column).Text)
                Catch ex As Exception
                End Try
                If theGroup Is Nothing Then
                    theGroup = New ListViewGroup
                    theGroup.Name = item.SubItems(column).Text
                    theGroup.Header = item.SubItems(column).Text
                    groups.Add(theGroup, theGroup.Name)
                End If
            End If
        Next

        'now copy the list of groups into an array and sort the list of groups ascending
        Dim groupsArr(groups.Count - 1) As ListViewGroup
        Dim i As Integer
        For i = 0 To groups.Count - 1
            groupsArr(i) = groups(i + 1)
        Next
        Dim lvgc As New ListViewGroupComparer(SortOrder.Ascending)
        Array.Sort(groupsArr, lvgc)

        'add the groups (now in order) to the listview
        lv.Groups.AddRange(groupsArr)

        'add each item to the appropriate group
        For Each item As ListViewItem In itemsArr
            item.Group = lv.Groups(item.SubItems(column).Text)
            lv.Items.Add(item)
        Next

        'reset the column widths
        For Each columnheader As ColumnHeader In lv.Columns
            If columnheader.Tag <> "" Then
                columnheader.Width = columnheader.Tag
            Else
                columnheader.Width = 80
            End If
        Next
        'hide the column we're grouping by
        lv.Columns(column).Width = 0

        lv.EndUpdate()
    End Sub
    Private Function GetEncoderInfo(ByVal mimeType As String) As Imaging.ImageCodecInfo
        Dim j As Integer
        Dim encoders() As Imaging.ImageCodecInfo
        encoders = Imaging.ImageCodecInfo.GetImageEncoders()

        j = 0
        While j < encoders.Length
            If encoders(j).MimeType = mimeType Then
                Return encoders(j)
            End If
            j += 1
        End While
        Return Nothing

    End Function
    Private Function convertToJpeg(ByVal filepath As String) As String
        'converts  BMP, GIF, EXIG, JPG, PNG and TIFF to valid jpg files 
        'jpg files must NOT be progressive or else the player won't recognize them
        'this function makes them non progressive
        'returns the path to the converted file
        Dim jpgEncoderInfo As Imaging.ImageCodecInfo
        Dim jpgEncoderParameters As Imaging.EncoderParameters
        Dim jpg As Bitmap
        Dim jpgTn As Bitmap
        Try
            jpgEncoderParameters = New Imaging.EncoderParameters(1)
            jpgEncoderInfo = GetEncoderInfo("image/jpeg")
            jpgEncoderParameters.Param(0) = New Imaging.EncoderParameter(Imaging.Encoder.Quality, 80)
            jpg = New Bitmap(filepath)

            'generates a 200px width thumbnail, preserving the aspect ratio of the original

            Dim ratio As Single = jpg.Width / 200

            jpgTn = New Bitmap(200, CInt(jpg.Height / ratio))

            Dim g As Graphics = Graphics.FromImage(jpgTn)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBilinear
            g.DrawImage(jpg, New Rectangle(0, 0, jpgTn.Width, jpgTn.Height), 0, 0, jpg.Width, jpg.Height, GraphicsUnit.Pixel)
            g.Dispose()
            jpg.Dispose()
            jpg = Nothing

        Catch ex As Exception
            Trace.WriteLine("ConvertToJpeg: couldn't open " & filepath & " - " & ex.Message)
            Return ""
        End Try

        Try
            Dim tmpfile As String = IO.Path.GetTempFileName
            jpgTn.Save(tmpfile, jpgEncoderInfo, jpgEncoderParameters)
            jpgTn.Dispose()
            jpgTn = Nothing
            deleteTmpFile(filepath)
            Return tmpfile
        Catch ex As Exception
            Trace.WriteLine("ConvertToJpeg: couldn't save " & filepath & "-" & ex.Message)
            Return ""
        End Try

        Return ""
    End Function
    Delegate Sub updateDeviceFreeSpaceCallback()
    Private Sub updateDeviceFreeSpace(Optional ByVal sizeToAdd As Long = 0, Optional ByVal refresh As Boolean = True)
        'update the free space and capacity from the device only the
        'first time this function is called. for any subsequent calls
        'the free space will be calculated internally (unless a refresh
        'is forced)
        '
        'the optional parameter is a long (in bytes) representing a 
        'file that has been added but not yet uploaded. this number will 
        'be subtracted from the current free space. If sizeToAdd is negative
        'the amout will be added (indicating the file was deleted)

        If axe Is Nothing Then
            Trace.WriteLine("updateDeviceFreeSpace: mtpAxe is not started")
            Exit Sub
        End If

        If Me.lblCapacity.InvokeRequired Then
            Dim d As New updateDeviceFreeSpaceCallback(AddressOf updateDeviceFreeSpace)
            Try
                Me.lblCapacity.Invoke(d)
            Catch e As Exception
            End Try
        Else

            If (capacity = -1 And freeSpace = -1) Or refresh Then
                'queries mtpaxe and updates the free space and capacity
                Trace.WriteLine("updateDeviceFreeSpace: getting device capacity")
                Dim ret() As String
                ret = axe.getDeviceCapacity().Split(":"c)
                If ret(0) = "-1" Then
                    Trace.WriteLine("updateDeviceFreeSpace: error getting device capacity")
                    Me.lblCapacity.Text = "N/A"
                    Exit Sub
                End If

                capacity = Double.Parse(ret(0)) / 1024 / 1024 'to MB
                freeSpace = Double.Parse(ret(1)) / 1024 / 1024
            Else
                freeSpace += sizeToAdd / 1024 / 1024
            End If

            Me.lblCapacity.Text = freeSpace.ToString("N2") & " of " & capacity.ToString("N2") & " MB"

        End If

    End Sub
    Private Function checkDeviceSupported() As Boolean
        'checks whethre the currently selected is a supported device
        'return true on supported device, false on unsupported device
        '(thanks poggos)

        If axe Is Nothing Then
            Trace.WriteLine("initSelectedDevice: MTPAxe is not initialized")
            MsgBox("initSelectedDevice: MTPAxe is not initialized", MsgBoxStyle.Critical Or MsgBoxStyle.ApplicationModal)
            Exit Function
        End If

        Dim str As String = axe.getDeviceType
        Dim strarr As String(), isPlayer As Boolean = False
        If str <> "" Then
            strarr = str.Split(":"c)
            For i As Integer = 0 To strarr.Length - 1
                If strarr(i) = "WMDM_DEVICE_TYPE_PLAYBACK" Then
                    isPlayer = True
                    Exit For
                End If
            Next
            If isPlayer = False Then                
                Me.removeAllAlbumsFromAlbumsList(True)
                Me.deleteAllPlaylists()
                tvPlaylistsFilesOnDevice.Nodes.Clear()
                lvFileManagementDeviceFilesInFolder.Items.Clear()
                tvPlaylistsFilesOnDevice.Nodes.Clear()
                Return False
            End If
        Else            
            Me.removeAllAlbumsFromAlbumsList(True)
            Me.deleteAllPlaylists()
            lvFileManagementDeviceFilesInFolder.Items.Clear()
            tvPlaylistsFilesOnDevice.Nodes.Clear()
            Return False
        End If

        Return True
    End Function


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
    Private Class ListViewItemComparer
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


            subitem_x = item_x.SubItems(whichColumn)
            subitem_y = item_y.SubItems(whichColumn)
            ret = String.Compare(subitem_x.Text, subitem_y.Text, True)

            If whichSortOrder = SortOrder.Descending Then
                ' Invert the value returned by String.Compare.
                ret *= -1
            End If


            Return ret
        End Function
    End Class
    Private Class ListViewGroupComparer
        Implements IComparer

        Private order As SortOrder

        ' Stores the sort order.
        Public Sub New(ByVal theOrder As SortOrder)
            order = theOrder
        End Sub 'New

        ' Compares the groups by header value, using the saved sort
        ' order to return the correct value.
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
            Implements IComparer.Compare
            Dim result As Integer = String.Compare( _
                CType(x, ListViewGroup).Header, _
                CType(y, ListViewGroup).Header)
            If order = SortOrder.Ascending Then
                Return result
            Else
                Return -result
            End If
        End Function 'Compare
    End Class

#End Region

    Private Sub CleanUpEmptyFoldersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CleanUpEmptyFoldersToolStripMenuItem.Click
        Dim str As String = ""
        Dim musicFolder As TreeNode = Nothing
        For Each node As TreeNode In fullFileListing.Nodes
            musicFolder = findTreeNodeByName(node, "MUSIC")
        Next
        If musicFolder IsNot Nothing Then
            deleteEmptyFolders(musicFolder)
        End If

    End Sub
    Private Sub deleteEmptyFolders(ByVal tn As TreeNode)
        'deletes all empty folders and subfolders in the given folder 
        '(doesn't delete the topmost folder, even if it's empty)

        For Each node As TreeNode In tn.Nodes
            'check to see if the node is a folder
            If (node.Tag.StorageType And MTPAxe.WMDM_FILE_ATTR_FOLDER) = MTPAxe.WMDM_FILE_ATTR_FOLDER Then
                'if the specified subfolder is not empty, make a recursive call 
                If node.Nodes.Count <> 0 Then
                    'if it's not empty, check the sub folders (if any)
                    deleteEmptyFolders(node)
                End If

                'if, after returning from the recursive call, the folder is now empty,
                'delete it
                If node.Nodes.Count = 0 Then
                    Trace.WriteLine("delete node" & CType(node.Tag, StorageItem).FileName)
                End If
            End If
        Next
    End Sub
End Class
