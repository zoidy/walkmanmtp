Public Class StorageItem

    Private mLevel As Integer
    Private mtype As Integer
    Private mParentFilename As String
    Private mSize As ULong
    Private mTitle As String
    Private mAlbumArtist As String
    Private mAlbumTitle As String
    Private mGenre As String
    Private mYear As String
    Private mTracknum As String
    Private mFileName As String
    Private mID As String


    Public Property DirectoryDepth() As Integer
        Get
            DirectoryDepth = mLevel
        End Get
        Set(ByVal value As Integer)
            mLevel = value
        End Set
    End Property
    Public Property StorageType() As Integer
        Get
            StorageType = mtype
        End Get
        Set(ByVal value As Integer)
            mtype = value
        End Set
    End Property
    Public Property ParentFileName() As String
        Get
            ParentFileName = mParentFilename
        End Get
        Set(ByVal value As String)
            mParentFilename = value
        End Set
    End Property
    Public Property Size() As ULong
        Get
            Size = mSize
        End Get
        Set(ByVal value As ULong)
            mSize = value
        End Set
    End Property
    Public Property Title() As String
        Get
            Title = mTitle
        End Get
        Set(ByVal value As String)
            mTitle = value
        End Set
    End Property
    Public Property AlbumArtist() As String
        Get
            AlbumArtist = mAlbumArtist
        End Get
        Set(ByVal value As String)
            mAlbumArtist = value
        End Set
    End Property
    Public Property AlbumTitle() As String
        Get
            AlbumTitle = mAlbumTitle
        End Get
        Set(ByVal value As String)
            mAlbumTitle = value
        End Set
    End Property
    Public Property Genre() As String
        Get
            Genre = mGenre
        End Get
        Set(ByVal value As String)
            mGenre = value
        End Set
    End Property
    Public Property Year() As String
        Get
            Year = mYear
        End Get
        Set(ByVal value As String)
            mYear = value
        End Set
    End Property
    Public Property TrackNum() As String
        Get
            TrackNum = mTracknum
        End Get
        Set(ByVal value As String)
            'check for valid values
            Dim tmp As Integer
            Integer.TryParse(value, tmp)
            'tmp now contains the integer representation of value, or 0 if conversion failed
            If tmp = 0 Then
                mTracknum = ""
            Else
                mTracknum = Format(tmp, "00")
            End If
        End Set
    End Property
    Public Property FileName() As String
        Get
            FileName = mFileName
        End Get
        Set(ByVal value As String)
            mFileName = value
        End Set
    End Property
    Public Property ID() As String
        Get
            ID = mID
        End Get
        Set(ByVal value As String)
            mID = value
        End Set
    End Property




    Public Sub New()
        mLevel = 0
        mtype = 0
        mParentFilename = ""
        mSize = 0
        mTitle = ""
        mAlbumArtist = ""
        mAlbumTitle = ""
        mGenre = ""
        mYear = ""
        mTracknum = ""
        mFileName = ""
        mID = ""
    End Sub
End Class
