Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Data

'	-----------------------------------------------------------------------------
'	Table:			FF_Job
'	Created:		16 Aug 2010 15:43:23
'	Server:			VOSTRO
'	Database:		DNN542c DNN542c
'	File location:		
'	-----------------------------------------------------------------------------

	Public Class FF_Job
	
#Region	"Declarations"

    Private Const BASE_DATE As String = "1-Jan-1900"

		Private m_ID As Int32
		Private m_JobGUID As String
		Private m_JobName As String
    Private m_CustomerKey As Int32
    Private m_ProductionCost As Decimal
    Private m_DistributionCost As Decimal

		Private m_CustomerContactKey As Int32
		Private m_IsCreated As Boolean
		Private m_IsTemplate As Boolean
		Private m_IsCancelled As Boolean
		Private m_IsDeleted As Boolean
		Private m_AccountHandlerKey As Int32
		Private m_CollateralDueOn As DateTime
		Private m_DeadlineOn As DateTime
		Private m_CompletedOn As DateTime
		Private m_CompletedByUserID As Int32
		Private m_CustCtcName As String
		Private m_CustPhone As String
		Private m_CustEmail As String
		Private m_CustRef1 As String
		Private m_CustRef2 As String
		Private m_ItemCount As Int32
		Private m_MaterialFromExternalSource As String
		Private m_EnvelopePackaging As String
    Private m_Instructions As String
    Private m_Notes As String
		Private m_CreatedOn As DateTime
		Private m_CreatedBy As Int32

#End Region

#Region	"Private methods"

		Private Sub Init()
			Try
			   	m_ID = 0
            m_JobGUID = Guid.NewGuid.ToString
			   	m_JobName = ""
			   	m_CustomerKey = 0
            m_CustomerContactKey = 0
            m_ProductionCost = 0.0
            m_DistributionCost = 0.0

			   	m_IsCreated = False
			   	m_IsTemplate = False
			   	m_IsCancelled = False
			   	m_IsDeleted = False
			   	m_AccountHandlerKey = 0
            m_CollateralDueOn = DateTime.Parse(BASE_DATE)
            m_DeadlineOn = DateTime.Parse(BASE_DATE)
            m_CompletedOn = DateTime.Parse(BASE_DATE)
			   	m_CompletedByUserID = 0
			   	m_CustCtcName = ""
			   	m_CustPhone = ""
			   	m_CustEmail = ""
			   	m_CustRef1 = ""
			   	m_CustRef2 = ""
			   	m_ItemCount = 0
			   	m_MaterialFromExternalSource = ""
			   	m_EnvelopePackaging = ""
            m_Instructions = ""
            m_Notes = ""
			   	m_CreatedOn = DateTime.Now
            m_CreatedBy = UserController.GetCurrentUserInfo.UserID

			Catch
				Throw New Exception ("Init failed.")
			End Try
		End Sub

#End Region

