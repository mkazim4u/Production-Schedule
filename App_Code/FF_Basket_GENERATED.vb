Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data

'	-----------------------------------------------------------------------------
'	Table:			FF_Basket
'	Created:		20 Aug 2010 11:32:41
'	-----------------------------------------------------------------------------

	Public Class FF_Basket
	
#Region	"Declarations"

        Private Const BASE_DATE As String = "1-Jan-1900"

		Private m_id As Int32
		Private m_JobId As Int32
		Private m_CustomerKey As Int32
		Private m_LogisticProductKey As Int32
		Private m_Qty As Int32
		Private m_IsPicked As Boolean
		Private m_PickDateTime As DateTime

#End Region

#Region	"Private methods"

        ''' <summary>
        ''' Initialises properties to default values.
        ''' </summary>
		Private Sub Init()
			Try
			   	m_id = 0
			   	m_JobId = 0
			   	m_CustomerKey = 0
			   	m_LogisticProductKey = 0
			   	m_Qty = 0
			   	m_IsPicked = False
			   	m_PickDateTime = DateTime.Now
               m_PickDateTime = DateTime.Parse(BASE_DATE)
        Catch ex As Exception
            Throw New Exception("Init failed. " & ex.Message)
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

		Public Property JobId() As Int32
			Get
				Return m_JobId
			End Get
			Set(ByVal value As Int32)
				m_JobId = value
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

		Public Property LogisticProductKey() As Int32
			Get
				Return m_LogisticProductKey
			End Get
			Set(ByVal value As Int32)
				m_LogisticProductKey = value
			End Set
		End Property

		Public Property Qty() As Int32
			Get
				Return m_Qty
			End Get
			Set(ByVal value As Int32)
				m_Qty = value
			End Set
		End Property

		Public Property IsPicked() As Boolean
			Get
				Return m_IsPicked
			End Get
			Set(ByVal value As Boolean)
				m_IsPicked = value
			End Set
		End Property

		Public Property PickDateTime() As DateTime
			Get
				Return m_PickDateTime
			End Get
			Set(ByVal value As DateTime)
				m_PickDateTime = value
			End Set
		End Property

#End Region

#Region	"Constructors"

        ''' <summary>
        ''' Constructor which initialises the properties to default values.
        ''' </summary>
		Public Sub New()
			Try
				Init()

                        Catch ex As Exception
                            Throw New Exception("New() failed. " & ex.Message)
			End Try
		End Sub

        ''' <summary>
        ''' Constructor which loads the record from the given identity field.
        ''' </summary>
		Public Sub New(ByVal ID_id AS Int32)
			Try
				Load(ID_id)

                        Catch ex As Exception
                            Throw New Exception("New(id) failed. " & ex.Message)
			End Try
		End Sub

#End Region

#Region	"Public methods"

        ''' <summary>
        ''' Load record which has the given identity.
        ''' </summary>
		Public Function Load(ByVal ID_id As Int32) As Int32
			Dim RetVal As Int32 = 0

			Try
				Init()
				
				Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_Basket WHERE id = " + ID_id.ToString())

				If Db.HasRows(oDs) Then
					Dim oDr As DataRow = oDs.Tables(0).Rows(0)

					m_id = Db.ReplaceNull(oDr("id"), 0)
					m_JobId = Db.ReplaceNull(oDr("JobId"), 0)
					m_CustomerKey = Db.ReplaceNull(oDr("CustomerKey"), 0)
					m_LogisticProductKey = Db.ReplaceNull(oDr("LogisticProductKey"), 0)
					m_Qty = Db.ReplaceNull(oDr("Qty"), 0)
					m_IsPicked = Db.ReplaceNull(oDr("IsPicked"), False)
					m_PickDateTime = Db.ReplaceNull(oDr("PickDateTime"), DateTime.Now)

					RetVal = m_id
				End If

                        Catch ex As Exception
                            Throw New Exception("Load failed. " & ex.Message)
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

                        Catch ex As Exception
                            Throw New Exception("Save failed. " & ex.Message)
			End Try

			Return RetVal
		End Function

        ''' <summary>
        ''' INSERTs a new record into [FF_Basket].
        ''' </summary>
		Public Function Add() As Int32
			Dim RetVal As Int32 = 0

			Try
				Dim oStrInsert As New StringBuilder()

				oStrInsert.Append("INSERT INTO [FF_Basket] (")
				oStrInsert.Append("[JobId]")
				oStrInsert.Append(", [CustomerKey]")
				oStrInsert.Append(", [LogisticProductKey]")
				oStrInsert.Append(", [Qty]")
				oStrInsert.Append(", [IsPicked]")
				oStrInsert.Append(", [PickDateTime]")
				oStrInsert.Append(") VALUES (")
				oStrInsert.Append(string.format("{0}", m_JobId.ToString()))
				oStrInsert.Append(string.format(", {0}", m_CustomerKey.ToString()))
				oStrInsert.Append(string.format(", {0}", m_LogisticProductKey.ToString()))
				oStrInsert.Append(string.format(", {0}", m_Qty.ToString()))
				oStrInsert.Append(string.format(", {0}", Iif(m_IsPicked, "1", "0")))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
				oStrInsert.Append(");")

				Db.SqlAction(oStrInsert.ToString())

				Dim oDs AS DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_Basket")
				
				If Db.HasRows(oDs) Then
					RetVal = Db.ReplaceNull(oDs.Tables(0).Rows(0)(0), 0)
				End If
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

				oStrUpdate.Append("UPDATE [FF_Basket] SET ")
				oStrUpdate.Append("[JobId] = " + m_JobId.ToString())
				oStrUpdate.Append(", [CustomerKey] = " + m_CustomerKey.ToString())
				oStrUpdate.Append(", [LogisticProductKey] = " + m_LogisticProductKey.ToString())
				oStrUpdate.Append(", [Qty] = " + m_Qty.ToString())
				oStrUpdate.Append(", [IsPicked] = " + Iif(m_IsPicked, "1", "0"))
				oStrUpdate.Append(", [PickDateTime] = " + Db.Quoted(m_PickDateTime.ToLongDateString() + " " + m_PickDateTime.ToLongTimeString()))
				oStrUpdate.Append(" WHERE [id] = " + ID_id.ToString())

				Db.SqlAction(oStrUpdate.ToString())

				RetVal = ID_id

                        Catch ex As Exception
                            Throw New Exception("Update(id) failed. " & ex.Message)
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
				Db.SqlAction("DELETE FROM [FF_Basket] WHERE [id] = " + ID_id.ToString())

                        Catch ex As Exception
                            Throw New Exception("Delete(id) failed. " & ex.Message)
			End Try
		End Sub

#End Region

#Region	"Schema"

		'	Table: FF_Basket
		'	
		'	  1: id                                      System.Int32 (4)
		'	  2: JobId                                   System.Int32 (4)
		'	  3: CustomerKey                             System.Int32 (4)
		'	  4: LogisticProductKey                      System.Int32 (4)
		'	  5: Qty                                     System.Int32 (4)
		'	  6: IsPicked                                System.Boolean (1)
		'	  7: PickDateTime                            System.DateTime (4)

'IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FF_Basket]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
'DROP TABLE [dbo].[FF_Basket]
'GO
'CREATE TABLE [dbo].[FF_Basket](
'.
'.
') ON [PRIMARY]

#End Region

	End Class























