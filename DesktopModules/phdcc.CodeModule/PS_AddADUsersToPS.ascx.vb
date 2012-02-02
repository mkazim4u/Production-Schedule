Imports Telerik.OpenAccess
Imports FFDataLayer
Imports System.DirectoryServices
Imports Telerik.Web.UI

Partial Class PS_AddADUsersToPS
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffUser As New FFDataLayer.FF_User
    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo

    Property pnOrderID() As Integer
        Get
            Dim o As Object = ViewState("OrderID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("OrderID") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then


        End If

    End Sub

    Protected Sub BindDomainUsers(ByVal e As Telerik.Web.UI.GridItemEventArgs)

        Dim rcbUser As RadComboBox = e.Item.FindControl("rcbUser")

        rcbUser.DataSource = GetAllADDomainUsers()
        rcbUser.DataBind()
        rcbUser.Sort = RadComboBoxSort.Ascending
        rcbUser.SortItems()
        rcbUser.Items.Insert(0, New RadComboBoxItem("- Select User -", "-1"))

    End Sub

    Protected Sub BindRoles(ByVal e As Telerik.Web.UI.GridItemEventArgs)

        Dim rcbRole As RadComboBox = e.Item.FindControl("rcbRole")

        Dim alRoles As ArrayList = rc.GetRolesByGroup(DNN.GetPMB(Me).PortalId, FF_GLOBALS.ROLE_GROUP_ID)

        For Each role As DotNetNuke.Security.Roles.RoleInfo In alRoles

            Dim item As New RadComboBoxItem(role.RoleName, role.RoleID)
            rcbRole.Items.Add(item)

        Next

        rcbRole.Sort = RadComboBoxSort.Ascending
        rcbRole.SortItems()


        rcbRole.Items.Insert(0, New RadComboBoxItem("- Select Role -", "-1"))


    End Sub

    Protected Sub BindChildPages(ByVal e As Telerik.Web.UI.GridItemEventArgs)

        Dim rcbRedirectToFirstTime As RadComboBox = e.Item.FindControl("rcbRedirectToFirstTime")
        Dim rcbRedirectTo As RadComboBox = e.Item.FindControl("rcbRedirectTo")

        rcbRedirectTo.Items.Clear()
        rcbRedirectToFirstTime.Items.Clear()

        Dim hidParentTabID As HiddenField = e.Item.FindControl("hidParentTabID")

        Dim nParentTabId As Integer = hidParentTabID.Value

        Dim ITabs As List(Of DotNetNuke.Entities.Tabs.TabInfo) = GetChildTabs(nParentTabId)

        For Each tab As DotNetNuke.Entities.Tabs.TabInfo In ITabs

            Dim item As New RadComboBoxItem(tab.TabName, tab.TabID)

            rcbRedirectTo.Items.Add(item)

        Next

        For Each tab As DotNetNuke.Entities.Tabs.TabInfo In ITabs

            Dim item As New RadComboBoxItem(tab.TabName, tab.TabID)

            rcbRedirectToFirstTime.Items.Add(item)

        Next


        rcbRedirectTo.Sort = RadComboBoxSort.Ascending
        rcbRedirectTo.SortItems()

        rcbRedirectToFirstTime.Sort = RadComboBoxSort.Ascending
        rcbRedirectToFirstTime.SortItems()

        rcbRedirectTo.Items.Insert(0, New RadComboBoxItem("- Select Page -", "-1"))
        rcbRedirectToFirstTime.Items.Insert(0, New RadComboBoxItem("- Select Page -", "-1"))


    End Sub

    Protected Sub rcbParentTabs_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)

        Dim rcbParentTabs As RadComboBox = sender

        Dim rcbRedirectToFirstTime As RadComboBox = rcbParentTabs.NamingContainer.FindControl("rcbRedirectToFirstTime")
        Dim rcbRedirectTo As RadComboBox = rcbParentTabs.NamingContainer.FindControl("rcbRedirectTo")

        rcbRedirectTo.Items.Clear()
        rcbRedirectToFirstTime.Items.Clear()

        Dim tc As New DotNetNuke.Entities.Tabs.TabController

        Dim tabInfo As DotNetNuke.Entities.Tabs.TabInfo = tc.GetTab(e.Value, DNN.GetPMB(Me).PortalId, True)

        Dim nParentTabId As Integer = rcbParentTabs.SelectedValue

        Dim ITabs As List(Of DotNetNuke.Entities.Tabs.TabInfo) = GetChildTabs(nParentTabId)

        For Each tab As DotNetNuke.Entities.Tabs.TabInfo In ITabs

            Dim item As New RadComboBoxItem(tab.TabName, tab.TabID)

            rcbRedirectTo.Items.Add(item)

        Next

        For Each tab As DotNetNuke.Entities.Tabs.TabInfo In ITabs

            Dim item As New RadComboBoxItem(tab.TabName, tab.TabID)

            rcbRedirectToFirstTime.Items.Add(item)

        Next


        rcbRedirectTo.Sort = RadComboBoxSort.Ascending
        rcbRedirectTo.SortItems()

        rcbRedirectToFirstTime.Sort = RadComboBoxSort.Ascending
        rcbRedirectToFirstTime.SortItems()

        rcbRedirectTo.Items.Insert(0, New RadComboBoxItem("- Select Page -", "-1"))
        rcbRedirectToFirstTime.Items.Insert(0, New RadComboBoxItem("- Select Page -", "-1"))

    End Sub

    Protected Sub rblUserType_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim rbl As RadioButtonList = sender

        Dim rcbUser As RadComboBox = rbl.NamingContainer.FindControl("rcbUser")

        If rbl.SelectedValue = "0" Then

            rcbUser.Items.Clear()

            For Each user As DotNetNuke.Entities.Users.UserInfo In GetAllNormalUsers()

                Dim item As New RadComboBoxItem(user.Username, user.Username)
                rcbUser.Items.Add(item)

            Next

            rcbUser.Sort = RadComboBoxSort.Ascending
            rcbUser.SortItems()

            rcbUser.Items.Insert(0, New RadComboBoxItem("- Select User -", "-1"))

        ElseIf rbl.SelectedValue = "1" Then

            rcbUser.Items.Clear()
            rcbUser.DataSource = GetAllADDomainUsers()
            rcbUser.DataBind()
            rcbUser.Sort = RadComboBoxSort.Ascending
            rcbUser.SortItems()

            rcbUser.Items.Insert(0, New RadComboBoxItem("- Select User -", "-1"))

        End If


    End Sub
    Protected Sub BindParentPages(ByVal e As Telerik.Web.UI.GridItemEventArgs)

        Dim rcbParentTabs As RadComboBox = e.Item.FindControl("rcbParentTabs")

        For Each tab As DotNetNuke.Entities.Tabs.TabInfo In GetAllParentTabs()

            Dim item As New RadComboBoxItem(tab.TabName, tab.TabID)

            rcbParentTabs.Items.Add(item)

        Next

        rcbParentTabs.Sort = RadComboBoxSort.Ascending
        rcbParentTabs.SortItems()

        rcbParentTabs.Items.Insert(0, New RadComboBoxItem("- Select Parent Page -", "-1"))



    End Sub

    Protected Sub BindNormalUsers(ByVal e As Telerik.Web.UI.GridItemEventArgs)

        Dim rcbUser As RadComboBox = e.Item.FindControl("rcbUser")


        For Each user As DotNetNuke.Entities.Users.UserInfo In GetAllNormalUsers()

            Dim item As New RadComboBoxItem(user.Username, user.Username)
            rcbUser.Items.Add(item)

        Next

        rcbUser.Sort = RadComboBoxSort.Ascending
        rcbUser.SortItems()

        rcbUser.Items.Insert(0, New RadComboBoxItem("- Select User -", "-1"))


    End Sub

    Protected Function GetAllNormalUsers() As ArrayList

        Dim IList As ArrayList = DotNetNuke.Entities.Users.UserController.GetUsers(DNN.GetPMB(Me).PortalId)

        Return IList

    End Function

    Private Function GetAllADDomainUsers() As ArrayList

        Dim allUsers As New ArrayList()

        Dim domainpath As String = "LDAP://pdc.sprintexpress.co.uk"

        Dim searchRoot As New DirectoryEntry(domainpath)
        Dim search As New DirectorySearcher(searchRoot)
        search.Filter = "(&(objectClass=user)(objectCategory=person))"
        search.PropertiesToLoad.Add("samaccountname")

        Dim result As SearchResult
        Dim resultCol As SearchResultCollection = search.FindAll()
        If resultCol IsNot Nothing Then
            For counter As Integer = 0 To resultCol.Count - 1
                result = resultCol(counter)
                If result.Properties.Contains("samaccountname") Then
                    allUsers.Add(DirectCast(result.Properties("samaccountname")(0), [String]))
                End If
            Next
        End If

        Return allUsers


    End Function

    Private Function GetChildTabs(ByVal tabID As Integer) As List(Of DotNetNuke.Entities.Tabs.TabInfo)


        Dim tc As New DotNetNuke.Entities.Tabs.TabController

        'Dim pTabInfo As DotNetNuke.Entities.Tabs.TabInfo = tc.GetTabByName(FF_GLOBALS.PAGE_CREATE_MAIN_MENU, DNN.GetPMB(Me).PortalId)

        Dim IlistTabs As List(Of DotNetNuke.Entities.Tabs.TabInfo) = TabController.GetTabsByParent(tabID, DNN.GetPMB(Me).PortalId)

        Return IlistTabs


    End Function

    Private Function GetAllParentTabs() As List(Of DotNetNuke.Entities.Tabs.TabInfo)

        Dim IParentTabs As New List(Of DotNetNuke.Entities.Tabs.TabInfo)

        Dim tc As New DotNetNuke.Entities.Tabs.TabController

        Dim pTabInfo As DotNetNuke.Entities.Tabs.TabInfo = tc.GetTabByName(FF_GLOBALS.PAGE_CREATE_MAIN_MENU, DNN.GetPMB(Me).PortalId)

        Dim IlistTabs As List(Of DotNetNuke.Entities.Tabs.TabInfo) = TabController.GetPortalTabs(DNN.GetPMB(Me).PortalId, -1, False, True)

        For Each tabInfo As DotNetNuke.Entities.Tabs.TabInfo In IlistTabs

            If tabInfo.ParentId = -1 Then

                IParentTabs.Add(tabInfo)

            End If

        Next

        Return IParentTabs


    End Function

    Protected Sub rgFFUser_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgFFUser.NeedDataSource

        Dim IUsers = From Users In dbContext.FF_Users
                    Select Users

        rgFFUser.DataSource = IUsers.ToList

    End Sub
    Protected Sub rgFFUser_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgFFUser.ItemDataBound

        If TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode Then

            If e.Item.OwnerTableView.IsItemInserted Then

                BindNormalUsers(e)
                BindRoles(e)
                BindParentPages(e)

                Dim rcbRedirectToFirstTime As RadComboBox = e.Item.FindControl("rcbRedirectToFirstTime")
                Dim rcbRedirectTo As RadComboBox = e.Item.FindControl("rcbRedirectTo")
                rcbRedirectTo.Items.Insert(0, New RadComboBoxItem("- Select Page -", "-1"))
                rcbRedirectToFirstTime.Items.Insert(0, New RadComboBoxItem("- Select Page -", "-1"))

            Else


                BindRoles(e)
                BindParentPages(e)
                BindChildPages(e)

                'BindPages(e)

                Dim rcbUser As RadComboBox = e.Item.FindControl("rcbUser")
                Dim rcbRole As RadComboBox = e.Item.FindControl("rcbRole")
                Dim rcbParentTabs As RadComboBox = e.Item.FindControl("rcbParentTabs")
                Dim rcbRedirectToFirstTime As RadComboBox = e.Item.FindControl("rcbRedirectToFirstTime")
                Dim rcbRedirectTo As RadComboBox = e.Item.FindControl("rcbRedirectTo")
                Dim chkIsFirstTime As CheckBox = e.Item.FindControl("chkIsFirstTime")
                Dim hidFFUserID As HiddenField = e.Item.FindControl("hidFFUserID")
                Dim hidIsADUser As HiddenField = e.Item.FindControl("hidIsADUser")
                Dim hidParentTabID As HiddenField = e.Item.FindControl("hidParentTabID")
                Dim rblUserType As RadioButtonList = e.Item.FindControl("rblUserType")

                Dim isAdUser As Boolean = hidIsADUser.Value
                Dim nParentTabId As Integer = hidParentTabID.Value

                If isAdUser Then
                    BindDomainUsers(e)
                    rblUserType.SelectedValue = "1"
                Else
                    BindNormalUsers(e)
                    rblUserType.SelectedValue = "0"
                End If

                Dim nID As Int64 = hidFFUserID.Value
                ffUser = dbContext.GetObjectByKey(New ObjectKey(ffUser.GetType().Name, nID))
                rcbUser.SelectedItem.Text = ffUser.UserName
                rcbRole.SelectedValue = ffUser.RoleId
                rcbRedirectTo.SelectedValue = IIf(ffUser.RedirectTo = "-1", "- Select Page -", ffUser.RedirectTo)
                rcbRedirectToFirstTime.SelectedValue = IIf(ffUser.RedirectToFirstTime = "-1", "- Select Page -", ffUser.RedirectToFirstTime)
                'chkIsFirstTime.Checked = ffUser.IsFirstTime
                rcbParentTabs.SelectedValue = ffUser.ParentTabId

            End If

        ElseIf (TypeOf e.Item Is GridDataItem) Or (e.Item.ItemType = GridItemType.AlternatingItem) Then

            Dim lblRoleName As Label = e.Item.FindControl("lblRoleName")

            Dim lblRedirectToFirstTime As Label = e.Item.FindControl("lblRedirectToFirstTime")
            Dim lblRedirectTo As Label = e.Item.FindControl("lblRedirectTo")
            Dim hidRedirectToFirstTime As HiddenField = e.Item.FindControl("hidRedirectToFirstTime")
            Dim hidRedirectTo As HiddenField = e.Item.FindControl("hidRedirectTo")



            Dim nRoleId As Integer = Convert.ToInt64(lblRoleName.Text)
            Dim tc As New DotNetNuke.Entities.Tabs.TabController

            Dim redirectToFirstTimeTabInfo As DotNetNuke.Entities.Tabs.TabInfo = tc.GetTab(hidRedirectToFirstTime.Value, DNN.GetPMB(Me).PortalId, True)

            Dim redirectToTabInfo As DotNetNuke.Entities.Tabs.TabInfo = tc.GetTab(hidRedirectTo.Value, DNN.GetPMB(Me).PortalId, True)

            If redirectToFirstTimeTabInfo IsNot Nothing Then

                lblRedirectToFirstTime.Text = redirectToFirstTimeTabInfo.TabName

            Else

                lblRedirectToFirstTime.Text = ""

            End If

            If redirectToTabInfo IsNot Nothing Then

                lblRedirectTo.Text = redirectToTabInfo.TabName

            Else

                lblRedirectTo.Text = ""

            End If



            Dim ri As DotNetNuke.Security.Roles.RoleInfo = rc.GetRole(nRoleId, DNN.GetPMB(Me).PortalId)

            If ri IsNot Nothing Then
                lblRoleName.Text = ri.RoleName
            Else
                lblRoleName.Text = String.Empty
            End If



        End If

    End Sub

    Protected Sub rgFFUser_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgFFUser.ItemCommand

        If e.CommandName = "PerformInsert" Then
            Insert(e)
        ElseIf e.CommandName = "Update" Then
            Update(e)
        ElseIf e.CommandName = "Delete" Then
            Delete(e)
        End If


    End Sub
    Protected Sub Delete(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        Dim hidFFUserID As HiddenField = e.Item.FindControl("hidFFUserID")
        Dim nID As Int64 = hidFFUserID.Value
        ffUser = dbContext.GetObjectByKey(New ObjectKey(ffUser.GetType().Name, nID))
        dbContext.Delete(ffUser)
        dbContext.SaveChanges()


    End Sub

    Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If TypeOf e.Item Is GridEditFormInsertItem And e.Item.IsInEditMode Then


            Dim rblUserType As RadioButtonList = e.Item.FindControl("rblUserType")
            Dim rcbUser As RadComboBox = e.Item.FindControl("rcbUser")
            Dim rcbRole As RadComboBox = e.Item.FindControl("rcbRole")
            Dim rcbRedirectToFirstTime As RadComboBox = e.Item.FindControl("rcbRedirectToFirstTime")
            Dim rcbRedirectTo As RadComboBox = e.Item.FindControl("rcbRedirectTo")
            Dim chkIsFirstTime As CheckBox = e.Item.FindControl("chkIsFirstTime")
            Dim rcbParentTabs As RadComboBox = e.Item.FindControl("rcbParentTabs")

            If rcbRedirectToFirstTime.SelectedValue = "-1" Then

                ffUser.RedirectToFirstTime = -1
                'rcbRedirectToFirstTime.SelectedValue = -1

            Else

                ffUser.RedirectToFirstTime = Convert.ToInt64(rcbRedirectToFirstTime.SelectedValue)



            End If

            If rcbRedirectTo.SelectedValue = "-1" Then

                ffUser.RedirectTo = -1

                'rcbRedirectTo.SelectedValue = -1

            Else

                ffUser.RedirectTo = Convert.ToInt64(rcbRedirectTo.SelectedValue)

                'ffUser.RedirectToFirstTime = Convert.ToInt64(rcbRedirectToFirstTime.SelectedValue)

            End If


            'ffUser.RedirectTo = IIf(rcbRedirectTo.SelectedValue = "-1", -1, Convert.ToInt64(rcbRedirectTo.SelectedValue))
            'ffUser.RedirectToFirstTime = IIf(rcbRedirectToFirstTime.SelectedValue = "-1", -1, Convert.ToInt64(rcbRedirectToFirstTime.SelectedValue))
            'ffUser.IsFirstTime = chkIsFirstTime.Checked

            If rblUserType.SelectedValue = "0" Then
                ffUser.UserName = rcbUser.SelectedItem.Text
                ffUser.IsADUser = False
            ElseIf rblUserType.SelectedValue = "1" Then
                ffUser.UserName = FF_GLOBALS.PS_USER_NAME_PREFIX + rcbUser.SelectedItem.Text
                ffUser.IsADUser = True
            End If


            'ffUser.UserName = FF_GLOBALS.PS_USER_NAME_PREFIX + rcbUser.SelectedItem.Text
            ffUser.RoleId = Convert.ToInt64(rcbRole.SelectedValue)
            ffUser.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffUser.CreatedOn = DateTime.Now
            ffUser.ParentTabId = rcbParentTabs.SelectedValue
            dbContext.Add(ffUser)
            dbContext.SaveChanges()

        End If


    End Sub

    Protected Sub Update(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

            Dim rblUserType As RadioButtonList = e.Item.FindControl("rblUserType")
            Dim rcbUser As RadComboBox = e.Item.FindControl("rcbUser")
            Dim rcbRole As RadComboBox = e.Item.FindControl("rcbRole")
            Dim rcbRedirectToFirstTime As RadComboBox = e.Item.FindControl("rcbRedirectToFirstTime")
            Dim rcbRedirectTo As RadComboBox = e.Item.FindControl("rcbRedirectTo")
            Dim chkIsFirstTime As CheckBox = e.Item.FindControl("chkIsFirstTime")
            Dim rcbParentTabs As RadComboBox = e.Item.FindControl("rcbParentTabs")


            Dim hidFFUserID As HiddenField = e.Item.FindControl("hidFFUserID")

            Dim nID As Int64 = hidFFUserID.Value

            ffUser = dbContext.GetObjectByKey(New ObjectKey(ffUser.GetType().Name, nID))
            'ffUser.RedirectTo = IIf(rcbRedirectTo.SelectedValue = "-1", -1, Convert.ToInt64(rcbRedirectTo.SelectedValue))
            'ffUser.RedirectToFirstTime = IIf(rcbRedirectToFirstTime.SelectedValue = "-1", -1, Convert.ToInt64(rcbRedirectToFirstTime.SelectedValue))

            If rcbRedirectToFirstTime.SelectedValue = "-1" Then

                ffUser.RedirectToFirstTime = -1
                'rcbRedirectToFirstTime.SelectedValue = -1

            Else

                ffUser.RedirectToFirstTime = Convert.ToInt64(rcbRedirectToFirstTime.SelectedValue)



            End If

            If rcbRedirectTo.SelectedValue = "-1" Then

                ffUser.RedirectTo = -1

                'rcbRedirectTo.SelectedValue = -1

            Else

                ffUser.RedirectTo = Convert.ToInt64(rcbRedirectTo.SelectedValue)

                'ffUser.RedirectToFirstTime = Convert.ToInt64(rcbRedirectToFirstTime.SelectedValue)

            End If


            'ffUser.IsFirstTime = chkIsFirstTime.Checked
            ffUser.ParentTabId = rcbParentTabs.SelectedValue

            If rblUserType.SelectedValue = "0" Then
                ffUser.UserName = rcbUser.SelectedItem.Text
            Else
                ffUser.UserName = IIf(rcbUser.SelectedItem.Text.Contains("SPRINTPDC"), rcbUser.SelectedItem.Text, FF_GLOBALS.PS_USER_NAME_PREFIX + rcbUser.SelectedItem.Text)
            End If

            ffUser.RoleId = Convert.ToInt64(rcbRole.SelectedValue)
            ffUser.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffUser.CreatedOn = DateTime.Now
            dbContext.SaveChanges()


        End If
    End Sub


End Class