Public Class MTPAxe

#Region "MTPAxe defines"
    Private Const MTPAXE_M_QUIT As Short = 0
    Private Const MTPAXE_M_WMDMAUTHENTICATE = 1
    Private Const MTPAXE_M_DEVICEMANAGER_GETREVISION = 10
    Private Const MTPAXE_M_DEVICEMANAGER_GETDEVICECOUNT = 11
    Private Const MTPAXE_M_DEVICEMANAGER_ENUMERATEDEVICES = 12
    Private Const MTPAXE_M_DEVICEMANAGER_SETCURRENTDEVICE = 13
    Private Const MTPAXE_M_DEVICE_GETMANUFACTURER = 20
    Private Const MTPAXE_M_DEVICE_GETTYPE = 21
    Private Const MTPAXE_M_DEVICE_ENUMERATESTORAGE = 22
    Private Const MTPAXE_M_DEVICE_GETICON = 25
    Private Const MTPAXE_M_DEVICE_CREATEPLAYLIST = 26
    Private Const MTPAXE_M_DEVICE_DELETEPLAYLIST = 27
    Private Const MTPAXE_M_PLAYLIST_ENUMERATECONTENTS = 30
    Private Const MTPAXE_M_STORAGE_GETSIZEINFO = 40
    Private Const MTPAXE_M_STORAGE_CREATEFROMFILE = 41

    Public Const WMDM_FILE_ATTR_FOLDER = &H8
    Public Const WMDM_FILE_ATTR_FILE = &H20
#End Region

    'the mtpaxe.exe
    Private axe As Process

    Private sIn As System.IO.StreamReader
    Private sOut As System.IO.StreamWriter
    Private sErr As System.IO.StreamReader

    Public Function startAxe() As Boolean
        'create the MTPaxe process for communication
        axe = New Process
        axe.StartInfo.FileName = System.IO.Path.Combine(Application.StartupPath, "MTPAxe.exe")
        axe.StartInfo.UseShellExecute = False 'importante to be able to reassign io streams
        axe.StartInfo.CreateNoWindow = True
        axe.StartInfo.RedirectStandardInput = True
        axe.StartInfo.RedirectStandardOutput = True
        axe.StartInfo.RedirectStandardError = True

        Try
            axe.Start()

            sOut = axe.StandardInput
            sOut.AutoFlush = True
            sIn = axe.StandardOutput
            sErr = axe.StandardError


            'authenticate the interface. after this is done, we can communicate
            sOut.WriteLine(MTPAXE_M_WMDMAUTHENTICATE)
            'if 0 is returned, everything was ok
            Dim s As String
            s = sIn.ReadLine
            If Not s = "0" Then
                Throw New Exception("MTPAxe error - " & sErr.ReadLine)
            End If
        Catch ex As Exception
            Trace.WriteLine("Error initializing MTPAxe: " & ex.Message)
            axe = Nothing
            Return False
        End Try

        Return True
    End Function
    Public Function stopAxe() As Boolean
        'closes the connection to MTPAxe
        If axe Is Nothing Then Exit Function

        sOut.WriteLine(MTPAXE_M_QUIT)
        axe.WaitForExit(4000)
        If axe.HasExited Then
            sOut.Close()
            sIn.Close()
            sErr.Close()

            axe = Nothing
            Return True
        Else
            Return False
        End If
    End Function

#Region "Device manager functions"
    Public Function getDeviceManagerRevision() As String
        'this function returns "" on error
        'MTPAxe returns -1 on error, revision number if no error

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICEMANAGER_GETREVISION)
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return ""
        End If

        Return s
    End Function
    Public Function getDeviceCount() As String
        'this function returns "-1" on error
        'MTPAxe returns -1 on error, number if no error

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICEMANAGER_GETDEVICECOUNT)
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function enumerateDevices() As String
        'this function returns "-1" on error
        'MTPAxe returns -1 on error, else it returns an ':' separated list
        'of the name of each device

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICEMANAGER_ENUMERATEDEVICES)
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function setCurrentDevice(ByVal devName As String) As String
        'sets the currently active device. this is the device that all related
        'device functions will function on. 0 on succeess -1 on error
        '
        'note: after setting the current device suceesfully, the device storage must be 
        'enumerated first (e.g. by calling getFullTreeView before most device related funcions will work.

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICEMANAGER_SETCURRENTDEVICE)
        sOut.WriteLine(devName)

        Dim s As String = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return "0"
    End Function
