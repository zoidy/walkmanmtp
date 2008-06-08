''Copyright 2008 Dr. Zoidberg
' 
'Licensed under the Apache License, Version 2.0 (the "License");
'you may not use this file except in compliance with the License.
'You may obtain a copy of the License at
'
'	http://www.apache.org/licenses/LICENSE-2.0 
'
'Unless required by applicable law or agreed to in writing, software 
'distributed under the License is distributed on an "AS IS" 
'BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'See the License for the specific language governing permissions 
'and limitations under the License. 


Module WinAPI
    Public Structure SHFILEINFO
        Public hIcon As IntPtr ' : icon
        Public iIcon As Integer ' : icondex
        Public dwAttributes As Integer ' : SFGAO_ flags
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=260)> _
        Public szDisplayName As String
        <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=80)> _
        Public szTypeName As String
    End Structure

    Public Const SHGFI_ICON = &H100
    Public Const SHGFI_SMALLICON = &H1
    Public Const SHGFI_LARGEICON = &H0         ' Large icon
    Public Const SHGFI_USEFILEATTRIBUTES = &H10
    Public Const FILE_ATTRIBUTE_DIRECTORY = &H10
    Public Const FILE_ATTRIBUTE_NORMAL = &H80

    Public Declare Ansi Function SHGetFileInfo Lib "shell32.dll" (ByVal pszPath As String, _
    ByVal dwFileAttributes As Integer, ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer, _
    ByVal uFlags As Integer) As IntPtr

    Public Declare Function DestroyIcon Lib "user32.dll" (ByVal hIcon As Int32) As Int32
End Module
