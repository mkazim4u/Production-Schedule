
Partial Class SNR_Registration
    Inherits System.Web.UI.UserControl

    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private uc As New UserController
    Private Const sDeputyEmail As String = "mkazim4u@hotmail.com"
    Private Const sAdminEmail As String = "mkazim4u@hotmail.com"
    Private dbContext As New SNRDentonDBLayerDataContext

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then


        End If

    End Sub

    Protected Sub btnRegister_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRegister.Click

        Dim userInfo As New UserInfo

        userInfo.Username = txtUserName.Text.Trim
        userInfo.FirstName = txtFirstName.Text.Trim
        userInfo.LastName = txtLastName.Text.Trim
        userInfo.Email = txtEmailAddress.Text.Trim
        userInfo.DisplayName = txtDisplayName.Text.Trim
        userInfo.PortalID = DNN.GetPMB(Me).PortalId
        userInfo.IsSuperUser = False
        userInfo.Profile.InitialiseProfile(DNN.GetPMB(Me).PortalId)
        userInfo.Membership.Password = txtPassword.Text.Trim
        userInfo.Membership.UpdatePassword = True
        userInfo.Membership.Approved = False

        Dim status As DotNetNuke.Security.Membership.UserCreateStatus = UserController.CreateUser(userInfo)

        Select Case status
            Case DotNetNuke.Security.Membership.UserCreateStatus.Success
                lblAccountCreated.Text = "User Name : " & userInfo.Username & ", <br>" & userInfo.FirstName & " your account has successfully created, awaiting authorization."
                lblAccountCreated.Visible = True
                tblRegister.Visible = False
                SendEmailNotification(userInfo)
                Exit Select
            Case DotNetNuke.Security.Membership.UserCreateStatus.DuplicateUserName
                lblCreateAccountResults.ForeColor = Color.Red
                lblCreateAccountResults.Text = "There already exists a user with this username."
                Exit Select
            Case DotNetNuke.Security.Membership.UserCreateStatus.DuplicateEmail
                lblCreateAccountResults.ForeColor = Color.Red
                lblCreateAccountResults.Text = "There already exists a user with this email address."
                Exit Select
            Case DotNetNuke.Security.Membership.UserCreateStatus.InvalidEmail
                lblCreateAccountResults.ForeColor = Color.Red
                lblCreateAccountResults.Text = "There email address you provided in invalid."
                Exit Select
            Case DotNetNuke.Security.Membership.UserCreateStatus.InvalidAnswer
                lblCreateAccountResults.ForeColor = Color.Red
                lblCreateAccountResults.Text = "There security answer was invalid."
                Exit Select
            Case DotNetNuke.Security.Membership.UserCreateStatus.InvalidPassword
                lblCreateAccountResults.ForeColor = Color.Red
                lblCreateAccountResults.Text = "The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character."
            Case Else
                lblCreateAccountResults.ForeColor = Color.Red
                lblCreateAccountResults.Text = "There was an unknown error; the user account was NOT created."
                Exit Select

        End Select


    End Sub

    Private Sub SendEmailNotification(ByVal user As UserInfo)

        Dim pc As New PortalController
        Dim sAdminEmail As String = String.Empty

        Dim pi As PortalInfo = pc.GetPortal(DNN.GetPMB(Me).PortalId)

        If pi.PortalName = FF_GLOBALS.UK_PORTAL Then

            sAdminEmail = "Email_Admin_UK"

        ElseIf pi.PortalName = FF_GLOBALS.USA_PORTAL Then

            sAdminEmail = "Email_Admin_US"

        ElseIf pi.PortalName = FF_GLOBALS.ME_PORTAL Then

            sAdminEmail = "Email_Admin_ME"

        Else

            sAdminEmail = "Email_Admin_UK"

        End If

        Dim config = From con In dbContext.SNR_Configurations
                     Where con.ConfigKey = ""

        Dim sEmailBody As New StringBuilder

        sEmailBody.Append("This is to inform you that " & user.FirstName & " " & user.LastName & " created an account on SNR Denton ")
        sEmailBody.Append(DNN.GetPMB(Me).PortalAlias.ToString + " with user name (<b>" + user.Username + "</b>).<br>")
        sEmailBody.Append("<br>")
        sEmailBody.Append("")

        DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, sAdminEmail, "SNR-Denton - New User Registration", sEmailBody.ToString)

    End Sub

End Class
