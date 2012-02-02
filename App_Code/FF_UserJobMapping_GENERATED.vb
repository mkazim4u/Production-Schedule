Imports Microsoft.VisualBasic

'	-----------------------------------------------------------------------------
'	TABLE:	VB.NET
'
'	You should generate one copy of this class for each table in your database,
'	so there will be multiple copies of this class (one per table) in your
'	project. The resulting class for the table will expose the columns of the
'	table as properties, and provide simple Load/Save functionality.
'	-----------------------------------------------------------------------------
'	Template name:	TABLE - VbDotNet.template
'	Template date:	21 February 2011 15:05:01
'	Database type:	SqlConnection
'	Database name:	dnn560
'	Table name:		FF_UserJobMapping
'	Generated:		21 Feb 2011 15:07:01
'	-----------------------------------------------------------------------------
Imports System
Imports System.Text
Imports System.Data



Public Class FF_UserJobMapping

#Region "Schema"

    '	Table: FF_UserJobMapping
    '	
    '	  1: id                                      System.Int64 (8)
    '	  2: JobID                                   System.Int64 (8)
    '	  3: JobStateID                              System.Int64 (8)
    '	  4: UserID                                  System.Int64 (8)
    '	  5: RoleID                                  System.Int64 (8)
    '	  6: IsEmail                                 System.Boolean (1)
    '	  7: IsEmailSent                             System.Boolean (1)
    '	  8: CreatedOn                               System.DateTime (4)

#End Region

#Region "Declarations"

    Private m_id As Int64
    Private m_JobID As Int64
    Private m_JobStateID As Int64
    Private m_UserID As Int64
    Private m_RoleID As Int64
    Private m_IsEmail As Boolean
    Private m_IsEmailSent As Boolean
    Private m_CreatedOn As DateTime
    Private m_CreatedBy As Int32

#End Region

