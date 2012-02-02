Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data

'	-----------------------------------------------------------------------------
'	Table:			FF_JobState
'	Created:		05 Aug 2010 13:20:22
'	Server:			VOSTRO
'	Database:		 DNN542c
'	File location:		
'	-----------------------------------------------------------------------------

Public Class FF_JobState

#Region "Declarations"

    Private Const BASE_DATE As String = "1-Jan-1900"

    Private m_id As Int32
    Private m_JobStateName As String
    Private m_JobStateOnCompletionAction As String
    Private m_JobStateOnCompletionNotify As String
    Private m_JobStateUncompletedAlertDateTime As DateTime
    Private m_JobID As Int32
    Private m_Position As Int32
    Private m_IsCompleted As Boolean

#End Region

#Region "Private methods"

    Private Sub Init()
        Try
            m_id = 0
            m_JobStateName = ""
            m_JobStateOnCompletionAction = ""
            m_JobStateOnCompletionNotify = ""
            m_JobStateUncompletedAlertDateTime = DateTime.Now
            m_JobID = 0
            m_Position = 0
            m_IsCompleted = False

        Catch
            Throw New Exception("Init failed.")
        End Try
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property id() As Int32
        Get
            Return m_id
        End Get
    End Property

    Public Property JobStateName() As String
        Get
            Return m_JobStateName
        End Get
        Set(ByVal value As String)
            m_JobStateName = value
        End Set
    End Property

    Public Property JobStateOnCompletionAction() As String
        Get
            Return m_JobStateOnCompletionAction
        End Get
        Set(ByVal value As String)
            m_JobStateOnCompletionAction = value
        End Set
    End Property

    Public Property JobStateOnCompletionNotify() As String
        Get
            Return m_JobStateOnCompletionNotify
        End Get
        Set(ByVal value As String)
            m_JobStateOnCompletionNotify = value
        End Set
    End Property

    Public Property JobStateUncompletedAlertDateTime() As DateTime
        Get
            Return m_JobStateUncompletedAlertDateTime
        End Get
        Set(ByVal value As DateTime)
            m_JobStateUncompletedAlertDateTime = value
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

    Public Property Position() As Int32
        Get
            Return m_Position
        End Get
        Set(ByVal value As Int32)
            m_Position = value
        End Set
    End Property

    Public Property IsCompleted() As Boolean
        Get
            Return m_IsCompleted
        End Get
        Set(ByVal value As Boolean)
            m_IsCompleted = value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        Try
            Init()

        Catch
            Throw New Exception("Constructor failed.")
        End Try
    End Sub

    Public Sub New(ByVal ID_id As Int32)
        Try
            Load(ID_id)

        Catch
            Throw New Exception("Constructor for existing record failed.")
        End Try
    End Sub

#End Region

