Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports Telerik.Web.UI

Partial Class PS_UserRoleAssignment
    Inherits System.Web.UI.UserControl

    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'pnJobID = GetJobIDFromQueryString()
                'pdtJobStages = FF_JobState.FetchJobStages(pnJobID)
                'GetAllStagesByJobID(pnJobID)
                Call InitializeUsersList()
                'Call PopulateListAndTree()
                Call AddRolesInTreeView()
                'Else
                '    gdtDataTable = pdtJobStages
            End If
        Catch ex As Exception
            WebMsgBox.Show(ex.Message.ToString())
        End Try

    End Sub

    Protected Sub InitializeUsersList()

        Call PopulateUsersList()
        'cmbRoleName.Items.Clear()
        'Call PopulateUserRoleDropdown()
        ' Call LoadTreeViewControl()

    End Sub


    Protected Sub PopulateUserRoleDropdown()

        'Dim sql As String
        'sql = "SELECT Roles.RoleID, Roles.RoleName FROM RoleGroups INNER JOIN Roles ON RoleGroups.RoleGroupID = Roles.RoleGroupID where rolegroupname='Production Schedule'"
        'Dim dt As DataTable = DNNDB.Query(sql)
        'cmbRoleName.DataSource = dt
        'cmbRoleName.DataTextField = "RoleName"
        'cmbRoleName.DataValueField = "RoleId"
        'cmbRoleName.DataBind()
        'Call CreateSelectAllUsersCheckBox()


    End Sub


    Protected Sub rapUserRoleAssignment_AjaxRequest(ByVal sender As Object, ByVal e As Telerik.Web.UI.AjaxRequestEventArgs)
        If e.Argument IsNot Nothing Then

            'Dim str As String = e.Argument.ToString
            'Dim sRole As String = String.Empty
            'Dim userRoleId As Integer
            'Dim parts As String() = str.Split(New Char() {"-"c})
            'sRole = parts(0)
            'If Integer.TryParse(parts(1).Replace("Remove", ""), userRoleId) Then
            'End If
            'If e.Argument.Contains("Remove") Then
            '    RemoveUsersFromList(sRole)
            '    RemoveRoleFromTree(sRole)
            'Else
            '    PopulateUsersList(sRole, userRoleId)
            'End If

        End If
    End Sub

    'Protected Sub 

    'Protected Sub PopulateUsersList(ByVal userRole As String, ByVal userRoleId As Long)
    Protected Sub PopulateUsersList()

        'If cmbRoleName.SelectedIndex <> 0 And userRole <> String.Empty Then
        '    Dim sRoleName As String = userRole
        Dim lstUser As RadListBoxItem

        '    If sRoleName <> FF_GLOBALS.SELECT_ALL_USERS_FROM_PS Then
        '        Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, sRoleName)
        '        Dim roleName As String = sRoleName
        '        lstRole = lstUsers.FindItemByText(roleName)
        '        If lstRole Is Nothing Then
        '            'If findUserExistInAllStages(roleName) = False Then
        '            lstUsers.Items.Add(New RadListBoxItem(roleName, userRoleId))
        '            'End If
        '        End If

        '        For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
        '            Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, user.UserID)

        '            Dim userName As String = userInfo.Username
        '            lstUser = lstUsers.FindItemByText(userName)

        '            If lstUser Is Nothing Then
        '                'If findUserExistInAllStages(userName) = False Then
        '                lstUsers.Items.Add(New RadListBoxItem(userName, user.UserID))
        '                'End If
        '            End If
        '        Next
        '        lstUsers.SortItems()
        '        'SortUsersInTree(tvUserRole.Nodes)
        '    ElseIf sRoleName = FF_GLOBALS.SELECT_ALL_USERS_FROM_PS Then
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

        '    End If

        'End If

        'FindUserExistInEachTreeNode()


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
            SortUsersInTree(tvUserRole.Nodes)
        End If
    End Sub

    Protected Sub RemoveUsersFromTree(ByVal user As String)

        ''''''''''' work in this area ''''''''''''''
        For Each parentNode As RadTreeNode In tvUserRole.Nodes
            Dim found As RadTreeNode = findNode(parentNode, user)
            If found IsNot Nothing Then
                parentNode.Nodes.Remove(found)
            End If
        Next
        lstUsers.SortItems()
        SortUsersInTree(tvUserRole.Nodes)
    End Sub

    Protected Sub RemoveRoleFromTree(ByVal role As String)

        ''''''''''' work in this area ''''''''''''''
        For Each parentNode As RadTreeNode In tvUserRole.Nodes
            Dim found As RadTreeNode = findNode(parentNode, role)
            If found IsNot Nothing Then
                parentNode.Nodes.Remove(found)
            End If
        Next
        lstUsers.SortItems()
        SortUsersInTree(tvUserRole.Nodes)
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
        'Dim isRole As Boolean = False
        'If item IsNot Nothing Then
        '    isRole = True
        'End If
        'IsUserRole = isRole

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
        Dim ic As ICollection = tvUserRole.GetAllNodes()
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
        For Each childNode As RadTreeNode In tvUserRole.Nodes
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
            For Each parentNode As RadTreeNode In tvUserRole.Nodes
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

    Protected Sub AddRolesInTreeView()


        Dim node As New RadTreeNode(FF_GLOBALS.ROLE_SUPER_USER, 0)
        tvUserRole.Nodes.Add(node)
        node = New RadTreeNode(FF_GLOBALS.ROLE_MANAGER, 1)
        tvUserRole.Nodes.Add(node)
        node = New RadTreeNode(FF_GLOBALS.ROLE_ACCOUNT_HANDLER, 2)
        tvUserRole.Nodes.Add(node)
        'node = New RadTreeNode(FF_GLOBALS.ROLE_USER, 3)
        'tvUserRole.Nodes.Add(node)


        Dim alUsers As ArrayList = UserController.GetUsers(DNN.GetPMB(Me).PortalId)

        For Each user As UserInfo In alUsers
            Dim userId As Integer = user.UserID
            Dim alUserRoles As ArrayList = rc.GetUserRoles(DNN.GetPMB(Me).PortalId, userId)
            For Each userRole As UserRoleInfo In alUserRoles
                If String.Compare(userRole.RoleName, FF_GLOBALS.ROLE_SUPER_USER, True) = 0 Then
                    node = New RadTreeNode(user.Username, user.UserID)
                    tvUserRole.Nodes(0).Nodes.Add(node)
                ElseIf String.Compare(userRole.RoleName, FF_GLOBALS.ROLE_MANAGER, True) = 0 Then
                    node = New RadTreeNode(user.Username, user.UserID)
                    tvUserRole.Nodes(1).Nodes.Add(node)
                ElseIf String.Compare(userRole.RoleName, FF_GLOBALS.ROLE_ACCOUNT_HANDLER, True) = 0 Then
                    node = New RadTreeNode(user.Username, user.UserID)
                    tvUserRole.Nodes(2).Nodes.Add(node)
                    'ElseIf String.Compare(userRole.RoleName, FF_GLOBALS.ROLE_USER, True) = 0 Then
                    '    node = New RadTreeNode(user.Username, user.UserID)
                    '    tvUserRole.Nodes(3).Nodes.Add(node)
                End If
            Next
        Next


    End Sub

    Protected Sub lstUsers_ItemDropped(ByVal sender As Object, ByVal e As RadListBoxDroppedEventArgs)

        Dim found As RadTreeNode

        If e.HtmlElementID = tvUserRole.ClientID Then
            If tvUserRole.SelectedNode IsNot Nothing Then
                For Each item As RadListBoxItem In e.SourceDragItems
                    Dim node As New RadTreeNode(item.Text, item.Value)

                    If tvUserRole.SelectedNode.Text = "All Stages" Then
                        'item.Remove()
                        For Each parentNode As RadTreeNode In tvUserRole.Nodes
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
                    ElseIf tvUserRole.SelectedNode.Level = 0 Then

                        found = findNode(tvUserRole.SelectedNode, node.Text)
                        If found Is Nothing Then

                            If IsUserRole(node.Text) Then
                                Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, node.Text)
                                If usersList.Count <> 0 Then
                                    For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                                        userInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
                                        Dim foundNode As RadTreeNode = findNode(tvUserRole.SelectedNode, userInfo.Username)

                                        If foundNode IsNot Nothing Then
                                            RemoveUsersFromTree(userInfo.Username)
                                        Else
                                            Dim tempNode As New RadTreeNode(userInfo.Username, userInfo.UserID)
                                            tvUserRole.SelectedNode.Nodes.Add(tempNode)
                                            If findUserExistInAllStages(tempNode.Text) Then
                                                Dim removeUser As RadListBoxItem = lstUsers.FindItemByText(tempNode.Text)
                                                If removeUser IsNot Nothing Then
                                                    lstUsers.Items.Remove(removeUser)
                                                End If
                                            End If
                                            tvUserRole.SelectedNode.Nodes.Remove(tempNode)
                                        End If
                                    Next

                                    tvUserRole.SelectedNode.Nodes.Add(node)
                                    'If findUserExistInAllStages(item.Text) Then
                                    '    item.Remove()
                                    'End If


                                End If

                            ElseIf Not (FindUserInStage(tvUserRole.SelectedNode, node)) Then
                                tvUserRole.SelectedNode.Nodes.Add(node)
                                Dim userRoleInfo As RoleInfo = rc.GetRoleByName(DNN.GetPMB(Me).PortalId, tvUserRole.SelectedNode.Text)
                                rc.UpdateUserRole(DNN.GetPMB(Me).PortalId, node.Value, userRoleInfo.RoleID)

                            End If
                        End If
                    Else
                        tvUserRole.SelectedNode.InsertAfter(node)
                    End If
                Next

            End If

        End If

        'FindUserExistInEachTreeNode()
        'lstUsers.SortItems()
        SortUsersInTree(tvUserRole.Nodes)
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

    Protected Sub tvUserRole_NodeDrop(ByVal sender As Object, ByVal e As RadTreeNodeDragDropEventArgs)

        Dim found As RadTreeNode
        Dim item As New RadListBoxItem()

        'Dim str1 As String = e.DestDragNode.Text
        'Dim str2 As String = e.DropPosition. .DraggedNodes.Count 
        item.Text = e.SourceDragNode.Text
        item.Value = e.SourceDragNode.Value

        If e.HtmlElementID = lstUsers.ClientID Then

            Dim userRole As RoleInfo = rc.GetRoleByName(DNN.GetPMB(Me).PortalId, e.SourceDragNode.ParentNode.Text)
            rc.DeleteUserRole(DNN.GetPMB(Me).PortalId, item.Value, userRole.RoleID)

        End If

        e.SourceDragNode.Remove()
        '    Dim existingItem As New RadListBoxItem
        '    existingItem = lstUsers.FindItemByText(item.Text)

        '    If existingItem Is Nothing Then

        '        If lstUsers.SelectedIndex > -1 Then
        '            lstUsers.Items.Insert(lstUsers.SelectedIndex + 1, item)
        '        Else
        '            lstUsers.Items.Add(item)
        '            If IsUserRole(item.Text) Then
        '                Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, item.Text)
        '                If usersList.Count <> 0 Then
        '                    Dim uc As New DotNetNuke.Entities.Users.UserController
        '                    For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
        '                        Dim userInfo As DotNetNuke.Entities.Users.UserInfo = uc.GetUser(DNN.GetPMB(Me).PortalId, user.UserID)
        '                        AddNodeInList(userInfo.Username, userInfo.UserID)
        '                    Next
        '                End If


        '            End If

        '        End If

        '    End If

        'Else
        '    If e.DestDragNode.Level = 0 Then
        '        If e.DestDragNode.Text <> "All Stages" Then
        '            found = findNode(e.DestDragNode, e.SourceDragNode.Text)
        '            If found Is Nothing Then
        '                e.DestDragNode.Nodes.Add(e.SourceDragNode)
        '            End If
        '        End If

        '    Else
        '        e.DestDragNode.InsertAfter(e.SourceDragNode)
        '    End If
        'End If

        lstUsers.SortItems()
        SortUsersInTree(tvUserRole.Nodes)
    End Sub

    Protected Sub tvUserRole_ContextMenuItemClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeViewContextMenuEventArgs)
        Select Case e.MenuItem.Value
            Case "Remove"
                If e.Node.Level = 1 Then
                    e.Node.Remove()
                    AddNodeInList(e.Node.Text, e.Node.Value)
                    SortUsersInTree(tvUserRole.Nodes)
                    lstUsers.SortItems()
                End If
                Exit Select
            Case "Remove All"
                If e.Node.Level = 1 Then
                    RemoveUsersFromTree(e.Node.Text)
                    AddNodeInList(e.Node.Text, e.Node.Value)
                    SortUsersInTree(tvUserRole.Nodes)
                End If
                Exit Select
        End Select
    End Sub

    Protected Sub SortUsersInTree(ByVal nodes As RadTreeNodeCollection)

        Dim i, j As Integer

        Dim IC As ICollection = tvUserRole.GetAllNodes()

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
    'Dim IC As ICollection = tvUserRole.GetAllNodes()
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

        '                Dim parentNode As RadTreeNode = tvUserRole.Nodes.FindNodeByValue(dr("id"))

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

        'SortUsersInTree(tvUserRole.Nodes)
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