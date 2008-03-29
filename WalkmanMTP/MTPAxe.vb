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
                theTreeView = buildTreeViewFromDirectoryTree(strTree, False)
            Catch ex As Exception
                theTreeView = New TreeView
                Trace.WriteLine("MTPAxe: building full directory tree - empty tree returned")
            End Try
        End If

        Return theTreeView
    End Function
    Public Function getTreeViewByName(ByVal storageItemName As String) As TreeView
        'returns the tree of the specified directory only
        'returns an empty treeview on error

        Trace.WriteLine("MTPAxe: building directory tree for " & storageItemName)

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        Dim theTreeView As New TreeView

        'get the directory tree from the device
        Dim strTree As String
        strTree = enumerateStorage()
        If Not strTree = "-1" Then
            Try
                'get the full tree first
                Dim tmpTree As TreeView
                tmpTree = buildTreeViewFromDirectoryTree(strTree, False)

                Dim foundStorage As TreeNode = Nothing
                For Each node As TreeNode In tmpTree.Nodes
                    foundStorage = findTreeNode(node, storageItemName, WMDM_FILE_ATTR_FOLDER, 1)
                    If Not foundStorage Is Nothing Then
                        Exit For
                    End If
                Next

                theTreeView.Nodes.Add(foundStorage.Clone)
                theTreeView.ImageList = tmpTree.ImageList

            Catch ex As Exception
                theTreeView = New TreeView
                Trace.WriteLine("MTPAxe: building directory tree for " & storageItemName & " - empty tree returned")
            End Try

        End If

        Return theTreeView
    End Function
    Private Function findTreeNode(ByRef root As TreeNode, ByVal nName As String, ByVal nType As Integer, ByVal nLevel As Integer) As TreeNode
        'searches for a node given a starting root node.  the search includes the root node (not just the children)
        'if the node is not found, Nothing is returned

        Dim nodeLevel As Integer, nodeType As Integer

        'first, check to see if the root node matches the node we're loking for
        'get node attributes
        nodeLevel = (root.Tag.ToString.Split(","c))(0)
        nodeType = (root.Tag.ToString.Split(","c))(1)
        If root.Text = nName AndAlso nodeLevel = nLevel AndAlso nodeType & nType Then
            'were done
            Return root
        End If

        For Each tn As TreeNode In root.Nodes
            'get node attributes
            nodeLevel = (tn.Tag.ToString.Split(","c))(0)
            nodeType = (tn.Tag.ToString.Split(","c))(1)

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
    Private Function buildTreeViewFromDirectoryTree(ByVal directoryTree As String, ByVal usePlaylistMode As Boolean) As TreeView
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
        Dim hImgSmall As IntPtr  'The handle to the system image list.
        Dim myIcon As System.Drawing.Icon 'The icon is returned in the hIcon member of the shinfo struct.
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
            level = Byte.Parse((properties.Split(","c))(0))
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

            'need to find out if the node is a file or directory
            If (Integer.Parse((tn.Tag.ToString.Split(","c))(1)) And WMDM_FILE_ATTR_FILE) = WMDM_FILE_ATTR_FILE Then
                'Use this to get the small icon.
                hImgSmall = SHGetFileInfo("x" & fileExt, _
                                          FILE_ATTRIBUTE_NORMAL, _
                                          shinfo, _
                                          System.Runtime.InteropServices.Marshal.SizeOf(shinfo), _
                                          SHGFI_ICON Or SHGFI_SMALLICON Or SHGFI_USEFILEATTRIBUTES)
            Else
                'Use this to get the small icon.
                hImgSmall = SHGetFileInfo("*", _
                                          FILE_ATTRIBUTE_DIRECTORY, _
                                          shinfo, _
                                          System.Runtime.InteropServices.Marshal.SizeOf(shinfo), _
                                          SHGFI_ICON Or SHGFI_SMALLICON Or SHGFI_USEFILEATTRIBUTES)
            End If



            myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon)
            Try
                imglst.Images.Add(fileExt, myIcon) 'Add icon to imageList
            Catch ex As Exception
                'image already exists
            End Try

            treenodes(index) = tn
            index += 1
        Next

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
                    nodeLevel = (tn.Tag.ToString.Split(","c))(0)
                    nodeType = (tn.Tag.ToString.Split(","c))(1)
                    nodeParent = (tn.Tag.ToString.Split(","c))(2)

                    'process only nodes with level=index
                    If nodeLevel = index Then
                        If index = minLevel Then
                            'for level 0 items, add them straight to the tree
                            treeview1.Nodes.Add(tn)
                        Else
                            'for all other levels, search the tree for the parent
                            Dim parent As TreeNode = Nothing
                            For Each tnode As TreeNode In treeview1.Nodes
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
    Public Function getPlaylistContentsAsTreeview(ByVal name As String) As TreeView
        'returns the full directory structure of the device
        'returns an empty treeview if theplaylist is empty

        Trace.WriteLine("MTPAxe: bulding playlist contents")
        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        Dim theTreeView As New TreeView

        'get the directory tree from the device
        Dim strTree As String
        strTree = enumeratePlaylist(name)
        If Not strTree = "-1" Then
            Try
                theTreeView = buildTreeViewFromDirectoryTree(strTree, True)
            Catch ex As Exception
                theTreeView = New TreeView
                Trace.WriteLine("MTPAxe: bulding playlist contents - empty playlist")
            End Try
        End If

        Return theTreeView
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
