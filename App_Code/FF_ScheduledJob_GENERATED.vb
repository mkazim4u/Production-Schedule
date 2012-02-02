Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data

'	-----------------------------------------------------------------------------
'	Table:			FF_ScheduledJob
'	Created:		07 Aug 2010 15:00:16
'	Server:			VOSTRO
'	Database:		 DNN542c
'	File location:		
'	-----------------------------------------------------------------------------

	Public Class FF_ScheduledJob
	
#Region	"Declarations"

    Private Const BASE_DATE As String = "1-Jan-1900"

		Private m_id As Int32
		Private m_JobId As Int32
    Private m_IsIntervalInMonths As Boolean
    Private m_IsIntervalInWeeks As Boolean
    Private m_IsIntervalOnDaily As Boolean

		Private m_Interval As Int32
		Private m_LastRun As DateTime
		Private m_NextRun As DateTime
		Private m_CreatedBy As Int32
		Private m_CreatedOn As DateTime

#End Region

#Region	"Private methods"

		Private Sub Init()
			Try
			   	m_id = 0
			   	m_JobId = 0
            m_IsIntervalInMonths = False
            m_IsIntervalInWeeks = False
            m_IsIntervalOnDaily = False

            m_Interval = 0

            m_LastRun = DateTime.Parse(BASE_DATE)
            m_NextRun = DateTime.Parse(BASE_DATE)
            m_CreatedBy = UserController.GetCurrentUserInfo.UserID
			   	m_CreatedOn = DateTime.Now

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

		Public Property JobId() As Int32
			Get
				Return m_JobId
			End Get
			Set(ByVal value As Int32)
				m_JobId = value
			End Set
		End Property

		Public Property IsIntervalInMonths() As Boolean
			Get
				Return m_IsIntervalInMonths
			End Get
			Set(ByVal value As Boolean)
				m_IsIntervalInMonths = value
			End Set
		End Property

		Public Property Interval() As Int32
			Get
				Return m_Interval
			End Get
			Set(ByVal value As Int32)
				m_Interval = value
			End Set
    End Property
    Public Property IsIntervalInWeeks() As Boolean
        Get
            Return m_IsIntervalInWeeks
        End Get
        Set(ByVal value As Boolean)
            m_IsIntervalInWeeks = value
        End Set
    End Property

    Public Property IsIntervalOnDaily() As Boolean
        Get
            Return m_IsIntervalOnDaily
        End Get
        Set(ByVal value As Boolean)
            m_IsIntervalOnDaily = value
        End Set
    End Property

		Public Property LastRun() As DateTime
			Get
				Return m_LastRun
			End Get
			Set(ByVal value As DateTime)
				m_LastRun = value
			End Set
		End Property

		Public Property NextRun() As DateTime
			Get
				Return m_NextRun
			End Get
			Set(ByVal value As DateTime)
				m_NextRun = value
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
				
				Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_ScheduledJob WHERE id = " + ID_id.ToString())

				If Db.HasRows(oDs) Then
					Dim oDr As DataRow = oDs.Tables(0).Rows(0)

					m_id = Db.ReplaceNull(oDr("id"), 0)
					m_JobId = Db.ReplaceNull(oDr("JobId"), 0)
                m_IsIntervalInMonths = Db.ReplaceNull(oDr("IsIntervalInMonths"), False)
                m_IsIntervalInWeeks = Db.ReplaceNull(oDr("IsIntervalInWeeks"), False)
                m_IsIntervalOnDaily = Db.ReplaceNull(oDr("IsIntervalOnDaily"), False)
					m_Interval = Db.ReplaceNull(oDr("Interval"), 0)
					m_LastRun = Db.ReplaceNull(oDr("LastRun"), DateTime.Now)
					m_NextRun = Db.ReplaceNull(oDr("NextRun"), DateTime.Now)
					m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)
					m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)

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

				oStrInsert.Append("INSERT INTO FF_ScheduledJob (")
				oStrInsert.Append("JobId")
            oStrInsert.Append(", [IsIntervalInMonths]")
            oStrInsert.Append(", [IsIntervalInWeeks]")
            oStrInsert.Append(", [IsIntervalOnDaily]")
				oStrInsert.Append(", Interval")
				oStrInsert.Append(", LastRun")
				oStrInsert.Append(", NextRun")
				oStrInsert.Append(", CreatedBy")
				oStrInsert.Append(", CreatedOn")
				oStrInsert.Append(") VALUES (")
				oStrInsert.Append(string.format("{0}", m_JobId.ToString()))
            oStrInsert.Append(String.Format(", {0}", IIf(m_IsIntervalInMonths, "1", "0")))
            oStrInsert.Append(String.Format(", {0}", IIf(m_IsIntervalInWeeks, "1", "0")))
            oStrInsert.Append(String.Format(", {0}", IIf(m_IsIntervalOnDaily, "1", "0")))
				oStrInsert.Append(string.format(", {0}", m_Interval.ToString()))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_LastRun.ToShortDateString() + " " + m_LastRun.ToShortTimeString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_NextRun.ToShortDateString() + " " + m_NextRun.ToShortTimeString())))
				oStrInsert.Append(string.format(", {0}", m_CreatedBy.ToString()))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
				oStrInsert.Append(");")

				Db.SqlAction(oStrInsert.ToString())
				
				Dim oDs AS DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_ScheduledJob")
				
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

				oStrUpdate.Append("UPDATE FF_ScheduledJob SET ")
				oStrUpdate.Append("JobId = " + m_JobId.ToString())
            oStrUpdate.Append(", [IsIntervalInMonths] = " + IIf(m_IsIntervalInMonths, "1", "0"))
            oStrUpdate.Append(", [IsIntervalInWeeks] = " + IIf(m_IsIntervalInWeeks, "1", "0"))
            oStrUpdate.Append(", [IsIntervalOnDaily] = " + IIf(m_IsIntervalOnDaily, "1", "0"))

				oStrUpdate.Append(", Interval = " + m_Interval.ToString())
            oStrUpdate.Append(", LastRun = " + Db.Quoted(m_LastRun.ToShortDateString() + " " + m_LastRun.ToShortTimeString()))
            oStrUpdate.Append(", NextRun = " + Db.Quoted(m_NextRun.ToShortDateString() + " " + m_NextRun.ToShortTimeString()))
				oStrUpdate.Append(", CreatedBy = " + m_CreatedBy.ToString())
            oStrUpdate.Append(", CreatedOn = " + Db.Quoted(m_CreatedOn.ToShortDateString() + " " + m_CreatedOn.ToShortTimeString()))
				oStrUpdate.Append(" WHERE id = " + ID_id.ToString())

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
				Db.SqlAction("DELETE FROM FF_ScheduledJob WHERE ID = " + ID_id.ToString())

			Catch
				Throw New Exception ("Deleted failed.")
			End Try
		End Sub

#End Region

End Class

#Region	"Schema"
		
		'	Table: FF_ScheduledJob
		'	
		'	  1: id                                      System.Int32 (4)
		'	  2: JobId                                   System.Int32 (4)
		'	  3: IsIntervalInMonths                      System.Boolean (1)
		'	  4: Interval                                System.Int32 (4)
		'	  5: LastRun                                 System.DateTime (4)
		'	  6: NextRun                                 System.DateTime (4)
		'	  7: CreatedBy                               System.Int32 (4)
		'	  8: CreatedOn                               System.DateTime (4)

'IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FF_ScheduledJob]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
'DROP TABLE [dbo].[FF_ScheduledJob]
'GO
'CREATE TABLE [dbo].[FF_ScheduledJob](
'.
'.
') ON [PRIMARY]
	
#End Region

