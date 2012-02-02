Imports System
Imports System.Web.UI
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Data
Imports System.Web.UI.WebControls
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports Telerik.Web.UI

Partial Class PS_UserJobMapping
    Inherits System.Web.UI.UserControl


    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo

    Private user_Id As Integer
    Dim gdtDataTable As DataTable

#Region "Properties"

    Property pnJobID() As Integer
        Get
            Dim o As Object = ViewState("JobID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("JobID") = Value
        End Set
    End Property

    Property pdtJobStages() As DataTable
        Get
            Dim o As Object = ViewState("JobStages")
            If o Is Nothing Then
                Return Nothing
            End If
            Return o
        End Get
        Set(ByVal Value As DataTable)
            ViewState("JobStages") = Value
        End Set
    End Property

    Public Property pdtTeam() As DataTable
        Get
            Dim o As Object = ViewState("pdtTeam")
            If o Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(o, DataTable)
        End Get
        Set(ByVal Value As DataTable)
            ViewState("pdtTeam") = Value
        End Set
    End Property

    Property pdtUserJobMapping() As DataTable
        Get
            Dim o As Object = ViewState("UserJobMapping")
            If o Is Nothing Then
                Return Nothing
            End If
            Return o
        End Get
        Set(ByVal Value As DataTable)
            ViewState("UserJobMapping") = Value
        End Set
    End Property

    Public ReadOnly Property BasePage As DotNetNuke.Framework.CDefault
        Get
            Return CType(Me.Page, DotNetNuke.Framework.CDefault)
        End Get
    End Property

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not IsPostBack Then
                pnJobID = GetJobIDFromQueryString()
                If pnJobID > 0 Then

                    GetAllStagesByJobID(pnJobID)
                    InitializeUsersList()
                    PopulateListAndTree()

                End If
                'pdtJobStages = FF_JobState.FetchJobStages(pnJobID)
            Else
                gdtDataTable = pdtJobStages
            End If

        Catch ex As Exception
            WebMsgBox.Show(ex.Message.ToString())
        End Try


    End Sub

    Protected Sub SetTitles()

        Me.BasePage.Title = FF_GLOBALS.PAGE_TITLE_TEXT + " - " + FF_GLOBALS.PAGE_ADD_RESOURCES

    End Sub
    Protected Sub GetAllStagesByJobID(ByVal njobId As Integer)
        Dim paramSql(0) As SqlParameter
        paramSql(0) = New SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)
        paramSql(0).Value = njobId
        pdtJobStages = DNNDB.ExecuteStoredProcedure("FF_GetAllStatesByJobID", paramSql)
    End Sub


    Protected Function GetJobIDFromQueryString() As String
        GetJobIDFromQueryString = String.Empty
        If Request.Params.Count > 0 Then
            Try
                GetJobIDFromQueryString = Request.Params("job")
            Catch
            End Try
        End If
    End Function

    Protected Sub InitializeUsersList()
        cmbRoleName.Items.Clear()
        Call PopulateUserRoleDropdown()
        Call LoadTreeViewControl()

    End Sub

    Protected Sub lstUsers_Dropped(ByVal sender As Object, ByVal e As RadListBoxDroppedEventArgs)
        If tvUsersInJobStages.ClientID = e.HtmlElementID Then
            For Each item As RadListBoxItem In e.SourceDragItems
                'tvUsersInJobStages.Nodesext += item.Text + ", \n";
            Next

        End If

    End Sub

    Protected Sub LoadTreeViewControl()

        tvUsersInJobStages.Nodes.Clear()
        AddVirtualStage()
        tvUsersInJobStages.DataSource = pdtJobStages
        tvUsersInJobStages.DataTextField = "JobStateName"
        tvUsersInJobStages.DataValueField = "id"
        tvUsersInJobStages.DataBind()

    End Sub
    Protected Sub AddVirtualStage()
        Dim dr As DataRow = pdtJobStages.NewRow
        dr("id") = -1
        dr("jobStateName") = "All Stages"
        dr("jobid") = -1
        dr("position") = -1
        dr("IsCompleted") = False
        pdtJobStages.Rows.Add(dr)
    End Sub


    'Protected Sub rapUserJobMapping_AjaxRequest(ByVal sender As Object, ByVal e As Telerik.Web.UI.AjaxRequestEventArgs)
    'If e.Argument IsNot Nothing Then

    '    Dim str As String = e.Argument.ToString
    '    Dim sRole As String = String.Empty
    '    Dim userRoleId As Integer
    '    Dim parts As String() = str.Split(New Char() {"-"c})
    '    sRole = parts(0)
    '    If Integer.TryParse(parts(1).Replace("Remove", ""), userRoleId) Then
    '    End If
    '    If e.Argument.Contains("Remove") Then
    '        RemoveUsersFromList(sRole)
    '        RemoveRoleFromTree(sRole)
    '    Else
    '        PopulateUsersList(sRole, userRoleId)
    '    End If

    'End If
    'End Sub
    Protected Function FetchTeams() As DataTable

        pdtTeam = DNNDB.ExecuteStoredProcedure("FF_GetAllTeams")
        Return pdtTeam

    End Function

    Protected Sub PopulateUserRoleDropdown()

        Dim dt As DataTable = FetchTeams()

        For Each dr As DataRow In dt.Rows
            Dim rcbItem As New RadComboBoxItem(dr("TeamName"), dr("ID"))
            cmbRoleName.Items.Add(rcbItem)
            Dim chkRole As CheckBox = rcbItem.FindControl("chkRole")
            chkRole.Text = dr("TeamName")
        Next

        Dim rcbSelectAll As New RadComboBoxItem(FF_GLOBALS.SELECT_ALL, -1)
        cmbRoleName.Items.Insert(0, rcbSelectAll)
        Dim chkSelectAll As CheckBox = rcbSelectAll.FindControl("chkRole")
        chkSelectAll.Text = "- Select All -"
        'chkSelectAll.v.Text = "- Select All -"


    End Sub


    Protected Sub PopulateUsersList(ByVal userRole As String, ByVal userRoleId As Long)

        If cmbRoleName.SelectedIndex <> 0 And userRole <> String.Empty Then
            Dim sRoleName As String = userRole
            Dim lstUser, lstRole As RadListBoxItem

            If sRoleName <> FF_GLOBALS.SELECT_ALL Then
                Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, sRoleName)

                For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                    Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, user.UserID)
                    Dim roleName As String = sRoleName
                    Dim userName As String = userInfo.Username
                    lstUser = lstUsers.FindItemByText(userName)
                    lstRole = lstUsers.FindItemByText(roleName)
                    If lstRole Is Nothing Then
                        If findUserExistInAllStages(roleName) = False Then
                            lstUsers.Items.Add(New RadListBoxItem(roleName, userRoleId))
                        End If
                    End If

                    If lstUser Is Nothing Then
                        If findUserExistInAllStages(userName) = False Then
                            lstUsers.Items.Add(New RadListBoxItem(userName, user.UserID))
                        End If
                    End If
                Next
                lstUsers.SortItems()
                SortUsersInTree(tvUsersInJobStages.Nodes)
            ElseIf sRoleName = FF_GLOBALS.SELECT_ALL Then
                ''''''''''''''''' this method will be used if I would be successful in placing my checkallusers checkbox in the radcombobox
                Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllUsersFromPSRole")
                If dt IsNot Nothing And dt.Rows.Count <> 0 Then
                    For Each dr As DataRow In dt.Rows
                        Dim sUserName As String = dr("username")
                        Dim nUserID As Integer = dr("UserID")
                        lstUser = lstUsers.FindItemByText(sUserName)
                        If lstUser Is Nothing Then
                            lstUsers.Items.Add(New RadListBoxItem(sUserName, nUserID))
                        End If
                    Next
                End If

            End If

        End If

        'FindUserExistInEachTreeNode()


    End Sub

   

    Protected Sub selectAllCheckboxes()

        For Each item As RadComboBoxItem In cmbRoleName.Items

            Dim cb As CheckBox = item.FindControl("chkRole")
            If cb IsNot Nothing Then
                cb.Checked = True
            End If

        Next

    End Sub

    Protected Sub UnselectAllCheckboxes()

        For Each item As RadComboBoxItem In cmbRoleName.Items

            Dim cb As CheckBox = item.FindControl("chkRole")
            If cb IsNot Nothing Then
                cb.Checked = False
            End If

        Next

    End Sub

    Protected Sub RemoveAllTeams()

        Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllTeams")
        For Each dr As DataRow In dt.Rows
            Dim sTeamName As String = dr("TeamName")
            Dim nTeamID As Integer = dr("ID")
            Dim lstRole As RadListBoxItem
            lstRole = lstUsers.FindItemByText(sTeamName)
            If lstRole IsNot Nothing Then
                lstUsers.Items.Remove(lstRole)
                Dim sql As String = "select * from FF_UserTeamMapping where TeamID = " & nTeamID
                Dim dtUsers As DataTable = DNNDB.Query(sql)
                If dtUsers IsNot Nothing And dt.Rows.Count <> 0 Then
                    For Each row As DataRow In dtUsers.Rows
                        Dim nUserId As Integer = Convert.ToInt64(row("UserId"))
                        lstRole = lstUsers.FindItemByValue(nUserId)
                        lstUsers.Items.Remove(lstRole)
                    Next
                End If

                'Call RemoveUsersByTeamName(sTeamName)
            End If
        Next


    End Sub

    Protected Sub GetAllTeamsAndAllUsers()

        Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllTeams")
        For Each dr As DataRow In dt.Rows
            Dim sTeamName As String = dr("TeamName")
            Dim nTeamID As Integer = dr("ID")
            'Dim alUsers As ArrayList = rc.GetUsersByRoleName(DNN.GetPMB(Me).PortalId, sTeamName)
            'If alUsers IsNot Nothing And alUsers.Count <> 0 Then

            Dim lstRole As RadListBoxItem
            lstRole = lstUsers.FindItemByText(sTeamName)
            If lstRole Is Nothing Then
                lstUsers.Items.Add(New RadListBoxItem(sTeamName, nTeamID))
                Call GetUsersByTeamId(nTeamID)
            End If

            'End If
        Next

    End Sub

    Protected Sub GetAllUsers()

        'Dim lstUser As RadListBoxItem
        ''Dim lstRole As RadListBoxItem
        'Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllUsersFromPSRole")
        'If dt IsNot Nothing And dt.Rows.Count <> 0 Then
        '    For Each dr As DataRow In dt.Rows
        '        Dim sUserName As String = dr("username")
        '        Dim nUserID As Integer = dr("UserID")
        '        'Dim sRoleName As String = dr("rolename")
        '        'Dim nRoleID As Integer = dr("roleId")
        '        'lstRole = lstUsers.FindItemByText(sRoleName)
        '        'If lstRole Is Nothing Then
        '        '    lstUsers.Items.Add(New RadListBoxItem(sRoleName, nRoleID))
        '        'End If
        '        lstUser = lstUsers.FindItemByText(sUserName)
        '        If lstUser Is Nothing Then
        '            lstUsers.Items.Add(New RadListBoxItem(sUserName, nUserID))

        '        End If
        '    Next
        'End If

    End Sub

    Protected Sub RemoveAllusers()

        Dim lstUser As RadListBoxItem
        'Dim lstRole As RadListBoxItem
        Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllUsersFromPSRole")
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            For Each dr As DataRow In dt.Rows
                Dim sUserName As String = dr("username")
                Dim nUserID As Integer = dr("UserID")
                'Dim sRoleName As String = dr("rolename")
                'Dim nRoleID As Integer = dr("roleId")
                'lstRole = lstUsers.FindItemByText(sRoleName)
                'If lstRole IsNot Nothing Then
                '    lstUsers.Items.Remove(lstRole)
                'End If
                lstUser = lstUsers.FindItemByText(sUserName)
                If lstUser IsNot Nothing Then
                    lstUsers.Items.Remove(lstUser)
                End If
            Next
        End If



    End Sub

    Protected Sub chkRole_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim cb As CheckBox = sender

        If cb.Text = FF_GLOBALS.SELECT_ALL Then

            If cb.Checked Then

                'GetAllUsers()
                GetAllTeamsAndAllUsers()
                selectAllCheckboxes()

            ElseIf cb.Checked = False Then

                'RemoveAllusers()
                RemoveAllTeams()
                UnselectAllCheckboxes()
            End If

        Else

            If cb.Checked Then

                GetUsersByTeamName(cb.Text)

            ElseIf cb.Checked = False Then

                Dim rcbi As RadComboBoxItem = cmbRoleName.Items.FindItemByText(FF_GLOBALS.SELECT_ALL)
                Dim chkSelectAll As CheckBox

                chkSelectAll = rcbi.FindControl("chkRole")

                If chkSelectAll IsNot Nothing Then
                    chkSelectAll.Checked = False
                End If

                RemoveUsersByTeamName(cb.Text)

            End If


        End If


        lstUsers.Items.Sort()

    End Sub

    Protected Sub GetUsersByTeamName(ByVal sTeamName As String)

        Dim sql As String = "select * from ff_team where teamname = '" & sTeamName & "'"



        Dim dtTeam As DataTable = DNNDB.Query(sql)

        If dtTeam IsNot Nothing And dtTeam.Rows.Count <> 0 Then

            Dim nTeamId As Integer = Convert.ToInt64(dtTeam.Rows(0)("Id"))

            Dim lstTeam As New RadListBoxItem(sTeamName, nTeamId)

            lstUsers.Items.Add(lstTeam) ' Add Team

            Call GetUsersByTeamId(nTeamId) ' Add Users

        End If

        'GetUsersByTeamName = dtTeam

    End Sub

    Protected Sub GetUsersByTeamId(ByVal nTeamId As Integer)

        Dim lstUser As RadListBoxItem

        Dim sql As String = "select * from ff_UserTeamMapping where TeamId = " & nTeamId

        Dim dt As DataTable = DNNDB.Query(sql)

        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            For Each row As DataRow In dt.Rows

                Dim userId As Integer = Convert.ToInt64(row("UserId"))
                Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, userId)

                lstUser = lstUsers.FindItemByText(userInfo.Username)
                If lstUser Is Nothing Then
                    lstUsers.Items.Add(New RadListBoxItem(userInfo.Username, userInfo.UserID))
                End If

            Next

        End If

    End Sub

    Protected Sub GetUsersByRoleName(ByVal roleName As String)

        Dim lstUser As RadListBoxItem
        Dim sRoleName As String = roleName


        Dim alUsers As ArrayList = rc.GetUsersByRoleName(DNN.GetPMB(Me).PortalId, sRoleName)

        If alUsers IsNot Nothing And alUsers.Count <> 0 Then

            For Each userInfo As UserInfo In alUsers

                lstUser = lstUsers.FindItemByText(userInfo.Username)
                If lstUser Is Nothing Then
                    lstUsers.Items.Add(New RadListBoxItem(userInfo.Username, userInfo.UserID))
                End If

            Next

        End If


    End Sub

    Protected Sub RemoveUsersByTeamName(ByVal sTeamName As String)

        Dim sql As String = "select * from FF_UserTeamMapping where TeamId = (select Id from ff_team where TeamName = '" & sTeamName & "')"
        Dim dt As DataTable = DNNDB.Query(sql)

        Dim rlbUser As RadListBoxItem
        Dim rlbTeam As RadListBoxItem


        rlbTeam = lstUsers.FindItemByText(sTeamName)

        If rlbTeam IsNot Nothing Then

            lstUsers.Items.Remove(rlbTeam) ' remove team

        End If

        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            For Each row As DataRow In dt.Rows
                Dim nUserId As Integer = Convert.ToInt64(row("UserId"))
                Dim nTeamId As Integer = Convert.ToInt64(row("TeamId"))
                Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)
                rlbUser = lstUsers.FindItemByText(userInfo.Username)
                If rlbUser IsNot Nothing Then
                    lstUsers.Items.Remove(rlbUser) ' remove users
                End If
            Next

        End If


    End Sub

    'For Each userInfo As UserInfo In alUsers

    '    lstUser = lstUsers.FindItemByText(userInfo.Username)
    '    If lstUser IsNot Nothing Then
    '        lstUsers.Items.Remove(lstUser)
    '    End If

    'Next

    'If alUsers IsNot Nothing And alUsers.Count <> 0 Then

    '    For Each userInfo As UserInfo In alUsers

    '        lstUser = lstUsers.FindItemByText(userInfo.Username)
    '        If lstUser IsNot Nothing Then
    '            lstUsers.Items.Remove(lstUser)
    '        End If

    '    Next

    'End If




  

    Protected Sub chkAllPSUsers_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim cb As CheckBox = sender
        Dim lstUser As RadListBoxItem
        If cb.Checked Then
            Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllUsersFromPSRole")
            If dt IsNot Nothing And dt.Rows.Count <> 0 Then
                For Each dr As DataRow In dt.Rows
                    Dim sUserName As String = dr("username")
                    Dim nUserID As Integer = dr("UserID")
                    lstUser = lstUsers.FindItemByText(sUserName)
                    If lstUser Is Nothing Then
                        lstUsers.Items.Add(New RadListBoxItem(sUserName, nUserID))
                    End If
                Next
            End If
        ElseIf cb.Checked = False Then

            Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllUsersFromPSRole")
            Dim item As New RadListBoxItem
            Dim userFound = False
            For Each dr As DataRow In dt.Rows
                For Each item In lstUsers.Items
                    If IsUserRole(item.Text) Then
                        Dim sRole As String = item.Text
                        Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, sRole)
                        For Each suser As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                            If suser.UserID = dr("userid") Then
                                userFound = True
                            End If
                        Next
                    End If
                Next
                If userFound = False Then
                    lstUsers.Items.Remove(item)
                End If
            Next
        End If

    End Sub

    Protected Sub RemoveUsersFromList(ByVal userRole As String)
        If userRole.Trim() <> String.Empty Then
            Dim sRoleName As String = userRole
            Dim lstUser As RadListBoxItem
            Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, sRoleName)
            For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, user.UserID)
                Dim userName As String = userInfo.Username
                lstUser = lstUsers.FindItemByText(userName)
                If lstUser IsNot Nothing Then
                    lstUsers.Items.Remove(lstUser)
                End If
                RemoveUsersFromTree(userName)
            Next
            lstUser = lstUsers.FindItemByText(sRoleName)
            If lstUser IsNot Nothing Then
                lstUsers.Items.Remove(lstUser)
            End If
            lstUsers.SortItems()
            SortUsersInTree(tvUsersInJobStages.Nodes)
        End If
    End Sub

    Protected Sub RemoveUsersFromTree(ByVal user As String)

        ''''''''''' work in this area ''''''''''''''
        For Each parentNode As RadTreeNode In tvUsersInJobStages.Nodes
            Dim found As RadTreeNode = findNode(parentNode, user)
            If found IsNot Nothing Then
                parentNode.Nodes.Remove(found)
            End If
        Next
        lstUsers.SortItems()
        SortUsersInTree(tvUsersInJobStages.Nodes)
    End Sub

    Protected Sub RemoveRoleFromTree(ByVal role As String)

        ''''''''''' work in this area ''''''''''''''
        For Each parentNode As RadTreeNode In tvUsersInJobStages.Nodes
            Dim found As RadTreeNode = findNode(parentNode, role)
            If found IsNot Nothing Then
                parentNode.Nodes.Remove(found)
            End If
        Next
        lstUsers.SortItems()
        SortUsersInTree(tvUsersInJobStages.Nodes)
    End Sub

    Protected Sub AddNodeInList(ByVal str As String, ByVal Id As Integer)
        If IsUserRole(str) Then
            Dim roleName As String
            Dim roleId As Int32
            roleName = str
            roleId = Id

            If lstUsers.FindItemByText(roleName) Is Nothing Then
                lstUsers.Items.Add(New RadListBoxItem(roleName, roleId))
            End If

            Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, roleName)
            For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
                If lstUsers.FindItemByText(userInfo.Username) Is Nothing Then
                    lstUsers.Items.Add(New RadListBoxItem(userInfo.Username, userInfo.UserID))
                End If
            Next

        ElseIf lstUsers.FindItemByText(str) Is Nothing Then
            lstUsers.Items.Add(New RadListBoxItem(str, Id))
        End If
    End Sub

    Protected Function IsUserRole(ByVal str As String) As Boolean
        Dim item As RadComboBoxItem = cmbRoleName.FindItemByText(str)
        Dim isRole As Boolean = False
        If item IsNot Nothing Then
            isRole = True
        End If
        IsUserRole = isRole

    End Function

    Protected Sub RemoveUsersFromListForRole(ByVal role As String)

        '''''''''' for all stages ''''''''''''''''''
        Dim uc As New DotNetNuke.Entities.Users.UserController
        Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, role)
        If usersList.Count <> 0 Then
            For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
                Dim findUser As RadListBoxItem = lstUsers.FindItemByText(userInfo.Username)
                lstUsers.Items.Remove(findUser)
            Next
        End If

        lstUsers.SortItems()

    End Sub

    Protected Function FindUsersInRoleInTree(ByVal node As RadTreeNode) As Boolean
        FindUsersInRoleInTree = False
        Dim ic As ICollection = tvUsersInJobStages.GetAllNodes()
        For Each parentNode As RadTreeNode In ic
            For Each childNode As RadTreeNode In parentNode.GetAllNodes
                If IsUserRole(childNode.Text) = False Then
                    Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, node.Text)
                    If usersList.Count <> 0 Then
                        For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                            Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
                            If childNode.Value = userInfo.UserID Then
                                parentNode.Nodes.Remove(childNode)
                                FindUsersInRoleInTree = True
                                Return FindUsersInRoleInTree
                            End If
                        Next
                    End If
                End If
            Next
        Next

        Return FindUsersInRoleInTree

    End Function

    Protected Function FindUserInTeam(ByVal nTeamId As Integer, ByVal nUserId As Integer) As Boolean
        Dim found As Boolean = False
        Dim sql As String = "select * from FF_UserTeamMapping where TeamId = " & nTeamId & "and UserId = " & nUserId
        Dim dt As DataTable = DNNDB.Query(sql)
        'Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, role)
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then
            'For Each row As DataRow In dt.Rows
            '    dim
            '    Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
            'If username = userInfo.Username Then
            found = True
            Return found
            'End If
            'Next
        End If
        Return found
    End Function

    Protected Function FindNodeInStage(ByVal parentNode As RadTreeNode, ByVal user As String) As Boolean

        Dim findUser As RadTreeNode
        Dim itemFound As Boolean = False
        findUser = parentNode.Nodes.FindNodeByText(user)
        If findUser IsNot Nothing Then
            itemFound = True
            Return itemFound
        End If
        Return itemFound

    End Function

    Protected Function findNode(ByVal rootNode As RadTreeNode, ByVal val As String) As RadTreeNode

        For Each childNode As RadTreeNode In rootNode.Nodes
            If childNode.Text = val Then
                findNode = childNode
                Return findNode
            End If
        Next

        Return Nothing

    End Function

    Private Function findUserExistInAllStages(ByVal val As String) As Boolean

        Dim findNode As New RadTreeNode
        For Each childNode As RadTreeNode In tvUsersInJobStages.Nodes
            If childNode.Text <> "All Stages" Then
                findNode = childNode.Nodes.FindNodeByText(val)
                If findNode Is Nothing Then
                    findUserExistInAllStages = False
                    Return findUserExistInAllStages
                End If
            End If
        Next

        findUserExistInAllStages = True

        Return findUserExistInAllStages

    End Function
    ''' <summary>
    ''' ''''''''''''''''''''''' easy first find node in stage then find role in stage and in that role find user ''''''''''''''''''
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub FindUserExistInEachTreeNode()

        Dim removeUsersList As New ArrayList
        Dim findInAllStages As Boolean = False
        Dim userNotExistInOneStage As Boolean = False
        For Each user As RadListBoxItem In lstUsers.Items
            For Each parentNode As RadTreeNode In tvUsersInJobStages.Nodes
                If parentNode.Text <> "All Stages" Then
                    If parentNode.Nodes.Count = 0 Then
                        findInAllStages = False
                        Exit For
                    Else
                        For Each childNode As RadTreeNode In parentNode.GetAllNodes
                            If FindNodeInStage(parentNode, user.Text) Then
                                findInAllStages = True
                                Exit For
                            End If
                            If IsUserRole(childNode.Text) Then
                                findInAllStages = False
                                If FindUserInTeam(childNode.Text, user.Text) Then
                                    findInAllStages = True
                                    Exit For
                                End If
                            End If
                        Next
                        If findInAllStages = False Then
                            Exit For
                        End If
                    End If
                End If
            Next
            If findInAllStages = True Then
                removeUsersList.Add(user)
            End If

        Next

        'For Each item As RadListBoxItem In removeUsersList
        '    lstUsers.Items.Remove(item)
        'Next


    End Sub

    Protected Sub lstUsers_ItemDropped(ByVal sender As Object, ByVal e As RadListBoxDroppedEventArgs)
        Dim found As RadTreeNode

        If e.HtmlElementID = tvUsersInJobStages.ClientID Then
            If tvUsersInJobStages.SelectedNode IsNot Nothing Then
                For Each item As RadListBoxItem In e.SourceDragItems
                    Dim node As New RadTreeNode(item.Text, item.Value)

                    If tvUsersInJobStages.SelectedNode.Text = "All Stages" Then
                        'item.Remove()
                        For Each parentNode As RadTreeNode In tvUsersInJobStages.Nodes
                            Dim childNode As New RadTreeNode(item.Text, item.Value)

                            If parentNode.Text <> "All Stages" Then
                                found = findNode(parentNode, childNode.Text)
                                If found Is Nothing Then
                                    parentNode.Nodes.Add(childNode)
                                    'RadToolTipManager.TargetControls.Add(node.Attributes(node.Value), node.Value, True)
                                    If IsUserRole(childNode.Text) Then
                                        ''''''''''''' commented for the time being ''''''''''''''''''''' 10th feb,2011
                                        'RemoveUsersFromListForRole(childNode.Text)
                                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                        ''''''''''' find and remove users from treee '''''''''
                                        FindUsersInRoleInTree(childNode)
                                    End If
                                End If
                            End If
                        Next
                    ElseIf tvUsersInJobStages.SelectedNode.Level = 0 Then

                        found = findNode(tvUsersInJobStages.SelectedNode, node.Text)
                        If found Is Nothing Then

                            If IsUserRole(node.Text) Then

                                Dim sql As String = "select * from ff_UserteamMapping where teamId = '" & node.Value & "'"
                                Dim dtTeam As DataTable = DNNDB.Query(sql)

                                'Dim usersList As ArrayList = GetUsersByTeamName(node.Text)
                                If dtTeam.Rows.Count <> 0 Then

                                    For Each row As DataRow In dtTeam.Rows
                                        Dim nUserId As Integer = row("UserId")
                                        userInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, nUserId)
                                        Dim foundNode As RadTreeNode = findNode(tvUsersInJobStages.SelectedNode, userInfo.Username)

                                        If foundNode IsNot Nothing Then
                                            RemoveUsersFromTree(userInfo.Username)
                                        Else
                                            'Dim tempNode As New RadTreeNode(userInfo.Username, userInfo.UserID)
                                            'tvUsersInJobStages.SelectedNode.Nodes.Add(tempNode)
                                            'If findUserExistInAllStages(tempNode.Text) Then
                                            '    Dim removeUser As RadListBoxItem = lstUsers.FindItemByText(tempNode.Text)
                                            '    If removeUser IsNot Nothing Then
                                            '        'lstUsers.Items.Remove(removeUser)
                                            '    End If
                                            'End If
                                            'tvUsersInJobStages.SelectedNode.Nodes.Remove(tempNode)
                                        End If
                                    Next

                                    tvUsersInJobStages.SelectedNode.Nodes.Add(node)
                                    'If findUserExistInAllStages(item.Text) Then
                                    '    item.Remove()
                                    'End If


                                End If

                            ElseIf Not (FindUserInStage(tvUsersInJobStages.SelectedNode, node)) Then
                                tvUsersInJobStages.SelectedNode.Nodes.Add(node)
                                'If findUserExistInAllStages(item.Text) Then
                                '    item.Remove()
                                'End If

                            End If
                        End If
                    Else
                        tvUsersInJobStages.SelectedNode.InsertAfter(node)
                    End If
                Next

            End If

        End If

        'FindUserExistInEachTreeNode()
        'lstUsers.SortItems()
        SortUsersInTree(tvUsersInJobStages.Nodes)
    End Sub

    Private Function FindUserInStage(ByVal parentNode As RadTreeNode, ByVal userNode As RadTreeNode) As Boolean
        Dim bfound As Boolean = False
        For Each childNode As RadTreeNode In parentNode.Nodes
            If IsUserRole(childNode.Text) Then
                If FindUserInTeam(childNode.Value, userNode.Value) Then
                    bfound = True
                    Exit For
                End If
            End If
        Next
        Return bfound
    End Function

    Protected Sub tvUsersInJobStages_NodeDrop(ByVal sender As Object, ByVal e As RadTreeNodeDragDropEventArgs)

        Dim found As RadTreeNode
        Dim item As New RadListBoxItem()
        item.Text = e.SourceDragNode.Text
        item.Value = e.SourceDragNode.Value

        If e.HtmlElementID = lstUsers.ClientID Then

            e.SourceDragNode.Remove()
            Dim existingItem As New RadListBoxItem
            existingItem = lstUsers.FindItemByText(item.Text)

            If existingItem Is Nothing Then

                If lstUsers.SelectedIndex > -1 Then
                    lstUsers.Items.Insert(lstUsers.SelectedIndex + 1, item)
                Else
                    lstUsers.Items.Add(item)
                    If IsUserRole(item.Text) Then
                        Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, item.Text)
                        If usersList.Count <> 0 Then
                            Dim uc As New DotNetNuke.Entities.Users.UserController
                            For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                                Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
                                AddNodeInList(userInfo.Username, userInfo.UserID)
                            Next
                        End If


                    End If

                End If

            End If

        Else
            If e.DestDragNode.Level = 0 Then
                If e.DestDragNode.Text <> "All Stages" Then
                    found = findNode(e.DestDragNode, e.SourceDragNode.Text)
                    If found Is Nothing Then
                        e.DestDragNode.Nodes.Add(e.SourceDragNode)
                    End If
                End If

            Else
                e.DestDragNode.InsertAfter(e.SourceDragNode)
            End If
        End If

        lstUsers.SortItems()
        SortUsersInTree(tvUsersInJobStages.Nodes)
    End Sub

    Protected Sub tvUsersInJobStages_ContextMenuItemClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeViewContextMenuEventArgs)
        Select Case e.MenuItem.Value
            Case "Remove"
                If e.Node.Level = 1 Then
                    e.Node.Remove()
                    AddNodeInList(e.Node.Text, e.Node.Value)
                    SortUsersInTree(tvUsersInJobStages.Nodes)
                    lstUsers.SortItems()
                End If
                Exit Select
            Case "Remove All"
                If e.Node.Level = 1 Then
                    RemoveUsersFromTree(e.Node.Text)
                    AddNodeInList(e.Node.Text, e.Node.Value)
                    SortUsersInTree(tvUsersInJobStages.Nodes)
                End If
                Exit Select
        End Select
    End Sub

    Protected Sub SortUsersInTree(ByVal nodes As RadTreeNodeCollection)

        Dim i, j As Integer
        Dim swapNode As New RadTreeNode
        Dim IC As ICollection = tvUsersInJobStages.GetAllNodes()
        Dim swapNodeName As String
        Dim swapNodeId As String

        For Each node As RadTreeNode In IC

            For i = node.Nodes.Count - 1 To 0 Step -1
                For j = 1 To i
                    If node.Nodes(j - 1).Text.CompareTo(node.Nodes(j).Text) > 0 Then
                        swapNodeName = node.Nodes(j - 1).Text
                        swapNodeId = node.Nodes(j - 1).Value

                        node.Nodes(j - 1).Text = node.Nodes(j).Text
                        node.Nodes(j - 1).Value = node.Nodes(j).Value

                        node.Nodes(j).Text = swapNodeName
                        node.Nodes(j).Value = swapNodeId

                    End If
                Next
            Next


        Next


    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim sQueryParams(0) As String
        sQueryParams(0) = "job=" & pnJobID
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim ffujm As New FF_UserJobMapping()

        Call DNNDB.Query("DELETE FROM FF_UserJobMapping WHERE JobID = " & pnJobID)


        Dim ic As ICollection = tvUsersInJobStages.GetAllNodes()
        For Each parentNode As RadTreeNode In ic
            For Each childNode As RadTreeNode In parentNode.GetAllNodes
                Dim sql As String = "select * from ff_team where teamName = '" & childNode.Text & "'"
                Dim dt As DataTable = DNNDB.Query(sql)
                If dt IsNot Nothing And dt.Rows.Count <> 0 Then
                    sql = "select * from ff_UserTeamMapping where teamId = " & childNode.Value
                    dt = DNNDB.Query(sql)
                    'Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, childNode.Text)
                    If dt IsNot Nothing And dt.Rows.Count <> 0 Then
                        For Each row As DataRow In dt.Rows
                            Dim nUserId As Integer = row("UserId")
                            Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, nUserId)
                            Dim sUserName As String = userInfo.Username
                            'Dim nUserId As Integer = userInfo.UserID
                            ffujm.JobID = pnJobID
                            ffujm.JobStateID = parentNode.Value
                            ffujm.UserID = nUserId
                            ffujm.RoleID = childNode.Value
                            ffujm.IsEmail = 1
                            ffujm.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                            Dim nSourceId As Integer = ffujm.Add()
                            '''''''''''' event log '''''''''''''''
                            Dim sChangeDetail As String = userInfo.Username & " is assigned to JobId = " & ffujm.JobID & ", JobStateId = " & ffujm.JobStateID
                            Dim ffAuditTrial As New FF_AuditTrail(ffujm, sChangeDetail, nSourceId)
                        Next
                    End If
                Else
                    Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, childNode.Value)
                    ffujm.JobID = pnJobID
                    ffujm.JobStateID = parentNode.Value
                    ffujm.UserID = userInfo.UserID
                    ffujm.IsEmail = 1
                    ffujm.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                    Dim nSourceId As Integer = ffujm.Add()
                    Dim sChangeDetail As String = userInfo.Username & " is assigned to JobId = " & ffujm.JobID & ", JobStateId = " & ffujm.JobStateID
                    Dim ffAuditTrial As New FF_AuditTrail(ffujm, sChangeDetail, nSourceId)

                End If
            Next
        Next


        Dim sQueryParams(0) As String
        sQueryParams(0) = "job=" & pnJobID
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    End Sub

    Protected Sub PopulateListAndTree()

        ''''''''''''''''' loading data using user string '''''''''''''''''''
        ''''''''''''''''' i can't store just roleid because if i just store roleid then in future if new user added in the role then it will also appear
        ''''''''''''''''' in the stage 


        ''''''''''''''''''''''''' UsersInJob TreeView ''''''''''''''''''''''''''''''''''''

        'Dim dtJobStages As DataTable = FF_JobState.FetchJobStages(pnJobID)

        Dim dtJobStages As DataTable = pdtJobStages

        If dtJobStages IsNot Nothing Then

            For Each dr As DataRow In dtJobStages.Rows '''''''''' stages

                Dim dtUsersInStages As DataTable = FF_UserJobMapping.FetchUserInJobStage(pnJobID, dr("id")) '''' users in stages

                If dtUsersInStages IsNot Nothing Then

                    For Each drUser In dtUsersInStages.Rows

                        Dim parentNode As RadTreeNode = tvUsersInJobStages.Nodes.FindNodeByValue(dr("id"))

                        '''''''''''''''''' adding role in a radtreenode '''''''''''''''''''''
                        Dim nTeamId As Integer
                        If drUser("RoleId") IsNot DBNull.Value Then
                            nTeamId = drUser("RoleID")
                        Else
                            nTeamId = 0
                        End If
                        If nTeamId <> 0 Then
                            'If drUser("RoleID") IsNot DBNull.Value And drUser("RoleID") <> 0 Then
                            'Dim roleInfo As RoleInfo = rc.GetRole(drUser("RoleID"), DNN.GetPMB(Me).PortalId)
                            Dim sql As String = "select * from ff_team where Id = " & nTeamId
                            Dim dt As DataTable = DNNDB.Query(sql)
                            If dt IsNot Nothing And dt.Rows.Count <> 0 Then
                                'Dim nTeamId As Integer = dt.Rows(0)("TeamId")
                                Dim sTeamName As String = dt.Rows(0)("TeamName")
                                Dim roleNode As New RadTreeNode(sTeamName, nTeamId)
                                If FindNodeInStage(parentNode, sTeamName) = False Then
                                    parentNode.Nodes.Add(roleNode)
                                    AddNodeInList(sTeamName, nTeamId)
                                End If
                                Dim findItem As RadComboBoxItem = cmbRoleName.FindItemByText(sTeamName)
                                If findItem IsNot Nothing Then
                                    Dim chkRole As CheckBox = CType(findItem.FindControl("chkRole"), CheckBox)
                                    chkRole.Checked = True
                                End If
                            End If
                        Else

                            Dim userNode As New RadTreeNode(drUser("username"), drUser("userid"))
                            parentNode.Nodes.Add(userNode)

                            Dim sql As String = "select * from ff_UserTeamMapping where UserId = " & drUser("userId")
                            Dim dt As DataTable = DNNDB.Query(Sql)
                            'Dim roleList As ArrayList = rc.GetUserRoles(DNN.GetPMB(Me).PortalId, drUser("userid"))
                            For Each row As DataRow In dt.Rows
                                Dim teamId As Integer = row("TeamId")
                                Dim findItem As RadComboBoxItem = cmbRoleName.FindItemByValue(teamId)
                                If findItem IsNot Nothing Then
                                    Dim chkRole As CheckBox = CType(findItem.FindControl("chkRole"), CheckBox)
                                    chkRole.Checked = True
                                    sql = "select Id, UserId, TeamId, (Select TeamName from ff_team where Id = TeamId) 'TeamName' from ff_UserTeamMapping where TeamId = " & teamId
                                    Dim dtUsers As DataTable = DNNDB.Query(sql)

                                    Dim sTeamName As String = dtUsers.Rows(0)("TeamName")
                                    'Dim nUserId As Integer = 
                                    'Dim usersList As ArrayList = uc.GetUser(DNN.GetPMB(Me).PortalId, Uri.RoleName)
                                    For Each rowUser As DataRow In dtUsers.Rows
                                        Dim nUserid As Integer = rowUser("UserId")
                                        Dim user As UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, nUserid)
                                        AddNodeInList(user.Username, user.UserID)
                                    Next
                                    AddNodeInList(sTeamName, teamId)
                                End If
                            Next

                        End If
                    Next

                End If

            Next

        End If


        ''''''''''''''''''''''''''''''''''''''''''''' sorting '''''''''''''''''''''''''''

        SortUsersInTree(tvUsersInJobStages.Nodes)
        lstUsers.SortItems()


    End Sub

    Protected Sub RadToolTipmanager_AjaxUpdate(ByVal sender As Object, ByVal e As ToolTipUpdateEventArgs)

        Dim roleName As String = e.Value
        Dim listOfUsersInToolTip As New RadListBox
        Dim label As New Label
        listOfUsersInToolTip.Width = 150
        listOfUsersInToolTip.Height = 200
        If IsUserRole(e.Value) Then

            Dim sql As String = "select UserId, TeamId from FF_UserTeamMapping where TeamId = (select ID from FF_Team where TeamName = '" & e.Value & "')"
            Dim dt As DataTable = DNNDB.Query(sql)
            'Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, roleName)
            If dt IsNot Nothing And dt.Rows.Count <> 0 Then
                For Each row As DataRow In dt.Rows
                    Dim nUserId As Integer = row("UserId")
                    Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)
                    listOfUsersInToolTip.Items.Add(New RadListBoxItem(userInfo.Username, userInfo.UserID))
                Next
                listOfUsersInToolTip.Sort = RadListBoxSort.Ascending
                listOfUsersInToolTip.SortItems()
                RadToolTipManager.Width = "150"
                RadToolTipManager.Height = "200"
                RadToolTipManager.Position = ToolTipPosition.TopRight
                listOfUsersInToolTip.BackColor = Color.DimGray
                listOfUsersInToolTip.ID = Guid.NewGuid().ToString()
                listOfUsersInToolTip.ID = Guid.NewGuid().ToString()
                e.UpdatePanel.ContentTemplateContainer.Controls.Add(listOfUsersInToolTip)

            End If
        Else
            For Each role As RadComboBoxItem In cmbRoleName.Items
                Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserByName(DNN.GetPMB(Me).PortalId, e.Value)
                Dim found = FindUserInTeam(role.Value, userInfo.UserID)
                If found Then
                    label.Text = e.Value & " belongs to " & role.Text
                    RadToolTipManager.Width = "160"
                    RadToolTipManager.Height = "50"
                    RadToolTipManager.BackColor = Color.DimGray
                    listOfUsersInToolTip.BackColor = Color.DimGray
                    listOfUsersInToolTip.ID = Guid.NewGuid().ToString()
                    e.UpdatePanel.ContentTemplateContainer.Controls.Add(label)
                    Exit For

                Else
                    label.Text = e.Value & " doesn't belong to any team"
                    RadToolTipManager.Width = "160"
                    RadToolTipManager.Height = "50"
                    RadToolTipManager.BackColor = Color.DimGray
                    listOfUsersInToolTip.BackColor = Color.DimGray
                    listOfUsersInToolTip.ID = Guid.NewGuid().ToString()
                    e.UpdatePanel.ContentTemplateContainer.Controls.Add(label)

                End If


            Next

        End If



    End Sub

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub

End Class
