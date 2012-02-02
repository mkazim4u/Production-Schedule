Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports Telerik.Web.UI

Partial Class PS_TeamManagement
    Inherits System.Web.UI.UserControl


    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo
    Dim gdv As DataView

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

    Public ReadOnly Property BasePage As DotNetNuke.Framework.CDefault
        Get
            Return CType(Me.Page, DotNetNuke.Framework.CDefault)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Call InitializeControls()
        Else
            gvTeam.DataSource = pdtTeam

        End If

    End Sub
    Protected Sub InitializeControls()

        Call PopulateUsersList()
        pdtTeam = FetchTeams()
        Call BindTeamGrid(-1)
        Call PopulateTeamsInTreeView()

    End Sub
    Protected Function FetchTeams() As DataTable

        pdtTeam = DNNDB.ExecuteStoredProcedure("FF_GetAllTeams")
        Return pdtTeam

    End Function

    Protected Sub BindTeamGrid(ByVal nEditIndex As Integer)


        gvTeam.EditIndex = nEditIndex
        gvTeam.DataSource = pdtTeam
        gvTeam.DataBind()

    End Sub

    Protected Sub SetTitles()

        Me.BasePage.Title = FF_GLOBALS.PAGE_TITLE_TEXT + " - " + FF_GLOBALS.PAGE_TEAM_MANAGEMENT

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Call AddTeam()

    End Sub

    Protected Function GetControl(ByRef gvr As GridViewRow, ByVal sControlName As String) As Control
        GetControl = Nothing
        Dim bFound As Boolean = False
        For Each tc As TableCell In gvr.Cells
            GetControl = tc.FindControl(sControlName)
            If GetControl IsNot Nothing Then
                Exit For
            End If
        Next
    End Function

    Public Shared Function FindControlInHeirarchy(ByVal root As Control, ByVal controlId As String) As Control
        If root.ID = controlId Then
            Return root
        End If

        For Each control As Control In root.Controls
            Dim fintCtl As Control = FindControlInHeirarchy(control, controlId)
            If fintCtl IsNot Nothing Then
                Return fintCtl
            End If
        Next

        Return Nothing
    End Function

    Protected Sub lnkbtnEditTeam_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender
        'gdv = New DataView(pdtTeam)
        'gdv.Sort = "RoleId"
        'Dim nIndex As Integer = gdv.Find(lb.CommandArgument)

        Dim gvr As GridViewRow = lb.NamingContainer
        Dim rowIndex As Integer = gvr.RowIndex

        'Dim tbTeamDescription As TextBox = CType(GetControl(gvr, "tbTeamDescription"), TextBox)
        'tbTeamDescription.Focus()
        Call BindTeamGrid(rowIndex)


    End Sub
    Protected Sub lnkbtnRemoveTeam_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lb As LinkButton = sender


    End Sub

    Protected Sub lnkbtnCreatedColumn_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        gdv = New DataView(pdtTeam)
        gdv.Sort = "CreatedOn DESC"
        pdtTeam = gdv.ToTable()
        gvTeam.DataSource = pdtTeam
        gvTeam.DataBind()

    End Sub

    Protected Sub lnkbtnTeamName_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        gdv = New DataView(pdtTeam)
        gdv.Sort = "TeamName ASC"
        pdtTeam = gdv.ToTable()
        gvTeam.DataSource = pdtTeam
        gvTeam.DataBind()

    End Sub

    'roleInfo = rc.GetRoleByName(DNN.GetPMB(Me).PortalId, sRoleName)

    'If dt IsNot Nothing And dt.Rows.Count <> 0 And roleInfo Is Nothing Then

    '    userRoleInfo.RoleGroupID = Convert.ToInt64(dt.Rows(0)("RoleGroupId"))
    '    userRoleInfo.RoleName = tbTeamName.Text.Trim()
    '    userRoleInfo.Description = tbTeamDescription.Text.Trim()
    '    userRoleInfo.PortalID = DNN.GetPMB(Me).PortalId

    '    rc.AddRole(userRoleInfo)

    'Dim sqlParamRoleGroupName(0) As SqlParameter
    'sqlParamRoleGroupName(0) = New SqlClient.SqlParameter("@RoleGroupName", SqlDbType.VarChar)
    'sqlParamRoleGroupName(0).Value = FF_GLOBALS.ROLE_GROUP_NAME
    'Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetRoleGroupIdByName", sqlParamRoleGroupName)
    'Dim roleInfo As New RoleInfo
   

    Protected Sub AddTeam()

        Dim sTeamName, sDescription As String

        sTeamName = tbTeamName.Text.Trim()
        sDescription = tbTeamDescription.Text.Trim()

        Dim ffTeam As New FF_Team

        Dim sql As String = "select * from ff_team where teamname = '" & sTeamName & "'"
        Dim dt As DataTable = DNNDB.Query(sql)
        If dt IsNot Nothing And dt.Rows.Count = 0 Then

            ffTeam.TeamName = sTeamName
            ffTeam.TeamDescription = sDescription
            ffTeam.IsActive = 1
            ffTeam.CreatedOn = DateTime.Now
            ffTeam.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            Dim teamId As Integer = ffTeam.Add()

            '''''''''''''''''' Adding a node in treeview ''''''''''''''''''''''''''

            Dim node As New RadTreeNode(sTeamName, teamId)
            tvTeams.Nodes.Add(node)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim script As ClientScriptManager = Page.ClientScript
            If (script.IsStartupScriptRegistered(Me.GetType(), "clear")) = False Then
                script.RegisterStartupScript(Me.GetType(), "clear", "Reset();", True)
            End If

            tbTeamName.Text = ""
            tbTeamDescription.Text = ""
            pdtTeam = FetchTeams() ' getting all teams

            BindTeamGrid(-1)


        Else

            rapUserRoleAssignment.Alert("Team Already Exists !!!")
            tbTeamName.Text = ""
            tbTeamDescription.Text = ""
            tbTeamName.Focus()

        End If

    End Sub


    Protected Sub gvTeam_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTeam.PageIndexChanging
        gvTeam.PageIndex = e.NewPageIndex
        Call BindTeamGrid(-1)
    End Sub
    
    Protected Sub gvTeam_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTeam.RowDataBound

        If (e.Row.RowState And DataControlRowState.Edit) > 0 OrElse (e.Row.RowState And DataControlRowState.Insert) > 0 Then

            Dim txtTeamName As TextBox = e.Row.FindControl("txtTeamName")
            If Not txtTeamName Is Nothing Then
                txtTeamName.Focus()
            End If

        End If


    End Sub
    'Dim roleInfo As RoleInfo = rc.GetRole(hidRoleId.Value, DNN.GetPMB(Me).PortalId)
    'roleInfo.Description = tbDescription.Text.Trim()
    'rc.UpdateRole(roleInfo)

    Protected Sub gvTeam_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvTeam.RowUpdating

        Dim gvr As GridViewRow = gvTeam.Rows(e.RowIndex)
        Dim tbDescription As TextBox = gvr.FindControl("tbTeamDescription")
        Dim txtTeamName As TextBox = gvr.FindControl("txtTeamName")
        Dim hidTeamId As HiddenField = gvr.FindControl("hidTeamID")
        Dim nTeamId As Integer = hidTeamId.Value
        Dim ff_Team As New FF_Team(nTeamId)

        ff_Team.TeamName = txtTeamName.Text.Trim()
        ff_Team.TeamDescription = tbDescription.Text.Trim()
        ff_Team.CreatedOn = DateTime.Now
        ff_Team.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        ff_Team.Update(nTeamId)
        pdtTeam.Rows(gvr.DataItemIndex)("TeamDescription") = tbDescription.Text.Trim()
        pdtTeam.Rows(gvr.DataItemIndex)("TeamName") = txtTeamName.Text.Trim()
        Call BindTeamGrid(-1)

    End Sub

    Protected Sub gvTeam_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvTeam.RowEditing

        Call BindTeamGrid(e.NewEditIndex)

    End Sub

    Protected Sub gvTeam_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvTeam.RowDeleting

        Dim gvr As GridViewRow = gvTeam.Rows(e.RowIndex)
        Dim drDeleted() As DataRow
        Dim hidTeamId As HiddenField = gvr.FindControl("hidTeamID")
        Dim lblTeamName As Label = gvr.FindControl("lblTeamName")
        Dim sTeamName As String = lblTeamName.Text
        Dim nTeamID As Integer = hidTeamID.Value


        drDeleted = pdtTeam.Select("ID=" & nTeamID, "")
        If (drDeleted IsNot Nothing) Then
            pdtTeam.Rows.Remove(drDeleted(0))
            Dim ff_Team As New FF_Team()
            ff_Team.Delete(nTeamID)
            'rc.DeleteRole(nID, DNN.GetPMB(Me).PortalId)
            Dim node As RadTreeNode = tvTeams.Nodes.FindNodeByText(sTeamName)
            tvTeams.Nodes.Remove(node)
        End If
        Call BindTeamGrid(-1)

    End Sub

    Protected Sub gvTeam_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvTeam.RowCancelingEdit
        Call BindTeamGrid(-1)
    End Sub

    Protected Sub rapUserRoleAssignment_AjaxRequest(ByVal sender As Object, ByVal e As Telerik.Web.UI.AjaxRequestEventArgs)
        If e.Argument IsNot Nothing Then

        End If
    End Sub

    Protected Sub PopulateUsersList()

        Dim lstUser As RadListBoxItem
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

    End Sub

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
            SortUsersInTree(tvTeams.Nodes)
        End If
    End Sub

    Protected Sub RemoveUsersFromTree(ByVal user As String)

        ''''''''''' work in this area ''''''''''''''
        For Each parentNode As RadTreeNode In tvTeams.Nodes
            Dim found As RadTreeNode = findNode(parentNode, user)
            If found IsNot Nothing Then
                parentNode.Nodes.Remove(found)
            End If
        Next
        lstUsers.SortItems()
        SortUsersInTree(tvTeams.Nodes)
    End Sub

    Protected Sub RemoveRoleFromTree(ByVal role As String)

        ''''''''''' work in this area ''''''''''''''
        For Each parentNode As RadTreeNode In tvTeams.Nodes
            Dim found As RadTreeNode = findNode(parentNode, role)
            If found IsNot Nothing Then
                parentNode.Nodes.Remove(found)
            End If
        Next
        lstUsers.SortItems()
        SortUsersInTree(tvTeams.Nodes)
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
        'Dim item As RadComboBoxItem = cmbRoleName.FindItemByText(str)
        Dim isRole As Boolean = False
        'If item IsNot Nothing Then
        '    isRole = True
        'End If
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
        Dim ic As ICollection = tvTeams.GetAllNodes()
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

    Protected Function FindUserInRole(ByVal role As String, ByVal username As String) As Boolean
        Dim found As Boolean = False
        Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, role)
        If usersList.Count <> 0 Then
            For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
                If username = userInfo.Username Then
                    found = True
                    Return found
                End If
            Next
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
        For Each childNode As RadTreeNode In tvTeams.Nodes
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
            For Each parentNode As RadTreeNode In tvTeams.Nodes
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
                                If FindUserInRole(childNode.Text, user.Text) Then
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

        For Each item As RadListBoxItem In removeUsersList
            lstUsers.Items.Remove(item)
        Next


    End Sub

    Protected Sub PopulateTeamsInTreeView()

        For Each dr As DataRow In pdtTeam.Rows
            Dim sTeamName As String = dr("TeamName")
            Dim nTeamId As String = dr("ID")
            Dim node As New RadTreeNode(sTeamName, nTeamId)
            tvTeams.Nodes.Add(node)

            Dim sql As String = "select * from ff_UserTeamMapping where TeamId = " & nTeamId

            Dim dt As DataTable = DNNDB.Query(sql)

            If dt IsNot Nothing And dt.Rows.Count <> 0 Then

                Dim teamNode As RadTreeNode = tvTeams.Nodes.FindNodeByValue(nTeamId)

                For Each row As DataRow In dt.Rows

                    Dim userId As Integer = Convert.ToInt64(row("UserId"))
                    Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, userId)
                    Dim userNode As New RadTreeNode(userInfo.Username, userInfo.UserID)
                    teamNode.Nodes.Add(userNode)

                Next

            End If

        Next

        'Dim alUsers As ArrayList = dt.
        'parentNode.Nodes.Add (

        'Dim node As New RadTreeNode(FF_GLOBALS.PS_SUPER_USER_ROLE, 0)
        'tvTeams.Nodes.Add(node)
        'node = New RadTreeNode(FF_GLOBALS.PS_MANAGER_ROLE, 1)
        'tvTeams.Nodes.Add(node)
        'node = New RadTreeNode(FF_GLOBALS.PS_USER_ROLE, 2)
        'tvTeams.Nodes.Add(node)

        'Dim alUsers As ArrayList = UserController.GetUsers(DNN.GetPMB(Me).PortalId)

        'For Each user As UserInfo In alUsers
        '    Dim userId As Integer = user.UserID
        '    Dim alUserRoles As ArrayList = rc.GetUserRoles(DNN.GetPMB(Me).PortalId, userId)
        '    For Each userRole As UserRoleInfo In alUserRoles
        '        If String.Compare(userRole.RoleName, FF_GLOBALS.PS_SUPER_USER_ROLE, True) = 0 Then
        '            node = New RadTreeNode(user.Username, user.UserID)
        '            tvTeams.Nodes(0).Nodes.Add(node)
        '        ElseIf String.Compare(userRole.RoleName, FF_GLOBALS.PS_MANAGER_ROLE, True) = 0 Then
        '            node = New RadTreeNode(user.Username, user.UserID)
        '            tvTeams.Nodes(1).Nodes.Add(node)
        '        ElseIf String.Compare(userRole.RoleName, FF_GLOBALS.PS_USER_ROLE, True) = 0 Then
        '            node = New RadTreeNode(user.Username, user.UserID)
        '            tvTeams.Nodes(2).Nodes.Add(node)
        '        End If
        '    Next
        'Next


    End Sub

    Protected Sub lstUsers_ItemDropped(ByVal sender As Object, ByVal e As RadListBoxDroppedEventArgs)

        Dim found As RadTreeNode

        If e.HtmlElementID = tvTeams.ClientID Then
            If tvTeams.SelectedNode IsNot Nothing Then
                For Each item As RadListBoxItem In e.SourceDragItems
                    Dim node As New RadTreeNode(item.Text, item.Value)

                    If tvTeams.SelectedNode.Text = "All Stages" Then
                        'item.Remove()
                        'For Each parentNode As RadTreeNode In tvTeams.Nodes
                        '    Dim childNode As New RadTreeNode(item.Text, item.Value)

                        '    If parentNode.Text <> "All Stages" Then
                        '        found = findNode(parentNode, childNode.Text)
                        '        If found Is Nothing Then
                        '            parentNode.Nodes.Add(childNode)
                        '            'RadToolTipManager.TargetControls.Add(node.Attributes(node.Value), node.Value, True)
                        '            If IsUserRole(childNode.Text) Then
                        '                ''''''''''''' commented for the time being ''''''''''''''''''''' 10th feb,2011
                        '                'RemoveUsersFromListForRole(childNode.Text)
                        '                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '                ''''''''''' find and remove users from treee '''''''''
                        '                FindUsersInRoleInTree(childNode)
                        '            End If
                        '        End If
                        '    End If
                        'Next
                    ElseIf tvTeams.SelectedNode.Level = 0 Then

                        found = findNode(tvTeams.SelectedNode, node.Text)
                        If found Is Nothing Then

                            If IsUserRole(node.Text) Then
                                Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, node.Text)
                                If usersList.Count <> 0 Then
                                    For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                                        userInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
                                        Dim foundNode As RadTreeNode = findNode(tvTeams.SelectedNode, userInfo.Username)

                                        If foundNode IsNot Nothing Then
                                            RemoveUsersFromTree(userInfo.Username)
                                        Else
                                            Dim tempNode As New RadTreeNode(userInfo.Username, userInfo.UserID)
                                            tvTeams.SelectedNode.Nodes.Add(tempNode)
                                            If findUserExistInAllStages(tempNode.Text) Then
                                                Dim removeUser As RadListBoxItem = lstUsers.FindItemByText(tempNode.Text)
                                                If removeUser IsNot Nothing Then
                                                    lstUsers.Items.Remove(removeUser)
                                                End If
                                            End If
                                            tvTeams.SelectedNode.Nodes.Remove(tempNode)
                                        End If
                                    Next

                                    tvTeams.SelectedNode.Nodes.Add(node)
                                    'If findUserExistInAllStages(item.Text) Then
                                    '    item.Remove()
                                    'End If


                                End If

                            ElseIf Not (FindUserInStage(tvTeams.SelectedNode, node)) Then
                                tvTeams.SelectedNode.Nodes.Add(node)
                                Dim ff_UserTeamMapping As New FF_UserTeamMapping()
                                Dim nTeamId As Integer = tvTeams.SelectedNode.Value
                                Dim nUserId As Integer = node.Value
                                ff_UserTeamMapping.TeamId = nTeamId
                                ff_UserTeamMapping.UserId = nUserId
                                ff_UserTeamMapping.CreatedOn = DateTime.Now
                                ff_UserTeamMapping.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                                ff_UserTeamMapping.Add()
                                'Dim userRoleInfo As RoleInfo = rc.GetRoleByName(DNN.GetPMB(Me).PortalId, tvTeams.SelectedNode.Text)
                                'rc.UpdateUserRole(DNN.GetPMB(Me).PortalId, node.Value, userRoleInfo.RoleID)

                            End If
                        End If
                    Else
                        tvTeams.SelectedNode.InsertAfter(node)
                    End If
                Next

            End If

        End If

        'FindUserExistInEachTreeNode()
        'lstUsers.SortItems()
        SortUsersInTree(tvTeams.Nodes)
    End Sub

    Private Function FindUserInStage(ByVal parentNode As RadTreeNode, ByVal userNode As RadTreeNode) As Boolean
        Dim bfound As Boolean = False
        For Each childNode As RadTreeNode In parentNode.Nodes
            If IsUserRole(childNode.Text) Then
                If FindUserInRole(childNode.Text, userNode.Text) Then
                    bfound = True
                    Exit For
                End If
            End If
        Next
        Return bfound
    End Function

    Protected Sub tvTeams_NodeDrop(ByVal sender As Object, ByVal e As RadTreeNodeDragDropEventArgs)

        Dim found As RadTreeNode
        Dim item As New RadListBoxItem()

        'Dim str1 As String = e.DestDragNode.Text
        'Dim str2 As String = e.DropPosition. .DraggedNodes.Count 
        item.Text = e.SourceDragNode.Text
        item.Value = e.SourceDragNode.Value

        If e.HtmlElementID = lstUsers.ClientID Then

            Dim userRole As RoleInfo = rc.GetRoleByName(DNN.GetPMB(Me).PortalId, e.SourceDragNode.ParentNode.Text)
            Dim sql As String = "delete from ff_UserTeamMapping where userid = " & item.Value & "and teamid = " & e.SourceDragNode.ParentNode.Value
            DNNDB.Query(sql)
            'rc.DeleteUserRole(DNN.GetPMB(Me).PortalId, item.Value, userRole.RoleID)

        End If

        e.SourceDragNode.Remove()
        lstUsers.SortItems()
        SortUsersInTree(tvTeams.Nodes)
    End Sub

    Protected Sub tvTeams_ContextMenuItemClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeViewContextMenuEventArgs)
        Select Case e.MenuItem.Value
            Case "Remove"
                If e.Node.Level = 1 Then
                    e.Node.Remove()
                    'AddNodeInList(e.Node.Text, e.Node.Value)
                    SortUsersInTree(tvTeams.Nodes)
                    lstUsers.SortItems()
                End If
                Exit Select
            Case "Remove All"
                If e.Node.Level = 1 Then
                    RemoveUsersFromTree(e.Node.Text)
                    'AddNodeInList(e.Node.Text, e.Node.Value)
                    SortUsersInTree(tvTeams.Nodes)
                End If
                Exit Select
        End Select
    End Sub

    Protected Sub SortUsersInTree(ByVal nodes As RadTreeNodeCollection)

        Dim i, j As Integer

        Dim IC As ICollection = tvTeams.GetAllNodes()

        For Each node As RadTreeNode In IC

            For i = node.Nodes.Count - 1 To 0 Step -1
                For j = 1 To i
                    If node.Nodes(j - 1).Text.CompareTo(node.Nodes(j).Text) > 0 Then
                        Dim swap As String = node.Nodes(j - 1).Text
                        node.Nodes(j - 1).Text = node.Nodes(j).Text
                        node.Nodes(j).Text = swap
                    End If
                Next
            Next


        Next


    End Sub

    'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
    'Dim sQueryParams(0) As String
    'sQueryParams(0) = "job=" & pnJobID
    'Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    'End Sub

    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
    'Dim sSQLPreamble As String = "INSERT INTO FF_UserJobMapping (JobID, JobStateID, UserID, RoleID, IsEmail, CreatedOn,CreatedBy) VALUES ("
    'Dim sSQL As String
    'Call DNNDB.Query("DELETE FROM FF_UserJobMapping WHERE JobID = " & pnJobID)
    'Dim IC As ICollection = tvTeams.GetAllNodes()
    'For Each node As RadTreeNode In IC
    '    For i = node.Nodes.Count - 1 To 0 Step -1
    '        Dim findItem As New RadComboBoxItem(node.Nodes(i).Text, node.Nodes(i).Value)
    '        findItem = cmbRoleName.Items.FindItemByText(node.Nodes(i).Text)
    '        If findItem IsNot Nothing Then
    '            Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, findItem.Text)
    '            If usersList.Count <> 0 Then
    '                For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
    '                    Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
    '                    sSQL = sSQLPreamble & pnJobID & ", " & node.Value & "," & user.UserID & "," & node.Nodes(i).Value & "," & 1 & "," & "'" & DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") & "'" & "," & DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID & ")"
    '                    Call DNNDB.Query(sSQL)
    '                Next
    '            End If
    '        Else
    '            sSQL = sSQLPreamble & pnJobID & ", " & node.Value & "," & node.Nodes(i).Value & "," & "null" & "," & 1 & "," & "'" & DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") & "'" & "," & DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID & ")"
    '            Call DNNDB.Query(sSQL)
    '        End If

    '    Next
    'Next
    'Dim sQueryParams(0) As String
    'sQueryParams(0) = "job=" & pnJobID
    'Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)
    'End Sub

    Protected Sub PopulateListAndTree()

        ''''''''''''''''' loading data using user string '''''''''''''''''''
        ''''''''''''''''' i can't store just roleid because if i just store roleid then in future if new user added in the role then it will also appear
        ''''''''''''''''' in the stage 


        ''''''''''''''''''''''''' UsersInJob TreeView ''''''''''''''''''''''''''''''''''''


        'Dim dtJobStages As DataTable = rc.GetRolesByGroup(DNN.GetPMB(Me).PortalId,

        'Dim dtJobStages As DataTable = pdtJobStages

        'If dtJobStages IsNot Nothing Then

        '    For Each dr As DataRow In dtJobStages.Rows '''''''''' stages

        '        Dim dtUsersInStages As DataTable = FF_UserJobMapping.FetchUserInJobStage(pnJobID, dr("id")) '''' users in stages

        '        If dtUsersInStages IsNot Nothing Then

        '            For Each drUser In dtUsersInStages.Rows

        '                Dim parentNode As RadTreeNode = tvTeams.Nodes.FindNodeByValue(dr("id"))

        '                '''''''''''''''''' adding role in a radtreenode '''''''''''''''''''''
        '                If drUser("RoleID") > 0 And drUser("RoleID") IsNot DBNull.Value Then
        '                    Dim roleInfo As RoleInfo = rc.GetRole(drUser("RoleID"), DNN.GetPMB(Me).PortalId)
        '                    Dim roleNode As New RadTreeNode(roleInfo.RoleName, drUser("Roleid"))
        '                    If FindNodeInStage(parentNode, roleNode.Text) = False Then
        '                        parentNode.Nodes.Add(roleNode)
        '                        AddNodeInList(roleInfo.RoleName, roleInfo.RoleID)
        '                    End If
        '                    Dim findItem As RadComboBoxItem = cmbRoleName.FindItemByText(roleInfo.RoleName)
        '                    If findItem IsNot Nothing Then
        '                        Dim chkRole As CheckBox = CType(findItem.FindControl("chkRole"), CheckBox)
        '                        chkRole.Checked = True
        '                    End If

        '                Else

        '                    Dim userNode As New RadTreeNode(drUser("username"), drUser("userid"))
        '                    parentNode.Nodes.Add(userNode)
        '                    Dim roleList As ArrayList = rc.GetUserRoles(DNN.GetPMB(Me).PortalId, drUser("userid"))
        '                    For Each uri As UserRoleInfo In roleList
        '                        Dim findItem As RadComboBoxItem = cmbRoleName.FindItemByText(uri.RoleName)
        '                        If findItem IsNot Nothing Then
        '                            Dim chkRole As CheckBox = CType(findItem.FindControl("chkRole"), CheckBox)
        '                            chkRole.Checked = True
        '                            Dim usersList As ArrayList = rc.GetUsersByRoleName(DNN.GetPMB(Me).PortalId, uri.RoleName)
        '                            For Each user As UserInfo In usersList
        '                                AddNodeInList(user.Username, user.UserID)
        '                                AddNodeInList(uri.RoleName, uri.RoleID)
        '                            Next
        '                        End If
        '                    Next

        '                End If
        '            Next

        '        End If

        '    Next

        'End If


        '''''''''''''''''''''''''''' Users listbox ''''''''''''''''''''''''''''''''''''''
        'Dim dtUsersInJob As DataTable = FF_UserJobMapping.FetchUserJobMapping(pnJobID)

        'If dtUsersInJob IsNot Nothing Then
        '    For Each dr As DataRow In dtUsersInJob.Rows
        '        If findUserExistInAllStages(dr("username")) = False Then
        '            AddNodeInList(dr("username"), dr("userid"))
        '        Else
        '            Dim findUser As RadListBoxItem = lstUsers.FindItemByText(dr("username"))
        '            lstUsers.Items.Remove(findUser)
        '        End If
        '    Next

        'End If


        ''''''''''''''''''''''''''''''''''''''''''''' sorting '''''''''''''''''''''''''''

        'SortUsersInTree(tvTeams.Nodes)
        'lstUsers.SortItems()


    End Sub

    Protected Sub RadToolTipmanager_AjaxUpdate(ByVal sender As Object, ByVal e As ToolTipUpdateEventArgs)

        Dim roleName As String = e.Value
        'Dim listOfUsersInToolTip As New RadListBox
        'Dim label As New Label
        'listOfUsersInToolTip.Width = 150
        'listOfUsersInToolTip.Height = 200
        'If IsUserRole(e.Value) Then
        '    Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, roleName)
        '    If usersList.Count <> 0 Then
        '        For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
        '            Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, user.UserID)
        '            listOfUsersInToolTip.Items.Add(New RadListBoxItem(userInfo.Username, user.UserID))
        '        Next
        '        listOfUsersInToolTip.Sort = RadListBoxSort.Ascending
        '        listOfUsersInToolTip.SortItems()
        '        RadToolTipManager.Width = "150"
        '        RadToolTipManager.Height = "200"
        '        RadToolTipManager.Position = ToolTipPosition.TopRight
        '        listOfUsersInToolTip.BackColor = Color.DimGray
        '        listOfUsersInToolTip.ID = Guid.NewGuid().ToString()
        '        listOfUsersInToolTip.ID = Guid.NewGuid().ToString()
        '        e.UpdatePanel.ContentTemplateContainer.Controls.Add(listOfUsersInToolTip)

        '    End If
        'Else
        'For Each role As RadComboBoxItem In cmbRoleName.Items
        '    If FindUserInRole(role.Text, e.Value) Then
        '        label.Text = e.Value & " belongs to " & role.Text
        '        RadToolTipManager.Width = "160"
        '        RadToolTipManager.Height = "50"
        '        RadToolTipManager.BackColor = Color.DimGray
        '        listOfUsersInToolTip.BackColor = Color.DimGray
        '        listOfUsersInToolTip.ID = Guid.NewGuid().ToString()
        '        e.UpdatePanel.ContentTemplateContainer.Controls.Add(label)
        '        Exit For
        '    End If
        'Next

        'End If



    End Sub

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub


End Class
