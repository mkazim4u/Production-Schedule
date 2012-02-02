Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data

'	-----------------------------------------------------------------------------
'	Table:			FF_Customer
'	Created:		11 Aug 2010 20:54:38
'	Server:			VOSTRO
'	Database:		DNN542c DNN542c
'	File location:		
'	-----------------------------------------------------------------------------

	Public Class FF_Customer
	
#Region	"Declarations"

    Private Const BASE_DATE As String = "1-Jan-1900"

		Private m_id As Int32
		Private m_ExternalCustomerKey As Int32
		Private m_CustomerCode As String
		Private m_CustomerName As String
		Private m_CustomerAddr1 As String
		Private m_CustomerAddr2 As String
		Private m_CustomerAddr3 As String
		Private m_CustomerTown As String
		Private m_CustomerPostcode As String
		Private m_CustomerCountryKey As Int32
		Private m_CustomerTelephone As String
		Private m_CustomerEmail As String
		Private m_CustomerContact As String
		Private m_CustomerAccountHandler As Int32
		Private m_IsActive As Boolean
		Private m_IsDeleted As Boolean
		Private m_CreatedOn As DateTime
		Private m_CreatedBy As Int32

#End Region

#Region	"Private methods"

		Private Sub Init()
			Try
			   	m_id = 0
			   	m_ExternalCustomerKey = 0
			   	m_CustomerCode = ""
			   	m_CustomerName = ""
			   	m_CustomerAddr1 = ""
			   	m_CustomerAddr2 = ""
			   	m_CustomerAddr3 = ""
			   	m_CustomerTown = ""
			   	m_CustomerPostcode = ""
			   	m_CustomerCountryKey = 0
			   	m_CustomerTelephone = ""
			   	m_CustomerEmail = ""
			   	m_CustomerContact = ""
			   	m_CustomerAccountHandler = 0
			   	m_IsActive = False
			   	m_IsDeleted = False
			   	m_CreatedOn = DateTime.Now
			   	m_CreatedBy = 0

			Catch
				Throw New Exception ("Init failed.")
			End Try
		End Sub

#End Region

#Region	"Properties"

		Public Property id() As Int32
			Get
				Return m_id
			End Get
			Set(ByVal value As Int32)
				m_id = value
			End Set
		End Property

		Public Property ExternalCustomerKey() As Int32
			Get
				Return m_ExternalCustomerKey
			End Get
			Set(ByVal value As Int32)
				m_ExternalCustomerKey = value
			End Set
		End Property

		Public Property CustomerCode() As String
			Get
				Return m_CustomerCode
			End Get
			Set(ByVal value As String)
				m_CustomerCode = value
			End Set
		End Property

		Public Property CustomerName() As String
			Get
				Return m_CustomerName
			End Get
			Set(ByVal value As String)
				m_CustomerName = value
			End Set
		End Property

		Public Property CustomerAddr1() As String
			Get
				Return m_CustomerAddr1
			End Get
			Set(ByVal value As String)
				m_CustomerAddr1 = value
			End Set
		End Property

		Public Property CustomerAddr2() As String
			Get
				Return m_CustomerAddr2
			End Get
			Set(ByVal value As String)
				m_CustomerAddr2 = value
			End Set
		End Property

		Public Property CustomerAddr3() As String
			Get
				Return m_CustomerAddr3
			End Get
			Set(ByVal value As String)
				m_CustomerAddr3 = value
			End Set
		End Property

		Public Property CustomerTown() As String
			Get
				Return m_CustomerTown
			End Get
			Set(ByVal value As String)
				m_CustomerTown = value
			End Set
		End Property

		Public Property CustomerPostcode() As String
			Get
				Return m_CustomerPostcode
			End Get
			Set(ByVal value As String)
				m_CustomerPostcode = value
			End Set
		End Property

		Public Property CustomerCountryKey() As Int32
			Get
				Return m_CustomerCountryKey
			End Get
			Set(ByVal value As Int32)
				m_CustomerCountryKey = value
			End Set
		End Property

		Public Property CustomerTelephone() As String
			Get
				Return m_CustomerTelephone
			End Get
			Set(ByVal value As String)
				m_CustomerTelephone = value
			End Set
		End Property

		Public Property CustomerEmail() As String
			Get
				Return m_CustomerEmail
			End Get
			Set(ByVal value As String)
				m_CustomerEmail = value
			End Set
		End Property

		Public Property CustomerContact() As String
			Get
				Return m_CustomerContact
			End Get
			Set(ByVal value As String)
				m_CustomerContact = value
			End Set
		End Property

		Public Property CustomerAccountHandler() As Int32
			Get
				Return m_CustomerAccountHandler
			End Get
			Set(ByVal value As Int32)
				m_CustomerAccountHandler = value
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

		Public Property IsDeleted() As Boolean
			Get
				Return m_IsDeleted
			End Get
			Set(ByVal value As Boolean)
				m_IsDeleted = value
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

