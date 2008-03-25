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
    'setCurrentDevice must be called prior to calling the functions in this section
    ''' <summary>
    ''' returns a treeview of the full directory structure of the current device
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getFullTreeView() As TreeView
        'returns the full directory structure of the device

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        Dim theTreeView As TreeView = Nothing

        'get the directory tree from the device
        Dim strTree As String
        strTree = enumerateStorage()
        'strTree = "<0,295176,>Storage Media:<1,295200,Storage Media>DevIcon.fil:<1,295200,Storage Media>DevLogo.fil:<1,295200,Storage Media>capability_00.xml:<1,295176,Storage Media>MUSIC:<2,295176,MUSIC>Angra - Rebirth:<3,295200,Angra - Rebirth>01 - In Excelsis.m4a:<3,295200,Angra - Rebirth>02 - Nova Era.m4a:<3,295200,Angra - Rebirth>03 - Millenium Sun.m4a:<3,295200,Angra - Rebirth>04 - Acid Rain.m4a:<3,295200,Angra - Rebirth>05 - Heroes Of Sand.m4a:<3,295200,Angra - Rebirth>06 - Unholy Wars (Part I_ Imperial Crown; Part II_ Forgiven Return).m4a:<3,295200,Angra - Rebirth>07 - Rebirth.m4a:<3,295200,Angra - Rebirth>08 - Judgement Day.m4a:<3,295200,Angra - Rebirth>09 - Running Alone.m4a:<3,295200,Angra - Rebirth>10 - Visions Prelude.m4a:<2,295176,MUSIC>Falconer - The Sceptre Of Deception:<3,295200,Falconer - The Sceptre Of Deception>01 - The Coronation.m4a:<3,295200,Falconer - The Sceptre Of Deception>02 - The Trail Of Flames.m4a:<3,295200,Falconer - The Sceptre Of Deception>03 - Under The Sword.m4a:<3,295200,Falconer - The Sceptre Of Deception>04 - Night Of Infamy.m4a:<3,295200,Falconer - The Sceptre Of Deception>05 - Hooves Over Northland.m4a:<3,295200,Falconer - The Sceptre Of Deception>06 - Pledge For Freedom.m4a:<3,295200,Falconer - The Sceptre Of Deception>07 - Ravenhair.m4a:<3,295200,Falconer - The Sceptre Of Deception>08 - The Sceptre Of Deception.m4a:<3,295200,Falconer - The Sceptre Of Deception>09 - Hear Me Pray.m4a:<3,295200,Falconer - The Sceptre Of Deception>10 - Child Of Innocence.m4a:<2,295176,MUSIC>Dragonforce - Inhuman Rampage:<3,295200,Dragonforce - Inhuman Rampage>01 - Through The Fire And Flames.m4a:<3,295200,Dragonforce - Inhuman Rampage>02 - Revolution Deathsquad.m4a:<3,295200,Dragonforce - Inhuman Rampage>03 - Storming The Burning Fields.m4a:<3,295200,Dragonforce - Inhuman Rampage>04 - Operation Ground And Pound.m4a:<3,295200,Dragonforce - Inhuman Rampage>05 - Body Breakdown.m4a:<3,295200,Dragonforce - Inhuman Rampage>06 - Cry For Eternity.m4a:<3,295200,Dragonforce - Inhuman Rampage>07 - The Flame Of Youth.m4a:<3,295200,Dragonforce - Inhuman Rampage>08 - Trail Of Broken Hearts.m4a:<3,295200,Dragonforce - Inhuman Rampage>09 - Lost Souls In Endless Time.m4a:<2,295200,MUSIC>Bush - The Chemicals Between Us.mp3:<2,295176,MUSIC>Edu Falaschi - Almah:<3,295200,Edu Falaschi - Almah>01 - King.m4a:<3,295200,Edu Falaschi - Almah>02 - Take Back Your Spell.m4a:<3,295200,Edu Falaschi - Almah>03 - Forgotten Land (Com Edu Ardanu).m4a:<3,295200,Edu Falaschi - Almah>04 - Scary Zone.m4a:<3,295200,Edu Falaschi - Almah>05 - Children Of Lies.m4a:<3,295200,Edu Falaschi - Almah>06 - Break All The Welds.m4a:<3,295200,Edu Falaschi - Almah>07 - Golden Empire.m4a:<3,295200,Edu Falaschi - Almah>08 - Primitive Chaos.m4a:<3,295200,Edu Falaschi - Almah>09 - Breathe.m4a:<3,295200,Edu Falaschi - Almah>10 - Box Of Illusion.m4a:<3,295200,Edu Falaschi - Almah>11 - Almah.m4a:<2,295176,MUSIC>Kiko Loureiro - No Gravity:<3,295200,Kiko Loureiro - No Gravity>10 - In A Gentle Way.m4a:<3,295200,Kiko Loureiro - No Gravity>11 - Dilemma.m4a:<3,295200,Kiko Loureiro - No Gravity>12 - Feliz Desilusão.m4a:<3,295200,Kiko Loureiro - No Gravity>13 - Choro De Crianca.m4a:<3,295200,Kiko Loureiro - No Gravity>01 - Enfermo.m4a:<3,295200,Kiko Loureiro - No Gravity>02 - Endangered Species.m4a:<3,295200,Kiko Loureiro - No Gravity>03 - Escaping.m4a:<3,295200,Kiko Loureiro - No Gravity>04 - No Gravity.m4a:<3,295200,Kiko Loureiro - No Gravity>05 - Pau-De-Arara.m4a:<3,295200,Kiko Loureiro - No Gravity>06 - La Force De L'âme.m4a:<3,295200,Kiko Loureiro - No Gravity>07 - Tapping Into My Dark Tranquility.m4a:<3,295200,Kiko Loureiro - No Gravity>08 - Moment Of Truth.m4a:<3,295200,Kiko Loureiro - No Gravity>09 - Beautiful Language.m4a:<2,295176,MUSIC>MAG Project - MAG Project:<3,295200,MAG Project - MAG Project>11 - Coma.m4a:<3,295200,MAG Project - MAG Project>01 - Cosmic Disturbance.m4a:<3,295200,MAG Project - MAG Project>02 - A Drop In The Sea.m4a:<3,295200,MAG Project - MAG Project>03 - Nowhere In Sight.m4a:<3,295200,MAG Project - MAG Project>04 - More Or Less.m4a:<3,295200,MAG Project - MAG Project>05 - Open Up.m4a:<3,295200,MAG Project - MAG Project>06 - Eternity.m4a:<3,295200,MAG Project - MAG Project>07 - Looking Up.m4a:<3,295200,MAG Project - MAG Project>08 - Storm.m4a:<3,295200,MAG Project - MAG Project>09 - No One Like You.m4a:<3,295200,MAG Project - MAG Project>10 - Highway 80.m4a:<2,295200,MUSIC>testfile.mp3:<2,295200,MUSIC>theforgottenpt2.mp3:<2,295176,MUSIC>Live - Throwing Copper:<3,295200,Live - Throwing Copper>09 - T.B.D..m4a:<3,295200,Live - Throwing Copper>10 - Stage.m4a:<3,295200,Live - Throwing Copper>11 - Waitress.m4a:<3,295200,Live - Throwing Copper>12 - Pillar Of Davidson.m4a:<3,295200,Live - Throwing Copper>13 - White, Discussion.m4a:<3,295200,Live - Throwing Copper>14 - Horse.m4a:<3,295200,Live - Throwing Copper>01 - The Dam At Otter Creek.m4a:<3,295200,Live - Throwing Copper>02 - Selling The Drama.m4a:<3,295200,Live - Throwing Copper>03 - I Alone.m4a:<3,295200,Live - Throwing Copper>04 - Iris.m4a:<3,295200,Live - Throwing Copper>05 - Lightning Crashes.m4a:<3,295200,Live - Throwing Copper>06 - Top.m4a:<3,295200,Live - Throwing Copper>07 - All Over You.m4a:<3,295200,Live - Throwing Copper>08 - Shit Towne.m4a:<2,295176,MUSIC>Megadeth - Youthanasia:<3,295200,Megadeth - Youthanasia>09 - Youthanasia.m4a:<3,295200,Megadeth - Youthanasia>10 - I Thought I Knew It All.m4a:<3,295200,Megadeth - Youthanasia>11 - Black Curtain.m4a:<3,295200,Megadeth - Youthanasia>12 - Victory.m4a:<3,295200,Megadeth - Youthanasia>01 - Reckoning Day.m4a:<3,295200,Megadeth - Youthanasia>02 - Train of Consequences.m4a:<3,295200,Megadeth - Youthanasia>03 - Addicted to Chaos.m4a:<3,295200,Megadeth - Youthanasia>04 - A Tout Le Monde.m4a:<3,295200,Megadeth - Youthanasia>05 - Elysian Fields.m4a:<3,295200,Megadeth - Youthanasia>06 - The Killing Road.m4a:<3,295200,Megadeth - Youthanasia>07 - Blood of Heroes.m4a:<3,295200,Megadeth - Youthanasia>08 - Family Tree.m4a:<2,295176,MUSIC>Metallica - Reload:<3,295200,Metallica - Reload>06 - Slither.m4a:<3,295200,Metallica - Reload>07 - Carpe Diem Baby.m4a:<3,295200,Metallica - Reload>08 - Bad Seed.m4a:<3,295200,Metallica - Reload>09 - Where The Wild Things Are.m4a:<3,295200,Metallica - Reload>10 - Prince Charming.m4a:<3,295200,Metallica - Reload>11 - Low Man's Lyric.m4a:<3,295200,Metallica - Reload>12 - Attitude.m4a:<3,295200,Metallica - Reload>13 - Fixxxer.m4a:<3,295200,Metallica - Reload>01 - Fuel.m4a:<3,295200,Metallica - Reload>02 - The Memory Remains.m4a:<3,295200,Metallica - Reload>03 - Devil's Dance.m4a:<3,295200,Metallica - Reload>04 - The Unforgiven II.m4a:<3,295200,Metallica - Reload>05 - Better Than You.m4a:<2,295176,MUSIC>Joe Satriani - The Extremist:<3,295200,Joe Satriani - The Extremist>05 - Rubina's Blue Sky Happiness.m4a:<3,295200,Joe Satriani - The Extremist>06 - Summer Song.m4a:<3,295200,Joe Satriani - The Extremist>07 - Tears In The Rain.m4a:<3,295200,Joe Satriani - The Extremist>08 - Why.m4a:<3,295200,Joe Satriani - The Extremist>09 - Motorcycle Driver.m4a:<3,295200,Joe Satriani - The Extremist>10 - New Blues.m4a:<3,295200,Joe Satriani - The Extremist>01 - Friends.m4a:<3,295200,Joe Satriani - The Extremist>02 - The Extremist.m4a:<3,295200,Joe Satriani - The Extremist>03 - War.m4a:<3,295200,Joe Satriani - The Extremist>04 - Cryin'.m4a:<2,295176,MUSIC>Angra - Holy Land:<3,295200,Angra - Holy Land>05 - Holy Land.m4a:<3,295200,Angra - Holy Land>06 - The Shaman.m4a:<3,295200,Angra - Holy Land>Angra - Holy Land.jpg:<3,295200,Angra - Holy Land>07 - Make Believe.m4a:<3,295200,Angra - Holy Land>08 - Z.I.T.O..m4a:<3,295200,Angra - Holy Land>09 - Deep Blue.m4a:<3,295200,Angra - Holy Land>10 - Lullaby For Lucifer.m4a:<3,295200,Angra - Holy Land>FOLDER.JPG:<3,295200,Angra - Holy Land>01 - Crossing.m4a:<3,295200,Angra - Holy Land>02 - Nothing To Say.m4a:<3,295200,Angra - Holy Land>03 - Silence And Distance.m4a:<3,295200,Angra - Holy Land>04 - Carolina IV.m4a:<2,295176,MUSIC>Joe Satriani - Joe Satriani:<3,295200,Joe Satriani - Joe Satriani>05 - S.M.F..m4a:<3,295200,Joe Satriani - Joe Satriani>06 - Look My Way.m4a:<3,295200,Joe Satriani - Joe Satriani>07 - Home.m4a:<3,295200,Joe Satriani - Joe Satriani>08 - Moroccan Sunset.m4a:<3,295200,Joe Satriani - Joe Satriani>09 - Killer Bee Bop.m4a:<3,295200,Joe Satriani - Joe Satriani>10 - Slow Down Blues.m4a:<3,295200,Joe Satriani - Joe Satriani>11 - (You're) My World.m4a:<3,295200,Joe Satriani - Joe Satriani>12 - Sittin' Around.m4a:<3,295200,Joe Satriani - Joe Satriani>01 - Cool #9.m4a:<3,295200,Joe Satriani - Joe Satriani>02 - If.m4a:<3,295200,Joe Satriani - Joe Satriani>03 - Down, Down, Down.m4a:<3,295200,Joe Satriani - Joe Satriani>04 - Luminous Flesh Giants.m4a:<2,295176,MUSIC>Dream Theater - A Change Of Seasons EP:<3,295200,Dream Theater - A Change Of Seasons EP>05 - The Big Medley.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>01 - A Change Of Seasons.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>02 - Funeral For A Friend_Love Lies Bleeding.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>03 - Perfect Strangers.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>04 - The Rover_Achilles Last Stand_The Song Remains The Same.m4a:<2,295176,MUSIC>Moist - Creature:<3,295200,Moist - Creature>04 - Leave It Alone.m4a:<3,295200,Moist - Creature>05 - Creature.m4a:<3,295200,Moist - Creature>06 - Shotgun.m4a:<3,295200,Moist - Creature>07 - Disco Days.m4a:<3,295200,Moist - Creature>08 - Tangerine.m4a:<3,295200,Moist - Creature>09 - Better Than You.m4a:<3,295200,Moist - Creature>10 - Baby Skin Tattoo.m4a:<3,295200,Moist - Creature>11 - Ophelia.m4a:<3,295200,Moist - Creature>12 - Gasoline.m4a:<3,295200,Moist - Creature>01 - Hate.m4a:<3,295200,Moist - Creature>02 - Theme From Cola.m4a:<3,295200,Moist - Creature>03 - Resurrection.m4a:<2,295176,MUSIC>Metallica - The Black Album:<3,295200,Metallica - The Black Album>01 - Enter Sandman.m4a:<3,295200,Metallica - The Black Album>02 - Sad But True.m4a:<3,295200,Metallica - The Black Album>03 - Holier Than Thou.m4a:<3,295200,Metallica - The Black Album>04 - The Unforgiven.m4a:<3,295200,Metallica - The Black Album>05 - Wherever I May Roam.m4a:<3,295200,Metallica - The Black Album>06 - Don't Tread On Me.m4a:<3,295200,Metallica - The Black Album>07 - Through The Never.m4a:<3,295200,Metallica - The Black Album>08 - Nothing Else Matters.m4a:<3,295200,Metallica - The Black Album>09 - Of Wolf And Man.m4a:<3,295200,Metallica - The Black Album>10 - The God That Failed.m4a:<3,295200,Metallica - The Black Album>11 - My Friend Of Misery.m4a:<3,295200,Metallica - The Black Album>12 - The Struggle Within.m4a:<1,295176,Storage Media>DCIM:<1,295176,Storage Media>PICTURE:<1,295176,Storage Media>PICTURES:<2,295176,PICTURES>autoshow:<3,295200,autoshow>DSC00014.JPG:<3,295200,autoshow>DSC00015.JPG:<3,295200,autoshow>DSC00016.JPG:<3,295200,autoshow>DSC00017.JPG:<3,295200,autoshow>DSC00018.JPG:<3,295200,autoshow>DSC00019.JPG:<3,295200,autoshow>DSC00021.JPG:<3,295200,autoshow>DSC00020.JPG:<3,295200,autoshow>DSC00003.JPG:<3,295200,autoshow>DSC00004.JPG:<3,295200,autoshow>DSC00005.JPG:<3,295200,autoshow>DSC00006.JPG:<3,295200,autoshow>DSC00007.JPG:<3,295200,autoshow>DSC00008.JPG:<3,295200,autoshow>DSC00009.JPG:<3,295200,autoshow>DSC00010.JPG:<3,295200,autoshow>DSC00011.JPG:<3,295200,autoshow>DSC00012.JPG:<3,295200,autoshow>DSC00013.JPG:<1,295176,Storage Media>VIDEO:<2,295200,VIDEO>test.mp4:<2,295200,VIDEO>subtest.mp4:<2,295200,VIDEO>Reboot 2x09 Trust No One (Dinothunderblack).mp4:<2,295200,VIDEO>Blue Harvest.JPG:<2,295200,VIDEO>Blue Harvest.mp4:<1,295176,Storage Media>MP_ROOT:<1,295200,Storage Media>WMPInfo.xml:<1,295176,Storage Media>Albums"
        If Not strTree = "-1" Then
            Try
                theTreeView = buildTreeViewFromDirectoryTree(strTree)
            Catch ex As Exception
                theTreeView = New TreeView
            End Try
        End If

        Return theTreeView
    End Function
    Public Function getTreeViewByName(ByVal storageItemName As String) As TreeView
        'returns the tree of the MUSIC directory only
        'returns an empty treeview on error

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        Dim theTreeView As New TreeView

        'get the directory tree from the device
        Dim strTree As String
        strTree = enumerateStorage()
        'strTree = "<0,295176,>Storage Media:<1,295200,Storage Media>DevIcon.fil:<1,295200,Storage Media>DevLogo.fil:<1,295200,Storage Media>capability_00.xml:<1,295176,Storage Media>MUSIC:<2,295176,MUSIC>Angra - Rebirth:<3,295200,Angra - Rebirth>01 - In Excelsis.m4a:<3,295200,Angra - Rebirth>02 - Nova Era.m4a:<3,295200,Angra - Rebirth>03 - Millenium Sun.m4a:<3,295200,Angra - Rebirth>04 - Acid Rain.m4a:<3,295200,Angra - Rebirth>05 - Heroes Of Sand.m4a:<3,295200,Angra - Rebirth>06 - Unholy Wars (Part I_ Imperial Crown; Part II_ Forgiven Return).m4a:<3,295200,Angra - Rebirth>07 - Rebirth.m4a:<3,295200,Angra - Rebirth>08 - Judgement Day.m4a:<3,295200,Angra - Rebirth>09 - Running Alone.m4a:<3,295200,Angra - Rebirth>10 - Visions Prelude.m4a:<2,295176,MUSIC>Falconer - The Sceptre Of Deception:<3,295200,Falconer - The Sceptre Of Deception>01 - The Coronation.m4a:<3,295200,Falconer - The Sceptre Of Deception>02 - The Trail Of Flames.m4a:<3,295200,Falconer - The Sceptre Of Deception>03 - Under The Sword.m4a:<3,295200,Falconer - The Sceptre Of Deception>04 - Night Of Infamy.m4a:<3,295200,Falconer - The Sceptre Of Deception>05 - Hooves Over Northland.m4a:<3,295200,Falconer - The Sceptre Of Deception>06 - Pledge For Freedom.m4a:<3,295200,Falconer - The Sceptre Of Deception>07 - Ravenhair.m4a:<3,295200,Falconer - The Sceptre Of Deception>08 - The Sceptre Of Deception.m4a:<3,295200,Falconer - The Sceptre Of Deception>09 - Hear Me Pray.m4a:<3,295200,Falconer - The Sceptre Of Deception>10 - Child Of Innocence.m4a:<2,295176,MUSIC>Dragonforce - Inhuman Rampage:<3,295200,Dragonforce - Inhuman Rampage>01 - Through The Fire And Flames.m4a:<3,295200,Dragonforce - Inhuman Rampage>02 - Revolution Deathsquad.m4a:<3,295200,Dragonforce - Inhuman Rampage>03 - Storming The Burning Fields.m4a:<3,295200,Dragonforce - Inhuman Rampage>04 - Operation Ground And Pound.m4a:<3,295200,Dragonforce - Inhuman Rampage>05 - Body Breakdown.m4a:<3,295200,Dragonforce - Inhuman Rampage>06 - Cry For Eternity.m4a:<3,295200,Dragonforce - Inhuman Rampage>07 - The Flame Of Youth.m4a:<3,295200,Dragonforce - Inhuman Rampage>08 - Trail Of Broken Hearts.m4a:<3,295200,Dragonforce - Inhuman Rampage>09 - Lost Souls In Endless Time.m4a:<2,295200,MUSIC>Bush - The Chemicals Between Us.mp3:<2,295176,MUSIC>Edu Falaschi - Almah:<3,295200,Edu Falaschi - Almah>01 - King.m4a:<3,295200,Edu Falaschi - Almah>02 - Take Back Your Spell.m4a:<3,295200,Edu Falaschi - Almah>03 - Forgotten Land (Com Edu Ardanu).m4a:<3,295200,Edu Falaschi - Almah>04 - Scary Zone.m4a:<3,295200,Edu Falaschi - Almah>05 - Children Of Lies.m4a:<3,295200,Edu Falaschi - Almah>06 - Break All The Welds.m4a:<3,295200,Edu Falaschi - Almah>07 - Golden Empire.m4a:<3,295200,Edu Falaschi - Almah>08 - Primitive Chaos.m4a:<3,295200,Edu Falaschi - Almah>09 - Breathe.m4a:<3,295200,Edu Falaschi - Almah>10 - Box Of Illusion.m4a:<3,295200,Edu Falaschi - Almah>11 - Almah.m4a:<2,295176,MUSIC>Kiko Loureiro - No Gravity:<3,295200,Kiko Loureiro - No Gravity>10 - In A Gentle Way.m4a:<3,295200,Kiko Loureiro - No Gravity>11 - Dilemma.m4a:<3,295200,Kiko Loureiro - No Gravity>12 - Feliz Desilusão.m4a:<3,295200,Kiko Loureiro - No Gravity>13 - Choro De Crianca.m4a:<3,295200,Kiko Loureiro - No Gravity>01 - Enfermo.m4a:<3,295200,Kiko Loureiro - No Gravity>02 - Endangered Species.m4a:<3,295200,Kiko Loureiro - No Gravity>03 - Escaping.m4a:<3,295200,Kiko Loureiro - No Gravity>04 - No Gravity.m4a:<3,295200,Kiko Loureiro - No Gravity>05 - Pau-De-Arara.m4a:<3,295200,Kiko Loureiro - No Gravity>06 - La Force De L'âme.m4a:<3,295200,Kiko Loureiro - No Gravity>07 - Tapping Into My Dark Tranquility.m4a:<3,295200,Kiko Loureiro - No Gravity>08 - Moment Of Truth.m4a:<3,295200,Kiko Loureiro - No Gravity>09 - Beautiful Language.m4a:<2,295176,MUSIC>MAG Project - MAG Project:<3,295200,MAG Project - MAG Project>11 - Coma.m4a:<3,295200,MAG Project - MAG Project>01 - Cosmic Disturbance.m4a:<3,295200,MAG Project - MAG Project>02 - A Drop In The Sea.m4a:<3,295200,MAG Project - MAG Project>03 - Nowhere In Sight.m4a:<3,295200,MAG Project - MAG Project>04 - More Or Less.m4a:<3,295200,MAG Project - MAG Project>05 - Open Up.m4a:<3,295200,MAG Project - MAG Project>06 - Eternity.m4a:<3,295200,MAG Project - MAG Project>07 - Looking Up.m4a:<3,295200,MAG Project - MAG Project>08 - Storm.m4a:<3,295200,MAG Project - MAG Project>09 - No One Like You.m4a:<3,295200,MAG Project - MAG Project>10 - Highway 80.m4a:<2,295200,MUSIC>testfile.mp3:<2,295200,MUSIC>theforgottenpt2.mp3:<2,295176,MUSIC>Live - Throwing Copper:<3,295200,Live - Throwing Copper>09 - T.B.D..m4a:<3,295200,Live - Throwing Copper>10 - Stage.m4a:<3,295200,Live - Throwing Copper>11 - Waitress.m4a:<3,295200,Live - Throwing Copper>12 - Pillar Of Davidson.m4a:<3,295200,Live - Throwing Copper>13 - White, Discussion.m4a:<3,295200,Live - Throwing Copper>14 - Horse.m4a:<3,295200,Live - Throwing Copper>01 - The Dam At Otter Creek.m4a:<3,295200,Live - Throwing Copper>02 - Selling The Drama.m4a:<3,295200,Live - Throwing Copper>03 - I Alone.m4a:<3,295200,Live - Throwing Copper>04 - Iris.m4a:<3,295200,Live - Throwing Copper>05 - Lightning Crashes.m4a:<3,295200,Live - Throwing Copper>06 - Top.m4a:<3,295200,Live - Throwing Copper>07 - All Over You.m4a:<3,295200,Live - Throwing Copper>08 - Shit Towne.m4a:<2,295176,MUSIC>Megadeth - Youthanasia:<3,295200,Megadeth - Youthanasia>09 - Youthanasia.m4a:<3,295200,Megadeth - Youthanasia>10 - I Thought I Knew It All.m4a:<3,295200,Megadeth - Youthanasia>11 - Black Curtain.m4a:<3,295200,Megadeth - Youthanasia>12 - Victory.m4a:<3,295200,Megadeth - Youthanasia>01 - Reckoning Day.m4a:<3,295200,Megadeth - Youthanasia>02 - Train of Consequences.m4a:<3,295200,Megadeth - Youthanasia>03 - Addicted to Chaos.m4a:<3,295200,Megadeth - Youthanasia>04 - A Tout Le Monde.m4a:<3,295200,Megadeth - Youthanasia>05 - Elysian Fields.m4a:<3,295200,Megadeth - Youthanasia>06 - The Killing Road.m4a:<3,295200,Megadeth - Youthanasia>07 - Blood of Heroes.m4a:<3,295200,Megadeth - Youthanasia>08 - Family Tree.m4a:<2,295176,MUSIC>Metallica - Reload:<3,295200,Metallica - Reload>06 - Slither.m4a:<3,295200,Metallica - Reload>07 - Carpe Diem Baby.m4a:<3,295200,Metallica - Reload>08 - Bad Seed.m4a:<3,295200,Metallica - Reload>09 - Where The Wild Things Are.m4a:<3,295200,Metallica - Reload>10 - Prince Charming.m4a:<3,295200,Metallica - Reload>11 - Low Man's Lyric.m4a:<3,295200,Metallica - Reload>12 - Attitude.m4a:<3,295200,Metallica - Reload>13 - Fixxxer.m4a:<3,295200,Metallica - Reload>01 - Fuel.m4a:<3,295200,Metallica - Reload>02 - The Memory Remains.m4a:<3,295200,Metallica - Reload>03 - Devil's Dance.m4a:<3,295200,Metallica - Reload>04 - The Unforgiven II.m4a:<3,295200,Metallica - Reload>05 - Better Than You.m4a:<2,295176,MUSIC>Joe Satriani - The Extremist:<3,295200,Joe Satriani - The Extremist>05 - Rubina's Blue Sky Happiness.m4a:<3,295200,Joe Satriani - The Extremist>06 - Summer Song.m4a:<3,295200,Joe Satriani - The Extremist>07 - Tears In The Rain.m4a:<3,295200,Joe Satriani - The Extremist>08 - Why.m4a:<3,295200,Joe Satriani - The Extremist>09 - Motorcycle Driver.m4a:<3,295200,Joe Satriani - The Extremist>10 - New Blues.m4a:<3,295200,Joe Satriani - The Extremist>01 - Friends.m4a:<3,295200,Joe Satriani - The Extremist>02 - The Extremist.m4a:<3,295200,Joe Satriani - The Extremist>03 - War.m4a:<3,295200,Joe Satriani - The Extremist>04 - Cryin'.m4a:<2,295176,MUSIC>Angra - Holy Land:<3,295200,Angra - Holy Land>05 - Holy Land.m4a:<3,295200,Angra - Holy Land>06 - The Shaman.m4a:<3,295200,Angra - Holy Land>Angra - Holy Land.jpg:<3,295200,Angra - Holy Land>07 - Make Believe.m4a:<3,295200,Angra - Holy Land>08 - Z.I.T.O..m4a:<3,295200,Angra - Holy Land>09 - Deep Blue.m4a:<3,295200,Angra - Holy Land>10 - Lullaby For Lucifer.m4a:<3,295200,Angra - Holy Land>FOLDER.JPG:<3,295200,Angra - Holy Land>01 - Crossing.m4a:<3,295200,Angra - Holy Land>02 - Nothing To Say.m4a:<3,295200,Angra - Holy Land>03 - Silence And Distance.m4a:<3,295200,Angra - Holy Land>04 - Carolina IV.m4a:<2,295176,MUSIC>Joe Satriani - Joe Satriani:<3,295200,Joe Satriani - Joe Satriani>05 - S.M.F..m4a:<3,295200,Joe Satriani - Joe Satriani>06 - Look My Way.m4a:<3,295200,Joe Satriani - Joe Satriani>07 - Home.m4a:<3,295200,Joe Satriani - Joe Satriani>08 - Moroccan Sunset.m4a:<3,295200,Joe Satriani - Joe Satriani>09 - Killer Bee Bop.m4a:<3,295200,Joe Satriani - Joe Satriani>10 - Slow Down Blues.m4a:<3,295200,Joe Satriani - Joe Satriani>11 - (You're) My World.m4a:<3,295200,Joe Satriani - Joe Satriani>12 - Sittin' Around.m4a:<3,295200,Joe Satriani - Joe Satriani>01 - Cool #9.m4a:<3,295200,Joe Satriani - Joe Satriani>02 - If.m4a:<3,295200,Joe Satriani - Joe Satriani>03 - Down, Down, Down.m4a:<3,295200,Joe Satriani - Joe Satriani>04 - Luminous Flesh Giants.m4a:<2,295176,MUSIC>Dream Theater - A Change Of Seasons EP:<3,295200,Dream Theater - A Change Of Seasons EP>05 - The Big Medley.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>01 - A Change Of Seasons.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>02 - Funeral For A Friend_Love Lies Bleeding.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>03 - Perfect Strangers.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>04 - The Rover_Achilles Last Stand_The Song Remains The Same.m4a:<2,295176,MUSIC>Moist - Creature:<3,295200,Moist - Creature>04 - Leave It Alone.m4a:<3,295200,Moist - Creature>05 - Creature.m4a:<3,295200,Moist - Creature>06 - Shotgun.m4a:<3,295200,Moist - Creature>07 - Disco Days.m4a:<3,295200,Moist - Creature>08 - Tangerine.m4a:<3,295200,Moist - Creature>09 - Better Than You.m4a:<3,295200,Moist - Creature>10 - Baby Skin Tattoo.m4a:<3,295200,Moist - Creature>11 - Ophelia.m4a:<3,295200,Moist - Creature>12 - Gasoline.m4a:<3,295200,Moist - Creature>01 - Hate.m4a:<3,295200,Moist - Creature>02 - Theme From Cola.m4a:<3,295200,Moist - Creature>03 - Resurrection.m4a:<2,295176,MUSIC>Metallica - The Black Album:<3,295200,Metallica - The Black Album>01 - Enter Sandman.m4a:<3,295200,Metallica - The Black Album>02 - Sad But True.m4a:<3,295200,Metallica - The Black Album>03 - Holier Than Thou.m4a:<3,295200,Metallica - The Black Album>04 - The Unforgiven.m4a:<3,295200,Metallica - The Black Album>05 - Wherever I May Roam.m4a:<3,295200,Metallica - The Black Album>06 - Don't Tread On Me.m4a:<3,295200,Metallica - The Black Album>07 - Through The Never.m4a:<3,295200,Metallica - The Black Album>08 - Nothing Else Matters.m4a:<3,295200,Metallica - The Black Album>09 - Of Wolf And Man.m4a:<3,295200,Metallica - The Black Album>10 - The God That Failed.m4a:<3,295200,Metallica - The Black Album>11 - My Friend Of Misery.m4a:<3,295200,Metallica - The Black Album>12 - The Struggle Within.m4a:<1,295176,Storage Media>DCIM:<1,295176,Storage Media>PICTURE:<1,295176,Storage Media>PICTURES:<2,295176,PICTURES>autoshow:<3,295200,autoshow>DSC00014.JPG:<3,295200,autoshow>DSC00015.JPG:<3,295200,autoshow>DSC00016.JPG:<3,295200,autoshow>DSC00017.JPG:<3,295200,autoshow>DSC00018.JPG:<3,295200,autoshow>DSC00019.JPG:<3,295200,autoshow>DSC00021.JPG:<3,295200,autoshow>DSC00020.JPG:<3,295200,autoshow>DSC00003.JPG:<3,295200,autoshow>DSC00004.JPG:<3,295200,autoshow>DSC00005.JPG:<3,295200,autoshow>DSC00006.JPG:<3,295200,autoshow>DSC00007.JPG:<3,295200,autoshow>DSC00008.JPG:<3,295200,autoshow>DSC00009.JPG:<3,295200,autoshow>DSC00010.JPG:<3,295200,autoshow>DSC00011.JPG:<3,295200,autoshow>DSC00012.JPG:<3,295200,autoshow>DSC00013.JPG:<1,295176,Storage Media>VIDEO:<2,295200,VIDEO>test.mp4:<2,295200,VIDEO>subtest.mp4:<2,295200,VIDEO>Reboot 2x09 Trust No One (Dinothunderblack).mp4:<2,295200,VIDEO>Blue Harvest.JPG:<2,295200,VIDEO>Blue Harvest.mp4:<1,295176,Storage Media>MP_ROOT:<1,295200,Storage Media>WMPInfo.xml:<1,295176,Storage Media>Albums"
        If Not strTree = "-1" Then
            Try
                'get the full tree first
                Dim tmpTree As TreeView
                tmpTree = buildTreeViewFromDirectoryTree(strTree)

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
            End Try

        End If

        Return theTreeView
    End Function
    Public Function getDeviceManufacturer() As String
        'gets the manufacturer of the current device
        'this function returns "-1" on error
        'MTPAxe returns -1 on error, name otherwise

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_GETMANUFACTURER)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function getDeviceType() As String
        'gets the attributes of the current device
        'this function returns "-1" on error
        'MTPAxe returns -1 on error, ';' separated list otherwise

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_GETTYPE)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
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

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_ENUMERATESTORAGE)
        'MTPAxe will now be expecting the devicename parameter

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Private Function buildTreeViewFromDirectoryTree(ByVal directoryTree As String) As TreeView
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
        Dim nodeLevel As Integer, nodeType As Integer, nodeParent As String
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
                                'the parent was not foundfor some reason. error building the tree
                                Throw New Exception("Error building treeview")
                                Trace.WriteLine("error building treeview")
                                Exit For
                            End If
                        Next
                        'now that we have the parent, add to it it's child
                        parent.Nodes.Add(tn)

                    End If

                End If
            Next
        Next

        Return treeview1
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

    Public Function createPlaylist(ByVal playlistName As String, ByVal items As String) As String
        'creates a playlist. items are in the same format as returned by enumerateStorage
        'this function returns "-1" on error otherwise
        'MTPAxe returns -1 on error, 0 otherwise

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_CREATEPLAYLIST)
        sOut.WriteLine(playlistName)
        sOut.WriteLine(items)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function deletePlaylist(ByVal playlistName As String) As String
        'gets the attributes of the current device
        'this function returns "-1" on error otherwise
        'MTPAxe returns -1 on error, 0 otherwise

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_DEVICE_DELETEPLAYLIST)
        sOut.WriteLine(playlistName)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Private Function enumeratePlaylist(ByVal name As String)
        'gets the contents of a playlist in the same format as enumerateStorage
        'this function returns "-1" on error otherwise
        'MTPAxe returns -1 on error, 0 otherwise

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_PLAYLIST_ENUMERATECONTENTS)
        sOut.WriteLine(name)

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function getPlaylistContentsAsTreeview(ByVal name As String) As TreeView
        'returns the full directory structure of the device

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        Dim theTreeView As TreeView = Nothing

        'get the directory tree from the device
        Dim strTree As String
        strTree = enumeratePlaylist(name)
        'strTree = "<0,295176,>Storage Media:<1,295200,Storage Media>DevIcon.fil:<1,295200,Storage Media>DevLogo.fil:<1,295200,Storage Media>capability_00.xml:<1,295176,Storage Media>MUSIC:<2,295176,MUSIC>Angra - Rebirth:<3,295200,Angra - Rebirth>01 - In Excelsis.m4a:<3,295200,Angra - Rebirth>02 - Nova Era.m4a:<3,295200,Angra - Rebirth>03 - Millenium Sun.m4a:<3,295200,Angra - Rebirth>04 - Acid Rain.m4a:<3,295200,Angra - Rebirth>05 - Heroes Of Sand.m4a:<3,295200,Angra - Rebirth>06 - Unholy Wars (Part I_ Imperial Crown; Part II_ Forgiven Return).m4a:<3,295200,Angra - Rebirth>07 - Rebirth.m4a:<3,295200,Angra - Rebirth>08 - Judgement Day.m4a:<3,295200,Angra - Rebirth>09 - Running Alone.m4a:<3,295200,Angra - Rebirth>10 - Visions Prelude.m4a:<2,295176,MUSIC>Falconer - The Sceptre Of Deception:<3,295200,Falconer - The Sceptre Of Deception>01 - The Coronation.m4a:<3,295200,Falconer - The Sceptre Of Deception>02 - The Trail Of Flames.m4a:<3,295200,Falconer - The Sceptre Of Deception>03 - Under The Sword.m4a:<3,295200,Falconer - The Sceptre Of Deception>04 - Night Of Infamy.m4a:<3,295200,Falconer - The Sceptre Of Deception>05 - Hooves Over Northland.m4a:<3,295200,Falconer - The Sceptre Of Deception>06 - Pledge For Freedom.m4a:<3,295200,Falconer - The Sceptre Of Deception>07 - Ravenhair.m4a:<3,295200,Falconer - The Sceptre Of Deception>08 - The Sceptre Of Deception.m4a:<3,295200,Falconer - The Sceptre Of Deception>09 - Hear Me Pray.m4a:<3,295200,Falconer - The Sceptre Of Deception>10 - Child Of Innocence.m4a:<2,295176,MUSIC>Dragonforce - Inhuman Rampage:<3,295200,Dragonforce - Inhuman Rampage>01 - Through The Fire And Flames.m4a:<3,295200,Dragonforce - Inhuman Rampage>02 - Revolution Deathsquad.m4a:<3,295200,Dragonforce - Inhuman Rampage>03 - Storming The Burning Fields.m4a:<3,295200,Dragonforce - Inhuman Rampage>04 - Operation Ground And Pound.m4a:<3,295200,Dragonforce - Inhuman Rampage>05 - Body Breakdown.m4a:<3,295200,Dragonforce - Inhuman Rampage>06 - Cry For Eternity.m4a:<3,295200,Dragonforce - Inhuman Rampage>07 - The Flame Of Youth.m4a:<3,295200,Dragonforce - Inhuman Rampage>08 - Trail Of Broken Hearts.m4a:<3,295200,Dragonforce - Inhuman Rampage>09 - Lost Souls In Endless Time.m4a:<2,295200,MUSIC>Bush - The Chemicals Between Us.mp3:<2,295176,MUSIC>Edu Falaschi - Almah:<3,295200,Edu Falaschi - Almah>01 - King.m4a:<3,295200,Edu Falaschi - Almah>02 - Take Back Your Spell.m4a:<3,295200,Edu Falaschi - Almah>03 - Forgotten Land (Com Edu Ardanu).m4a:<3,295200,Edu Falaschi - Almah>04 - Scary Zone.m4a:<3,295200,Edu Falaschi - Almah>05 - Children Of Lies.m4a:<3,295200,Edu Falaschi - Almah>06 - Break All The Welds.m4a:<3,295200,Edu Falaschi - Almah>07 - Golden Empire.m4a:<3,295200,Edu Falaschi - Almah>08 - Primitive Chaos.m4a:<3,295200,Edu Falaschi - Almah>09 - Breathe.m4a:<3,295200,Edu Falaschi - Almah>10 - Box Of Illusion.m4a:<3,295200,Edu Falaschi - Almah>11 - Almah.m4a:<2,295176,MUSIC>Kiko Loureiro - No Gravity:<3,295200,Kiko Loureiro - No Gravity>10 - In A Gentle Way.m4a:<3,295200,Kiko Loureiro - No Gravity>11 - Dilemma.m4a:<3,295200,Kiko Loureiro - No Gravity>12 - Feliz Desilusão.m4a:<3,295200,Kiko Loureiro - No Gravity>13 - Choro De Crianca.m4a:<3,295200,Kiko Loureiro - No Gravity>01 - Enfermo.m4a:<3,295200,Kiko Loureiro - No Gravity>02 - Endangered Species.m4a:<3,295200,Kiko Loureiro - No Gravity>03 - Escaping.m4a:<3,295200,Kiko Loureiro - No Gravity>04 - No Gravity.m4a:<3,295200,Kiko Loureiro - No Gravity>05 - Pau-De-Arara.m4a:<3,295200,Kiko Loureiro - No Gravity>06 - La Force De L'âme.m4a:<3,295200,Kiko Loureiro - No Gravity>07 - Tapping Into My Dark Tranquility.m4a:<3,295200,Kiko Loureiro - No Gravity>08 - Moment Of Truth.m4a:<3,295200,Kiko Loureiro - No Gravity>09 - Beautiful Language.m4a:<2,295176,MUSIC>MAG Project - MAG Project:<3,295200,MAG Project - MAG Project>11 - Coma.m4a:<3,295200,MAG Project - MAG Project>01 - Cosmic Disturbance.m4a:<3,295200,MAG Project - MAG Project>02 - A Drop In The Sea.m4a:<3,295200,MAG Project - MAG Project>03 - Nowhere In Sight.m4a:<3,295200,MAG Project - MAG Project>04 - More Or Less.m4a:<3,295200,MAG Project - MAG Project>05 - Open Up.m4a:<3,295200,MAG Project - MAG Project>06 - Eternity.m4a:<3,295200,MAG Project - MAG Project>07 - Looking Up.m4a:<3,295200,MAG Project - MAG Project>08 - Storm.m4a:<3,295200,MAG Project - MAG Project>09 - No One Like You.m4a:<3,295200,MAG Project - MAG Project>10 - Highway 80.m4a:<2,295200,MUSIC>testfile.mp3:<2,295200,MUSIC>theforgottenpt2.mp3:<2,295176,MUSIC>Live - Throwing Copper:<3,295200,Live - Throwing Copper>09 - T.B.D..m4a:<3,295200,Live - Throwing Copper>10 - Stage.m4a:<3,295200,Live - Throwing Copper>11 - Waitress.m4a:<3,295200,Live - Throwing Copper>12 - Pillar Of Davidson.m4a:<3,295200,Live - Throwing Copper>13 - White, Discussion.m4a:<3,295200,Live - Throwing Copper>14 - Horse.m4a:<3,295200,Live - Throwing Copper>01 - The Dam At Otter Creek.m4a:<3,295200,Live - Throwing Copper>02 - Selling The Drama.m4a:<3,295200,Live - Throwing Copper>03 - I Alone.m4a:<3,295200,Live - Throwing Copper>04 - Iris.m4a:<3,295200,Live - Throwing Copper>05 - Lightning Crashes.m4a:<3,295200,Live - Throwing Copper>06 - Top.m4a:<3,295200,Live - Throwing Copper>07 - All Over You.m4a:<3,295200,Live - Throwing Copper>08 - Shit Towne.m4a:<2,295176,MUSIC>Megadeth - Youthanasia:<3,295200,Megadeth - Youthanasia>09 - Youthanasia.m4a:<3,295200,Megadeth - Youthanasia>10 - I Thought I Knew It All.m4a:<3,295200,Megadeth - Youthanasia>11 - Black Curtain.m4a:<3,295200,Megadeth - Youthanasia>12 - Victory.m4a:<3,295200,Megadeth - Youthanasia>01 - Reckoning Day.m4a:<3,295200,Megadeth - Youthanasia>02 - Train of Consequences.m4a:<3,295200,Megadeth - Youthanasia>03 - Addicted to Chaos.m4a:<3,295200,Megadeth - Youthanasia>04 - A Tout Le Monde.m4a:<3,295200,Megadeth - Youthanasia>05 - Elysian Fields.m4a:<3,295200,Megadeth - Youthanasia>06 - The Killing Road.m4a:<3,295200,Megadeth - Youthanasia>07 - Blood of Heroes.m4a:<3,295200,Megadeth - Youthanasia>08 - Family Tree.m4a:<2,295176,MUSIC>Metallica - Reload:<3,295200,Metallica - Reload>06 - Slither.m4a:<3,295200,Metallica - Reload>07 - Carpe Diem Baby.m4a:<3,295200,Metallica - Reload>08 - Bad Seed.m4a:<3,295200,Metallica - Reload>09 - Where The Wild Things Are.m4a:<3,295200,Metallica - Reload>10 - Prince Charming.m4a:<3,295200,Metallica - Reload>11 - Low Man's Lyric.m4a:<3,295200,Metallica - Reload>12 - Attitude.m4a:<3,295200,Metallica - Reload>13 - Fixxxer.m4a:<3,295200,Metallica - Reload>01 - Fuel.m4a:<3,295200,Metallica - Reload>02 - The Memory Remains.m4a:<3,295200,Metallica - Reload>03 - Devil's Dance.m4a:<3,295200,Metallica - Reload>04 - The Unforgiven II.m4a:<3,295200,Metallica - Reload>05 - Better Than You.m4a:<2,295176,MUSIC>Joe Satriani - The Extremist:<3,295200,Joe Satriani - The Extremist>05 - Rubina's Blue Sky Happiness.m4a:<3,295200,Joe Satriani - The Extremist>06 - Summer Song.m4a:<3,295200,Joe Satriani - The Extremist>07 - Tears In The Rain.m4a:<3,295200,Joe Satriani - The Extremist>08 - Why.m4a:<3,295200,Joe Satriani - The Extremist>09 - Motorcycle Driver.m4a:<3,295200,Joe Satriani - The Extremist>10 - New Blues.m4a:<3,295200,Joe Satriani - The Extremist>01 - Friends.m4a:<3,295200,Joe Satriani - The Extremist>02 - The Extremist.m4a:<3,295200,Joe Satriani - The Extremist>03 - War.m4a:<3,295200,Joe Satriani - The Extremist>04 - Cryin'.m4a:<2,295176,MUSIC>Angra - Holy Land:<3,295200,Angra - Holy Land>05 - Holy Land.m4a:<3,295200,Angra - Holy Land>06 - The Shaman.m4a:<3,295200,Angra - Holy Land>Angra - Holy Land.jpg:<3,295200,Angra - Holy Land>07 - Make Believe.m4a:<3,295200,Angra - Holy Land>08 - Z.I.T.O..m4a:<3,295200,Angra - Holy Land>09 - Deep Blue.m4a:<3,295200,Angra - Holy Land>10 - Lullaby For Lucifer.m4a:<3,295200,Angra - Holy Land>FOLDER.JPG:<3,295200,Angra - Holy Land>01 - Crossing.m4a:<3,295200,Angra - Holy Land>02 - Nothing To Say.m4a:<3,295200,Angra - Holy Land>03 - Silence And Distance.m4a:<3,295200,Angra - Holy Land>04 - Carolina IV.m4a:<2,295176,MUSIC>Joe Satriani - Joe Satriani:<3,295200,Joe Satriani - Joe Satriani>05 - S.M.F..m4a:<3,295200,Joe Satriani - Joe Satriani>06 - Look My Way.m4a:<3,295200,Joe Satriani - Joe Satriani>07 - Home.m4a:<3,295200,Joe Satriani - Joe Satriani>08 - Moroccan Sunset.m4a:<3,295200,Joe Satriani - Joe Satriani>09 - Killer Bee Bop.m4a:<3,295200,Joe Satriani - Joe Satriani>10 - Slow Down Blues.m4a:<3,295200,Joe Satriani - Joe Satriani>11 - (You're) My World.m4a:<3,295200,Joe Satriani - Joe Satriani>12 - Sittin' Around.m4a:<3,295200,Joe Satriani - Joe Satriani>01 - Cool #9.m4a:<3,295200,Joe Satriani - Joe Satriani>02 - If.m4a:<3,295200,Joe Satriani - Joe Satriani>03 - Down, Down, Down.m4a:<3,295200,Joe Satriani - Joe Satriani>04 - Luminous Flesh Giants.m4a:<2,295176,MUSIC>Dream Theater - A Change Of Seasons EP:<3,295200,Dream Theater - A Change Of Seasons EP>05 - The Big Medley.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>01 - A Change Of Seasons.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>02 - Funeral For A Friend_Love Lies Bleeding.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>03 - Perfect Strangers.m4a:<3,295200,Dream Theater - A Change Of Seasons EP>04 - The Rover_Achilles Last Stand_The Song Remains The Same.m4a:<2,295176,MUSIC>Moist - Creature:<3,295200,Moist - Creature>04 - Leave It Alone.m4a:<3,295200,Moist - Creature>05 - Creature.m4a:<3,295200,Moist - Creature>06 - Shotgun.m4a:<3,295200,Moist - Creature>07 - Disco Days.m4a:<3,295200,Moist - Creature>08 - Tangerine.m4a:<3,295200,Moist - Creature>09 - Better Than You.m4a:<3,295200,Moist - Creature>10 - Baby Skin Tattoo.m4a:<3,295200,Moist - Creature>11 - Ophelia.m4a:<3,295200,Moist - Creature>12 - Gasoline.m4a:<3,295200,Moist - Creature>01 - Hate.m4a:<3,295200,Moist - Creature>02 - Theme From Cola.m4a:<3,295200,Moist - Creature>03 - Resurrection.m4a:<2,295176,MUSIC>Metallica - The Black Album:<3,295200,Metallica - The Black Album>01 - Enter Sandman.m4a:<3,295200,Metallica - The Black Album>02 - Sad But True.m4a:<3,295200,Metallica - The Black Album>03 - Holier Than Thou.m4a:<3,295200,Metallica - The Black Album>04 - The Unforgiven.m4a:<3,295200,Metallica - The Black Album>05 - Wherever I May Roam.m4a:<3,295200,Metallica - The Black Album>06 - Don't Tread On Me.m4a:<3,295200,Metallica - The Black Album>07 - Through The Never.m4a:<3,295200,Metallica - The Black Album>08 - Nothing Else Matters.m4a:<3,295200,Metallica - The Black Album>09 - Of Wolf And Man.m4a:<3,295200,Metallica - The Black Album>10 - The God That Failed.m4a:<3,295200,Metallica - The Black Album>11 - My Friend Of Misery.m4a:<3,295200,Metallica - The Black Album>12 - The Struggle Within.m4a:<1,295176,Storage Media>DCIM:<1,295176,Storage Media>PICTURE:<1,295176,Storage Media>PICTURES:<2,295176,PICTURES>autoshow:<3,295200,autoshow>DSC00014.JPG:<3,295200,autoshow>DSC00015.JPG:<3,295200,autoshow>DSC00016.JPG:<3,295200,autoshow>DSC00017.JPG:<3,295200,autoshow>DSC00018.JPG:<3,295200,autoshow>DSC00019.JPG:<3,295200,autoshow>DSC00021.JPG:<3,295200,autoshow>DSC00020.JPG:<3,295200,autoshow>DSC00003.JPG:<3,295200,autoshow>DSC00004.JPG:<3,295200,autoshow>DSC00005.JPG:<3,295200,autoshow>DSC00006.JPG:<3,295200,autoshow>DSC00007.JPG:<3,295200,autoshow>DSC00008.JPG:<3,295200,autoshow>DSC00009.JPG:<3,295200,autoshow>DSC00010.JPG:<3,295200,autoshow>DSC00011.JPG:<3,295200,autoshow>DSC00012.JPG:<3,295200,autoshow>DSC00013.JPG:<1,295176,Storage Media>VIDEO:<2,295200,VIDEO>test.mp4:<2,295200,VIDEO>subtest.mp4:<2,295200,VIDEO>Reboot 2x09 Trust No One (Dinothunderblack).mp4:<2,295200,VIDEO>Blue Harvest.JPG:<2,295200,VIDEO>Blue Harvest.mp4:<1,295176,Storage Media>MP_ROOT:<1,295200,Storage Media>WMPInfo.xml:<1,295176,Storage Media>Albums"
        If Not strTree = "-1" Then
            Try
                theTreeView = buildTreeViewFromDirectoryTree(strTree)
            Catch ex As Exception
                theTreeView = New TreeView
            End Try
        End If

        Return theTreeView
    End Function
    '*****************************************************************************************

    '*****************************************************************************************
    'enumerate storage must be called prior to using the following functions in this section
    Public Function getDeviceIcon(ByVal savePath As String) As String
        'gets the device icon
        'this function returns "-1" on error
        'MTPAxe returns -1 on error, ';' separated list otherwise

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")


        sOut.WriteLine(MTPAXE_M_DEVICE_GETICON)
        'MTPAxe will now be expecting the devicename parameters
        sOut.WriteLine(savePath)

        'now wait for the return value to be sent to the buffer.  the return value
        'of MTPAxe for getDeviceIcon is the hICON handle
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    Public Function getDeviceCapacity() As String
        'returns the capacity of the current device and free space as a : separated string, -1 on error

        Dim s As String

        If axe Is Nothing Then Throw New Exception("MTPAxe is not started")

        sOut.WriteLine(MTPAXE_M_STORAGE_GETSIZEINFO)
        'MTPAxe will now be expecting the devicename parameter

        'now wait for the return value to be sent to the buffer
        s = sIn.ReadLine

        If s = "-1" Then
            Trace.WriteLine(sErr.ReadLine)
            Return "-1"
        End If

        Return s
    End Function
    '*****************************************************************************************
#End Region







End Class
