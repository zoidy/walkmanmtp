<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DeviceDetails
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DeviceDetails))
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtDeviceManagerRev = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txtAttrib = New System.Windows.Forms.TextBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtFormats = New System.Windows.Forms.TextBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.txtInfo = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(212, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Windows Media Device Manager Revision:"
        '
        'txtDeviceManagerRev
        '
        Me.txtDeviceManagerRev.Location = New System.Drawing.Point(230, 6)
        Me.txtDeviceManagerRev.Name = "txtDeviceManagerRev"
        Me.txtDeviceManagerRev.ReadOnly = True
        Me.txtDeviceManagerRev.Size = New System.Drawing.Size(234, 20)
        Me.txtDeviceManagerRev.TabIndex = 1
        Me.txtDeviceManagerRev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.txtDeviceManagerRev.WordWrap = False
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(364, 413)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(100, 29)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "&Ok"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtAttrib)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 160)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(449, 110)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Device Attributes"
        '
        'txtAttrib
        '
        Me.txtAttrib.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtAttrib.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAttrib.Location = New System.Drawing.Point(3, 16)
        Me.txtAttrib.Multiline = True
        Me.txtAttrib.Name = "txtAttrib"
        Me.txtAttrib.ReadOnly = True
        Me.txtAttrib.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAttrib.Size = New System.Drawing.Size(443, 91)
        Me.txtAttrib.TabIndex = 0
        Me.txtAttrib.Text = "test"
        Me.txtAttrib.WordWrap = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtFormats)
        Me.GroupBox2.Location = New System.Drawing.Point(14, 272)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(449, 136)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Device Supported Formats"
        '
        'txtFormats
        '
        Me.txtFormats.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtFormats.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormats.Location = New System.Drawing.Point(3, 16)
        Me.txtFormats.Multiline = True
        Me.txtFormats.Name = "txtFormats"
        Me.txtFormats.ReadOnly = True
        Me.txtFormats.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtFormats.Size = New System.Drawing.Size(443, 117)
        Me.txtFormats.TabIndex = 0
        Me.txtFormats.Text = "test"
        Me.txtFormats.WordWrap = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtInfo)
        Me.GroupBox3.Location = New System.Drawing.Point(15, 32)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(449, 127)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Device Information"
        '
        'txtInfo
        '
        Me.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtInfo.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfo.Location = New System.Drawing.Point(3, 16)
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ReadOnly = True
        Me.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal
        Me.txtInfo.Size = New System.Drawing.Size(443, 108)
        Me.txtInfo.TabIndex = 0
        Me.txtInfo.Text = "test"
        Me.txtInfo.WordWrap = False
        '
        'DeviceDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(476, 451)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtDeviceManagerRev)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "DeviceDetails"
        Me.Text = "Device Details"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtDeviceManagerRev As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtAttrib As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtFormats As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtInfo As System.Windows.Forms.TextBox
End Class
