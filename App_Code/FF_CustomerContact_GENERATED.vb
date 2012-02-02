'	-----------------------------------------------------------------------------
'	This template supports a single table within a database. It doesn't care
'	about the database type (Sqlite, SQL Server or MySql) as the functionality
'	calls upon another class which is targeted at a specific database type.
'
'	Created:		09 Aug 2010 11:09:05
'	Server:			.
'	Database:		DNN542c
'	Description:	DNN542c
'	Database type:	SqlConnection
'	User name:		sa
'	Password:		rugby22
'	File location:	
'	Table:			FF_CustomerContact
'	Connection:		Data Source=.;Initial Catalog=DNN542c;User Id=sa;Password=rugby22;Connection Timeout=20
'	-----------------------------------------------------------------------------
Imports System
Imports System.Text
Imports System.Data


Public Class FF_CustomerContact

#Region "Schema"

    '	Table: FF_CustomerContact
    '	
    '	  1: id                                      System.Int32 (4)
    '	  2: CustomerKey                             System.Int32 (4)
    '	  3: Name                                    System.String (100)
    '	  4: Telephone                               System.String (50)
    '	  5: Mobile                                  System.String (50)
    '	  6: EmailAddr                               System.String (100)
    '	  7: Notes                                   System.String (2000)
    '	  8: CreatedOn                               System.DateTime (4)
    '	  9: CreatedBy                               System.Int32 (4)

#End Region

#Region "Declarations"

    Private m_id As Int32
    Private m_CustomerKey As Int32
    Private m_Name As String
    Private m_Telephone As String
    Private m_Mobile As String
    Private m_EmailAddr As String
    Private m_Notes As String
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
            m_CustomerKey = 0
            m_Name = ""
            m_Telephone = ""
            m_Mobile = ""
            m_EmailAddr = ""
            m_Notes = ""
            m_CreatedOn = DateTime.Now
            m_CreatedBy = 0

        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "Properties"

    Public Property id() As Int32
        Get
            Return m_id
        End Get
        Set(ByVal value As Int32)
            m_id = value
        End Set
    End Property

    Public Property CustomerKey() As Int32
        Get
            Return m_CustomerKey
        End Get
        Set(ByVal value As Int32)
            m_CustomerKey = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

    Public Property Telephone() As String
        Get
            Return m_Telephone
        End Get
        Set(ByVal value As String)
            m_Telephone = value
        End Set
    End Property

    Public Property Mobile() As String
        Get
            Return m_Mobile
        End Get
        Set(ByVal value As String)
            m_Mobile = value
        End Set
    End Property

    Public Property EmailAddr() As String
        Get
            Return m_EmailAddr
        End Get
        Set(ByVal value As String)
            m_EmailAddr = value
        End Set
    End Property

    Public Property Notes() As String
        Get
            Return m_Notes
        End Get
        Set(ByVal value As String)
            m_Notes = value
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
    Public Sub New(ByVal ID_id As Int32)
        Try
            Load(ID_id)

        Catch
            Throw
        End Try
    End Sub

#End Region

