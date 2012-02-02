Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data

'	-----------------------------------------------------------------------------
'	Table:			FF_AuditTrail
'	Created:		26 Aug 2010 14:06:31
'	-----------------------------------------------------------------------------

	Public Class FF_AuditTrail
	
#Region	"Declarations"

		Private m_id As Int32
		Private m_RecordType As String
		Private m_SourceId As Int32
		Private m_AuditEvent As String
		Private m_ChangeDetail As String
		Private m_CreatedOn As DateTime
		Private m_CreatedBy As Int32
        Private Const BASE_DATE As String = "1-Jan-1900"

#End Region

#Region	"Private methods"

        ''' <summary>
        ''' Initialises properties to default values.
        ''' </summary>
		Private Sub Init()
			Try
			   	m_id = 0
			   	m_RecordType = ""
			   	m_SourceId = 0
			   	m_AuditEvent = ""
			   	m_ChangeDetail = ""
			   	m_CreatedOn = DateTime.Now
			   	m_CreatedBy = 0
               m_CreatedOn = DateTime.Now
        Catch ex As Exception
            Throw New Exception("Init failed. " & ex.Message)
			End Try
		End Sub

#End Region

#Region	"Properties"

		Friend ReadOnly Property id() As Int32
			Get
				Return m_id
			End Get
		End Property

		Friend Property RecordType() As String
			Get
				Return m_RecordType
			End Get
			Set(ByVal value As String)
				m_RecordType = value
			End Set
		End Property

		Friend Property SourceId() As Int32
			Get
				Return m_SourceId
			End Get
			Set(ByVal value As Int32)
				m_SourceId = value
			End Set
		End Property

		Friend Property AuditEvent() As String
			Get
				Return m_AuditEvent
			End Get
			Set(ByVal value As String)
				m_AuditEvent = value
			End Set
		End Property

		Friend Property ChangeDetail() As String
			Get
				Return m_ChangeDetail
			End Get
			Set(ByVal value As String)
				m_ChangeDetail = value
			End Set
		End Property

		Friend Property CreatedOn() As DateTime
			Get
				Return m_CreatedOn
			End Get
			Set(ByVal value As DateTime)
				m_CreatedOn = value
			End Set
		End Property

		Friend Property CreatedBy() As Int32
			Get
				Return m_CreatedBy
			End Get
			Set(ByVal value As Int32)
				m_CreatedBy = value
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
				
				Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_AuditTrail WHERE id = " + ID_id.ToString())

				If Db.HasRows(oDs) Then
					Dim oDr As DataRow = oDs.Tables(0).Rows(0)

					m_id = Db.ReplaceNull(oDr("id"), 0)
					m_RecordType = Db.ReplaceNull(oDr("RecordType"), "")
					m_SourceId = Db.ReplaceNull(oDr("SourceId"), 0)
					m_AuditEvent = Db.ReplaceNull(oDr("AuditEvent"), "")
					m_ChangeDetail = Db.ReplaceNull(oDr("ChangeDetail"), "")
					m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)
					m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)

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
        ''' INSERTs a new record into [FF_AuditTrail].
        ''' </summary>
		Public Function Add() As Int32
			Dim RetVal As Int32 = 0

			Try
				Dim oStrInsert As New StringBuilder()

				oStrInsert.Append("INSERT INTO [FF_AuditTrail] (")
				oStrInsert.Append("[RecordType]")
				oStrInsert.Append(", [SourceId]")
				oStrInsert.Append(", [AuditEvent]")
				oStrInsert.Append(", [ChangeDetail]")
				oStrInsert.Append(", [CreatedOn]")
				oStrInsert.Append(", [CreatedBy]")
				oStrInsert.Append(") VALUES (")
				oStrInsert.Append(string.format("{0}", Db.Quoted(m_RecordType.ToString())))
				oStrInsert.Append(string.format(", {0}", m_SourceId.ToString()))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_AuditEvent.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_ChangeDetail.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
				oStrInsert.Append(string.format(", {0}", m_CreatedBy.ToString()))
				oStrInsert.Append(");")

				Db.SqlAction(oStrInsert.ToString())

				Dim oDs AS DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_AuditTrail")
				
				If Db.HasRows(oDs) Then
					RetVal = Db.ReplaceNull(oDs.Tables(0).Rows(0)(0), 0)
				End If
                        Catch ex As Exception
            Throw New Exception(Me.ToString() & " Add failed. " & ex.Message)
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

				oStrUpdate.Append("UPDATE [FF_AuditTrail] SET ")
				oStrUpdate.Append("[RecordType] = " + Db.Quoted(m_RecordType.ToString()))
				oStrUpdate.Append(", [SourceId] = " + m_SourceId.ToString())
				oStrUpdate.Append(", [AuditEvent] = " + Db.Quoted(m_AuditEvent.ToString()))
				oStrUpdate.Append(", [ChangeDetail] = " + Db.Quoted(m_ChangeDetail.ToString()))
				oStrUpdate.Append(", [CreatedOn] = " + Db.Quoted(m_CreatedOn.ToLongDateString() + " " + m_CreatedOn.ToLongTimeString()))
				oStrUpdate.Append(", [CreatedBy] = " + m_CreatedBy.ToString())
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
				Db.SqlAction("DELETE FROM [FF_AuditTrail] WHERE [id] = " + ID_id.ToString())

                        Catch ex As Exception
                            Throw New Exception("Delete(id) failed. " & ex.Message)
			End Try
		End Sub

#End Region

#Region	"Schema"

		'	Table: FF_AuditTrail
		'	
		'	  1: id                                      System.Int32 (4)
		'	  2: RecordType                              System.String (1)
		'	  3: SourceId                                System.Int32 (4)
		'	  4: AuditEvent                              System.String (50)
		'	  5: ChangeDetail                            System.String (500)
		'	  6: CreatedOn                               System.DateTime (4)
		'	  7: CreatedBy                               System.Int32 (4)

'IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FF_AuditTrail]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
'DROP TABLE [dbo].[FF_AuditTrail]
'GO
'CREATE TABLE [dbo].[FF_AuditTrail](
'.
'.
') ON [PRIMARY]

' CHANGE SUMMARY
' Set CreatedOn to DateTime.Now

#End Region

	End Class




















