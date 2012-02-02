Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data

'	-----------------------------------------------------------------------------
'	Created:		11 Aug 2010 09:03:04
'	Description:	DNN542c
'	File location:	
'	Table:			FF_Note
'	-----------------------------------------------------------------------------

	Public Class FF_Note
	
#Region	"Declarations"

		Private m_id As Int32
		Private m_JobID As Int32
		Private m_Note As String
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
			   	m_Note = ""
			   	m_CreatedBy = 0
			   	m_CreatedOn = DateTime.Now

			Catch ex As Exception
				Throw New Exception
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

		Public Property Note() As String
			Get
				Return m_Note
			End Get
			Set(ByVal value As String)
				m_Note = value
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

			Catch
				Throw
			End Try
		End Sub

        ''' <summary>
        ''' Constructor which loads the record from the given identity field.
        ''' </summary>
		Public Sub New(ByVal ID_id AS Int32)
			Try
				Load(ID_id)

			Catch
				Throw
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
				
				Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_Note WHERE id = " + ID_id.ToString())

				If Db.HasRows(oDs) Then
					Dim oDr As DataRow = oDs.Tables(0).Rows(0)

					m_id = Db.ReplaceNull(oDr("id"), 0)
					m_JobID = Db.ReplaceNull(oDr("JobID"), 0)
					m_Note = Db.ReplaceNull(oDr("Note"), "")
					m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)
					m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)

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

        Catch ex As Exception
            Throw New Exception("Save failed. " & ex.Message)
			End Try

			Return RetVal
		End Function

        ''' <summary>
        ''' INSERTs a new record into [FF_Note].
        ''' </summary>
		Public Function Add() As Int32
			Dim RetVal As Int32 = 0

			Try
				Dim oStrInsert As New StringBuilder()

				oStrInsert.Append("INSERT INTO [FF_Note] (")
				oStrInsert.Append("[JobID]")
				oStrInsert.Append(", [Note]")
				oStrInsert.Append(", [CreatedBy]")
				oStrInsert.Append(", [CreatedOn]")
				oStrInsert.Append(") VALUES (")
				oStrInsert.Append(string.format("{0}", m_JobID.ToString()))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_Note.ToString())))
				oStrInsert.Append(string.format(", {0}", m_CreatedBy.ToString()))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_CreatedOn.ToShortDateString() + " " + m_CreatedOn.ToShortTimeString())))
				oStrInsert.Append(");")

				Db.SqlAction(oStrInsert.ToString())

				Dim oDs AS DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_Note")
				
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
		Public Function Update(ByVal ID_id As Int32) As Int32
			Dim RetVal As Int32 = 0

			Try
				Dim oStrUpdate As New StringBuilder()

				oStrUpdate.Append("UPDATE [FF_Note] SET ")
				oStrUpdate.Append("[JobID] = " + m_JobID.ToString())
				oStrUpdate.Append(", [Note] = " + Db.Quoted(m_Note.ToString()))
				oStrUpdate.Append(", [CreatedBy] = " + m_CreatedBy.ToString())
				oStrUpdate.Append(", [CreatedOn] = " + Db.Quoted(m_CreatedOn.ToLongDateString() + " " + m_CreatedOn.ToLongTimeString()))
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
				Db.SqlAction("DELETE FROM [FF_Note] WHERE [id] = " + ID_id.ToString())

			Catch
				Throw
			End Try
		End Sub

#End Region

#Region	"Schema"

		'	Table: FF_Note
		'	
		'	  1: id                                      System.Int32 (4)
		'	  2: JobID                                   System.Int32 (4)
		'	  3: Note                                    System.String (4000)
		'	  4: CreatedBy                               System.Int32 (4)
		'	  5: CreatedOn                               System.DateTime (4)

'IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FF_Note]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
'DROP TABLE [dbo].[FF_Note]
'GO
'CREATE TABLE [dbo].[FF_Note](
'.
'.
') ON [PRIMARY]

#End Region

	End Class


