Imports Microsoft.VisualBasic
Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity

Public Class DNN

    Const SETTING_CUSTOMER_NAME As String = "CustomerName"
    Const SETTING_USERID As String = "UserID"
    Const SETTING_CUSTOMER_KEY As String = "CustomerKey"
    Const SETTING_USERKEY As String = "UserKey"

    Public Shared Function GetPMB(ByRef Myself As Control) As DotNetNuke.Entities.Modules.PortalModuleBase
        GetPMB = Nothing
        Dim ViewControl As Control = Myself.TemplateControl.Parent
        Dim View As Control = ViewControl.Parent
        If TypeOf (View) Is DotNetNuke.Entities.Modules.PortalModuleBase Then
            GetPMB = CType(View, DotNetNuke.Entities.Modules.PortalModuleBase)
        End If
    End Function

    Public Shared Function GetPortalSettings(ByRef Myself As Object) As DotNetNuke.Entities.Portals.PortalSettings
        Dim pmb As DotNetNuke.Entities.Modules.PortalModuleBase = GetPMB(Myself)
        GetPortalSettings = pmb.PortalSettings
    End Function

    Public Shared Function GetModuleID(ByRef Myself As Object) As Integer
        Dim pmb As DotNetNuke.Entities.Modules.PortalModuleBase = GetPMB(Myself)
        GetModuleID = pmb.ModuleId
    End Function

    Public Shared Function GetCustomerKey(ByRef Myself As Control) As Integer
        GetCustomerKey = 0
        Dim pmb As PortalModuleBase = GetPMB(Myself)
        If IsNumeric(pmb.Settings(SETTING_CUSTOMER_KEY)) AndAlso pmb.Settings(SETTING_CUSTOMER_KEY) > 0 Then  ' local setting takes precedence
            GetCustomerKey = pmb.Settings(SETTING_CUSTOMER_KEY)
        Else
            Dim sUserID As String
            Dim up As New DotNetNuke.Entities.Users.UserInfo
            sUserID = up.UserID()
            Dim sSQL As String = "SELECT CustomerKey FROM UserProfile WHERE UserID = " & sUserID
            Dim oDataTable As DataTable = SprintDB.Query(sSQL)
            If oDataTable.Rows.Count > 0 Then
                If oDataTable.Rows.Count > 1 Then
                    Throw New Exception("More than one user found with UserID " & sUserID)
                Else
                    GetCustomerKey = oDataTable.Rows(0).Item(0)
                End If
            Else
                WebMsgBox.Show("User undefined.")
            End If
        End If
    End Function

    Public Shared Function GetUserKey(ByRef Myself As Control) As Integer
        GetUserKey = 0
        Dim pmb As PortalModuleBase = GetPMB(Myself)
        If IsNumeric(pmb.Settings(SETTING_USERKEY)) AndAlso pmb.Settings(SETTING_USERKEY) > 0 Then  ' local setting takes precedence
            GetUserKey = pmb.Settings(SETTING_USERKEY)
        Else
            Dim sUserID As String
            Dim up As New DotNetNuke.Entities.Users.UserInfo
            sUserID = up.UserID()
            Dim sSQL As String = "SELECT [key] FROM UserProfile WHERE UserID = " & sUserID
            Dim oDataTable As DataTable = SprintDB.Query(sSQL)
            If oDataTable.Rows.Count > 0 Then
                If oDataTable.Rows.Count > 1 Then
                    Throw New Exception("More than one user found with UserID " & sUserID)
                Else
                    GetUserKey = oDataTable.Rows(0).Item(0)
                End If
            Else
                WebMsgBox.Show("User undefined.")
            End If
        End If
    End Function

    Public Shared Sub PopulateCustomerDropdown(ByRef ddlCustomer As DropDownList)
        Dim sSQL As String
        sSQL = "SELECT CustomerAccountCode, CustomerKey FROM Customer WHERE CustomerStatusId = 'ACTIVE' ORDER BY CustomerAccountCode"
        Dim oListItemCollection As ListItemCollection = SprintDB.Query(sSQL, "CustomerAccountCode", "CustomerKey")
        ddlCustomer.Items.Add(New ListItem("- please select -", 0))
        For Each li As ListItem In oListItemCollection
            ddlCustomer.Items.Add(li)
        Next
    End Sub

    Public Shared Sub CustomerDropdownSelectedIndexChanged(ByRef ddlCustomer As DropDownList, ByRef ddlUser As DropDownList, ByVal nModuleID As Integer, ByRef lblCustomer As Label)
        If ddlCustomer.Items(0).Value = 0 Then
            ddlCustomer.Items.RemoveAt(0)
        End If
        If ddlCustomer.SelectedValue > 0 Then
            ddlUser.Enabled = True
            Dim sSQL As String = "SELECT UserId, [key] FROM UserProfile WHERE CustomerKey = " & ddlCustomer.SelectedValue & " AND Status = 'Active'"
            Dim oListItemCollection As ListItemCollection = SprintDB.Query(sSQL, "UserId", "key")
            ddlUser.Items.Clear()
            ddlUser.Items.Add(New ListItem("- please select -", 0))
            For Each li As ListItem In oListItemCollection
                ddlUser.Items.Add(li)
            Next

            Dim mc As New Entities.Modules.ModuleController
            mc.UpdateModuleSetting(nModuleID, SETTING_CUSTOMER_NAME, ddlCustomer.SelectedItem.Text)
            lblCustomer.Text = ddlCustomer.SelectedItem.Text
            mc.UpdateModuleSetting(nModuleID, SETTING_CUSTOMER_KEY, ddlCustomer.SelectedValue)
        End If
    End Sub

    Public Shared Sub UserDropdownSelectedIndexChanged(ByRef ddlUser As DropDownList, ByVal nModuleID As Integer, ByRef lblUser As Label)
        If ddlUser.Items(0).Value = 0 Then
            ddlUser.Items.RemoveAt(0)
        End If
        If ddlUser.SelectedValue > 0 Then
            Dim mc As New Entities.Modules.ModuleController
            mc.UpdateModuleSetting(nModuleID, SETTING_USERID, ddlUser.SelectedItem.Text)
            lblUser.Text = ddlUser.SelectedItem.Text
            mc.UpdateModuleSetting(nModuleID, SETTING_USERKEY, ddlUser.SelectedValue)
        End If
    End Sub

    Public Shared Sub GetCustomerAndUser(ByVal Myself As Object, ByRef lblCustomer As Label, ByRef lblUser As Label)
        Dim pmb As PortalModuleBase = GetPMB(Myself)
        lblCustomer.Text = pmb.Settings(SETTING_CUSTOMER_NAME)
        lblUser.Text = pmb.Settings(SETTING_USERID)
    End Sub

    Public Shared Sub DeleteSettings(ByVal Myself As Object, ByRef lblCustomer As Label, ByRef lblUser As Label)
        Dim pmb As PortalModuleBase = GetPMB(Myself)
        Dim mc As New Entities.Modules.ModuleController
        mc.DeleteModuleSetting(pmb.ModuleId, SETTING_CUSTOMER_NAME)
        mc.DeleteModuleSetting(pmb.ModuleId, SETTING_CUSTOMER_KEY)
        mc.DeleteModuleSetting(pmb.ModuleId, SETTING_USERID)
        mc.DeleteModuleSetting(pmb.ModuleId, SETTING_USERKEY)
        lblCustomer.Text = String.Empty
        lblUser.Text = String.Empty
    End Sub

    Public Shared Function GetUserInfo(ByVal Myself As Control, ByVal UserId As Integer) As DotNetNuke.Entities.Users.UserInfo
        Dim usercontroller As DotNetNuke.Entities.Users.UserController = New DotNetNuke.Entities.Users.UserController
        GetUserInfo = usercontroller.GetUser(DNN.GetPMB(Myself).PortalId, UserId)
    End Function

    Public Shared Function GetUserDisplayName(ByVal Myself As Control, ByVal UserId As Integer) As String
        GetUserDisplayName = GetUserInfo(Myself, UserId).DisplayName
    End Function

    Public Shared Function GetUserInitials(ByVal Myself As Control, ByVal UserId As Integer) As String
        GetUserInitials = GetUserInfo(Myself, UserId).FirstName.Substring(0, 1) & GetUserInfo(Myself, UserId).LastName.Substring(0, 1)
    End Function

End Class