#Region "Public methods"

    ''' <summary>
    ''' Load record which has the given identity.
    ''' </summary>
    Public Function Load(ByVal ID_id As Int32) As Int32
        Dim RetVal As Int32 = 0

        Try
            Init()

            Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_CustomerContact WHERE id = " + ID_id.ToString())

            If Db.HasRows(oDs) Then
                Dim oDr As DataRow = oDs.Tables(0).Rows(0)

                m_id = Db.ReplaceNull(oDr("id"), 0)
                m_CustomerKey = Db.ReplaceNull(oDr("CustomerKey"), 0)
                m_Name = Db.ReplaceNull(oDr("Name"), "")
                m_Telephone = Db.ReplaceNull(oDr("Telephone"), "")
                m_Mobile = Db.ReplaceNull(oDr("Mobile"), "")
                m_EmailAddr = Db.ReplaceNull(oDr("EmailAddr"), "")
                m_Notes = Db.ReplaceNull(oDr("Notes"), "")
                m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)
                m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)

                RetVal = m_id
            End If

        Catch
            Throw
        End Try

        Return m_id
    End Function

    ''' <summary>
    ''' Save the record. If the provided ID is zero the record is INSERTed, otherwise
    '''	it is assumed to be an UPDATE. It might be better to verify that the record
    '''	exists prior to being updated, but keeping things simple is preferable as a
    '''	starting point.
    ''' </summary>
    Public Function Save(ByVal ID_id As Int32) As Int32
        Dim RetVal As Int32 = 0

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
    ''' INSERTs a new record into [FF_CustomerContact].
    ''' </summary>
    Public Function Add() As Int32
        Dim RetVal As Int32 = 0

        Try
            Dim oStrInsert As New StringBuilder()

            oStrInsert.Append("INSERT INTO [FF_CustomerContact] (")
            oStrInsert.Append("[CustomerKey]")
            oStrInsert.Append(", [Name]")
            oStrInsert.Append(", [Telephone]")
            oStrInsert.Append(", [Mobile]")
            oStrInsert.Append(", [EmailAddr]")
            oStrInsert.Append(", [Notes]")
            oStrInsert.Append(", [CreatedOn]")
            oStrInsert.Append(", [CreatedBy]")
            oStrInsert.Append(") VALUES (")
            oStrInsert.Append(String.Format("{0}", m_CustomerKey.ToString()))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_Name.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_Telephone.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_Mobile.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_EmailAddr.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_Notes.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
            oStrInsert.Append(String.Format(", {0}", m_CreatedBy.ToString()))
            oStrInsert.Append(");")

            Db.SqlAction(oStrInsert.ToString())

            Dim oDs As DataSet = Db.SqlQuery("SELECT MAX([id]) FROM FF_CustomerContact")

            RetVal = oDs.Tables(0).Rows(0)(0)

        Catch ex As Exception
            Throw New Exception("Add failed. " & ex.Message)
        End Try

        Return RetVal
    End Function

    ''' <summary>
    ''' UPDATEs an existing record.
    ''' </summary>
    Public Function Update(ByVal ID_id As Int32) As Int32
        Dim RetVal As Int32 = 0

        Try
            Dim oStrUpdate As New StringBuilder()

            oStrUpdate.Append("UPDATE [FF_CustomerContact] SET ")
            oStrUpdate.Append("[CustomerKey] = " + m_CustomerKey.ToString())
            oStrUpdate.Append(", [Name] = " + Db.Quoted(m_Name.ToString()))
            oStrUpdate.Append(", [Telephone] = " + Db.Quoted(m_Telephone.ToString()))
            oStrUpdate.Append(", [Mobile] = " + Db.Quoted(m_Mobile.ToString()))
            oStrUpdate.Append(", [EmailAddr] = " + Db.Quoted(m_EmailAddr.ToString()))
            oStrUpdate.Append(", [Notes] = " + Db.Quoted(m_Notes.ToString()))
            oStrUpdate.Append(", [CreatedOn] = " + Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString()))
            oStrUpdate.Append(", [CreatedBy] = " + m_CreatedBy.ToString())
            oStrUpdate.Append(" WHERE [id] = " + ID_id.ToString())

            Db.SqlAction(oStrUpdate.ToString())

            RetVal = ID_id

        Catch
            Throw
        End Try

        Return RetVal
    End Function

    ''' <summary>
    ''' Deletes an existing record. Prior to the deletion the record is loaded, so it could
    '''	potentially be re-added straightaway after deletion takes place.
    ''' </summary>
    Public Sub Delete(ByVal ID_id As Int32)
        Try
            Load(ID_id)
            Db.SqlAction("DELETE FROM [FF_CustomerContact] WHERE [id] = " + ID_id.ToString())

        Catch
            Throw
        End Try
    End Sub

#End Region

End Class