#Region	"Properties"

    Public ReadOnly Property ID() As Int32
			Get
				Return m_ID
			End Get
		End Property

    '  Public ReadOnly Property JobGUID() As String
    '	Get
    '		Return m_JobGUID
    '	End Get
    'End Property

    Public Property JobGUID() As String
        Get
            Return m_JobGUID
        End Get
        Set(ByVal value As String)
            m_JobGUID = value
        End Set
    End Property

		Public Property JobName() As String
			Get
				Return m_JobName
			End Get
			Set(ByVal value As String)
				m_JobName = value
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

		Public Property CustomerContactKey() As Int32
			Get
				Return m_CustomerContactKey
			End Get
			Set(ByVal value As Int32)
				m_CustomerContactKey = value
			End Set
		End Property

		Public Property IsCreated() As Boolean
			Get
				Return m_IsCreated
			End Get
			Set(ByVal value As Boolean)
				m_IsCreated = value
			End Set
		End Property

		Public Property IsTemplate() As Boolean
			Get
				Return m_IsTemplate
			End Get
			Set(ByVal value As Boolean)
				m_IsTemplate = value
			End Set
		End Property

		Public Property IsCancelled() As Boolean
			Get
				Return m_IsCancelled
			End Get
			Set(ByVal value As Boolean)
				m_IsCancelled = value
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

		Public Property AccountHandlerKey() As Int32
			Get
				Return m_AccountHandlerKey
			End Get
			Set(ByVal value As Int32)
				m_AccountHandlerKey = value
			End Set
		End Property

		Public Property CollateralDueOn() As DateTime
			Get
				Return m_CollateralDueOn
			End Get
			Set(ByVal value As DateTime)
				m_CollateralDueOn = value
			End Set
		End Property

		Public Property DeadlineOn() As DateTime
			Get
				Return m_DeadlineOn
			End Get
			Set(ByVal value As DateTime)
				m_DeadlineOn = value
			End Set
		End Property

		Public Property CompletedOn() As DateTime
			Get
				Return m_CompletedOn
			End Get
			Set(ByVal value As DateTime)
				m_CompletedOn = value
			End Set
		End Property

		Public Property CompletedByUserID() As Int32
			Get
				Return m_CompletedByUserID
			End Get
			Set(ByVal value As Int32)
				m_CompletedByUserID = value
			End Set
		End Property

		Public Property CustCtcName() As String
			Get
				Return m_CustCtcName
			End Get
			Set(ByVal value As String)
				m_CustCtcName = value
			End Set
		End Property

		Public Property CustPhone() As String
			Get
				Return m_CustPhone
			End Get
			Set(ByVal value As String)
				m_CustPhone = value
			End Set
		End Property

		Public Property CustEmail() As String
			Get
				Return m_CustEmail
			End Get
			Set(ByVal value As String)
				m_CustEmail = value
			End Set
		End Property

		Public Property CustRef1() As String
			Get
				Return m_CustRef1
			End Get
			Set(ByVal value As String)
				m_CustRef1 = value
			End Set
		End Property

		Public Property CustRef2() As String
			Get
				Return m_CustRef2
			End Get
			Set(ByVal value As String)
				m_CustRef2 = value
			End Set
		End Property

		Public Property ItemCount() As Int32
			Get
				Return m_ItemCount
			End Get
			Set(ByVal value As Int32)
				m_ItemCount = value
			End Set
		End Property

		Public Property MaterialFromExternalSource() As String
			Get
				Return m_MaterialFromExternalSource
			End Get
			Set(ByVal value As String)
				m_MaterialFromExternalSource = value
			End Set
		End Property

		Public Property EnvelopePackaging() As String
			Get
				Return m_EnvelopePackaging
			End Get
			Set(ByVal value As String)
				m_EnvelopePackaging = value
			End Set
		End Property

		Public Property Instructions() As String
			Get
				Return m_Instructions
			End Get
			Set(ByVal value As String)
				m_Instructions = value
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

    Public Property ProductionCost() As Decimal
        Get
            Return m_ProductionCost
        End Get
        Set(ByVal value As Decimal)
            m_ProductionCost = value
        End Set
    End Property

    Public Property DistributionCost() As Decimal
        Get
            Return m_DistributionCost
        End Get
        Set(ByVal value As Decimal)
            m_DistributionCost = value
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

		Public Sub New(ByVal ID_ID AS Int32)
			Try
            GetJobByID(ID_ID)

			Catch
				Throw New Exception ("Constructor for existing record failed.")
			End Try
		End Sub

#End Region

