'by arik poznanski www.codeproject.com/kb/trace/minememoryreader.aspx
'converted to vb.net 
Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Namespace MemoryReader
    ''' <summary>
    ''' ProcessMemoryReader is a class that enables direct reading a process memory
    ''' </summary>
    Class ProcessMemoryReaderApi
        ' constants information can be found in <winnt.h>
        Public Const PROCESS_VM_READ As UInteger = (16)

        ' function declarations are found in the MSDN and in <winbase.h> 

        '		HANDLE OpenProcess(
        '			DWORD dwDesiredAccess,  // access flag
        '			BOOL bInheritHandle,    // handle inheritance option
        '			DWORD dwProcessId       // process identifier
        '			);
        <DllImport("kernel32.dll")> _
        Public Shared Function OpenProcess(ByVal dwDesiredAccess As UInt32, ByVal bInheritHandle As Int32, ByVal dwProcessId As UInt32) As IntPtr
        End Function

        '		BOOL CloseHandle(
        '			HANDLE hObject   // handle to object
        '			);
        <DllImport("kernel32.dll")> _
        Public Shared Function CloseHandle(ByVal hObject As IntPtr) As Int32
        End Function

        '		BOOL ReadProcessMemory(
        '			HANDLE hProcess,              // handle to the process
        '			LPCVOID lpBaseAddress,        // base of memory area
        '			LPVOID lpBuffer,              // data buffer
        '			SIZE_T nSize,                 // number of bytes to read
        '			SIZE_T * lpNumberOfBytesRead  // number of bytes read
        '			);
        <DllImport("kernel32.dll")> _
        Public Shared Function ReadProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, <[In](), Out()> _
ByVal buffer As Byte(), ByVal size As UInt32, ByRef lpNumberOfBytesRead As IntPtr) As Int32
        End Function
    End Class

    Public Class ProcessMemoryReader

        Public Sub New()
        End Sub

        ''' <summary>	
        ''' Process from which to read		
        ''' </summary>
        Public Property ReadProcess() As Process
            Get
                Return m_ReadProcess
            End Get
            Set(ByVal value As Process)
                m_ReadProcess = value
            End Set
        End Property

        Private m_ReadProcess As Process = Nothing

        Private m_hProcess As IntPtr = IntPtr.Zero

        Public Sub OpenProcess()
            m_hProcess = ProcessMemoryReaderApi.OpenProcess(ProcessMemoryReaderApi.PROCESS_VM_READ, 1, CInt(m_ReadProcess.Id))
        End Sub

        Public Sub CloseHandle()
            Dim iRetValue As Integer
            iRetValue = ProcessMemoryReaderApi.CloseHandle(m_hProcess)
            If iRetValue = 0 Then
                Throw New Exception("CloseHandle failed")
            End If
        End Sub

        Public Function ReadProcessMemory(ByVal MemoryAddress As IntPtr, ByVal bytesToRead As UInteger, ByRef bytesReaded As Integer) As Byte()
            Dim buffer As Byte() = New Byte(bytesToRead - 1) {}

            Dim ptrBytesReaded As IntPtr
            ProcessMemoryReaderApi.ReadProcessMemory(m_hProcess, MemoryAddress, buffer, bytesToRead, ptrBytesReaded)

            bytesReaded = ptrBytesReaded.ToInt32()

            Return buffer
        End Function
    End Class
End Namespace
