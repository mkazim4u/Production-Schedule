Imports System
Imports System.Text
Imports System.Data


Public Class FF_UserTeamMapping

#Region "Schema"

    '	Table: FF_UserTeamMapping
    '	
    '	  1: ID                                      System.Int64 (8)
    '	  2: UserId                                  System.Int64 (8)
    '	  3: TeamId                                  System.Int64 (8)
    '	  4: CreatedOn                               System.DateTime (8)
    '	  5: CreatedBy                               System.Int64 (8)

#End Region

#Region "Declarations"

    Private m_ID As Int64
    Private m_UserId As Int64
    Private m_TeamId As Int64
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
            m_UserId = 0
            m_TeamId = 0
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

    Public Property UserId() As Int64
        Get
            Return m_UserId
        End Get
        Set(ByVal value As Int64)
            m_UserId = value
        End Set
    End Property

    Public Property TeamId() As Int64
        Get
            Return m_TeamId
        End Get
        Set(ByVal value As Int64)
            m_TeamId = value
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

            Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_UserTeamMapping WHERE ID = " + ID_ID.ToString())

            If Db.HasRows(oDs) Then
                Dim oDr As DataRow = oDs.Tables(0).Rows(0)

                m_ID = Db.ReplaceNull(oDr("ID"), 0)
                m_UserId = Db.ReplaceNull(oDr("UserId"), 0)
                m_TeamId = Db.ReplaceNull(oDr("TeamId"), 0)
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
    ''' INSERTs a new record into [FF_UserTeamMapping].
    ''' </summary>
    Public Function Add() As Int64
        Dim RetVal As Int64 = 0

        Try
            Dim oStrInsert As New StringBuilder()

            oStrInsert.Append("INSERT INTO [FF_UserTeamMapping] (")
            oStrInsert.Append("[UserId]")
            oStrInsert.Append(", [TeamId]")
            oStrInsert.Append(", [CreatedOn]")
            oStrInsert.Append(", [CreatedBy]")
            oStrInsert.Append(") VALUES (")
            oStrInsert.Append(String.Format("{0}", m_UserId.ToString()))
            oStrInsert.Append(String.Format(", {0}", m_TeamId.ToString()))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
            oStrInsert.Append(String.Format(", {0}", m_CreatedBy.ToString()))
            oStrInsert.Append(");")

            Db.SqlAction(oStrInsert.ToString())

            Dim oDs As DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_UserTeamMapping")

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

            oStrUpdate.Append("UPDATE [FF_UserTeamMapping] SET ")
            oStrUpdate.Append("[UserId] = " + m_UserId.ToString())
            oStrUpdate.Append(", [TeamId] = " + m_TeamId.ToString())
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
            Db.SqlAction("DELETE FROM [FF_UserTeamMapping] WHERE [ID] = " + ID_ID.ToString())

        Catch
            Throw
        End Try
    End Sub

#End Region

End Class


