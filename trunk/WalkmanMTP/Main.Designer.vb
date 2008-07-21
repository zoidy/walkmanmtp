<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.tabviewMain = New System.Windows.Forms.TabControl
        Me.tabpageAlbums = New System.Windows.Forms.TabPage
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer
        Me.cmbAlbumListGroupBy = New System.Windows.Forms.ComboBox
        Me.btnClearAllAlbums = New System.Windows.Forms.LinkLabel
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lvAlbumsList = New System.Windows.Forms.ListView
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.chkDeleteSongsOnAlbumDelete = New System.Windows.Forms.CheckBox
        Me.btnDeleteAlbum = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblAlbumNumberOfTracks = New System.Windows.Forms.Label
        Me.lblAlbumTotalSize = New System.Windows.Forms.Label
        Me.txtAlbumYear = New System.Windows.Forms.TextBox
        Me.txtAlbumGenre = New System.Windows.Forms.TextBox
        Me.txtAlbumArtist = New System.Windows.Forms.TextBox
        Me.txtAlbumTitle = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.pbAlbumArt = New System.Windows.Forms.PictureBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.lblAlbumArtFileSize = New System.Windows.Forms.Label
        Me.lblAlbumArtDimensions = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.tabpagePlaylists = New System.Windows.Forms.TabPage
        Me.tabpageFileManagement = New System.Windows.Forms.TabPage
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.btnFileManagementRefresh = New System.Windows.Forms.LinkLabel
        Me.Label5 = New System.Windows.Forms.Label
        Me.tvFileManagementDeviceFolders = New System.Windows.Forms.TreeView
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
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
        Me.lvAlbumItems = New WalkmanMTP.ListViewDnD.ListViewEx
        Me.lvAlbumItemsFileName = New System.Windows.Forms.ColumnHeader
        Me.lvAlbumItemsTrackNum = New System.Windows.Forms.ColumnHeader
        Me.lvAlbumItemsTitle = New System.Windows.Forms.ColumnHeader
        Me.lvAlbumItemsArtist = New System.Windows.Forms.ColumnHeader
        Me.lvAlbumItemsYear = New System.Windows.Forms.ColumnHeader
        Me.lvAlbumItemsGenre = New System.Windows.Forms.ColumnHeader
        Me.lvAlbumItemsSize = New System.Windows.Forms.ColumnHeader
        Me.tvPlaylistsFilesOnDevice = New WalkmanMTP.MultiSelectTreeview.MultiSelectTreeview
        Me.lvFileManagementDeviceFilesInFolder = New WalkmanMTP.ListViewDnD.ListViewEx
        Me.fileName = New System.Windows.Forms.ColumnHeader
        Me.Size = New System.Windows.Forms.ColumnHeader
        Me.Title = New System.Windows.Forms.ColumnHeader
        Me.Artist = New System.Windows.Forms.ColumnHeader
        Me.Album = New System.Windows.Forms.ColumnHeader
        Me.year = New System.Windows.Forms.ColumnHeader
        Me.trackNum = New System.Windows.Forms.ColumnHeader
        Me.genre = New System.Windows.Forms.ColumnHeader
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CleanUpEmptyFoldersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1.SuspendLayout()
        Me.tabviewMain.SuspendLayout()
        Me.tabpageAlbums.SuspendLayout()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.pbAlbumArt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabpagePlaylists.SuspendLayout()
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
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabPlaylists.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.OptionsStripMenuItem, Me.ToolsToolStripMenuItem, Me.mnuHelpHelpToolStripMenuItem})
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
        Me.mnuOptionsShowDeviceIconToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.mnuOptionsShowDeviceIconToolStripMenuItem.Text = "Show device Icon"
        '
        'mnuOptionsShowDebugWindowToolStripMenuItem
        '
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.CheckOnClick = True
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.Name = "mnuOptionsShowDebugWindowToolStripMenuItem"
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.mnuOptionsShowDebugWindowToolStripMenuItem.Text = "Show Log"
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
        Me.mnuHelpInformationToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.mnuHelpInformationToolStripMenuItem.Text = "Information"
        '
        'mnuHelpAboutToolStripMenuItem
        '
        Me.mnuHelpAboutToolStripMenuItem.Name = "mnuHelpAboutToolStripMenuItem"
        Me.mnuHelpAboutToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
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
        'tabviewMain
        '
        Me.tabviewMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabviewMain.Controls.Add(Me.tabpageAlbums)
        Me.tabviewMain.Controls.Add(Me.tabpagePlaylists)
        Me.tabviewMain.Controls.Add(Me.tabpageFileManagement)
        Me.tabviewMain.ImageList = Me.imlTabIcons
        Me.tabviewMain.Location = New System.Drawing.Point(2, 81)
        Me.tabviewMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tabviewMain.Name = "tabviewMain"
        Me.tabviewMain.SelectedIndex = 0
        Me.tabviewMain.Size = New System.Drawing.Size(914, 527)
        Me.tabviewMain.TabIndex = 4
        '
        'tabpageAlbums
        '
        Me.tabpageAlbums.Controls.Add(Me.SplitContainer3)
        Me.tabpageAlbums.Controls.Add(Me.Label6)
        Me.tabpageAlbums.ImageKey = "Albums"
        Me.tabpageAlbums.Location = New System.Drawing.Point(4, 23)
        Me.tabpageAlbums.Name = "tabpageAlbums"
        Me.tabpageAlbums.Size = New System.Drawing.Size(906, 500)
        Me.tabpageAlbums.TabIndex = 3
        Me.tabpageAlbums.Text = "Albums"
        Me.tabpageAlbums.UseVisualStyleBackColor = True
        '
        'SplitContainer3
        '
        Me.SplitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.cmbAlbumListGroupBy)
        Me.SplitContainer3.Panel1.Controls.Add(Me.btnClearAllAlbums)
        Me.SplitContainer3.Panel1.Controls.Add(Me.Label14)
        Me.SplitContainer3.Panel1.Controls.Add(Me.Label7)
        Me.SplitContainer3.Panel1.Controls.Add(Me.lvAlbumsList)
        Me.SplitContainer3.Panel1.Controls.Add(Me.chkDeleteSongsOnAlbumDelete)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.btnDeleteAlbum)
        Me.SplitContainer3.Panel2.Controls.Add(Me.GroupBox2)
        Me.SplitContainer3.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer3.Size = New System.Drawing.Size(906, 500)
        Me.SplitContainer3.SplitterDistance = 419
        Me.SplitContainer3.SplitterWidth = 2
        Me.SplitContainer3.TabIndex = 9
        '
        'cmbAlbumListGroupBy
        '
        Me.cmbAlbumListGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbAlbumListGroupBy.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAlbumListGroupBy.FormattingEnabled = True
        Me.cmbAlbumListGroupBy.Items.AddRange(New Object() {"Artist", "Year", "Genre"})
        Me.cmbAlbumListGroupBy.Location = New System.Drawing.Point(139, 2)
        Me.cmbAlbumListGroupBy.Name = "cmbAlbumListGroupBy"
        Me.cmbAlbumListGroupBy.Size = New System.Drawing.Size(121, 19)
        Me.cmbAlbumListGroupBy.TabIndex = 10
        '
        'btnClearAllAlbums
        '
        Me.btnClearAllAlbums.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearAllAlbums.AutoSize = True
        Me.btnClearAllAlbums.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearAllAlbums.Location = New System.Drawing.Point(362, -2)
        Me.btnClearAllAlbums.Name = "btnClearAllAlbums"
        Me.btnClearAllAlbums.Size = New System.Drawing.Size(52, 24)
        Me.btnClearAllAlbums.TabIndex = 1
        Me.btnClearAllAlbums.TabStop = True
        Me.btnClearAllAlbums.Text = "Delete" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "All Albums"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(85, 5)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(48, 11)
        Me.Label14.TabIndex = 8
        Me.Label14.Text = "Group By:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(1, 3)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(45, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "Albums:"
        '
        'lvAlbumsList
        '
        Me.lvAlbumsList.AllowDrop = True
        Me.lvAlbumsList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAlbumsList.AutoArrange = False
        Me.lvAlbumsList.CausesValidation = False
        Me.lvAlbumsList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9})
        Me.lvAlbumsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvAlbumsList.HideSelection = False
        Me.lvAlbumsList.LabelWrap = False
        Me.lvAlbumsList.Location = New System.Drawing.Point(-2, 23)
        Me.lvAlbumsList.Name = "lvAlbumsList"
        Me.lvAlbumsList.ShowItemToolTips = True
        Me.lvAlbumsList.Size = New System.Drawing.Size(419, 475)
        Me.lvAlbumsList.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.lvAlbumsList, "Drag Folders to here. Each folder will be considered as a separate album")
        Me.lvAlbumsList.UseCompatibleStateImageBehavior = False
        Me.lvAlbumsList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Tag = "230"
        Me.ColumnHeader6.Text = "Title"
        Me.ColumnHeader6.Width = 230
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Tag = "110"
        Me.ColumnHeader7.Text = "Artist"
        Me.ColumnHeader7.Width = 110
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Tag = "40"
        Me.ColumnHeader8.Text = "Year"
        Me.ColumnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader8.Width = 40
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Tag = "120"
        Me.ColumnHeader9.Text = "Genre"
        Me.ColumnHeader9.Width = 120
        '
        'chkDeleteSongsOnAlbumDelete
        '
        Me.chkDeleteSongsOnAlbumDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkDeleteSongsOnAlbumDelete.AutoSize = True
        Me.chkDeleteSongsOnAlbumDelete.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDeleteSongsOnAlbumDelete.Location = New System.Drawing.Point(269, -2)
        Me.chkDeleteSongsOnAlbumDelete.Name = "chkDeleteSongsOnAlbumDelete"
        Me.chkDeleteSongsOnAlbumDelete.Size = New System.Drawing.Size(92, 26)
        Me.chkDeleteSongsOnAlbumDelete.TabIndex = 9
        Me.chkDeleteSongsOnAlbumDelete.Text = "Delete songs" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "on Album delete"
        Me.chkDeleteSongsOnAlbumDelete.UseVisualStyleBackColor = True
        '
        'btnDeleteAlbum
        '
        Me.btnDeleteAlbum.Location = New System.Drawing.Point(9, 10)
        Me.btnDeleteAlbum.Name = "btnDeleteAlbum"
        Me.btnDeleteAlbum.Size = New System.Drawing.Size(25, 25)
        Me.btnDeleteAlbum.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.btnDeleteAlbum, "Delete this album")
        Me.btnDeleteAlbum.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.lvAlbumItems)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 144)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(477, 351)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Album Items"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblAlbumNumberOfTracks)
        Me.GroupBox1.Controls.Add(Me.lblAlbumTotalSize)
        Me.GroupBox1.Controls.Add(Me.txtAlbumYear)
        Me.GroupBox1.Controls.Add(Me.txtAlbumGenre)
        Me.GroupBox1.Controls.Add(Me.txtAlbumArtist)
        Me.GroupBox1.Controls.Add(Me.txtAlbumTitle)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.lblAlbumArtFileSize)
        Me.GroupBox1.Controls.Add(Me.lblAlbumArtDimensions)
        Me.GroupBox1.Location = New System.Drawing.Point(44, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(434, 135)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Album Information"
        '
        'lblAlbumNumberOfTracks
        '
        Me.lblAlbumNumberOfTracks.AutoSize = True
        Me.lblAlbumNumberOfTracks.Location = New System.Drawing.Point(115, 112)
        Me.lblAlbumNumberOfTracks.Name = "lblAlbumNumberOfTracks"
        Me.lblAlbumNumberOfTracks.Size = New System.Drawing.Size(19, 13)
        Me.lblAlbumNumberOfTracks.TabIndex = 9
        Me.lblAlbumNumberOfTracks.Text = "00"
        '
        'lblAlbumTotalSize
        '
        Me.lblAlbumTotalSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAlbumTotalSize.AutoSize = True
        Me.lblAlbumTotalSize.Location = New System.Drawing.Point(277, 112)
        Me.lblAlbumTotalSize.Name = "lblAlbumTotalSize"
        Me.lblAlbumTotalSize.Size = New System.Drawing.Size(30, 13)
        Me.lblAlbumTotalSize.TabIndex = 9
        Me.lblAlbumTotalSize.Text = "0 MB"
        '
        'txtAlbumYear
        '
        Me.txtAlbumYear.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlbumYear.Location = New System.Drawing.Point(56, 84)
        Me.txtAlbumYear.Name = "txtAlbumYear"
        Me.txtAlbumYear.ReadOnly = True
        Me.txtAlbumYear.Size = New System.Drawing.Size(266, 21)
        Me.txtAlbumYear.TabIndex = 7
        '
        'txtAlbumGenre
        '
        Me.txtAlbumGenre.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlbumGenre.Location = New System.Drawing.Point(56, 61)
        Me.txtAlbumGenre.Name = "txtAlbumGenre"
        Me.txtAlbumGenre.ReadOnly = True
        Me.txtAlbumGenre.Size = New System.Drawing.Size(266, 21)
        Me.txtAlbumGenre.TabIndex = 7
        '
        'txtAlbumArtist
        '
        Me.txtAlbumArtist.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlbumArtist.Location = New System.Drawing.Point(56, 39)
        Me.txtAlbumArtist.Name = "txtAlbumArtist"
        Me.txtAlbumArtist.ReadOnly = True
        Me.txtAlbumArtist.Size = New System.Drawing.Size(266, 21)
        Me.txtAlbumArtist.TabIndex = 7
        '
        'txtAlbumTitle
        '
        Me.txtAlbumTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlbumTitle.Location = New System.Drawing.Point(56, 16)
        Me.txtAlbumTitle.Name = "txtAlbumTitle"
        Me.txtAlbumTitle.ReadOnly = True
        Me.txtAlbumTitle.Size = New System.Drawing.Size(266, 21)
        Me.txtAlbumTitle.TabIndex = 7
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.pbAlbumArt)
        Me.GroupBox3.Location = New System.Drawing.Point(328, 9)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(100, 100)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        '
        'pbAlbumArt
        '
        Me.pbAlbumArt.Location = New System.Drawing.Point(4, 10)
        Me.pbAlbumArt.Name = "pbAlbumArt"
        Me.pbAlbumArt.Size = New System.Drawing.Size(91, 85)
        Me.pbAlbumArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbAlbumArt.TabIndex = 0
        Me.pbAlbumArt.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbAlbumArt, "Drag new album cover to here")
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(5, 112)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(109, 13)
        Me.Label15.TabIndex = 6
        Me.Label15.Text = "Number of Tracks:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(6, 64)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(44, 13)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "Genre:"
        '
        'Label13
        '
        Me.Label13.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(167, 112)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(104, 13)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "Total Album Size:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(6, 87)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(36, 13)
        Me.Label10.TabIndex = 6
        Me.Label10.Text = "Year:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(6, 42)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(42, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Artist:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(6, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 13)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Title:"
        '
        'lblAlbumArtFileSize
        '
        Me.lblAlbumArtFileSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAlbumArtFileSize.AutoSize = True
        Me.lblAlbumArtFileSize.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlbumArtFileSize.Location = New System.Drawing.Point(357, 121)
        Me.lblAlbumArtFileSize.Name = "lblAlbumArtFileSize"
        Me.lblAlbumArtFileSize.Size = New System.Drawing.Size(25, 11)
        Me.lblAlbumArtFileSize.TabIndex = 8
        Me.lblAlbumArtFileSize.Text = "0 KB"
        Me.lblAlbumArtFileSize.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAlbumArtDimensions
        '
        Me.lblAlbumArtDimensions.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAlbumArtDimensions.AutoSize = True
        Me.lblAlbumArtDimensions.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlbumArtDimensions.Location = New System.Drawing.Point(357, 110)
        Me.lblAlbumArtDimensions.Name = "lblAlbumArtDimensions"
        Me.lblAlbumArtDimensions.Size = New System.Drawing.Size(25, 11)
        Me.lblAlbumArtDimensions.TabIndex = 8
        Me.lblAlbumArtDimensions.Text = "0 x 0"
        Me.lblAlbumArtDimensions.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me.Label5.Size = New System.Drawing.Size(232, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Folders (no sync required for file management)"
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
        Me.mnuTvFileManagementRightClick.Size = New System.Drawing.Size(168, 48)
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
        'lvAlbumItems
        '
        Me.lvAlbumItems.AllowReorder = False
        Me.lvAlbumItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAlbumItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvAlbumItemsFileName, Me.lvAlbumItemsTrackNum, Me.lvAlbumItemsTitle, Me.lvAlbumItemsArtist, Me.lvAlbumItemsYear, Me.lvAlbumItemsGenre, Me.lvAlbumItemsSize})
        Me.lvAlbumItems.FullRowSelect = True
        Me.lvAlbumItems.GridLines = True
        Me.lvAlbumItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvAlbumItems.HideSelection = False
        Me.lvAlbumItems.LabelWrap = False
        Me.lvAlbumItems.LineColor = System.Drawing.Color.Red
        Me.lvAlbumItems.Location = New System.Drawing.Point(6, 14)
        Me.lvAlbumItems.Name = "lvAlbumItems"
        Me.lvAlbumItems.ShowGroups = False
        Me.lvAlbumItems.ShowItemToolTips = True
        Me.lvAlbumItems.Size = New System.Drawing.Size(465, 331)
        Me.lvAlbumItems.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.lvAlbumItems, "Drag songs to here")
        Me.lvAlbumItems.UseCompatibleStateImageBehavior = False
        Me.lvAlbumItems.View = System.Windows.Forms.View.Details
        '
        'lvAlbumItemsFileName
        '
        Me.lvAlbumItemsFileName.Text = "File Name"
        Me.lvAlbumItemsFileName.Width = 224
        '
        'lvAlbumItemsTrackNum
        '
        Me.lvAlbumItemsTrackNum.Text = "Track"
        Me.lvAlbumItemsTrackNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.lvAlbumItemsTrackNum.Width = 31
        '
        'lvAlbumItemsTitle
        '
        Me.lvAlbumItemsTitle.Text = "Title"
        Me.lvAlbumItemsTitle.Width = 208
        '
        'lvAlbumItemsArtist
        '
        Me.lvAlbumItemsArtist.Text = "Artist"
        Me.lvAlbumItemsArtist.Width = 130
        '
        'lvAlbumItemsYear
        '
        Me.lvAlbumItemsYear.Text = "Year"
        Me.lvAlbumItemsYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.lvAlbumItemsYear.Width = 47
        '
        'lvAlbumItemsGenre
        '
        Me.lvAlbumItemsGenre.Text = "Genre"
        Me.lvAlbumItemsGenre.Width = 121
        '
        'lvAlbumItemsSize
        '
        Me.lvAlbumItemsSize.Text = "Size"
        Me.lvAlbumItemsSize.Width = 80
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
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CleanUpEmptyFoldersToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ToolsToolStripMenuItem.Text = "&Tools"
        '
        'CleanUpEmptyFoldersToolStripMenuItem
        '
        Me.CleanUpEmptyFoldersToolStripMenuItem.Name = "CleanUpEmptyFoldersToolStripMenuItem"
        Me.CleanUpEmptyFoldersToolStripMenuItem.Size = New System.Drawing.Size(196, 22)
        Me.CleanUpEmptyFoldersToolStripMenuItem.Text = "&Clean up empty folders"
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
        Me.Controls.Add(Me.tabviewMain)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Main"
        Me.Text = "WalkmanMTP"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.tabviewMain.ResumeLayout(False)
        Me.tabpageAlbums.ResumeLayout(False)
        Me.tabpageAlbums.PerformLayout()
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.PerformLayout()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.pbAlbumArt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabpagePlaylists.ResumeLayout(False)
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
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.tabPlaylists.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
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
    Friend WithEvents tabviewMain As System.Windows.Forms.TabControl
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
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents lvAlbumsList As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkDeleteSongsOnAlbumDelete As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lvAlbumItems As WalkmanMTP.ListViewDnD.ListViewEx
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnClearAllAlbums As System.Windows.Forms.LinkLabel
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents pbAlbumArt As System.Windows.Forms.PictureBox
    Friend WithEvents btnDeleteAlbum As System.Windows.Forms.Button
    Friend WithEvents txtAlbumTitle As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblAlbumArtDimensions As System.Windows.Forms.Label
    Friend WithEvents txtAlbumGenre As System.Windows.Forms.TextBox
    Friend WithEvents txtAlbumArtist As System.Windows.Forms.TextBox
    Friend WithEvents lblAlbumTotalSize As System.Windows.Forms.Label
    Friend WithEvents txtAlbumYear As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents lblAlbumArtFileSize As System.Windows.Forms.Label
    Friend WithEvents lblAlbumNumberOfTracks As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents lvAlbumItemsFileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvAlbumItemsTitle As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvAlbumItemsArtist As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvAlbumItemsYear As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvAlbumItemsGenre As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvAlbumItemsTrackNum As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvAlbumItemsSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmbAlbumListGroupBy As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CleanUpEmptyFoldersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