#Region	"Public methods"

    Public Function GetJobByID(ByVal ID_ID As Int32) As Int32
			Dim RetVal As Int32 = 0

			Try
				Init()
				
				Dim oDs As DataSet = Db.SqlQuery("SELECT * FROM FF_Job WHERE ID = " + ID_ID.ToString())

				If Db.HasRows(oDs) Then
					Dim oDr As DataRow = oDs.Tables(0).Rows(0)

					m_ID = Db.ReplaceNull(oDr("ID"), 0)
					m_JobGUID = Db.ReplaceNull(oDr("JobGUID"), "")
					m_JobName = Db.ReplaceNull(oDr("JobName"), "")
					m_CustomerKey = Db.ReplaceNull(oDr("CustomerKey"), 0)
                m_CustomerContactKey = Db.ReplaceNull(oDr("CustomerContactKey"), 0)
                m_ProductionCost = Db.ReplaceNull(oDr("ProductionCost"), 0.0)
                m_DistributionCost = Db.ReplaceNull(oDr("DistributionCost"), 0.0)
					m_IsCreated = Db.ReplaceNull(oDr("IsCreated"), False)
					m_IsTemplate = Db.ReplaceNull(oDr("IsTemplate"), False)
					m_IsCancelled = Db.ReplaceNull(oDr("IsCancelled"), False)
					m_IsDeleted = Db.ReplaceNull(oDr("IsDeleted"), False)
					m_AccountHandlerKey = Db.ReplaceNull(oDr("AccountHandlerKey"), 0)
					m_CollateralDueOn = Db.ReplaceNull(oDr("CollateralDueOn"), DateTime.Now)
					m_DeadlineOn = Db.ReplaceNull(oDr("DeadlineOn"), DateTime.Now)
					m_CompletedOn = Db.ReplaceNull(oDr("CompletedOn"), DateTime.Now)
					m_CompletedByUserID = Db.ReplaceNull(oDr("CompletedByUserID"), 0)
					m_CustCtcName = Db.ReplaceNull(oDr("CustCtcName"), "")
					m_CustPhone = Db.ReplaceNull(oDr("CustPhone"), "")
					m_CustEmail = Db.ReplaceNull(oDr("CustEmail"), "")
					m_CustRef1 = Db.ReplaceNull(oDr("CustRef1"), "")
					m_CustRef2 = Db.ReplaceNull(oDr("CustRef2"), "")
					m_ItemCount = Db.ReplaceNull(oDr("ItemCount"), 0)
					m_MaterialFromExternalSource = Db.ReplaceNull(oDr("MaterialFromExternalSource"), "")
					m_EnvelopePackaging = Db.ReplaceNull(oDr("EnvelopePackaging"), "")
                m_Instructions = Db.ReplaceNull(oDr("Instructions"), "")
                m_Notes = Db.ReplaceNull(oDr("Notes"), "")
					m_CreatedOn = Db.ReplaceNull(oDr("CreatedOn"), DateTime.Now)
					m_CreatedBy = Db.ReplaceNull(oDr("CreatedBy"), 0)

					RetVal = m_ID
				End If

			Catch
            Throw New Exception("GetJobByID failed.")
			End Try

			Return m_ID
		End Function

		Public Function Add() As Int32
			Dim RetVal As Int64 = 0

			Try
				Dim oStrInsert As New StringBuilder()

				oStrInsert.Append("INSERT INTO [FF_Job] (")
				oStrInsert.Append("[JobGUID]")
				oStrInsert.Append(", [JobName]")
            oStrInsert.Append(", [CustomerKey]")
            oStrInsert.Append(", [ProductionCost]")
            oStrInsert.Append(", [DistributionCost]")
				oStrInsert.Append(", [CustomerContactKey]")
				oStrInsert.Append(", [IsCreated]")
				oStrInsert.Append(", [IsTemplate]")
				oStrInsert.Append(", [IsCancelled]")
				oStrInsert.Append(", [IsDeleted]")
				oStrInsert.Append(", [AccountHandlerKey]")
				oStrInsert.Append(", [CollateralDueOn]")
				oStrInsert.Append(", [DeadlineOn]")
				oStrInsert.Append(", [CompletedOn]")
				oStrInsert.Append(", [CompletedByUserID]")
				oStrInsert.Append(", [CustCtcName]")
				oStrInsert.Append(", [CustPhone]")
				oStrInsert.Append(", [CustEmail]")
				oStrInsert.Append(", [CustRef1]")
				oStrInsert.Append(", [CustRef2]")
				oStrInsert.Append(", [ItemCount]")
				oStrInsert.Append(", [MaterialFromExternalSource]")
				oStrInsert.Append(", [EnvelopePackaging]")
            oStrInsert.Append(", [Instructions]")
            oStrInsert.Append(", [Notes]")
				oStrInsert.Append(", [CreatedOn]")
				oStrInsert.Append(", [CreatedBy]")
				oStrInsert.Append(") VALUES (")
				oStrInsert.Append(string.format("{0}", Db.Quoted(m_JobGUID.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_JobName.ToString())))
            oStrInsert.Append(String.Format(", {0}", m_CustomerKey.ToString()))
            oStrInsert.Append(String.Format(", {0}", m_ProductionCost.ToString()))
            oStrInsert.Append(String.Format(", {0}", m_DistributionCost.ToString()))
            oStrInsert.Append(String.Format(", {0}", m_CustomerContactKey.ToString()))
            oStrInsert.Append(String.Format(", {0}", IIf(m_IsCreated, "1", "0")))
				oStrInsert.Append(string.format(", {0}", Iif(m_IsTemplate, "1", "0")))
				oStrInsert.Append(string.format(", {0}", Iif(m_IsCancelled, "1", "0")))
				oStrInsert.Append(string.format(", {0}", Iif(m_IsDeleted, "1", "0")))
				oStrInsert.Append(string.format(", {0}", m_AccountHandlerKey.ToString()))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_CollateralDueOn.ToShortDateString() + " " + m_CollateralDueOn.ToShortTimeString())))
            'oStrInsert.Append(String.Format(", {0}", Db.Quoted(DateTime.Now + " " + DateTime.Now)))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_DeadlineOn.ToShortDateString() + " " + m_DeadlineOn.ToShortTimeString())))
            'oStrInsert.Append(String.Format(", {0}", Db.Quoted(DateTime.Now() + " " + DateTime.Now)))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_CompletedOn.ToShortDateString() + " " + m_CompletedOn.ToShortTimeString())))
            'oStrInsert.Append(String.Format(", {0}", Db.Quoted(DateTime.Now + " " + DateTime.Now)))
				oStrInsert.Append(string.format(", {0}", m_CompletedByUserID.ToString()))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustCtcName.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustPhone.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustEmail.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustRef1.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_CustRef2.ToString())))
				oStrInsert.Append(string.format(", {0}", m_ItemCount.ToString()))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_MaterialFromExternalSource.ToString())))
				oStrInsert.Append(string.format(", {0}", Db.Quoted(m_EnvelopePackaging.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_Instructions.ToString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_Notes.ToString())))
            'oStrInsert.Append(String.Format(", {0}", Db.Quoted(m_CreatedOn.ToLongDateString() + " " + m_CreatedOn.ToLongTimeString())))
            oStrInsert.Append(String.Format(", {0}", Db.Quoted(Date.Now.ToShortDateString() + " " + Date.Now.ToShortTimeString())))
				oStrInsert.Append(string.format(", {0}", m_CreatedBy.ToString()))
				oStrInsert.Append(");")

				Db.SqlAction(oStrInsert.ToString())
				
				Dim oDs AS DataSet = Db.SqlQuery("SELECT MAX(ID) FROM FF_Job")
				
				If Db.HasRows(oDs) Then
					RetVal = Db.ReplaceNull(oDs.Tables(0).Rows(0)(0), 0)
				End If

        Catch ex As Exception
            Throw New Exception("Add failed. FF_Job_Generated" & ex.Message)
			End Try

			Return RetVal
		End Function

		Public Function Update(ByVal ID_ID As Int32) As Int32
			Dim RetVal As Int64 = 0

			Try
				Dim oStrUpdate As New StringBuilder()

				oStrUpdate.Append("UPDATE [FF_Job] SET ")
				oStrUpdate.Append("[JobGUID] = " + Db.Quoted(m_JobGUID.ToString()))
				oStrUpdate.Append(", [JobName] = " + Db.Quoted(m_JobName.ToString()))
            oStrUpdate.Append(", [CustomerKey] = " + m_CustomerKey.ToString())
            oStrUpdate.Append(", [ProductionCost] = " + m_ProductionCost.ToString())
            oStrUpdate.Append(", [DistributionCost] = " + m_DistributionCost.ToString())
				oStrUpdate.Append(", [CustomerContactKey] = " + m_CustomerContactKey.ToString())
				oStrUpdate.Append(", [IsCreated] = " + Iif(m_IsCreated, "1", "0"))
				oStrUpdate.Append(", [IsTemplate] = " + Iif(m_IsTemplate, "1", "0"))
				oStrUpdate.Append(", [IsCancelled] = " + Iif(m_IsCancelled, "1", "0"))
				oStrUpdate.Append(", [IsDeleted] = " + Iif(m_IsDeleted, "1", "0"))
            oStrUpdate.Append(", [AccountHandlerKey] = " + m_AccountHandlerKey.ToString())

            oStrUpdate.Append(", CollateralDueOn = " + Db.Quoted(m_CollateralDueOn.ToShortDateString() + " " + m_CollateralDueOn.ToShortTimeString()))
            'oStrUpdate.Append(String.Format(", {0}", Db.Quoted(m_CollateralDueOn.ToShortDateString() + " " + m_CollateralDueOn.ToShortTimeString())))

            'oStrUpdate.Append(String.Format(", {0}", Db.Quoted(m_DeadlineOn.ToShortDateString() + " " + m_DeadlineOn.ToShortTimeString())))
            oStrUpdate.Append(", DeadlineOn = " + Db.Quoted(m_DeadlineOn.ToShortDateString() + " " + m_DeadlineOn.ToShortTimeString()))

            'oStrUpdate.Append(String.Format(", {0}", Db.Quoted(m_CompletedOn.ToShortDateString() + " " + m_CompletedOn.ToShortTimeString())))

            oStrUpdate.Append(", CompletedOn = " + Db.Quoted(m_CompletedOn.ToShortDateString() + " " + m_CompletedOn.ToShortTimeString()))

            oStrUpdate.Append(", [CompletedByUserID] = " + m_CompletedByUserID.ToString())
				oStrUpdate.Append(", [CustCtcName] = " + Db.Quoted(m_CustCtcName.ToString()))
				oStrUpdate.Append(", [CustPhone] = " + Db.Quoted(m_CustPhone.ToString()))
				oStrUpdate.Append(", [CustEmail] = " + Db.Quoted(m_CustEmail.ToString()))
				oStrUpdate.Append(", [CustRef1] = " + Db.Quoted(m_CustRef1.ToString()))
				oStrUpdate.Append(", [CustRef2] = " + Db.Quoted(m_CustRef2.ToString()))
				oStrUpdate.Append(", [ItemCount] = " + m_ItemCount.ToString())
				oStrUpdate.Append(", [MaterialFromExternalSource] = " + Db.Quoted(m_MaterialFromExternalSource.ToString()))
				oStrUpdate.Append(", [EnvelopePackaging] = " + Db.Quoted(m_EnvelopePackaging.ToString()))
            oStrUpdate.Append(", [Instructions] = " + Db.Quoted(m_Instructions.ToString()))
            oStrUpdate.Append(", [Notes] = " + Db.Quoted(m_Notes.ToString()))

            'oStrUpdate.Append(String.Format(", {0}", Db.Quoted(Db.Quoted(m_CreatedOn.ToShortDateString() + " " + m_CreatedOn.ToString("T", New CultureInfo("en-us")))))
            'oStrUpdate.Append(String.Format(", {0}", Db.Quoted(Db.Quoted(m_CreatedOn.ToShortDateString() + " " + m_CreatedOn.ToShortTimeString))))
            oStrUpdate.Append(", CreatedOn = " + Db.Quoted(m_CreatedOn.ToShortDateString() + " " + m_CreatedOn.ToShortTimeString()))
            'oStrUpdate.Append(String.Format(", {0}", Db.Quoted(m_CreatedOn.ToString("MM-dd-yyyy hh:mm"))))
            '''''''''''''''''' this line of code is for server '''''''''''''''''''''''''''''''''''
            'oStrUpdate.Append(String.Format(", {0}", Db.Quoted(m_CreatedOn.ToShortDateString() + " " + m_CreatedOn.ToShortTimeString())))
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'oStrUpdate.Append(", [CreatedOn] = " + Db.Quoted(m_CreatedOn.ToShortDateString() + " " + m_CreatedOn.ToShortTimeString()))
				oStrUpdate.Append(", [CreatedBy] = " + m_CreatedBy.ToString())
				oStrUpdate.Append(" WHERE [ID] = " + ID_ID.ToString())

				Db.SqlAction(oStrUpdate.ToString())

				RetVal = ID_ID

        Catch ex As Exception
            Throw New Exception("Update failed. " & ex.Message)
			End Try

			Return RetVal
		End Function
		
		Public Sub Delete(ByVal ID_ID As Int32)
			Try
            GetJobByID(ID_ID)
				Db.SqlAction("DELETE FROM FF_Job WHERE ID = " + ID_ID.ToString())

			Catch
				Throw New Exception ("Deleted failed.")
			End Try
		End Sub

#End Region

	End Class

#Region	"Schema"
		
		'	Table: FF_Job
		'	
		'	  1: ID                                      System.Int32 (4)
		'	  2: JobGUID                                 System.String (50)
		'	  3: JobName                                 System.String (100)
		'	  4: CustomerKey                             System.Int32 (4)
		'	  5: CustomerContactKey                      System.Int32 (4)
		'	  6: IsCreated                               System.Boolean (1)
		'	  7: IsTemplate                              System.Boolean (1)
		'	  8: IsCancelled                             System.Boolean (1)
		'	  9: IsDeleted                               System.Boolean (1)
		'	 10: AccountHandlerKey                       System.Int32 (4)
		'	 11: CollateralDueOn                         System.DateTime (4)
		'	 12: DeadlineOn                              System.DateTime (4)
		'	 13: CompletedOn                             System.DateTime (4)
		'	 14: CompletedByUserID                       System.Int32 (4)
		'	 15: CustCtcName                             System.String (50)
		'	 16: CustPhone                               System.String (50)
		'	 17: CustEmail                               System.String (100)
		'	 18: CustRef1                                System.String (50)
		'	 19: CustRef2                                System.String (50)
		'	 20: ItemCount                               System.Int32 (4)
		'	 21: MaterialFromExternalSource              System.String (750)
		'	 22: EnvelopePackaging                       System.String (750)
'	 23: Instructions                            System.String (1000)
'	 24: Notes                            System.String (1000)
		'	 24: CreatedOn                               System.DateTime (4)
		'	 25: CreatedBy                               System.Int32 (4)

'IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[FF_Job]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
'DROP TABLE [dbo].[FF_Job]
'GO
'CREATE TABLE [dbo].[FF_Job](
'.
'.
') ON [PRIMARY]

    ' NOTES
    ' Rename Load to GetJobByID
    ' make ID a readonly property
    ' make JobGUID a readonly property
    ' in Init             m_JobGUID = Guid.NewGuid.ToString
    ' in Init             m_DeadlineOn = DateTime.Parse(BASE_DATE)
    ' in Init             m_CompletedOn = DateTime.Parse(BASE_DATE)

#End Region



