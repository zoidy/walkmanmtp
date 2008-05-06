﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFileSyncDeviceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFileQuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OptionsStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOptionsShowDeviceIconToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuOptionsShowDebugWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpHelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpInformationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuHelpAboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.cmbDevices = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabpagePlaylists = New System.Windows.Forms.TabPage
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.tvPlaylistsFilesOnDevice = New WalkmanMTP.MultiSelectTreeview.MultiSelectTreeview
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnPlaylistsFilesOnDeviceRefresh = New System.Windows.Forms.LinkLabel
        Me.btnRenamePlaylist = New System.Windows.Forms.Button
        Me.btnDeleteAllLists = New System.Windows.Forms.LinkLabel
        Me.btnDelPlaylist = New System.Windows.Forms.Button
        Me.btnAddPlaylist = New System.Windows.Forms.Button
        Me.tabPlaylists = New System.Windows.Forms.TabControl
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.tabpageAlbums = New System.Windows.Forms.TabPage
        Me.Label6 = New System.Windows.Forms.Label
        Me.tabpageFileManagement = New System.Windows.Forms.TabPage
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.btnFileManagementRefresh = New System.Windows.Forms.LinkLabel
        Me.Label5 = New System.Windows.Forms.Label
        Me.tvFileManagementDeviceFolders = New System.Windows.Forms.TreeView
        Me.lvFileManagementDeviceFilesInFolder = New WalkmanMTP.ListViewDnD.ListViewEx
        Me.fileName = New System.Windows.Forms.ColumnHeader
        Me.Size = New System.Windows.Forms.ColumnHeader
        Me.Title = New System.Windows.Forms.ColumnHeader
        Me.Artist = New System.Windows.Forms.ColumnHeader
        Me.Album = New System.Windows.Forms.ColumnHeader
        Me.year = New System.Windows.Forms.ColumnHeader
        Me.trackNum = New System.Windows.Forms.ColumnHeader
        Me.genre = New System.Windows.Forms.ColumnHeader
        Me.lblFileManagementSelectedFolder = New System.Windows.Forms.Label
        Me.imlTabIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.btnDeviceDetails = New System.Windows.Forms.LinkLabel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblManufacturer = New System.Windows.Forms.Label
        Me.lblCapacity = New System.Windows.Forms.Label
        Me.gbDevIcon = New System.Windows.Forms.GroupBox
        Me.pboxDevIcon = New System.Windows.Forms.PictureBox
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSync = New System.Windows.Forms.Button
        Me.btnRefreshDevices = New System.Windows.Forms.Button
        Me.mnuLvPlaylistContentsRightClick = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.lvPlaylistContentsSelctionSortAscendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lvPlaylistContentsSelectionSortDescendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.lvPlaylistContentsDeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuLvFileManagementRightClick = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnulvFileManagementSortAscendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnulvFileManagementSortDescendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.lvFileManagementDeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTvPlaylistFilesRightClick = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnutvPlaylistFilesCollapseChildrenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnutvPlaylistFilesExpandChildrenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTvFileManagementRightClick = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuTVFileManagementCollapseChildren = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTVFileManagementExpandChildren = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabpagePlaylists.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabPlaylists.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.tabpageAlbums.SuspendLayout()
        Me.tabpageFileManagement.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.gbDevIcon.SuspendLayout()
        CType(Me.pboxDevIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuLvPlaylistContentsRightClick.SuspendLayout()
        Me.mnuLvFileManagementRightClick.SuspendLayout()
        Me.mnuTvPlaylistFilesRightClick.SuspendLayout()
        Me.mnuTvFileManagementRightClick.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.OptionsStripMenuItem, Me.mnuHelpHelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(917, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileSyncDeviceToolStripMenuItem, Me.ToolStripSeparator1, Me.mnuFileQuitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'mnuFileSyncDeviceToolStripMenuItem
        '
        Me.mnuFileSyncDeviceToolStripMenuItem.Name = "mnuFileSyncDeviceToolStripMenuItem"
        Me.mnuFileSyncDeviceToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mnuFileSyncDeviceToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.mnuFileSyncDeviceToolStripMenuItem.Text = "Sync Device"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(178, 6)
        '
        'mnuFileQuitToolStripMenuItem
        '
        Me.mnuFileQuitToolStripMenuItem.Name = "mnuFileQuitToolStripMenuItem"
        Me.mnuFileQuitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.mnuFileQuitToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.mnuFileQuitToolStripMenuItem.Text = "&Quit"
        '
        'OptionsStripMenuItem
        '
        Me.OptionsStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuOptionsShowDeviceIconToolStripMenuItem, Me.mnuOptionsShowDebugWindowToolStripMenuItem})
        Me.OptionsStripMenuItem.Name = "OptionsStripMenuItem"
        Me.OptionsStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.OptionsStripMenuItem.Text = "&Options"
        '
        'mnuOptionsShowDeviceIconToolStripMenuItem
        '
        Me.mnuOptionsShowDeviceIconToolStripMenuItem.Checked = True
        Me.mnuOptionsShowDeviceIconToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.mnuOptionsShowDeviceIconToolStripMenuItem.Name = "mnuOptionsShowDeviceIconToolStripMenuItem"
        Me.mnuOptionsShowDeviceIconToolStripMenuItem.Size = New System.Drawing.Size(228, 22)
        Me.mnuOptionsShowDeviceIconToolStripMenuItem.Text = "Show device Icon"
        '
        'mnuOptionsShowDebugWindowToolStripMenuItem
        '
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.CheckOnClick = True
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.Name = "mnuOptionsShowDebugWindowToolStripMenuItem"
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.Size = New System.Drawing.Size(228, 22)
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.Text = "Show Verbose Output"
        '
        'mnuHelpHelpToolStripMenuItem
        '
        Me.mnuHelpHelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpInformationToolStripMenuItem, Me.mnuHelpAboutToolStripMenuItem})
        Me.mnuHelpHelpToolStripMenuItem.Name = "mnuHelpHelpToolStripMenuItem"
        Me.mnuHelpHelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.mnuHelpHelpToolStripMenuItem.Text = "&Help"
        '
        'mnuHelpInformationToolStripMenuItem
        '
        Me.mnuHelpInformationToolStripMenuItem.Name = "mnuHelpInformationToolStripMenuItem"
        Me.mnuHelpInformationToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.mnuHelpInformationToolStripMenuItem.Text = "Information"
        '
        'mnuHelpAboutToolStripMenuItem
        '
        Me.mnuHelpAboutToolStripMenuItem.Name = "mnuHelpAboutToolStripMenuItem"
        Me.mnuHelpAboutToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.mnuHelpAboutToolStripMenuItem.Text = "&About"
        '
        'cmbDevices
        '
        Me.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDevices.FormattingEnabled = True
        Me.cmbDevices.Location = New System.Drawing.Point(15, 41)
        Me.cmbDevices.Name = "cmbDevices"
        Me.cmbDevices.Size = New System.Drawing.Size(232, 21)
        Me.cmbDevices.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Select MTP Device:"
        '
        'tabMain
        '
        Me.tabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMain.Controls.Add(Me.tabpagePlaylists)
        Me.tabMain.Controls.Add(Me.tabpageAlbums)
        Me.tabMain.Controls.Add(Me.tabpageFileManagement)
        Me.tabMain.ImageList = Me.imlTabIcons
        Me.tabMain.Location = New System.Drawing.Point(2, 81)
        Me.tabMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(914, 527)
        Me.tabMain.TabIndex = 4
        '
        'tabpagePlaylists
        '
        Me.tabpagePlaylists.Controls.Add(Me.SplitContainer1)
        Me.tabpagePlaylists.ImageKey = "Playlists"
        Me.tabpagePlaylists.Location = New System.Drawing.Point(4, 23)
        Me.tabpagePlaylists.Name = "tabpagePlaylists"
        Me.tabpagePlaylists.Size = New System.Drawing.Size(906, 500)
        Me.tabpagePlaylists.TabIndex = 0
        Me.tabpagePlaylists.Text = "Playlists"
        Me.tabpagePlaylists.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tvPlaylistsFilesOnDevice)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnPlaylistsFilesOnDeviceRefresh)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnRenamePlaylist)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnDeleteAllLists)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnDelPlaylist)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnAddPlaylist)
        Me.SplitContainer1.Panel2.Controls.Add(Me.tabPlaylists)
        Me.SplitContainer1.Size = New System.Drawing.Size(906, 500)
        Me.SplitContainer1.SplitterDistance = 364
        Me.SplitContainer1.SplitterWidth = 2
        Me.SplitContainer1.TabIndex = 0
        '
        'tvPlaylistsFilesOnDevice
        '
        Me.tvPlaylistsFilesOnDevice.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvPlaylistsFilesOnDevice.Location = New System.Drawing.Point(-3, 16)
        Me.tvPlaylistsFilesOnDevice.Name = "tvPlaylistsFilesOnDevice"
        Me.tvPlaylistsFilesOnDevice.SelectedNodes = CType(resources.GetObject("tvPlaylistsFilesOnDevice.SelectedNodes"), System.Collections.Generic.List(Of System.Windows.Forms.TreeNode))
        Me.tvPlaylistsFilesOnDevice.Size = New System.Drawing.Size(365, 481)
        Me.tvPlaylistsFilesOnDevice.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(1, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Files on Device:"
        '
        'btnPlaylistsFilesOnDeviceRefresh
        '
        Me.btnPlaylistsFilesOnDeviceRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPlaylistsFilesOnDeviceRefresh.AutoSize = True
        Me.btnPlaylistsFilesOnDeviceRefresh.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPlaylistsFilesOnDeviceRefresh.Location = New System.Drawing.Point(312, 0)
        Me.btnPlaylistsFilesOnDeviceRefresh.Name = "btnPlaylistsFilesOnDeviceRefresh"
        Me.btnPlaylistsFilesOnDeviceRefresh.Size = New System.Drawing.Size(45, 13)
        Me.btnPlaylistsFilesOnDeviceRefresh.TabIndex = 5
        Me.btnPlaylistsFilesOnDeviceRefresh.TabStop = True
        Me.btnPlaylistsFilesOnDeviceRefresh.Text = "Refresh"
        '
        'btnRenamePlaylist
        '
        Me.btnRenamePlaylist.Location = New System.Drawing.Point(4, 78)
        Me.btnRenamePlaylist.Name = "btnRenamePlaylist"
        Me.btnRenamePlaylist.Size = New System.Drawing.Size(25, 25)
        Me.btnRenamePlaylist.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.btnRenamePlaylist, "Rename Playlist")
        Me.btnRenamePlaylist.UseVisualStyleBackColor = True
        '
        'btnDeleteAllLists
        '
        Me.btnDeleteAllLists.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteAllLists.AutoSize = True
        Me.btnDeleteAllLists.Location = New System.Drawing.Point(445, 0)
        Me.btnDeleteAllLists.Name = "btnDeleteAllLists"
        Me.btnDeleteAllLists.Size = New System.Drawing.Size(93, 13)
        Me.btnDeleteAllLists.TabIndex = 1
        Me.btnDeleteAllLists.TabStop = True
        Me.btnDeleteAllLists.Text = "Delete All Playlists"
        Me.ToolTip1.SetToolTip(Me.btnDeleteAllLists, "Deletes ALL playlists")
        '
        'btnDelPlaylist
        '
        Me.btnDelPlaylist.Location = New System.Drawing.Point(4, 47)
        Me.btnDelPlaylist.Name = "btnDelPlaylist"
        Me.btnDelPlaylist.Size = New System.Drawing.Size(25, 25)
        Me.btnDelPlaylist.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.btnDelPlaylist, "Delete active playlist")
        Me.btnDelPlaylist.UseVisualStyleBackColor = True
        '
        'btnAddPlaylist
        '
        Me.btnAddPlaylist.Location = New System.Drawing.Point(4, 16)
        Me.btnAddPlaylist.Name = "btnAddPlaylist"
        Me.btnAddPlaylist.Size = New System.Drawing.Size(25, 25)
        Me.btnAddPlaylist.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.btnAddPlaylist, "Add New Playlist")
        Me.btnAddPlaylist.UseVisualStyleBackColor = True
        '
        'tabPlaylists
        '
        Me.tabPlaylists.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabPlaylists.Controls.Add(Me.TabPage4)
        Me.tabPlaylists.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabPlaylists.Location = New System.Drawing.Point(33, 16)
        Me.tabPlaylists.Name = "tabPlaylists"
        Me.tabPlaylists.SelectedIndex = 0
        Me.tabPlaylists.Size = New System.Drawing.Size(508, 485)
        Me.tabPlaylists.TabIndex = 0
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.ListView1)
        Me.TabPage4.Location = New System.Drawing.Point(4, 21)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(500, 460)
        Me.TabPage4.TabIndex = 0
        Me.TabPage4.Text = "New Playlist*"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.GridLines = True
        Me.ListView1.Location = New System.Drawing.Point(3, 3)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.ShowItemToolTips = True
        Me.ListView1.Size = New System.Drawing.Size(494, 454)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File Name"
        Me.ColumnHeader1.Width = 131
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Title"
        Me.ColumnHeader2.Width = 86
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Artist"
        Me.ColumnHeader3.Width = 79
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Album"
        Me.ColumnHeader4.Width = 88
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Year"
        '
        'tabpageAlbums
        '
        Me.tabpageAlbums.Controls.Add(Me.Label6)
        Me.tabpageAlbums.ImageKey = "Albums"
        Me.tabpageAlbums.Location = New System.Drawing.Point(4, 23)
        Me.tabpageAlbums.Name = "tabpageAlbums"
        Me.tabpageAlbums.Size = New System.Drawing.Size(906, 500)
        Me.tabpageAlbums.TabIndex = 3
        Me.tabpageAlbums.Text = "Albums"
        Me.tabpageAlbums.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(323, 167)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(106, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Not yet implemented"
        '
        'tabpageFileManagement
        '
        Me.tabpageFileManagement.Controls.Add(Me.SplitContainer2)
        Me.tabpageFileManagement.ImageKey = "Files"
        Me.tabpageFileManagement.Location = New System.Drawing.Point(4, 23)
        Me.tabpageFileManagement.Name = "tabpageFileManagement"
        Me.tabpageFileManagement.Size = New System.Drawing.Size(906, 500)
        Me.tabpageFileManagement.TabIndex = 2
        Me.tabpageFileManagement.Text = "File Management"
        Me.tabpageFileManagement.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        Me.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnFileManagementRefresh)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label5)
        Me.SplitContainer2.Panel1.Controls.Add(Me.tvFileManagementDeviceFolders)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.lvFileManagementDeviceFilesInFolder)
        Me.SplitContainer2.Panel2.Controls.Add(Me.lblFileManagementSelectedFolder)
        Me.SplitContainer2.Size = New System.Drawing.Size(906, 500)
        Me.SplitContainer2.SplitterDistance = 360
        Me.SplitContainer2.SplitterWidth = 2
        Me.SplitContainer2.TabIndex = 2
        '
        'btnFileManagementRefresh
        '
        Me.btnFileManagementRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFileManagementRefresh.AutoSize = True
        Me.btnFileManagementRefresh.Location = New System.Drawing.Point(309, 0)
        Me.btnFileManagementRefresh.Name = "btnFileManagementRefresh"
        Me.btnFileManagementRefresh.Size = New System.Drawing.Size(45, 13)
        Me.btnFileManagementRefresh.TabIndex = 9
        Me.btnFileManagementRefresh.TabStop = True
        Me.btnFileManagementRefresh.Text = "Refresh"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(1, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(42, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Folders"
        '
        'tvFileManagementDeviceFolders
        '
        Me.tvFileManagementDeviceFolders.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvFileManagementDeviceFolders.FullRowSelect = True
        Me.tvFileManagementDeviceFolders.HideSelection = False
        Me.tvFileManagementDeviceFolders.Location = New System.Drawing.Point(0, 16)
        Me.tvFileManagementDeviceFolders.Name = "tvFileManagementDeviceFolders"
        Me.tvFileManagementDeviceFolders.ShowNodeToolTips = True
        Me.tvFileManagementDeviceFolders.Size = New System.Drawing.Size(356, 480)
        Me.tvFileManagementDeviceFolders.TabIndex = 0
        '
        'lvFileManagementDeviceFilesInFolder
        '
        Me.lvFileManagementDeviceFilesInFolder.AllowDrop = True
        Me.lvFileManagementDeviceFilesInFolder.AllowReorder = True
        Me.lvFileManagementDeviceFilesInFolder.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvFileManagementDeviceFilesInFolder.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.fileName, Me.Size, Me.Title, Me.Artist, Me.Album, Me.year, Me.trackNum, Me.genre})
        Me.lvFileManagementDeviceFilesInFolder.FullRowSelect = True
        Me.lvFileManagementDeviceFilesInFolder.HideSelection = False
        Me.lvFileManagementDeviceFilesInFolder.LineColor = System.Drawing.Color.Red
        Me.lvFileManagementDeviceFilesInFolder.Location = New System.Drawing.Point(0, 16)
        Me.lvFileManagementDeviceFilesInFolder.Name = "lvFileManagementDeviceFilesInFolder"
        Me.lvFileManagementDeviceFilesInFolder.ShowItemToolTips = True
        Me.lvFileManagementDeviceFilesInFolder.Size = New System.Drawing.Size(540, 480)
        Me.lvFileManagementDeviceFilesInFolder.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.lvFileManagementDeviceFilesInFolder, "Drag files/folders to here")
        Me.lvFileManagementDeviceFilesInFolder.UseCompatibleStateImageBehavior = False
        Me.lvFileManagementDeviceFilesInFolder.View = System.Windows.Forms.View.Details
        '
        'fileName
        '
        Me.fileName.Text = "File Name"
        Me.fileName.Width = 247
        '
        'Size
        '
        Me.Size.Text = "Size"
        Me.Size.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.Size.Width = 85
        '
        'Title
        '
        Me.Title.Text = "Title"
        Me.Title.Width = 120
        '
        'Artist
        '
        Me.Artist.Text = "Artist"
        Me.Artist.Width = 100
        '
        'Album
        '
        Me.Album.Text = "Album"
        Me.Album.Width = 100
        '
        'year
        '
        Me.year.DisplayIndex = 7
        Me.year.Text = "Year"
        Me.year.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'trackNum
        '
        Me.trackNum.DisplayIndex = 5
        Me.trackNum.Text = "Track#"
        Me.trackNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.trackNum.Width = 30
        '
        'genre
        '
        Me.genre.DisplayIndex = 6
        Me.genre.Text = "Genre"
        Me.genre.Width = 80
        '
        'lblFileManagementSelectedFolder
        '
        Me.lblFileManagementSelectedFolder.AutoSize = True
        Me.lblFileManagementSelectedFolder.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileManagementSelectedFolder.Location = New System.Drawing.Point(3, 0)
        Me.lblFileManagementSelectedFolder.Name = "lblFileManagementSelectedFolder"
        Me.lblFileManagementSelectedFolder.Size = New System.Drawing.Size(0, 13)
        Me.lblFileManagementSelectedFolder.TabIndex = 8
        '
        'imlTabIcons
        '
        Me.imlTabIcons.ImageStream = CType(resources.GetObject("imlTabIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlTabIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.imlTabIcons.Images.SetKeyName(0, "Playlists")
        Me.imlTabIcons.Images.SetKeyName(1, "Albums")
        Me.imlTabIcons.Images.SetKeyName(2, "Files")
        '
        'btnDeviceDetails
        '
        Me.btnDeviceDetails.AutoSize = True
        Me.btnDeviceDetails.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeviceDetails.Location = New System.Drawing.Point(13, 65)
        Me.btnDeviceDetails.Name = "btnDeviceDetails"
        Me.btnDeviceDetails.Size = New System.Drawing.Size(65, 11)
        Me.btnDeviceDetails.TabIndex = 5
        Me.btnDeviceDetails.TabStop = True
        Me.btnDeviceDetails.Text = "Device Details"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(297, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Manufacturer:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(297, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Free Space:"
        '
        'lblManufacturer
        '
        Me.lblManufacturer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblManufacturer.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManufacturer.Location = New System.Drawing.Point(390, 38)
        Me.lblManufacturer.Name = "lblManufacturer"
        Me.lblManufacturer.Size = New System.Drawing.Size(240, 13)
        Me.lblManufacturer.TabIndex = 7
        Me.lblManufacturer.Text = "N/A"
        '
        'lblCapacity
        '
        Me.lblCapacity.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCapacity.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCapacity.Location = New System.Drawing.Point(390, 65)
        Me.lblCapacity.Name = "lblCapacity"
        Me.lblCapacity.Size = New System.Drawing.Size(240, 13)
        Me.lblCapacity.TabIndex = 7
        Me.lblCapacity.Text = "N/A"
        '
        'gbDevIcon
        '
        Me.gbDevIcon.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDevIcon.Controls.Add(Me.pboxDevIcon)
        Me.gbDevIcon.Location = New System.Drawing.Point(851, 22)
        Me.gbDevIcon.Name = "gbDevIcon"
        Me.gbDevIcon.Size = New System.Drawing.Size(61, 61)
        Me.gbDevIcon.TabIndex = 8
        Me.gbDevIcon.TabStop = False
        '
        'pboxDevIcon
        '
        Me.pboxDevIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.pboxDevIcon.Location = New System.Drawing.Point(6, 9)
        Me.pboxDevIcon.Name = "pboxDevIcon"
        Me.pboxDevIcon.Size = New System.Drawing.Size(48, 48)
        Me.pboxDevIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pboxDevIcon.TabIndex = 4
        Me.pboxDevIcon.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pboxDevIcon, "Device logo")
        '
        'btnSync
        '
        Me.btnSync.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSync.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSync.Location = New System.Drawing.Point(652, 34)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(195, 44)
        Me.btnSync.TabIndex = 10
        Me.btnSync.Text = "Sync"
        Me.btnSync.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.btnSync, "Sync [Ctrl+S]")
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'btnRefreshDevices
        '
        Me.btnRefreshDevices.Location = New System.Drawing.Point(259, 39)
        Me.btnRefreshDevices.Name = "btnRefreshDevices"
        Me.btnRefreshDevices.Size = New System.Drawing.Size(25, 25)
        Me.btnRefreshDevices.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.btnRefreshDevices, "Refresh device list")
        Me.btnRefreshDevices.UseVisualStyleBackColor = True
        '
        'mnuLvPlaylistContentsRightClick
        '
        Me.mnuLvPlaylistContentsRightClick.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lvPlaylistContentsSelctionSortAscendingToolStripMenuItem, Me.lvPlaylistContentsSelectionSortDescendingToolStripMenuItem, Me.ToolStripSeparator4, Me.lvPlaylistContentsDeleteToolStripMenuItem})
        Me.mnuLvPlaylistContentsRightClick.Name = "mnuLvPlaylistContents"
        Me.mnuLvPlaylistContentsRightClick.Size = New System.Drawing.Size(212, 76)
        '
        'lvPlaylistContentsSelctionSortAscendingToolStripMenuItem
        '
        Me.lvPlaylistContentsSelctionSortAscendingToolStripMenuItem.Name = "lvPlaylistContentsSelctionSortAscendingToolStripMenuItem"
        Me.lvPlaylistContentsSelctionSortAscendingToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.lvPlaylistContentsSelctionSortAscendingToolStripMenuItem.Text = "Sort Selected: Ascending"
        '
        'lvPlaylistContentsSelectionSortDescendingToolStripMenuItem
        '
        Me.lvPlaylistContentsSelectionSortDescendingToolStripMenuItem.Name = "lvPlaylistContentsSelectionSortDescendingToolStripMenuItem"
        Me.lvPlaylistContentsSelectionSortDescendingToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.lvPlaylistContentsSelectionSortDescendingToolStripMenuItem.Text = "Sort Selected: Descending"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(208, 6)
        '
        'lvPlaylistContentsDeleteToolStripMenuItem
        '
        Me.lvPlaylistContentsDeleteToolStripMenuItem.Name = "lvPlaylistContentsDeleteToolStripMenuItem"
        Me.lvPlaylistContentsDeleteToolStripMenuItem.ShortcutKeyDisplayString = ""
        Me.lvPlaylistContentsDeleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.lvPlaylistContentsDeleteToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.lvPlaylistContentsDeleteToolStripMenuItem.Text = "Delete"
        '
        'mnuLvFileManagementRightClick
        '
        Me.mnuLvFileManagementRightClick.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnulvFileManagementSortAscendingToolStripMenuItem, Me.mnulvFileManagementSortDescendingToolStripMenuItem, Me.ToolStripSeparator3, Me.lvFileManagementDeleteToolStripMenuItem})
        Me.mnuLvFileManagementRightClick.Name = "mnuLvFileManagementRightClick"
        Me.mnuLvFileManagementRightClick.Size = New System.Drawing.Size(212, 76)
        '
        'mnulvFileManagementSortAscendingToolStripMenuItem
        '
        Me.mnulvFileManagementSortAscendingToolStripMenuItem.Name = "mnulvFileManagementSortAscendingToolStripMenuItem"
        Me.mnulvFileManagementSortAscendingToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.mnulvFileManagementSortAscendingToolStripMenuItem.Text = "Sort Selected: Ascending"
        '
        'mnulvFileManagementSortDescendingToolStripMenuItem
        '
        Me.mnulvFileManagementSortDescendingToolStripMenuItem.Name = "mnulvFileManagementSortDescendingToolStripMenuItem"
        Me.mnulvFileManagementSortDescendingToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.mnulvFileManagementSortDescendingToolStripMenuItem.Text = "Sort Selected: Descending"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(208, 6)
        '
        'lvFileManagementDeleteToolStripMenuItem
        '
        Me.lvFileManagementDeleteToolStripMenuItem.Name = "lvFileManagementDeleteToolStripMenuItem"
        Me.lvFileManagementDeleteToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.lvFileManagementDeleteToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.lvFileManagementDeleteToolStripMenuItem.Text = "Delete"
        '
        'mnuTvPlaylistFilesRightClick
        '
        Me.mnuTvPlaylistFilesRightClick.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnutvPlaylistFilesCollapseChildrenToolStripMenuItem, Me.mnutvPlaylistFilesExpandChildrenToolStripMenuItem})
        Me.mnuTvPlaylistFilesRightClick.Name = "mnuTvPlaylistFilesRightClick"
        Me.mnuTvPlaylistFilesRightClick.Size = New System.Drawing.Size(168, 48)
        '
        'mnutvPlaylistFilesCollapseChildrenToolStripMenuItem
        '
        Me.mnutvPlaylistFilesCollapseChildrenToolStripMenuItem.Name = "mnutvPlaylistFilesCollapseChildrenToolStripMenuItem"
        Me.mnutvPlaylistFilesCollapseChildrenToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.mnutvPlaylistFilesCollapseChildrenToolStripMenuItem.Text = "Collapse Children"
        '
        'mnutvPlaylistFilesExpandChildrenToolStripMenuItem
        '
        Me.mnutvPlaylistFilesExpandChildrenToolStripMenuItem.Name = "mnutvPlaylistFilesExpandChildrenToolStripMenuItem"
        Me.mnutvPlaylistFilesExpandChildrenToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
        Me.mnutvPlaylistFilesExpandChildrenToolStripMenuItem.Text = "Expand Children"
        '
        'mnuTvFileManagementRightClick
        '
        Me.mnuTvFileManagementRightClick.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTVFileManagementCollapseChildren, Me.mnuTVFileManagementExpandChildren})
        Me.mnuTvFileManagementRightClick.Name = "mnuTvFileManagementRightClick"
        Me.mnuTvFileManagementRightClick.Size = New System.Drawing.Size(168, 70)
        '
        'mnuTVFileManagementCollapseChildren
        '
        Me.mnuTVFileManagementCollapseChildren.Name = "mnuTVFileManagementCollapseChildren"
        Me.mnuTVFileManagementCollapseChildren.Size = New System.Drawing.Size(167, 22)
        Me.mnuTVFileManagementCollapseChildren.Text = "Collapse Children"
        '
        'mnuTVFileManagementExpandChildren
        '
        Me.mnuTVFileManagementExpandChildren.Name = "mnuTVFileManagementExpandChildren"
        Me.mnuTVFileManagementExpandChildren.Size = New System.Drawing.Size(167, 22)
        Me.mnuTVFileManagementExpandChildren.Text = "Expand Children"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(917, 609)
        Me.Controls.Add(Me.btnRefreshDevices)
        Me.Controls.Add(Me.btnSync)
        Me.Controls.Add(Me.lblManufacturer)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblCapacity)
        Me.Controls.Add(Me.cmbDevices)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.gbDevIcon)
        Me.Controls.Add(Me.btnDeviceDetails)
        Me.Controls.Add(Me.tabMain)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Main"
        Me.Text = "WalkmanMTP"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.tabMain.ResumeLayout(False)
        Me.tabpagePlaylists.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.tabPlaylists.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.tabpageAlbums.ResumeLayout(False)
        Me.tabpageAlbums.PerformLayout()
        Me.tabpageFileManagement.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        Me.SplitContainer2.ResumeLayout(False)
        Me.gbDevIcon.ResumeLayout(False)
        CType(Me.pboxDevIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuLvPlaylistContentsRightClick.ResumeLayout(False)
        Me.mnuLvFileManagementRightClick.ResumeLayout(False)
        Me.mnuTvPlaylistFilesRightClick.ResumeLayout(False)
        Me.mnuTvFileManagementRightClick.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileQuitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpHelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpAboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbDevices As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabpagePlaylists As System.Windows.Forms.TabPage
    Friend WithEvents tabpageFileManagement As System.Windows.Forms.TabPage
    Friend WithEvents btnDeviceDetails As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblManufacturer As System.Windows.Forms.Label
    Friend WithEvents lblCapacity As System.Windows.Forms.Label
    Friend WithEvents gbDevIcon As System.Windows.Forms.GroupBox
    Friend WithEvents pboxDevIcon As System.Windows.Forms.PictureBox
    Friend WithEvents btnRefreshDevices As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tvPlaylistsFilesOnDevice As WalkmanMTP.MultiSelectTreeview.MultiSelectTreeview
    Friend WithEvents tabPlaylists As System.Windows.Forms.TabControl
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnPlaylistsFilesOnDeviceRefresh As System.Windows.Forms.LinkLabel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnDelPlaylist As System.Windows.Forms.Button
    Friend WithEvents btnAddPlaylist As System.Windows.Forms.Button
    Friend WithEvents btnDeleteAllLists As System.Windows.Forms.LinkLabel
    Friend WithEvents OptionsStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOptionsShowDeviceIconToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuOptionsShowDebugWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnRenamePlaylist As System.Windows.Forms.Button
    Friend WithEvents btnSync As System.Windows.Forms.Button
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents tvFileManagementDeviceFolders As System.Windows.Forms.TreeView
    Friend WithEvents lvFileManagementDeviceFilesInFolder As WalkmanMTP.ListViewDnD.ListViewEx
    Friend WithEvents fileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnFileManagementRefresh As System.Windows.Forms.LinkLabel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblFileManagementSelectedFolder As System.Windows.Forms.Label
    Friend WithEvents mnuHelpInformationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFileSyncDeviceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Size As System.Windows.Forms.ColumnHeader
    Friend WithEvents mnuLvPlaylistContentsRightClick As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents lvPlaylistContentsSelctionSortAscendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lvPlaylistContentsSelectionSortDescendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lvPlaylistContentsDeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuLvFileManagementRightClick As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnulvFileManagementSortAscendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnulvFileManagementSortDescendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTvPlaylistFilesRightClick As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnutvPlaylistFilesCollapseChildrenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnutvPlaylistFilesExpandChildrenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTvFileManagementRightClick As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuTVFileManagementCollapseChildren As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTVFileManagementExpandChildren As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Title As System.Windows.Forms.ColumnHeader
    Friend WithEvents Artist As System.Windows.Forms.ColumnHeader
    Friend WithEvents Album As System.Windows.Forms.ColumnHeader
    Friend WithEvents trackNum As System.Windows.Forms.ColumnHeader
    Friend WithEvents genre As System.Windows.Forms.ColumnHeader
    Friend WithEvents year As System.Windows.Forms.ColumnHeader
    Friend WithEvents tabpageAlbums As System.Windows.Forms.TabPage
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents imlTabIcons As System.Windows.Forms.ImageList
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents lvFileManagementDeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
End Class
