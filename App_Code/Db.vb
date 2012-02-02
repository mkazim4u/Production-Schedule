Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data
Imports System.Data.Common
Imports System.Timers
Imports System.Data.SqlClient

Public Class Db
    '	-----------------------------------------------------------------------------
    '	This template is used to generate a single class for a specific database.
    '	It contains several general-purpose methods used by other classes.
    '
    '	Created:		22 Jul 2010 21:53:56
    '	Server:			VOSTRO
    '	Database:		
    '	Description:	DNN542c
    '	Database type:	SqlConnection
    '	User name:		sa
    '	Password:		rugby22
    '	File location:	
    '	Table:			FFJob
    '	Connection:		Data Source=VOSTRO;Initial Catalog=DNN542c;User ID=sa;Password=rugby22
    '	-----------------------------------------------------------------------------


#Region "Class Db"

    '	This class contains several generic methods which can be used by other
    '	classes for manipulating a database.

    '    Friend Class Db
    ' Change for git

#Region "Declarations"

    Private Const INT_FORCECLOSE As Int32 = 60000

    Private Shared OpenCount As Int32
    Private Shared oConn As SqlConnection

    Private Shared oTimer As System.Timers.Timer

    Private Shared ConnStr As String

#End Region

#Region "Properties"

    Friend Shared Property ConnectionString() As String
        Get
            Return ConnStr
        End Get
        Set(ByVal value As String)
            ConnStr = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Shared Sub New()
        OpenCount = 0
        oConn = Nothing
        'ConnStr = "Data Source=oserv\SQLExpress;Initial Catalog=DNN560;User ID=sa;Password=sa"
        ConnStr = "Data Source=.;Initial Catalog=DNN562;User ID=sa;Password=rugby22"
    End Sub

#End Region

#Region "Private methods"

    Private Shared Sub StartTimer()
        Try
            If Not oTimer Is Nothing Then
                oTimer.Enabled = False
                oTimer.Dispose()
            End If

            oTimer = New System.Timers.Timer(INT_FORCECLOSE)
            AddHandler oTimer.Elapsed, New ElapsedEventHandler(AddressOf TimerEvent)
            oTimer.Start()

        Catch
            Throw
        End Try
    End Sub

    Private Shared Sub TimerEvent(ByVal source As Object, ByVal e As ElapsedEventArgs)
        Try
            '	The database connection has been open a long time. Forcibly close.

            oTimer.Enabled = False
            oTimer.Dispose()

            Do While (DatabaseClose() > 0)
                '	Do nothing...
            Loop

        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "internal methods"

#Region "ReplaceNull"

    '	These overloaded methods take a database field object and convert
    '	its content to the required value type. If the field is null
    '	a default value is returned.

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Boolean) As Boolean
        Dim RetVal As Boolean = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            ElseIf Not Boolean.TryParse(oFld.ToString(), RetVal) Then
                RetVal = DefVal
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Char) As Char
        Dim RetVal As Char = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                RetVal = oFld.ToString()(0)
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As String) As String
        Dim RetVal As String = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                RetVal = oFld.ToString()
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Byte) As Byte
        Dim RetVal As Byte = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                If Not Byte.TryParse(oFld.ToString(), RetVal) Then
                    RetVal = DefVal
                End If
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Int16) As Int16
        Dim RetVal As Int16 = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                If Not Int16.TryParse(oFld.ToString(), RetVal) Then
                    RetVal = DefVal
                End If
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Int32) As Int32
        Dim RetVal As Int32 = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                If Not Int32.TryParse(oFld.ToString(), RetVal) Then
                    RetVal = DefVal
                End If
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Int64) As Int64
        Dim RetVal As Int64 = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                If Not Int64.TryParse(oFld.ToString(), RetVal) Then
                    RetVal = DefVal
                End If
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Decimal) As Decimal
        Dim RetVal As Decimal = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                If Not Decimal.TryParse(oFld.ToString(), RetVal) Then
                    RetVal = DefVal
                End If
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Single) As Single
        Dim RetVal As Single = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                If Not Single.TryParse(oFld.ToString(), RetVal) Then
                    RetVal = DefVal
                End If
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Double) As Double
        Dim RetVal As Double = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                If Not Double.TryParse(oFld.ToString(), RetVal) Then
                    RetVal = DefVal
                End If
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As Guid) As Guid
        Dim RetVal As Guid = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                RetVal = New Guid(oFld.ToString())
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    Friend Shared Function ReplaceNull(ByVal oFld As Object, ByVal DefVal As DateTime) As DateTime
        Dim RetVal As DateTime = DefVal

        Try
            If oFld.Equals(DBNull.Value) Then
                RetVal = DefVal
            Else
                If Not DateTime.TryParse(oFld.ToString(), RetVal) Then
                    RetVal = DefVal
                End If
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

