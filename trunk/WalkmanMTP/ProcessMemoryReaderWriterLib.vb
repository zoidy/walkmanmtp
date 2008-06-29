Imports System
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Namespace ProcessMemoryReaderWriterLib
    ''' <summary>
    ''' ProcessMemoryReader is a class that enables direct reading a process memory
    ''' </summary>
    Public Class ProcessMemoryReaderWriterApi
        ' constants information can be found in <winnt.h>
        Public Const PROCESS_VM_READ As UInteger = &H10I
        Public Const PROCESS_VM_WRITE As UInteger = &H20I
        Public Const PROCESS_VM_OPERATION As UInteger = &H8I

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
        <DllImport("kernel32.dll", setlasterror:=True)> _
        Public Shared Function ReadProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, <[In](), Out()> _
ByVal buffer As Byte(), ByVal size As UInt32, ByRef lpNumberOfBytesRead As IntPtr) As Int32
        End Function

        <DllImport("kernel32.dll", setlasterror:=True)> _
        Public Shared Function VirtualAllocEx(ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As UInteger, _
                                              ByVal flAllocationType As UInteger, ByVal flProtect As UInteger) As IntPtr
        End Function

        <DllImport("kernel32.dll", setlasterror:=True)> _
        Public Shared Function WriteProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer As Byte(), _
                                                  ByVal nSize As UIntPtr, <Out()> ByRef lpNumberOfBytesWritten As IntPtr) As Boolean
        End Function

        <DllImport("kernel32.dll", setlasterror:=True)> _
        Public Shared Function VirtualFreeEx(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal dwSize As UIntPtr, ByVal dwFreeType As UInteger) As Boolean
        End Function
    End Class

    Public Class ProcessMemoryReaderWriter

        Public Sub New()
        End Sub

        ''' <summary>	
        ''' Process from which to read		
        ''' </summary>
        Public Property ReadWriteProcess() As Process
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
            m_hProcess = ProcessMemoryReaderWriterApi.OpenProcess(ProcessMemoryReaderWriterApi.PROCESS_VM_READ Or ProcessMemoryReaderWriterApi.PROCESS_VM_WRITE Or ProcessMemoryReaderWriterApi.PROCESS_VM_OPERATION _
                                                                  , 1, CInt(m_ReadProcess.Id))
        End Sub

        Public Sub CloseHandle()
            Dim iRetValue As Integer
            iRetValue = ProcessMemoryReaderWriterApi.CloseHandle(m_hProcess)
            If iRetValue = 0 Then
                Throw New Exception("CloseHandle failed")
            End If
        End Sub

        Public Function ReadProcessMemory(ByVal MemoryAddress As IntPtr, ByVal bytesToRead As UInteger, ByRef bytesReaded As Integer) As Byte()
            Dim buffer As Byte() = New Byte(bytesToRead - 1) {}

            Dim ptrBytesReaded As IntPtr
            ProcessMemoryReaderWriterApi.ReadProcessMemory(m_hProcess, MemoryAddress, buffer, bytesToRead, ptrBytesReaded)

            bytesReaded = ptrBytesReaded.ToInt32()

            Return buffer
        End Function

        Public Function AllocateAndWriteData(ByRef datablock As Byte()) As IntPtr
            'allocates space in the currently opened process and writes the given data
            'to that block.  The pointer to the allocated block is returned
            'if there is an error, the pointer will be 0
            '
            'it is the responsibility of the process that the data is allocated in to de allocate it


            Dim retPtr As New IntPtr(0)

            retPtr = ProcessMemoryReaderWriterApi.VirtualAllocEx(m_hProcess, New IntPtr(0), datablock.Length, &H1000I Or &H2000I, &H4I)
            If Err.LastDllError <> 0 Then
                Trace.WriteLine(New ComponentModel.Win32Exception().Message)
            End If

            If retPtr.ToInt32 <> 0 Then
                If ProcessMemoryReaderWriterApi.WriteProcessMemory(m_hProcess, retPtr, datablock, datablock.Length, New IntPtr(0)) = 0 Then
                    retPtr = New IntPtr(0)
                End If
            End If

            Return retPtr
        End Function

        Public Function DeallocateBlock(ByVal blockAddr As IntPtr) As Boolean
            'deallocates a block previously allocated with VirtualAllocEx
            If ProcessMemoryReaderWriterApi.VirtualFreeEx(m_hProcess, blockAddr, 0, &H8000I) = 0 Then
                Trace.WriteLine("VirtualFreeEx error code: " & Err.LastDllError)
                Return False
            End If
            Return True
        End Function
    End Class
End Namespace
