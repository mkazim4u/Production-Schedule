Imports System.ComponentModel
Imports System.IO
Imports System.Diagnostics

Namespace NEvoWeb.Modules.NB_Store

    Public Class LogText
        Inherits System.ComponentModel.Component

        Private Const DEFAULT_APP_NAME As String = "TextLogger"
        Private psFileName As String
        Private psAppName As String


        Public Sub New()
            MyBase.New()

            'This call is required by the Component Designer.
            InitializeComponent()

            ' TODO: Add any initialization after the InitializeComponent() call

        End Sub
        Property FileName() As String
            'Name of File to write to
            'Pass in a valid full path
            'otherwise a default will be used
            Get
                FileName = ResolveFileName()

            End Get
            Set(ByVal Value As String)
                psFileName = Value
            End Set
        End Property
        Property AppName() As String
            'Set for writing to the event log
            'To write to the event log, you 
            'have to create a source

            'The AppName property is used to set
            'the name of the source

            'If not supplied, a default source
            'name will be used
            Get
                AppName = psAppName
            End Get
            Set(ByVal Value As String)
                psAppName = Value
            End Set
        End Property

#Region " Component Designer generated code "

        'Required by the Component Designer
        Private components As Container

        'NOTE: The following procedure is required by the Component Designer
        'It can be modified using the Component Designer.  
        'Do not modify it using the code editor.
        Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
        End Sub

#End Region


        Private Function ResolveFileName() As String
            Dim sDefault As String
            Dim sFileName As String
            Dim sPath As String
            Dim sDirectory As String


            sPath = Environment.CurrentDirectory
            If Right(sPath, 1) <> "\" Then sPath = sPath & "\"
            sDefault = sPath & "Log.txt"

            If psFileName = "" Then
                ResolveFileName = sDefault
                Exit Function
            End If

            If Not File.Exists(psFileName) Then
                Try
                    Dim oInfo As New FileInfo(psFileName)
                    sDirectory = oInfo.DirectoryName

                    If Not Directory.Exists(sDirectory) Then
                        Directory.CreateDirectory(sDirectory)
                        sFileName = psFileName
                    End If
                    sFileName = psFileName

                Catch Ex As Exception

                    sFileName = sDefault
                End Try
            Else
                sFileName = psFileName
            End If

            ResolveFileName = sFileName


        End Function

        Public Sub Log(ByVal TextValue As String)
            'will append to file specified by filename property.
            'if that file is invalid, will write to a default file
            Try
                Dim sFileName As String
                Dim objWriter As StreamWriter
                sFileName = ResolveFileName()
                If Not File.Exists(sFileName) Then
                    objWriter = File.CreateText(sFileName)
                Else
                    objWriter = File.AppendText(sFileName)
                End If
                Try
                    objWriter.WriteLine(TextValue)
                    objWriter.Close()
                Catch Ex As Exception
                    Err.Raise(Err.Number, , Ex.Message)
                End Try
            Catch ex As Exception
                'maybe running in medium trust, so ignore
            End Try

        End Sub

        Public Function WriteToEventLog(ByVal Entry As String, _
                   Optional ByVal EventType As EventLogEntryType = EventLogEntryType.Information) As Boolean
            'Writes Entry to Application Event Log

            Dim sSourceName As String
            Dim objEventLog As New EventLog

            sSourceName = IIf(Trim(psAppName) = "", DEFAULT_APP_NAME, psAppName).ToString
            Try
                'Register the App as an Event Source
                If Not objEventLog.SourceExists(sSourceName) Then

                    objEventLog.CreateEventSource(sSourceName, "Application")
                End If
                objEventLog.Source = sSourceName
                objEventLog.WriteEntry(Entry, EventType)


                Return True
            Catch
                Return False

            End Try

        End Function
        Public Sub LogError(ByVal Ex As Exception, Optional ByVal ProcedureName As String = "")


            Dim sAns As String
            sAns = "Exception " & Ex.Message & " occurred"
            If ProcedureName <> "" Then
                sAns = sAns & " in " & ProcedureName
            End If

            sAns = sAns & " [" & Now.ToLongDateString & "]"

            Log(sAns)
        End Sub

        Public Sub Reset()
            'Deletes log and starts all over
            Dim sFileName As String

            sFileName = ResolveFileName()
            Try
                File.Delete(sFileName)
            Catch

            End Try


        End Sub

    End Class

End Namespace