#End Region


#Region "Device functions"
    '*****************************************************************************************
    'setCurrentDevice must be called at least once before calling the functions below
    '****************************************************************************************
    Private Function enumerateStorage() As String
        'this function returns "-1" on error
        'MTPAxe returns -1 on error, ':' separated list containing the directory tree otherwise
        '
        'the format of the returned string is like the following example:

        '       ROOT
        '           FOLDER1
        '               file1
        '           file2

        'would be returned as
        '<0,TYPE,NULL>ROOT:<1,TYPE,ROOT>FOLDER1:<2,TYPE,FOLDER1>file1:<1,TYPE,ROOT>file2
        '
        'where TYPE is a DWORD containing the type (must be AND'ed with WMDM_FILE_ATTR_FILE or
        'WMDM_FILE_ATTR_FOLDER to find out if the object is a file or folder)
        Trace.WriteLine("MTPAxe: enumerating storage")

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_ENUMERATESTORAGE)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: enumerating storage error - " & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function getDeviceManufacturer() As String
        'gets the manufacturer of the current device
        'this function returns "-1" on error
        'MTPAxe returns -1 on error, name otherwise

        Trace.WriteLine("MTPAxe: getting device manufacturer")

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_GETMANUFACTURER)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: getting device manufacturer error - " & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function getDeviceType() As String
        'gets the attributes of the current device
        'this function returns "-1" on error
        'MTPAxe returns -1 on error, ';' separated list otherwise

        Trace.WriteLine("MTPAxe: getting device type")

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_GETTYPE)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: getting device type error - " & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function

    '*****************************************************************************************
    'enumerate storage must be called at least once prior to using the following functions
    '****************************************************************************************
    ''' <summary>
    ''' returns a treeview of the full directory structure of the current device
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getFullTreeView() As TreeView
        'returns the full directory structure of the device
        Trace.WriteLine("MTPAxe: building full directory tree")

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        Dim theTreeView As TreeView = Nothing

        'get the directory tree from the device
        Dim strTree As String
        strTree = enumerateStorage()        
        If Not strTree = "-1" Then
            Try
                theTreeView = buildTreeViewFromDirectoryTreePtr(Integer.Parse(strTree.Split(":")(0), Globalization.NumberStyles.HexNumber), Integer.Parse(strTree.Split(":")(1)), False)
            Catch ex As Exception
                theTreeView = New TreeView
                Trace.WriteLine("MTPAxe: building full directory tree - empty tree returned")
            End Try
        End If

        Return theTreeView
    End Function

    Private Function findTreeNode(ByRef root As TreeNode, ByVal nName As String, ByVal nType As Integer, ByVal nLevel As Integer) As TreeNode
        'searches for a node given a starting root node.  the search includes the root node (not just the children)
        'if the node is not found, Nothing is returned

        Dim nodeLevel As Integer, nodeType As Integer, item As StorageItem

        'first, check to see if the root node matches the node we're loking for
        'get node attributes
        item = CType(root.Tag, StorageItem)
        nodeLevel = item.DirectoryDepth
        nodeType = item.StorageType
        If root.Text = nName AndAlso nodeLevel = nLevel AndAlso nodeType & nType Then
            'were done
            Return root
        End If

        For Each tn As TreeNode In root.Nodes
            'get node attributes
            item = CType(tn.Tag, StorageItem)
            nodeLevel = item.DirectoryDepth
            nodeType = item.StorageType

            'check for matches 
            If tn.Text = nName AndAlso nodeLevel = nLevel AndAlso nodeType & nType Then
                'were done
                Return tn
            Else
                'no match. check if it's a folder and make recursive cal
                If nodeType & MTPAxe.WMDM_FILE_ATTR_FOLDER Then
                    Dim foundNode As TreeNode
                    foundNode = findTreeNode(tn, nName, nType, nLevel)
                    'if the node was found in the subtree, then were done
                    If Not foundNode Is Nothing Then
                        Return foundNode
                    End If

                End If

            End If

        Next

        'if we reach here, no matching nodes were found in subtree
        Return Nothing

    End Function
    Private Function findTreeNodeByID(ByRef root As TreeNode, ByVal ID As String) As TreeNode
        'searches for the first matching treenode with the given PersistentUniqueID
        Dim ret As TreeNode = Nothing
        Dim item As StorageItem

        item = CType(root.Tag, StorageItem)
        If item.ID = ID Then
            Return root
        End If

        For Each node As TreeNode In root.Nodes
            item = CType(node.Tag, StorageItem)
            If item.ID = ID Then
                ret = node
            ElseIf node.Nodes.Count > 0 Then
                'else check the child nodes
                ret = findTreeNodeByID(node, ID)
            End If

            'if we found it, return it
            If ret IsNot Nothing Then
                Return ret
            End If
        Next

        Return ret
    End Function
    Private Function buildTreeViewFromDirectoryTreePtr(ByVal arrStorageItemsPtr As Integer, ByVal numItems As Integer, ByVal usePlaylistMode As Boolean) As TreeView
        'builds a treeview based on the given directory tree, in the format returned
        'by getDirectoryTree, if the directoryTree is not in the right format, unpredicable behaviour will occur
        Dim treenodes() As TreeNode
        Dim tn As TreeNode
        Dim index As Integer = 0
        Dim treeview1 As New TreeView

        Dim shinfo As New SHFILEINFO()
        Dim hImgSmall As IntPtr  'The handle to the system image list.
        Dim myIcon As System.Drawing.Icon 'The icon is returned in the hIcon member of the shinfo struct.
        Dim fileExt As String
        Dim imglst As New ImageList
        imglst.ColorDepth = ColorDepth.Depth32Bit
        treeview1.ImageList = imglst
        shinfo.szDisplayName = New String(Chr(0), 260)
        shinfo.szTypeName = New String(Chr(0), 80)
        Dim item As StorageItem

        Dim memreader As New MemoryReader.ProcessMemoryReader
        Dim buffer() As Byte
        Dim bytesRead As Integer
        memreader.ReadProcess = axe
        memreader.OpenProcess()

        'keeps track of the minimum and maximum directory level
        Dim minLevel As Short = -1, maxLevel As Short = -1

        ReDim treenodes(numItems - 1)

        Dim sizeofStruct As Integer = 64
        For i As Integer = 0 To numItems - 1
            '***read the arrStorageItems array from MTPAxe's memory space. Each item in the array 
            'is a struct with the folllowing format:
            'IWMDMStorage4 *pStorage;	//the storage item
            'IWMDMStorage4 *pStorageParent;
            'int level;					//the level in the directory tree (level 0 is the root)
            'int type;					//the type of the item e.g. file or folder (since a file and a folder can have the same name at thesame directory level)
            'unsigned long long size;	//the size of the file in bytes
            'wchar_t *parentFileName;
            'wchar_t *fileName;            
            'wchar_t *title;
            'wchar_t *albumArtist;
            'wchar_t *albumTitle;
            'wchar_t *genre;
            'wchar_t *year;
            'unsigned long trackNum;
            'wchar_t *persistentUniqueID;
            'wchar_t *parentUniqueID;

            item = New StorageItem

            'start reading at offset 8 so as to skip the first 2 pointers which we dont need
            'level
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 8, 4, bytesRead)
            item.DirectoryDepth = BitConverter.ToInt32(buffer, 0)

            'type
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 12, 4, bytesRead)
            item.StorageType = BitConverter.ToInt32(buffer, 0)

            'size
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 16, 8, bytesRead)
            item.Size = BitConverter.ToInt64(buffer, 0)

            'pointer to parentfileName
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 24, 4, bytesRead)
            item.ParentFileName = readWideCharFromPointer(memreader, BitConverter.ToInt32(buffer, 0))

            'pointer to fileName
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 28, 4, bytesRead)
            item.FileName = readWideCharFromPointer(memreader, BitConverter.ToInt32(buffer, 0))

            'pointer to title
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 32, 4, bytesRead)
            item.Title = readWideCharFromPointer(memreader, BitConverter.ToInt32(buffer, 0))

            'pointer to albumArtist
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 36, 4, bytesRead)
            item.AlbumArtist = readWideCharFromPointer(memreader, BitConverter.ToInt32(buffer, 0))

            'pointer to albumTitle
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 40, 4, bytesRead)
            item.AlbumTitle = readWideCharFromPointer(memreader, BitConverter.ToInt32(buffer, 0))

            'pointer to genre
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 44, 4, bytesRead)
            item.Genre = readWideCharFromPointer(memreader, BitConverter.ToInt32(buffer, 0))

            'pointer to year
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 48, 4, bytesRead)
            item.Year = readWideCharFromPointer(memreader, BitConverter.ToInt32(buffer, 0))

            'tracknum
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 52, 4, bytesRead)
            item.TrackNum = BitConverter.ToInt32(buffer, 0)

            'pointer to persistentUniqueID
            buffer = memreader.ReadProcessMemory(arrStorageItemsPtr + (i * sizeofStruct) + 56, 4, bytesRead)
            item.ID = readWideCharFromPointer(memreader, BitConverter.ToInt32(buffer, 0))
            '***

            'keep track of the maximum and minimim directory depth
            'if tihs is the first iteration, set the minimum and the maximum
            'to the value of the first node
            If minLevel = -1 And maxLevel = -1 Then
                minLevel = item.DirectoryDepth
                maxLevel = item.DirectoryDepth
            End If
            If item.DirectoryDepth < minLevel Then
                minLevel = item.DirectoryDepth
            End If
            If item.DirectoryDepth > maxLevel Then
                maxLevel = item.DirectoryDepth
            End If

            'hack for wmp11
            If item.DirectoryDepth = 0 Then
                item.FileName = "Storage Media"
            End If
            tn = New TreeNode
            tn.Tag = item 'store item attributes in the node tag
            tn.Text = item.FileName

            fileExt = IO.Path.GetExtension(tn.Text)
            tn.ImageKey = fileExt   'the key to the image is the file extension
            tn.SelectedImageKey = fileExt

            'see whether node is file or directory
            If (item.StorageType And WMDM_FILE_ATTR_FILE) = WMDM_FILE_ATTR_FILE Then
                'check to see if the file type already has an image in the image list
                'if it doesn't retreive it from the operating system
                If Not imglst.Images.ContainsKey(fileExt) Then
                    hImgSmall = SHGetFileInfo("x" & fileExt, _
                                                  FILE_ATTRIBUTE_NORMAL, _
                                                  shinfo, _
                                                  System.Runtime.InteropServices.Marshal.SizeOf(shinfo), _
                                                  SHGFI_ICON Or SHGFI_SMALLICON Or SHGFI_USEFILEATTRIBUTES)
                    myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon)
                    imglst.Images.Add(tn.ImageKey, myIcon) 'Add icon to imageList
                    shinfo = Nothing
                End If
            Else
                'its a directory
                'check to see if the file type already has an image in the image list
                'if it doesn't retreive it from the operating system
                If Not imglst.Images.ContainsKey("*") Then
                    hImgSmall = SHGetFileInfo("*", _
                                                  FILE_ATTRIBUTE_DIRECTORY, _
                                                  shinfo, _
                                                  System.Runtime.InteropServices.Marshal.SizeOf(shinfo), _
                                                  SHGFI_ICON Or SHGFI_SMALLICON Or SHGFI_USEFILEATTRIBUTES)
                    myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon)
                    imglst.Images.Add("*", myIcon) 'Add icon to imageList
                    shinfo = Nothing
                End If
                tn.ImageKey = "*"
                tn.SelectedImageKey = "*"
            End If

            treenodes(index) = tn
            index += 1
        Next
        memreader.CloseHandle()

        'now go though the treenodes array and pick out what goes where based on the level 
        'this loop only executes a maximum of 10 times since the sony supports a maximum depth of 10
        '(0 to 9). Starting from the MUSIC directory (level 1), a depth of 8 is supported (0 to 7)
        'if we're making a playlist (usePlaylistMode=true), then add all nodes to the root with no
        'heirarchical structure
        Dim nodeLevel As Integer, nodeType As Integer, nodeParent As String
        If Not usePlaylistMode Then
            For index = minLevel To maxLevel

                For Each tn In treenodes

                    'get node info
                    item = CType(tn.Tag, StorageItem)
                    nodeLevel = item.DirectoryDepth
                    nodeType = item.StorageType
                    nodeParent = item.ParentFileName

                    'process only nodes with level=index
                    If item.DirectoryDepth = index Then
                        If index = minLevel Then
                            'for level 0 items, add them straight to the tree
                            treeview1.Nodes.Add(tn)
                        Else
                            'for all other levels, search the tree for the parent
                            Dim parent As TreeNode = Nothing
                            For Each tnode As TreeNode In treeview1.Nodes
                                If nodeLevel - 1 = 0 Then 'hack for wmp11
                                    nodeParent = "Storage Media"
                                End If
                                parent = findTreeNode(tnode, nodeParent, MTPAxe.WMDM_FILE_ATTR_FOLDER, nodeLevel - 1)
                                If parent Is Nothing Then
                                    'the parent was not found
                                    Trace.WriteLine("error building treeview")
                                    Throw New Exception("Error building treeview")
                                End If
                            Next
                            'now that we have the parent, add to it it's child
                            parent.Nodes.Add(tn)

                        End If
                    End If

                Next
            Next
        Else
            'just make a list (for playlists)
            For Each tn In treenodes
                treeview1.Nodes.Add(tn)
            Next
        End If


        Return treeview1
    End Function
    Private Function readWideCharFromPointer(ByVal memreader As MemoryReader.ProcessMemoryReader, ByVal ptrToWcharStr As IntPtr) As String
        'reads a wchar from memory and returns it in a .net string

        Dim buffer() As Byte
        Dim bytesRead As Integer
        Dim retstr As String = ""

        'read 2 bytes at a time until the string terminator is found
        Do
            buffer = memreader.ReadProcessMemory(ptrToWcharStr, 2, bytesRead)
            If bytesRead <> 2 Then
                buffer(0) = 0
                buffer(1) = 0
            Else
                If Not (buffer(0) = 0 And buffer(1) = 0) Then
                    retstr = retstr & BitConverter.ToChar(buffer, 0)
                    'move to the next 2 bytes
                    ptrToWcharStr = New IntPtr(ptrToWcharStr.ToInt32 + 2)
                End If
            End If
        Loop Until (buffer(0) = 0 And buffer(1) = 0)

        Return retstr
    End Function

    Public Function createPlaylist(ByVal playlistName As String, ByVal items As String) As String
        'creates a playlist. items are in the same format as returned by enumerateStorage
        'this function returns "-1" on error otherwise
        'MTPAxe returns -1 on error, 0 otherwise

        Trace.WriteLine("MTPAxe: creating playlist " & playlistName)

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_CREATEPLAYLIST)
        sOut.WriteLine(playlistName)
        sOut.WriteLine(items)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: creating playlist" & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function deletePlaylist(ByVal playlistName As String) As String
        'deletes the specified playlist. if there is more than 1 playlist with the same name
        'the first one is deleted
        'this function returns "-1" on error otherwise
        'MTPAxe returns -1 on error, 0 otherwise

        Trace.WriteLine("MTPAxe: deleting playlist " & playlistName)

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_DELETEPLAYLIST)
        sOut.WriteLine(playlistName)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: deleting playlist error - " & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Private Function enumeratePlaylist(ByVal name As String)
        'gets the contents of a playlist in the same format as enumerateStorage
        'this function returns "-1" on error otherwise
        'MTPAxe returns -1 on error, 0 otherwise

        Trace.WriteLine("MTPAxe: enumerating playlist contents")

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_PLAYLIST_ENUMERATECONTENTS)
        sOut.WriteLine(name)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: enumerating playlist contents error - " & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function getPlaylistContentsIDs(ByVal playlistName As String) As String()
        'returns the PersistenUniqueID's of the contents of the playlist.  The caller
        'must then build the actual playlists contents.
        Dim str As String
        Dim strarr() As String = Nothing

        str = enumeratePlaylist(playlistName)
        If Not str = "-1" Then
            'parse the string
            strarr = str.Split(":"c)
        End If

        Return strarr
    End Function
    Public Function getPlaylistContentsAsTreeview(ByVal name As String) As TreeView
        'returns the full directory structure of the device
        'returns an empty treeview if theplaylist is empty

        Trace.WriteLine("MTPAxe: building playlist contents - " & name)
        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        Dim theTreeView As New TreeView

        'get the directory tree from the device
        Dim strTree As String
        strTree = enumeratePlaylist(name)
        If Not strTree = "-1" Then
            Try
                theTreeView = getPlaylistContentsAsTreeview_helper(strTree)
            Catch ex As Exception
                theTreeView = New TreeView
                Trace.WriteLine("MTPAxe: bulding playlist contents - empty playlist")
            End Try
        End If

        Return theTreeView
    End Function
    Private Function getPlaylistContentsAsTreeview_helper(ByVal directoryTree As String) As TreeView
        'builds a treeview based on the given directory tree, in the format returned
        'by getDirectoryTree, if the directoryTree is not in the right format, unpredicable behaviour will occur
        Dim nodes() As String
        Dim properties As String
        Dim treenodes() As TreeNode
        Dim tn As TreeNode
        Dim tagIndex As Integer
        Dim index As Integer = 0
        Dim index2 As Integer = 0
        Dim treeview1 As New TreeView

        Dim shinfo As New SHFILEINFO()
        Dim fileExt As String
        Dim imglst As New ImageList
        imglst.ColorDepth = ColorDepth.Depth32Bit
        treeview1.ImageList = imglst
        shinfo.szDisplayName = New String(Chr(0), 260)
        shinfo.szTypeName = New String(Chr(0), 80)

        'keeps track of the minimum and maximum directory level
        Dim minLevel As Short = -1, maxLevel As Short = -1, level As Byte

        nodes = directoryTree.Split(":"c)
        ReDim treenodes(nodes.Length - 1)

        For Each node As String In nodes
            'extract the properties associated with each node
            tagIndex = node.LastIndexOf(">"c)
            properties = node.Substring(1, tagIndex - 1)

            'keep track of the maximum and minimim directory depth
            'if tihs is the first iteration, set the minimum and the maximum
            'to the value of the first node
            level = Byte.Parse((properties.Split("/"c))(0))
            If minLevel = -1 And maxLevel = -1 Then
                minLevel = level
                maxLevel = level
            End If
            If level < minLevel Then
                minLevel = level
            End If
            If level > maxLevel Then
                maxLevel = level
            End If

            tn = New TreeNode
            tn.Tag = node.Substring(1, tagIndex - 1) 'store level,type,parent in the tag
            tn.Text = node.Substring(tagIndex + 1, node.Length - tagIndex - 1)

            fileExt = IO.Path.GetExtension(tn.Text)
            tn.ImageKey = fileExt   'the key to the image is the file extension
            tn.SelectedImageKey = fileExt

            'see whether node is file or directory
            If (Integer.Parse((tn.Tag.ToString.Split("/"c))(1)) And WMDM_FILE_ATTR_FILE) = WMDM_FILE_ATTR_FILE Then
            Else
                'its a directory
                tn.ImageKey = "*"
                tn.SelectedImageKey = "*"
            End If

            treenodes(index) = tn
            index += 1
        Next

        'just make a list
        For Each tn In treenodes
            treeview1.Nodes.Add(tn)
        Next


        Return treeview1
    End Function

    Public Function uploadFile(ByVal path As String, ByVal destinationID As String, ByVal type As Integer, Optional ByVal metadata As StorageItem = Nothing) As String
        'copies the file specified by path to the folder specified by destination
        'destination is the persistentuniqueid of the destination folder
        'type specifies is path is a file or a folder. 0=file 1=folder
        'for folders, path specifies the name of the folder. the contents are not uploaded automatically
        'the optional metadata argument specifies what metadata will be written along with the item. note
        'this argument is only valid for files, not folders. if metadatra is specified for a folder, it will be ignored
        'returns the persistientuniqueid of the created item on OK, -1 on error

        Trace.WriteLine("MTPAxe: uploading file " & path)

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        If metadata Is Nothing Then metadata = New StorageItem

        sOut.WriteLine(MTPAXE_M_STORAGE_CREATEFROMFILE)
        sOut.WriteLine(path.Replace("\"c, "\\"))
        sOut.WriteLine(destinationID)
        sOut.WriteLine(type)
        sOut.WriteLine(IIf(metadata.Title = "", "`", metadata.Title))
        sOut.WriteLine(IIf(metadata.AlbumArtist = "", "`", metadata.AlbumArtist))
        sOut.WriteLine(IIf(metadata.AlbumTitle = "", "`", metadata.AlbumTitle))
        sOut.WriteLine(IIf(metadata.Genre = "", "`", metadata.Genre))
        sOut.WriteLine(IIf(metadata.Year = "", "`", metadata.Year))
        sOut.WriteLine(IIf(metadata.TrackNum = "", "`", metadata.TrackNum))

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: upload file - " & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function getDeviceIcon(ByVal savePath As String) As String
        'gets the device icon
        'this function returns "-1" on error, 0 otherwise

        Trace.WriteLine("MTPAxe: saving device icon")

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")


        sOut.WriteLine(MTPAXE_M_DEVICE_GETICON)
        'MTPAxe will now be expecting the save path parameter
        sOut.WriteLine(savePath)

        'now wait for the return value to be sent to the buffer.
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: saving device icon error - " & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function getDeviceCapacity() As String
        'returns the capacity of the current device and free space as a : separated string, -1 on error

        Trace.WriteLine("MTPAxe: getting device capacity")

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_STORAGE_GETSIZEINFO)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine("MTPAxe: getting device capacity error - " & sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function

#End Region

End Class
