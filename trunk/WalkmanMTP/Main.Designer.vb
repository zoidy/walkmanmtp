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
        Me.QuitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OptionsStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShowDeviceIconToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.ShowDebugWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.cmbDevices = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabMain = New System.Windows.Forms.TabControl
        Me.tabpagePlaylists = New System.Windows.Forms.TabPage
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.tvPlaylistsFilesOnDevice = New WalkmanMTP.MultiSelectTreeview.MultiSelectTreeview
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnPlaylistsFilesOnDeviceRefresh = New System.Windows.Forms.LinkLabel
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
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
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.Label6 = New System.Windows.Forms.Label
        Me.TreeView2 = New System.Windows.Forms.TreeView
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblManufacturer = New System.Windows.Forms.Label
        Me.lblCapacity = New System.Windows.Forms.Label
        Me.gbDevIcon = New System.Windows.Forms.GroupBox
        Me.pboxDevIcon = New System.Windows.Forms.PictureBox
        Me.btnRefreshDevices = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSync = New System.Windows.Forms.Button
        Me.MenuStrip1.SuspendLayout()
        Me.tabMain.SuspendLayout()
        Me.tabpagePlaylists.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.tabPlaylists.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.gbDevIcon.SuspendLayout()
        CType(Me.pboxDevIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.OptionsStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(843, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.QuitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'QuitToolStripMenuItem
        '
        Me.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem"
        Me.QuitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.QuitToolStripMenuItem.Size = New System.Drawing.Size(145, 22)
        Me.QuitToolStripMenuItem.Text = "&Quit"
        '
        'OptionsStripMenuItem
        '
        Me.OptionsStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowDeviceIconToolStripMenuItem, Me.ToolStripSeparator2, Me.ShowDebugWindowToolStripMenuItem})
        Me.OptionsStripMenuItem.Name = "OptionsStripMenuItem"
        Me.OptionsStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.OptionsStripMenuItem.Text = "&Options"
        '
        'ShowDeviceIconToolStripMenuItem
        '
        Me.ShowDeviceIconToolStripMenuItem.Checked = True
        Me.ShowDeviceIconToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ShowDeviceIconToolStripMenuItem.Name = "ShowDeviceIconToolStripMenuItem"
        Me.ShowDeviceIconToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ShowDeviceIconToolStripMenuItem.Text = "Show device Icon"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(221, 6)
        '
        'ShowDebugWindowToolStripMenuItem
        '
        Me.ShowDebugWindowToolStripMenuItem.CheckOnClick = True
        Me.ShowDebugWindowToolStripMenuItem.Name = "ShowDebugWindowToolStripMenuItem"
        Me.ShowDebugWindowToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.ShowDebugWindowToolStripMenuItem.Size = New System.Drawing.Size(224, 22)
        Me.ShowDebugWindowToolStripMenuItem.Text = "Show Debug Window"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(114, 22)
        Me.AboutToolStripMenuItem.Text = "&About"
        '
        'cmbDevices
        '
        Me.cmbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDevices.FormattingEnabled = True
        Me.cmbDevices.Location = New System.Drawing.Point(12, 44)
        Me.cmbDevices.Name = "cmbDevices"
        Me.cmbDevices.Size = New System.Drawing.Size(232, 21)
        Me.cmbDevices.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Select MTP Device:"
        '
        'tabMain
        '
        Me.tabMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabMain.Controls.Add(Me.tabpagePlaylists)
        Me.tabMain.Controls.Add(Me.TabPage2)
        Me.tabMain.Controls.Add(Me.TabPage3)
        Me.tabMain.Location = New System.Drawing.Point(12, 91)
        Me.tabMain.Name = "tabMain"
        Me.tabMain.SelectedIndex = 0
        Me.tabMain.Size = New System.Drawing.Size(825, 470)
        Me.tabMain.TabIndex = 4
        '
        'tabpagePlaylists
        '
        Me.tabpagePlaylists.Controls.Add(Me.SplitContainer1)
        Me.tabpagePlaylists.Location = New System.Drawing.Point(4, 22)
        Me.tabpagePlaylists.Name = "tabpagePlaylists"
        Me.tabpagePlaylists.Padding = New System.Windows.Forms.Padding(3)
        Me.tabpagePlaylists.Size = New System.Drawing.Size(817, 444)
        Me.tabpagePlaylists.TabIndex = 0
        Me.tabpagePlaylists.Text = "Playlists"
        Me.tabpagePlaylists.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
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
        Me.SplitContainer1.Size = New System.Drawing.Size(811, 438)
        Me.SplitContainer1.SplitterDistance = 351
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
        Me.tvPlaylistsFilesOnDevice.Size = New System.Drawing.Size(353, 418)
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
        Me.btnPlaylistsFilesOnDeviceRefresh.Location = New System.Drawing.Point(299, 0)
        Me.btnPlaylistsFilesOnDeviceRefresh.Name = "btnPlaylistsFilesOnDeviceRefresh"
        Me.btnPlaylistsFilesOnDeviceRefresh.Size = New System.Drawing.Size(44, 13)
        Me.btnPlaylistsFilesOnDeviceRefresh.TabIndex = 5
        Me.btnPlaylistsFilesOnDeviceRefresh.TabStop = True
        Me.btnPlaylistsFilesOnDeviceRefresh.Text = "Refresh"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Redo.png")
        Me.ImageList1.Images.SetKeyName(1, "Star.png")
        Me.ImageList1.Images.SetKeyName(2, "Plus.png")
        Me.ImageList1.Images.SetKeyName(3, "Minus.png")
        Me.ImageList1.Images.SetKeyName(4, "Text.png")
        Me.ImageList1.Images.SetKeyName(5, "Up.png")
        Me.ImageList1.Images.SetKeyName(6, "Down.png")
        Me.ImageList1.Images.SetKeyName(7, "Restart.png")
        Me.ImageList1.Images.SetKeyName(8, "Trash.png")
        '
        'btnRenamePlaylist
        '
        Me.btnRenamePlaylist.ImageIndex = 4
        Me.btnRenamePlaylist.ImageList = Me.ImageList1
        Me.btnRenamePlaylist.Location = New System.Drawing.Point(8, 78)
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
        Me.btnDeleteAllLists.Location = New System.Drawing.Point(361, 0)
        Me.btnDeleteAllLists.Name = "btnDeleteAllLists"
        Me.btnDeleteAllLists.Size = New System.Drawing.Size(92, 13)
        Me.btnDeleteAllLists.TabIndex = 1
        Me.btnDeleteAllLists.TabStop = True
        Me.btnDeleteAllLists.Text = "Delete All Playlists"
        Me.ToolTip1.SetToolTip(Me.btnDeleteAllLists, "Deletes ALL playlists")
        '
        'btnDelPlaylist
        '
        Me.btnDelPlaylist.ImageKey = "Trash.png"
        Me.btnDelPlaylist.ImageList = Me.ImageList1
        Me.btnDelPlaylist.Location = New System.Drawing.Point(8, 47)
        Me.btnDelPlaylist.Name = "btnDelPlaylist"
        Me.btnDelPlaylist.Size = New System.Drawing.Size(25, 25)
        Me.btnDelPlaylist.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.btnDelPlaylist, "Delete active playlist")
        Me.btnDelPlaylist.UseVisualStyleBackColor = True
        '
        'btnAddPlaylist
        '
        Me.btnAddPlaylist.ImageIndex = 7
        Me.btnAddPlaylist.ImageList = Me.ImageList1
        Me.btnAddPlaylist.Location = New System.Drawing.Point(8, 16)
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
        Me.tabPlaylists.Location = New System.Drawing.Point(39, 16)
        Me.tabPlaylists.Name = "tabPlaylists"
        Me.tabPlaylists.SelectedIndex = 0
        Me.tabPlaylists.Size = New System.Drawing.Size(418, 423)
        Me.tabPlaylists.TabIndex = 0
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.ListView1)
        Me.TabPage4.Location = New System.Drawing.Point(4, 21)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(410, 398)
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
        Me.ListView1.Size = New System.Drawing.Size(404, 392)
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
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(817, 444)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Album Art"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Label6)
        Me.TabPage3.Controls.Add(Me.TreeView2)
        Me.TabPage3.Controls.Add(Me.TreeView1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(817, 444)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Upload Files"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 17)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(179, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "Files To Upload (add or drag 'n drop)"
        '
        'TreeView2
        '
        Me.TreeView2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView2.Location = New System.Drawing.Point(352, 33)
        Me.TreeView2.Name = "TreeView2"
        Me.TreeView2.Size = New System.Drawing.Size(316, 351)
        Me.TreeView2.TabIndex = 0
        '
        'TreeView1
        '
        Me.TreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeView1.Location = New System.Drawing.Point(14, 33)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(300, 351)
        Me.TreeView1.TabIndex = 0
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(9, 68)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(76, 13)
        Me.LinkLabel1.TabIndex = 5
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Device Details"
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
        Me.Label3.Location = New System.Drawing.Point(297, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Capacity:"
        '
        'lblManufacturer
        '
        Me.lblManufacturer.AutoSize = True
        Me.lblManufacturer.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblManufacturer.Location = New System.Drawing.Point(390, 38)
        Me.lblManufacturer.Name = "lblManufacturer"
        Me.lblManufacturer.Size = New System.Drawing.Size(25, 13)
        Me.lblManufacturer.TabIndex = 7
        Me.lblManufacturer.Text = "N/A"
        '
        'lblCapacity
        '
        Me.lblCapacity.AutoSize = True
        Me.lblCapacity.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCapacity.Location = New System.Drawing.Point(390, 68)
        Me.lblCapacity.Name = "lblCapacity"
        Me.lblCapacity.Size = New System.Drawing.Size(25, 13)
        Me.lblCapacity.TabIndex = 7
        Me.lblCapacity.Text = "N/A"
        '
        'gbDevIcon
        '
        Me.gbDevIcon.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDevIcon.Controls.Add(Me.pboxDevIcon)
        Me.gbDevIcon.Location = New System.Drawing.Point(776, 28)
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
        Me.pboxDevIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pboxDevIcon.TabIndex = 4
        Me.pboxDevIcon.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pboxDevIcon, "Device logo")
        '
        'btnRefreshDevices
        '
        Me.btnRefreshDevices.ImageIndex = 0
        Me.btnRefreshDevices.ImageList = Me.ImageList1
        Me.btnRefreshDevices.Location = New System.Drawing.Point(256, 42)
        Me.btnRefreshDevices.Name = "btnRefreshDevices"
        Me.btnRefreshDevices.Size = New System.Drawing.Size(25, 25)
        Me.btnRefreshDevices.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.btnRefreshDevices, "Refresh device list")
        Me.btnRefreshDevices.UseVisualStyleBackColor = True
        '
        'btnSync
        '
        Me.btnSync.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSync.Location = New System.Drawing.Point(561, 37)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(195, 52)
        Me.btnSync.TabIndex = 10
        Me.btnSync.Text = "Sync"
        Me.ToolTip1.SetToolTip(Me.btnSync, "All non empty playlists and albums will be written to the player")
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(843, 573)
        Me.Controls.Add(Me.btnSync)
        Me.Controls.Add(Me.btnRefreshDevices)
        Me.Controls.Add(Me.gbDevIcon)
        Me.Controls.Add(Me.lblCapacity)
        Me.Controls.Add(Me.lblManufacturer)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.tabMain)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbDevices)
        Me.Controls.Add(Me.MenuStrip1)
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
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.gbDevIcon.ResumeLayout(False)
        CType(Me.pboxDevIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents QuitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmbDevices As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tabMain As System.Windows.Forms.TabControl
    Friend WithEvents tabpagePlaylists As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblManufacturer As System.Windows.Forms.Label
    Friend WithEvents lblCapacity As System.Windows.Forms.Label
    Friend WithEvents TreeView2 As System.Windows.Forms.TreeView
    Friend WithEvents gbDevIcon As System.Windows.Forms.GroupBox
    Friend WithEvents pboxDevIcon As System.Windows.Forms.PictureBox
    Friend WithEvents btnRefreshDevices As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
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
    Friend WithEvents ShowDeviceIconToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ShowDebugWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnRenamePlaylist As System.Windows.Forms.Button
    Friend WithEvents btnSync As System.Windows.Forms.Button
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
End Class