#Region	"Constructors"

		Public Sub New()
			Try
				Init()

			Catch
				Throw New Exception ("Constructor failed.")
			End Try
		End Sub

		Public Sub New(ByVal ID_id AS Int32)
			Try
				Load(ID_id)

			Catch
				Throw New Exception ("Constructor for existing record failed.")
			End Try
		End Sub

#End Region

#Region	"Public methods"

		Public Function Load(ByVal ID_id As Int32) As Int32
			Dim RetVal As Int32 = 0

			Try
				Init()
				
				Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_Customer WHERE id = " + ID_id.ToString())

				If Db.HasRows(oDs) Then
					Dim oDr As DataRow = oDs.Tables(0).Rows(0)

					m_id = Db.ReplaceNull(oDr("id"), 0)
					m_ExternalCustomerKey = Db.ReplaceNull(oDr("ExternalCustomerKey"), 0)
					m_CustomerCode = Db.ReplaceNull(oDr("CustomerCode"), "")
					m_CustomerName = Db.ReplaceNull(oDr("CustomerName"), "")
					m_CustomerAddr1 = Db.ReplaceNull(oDr("CustomerAddr1"), "")
					m_CustomerAddr2 = Db.ReplaceNull(oDr("CustomerAddr2"), "")
					m_CustomerAddr3 = Db.ReplaceNull(oDr("CustomerAddr3"), "")
					m_CustomerTown = Db.ReplaceNull(oDr("CustomerTown"), "")
					m_CustomerPostcode = Db.ReplaceNull(oDr("CustomerPostcode"), "")
					m_CustomerCountryKey = Db.ReplaceNull(oDr("CustomerCountryKey"), 0)
					m_CustomerTelephone = Db.ReplaceNull(oDr("CustomerTelephone"), "")
					m_CustomerEmail = Db.ReplaceNull(oDr("CustomerEmail"), "")
					m_CustomerContact = Db.ReplaceNull(oDr("CustomerContact"), "")
					m_CustomerAccountHandler = Db.ReplaceNull(oDr("CustomerAccountHandler"), 0)
					m_IsActive = Db.ReplaceNull(oDr("IsActive"), False)
					m_IsDeleted = Db.ReplaceNull(oDr("IsDeleted"), False)
					m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)
					m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)

					RetVal = m_id
				End If

			Catch
				Throw New Exception ("Load failed.")
			End Try

			Return m_id
		End Function

		Public Function Add() As Int32
			Dim RetVal As Int64 = 0

			Try
				Dim oStrInsert As New StringBuilder()

				oStrInsert.Append("INSERT INTO [FF_Customer] (")
				oStrInsert.Append("[ExternalCustomerKey]")
				oStrInsert.Append(", [CustomerCode]")
				oStrInsert.Append(", [CustomerName]")
				oStrInsert.Append(", [CustomerAddr1]")
				oStrInsert.Append(", [CustomerAddr2]")
				oStrInsert.Append(", [CustomerAddr3]")
				oStrInsert.Append(", [CustomerTown]")
				oStrInsert.Append(", [CustomerPostcode]")
				oStrInsert.Append(", [CustomerCountryKey]")
				oStrInsert.Append(", [CustomerTelephone]")
				oStrInsert.Append(", [CustomerEmail]")
				oStrInsert.Append(", [CustomerContact]")
				oStrInsert.Append(", [CustomerAccountHandler]")
				oStrInsert.Append(", [IsActive]")
				oStrInsert.Append(", [IsDeleted]")
				oStrInsert.Append(", [CreatedOn]")
				oStrInsert.Append(", [CreatedBy]")
				oStrInsert.Append(") VALUES (")
				oStrInsert.Append(string.format("{0}", m_ExternalCustomerKey.ToString()))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerCode.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerName.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerAddr1.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerAddr2.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerAddr3.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerTown.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerPostcode.ToString())))
				oStrInsert.Append(string.format(", {0}", m_CustomerCountryKey.ToString()))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerTelephone.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerEmail.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustomerContact.ToString())))
				oStrInsert.Append(string.format(", {0}", m_CustomerAccountHandler.ToString()))
				oStrInsert.Append(string.format(", {0}", Iif(m_IsActive, "1", "0")))
				oStrInsert.Append(string.format(", {0}", Iif(m_IsDeleted, "1", "0")))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
				oStrInsert.Append(string.format(", {0}", m_CreatedBy.ToString()))
				oStrInsert.Append(");")

				Db.SqlAction(oStrInsert.ToString())
				
				Dim oDs AS DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_Customer")
				
				If Db.HasRows(oDs) Then
					RetVal = Db.ReplaceNull(oDs.Tables(0).Rows(0)(0), 0)
				End If

			Catch
				Throw New Exception ("Add failed.")
			End Try

			Return RetVal
		End Function

		Public Function Update(ByVal ID_id As Int32) As Int32
			Dim RetVal As Int64 = 0

			Try
				Dim oStrUpdate As New StringBuilder()

				oStrUpdate.Append("UPDATE [FF_Customer] SET ")
				oStrUpdate.Append("[ExternalCustomerKey] = " + m_ExternalCustomerKey.ToString())
				oStrUpdate.Append(", [CustomerCode] = " + Db.Quoted(m_CustomerCode.ToString()))
				oStrUpdate.Append(", [CustomerName] = " + Db.Quoted(m_CustomerName.ToString()))
				oStrUpdate.Append(", [CustomerAddr1] = " + Db.Quoted(m_CustomerAddr1.ToString()))
				oStrUpdate.Append(", [CustomerAddr2] = " + Db.Quoted(m_CustomerAddr2.ToString()))
				oStrUpdate.Append(", [CustomerAddr3] = " + Db.Quoted(m_CustomerAddr3.ToString()))
				oStrUpdate.Append(", [CustomerTown] = " + Db.Quoted(m_CustomerTown.ToString()))
				oStrUpdate.Append(", [CustomerPostcode] = " + Db.Quoted(m_CustomerPostcode.ToString()))
				oStrUpdate.Append(", [CustomerCountryKey] = " + m_CustomerCountryKey.ToString())
				oStrUpdate.Append(", [CustomerTelephone] = " + Db.Quoted(m_CustomerTelephone.ToString()))
				oStrUpdate.Append(", [CustomerEmail] = " + Db.Quoted(m_CustomerEmail.ToString()))
				oStrUpdate.Append(", [CustomerContact] = " + Db.Quoted(m_CustomerContact.ToString()))
				oStrUpdate.Append(", [CustomerAccountHandler] = " + m_CustomerAccountHandler.ToString())
				oStrUpdate.Append(", [IsActive] = " + Iif(m_IsActive, "1", "0"))
				oStrUpdate.Append(", [IsDeleted] = " + Iif(m_IsDeleted, "1", "0"))
            oStrUpdate.Append(", [CreatedOn] = " + Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString()))
				oStrUpdate.Append(", [CreatedBy] = " + m_CreatedBy.ToString())
				oStrUpdate.Append(" WHERE [id] = " + ID_id.ToString())

				Db.SqlAction(oStrUpdate.ToString())

				RetVal = ID_id

			Catch
				Throw New Exception ("Update failed.")
			End Try

			Return RetVal
		End Function
		
		Public Sub Delete(ByVal ID_id As Int32)
			Try
				Load(ID_id)
				Db.SqlAction("DELETE FROM FF_Customer WHERE ID = " + ID_id.ToString())

			Catch
				Throw New Exception ("Deleted failed.")
			End Try
		End Sub

