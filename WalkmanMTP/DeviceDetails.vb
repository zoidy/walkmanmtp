Public Class DeviceDetails


    Private Sub DeviceDetails_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If Me.Visible Then
            If Main.axe Is Nothing Then
                MsgBox("MTPAxe is not running", MsgBoxStyle.Critical)
                Me.Close()
                Exit Sub
            End If

            Dim strarr() As String
            Dim str As String

            Try
                Me.txtAttrib.Clear()
                Me.txtFormats.Clear()
                Me.txtInfo.Clear()

                Me.txtDeviceManagerRev.Text = "0x" & Main.axe.getDeviceManagerRevision

                str = Main.axe.getDeviceAdditionalInfo
                If str <> "" Then
                    strarr = str.Split(":"c)
                    For i As Integer = 0 To strarr.Length - 2 Step 2
                        Me.txtInfo.AppendText(strarr(i) & " = " & strarr(i + 1) & vbCrLf)
                    Next
                End If

                str = Main.axe.getDeviceType
                If str <> "" Then
                    strarr = str.Split(":"c)
                    For i As Integer = 0 To strarr.Length - 1
                        Me.txtAttrib.AppendText(strarr(i) & vbCrLf)
                    Next
                End If

                str = Main.axe.getDeviceSupportedFormats
                If str <> "" Then
                    strarr = str.Split(":"c)
                    For i As Integer = 0 To strarr.Length - 1
                        Me.txtFormats.AppendText(strarr(i) & vbCrLf)
                    Next
                End If
            Catch ex As Exception
                Trace.WriteLine("Device Details: " & ex.Message & "," & ex.Source)
            End Try
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
        Me.Dispose()
    End Sub
End Class