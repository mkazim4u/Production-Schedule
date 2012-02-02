Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data

'	-----------------------------------------------------------------------------
'	Table:			FF_Event
'	Created:		11 Aug 2010 21:10:18
'	-----------------------------------------------------------------------------

	Public Class FF_Event
	
#Region	"Declarations"

		Private m_id As Int32
		Private m_JobID As Int32
		Private m_EventDescription As String
		Private m_CreatedBy As Int32
		Private m_CreatedOn As DateTime

#End Region

#Region	"Private methods"

        ''' <summary>
        ''' Initialises properties to default values.
        ''' </summary>
		Private Sub Init()
			Try
			   	m_id = 0
			   	m_JobID = 0
			   	m_EventDescription = ""
			   	m_CreatedBy = 0
			   	m_CreatedOn = DateTime.Now

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

		Public Property JobID() As Int32
			Get
				Return m_JobID
			End Get
			Set(ByVal value As Int32)
				m_JobID = value
			End Set
		End Property

		Public Property EventDescription() As String
			Get
				Return m_EventDescription
			End Get
			Set(ByVal value As String)
				m_EventDescription = value
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

		Public Property CreatedOn() As DateTime
			Get
				Return m_CreatedOn
			End Get
			Set(ByVal value As DateTime)
				m_CreatedOn = value
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
				
				Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_Event WHERE id = " + ID_id.ToString())

				If Db.HasRows(oDs) Then
					Dim oDr As DataRow = oDs.Tables(0).Rows(0)

					m_id = Db.ReplaceNull(oDr("id"), 0)
					m_JobID = Db.ReplaceNull(oDr("JobID"), 0)
					m_EventDescription = Db.ReplaceNull(oDr("EventDescription"), "")
					m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)
					m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)

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
        ''' INSERTs a new record into [FF_Event].
        ''' </summary>
		Public Function Add() As Int32
			Dim RetVal As Int32 = 0

			Try
				Dim oStrInsert As New StringBuilder()

				oStrInsert.Append("INSERT INTO [FF_Event] (")
				oStrInsert.Append("[JobID]")
				oStrInsert.Append(", [EventDescription]")
				oStrInsert.Append(", [CreatedBy]")
				oStrInsert.Append(", [CreatedOn]")
				oStrInsert.Append(") VALUES (")
				oStrInsert.Append(string.format("{0}", m_JobID.ToString()))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_EventDescription.ToString())))
				oStrInsert.Append(string.format(", {0}", m_CreatedBy.ToString()))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CreatedOn.ToLongDateString() + " " + m_CreatedOn.ToLongTimeString())))
				oStrInsert.Append(");")

				Db.SqlAction(oStrInsert.ToString())

				Dim oDs AS DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_Event")
				
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

				oStrUpdate.Append("UPDATE [FF_Event] SET ")
				oStrUpdate.Append("[JobID] = " + m_JobID.ToString())
				oStrUpdate.Append(", [EventDescription] = " + Db.Quoted(m_EventDescription.ToString()))
				oStrUpdate.Append(", [CreatedBy] = " + m_CreatedBy.ToString())
				oStrUpdate.Append(", [CreatedOn] = " + Db.Quoted(m_CreatedOn.ToLongDateString() + " " + m_CreatedOn.ToLongTimeString()))
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
				Db.SqlAction("DELETE FROM [FF_Event] WHERE [id] = " + ID_id.ToString())

                        Catch ex As Exception
                            Throw New Exception("Delete(id) failed. " & ex.Message)
			End Try
		End Sub

#End Region

#Region	"Schema"

		'	Table: FF_Event
		'	
		'	  1: id                                      System.Int32 (4)
		'	  2: JobID                                   System.Int32 (4)
		'	  3: EventDescription                        System.String (500)
		'	  4: CreatedBy                               System.Int32 (4)
		'	  5: CreatedOn                               System.DateTime (4)

'IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FF_Event]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
'DROP TABLE [dbo].[FF_Event]
'GO
'CREATE TABLE [dbo].[FF_Event](
'.
'.
') ON [PRIMARY]

#End Region

	End Class