#End Region

#Region	"User methods"

#End Region

	End Class

#Region	"Schema"
		
		'	Table: FF_Customer
		'	
		'	  1: id                                      System.Int32 (4)
		'	  2: ExternalCustomerKey                     System.Int32 (4)
		'	  3: CustomerCode                            System.String (50)
		'	  4: CustomerName                            System.String (100)
		'	  5: CustomerAddr1                           System.String (100)
		'	  6: CustomerAddr2                           System.String (100)
		'	  7: CustomerAddr3                           System.String (100)
		'	  8: CustomerTown                            System.String (100)
		'	  9: CustomerPostcode                        System.String (100)
		'	 10: CustomerCountryKey                      System.Int32 (4)
		'	 11: CustomerTelephone                       System.String (50)
		'	 12: CustomerEmail                           System.String (100)
		'	 13: CustomerContact                         System.String (100)
		'	 14: CustomerAccountHandler                  System.Int32 (4)
		'	 15: IsActive                                System.Boolean (1)
		'	 16: IsDeleted                               System.Boolean (1)
		'	 17: CreatedOn                               System.DateTime (4)
		'	 18: CreatedBy                               System.Int32 (4)

'IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FF_Customer]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
'DROP TABLE [dbo].[FF_Customer]
'GO
'CREATE TABLE [dbo].[FF_Customer](
'.
'.
') ON [PRIMARY]
	
#End Region

