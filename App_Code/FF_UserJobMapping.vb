Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data


Partial Public Class FF_UserJobMapping

    Public Shared Function FetchUserJobMapping(ByVal nJobId As Int32) As DataTable
        Dim dt As DataTable
        dt = DNNDB.Query("SELECT FF_UserJobMapping.JobID, FF_UserJobMapping.JobStateID, FF_UserJobMapping.UserID, Users.Username, FF_JobState.Position, FF_JobState.JobStateName FROM FF_Job INNER JOIN FF_UserJobMapping ON FF_Job.ID = FF_UserJobMapping.JobID INNER JOIN FF_JobState ON FF_UserJobMapping.JobStateID = FF_JobState.id INNER JOIN Users ON FF_UserJobMapping.UserID = Users.UserID where FF_Job.ID = " & nJobId & "order by jobstateid" & " asc")
        FetchUserJobMapping = dt
    End Function

    Public Shared Function FetchUserInJobStage(ByVal nJobId As Int32, ByVal nJobStageId As Int32, Optional ByVal nRoleId() As Int32 = Nothing) As DataTable
        Dim dt As DataTable
        Dim sql As String = "SELECT distinct FF_UserJobMapping.JobStateID, FF_UserJobMapping.JobID, FF_JobState.JobStateName, FF_Job.JobName, FF_UserJobMapping.IsEmail, FF_UserJobMapping.IsEmailSent, Users.Username, Position, Users.UserID, FF_UserJobMapping.RoleID FROM FF_JobState INNER JOIN FF_Job INNER JOIN FF_UserJobMapping ON FF_Job.ID = FF_UserJobMapping.JobID INNER JOIN Users ON FF_UserJobMapping.UserID = Users.UserID ON FF_JobState.id = FF_UserJobMapping.JobStateID INNER JOIN UserRoles ON Users.UserID = UserRoles.UserID INNER JOIN Roles ON UserRoles.RoleID = Roles.RoleID INNER JOIN RoleGroups ON Roles.RoleGroupID = RoleGroups.RoleGroupID WHERE FF_Job.ID = " & nJobId & " AND FF_UserJobMapping.JobStateID = " & nJobStageId & " And RoleGroups.RoleGroupName In (select RoleGroups.RoleGroupName from dbo.RoleGroups where RoleGroups.RoleGroupName ='Production Schedule')"
        'dt = DNNDB.Query("SELECT FF_UserJobMapping.JobStateID , FF_UserJobMapping.JobID, FF_JobState.JobStateName, FF_Job.JobName, Users.Username, Users.UserID FROM FF_JobState INNER JOIN FF_Job INNER JOIN FF_UserJobMapping ON FF_Job.ID = FF_UserJobMapping.JobID INNER JOIN Users ON FF_UserJobMapping.UserID = Users.UserID ON FF_JobState.id = FF_UserJobMapping.JobStateID where FF_Job.ID =" & nJobId & " And " & "FF_UserJobMapping.JobStateID = " & nJobStageId)
        'dt = DNNDB.Query("SELECT FF_UserJobMapping.JobStateID, FF_UserJobMapping.JobID, FF_JobState.JobStateName, FF_Job.JobName, FF_UserJobMapping.IsEmail, FF_UserJobMapping.IsEmailSent, Users.Username,  Users.UserID, FF_UserJobMapping.RoleID FROM FF_JobState INNER JOIN FF_Job INNER JOIN FF_UserJobMapping ON FF_Job.ID = FF_UserJobMapping.JobID INNER JOIN Users ON FF_UserJobMapping.UserID = Users.UserID ON FF_JobState.id = FF_UserJobMapping.JobStateID INNER JOIN UserRoles ON Users.UserID = UserRoles.UserID INNER JOIN Roles ON UserRoles.RoleID = Roles.RoleID INNER JOIN RoleGroups ON Roles.RoleGroupID = RoleGroups.RoleGroupID WHERE (FF_Job.ID = " & nJobId & ") AND (FF_UserJobMapping.JobStateID = " & nJobStageId & ") And RoleGroups.RoleGroupName In (select RoleGroups.RoleGroupName from dbo.RoleGroups where RoleGroups.RoleGroupName ='Production Schedule')")
        dt = DNNDB.Query(sql)
        FetchUserInJobStage = dt
    End Function

    Public Shared Function FetchUsersInPosition(ByVal nJobId As Int32, ByVal nPosition As Int32) As DataTable
        Dim dt As DataTable
        Dim sSql As New StringBuilder
        sSql.Append(" SELECT distinct FF_UserJobMapping.JobStateID, FF_UserJobMapping.JobID, FF_JobState.JobStateName, FF_Job.JobName, FF_UserJobMapping.IsEmail, FF_UserJobMapping.IsEmailSent, Users.Username,  Users.UserID, FF_UserJobMapping.RoleID,FF_JobState.Position")
        sSql.Append(" FROM FF_JobState INNER JOIN FF_Job INNER JOIN FF_UserJobMapping ON FF_Job.ID = FF_UserJobMapping.JobID INNER JOIN Users ON FF_UserJobMapping.UserID = Users.UserID ON FF_JobState.id = FF_UserJobMapping.JobStateID INNER JOIN UserRoles ON Users.UserID = UserRoles.UserID INNER JOIN Roles ON UserRoles.RoleID = Roles.RoleID INNER JOIN RoleGroups ON Roles.RoleGroupID = RoleGroups.RoleGroupID WHERE FF_Job.ID = " & nJobId & " AND Position  = " & nPosition & " And RoleGroups.RoleGroupName ")
        sSql.Append(" In (select RoleGroups.RoleGroupName from dbo.RoleGroups where RoleGroups.RoleGroupName ='Production Schedule')")
        dt = DNNDB.Query(sSql.ToString())
        FetchUsersInPosition = dt
    End Function

    Public Shared Sub UpdateUserEmailPreference(ByVal njobId As Int32, ByVal nUserId As Int32, ByVal email As Int32, ByVal nStageID As Int32)
        Dim sql As String = "update FF_UserJobMapping set IsEmail=" & email & " where jobId=" & njobId & " and UserId=" & nUserId & " and JobStateId=" & nStageID
        DNNDB.Query(sql)
    End Sub

    Public Shared Sub UpdateEmailSent(ByVal njobId As Int32, ByVal nUserId As Int32, ByVal isEmailSent As Int32, ByVal nStageID As Int32)
        Dim sql As String = "update FF_UserJobMapping set IsEmailSent=" & isEmailSent & " where jobId=" & njobId & " and UserId=" & nUserId & " and JobStateId=" & nStageID
        DNNDB.Query(sql)
    End Sub

    Public Shared Sub CloneUserJobMapping(ByVal nSourceJobID As Integer, ByVal nDestinationJobID As Integer, Optional ByVal nCurrentUserId As Integer = Nothing)
        'Dim sqlParamnDestinationJobID(0) As SqlParameter
        'sqlParamnDestinationJobID(0) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)
        'sqlParamnDestinationJobID(0).Value = nDestinationJobID
        'DNNDB.ExecuteStoredProcedure("FF_DeleteJobStatesByJobID", sqlParamnDestinationJobID)

        Dim sqlParamnSourceID(0) As SqlParameter
        sqlParamnSourceID(0) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)
        sqlParamnSourceID(0).Value = nSourceJobID
        Dim odt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllUJMByJobId", sqlParamnSourceID)

        'Dim sqlParamnDestinationID(0) As SqlParameter
        'sqlParamnDestinationID(0) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)
        'sqlParamnDestinationID(0).Value = nDestinationJobID
        'Dim odtJobStages As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllStatesByJobID", sqlParamnDestinationID)

        'Dim odtJobStages As DataTable = FF_JobState.GetJobStages(nDestinationJobID)
        'Dim count As Integer = 0
        Dim odtJobStages As DataTable

        If odt IsNot Nothing And odt.Rows.Count <> 0 Then
            For Each dr As DataRow In odt.Rows
                Dim sbSQL As New StringBuilder
                Dim sql As String = "select * from FF_JobState where JobStateName='" & dr("JobStateName") & "' and jobid =" & nDestinationJobID
                odtJobStages = DNNDB.Query(sql)
                sbSQL.Append("Insert Into FF_UserJobMapping (JobId, JobStateId, UserId, RoleId, IsEmail, IsEmailSent, CreatedOn, CreatedBy) Values (")
                sbSQL.Append("'")
                sbSQL.Append(nDestinationJobID.ToString.Replace("'", "''"))
                sbSQL.Append("'")
                sbSQL.Append(",")
                sbSQL.Append("'")
                sbSQL.Append(odtJobStages.Rows(0)("Id").ToString.Replace("'", "''"))
                sbSQL.Append("'")
                sbSQL.Append(",")
                sbSQL.Append("'")
                sbSQL.Append(dr("UserId").ToString.Replace("'", "''"))
                sbSQL.Append("'")
                sbSQL.Append(",")
                sbSQL.Append("'")
                sbSQL.Append(dr("RoleId").ToString.Replace("'", "''"))
                sbSQL.Append("'")
                sbSQL.Append(",")
                sbSQL.Append("'")
                sbSQL.Append(dr("IsEmail").ToString.Replace("'", "''"))
                sbSQL.Append("'")
                sbSQL.Append(",")
                sbSQL.Append("'")
                sbSQL.Append(dr("IsEmailSent").ToString.Replace("'", "''"))
                sbSQL.Append("'")
                sbSQL.Append(",")
                sbSQL.Append("'")
                sbSQL.Append(DateTime.Now.ToString.Replace("'", "''"))
                sbSQL.Append("'")
                sbSQL.Append(",")
                sbSQL.Append("'")
                sbSQL.Append(nCurrentUserId.ToString.Replace("'", "''"))
                sbSQL.Append("'")
                sbSQL.Append(")")
                Call DNNDB.Query(sbSQL.ToString)
                'count = count + 1
            Next
        End If
    End Sub


End Class