#End Region

#Region "Quoted"

    '	This method receives a string which is destined to be used as a
    '	parameter which is sent to a database. As required when communicating
    '	with a database any single-instance quotes are replaced by
    '	double-instance quotes.

    Friend Shared Function Quoted(ByVal strSrc As String) As String
        Dim RetStr As String = strSrc

        Try
            RetStr = RetStr.Replace("'", "''")
            '	RetStr = RetStr.Replace("""", """""")
            RetStr = String.Format("'{0}'", RetStr)

        Catch
            Throw
        End Try

        Return RetStr
    End Function

#End Region

#Region "HasRows"

    '	These overloaded methods return a boolean value which indicates
    '	whether the supplied DataSet or DataTable contain at least one
    '	row of data. Sometimes when querying a database you receive no
    '	rows back, although the query succeeded.

    Friend Shared Function HasRows(ByVal oDt As DataTable) As [Boolean]
        Dim bRetVal As [Boolean] = False

        Try
            If oDt IsNot Nothing Then
                bRetVal = (oDt.Rows.Count > 0)
            End If

        Catch
            Throw
        End Try

        Return bRetVal
    End Function

    Friend Shared Function HasRows(ByVal oDs As DataSet) As [Boolean]
        Dim bRetVal As [Boolean] = False

        Try
            If oDs IsNot Nothing Then
                If oDs.Tables.Count > 0 Then
                    If oDs.Tables(0).Rows.Count > 0 Then
                        bRetVal = True
                    End If
                End If
            End If

        Catch
            Throw
        End Try

        Return bRetVal
    End Function

#End Region

#Region "Database operations"

    Friend Shared Function DatabaseOpen() As Int32
        Try
            '	Start/restart timer to ensure that if a database connection is
            '	left open for an extended period it is assumed that the calling
            '	operation has failed, and the database will be forcibly closed.

            'StartTimer()


            '	We don't have a database connection open right now, so create
            '	a new one.


            oConn = New SqlConnection(ConnStr)
            If oConn IsNot Nothing Then
                oConn.Open()
            End If



            '	Increate the open count. This is used to ensure that the database
            '	is only closed when the outer nesting level of open/close is reached.

            'OpenCount = OpenCount + 1

        Catch
            Throw
        End Try

        Return OpenCount
    End Function

    Friend Shared Function DatabaseClose() As Int32
        Try
            'If OpenCount > 0 Then
            '    OpenCount = OpenCount - 1

            '    If OpenCount = 0 Then
            '	We've reached the outer nesting level of open/close.

            If oConn IsNot Nothing Then
                '	Dispose of the timer, this isn't needed any more.

                'oTimer.Enabled = False
                'oTimer.Dispose()

                '	Close and dispose of the database connection.

                oConn.Close()
                '    oConn.Dispose()
                '    oConn = Nothing
            End If
            'End If
            'End If

        Catch
            Throw
        End Try

        Return OpenCount
    End Function

#End Region

#Region "SqlAction"

    '	This method performs an action on a database (using an INSERT,
    '	UPDATE or DELETE statement). No data is expected to be returned
    '	from executing these statements.

    Friend Shared Function SqlAction(ByVal SqlStr As String) As Boolean
        Dim bOkay As Boolean = False

        Try
            DatabaseOpen()
            Dim oCmd As SqlCommand

            oCmd = New SqlCommand(SqlStr, oConn)
            oCmd.CommandType = CommandType.Text
            Try
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            DatabaseClose()
            bOkay = True


        Catch
            Throw
        End Try

        Return bOkay
    End Function

#End Region

#Region "SqlQuery"

    '	This method performs a query on a database (using the SELECT statement).
    '	It is expected that executing the SQL will return data from the
    '	database.

    'Friend Shared Function SqlQuery(ByVal SqlStr As String) As DataSet
    Public Shared Function SqlQuery(ByVal SqlStr As String) As DataSet
        Dim oDs As New DataSet()

        Try
            DatabaseOpen()
            Dim oCmd As SqlCommand

            oCmd = New SqlCommand(SqlStr, oConn)
            oCmd.CommandType = CommandType.Text

            Dim oRdr As SqlDataReader = oCmd.ExecuteReader(CommandBehavior.CloseConnection)

            Dim oConv As New DBConvert()

            Dim oDt As New DataTable()

            oConv.FillFromReader(oDt, oRdr)
            oDs.Tables.Add(oDt)

            DatabaseClose()


        Catch
            Throw
        End Try

        Return oDs
    End Function

#End Region

#End Region
    '    End Class

#End Region

End Class
