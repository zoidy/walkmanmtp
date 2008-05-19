'this class displays calls to trace.write and trace.writeline in a separate window
'this class is thread-safe
'
'oct 25/2007 v1.0
Public Class TraceOutput

    Private Class theTraceListener
        Inherits System.Diagnostics.TraceListener

        Private theTextBox As TextBox
        Private theForm As TraceOutput
        Private logFile As IO.StreamWriter

        Public Overrides ReadOnly Property IsThreadSafe() As Boolean
            Get
                Return True
            End Get
        End Property

        'keep track of the number of lines
        Private numLines As Integer

        'this is to enable thread safe accesses output text box.  This is necessary
        'because the trace can be called from other threads.
        Delegate Sub SetTextCallback(ByVal text As String)

        Public Overloads Overrides Sub Write(ByVal message As String)
            SetText(message)
        End Sub

        Public Overloads Overrides Sub WriteLine(ByVal message As String)
            SetText(message & vbCrLf)
        End Sub

        Public Sub setForm(ByVal f As TraceOutput)
            Me.theTextBox = f.TextBox1
            Me.theForm = f
        End Sub

        'enables thread safe accesses of the text box
        Private Sub SetText(ByVal text As String)
            'write the log
            If logFile Is Nothing Then
                Try
                    logFile = New IO.StreamWriter(IO.Path.Combine(IO.Path.GetTempPath, "WMTP.log"))
                    logFile.AutoFlush = True
                    logFile.WriteLine("Walkman MTP v" & Application.ProductVersion & " Log start: " & Now)
                    logFile.Write(Now & " " & text)
                Catch ex As Exception
                    'TODO: do nothing?
                End Try
            Else
                Try
                    logFile.Write(Now & " " & text)
                Catch ex As Exception
                    'TODO: do nothing?
                End Try

            End If

            'InvokeRequired required compares the thread ID of the
            'calling thread to the thread ID of the creating thread.
            'If these threads are different, it returns true. and invokes the 
            'delegate
            'If Not Me.theForm.Visible Then Exit Sub
            If Me.theTextBox.InvokeRequired Then
                Dim d As New SetTextCallback(AddressOf SetText)
                Try
                    Me.theForm.Invoke(d, New Object() {text})
                Catch e As Exception
                End Try
            Else

                If Not Me.theForm.chkAutoScroll.Checked Then
                    Try
                        Me.theTextBox.AppendText(text)
                    Catch ex As Exception
                    End Try
                End If

                numLines = numLines + 1

                'keep it from growing too big
                If numLines > 1000 Then
                    Me.theTextBox.ResetText()
                    numLines = 0
                End If
                Application.DoEvents()
            End If

        End Sub

    End Class

    Private myTraceListener As theTraceListener

    Private closeNow As Boolean = False

    Private Sub TraceOutput_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'If Not closeNow Then e.Cancel = True
    End Sub

    Private Sub traceoutput_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Trace.UseGlobalLock = False
        Trace.AutoFlush = True
        Me.myTraceListener = Trace.Listeners("myTraceListener")
        If Me.myTraceListener Is Nothing Then
            Me.myTraceListener = New theTraceListener()
            Me.myTraceListener.setForm(Me)
            Trace.Listeners(Trace.Listeners.Add(Me.myTraceListener)).Name = "myTraceListener"
        Else
            Me.myTraceListener.setForm(Me)
        End If
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Me.Close()\
        'minimize instead of close b/c sometimes, it causes the ppmdatacollector
        'thread to crash (tracelistener issue)
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Public Sub superClose()
        'this method must be called to close this window. else the program won't 
        'be allowed to close
        closeNow = True
        Me.Close()
    End Sub

    Protected Overrides Sub Finalize()
        Trace.Listeners.Remove(Me.myTraceListener)
        Me.myTraceListener.Dispose()
        MyBase.Finalize()
    End Sub
End Class

