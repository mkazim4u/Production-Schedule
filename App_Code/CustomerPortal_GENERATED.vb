Imports System
Imports System.Text
Imports System.Data


Public Class CustomerPortalMapping

#Region "Schema"

    '	Table: CustomerPortalMapping
    '	
    '	  1: ID                                      System.Int64 (8)
    '	  2: PortalID                                System.Int64 (8)
    '	  3: SprintCustomerKey                       System.Int64 (8)
    '	  4: CreatedOn                               System.DateTime (8)
    '	  5: CreatedBy                               System.Int64 (8)

#End Region

#Region "Declarations"

    Private m_ID As Int64
    Private m_PortalID As Int64
    Private m_SprintCustomerKey As Int64
    Private m_CreatedOn As DateTime
    Private m_CreatedBy As Int64
    Private m_CustomerName As String
    Private m_PortalName As String


#End Region

#Region "Private methods"

    ''' <summary>
    ''' Initialises properties to default values.
    ''' </summary>
    Private Sub Init()
        Try
            m_ID = 0
            m_PortalID = 0
            m_SprintCustomerKey = 0
            m_CreatedOn = DateTime.Now
            m_CreatedBy = 0
            m_CustomerName = ""
            m_PortalName = ""

        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "Properties"

    Public Property CustomerName() As String
        Get
            Return m_CustomerName
        End Get
        Set(ByVal value As String)
            m_CustomerName = value
        End Set
    End Property
    Public Property PortalName() As String
        Get
            Return m_PortalName
        End Get
        Set(ByVal value As String)
            m_PortalName = value
        End Set
    End Property

    Public ReadOnly Property ID() As Int64
        Get
            Return m_ID
        End Get
    End Property

    Public Property PortalID() As Int64
        Get
            Return m_PortalID
        End Get
        Set(ByVal value As Int64)
            m_PortalID = value
        End Set
    End Property

    Public Property SprintCustomerKey() As Int64
        Get
            Return m_SprintCustomerKey
        End Get
        Set(ByVal value As Int64)
            m_SprintCustomerKey = value
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

            Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM CustomerPortalMapping WHERE ID = " + ID_ID.ToString())

            If Db.HasRows(oDs) Then
                Dim oDr As DataRow = oDs.Tables(0).Rows(0)

                m_ID = Db.ReplaceNull(oDr("ID"), 0)
                m_PortalID = Db.ReplaceNull(oDr("PortalID"), 0)
                m_SprintCustomerKey = Db.ReplaceNull(oDr("SprintCustomerKey"), 0)
                m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)
                m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)
                m_CustomerName = Db.ReplaceNull(oDr("CustomerName"), "")
                m_PortalName = Db.ReplaceNull(oDr("PortalName"), "")

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
    ''' INSERTs a new record into [CustomerPortalMapping].
    ''' </summary>
    Public Function Add() As Int64
        Dim RetVal As Int64 = 0
        Dim oStrInsert As New StringBuilder()

        Try


            oStrInsert.Append("INSERT INTO [CustomerPortalMapping] (")
            oStrInsert.Append("[PortalID]")
            oStrInsert.Append(", [PortalName]")
            oStrInsert.Append(", [SprintCustomerKey]")
            oStrInsert.Append(", [CustomerName]")
            oStrInsert.Append(", [CreatedOn]")
            oStrInsert.Append(", [CreatedBy]")
            oStrInsert.Append(") VALUES (")
            oStrInsert.Append(String.Format("{0}", m_PortalID.ToString()))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_PortalName.ToString())))
            oStrInsert.Append(String.Format(", {0}", m_SprintCustomerKey.ToString()))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_CustomerName.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
            oStrInsert.Append(String.Format(", {0}", m_CreatedBy.ToString()))
            oStrInsert.Append(");")
            Db.SqlAction(oStrInsert.ToString())
            Dim oDs As DataSet = Db.SqlQuery("SELECT MAX(id) FROM CustomerPortalMapping")

            If Db.HasRows(oDs) Then
                RetVal = Db.ReplaceNull(oDs.Tables(0).Rows(0)(0), 0)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message & "Add failed." & oStrInsert.ToString())
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

            oStrUpdate.Append("UPDATE [CustomerPortalMapping] SET ")
            oStrUpdate.Append("[PortalID] = " + m_PortalID.ToString())
            oStrUpdate.Append(", [PortalName] = " + Db.Quoted(m_PortalName.ToString()))
            oStrUpdate.Append(", [SprintCustomerKey] = " + m_SprintCustomerKey.ToString())
            oStrUpdate.Append(", [CustomerName] = " + Db.Quoted(m_CustomerName.ToString()))
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
            Db.SqlAction("DELETE FROM [CustomerPortalMapping] WHERE [ID] = " + ID_ID.ToString())

        Catch
            Throw
        End Try
    End Sub

#End Region

End Class



