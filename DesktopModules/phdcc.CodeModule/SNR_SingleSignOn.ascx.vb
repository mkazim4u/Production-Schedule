Imports DotNetNuke
Imports DotNetNuke.Entities
Partial Class SNR_SingleSignOn
    Inherits System.Web.UI.UserControl
    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private uc As New UserController

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub
   
 

    Protected Sub Login_Authenticate(sender As Object, e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login.Authenticate

        If Membership.ValidateUser(Login.UserName.Trim, Login.Password.Trim) Then

            e.Authenticated = True
            Login.Visible = False
            FormsAuthentication.SetAuthCookie(Login.UserName.Trim, False)
            FormsAuthentication.RedirectFromLoginPage(Login.UserName.Trim, True)
            Dim ri As DotNetNuke.Security.Roles.RoleInfo = rc.GetRoleByName(DNN.GetPMB(Me).PortalId, "NBStore_User")
            Dim ui As UserInfo = UserController.GetUserById(DNN.GetPMB(Me).PortalId, DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID)
            Dim status As MembershipCreateStatus = UserController.CreateUser(ui)
            If status Then

                rc.AddUserRole(DNN.GetPMB(Me).PortalId, DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID, ri.RoleID, Nothing)

            End If

        Else

            Login.Visible = True
            e.Authenticated = False


        End If
    End Sub

End Class

'Dim MyCookie As System.Web.HttpCookie = FormsAuthentication.GetAuthCookie(Login.UserName.Trim, False)

'MyCookie.

'MyCookie.Domain = "sprintexpress.co.uk"
'Response.AppendCookie(MyCookie)

'Protected Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)

'    If Membership.ValidateUser(UserName.Text.Trim, Password.Text.Trim) Then

'        'Dim fat As New FormsAuthenticationTicket(1, UserName.Text.Trim, DateTime.Now, DateTime.Now.AddYears(1), True, "")
'        'Dim cookie As HttpCookie = New HttpCookie(".SingleSignOn")
'        'cookie.Value = FormsAuthentication.Encrypt(fat)
'        'cookie.Expires = fat.Expiration
'        'HttpContext.Current.Response.Cookies.Add(cookie)
'        FormsAuthentication.SetAuthCookie(UserName.Text, False)
'        Dim MyCookie As System.Web.HttpCookie = FormsAuthentication.GetAuthCookie(UserName.Text.Trim, False)
'        MyCookie.Domain = "dnn563"
'        'Dim cookie As HttpCookie = FormsAuthentication.GetAuthCookie(UserName.Text, True)
'        ' Expires in 30 days, 12 hours and 30 minutes from today.
'        'cookie.Expires = DateTime.Now.Add(New TimeSpan(30, 12, 30, 0))
'        'Response.Cookies.Add(cookie)
'        InvalidCredentialsMessage.Visible = False
'        'Response.Redirect(FormsAuthentication.GetRedirectUrl(UserName.Text, chkPersistCookie.Checked))

'        '            FormsAuthentication.RedirectFromLoginPage(UserName.Text, True)

'    Else

'        InvalidCredentialsMessage.Visible = True


'    End If

'End Sub

