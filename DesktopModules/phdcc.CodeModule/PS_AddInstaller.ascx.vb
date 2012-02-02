Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.IO

Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Entities.Content.Taxonomy
Imports DotNetNuke.Security.Permissions
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Modules.Definitions
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Web.UI.WebControls

Partial Class PS_AddInstaller
    Inherits System.Web.UI.UserControl
    Private newTab As New DotNetNuke.Entities.Tabs.TabInfo
    Private match As System.Predicate(Of DotNetNuke.Entities.Content.Taxonomy.Term)


#Region "Enums"

    Private Enum ViewPermissionType
        View = 0
        Edit = 1
    End Enum

#End Region

#Region "Property"
    Property ModuleID() As Integer
        Get
            Dim o As Object = ViewState("ModuleID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("ModuleID") = Value
        End Set
    End Property

    Property ControlName() As String
        Get
            Dim o As Object = ViewState("ControlName")
            If o Is Nothing Then
                Return "create"
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("ControlName") = Value
        End Set
    End Property

    Property TermID() As Integer
        Get
            Dim o As Object = ViewState("TermID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("TermID") = Value
        End Set
    End Property

    Private _Terms As List(Of Term)


#End Region

    Private ReadOnly Property Terms() As List(Of Term)
        Get
            If _Terms Is Nothing Then
                Dim termRep As ITermController = DotNetNuke.Entities.Content.Common.Util.GetTermController()
                Dim vocabRep As IVocabularyController = DotNetNuke.Entities.Content.Common.Util.GetVocabularyController()
                _Terms = New List(Of Term)
                Dim vocabularies As IQueryable(Of Vocabulary) = From v In vocabRep.GetVocabularies() _
                                                                Where v.ScopeType.ScopeType = "Application" _
                                                                OrElse (v.ScopeType.ScopeType = "Portal" _
                                                                        AndAlso v.ScopeId = DNN.GetPMB(Me).PortalId)

                For Each v As Vocabulary In vocabularies
                    'Add a dummy parent term if simple vocabulary
                    If v.Type = VocabularyType.Simple Then
                        Dim dummyTerm As New Term(v.VocabularyId)
                        dummyTerm.ParentTermId = Nothing
                        dummyTerm.Name = v.Name
                        dummyTerm.TermId = -v.VocabularyId
                        If dummyTerm.Name = "PS" Then
                            _Terms.Add(dummyTerm)
                            Return _Terms
                            Exit Property
                        End If


                    End If
                    For Each t As Term In termRep.GetTermsByVocabulary(v.VocabularyId)
                        If v.Type = VocabularyType.Simple Then
                            t.ParentTermId = -v.VocabularyId
                        End If
                        If t.Name = "PS" Then
                            _Terms.Add(t)
                            Return _Terms
                            Exit Property
                        End If

                    Next
                Next
            End If
            Return _Terms
        End Get
    End Property


#Region "Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If (Not IsPostBack) Then
                PermGrid.TabID = -1
                'ParentsDropDownList.DataSource = TabController.GetPortalTabs(DNN.GetPMB(Me).PortalId, -1, True, False)
                'ParentsDropDownList.DataBind()
            End If

        Catch ex As Exception
            'failure
            Exceptions.ProcessModuleLoadException(Me, ex)
        End Try
    End Sub

    Private Sub CreatePage(ByVal PageName As String, ByVal PageTitle As String, ByVal Description As String, ByVal Keywords As String, ByVal parentId As Integer, ByVal control As String, ByVal Permissions As TabPermissionCollection, Optional ByVal LoadDefaultModules As Boolean = True)

        Try

            Dim controller As New TabController
            Dim newTab As New DotNetNuke.Entities.Tabs.TabInfo
            Dim newPermissions As TabPermissionCollection = newTab.TabPermissions
            Dim permissionProvider As PermissionProvider = permissionProvider.Instance
            Dim infPermission As TabPermissionInfo

            ' set new page properties
            newTab.PortalID = DNN.GetPMB(Me).PortalId
            newTab.TabName = PageName
            newTab.Title = PageTitle
            newTab.Description = Description
            newTab.KeyWords = Keywords
            newTab.IsDeleted = False
            newTab.IsSuperTab = False
            newTab.IsVisible = True
            newTab.DisableLink = False
            newTab.IconFile = ""
            newTab.Url = ""
            newTab.ParentId = parentId
            newTab.Terms.AddRange(Terms)
            controller.AddTab(newTab)
            Me.ControlName = control


            ' copy permissions selected in Permissions collection
            For index As Integer = 0 To (Permissions.Count - 1)
                infPermission = New TabPermissionInfo

                infPermission.AllowAccess = Permissions(index).AllowAccess
                infPermission.RoleID = Permissions(index).RoleID
                infPermission.RoleName = Permissions(index).RoleName
                infPermission.TabID = Permissions(index).TabID
                infPermission.PermissionID = Permissions(index).PermissionID

                'save permission info
                newPermissions.Add(infPermission, True)
                permissionProvider.SaveTabPermissions(newTab)
            Next index

            'create module on page
            'CreateModule(newTab.TabID, "MyHTMLModule", "ContentPane", "Text/HTML")
            CreateModule(newTab.TabID, "ContentPane")

            ' clear the cache
            DataCache.ClearModuleCache(newTab.TabID)

        Catch ex As Exception
            WebMsgBox.Show(ex.Message.ToString() & newTab.TabName & " " & newTab.TabID)
        End Try



    End Sub

    Protected Sub btnAddPage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddPages.Click

        AddingPages()

    End Sub

    Protected Sub AddingPages()

        Try
            Dim controller As New TabController
            Dim newTab As New DotNetNuke.Entities.Tabs.TabInfo

            'CreatePage(Trim(txtTabName.Text), Trim(txtTabTitle.Text), Trim(txtTabDesc.Text), Trim(txtTabKeyWords.Text), PermGrid.Permissions)
            lblMessage.Text = "Creating Pages ..."

            CreatePage(FF_GLOBALS.PAGE_CREATE_MAIN_MENU, FF_GLOBALS.PAGE_CREATE_MAIN_MENU, FF_GLOBALS.PAGE_CREATE_MAIN_MENU, FF_GLOBALS.PAGE_CREATE_MAIN_MENU, -1, "", PermGrid.Permissions)

            newTab = controller.GetTabByName(FF_GLOBALS.PAGE_CREATE_MAIN_MENU, DNN.GetPMB(Me).PortalId)

            CreatePage(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, FF_GLOBALS.PAGE_CREATE_EDIT_JOB, FF_GLOBALS.PAGE_CREATE_EDIT_JOB, FF_GLOBALS.PAGE_CREATE_EDIT_JOB, newTab.TabID, "PS_CreateEditJob.ascx", PermGrid.Permissions)
            CreatePage(FF_GLOBALS.PAGE_PRODUCTION_SCHEDULE_LIST, FF_GLOBALS.PAGE_PRODUCTION_SCHEDULE_LIST, FF_GLOBALS.PAGE_PRODUCTION_SCHEDULE_LIST, FF_GLOBALS.PAGE_PRODUCTION_SCHEDULE_LIST, newTab.TabID, "PS_ProductionScheduleMain.ascx", PermGrid.Permissions)
            CreatePage(FF_GLOBALS.PAGE_EDIT_JOB_STATES, FF_GLOBALS.PAGE_EDIT_JOB_STATES, FF_GLOBALS.PAGE_EDIT_JOB_STATES, FF_GLOBALS.PAGE_EDIT_JOB_STATES, newTab.TabID, "PS_CreateEditJobStates.ascx", PermGrid.Permissions)
            CreatePage(FF_GLOBALS.PAGE_CREATE_EDIT_CUSTOMER, FF_GLOBALS.PAGE_CREATE_EDIT_CUSTOMER, FF_GLOBALS.PAGE_CREATE_EDIT_CUSTOMER, FF_GLOBALS.PAGE_CREATE_EDIT_CUSTOMER, newTab.TabID, "PS_CreateEditCustomer.ascx", PermGrid.Permissions)
            CreatePage(FF_GLOBALS.PAGE_NEW_JOB_FROM_CLIPBOARD, FF_GLOBALS.PAGE_NEW_JOB_FROM_CLIPBOARD, FF_GLOBALS.PAGE_NEW_JOB_FROM_CLIPBOARD, FF_GLOBALS.PAGE_NEW_JOB_FROM_CLIPBOARD, newTab.TabID, "PS_ClipboardNewJobFrom.ascx", PermGrid.Permissions)
            CreatePage(FF_GLOBALS.PAGE_OPTIONS, FF_GLOBALS.PAGE_OPTIONS, FF_GLOBALS.PAGE_OPTIONS, FF_GLOBALS.PAGE_OPTIONS, newTab.TabID, "PS_MoreOptions.ascx", PermGrid.Permissions)
            CreatePage(FF_GLOBALS.PAGE_ADD_RESOURCES, FF_GLOBALS.PAGE_ADD_RESOURCES, FF_GLOBALS.PAGE_ADD_RESOURCES, FF_GLOBALS.PAGE_ADD_RESOURCES, newTab.TabID, "PS_UserJobMapping.ascx", PermGrid.Permissions)

            lblMessage.Text = "Pages Created ..."

        Catch ex As Exception
            WebMsgBox.Show(ex.Message.ToString() & newTab.TabID & " " & newTab.TabName & "AddingPages Method Error !!!")
        End Try


    End Sub

    Protected Sub btnDeletePages_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeletePages.Click

        Dim controller As New TabController
        Dim tabCollection As TabCollection = controller.GetTabsByPortal(DNN.GetPMB(Me).PortalId)
        'Dim moduleController As ModuleController

        Dim tPageTag As New DotNetNuke.Entities.Content.Taxonomy.Term
        Dim alTabCollection As ArrayList = tabCollection.ToArrayList()

        If tabCollection IsNot Nothing Then
            For Each tab As TabInfo In alTabCollection
                tPageTag = tab.Terms.Find(AddressOf IsFound)
                If tPageTag IsNot Nothing Then
                    'temporary delete
                    'tab.IsDeleted = True
                    'controller.UpdateTab(tab)
                    ' permanent delete
                    controller.DeleteTab(tab.TabID, DNN.GetPMB(Me).PortalId, True)


                End If
            Next
            NavigateTo("Home")
        Else
            WebMsgBox.Show("No Tabs Exists Containing PS Tags")
        End If



    End Sub

    Protected Function IsFound(ByVal term As DotNetNuke.Entities.Content.Taxonomy.Term) As Boolean
        Dim found As Boolean = False
        For Each psTag As DotNetNuke.Entities.Content.Taxonomy.Term In Terms
            If psTag.Name = term.Name Then
                found = True
            End If
        Next

        Return found
    End Function

#End Region



#Region "DNN Page Creation"



    Public Sub CreateModule(ByVal TabID As Integer, ByVal paneName As String)

        Try


            Dim desktopInfo As DotNetNuke.Entities.Modules.DesktopModuleInfo = DesktopModuleController.GetDesktopModuleByModuleName("phdcc.CodeModule", DNN.GetPMB(Me).PortalId)
            Dim modDefInfo As ModuleDefinitionInfo = ModuleDefinitionController.GetModuleDefinitionByFriendlyName(desktopInfo.FriendlyName)


            AddNewModule(TabID, "phdcc.CodeModule", desktopInfo.DesktopModuleID, paneName, 0, ViewPermissionType.View, "")

            'Configure Module Settings
            ConfigModuleSettings(modDefInfo.ModuleDefID)
        Catch ex As Exception
            ProcessModuleLoadException(Me, ex)
            WebMsgBox.Show(ex.Message.ToString() & "Create Module Error !!!")
        End Try

    End Sub
    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub

    Private Sub ConfigModuleSettings(ByVal moduleDefID As Integer)
        Try

            Dim objModules As ModuleController = New ModuleController()
            Dim TabModuleID As Integer = objModules.GetModule(ModuleID).TabModuleID

            Dim TabSettingControl As String = objModules.GetTabModuleSettings(TabModuleID)("control")
            If Not TabSettingControl Is Nothing Then
                objModules.DeleteTabModuleSetting(TabModuleID, "control")
                objModules.UpdateModuleSetting(ModuleID, "control", TabSettingControl)
            End If

            Dim CodeModuleDirectory As String = HttpContext.Current.Server.MapPath("~/DesktopModules/phdcc.CodeModule/")
            If File.Exists(CodeModuleDirectory + Me.ControlName) Then
                'ViewControl.Controls.Add(LoadControl("PS_UserJobMapping.ascx"))
                objModules.DeleteTabModuleSetting(TabModuleID, Me.ControlName)                   ' 01.01.00
                objModules.UpdateModuleSetting(ModuleID, "control", Me.ControlName)    ' 01.01.00
                ModuleController.SynchronizeModule(ModuleID)
                'NavigateTo(txtTabName.Text)
            Else
                'lblError.InnerText = "Control file does not exist: " + CodeModuleDirectory + "PS_UserJobMapping.ascx"
                'lblError.Visible = True
            End If

        Catch exc As Exception           'Module failed to load
            WebMsgBox.Show(exc.Message.ToString() & "Config Module Settings Error !!!")
            ProcessModuleLoadException(Me, exc)

        End Try


    End Sub

#Region "From DNN Source --mostly"

    ''' -----------------------------------------------------------------------------
    ''' <summary>Adds a New Module to a Pane</summary>
    ''' <param name="align">The alignment for the Module</param>
    ''' <param name="desktopModuleId">The Id of the DesktopModule</param>
    ''' <param name="permissionType">The View Permission Type for the Module</param>
    ''' <param name="title">The Title for the resulting module</param>
    ''' <param name="paneName">The pane to add the module to</param>
    ''' <param name="position">The relative position within the pane for the module</param>
    ''' -----------------------------------------------------------------------------
    Private Sub AddNewModule(ByVal TabID As Integer, ByVal title As String, ByVal desktopModuleId As Integer, ByVal paneName As String, ByVal position As Integer, ByVal permissionType As ViewPermissionType, ByVal align As String)

        Try

            Dim objTabController As New TabController
            Dim objTabPermissions As TabPermissionCollection = objTabController.GetTab(TabID, DNN.GetPMB(Me).PortalId, True).TabPermissions
            Dim objPermissionController As New PermissionController
            Dim objModules As New ModuleController
            Dim objModuleDefinition As ModuleDefinitionInfo
            Dim objEventLog As New DotNetNuke.Services.Log.EventLog.EventLogController
            Dim j As Integer

            Try
                Dim desktopModule As DesktopModuleInfo = Nothing
                If Not DesktopModuleController.GetDesktopModules(DNN.GetPMB(Me).PortalId).TryGetValue(desktopModuleId, desktopModule) Then
                    Throw New ArgumentException("desktopModuleId")
                End If
            Catch ex As Exception
                WebMsgBox.Show(ex.Message.ToString() & "AddNewModule Error !!!")
                LogException(ex)
            End Try

            Dim UserId As Integer = -1
            If Request.IsAuthenticated Then
                Dim objUserInfo As DotNetNuke.Entities.Users.UserInfo = UserController.GetCurrentUserInfo
                UserId = objUserInfo.UserID
            End If

            For Each objModuleDefinition In ModuleDefinitionController.GetModuleDefinitionsByDesktopModuleID(desktopModuleId).Values
                Dim objModule As New ModuleInfo
                objModule.Initialize(DNN.GetPMB(Me).PortalId)

                objModule.PortalID = DNN.GetPMB(Me).PortalId
                objModule.TabID = TabID
                objModule.ModuleOrder = position
                If title = "" Then
                    objModule.ModuleTitle = objModuleDefinition.FriendlyName
                Else
                    objModule.ModuleTitle = title
                End If
                objModule.PaneName = paneName
                objModule.ModuleDefID = objModuleDefinition.ModuleDefID
                If objModuleDefinition.DefaultCacheTime > 0 Then
                    objModule.CacheTime = objModuleDefinition.DefaultCacheTime
                    If DotNetNuke.Entities.Portals.PortalSettings.Current.DefaultModuleId > Null.NullInteger AndAlso DotNetNuke.Entities.Portals.PortalSettings.Current.DefaultTabId > Null.NullInteger Then
                        Dim defaultModule As ModuleInfo = objModules.GetModule(DotNetNuke.Entities.Portals.PortalSettings.Current.DefaultModuleId, DotNetNuke.Entities.Portals.PortalSettings.Current.DefaultTabId, True)
                        If Not defaultModule Is Nothing Then
                            objModule.CacheTime = defaultModule.CacheTime
                        End If
                    End If
                End If

                Select Case permissionType
                    Case ViewPermissionType.View
                        objModule.InheritViewPermissions = True
                    Case ViewPermissionType.Edit
                        objModule.InheritViewPermissions = False
                End Select

                ' get the default module view permissions
                Dim arrSystemModuleViewPermissions As ArrayList = objPermissionController.GetPermissionByCodeAndKey("SYSTEM_MODULE_DEFINITION", "VIEW")

                ' get the permissions from the page
                For Each objTabPermission As TabPermissionInfo In objTabPermissions
                    If objTabPermission.PermissionKey = "VIEW" AndAlso permissionType = ViewPermissionType.View Then
                        'Don't need to explicitly add View permisisons if "Same As Page"
                        Continue For
                    End If

                    ' get the system module permissions for the permissionkey
                    Dim arrSystemModulePermissions As ArrayList = objPermissionController.GetPermissionByCodeAndKey("SYSTEM_MODULE_DEFINITION", objTabPermission.PermissionKey)
                    ' loop through the system module permissions
                    For j = 0 To arrSystemModulePermissions.Count - 1
                        ' create the module permission
                        Dim objSystemModulePermission As PermissionInfo
                        objSystemModulePermission = CType(arrSystemModulePermissions(j), PermissionInfo)
                        If objSystemModulePermission.PermissionKey = "VIEW" AndAlso permissionType = ViewPermissionType.Edit AndAlso _
                             objTabPermission.PermissionKey <> "EDIT" Then
                            'Only Page Editors get View permissions if "Page Editors Only"
                            Continue For
                        End If

                        Dim objModulePermission As ModulePermissionInfo = AddModulePermission(objModule, _
                                                                                objSystemModulePermission, _
                                                                                objTabPermission.RoleID, objTabPermission.UserID, _
                                                                                objTabPermission.AllowAccess)

                        ' ensure that every EDIT permission which allows access also provides VIEW permission
                        If objModulePermission.PermissionKey = "EDIT" And objModulePermission.AllowAccess Then
                            Dim objModuleViewperm As ModulePermissionInfo = AddModulePermission(objModule, _
                                                                                CType(arrSystemModuleViewPermissions(0), PermissionInfo), _
                                                                                objModulePermission.RoleID, objModulePermission.UserID, _
                                                                                True)
                        End If
                    Next

                    'Get the custom Module Permissions,  Assume that roles with Edit Tab Permissions
                    'are automatically assigned to the Custom Module Permissions
                    If objTabPermission.PermissionKey = "EDIT" Then
                        Dim arrCustomModulePermissions As ArrayList = objPermissionController.GetPermissionsByModuleDefID(objModule.ModuleDefID)

                        ' loop through the custom module permissions
                        For j = 0 To arrCustomModulePermissions.Count - 1
                            ' create the module permission
                            Dim objCustomModulePermission As PermissionInfo
                            objCustomModulePermission = CType(arrCustomModulePermissions(j), PermissionInfo)

                            AddModulePermission(objModule, objCustomModulePermission, _
                                                                    objTabPermission.RoleID, objTabPermission.UserID, _
                                                                    objTabPermission.AllowAccess)
                        Next
                    End If
                Next

                objModule.AllTabs = False
                objModule.Alignment = align

                ModuleID = objModules.AddModule(objModule)
            Next

        Catch ex As Exception
            ProcessModuleLoadException(Me, ex)
            WebMsgBox.Show(ex.Message.ToString() & "AddNewModule Error !!!")

        End Try


    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>Adds a Module Permission</summary>
    ''' <param name="permission">The permission to add</param>
    ''' <param name="roleId">The Id of the role to add the permission for.</param>
    ''' -----------------------------------------------------------------------------


    Private Function AddModulePermission(ByVal objModule As ModuleInfo, ByVal permission As PermissionInfo, ByVal roleId As Integer, ByVal userId As Integer, ByVal allowAccess As Boolean) As ModulePermissionInfo

        Try

            Dim objModulePermission As New ModulePermissionInfo
            objModulePermission.ModuleID = objModule.ModuleID
            objModulePermission.PermissionID = permission.PermissionID
            objModulePermission.RoleID = roleId
            objModulePermission.UserID = userId
            objModulePermission.PermissionKey = permission.PermissionKey
            objModulePermission.AllowAccess = allowAccess

            ' add the permission to the collection
            If Not objModule.ModulePermissions.Contains(objModulePermission) Then
                objModule.ModulePermissions.Add(objModulePermission)
            End If

            Return objModulePermission

        Catch ex As Exception
            ProcessModuleLoadException(Me, ex)
            WebMsgBox.Show(ex.Message.ToString() & "AddModulePermission Error !!!")
            Return Nothing
        End Try
    End Function

#End Region

#End Region

End Class



