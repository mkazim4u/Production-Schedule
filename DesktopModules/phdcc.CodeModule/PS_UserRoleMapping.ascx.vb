Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports Telerik.Web.UI

Partial Class PS_UserRoleMapping
    Inherits System.Web.UI.UserControl

    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private m_Page_prefiex As String = "PS_UserRole_Mapping"
    Private user_Id As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitializeCombos()
        End If
    End Sub
    Protected Sub PopulateUserRoleDropdown()
        rgi = DotNetNuke.Security.Roles.RoleController.GetRoleGroupByName(DNN.GetPMB(Me).PortalId, "Production Schedule")
        Dim a As ArrayList = rc.GetRolesByGroup(DNN.GetPMB(Me).PortalId, rgi.RoleGroupID)
        cmbRoleName.Items.Clear()
        cmbRoleName.Items.Add(New RadComboBoxItem("- please select -", 0))
        For Each ri As DotNetNuke.Security.Roles.RoleInfo In a
            Dim roleName As String = ri.RoleName
            cmbRoleName.Items.Add(New RadComboBoxItem(roleName))
        Next

    End Sub
    Protected Sub PopulateUserDropDown()
        If cmbRoleName.SelectedIndex <> 0 Then
            Dim sRoleName As String = cmbRoleName.Text.Trim()
            Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, sRoleName)
            cmbUserName.Items.Clear()
            cmbUserName.Items.Add(New RadComboBoxItem("- please select -", 0))
            For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList
                Dim userInfo As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, user.UserID)
                Dim userName As String = userInfo.Username
                cmbUserName.Items.Add(New RadComboBoxItem(userName, userInfo.UserID))
            Next

        End If
    End Sub
    Protected Sub InitializeCombos()
        cmbRoleName.Items.Clear()
        cmbUserName.Items.Clear()
        cmbUserName.Items.Add(New RadComboBoxItem("- please select -", 0))
        Call PopulateUserRoleDropdown()
    End Sub
    Sub cmbRoleName_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        PopulateUserDropDown()
    End Sub
    Sub cmbUserName_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Call BindDataListWithUserAssignedRoles()
    End Sub
    Sub dlRoleAssignment_ItemDataBound(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        'If e.Item.FindControl("rbHighPrivilege") IsNot Nothing Then
        'Dim rbHighPrivilege As CheckBox = CType(e.Item.FindControl("rbHighPrivilege"), CheckBox)
        'rbHighPrivilege.Attributes.Add("onclick", "SetUniqueRadioButton('" + rbHighPrivilege.ClientID + "')")
        'End If


    End Sub
    Sub BindDataListWithUserAssignedRoles()
        Dim privilegeValue As Boolean
        rgi = DotNetNuke.Security.Roles.RoleController.GetRoleGroupByName(DNN.GetPMB(Me).PortalId, "Production Schedule")
        Dim a As ArrayList = rc.GetRolesByGroup(DNN.GetPMB(Me).PortalId, rgi.RoleGroupID)
        dlRoleAssignment.DataSource = a
        dlRoleAssignment.DataBind()

        privilegeValue = GetProfilePropertyValue(cmbUserName.SelectedValue, "Privilege")
        If (privilegeValue) Then
            rbHighPrivilege.Checked = True
        End If


        For Each itemCollection As DataListItem In dlRoleAssignment.Items

            Dim grbg As ArrayList = rc.GetRolesByGroup(DNN.GetPMB(Me).PortalId, rgi.RoleGroupID)
            Dim userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
            Dim userRoles As ArrayList = rc.GetUserRoles(DNN.GetPMB(Me).PortalId, Val(cmbUserName.SelectedValue))
            Dim chkselectedRole As CheckBox = CType(itemCollection.FindControl("chkRole"), CheckBox)
            Dim lblRoleName As Label = CType(itemCollection.FindControl("lblRoleName"), Label)

            For Each userRole As DotNetNuke.Entities.Users.UserRoleInfo In userRoles

                If userRole.RoleName.Equals(lblRoleName.Text.Trim()) Then
                    chkselectedRole.Checked = True                    
                Else
                    chkselectedRole.Checked = privilegeValue
                End If

            Next

        Next

        '        Session(m_Page_prefiex & "dlRoleAssignment") = dlRoleAssignment

    End Sub

    Function GetProfilePropertyValue(ByVal userId As Integer, ByVal propertyName As String) As Boolean

        Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetUserById(DNN.GetPMB(Me).PortalId, userId)
        Dim ppc As DotNetNuke.Entities.Profile.ProfilePropertyDefinitionCollection = uiCurrentUser.Profile.ProfileProperties
        Dim ppd As DotNetNuke.Entities.Profile.ProfilePropertyDefinition = ppc.GetByName(propertyName)

        Return ppd.PropertyValue

    End Function

    Function SetProfilePropertyValue(ByVal userId As Integer, ByVal propertyName As String, ByVal propertyVale As Boolean) As DotNetNuke.Entities.Users.UserInfo

        Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetUserById(0, userId)
        Dim ppc As DotNetNuke.Entities.Profile.ProfilePropertyDefinitionCollection = uiCurrentUser.Profile.ProfileProperties
        uiCurrentUser.Profile.SetProfileProperty(propertyName, propertyVale)
        DotNetNuke.Entities.Profile.ProfileController.UpdateUserProfile(uiCurrentUser, uiCurrentUser.Profile.ProfileProperties)

        Return uiCurrentUser

    End Function

    Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetUserById(0, cmbUserName.SelectedValue)
        If rbHighPrivilege.Checked Then
            SetProfilePropertyValue(cmbUserName.SelectedValue, "Privilege", True)
        End If
        For Each itemCollection As DataListItem In dlRoleAssignment.Items
            Dim chkNewRoleSelected As CheckBox = CType(itemCollection.FindControl("chkRole"), CheckBox)
            If chkNewRoleSelected.Checked Then
                rc.UpdateUserRole(0, cmbUserName.SelectedValue, cmbRoleName.SelectedValue)
            End If

        Next





        'Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetUserById(0, cmbUserName.SelectedValue)
        'Dim userRoleInfo uri As DotNetNuke.Entities.
        'Dim userRoles As ArrayList = rc.UpdateRole()

        'Dim dlTemp As DataList = Session(m_Page_prefiex & "dlRoleAssignment")
        'If (dlTemp IsNot Nothing) Then
        '    Dim highPrivilege As Boolean = False
        '    For Each itemCollection As DataListItem In dlRoleAssignment.Items
        '        Dim chkNewRoleSelected As CheckBox = CType(itemCollection.FindControl("chkRole"), CheckBox)
        '        Dim rbHighPrivilege As RadioButton = CType(itemCollection.FindControl("rbHighPrivilege"), RadioButton)
        '        If chkNewRoleSelected.Checked Then
        '            If rbHighPrivilege.Checked Then
        '                highPrivilege = True
        '            End If
        '        End If


        '    Next
        '    If (highPrivilege) Then
        '        For int i = 0 ; i <dlTemp.co
        '    End If
        'End If
    End Sub

End Class
