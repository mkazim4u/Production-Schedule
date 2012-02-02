Imports Microsoft.VisualBasic

'	-----------------------------------------------------------------------------
'	TABLE:	VB.NET
'
'	You should generate one copy of this class for each table in your database,
'	so there will be multiple copies of this class (one per table) in your
'	project. The resulting class for the table will expose the columns of the
'	table as properties, and provide simple Load/Save functionality.
Imports System
Imports System.Text
Imports System.Data

Public Class FF_Team

#Region "Schema"

    '	Table: FF_Team
    '	
    '	  1: ID                                      System.Int64 (8)
    '	  2: TeamName                                System.String (50)
    '	  3: TeamDescription                         System.String (50)
    '	  4: IsActive                                System.Boolean (1)
    '	  5: CreatedOn                               System.DateTime (8)
    '	  6: CreatedBy                               System.Int64 (8)

#End Region

#Region "Declarations"

    Private Const BASE_DATE As String = "1-Jan-1900"

    Private m_ID As Int64
    Private m_TeamName As String
    Private m_TeamDescription As String
    Private m_IsActive As Boolean
    Private m_CreatedOn As DateTime
    Private m_CreatedBy As Int64

#End Region

#Region "Private methods"

    ''' <summary>
    ''' Initialises properties to default values.
    ''' </summary>
    Private Sub Init()
        Try
            m_ID = 0
            m_TeamName = ""
            m_TeamDescription = ""
            m_IsActive = False
            m_CreatedOn = DateTime.Now
            m_CreatedBy = 0

        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property ID() As Int64
        Get
            Return m_ID
        End Get
    End Property

    Public Property TeamName() As String
        Get
            Return m_TeamName
        End Get
        Set(ByVal value As String)
            m_TeamName = value
        End Set
    End Property

    Public Property TeamDescription() As String
        Get
            Return m_TeamDescription
        End Get
        Set(ByVal value As String)
            m_TeamDescription = value
        End Set
    End Property

    Public Property IsActive() As Boolean
        Get
            Return m_IsActive
        End Get
        Set(ByVal value As Boolean)
            m_IsActive = value
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

    Public Property CreatedBy() As Int64
        Get
            Return m_CreatedBy
        End Get
        Set(ByVal value As Int64)
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
    Public Sub New(ByVal ID_ID As Int64)
        Try
            Load(ID_ID)

        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "Public methods"

    ''' <summary>
    ''' Load record which has the given identity.
    ''' </summary>
    Public Function Load(ByVal ID_ID As Int64) As Int64
        Dim RetVal As Int64 = 0

        Try
            Init()

            Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_Team WHERE ID = " + ID_ID.ToString())

            If Db.HasRows(oDs) Then
                Dim oDr As DataRow = oDs.Tables(0).Rows(0)

                m_ID = Db.ReplaceNull(oDr("ID"), 0)
                m_TeamName = Db.ReplaceNull(oDr("TeamName"), "")
                m_TeamDescription = Db.ReplaceNull(oDr("TeamDescription"), "")
                m_IsActive = Db.ReplaceNull(oDr("IsActive"), False)
                m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)
                m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)

                RetVal = m_ID
            End If

        Catch
            Throw
        End Try

        Return m_ID
    End Function

    ''' <summary>
    ''' Save the record. If the provided ID is zero the record is INSERTed, otherwise
    '''	it is assumed to be an UPDATE. It might be better to verify that the record
    '''	exists prior to being updated, but keeping things simple is preferable as a
    '''	starting point.
    ''' </summary>
    Public Function Save(ByVal ID_ID As Int64) As Int64
        Dim RetVal As Int64 = 0

        Try
            If ID_ID = 0 Then
                RetVal = Add()
            Else
                RetVal = Update(ID_ID)
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    ''' <summary>
    ''' INSERTs a new record into [FF_Team].
    ''' </summary>
    Public Function Add() As Int64
        Dim RetVal As Int64 = 0

        Try
            Dim oStrInsert As New StringBuilder()

            oStrInsert.Append("INSERT INTO [FF_Team] (")
            oStrInsert.Append("[TeamName]")
            oStrInsert.Append(", [TeamDescription]")
            oStrInsert.Append(", [IsActive]")
            oStrInsert.Append(", [CreatedOn]")
            oStrInsert.Append(", [CreatedBy]")
            oStrInsert.Append(") VALUES (")
            oStrInsert.Append(String.Format("{0}", Db.Quoted(m_TeamName.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_TeamDescription.ToString())))
            oStrInsert.Append(String.Format(", {0}", IIf(m_IsActive, "1", "0")))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
            oStrInsert.Append(String.Format(", {0}", m_CreatedBy.ToString()))
            oStrInsert.Append(");")

            Db.SqlAction(oStrInsert.ToString())

            Dim oDs As DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_Team")

            If Db.HasRows(oDs) Then
                RetVal = Db.ReplaceNull(oDs.Tables(0).Rows(0)(0), 0)
            End If

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    ''' <summary>
    ''' UPDATEs an existing record.
    ''' </summary>
    Public Function Update(ByVal ID_ID As Int64) As Int64
        Dim RetVal As Int64 = 0

        Try
            Dim oStrUpdate As New StringBuilder()

            oStrUpdate.Append("UPDATE [FF_Team] SET ")
            oStrUpdate.Append("[TeamName] = " + Db.Quoted(m_TeamName.ToString()))
            oStrUpdate.Append(", [TeamDescription] = " + Db.Quoted(m_TeamDescription.ToString()))
            oStrUpdate.Append(", [IsActive] = " + IIf(m_IsActive, "1", "0"))
            oStrUpdate.Append(", [CreatedOn] = " + Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString()))
            oStrUpdate.Append(", [CreatedBy] = " + m_CreatedBy.ToString())
            oStrUpdate.Append(" WHERE [ID] = " + ID_ID.ToString())

            Db.SqlAction(oStrUpdate.ToString())

            RetVal = ID_ID

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    ''' <summary>
    ''' Deletes an existing record. Prior to the deletion the record is loaded, so it could
    '''	potentially be re-added straightaway after deletion takes place.
    ''' </summary>
    Public Sub Delete(ByVal ID_ID As Int64)
        Try
            Load(ID_ID)
            Db.SqlAction("DELETE FROM [FF_Team] WHERE [ID] = " + ID_ID.ToString())

        Catch
            Throw
        End Try
    End Sub

#End Region

End Class



