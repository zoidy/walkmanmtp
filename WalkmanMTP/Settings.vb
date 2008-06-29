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

Imports System.IO
Public Class Settings


    'the class to manage writing/reading settings
    Private theSettings As New ConfigOpt

    Public Sub New()
        'the settings are loaded when a new instance of this class is created
        theSettings.Initialize(Path.Combine(Application.StartupPath, "settings.config"))
    End Sub
    Public Sub save()
        theSettings.Store()
    End Sub

#Region "setings"

    Public Property MainFormWindowState() As FormWindowState
        Get
            Dim str As String = theSettings.GetOption("MainFormWindowState")

            'the default option is window state normal
            If str = "" Then
                MainFormWindowState = FormWindowState.Normal
            Else
                MainFormWindowState = str
            End If

        End Get
        Set(ByVal value As FormWindowState)
            theSettings.SetOption("MainFormWindowState", value)
        End Set
    End Property
    Public Property MainFormWindowWidth() As Integer
        Get
            Dim str As String = theSettings.GetOption("MainFormWindowWidth")

            'the default window width is 925px
            If str = "" Then
                MainFormWindowWidth = 925
            Else
                MainFormWindowWidth = str
            End If
        End Get
        Set(ByVal value As Integer)
            theSettings.SetOption("MainFormWindowWidth", value)
        End Set
    End Property
    Public Property MainFormWindowHeight() As Integer
        Get
            Dim str As String = theSettings.GetOption("MainFormWindowHeight")

            'the default window width is 636px
            If str = "" Then
                MainFormWindowHeight = 636
            Else
                MainFormWindowHeight = str
            End If
        End Get
        Set(ByVal value As Integer)
            theSettings.SetOption("MainFormWindowHeight", value)
        End Set
    End Property

    ''' <summary>
    ''' the value of the selected device combo box
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectedDevice() As String
        Get
            'if the setting doesn't exist, the default of WALKMAN is returned
            Dim str As String = theSettings.GetOption("SelectedDevice")

            If str = "" Then
                SelectedDevice = "WALKMAN"
            Else
                SelectedDevice = str
            End If
        End Get
        Set(ByVal value As String)
            theSettings.SetOption("SelectedDevice", value)
        End Set
    End Property

    Public Property ShowDeviceIcon() As Boolean
        Get
            Dim str As String = theSettings.GetOption("ShowDeviceIcon")

            If str = "" Then
                ShowDeviceIcon = True
            Else
                ShowDeviceIcon = str
            End If
        End Get
        Set(ByVal value As Boolean)
            theSettings.SetOption("ShowDeviceIcon", value)
        End Set
    End Property
    ''' <summary>
    ''' used for setting the checkbox of the same name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeleteAlbumSongsOnAlbumDelete() As Boolean
        Get
            Dim str As String = theSettings.GetOption("DeleteAlbumSongsOnAlbumDelete")

            If str = "" Then
                DeleteAlbumSongsOnAlbumDelete = True
            Else
                ShowDeviceIcon = str
            End If
        End Get
        Set(ByVal value As Boolean)
            theSettings.SetOption("DeleteAlbumSongsOnAlbumDelete", value)
        End Set
    End Property

#End Region




    ' Class for managing configuration persistence
    'http://www.codeproject.com/KB/vb/ConfigOpt.aspx
    'modified by dr. zoidberg
    Private Class ConfigOpt

        ' This DataSet is used as a memory data
        ' structure to hold config key/value pairs
        ' Inside this DataSet, a single DataTable named ConfigValues is created
        Private DSoptions As DataSet

        ' This is the filename for the DataSet XML serialization
        Private mConfigFileName As String

        ' This property is read-only, because it is set
        ' through Initialize or Store methods
        Public ReadOnly Property ConfigFileName() As String
            Get
                Return mConfigFileName
            End Get
        End Property

        ' This method has to be invoked before using
        ' any other method of ConfigOpt class
        ' ConfigFile parameter is the name of the config file to be read
        ' (if that file doesn't exists, the method
        ' simply initialize the data structure
        ' and the ConfigFileName property)
        Public Sub Initialize(ByVal ConfigFile As String)
            mConfigFileName = ConfigFile
            DSoptions = New DataSet("ConfigOpt")
            If File.Exists(ConfigFile) Then
                ' If the specified config file exists,
                ' it is read to populate the DataSet
                Try
                    DSoptions.ReadXml(ConfigFile)
                Catch ex As Exception
                    Trace.WriteLine("Couldn't read settings " & ConfigFile & " - " & ex.Message)
                End Try
            Else
                ' If the specified config file doesn't exists, 
                ' the DataSet is simply initialized (and left empty):
                ' the ConfigValues DataTable is created
                ' with two fields (to hold key/values pairs)
                Dim dt As New DataTable("ConfigValues")
                dt.Columns.Add("OptionName", System.Type.GetType("System.String"))
                dt.Columns.Add("OptionValue", System.Type.GetType("System.String"))
                dt.Columns("OptionName").ColumnMapping = MappingType.Attribute
                dt.Columns("OptionValue").ColumnMapping = MappingType.SimpleContent
                DSoptions.Tables.Add(dt)
            End If
        End Sub

        ' This method serializes the memory data
        ' structure holding the config parameters
        ' The filename used is the one defined calling Initialize method
        Public Sub Store()
            Store(mConfigFileName)
        End Sub

        ' Same as Store() method, but with the ability
        ' to serialize on a different filename
        Public Sub Store(ByVal ConfigFile As String)
            mConfigFileName = ConfigFile
            Try
                DSoptions.WriteXml(ConfigFile)
            Catch ex As Exception
                Trace.WriteLine("Couldn't save settings " & ConfigFile & " - " & ex.Message)
            End Try
        End Sub

        ' Read a configuration Value (aka OptionValue),
        ' given its Key (aka OptionName)
        ' If the Key is not defined, an empty string is returned
        Public Function GetOption(ByVal OptionName As String) As String
            Dim dv As DataView = DSoptions.Tables("ConfigValues").DefaultView
            dv.RowFilter = "OptionName='" & OptionName & "'"
            If dv.Count > 0 Then
                Try
                    Return CStr(dv.Item(0).Item(1))
                Catch ex As Exception
                    Trace.WriteLine("Error reading setting '" & OptionName & "' - " & ex.Message)
                    Return ""
                End Try
            Else
                Return ""
            End If
        End Function

        ' Write in the memory data structure a Key/Value
        ' pair for a configuration setting
        ' If the Key already exists, the Value is simply updated,
        ' else the Key/Value pair is added
        ' Warning: to update the written Key/Value pair
        ' on the config file, you need to call Store
        Public Sub SetOption(ByVal OptionName As String, ByVal OptionValue As String)
            Dim dv As DataView = DSoptions.Tables("ConfigValues").DefaultView
            dv.RowFilter = "OptionName='" & OptionName & "'"
            If dv.Count > 0 Then
                dv.Item(0).Item(1) = OptionValue
            Else
                Dim dr As DataRow = DSoptions.Tables("ConfigValues").NewRow()
                dr("OptionName") = OptionName
                dr(1) = OptionValue
                DSoptions.Tables("ConfigValues").Rows.Add(dr)
            End If
        End Sub

    End Class


End Class
