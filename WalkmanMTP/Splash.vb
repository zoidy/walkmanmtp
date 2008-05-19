Public Class Splash
    Public Sub setText(ByVal text As String)
        Try
            Control.CheckForIllegalCrossThreadCalls = False
            Me.txtText.Text = text
        Catch ex As Exception

        End Try
    End Sub
    Public Sub setTitle(ByVal txt As String)
        Try
            Form.CheckForIllegalCrossThreadCalls = False
            Me.Text = txt
        Catch ex As Exception

        End Try

    End Sub
    Public Sub initProgBar(ByVal maxVal As Integer)
        Try
            Me.ProgressBar1.Style = ProgressBarStyle.Continuous
            Me.ProgressBar1.Minimum = 0
            Me.ProgressBar1.Maximum = maxVal
        Catch ex As Exception

        End Try

    End Sub
    Public Sub resetProgBar()
        Try
            Me.ProgressBar1.Style = ProgressBarStyle.Marquee
        Catch ex As Exception

        End Try

    End Sub
    Public Sub incProgBar()
        Try
            Me.ProgressBar1.Value += 1
        Catch ex As Exception

        End Try

    End Sub

End Class