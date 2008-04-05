Public Class Splash
    Public Sub setText(ByVal text As String)
        Control.CheckForIllegalCrossThreadCalls = False
        Me.txtText.Text = text
    End Sub
    Public Sub setTitle(ByVal txt As String)
        Form.CheckForIllegalCrossThreadCalls = False
        Me.Text = txt
    End Sub
    Public Sub initProgBar(ByVal maxVal As Integer)
        Me.ProgressBar1.Style = ProgressBarStyle.Continuous
        Me.ProgressBar1.Minimum = 0
        Me.ProgressBar1.Maximum = maxVal
    End Sub
    Public Sub resetProgBar()
        Me.ProgressBar1.Style = ProgressBarStyle.Marquee
    End Sub
    Public Sub incProgBar()
        Me.ProgressBar1.Value += 1
    End Sub
End Class