#Region "Public methods"

    Public Function Load(ByVal ID_id As Int32) As Int32
        Dim RetVal As Int32 = 0

        Try
            Init()

            Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_JobState WHERE id = " + ID_id.ToString())

            If Db.HasRows(oDs) Then
                Dim oDr As DataRow = oDs.Tables(0).Rows(0)

                m_id = Db.ReplaceNull(oDr("id"), 0)
                m_JobStateName = Db.ReplaceNull(oDr("JobStateName"), "")
                m_JobStateOnCompletionAction = Db.ReplaceNull(oDr("JobStateOnCompletionAction"), "")
                m_JobStateOnCompletionNotify = Db.ReplaceNull(oDr("JobStateOnCompletionNotify"), "")
                m_JobStateUncompletedAlertDateTime = Db.ReplaceNull(oDr("JobStateUncompletedAlertDateTime"), DateTime.Now)
                m_JobID = Db.ReplaceNull(oDr("JobID"), 0)
                m_Position = Db.ReplaceNull(oDr("Position"), 0)
                m_IsCompleted = Db.ReplaceNull(oDr("IsCompleted"), False)

                RetVal = m_id
            End If

        Catch
            Throw New Exception("Load failed.")
        End Try

        Return m_id
    End Function

    Public Function Add() As Int32
        Dim RetVal As Int64 = 0

        Try
            Dim oStrInsert As New StringBuilder()

            oStrInsert.Append("INSERT INTO FF_JobState (")
            oStrInsert.Append("JobStateName")
            oStrInsert.Append(", JobStateOnCompletionAction")
            oStrInsert.Append(", JobStateOnCompletionNotify")
            oStrInsert.Append(", JobStateUncompletedAlertDateTime")
            oStrInsert.Append(", JobID")
            oStrInsert.Append(", Position")
            oStrInsert.Append(", IsCompleted")
            oStrInsert.Append(") VALUES (")
            oStrInsert.Append(String.format("{0}", Db.Quoted(m_JobStateName.ToString())))
            oStrInsert.Append(String.format(", {0}", Db.Quoted(m_JobStateOnCompletionAction.ToString())))
            oStrInsert.Append(String.format(", {0}", Db.Quoted(m_JobStateOnCompletionNotify.ToString())))
            oStrInsert.Append(String.format(", {0}", Db.Quoted(m_JobStateUncompletedAlertDateTime.ToLongDateString() + " " + m_JobStateUncompletedAlertDateTime.ToLongTimeString())))
            oStrInsert.Append(String.format(", {0}", m_JobID.ToString()))
            oStrInsert.Append(String.format(", {0}", m_Position.ToString()))
            oStrInsert.Append(String.format(", {0}", Iif(m_IsCompleted, 1, 0)))
            oStrInsert.Append(");")

            Db.SqlAction(oStrInsert.ToString())

            Dim oDs As DataSet = Db.SqlQuery("SELECT MAX(id) FROM FF_JobState")

            If Db.HasRows(oDs) Then
                RetVal = Db.ReplaceNull(oDs.Tables(0).Rows(0)(0), 0)
            End If

        Catch
            Throw New Exception("Add failed.")
        End Try

        Return RetVal
    End Function

    Public Function Update(ByVal ID_id As Int32) As Int32
        Dim RetVal As Int64 = 0

        Try
            Dim oStrUpdate As New StringBuilder()

            oStrUpdate.Append("UPDATE FF_JobState SET ")
            oStrUpdate.Append("JobStateName = " + Db.Quoted(m_JobStateName.ToString()))
            oStrUpdate.Append(", JobStateOnCompletionAction = " + Db.Quoted(m_JobStateOnCompletionAction.ToString()))
            oStrUpdate.Append(", JobStateOnCompletionNotify = " + Db.Quoted(m_JobStateOnCompletionNotify.ToString()))
            oStrUpdate.Append(", JobStateUncompletedAlertDateTime = " + Db.Quoted(m_JobStateUncompletedAlertDateTime.ToShortDateString() + " " + m_JobStateUncompletedAlertDateTime.ToShortTimeString()))
            oStrUpdate.Append(", JobID = " + m_JobID.ToString())
            oStrUpdate.Append(", Position = " + m_Position.ToString())
            oStrUpdate.Append(", IsCompleted = " + IIf(m_IsCompleted, "1", "0"))
            oStrUpdate.Append(" WHERE id = " + ID_id.ToString())

            Db.SqlAction(oStrUpdate.ToString())

            RetVal = ID_id

        Catch
            Throw New Exception("Update failed.")
        End Try

        Return RetVal
    End Function

    Public Sub Delete(ByVal ID_id As Int32)
        Try
            Load(ID_id)
            Db.SqlAction("DELETE FROM FF_JobState WHERE ID = " + ID_id.ToString())

        Catch
            Throw New Exception("Deleted failed.")
        End Try
    End Sub

#End Region

    Public Sub New(ByVal sJobStateName As String, ByVal sJobStateOnCompletionAction As String, ByVal sJobStateOnCompletionNotify As String, ByVal nJobID As Integer, ByVal nPosition As Integer, ByVal bIsCompleted As Boolean)
        m_JobStateName = sJobStateName
        m_JobStateOnCompletionAction = sJobStateOnCompletionAction
        m_JobStateOnCompletionNotify = sJobStateOnCompletionNotify
        m_JobID = nJobID
        m_Position = nPosition
        m_IsCompleted = bIsCompleted
    End Sub

End Class

#Region "Schema"

'	Table: FF_JobState
'	
'	  1: id                                      System.Int32 (4)
'	  2: JobStateName                            System.String (50)
'	  3: JobStateOnCompletionAction              System.String (50)
'	  4: JobStateOnCompletionNotify              System.String (50)
'	  5: JobStateUncompletedAlertDateTime        System.DateTime (4)
'	  6: JobID                                   System.Int32 (4)
'	  7: Position                                System.Int32 (4)
'	  8: IsCompleted                             System.Boolean (1)

'IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FF_JobState]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
'DROP TABLE [dbo].[FF_JobState]
'GO
'CREATE TABLE [dbo].[FF_JobState](
'.
'.
') ON [PRIMARY]

#End Region