#Region "Private methods"

    ''' <summary>
    ''' Initialises properties to default values.
    ''' </summary>
    Private Sub Init()
        Try
            m_id = 0
            m_JobID = 0
            m_JobStateID = 0
            m_UserID = 0
            m_RoleID = 0
            m_IsEmail = False
            m_IsEmailSent = False
            m_CreatedOn = DateTime.Now
            m_CreatedBy = UserController.GetCurrentUserInfo.UserID

        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property ID() As Int64
        Get
            Return m_id
        End Get
    End Property

    Public Property JobID() As Int64
        Get
            Return m_JobID
        End Get
        Set(ByVal value As Int64)
            m_JobID = value
        End Set
    End Property

    Public Property JobStateID() As Int64
        Get
            Return m_JobStateID
        End Get
        Set(ByVal value As Int64)
            m_JobStateID = value
        End Set
    End Property

    Public Property UserID() As Int64
        Get
            Return m_UserID
        End Get
        Set(ByVal value As Int64)
            m_UserID = value
        End Set
    End Property

    Public Property RoleID() As Int64
        Get
            Return m_RoleID
        End Get
        Set(ByVal value As Int64)
            m_RoleID = value
        End Set
    End Property

    Public Property IsEmail() As Boolean
        Get
            Return m_IsEmail
        End Get
        Set(ByVal value As Boolean)
            m_IsEmail = value
        End Set
    End Property

    Public Property IsEmailSent() As Boolean
        Get
            Return m_IsEmailSent
        End Get
        Set(ByVal value As Boolean)
            m_IsEmailSent = value
        End Set
    End Property

    Public Property CreatedOn() As DateTime
        Get
            Return m_CreatedOn
        End Get
        Set(ByVal value As DateTime)
            m_CreatedOn = value
        End Set
    End Property

    Public Property CreatedBy() As Int32
        Get
            Return m_CreatedBy
        End Get
        Set(ByVal value As Int32)
            m_CreatedBy = value
        End Set
    End Property
#End Region

#Region "Constructors"

    ''' <summary>
    ''' Constructor which initialises the properties to default values.
    ''' </summary>
    Public Sub New()
        Try
            Init()

        Catch
            Throw
        End Try
    End Sub

    ''' <summary>
    ''' Constructor which loads the record from the given identity field.
    ''' </summary>
    Public Sub New(ByVal ID_id As Int64)
        Try
            GetUserJobMappingByID(ID_id)

        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "Public methods"

    ''' <summary>
    ''' Load record which has the given identity.
    ''' </summary>
    Public Function GetUserJobMappingByID(ByVal ID_id As Int64) As Int64
        Dim RetVal As Int64 = 0

        Try
            Init()

            Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_UserJobMapping WHERE id = " + ID_id.ToString())

            If Db.HasRows(oDs) Then
                Dim oDr As DataRow = oDs.Tables(0).Rows(0)

                m_id = Db.ReplaceNull(oDr("id"), 0)
                m_JobID = Db.ReplaceNull(oDr("JobID"), 0)
                m_JobStateID = Db.ReplaceNull(oDr("JobStateID"), 0)
                m_UserID = Db.ReplaceNull(oDr("UserID"), 0)
                m_RoleID = Db.ReplaceNull(oDr("RoleID"), 0)
                m_IsEmail = Db.ReplaceNull(oDr("IsEmail"), False)
                m_IsEmailSent = Db.ReplaceNull(oDr("IsEmailSent"), False)
                m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)
                m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)

                RetVal = m_id
            End If

        Catch
            Throw New Exception("GetUserJobMappingByID failed.")
        End Try

        Return m_id
    End Function

    ''' <summary>
    ''' Save the record. If the provided ID is zero the record is INSERTed, otherwise
    '''	it is assumed to be an UPDATE. It might be better to verify that the record
    '''	exists prior to being updated, but keeping things simple is preferable as a
    '''	starting point.
    ''' </summary>
    Public Function Save(ByVal ID_id As Int64) As Int64
        Dim RetVal As Int64 = 0

        Try
            If ID_id = 0 Then
                RetVal = Add()
            Else
                RetVal = Update(ID_id)
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    ''' <summary>
    ''' INSERTs a new record into [FF_UserJobMapping].
    ''' </summary>
    Public Function Add() As Int64
        Dim RetVal As Int64 = 0

        Try
            Dim oStrInsert As New StringBuilder()

            oStrInsert.Append("INSERT INTO [FF_UserJobMapping] (")
            oStrInsert.Append("[JobID]")
            oStrInsert.Append(", [JobStateID]")
            oStrInsert.Append(", [UserID]")
            oStrInsert.Append(", [RoleID]")
            oStrInsert.Append(", [IsEmail]")
            oStrInsert.Append(", [IsEmailSent]")
            oStrInsert.Append(", [CreatedOn]")
            oStrInsert.Append(", [CreatedBy]")
            oStrInsert.Append(") VALUES (")
            oStrInsert.Append(String.Format("{0}", m_JobID.ToString()))
            oStrInsert.Append(String.Format(", {0}", m_JobStateID.ToString()))
            oStrInsert.Append(String.Format(", {0}", m_UserID.ToString()))
            oStrInsert.Append(String.Format(", {0}", m_RoleID.ToString()))
            oStrInsert.Append(String.Format(", {0}", IIf(m_IsEmail, "1", "0")))
            oStrInsert.Append(String.Format(", {0}", IIf(m_IsEmailSent, "1", "0")))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
            oStrInsert.Append(String.Format(", {0}", m_CreatedBy.ToString()))
            oStrInsert.Append(");")

            Db.SqlAction(oStrInsert.ToString())


            Dim oDs As DataSet = Db.SqlQuery("SELECT MAX(ID) FROM FF_UserJobMapping")

            If Db.HasRows(oDs) Then
                RetVal = Db.ReplaceNull(oDs.Tables(0).Rows(0)(0), 0)
            End If

            '	Additional logic is required here to determine the return value of the
            '	inserted record. This logic depends upon the data type of the
            '	identity field. What would work for an Integer field would fail for
            '	string and GUID return values, and if the database is using auto-generation
            '	of the identity field (for example with a GUID value) then it isn't
            '	possible to easily figure out programmatically how to construct the
            '	necessary SQL logic to suit every data type.
            '
            '	Note that if it were an Integer field then SCOPE_IDENTITY would work
            '	for SQL Server - but not other database types. Therefore the logic
            '	required to obtain the return value is left up to the user.

        Catch ex As Exception
            Throw New Exception(Me.ToString() & " Add failed." & ex.Message)
        End Try

        Return RetVal
    End Function

    ''' <summary>
    ''' UPDATEs an existing record.
    ''' </summary>
    Public Function Update(ByVal ID_id As Int64) As Int64
        Dim RetVal As Int64 = 0

        Try
            Dim oStrUpdate As New StringBuilder()

            oStrUpdate.Append("UPDATE [FF_UserJobMapping] SET ")
            oStrUpdate.Append("[JobID] = " + m_JobID.ToString())
            oStrUpdate.Append(", [JobStateID] = " + m_JobStateID.ToString())
            oStrUpdate.Append(", [UserID] = " + m_UserID.ToString())
            oStrUpdate.Append(", [RoleID] = " + m_RoleID.ToString())
            oStrUpdate.Append(", [IsEmail] = " + IIf(m_IsEmail, "1", "0"))
            oStrUpdate.Append(", [IsEmailSent] = " + IIf(m_IsEmailSent, "1", "0"))
            oStrUpdate.Append(", CreatedOn = " + Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString()))
            oStrUpdate.Append(", [CreatedBy] = " + m_CreatedBy.ToString())
            oStrUpdate.Append(" WHERE [id] = " + ID_id.ToString())

            Db.SqlAction(oStrUpdate.ToString())

            RetVal = ID_id

        Catch ex As Exception
            Throw New Exception(Me.ToString() & " Update failed. " & ex.Message)
        End Try

        Return RetVal
    End Function

    ''' <summary>
    ''' Deletes an existing record. Prior to the deletion the record is loaded, so it could
    '''	potentially be re-added straightaway after deletion takes place.
    ''' </summary>
    Public Sub Delete(ByVal ID_id As Int64)
        Try
            GetUserJobMappingByID(ID_id)
            Db.SqlAction("DELETE FROM [FF_UserJobMapping] WHERE [id] = " + ID_id.ToString())

        Catch
            Throw New Exception(Me.ToString() & " Deleted failed.")
        End Try
    End Sub

#End Region

End Class



