Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports System.Linq
Imports System.Reflection
Imports Telerik.Web.UI

Partial Class SNR_UserAuthorization
    Inherits System.Web.UI.UserControl

    Private dbContext As New SNRDentonDBLayerDataContext
    Private pc As New PortalController

    Protected Sub rgUsers_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgUsers.NeedDataSource

        Dim usersList As ArrayList = DotNetNuke.Entities.Users.UserController.GetUsers(DNN.GetPMB(Me).PortalId)

        Dim Users = (From u As UserInfo In usersList Where u.Username <> "admin" Order By u.Membership.CreatedDate Descending).ToList

        rgUsers.DataSource = Users

    End Sub

    Protected Sub chkAuthorized_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkAuthorized As CheckBox = sender

        Dim hidUserID As HiddenField = chkAuthorized.NamingContainer.FindControl("hidUserID")

        Dim nUserID As Integer = hidUserID.Value

        Dim ui As UserInfo

        If chkAuthorized.Checked Then

            ui = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserID)

            ui.Membership.Approved = True

            UserController.UpdateUser(DNN.GetPMB(Me).PortalId, ui)

            Dim alPortalList As ArrayList = pc.GetPortals()

            Dim IPortalList = (From p As PortalInfo In alPortalList Where p.PortalID <> DNN.GetPMB(Me).PortalId)

            For Each p As PortalInfo In IPortalList

                Dim up As New UserPortal

                up.UserId = ui.UserID
                up.PortalId = p.PortalID
                up.Authorised = True
                up.CreatedDate = DateTime.Now
                up.IsDeleted = False
                dbContext.UserPortals.InsertOnSubmit(up)
                dbContext.SubmitChanges()
                SendEmailNotification(ui, True)


            Next


        Else

            ui = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserID)

            ui.Membership.Approved = False

            UserController.UpdateUser(DNN.GetPMB(Me).PortalId, ui)

            SendEmailNotification(ui, False)

        End If



    End Sub

    Private Sub SendEmailNotification(ByVal user As UserInfo, ByVal bAuthorize As Boolean)

        Dim sEmailBody As New StringBuilder

        sEmailBody.Append("Dear " + user.FirstName + ",")
        sEmailBody.Append("<br>")
        If bAuthorize Then
            sEmailBody.Append("This is to inform you that an administrator has authorized your account for online shoppping.")
        Else
            sEmailBody.Append("This is to inform you that an administrator has unauthorized your account for online shoppping.")
        End If
        sEmailBody.Append("<br>")
        sEmailBody.Append("Thanks.")
        sEmailBody.Append("<br>")
        sEmailBody.Append("<br>")
        sEmailBody.Append("Regards")
        sEmailBody.Append("<br>")
        sEmailBody.Append("SNR-Denton")

        DotNetNuke.Services.Mail.Mail.SendEmail(FF_GLOBALS.PS_EMAIL_NOTIFIER, user.Email, "SNR-Denton - Authorization", sEmailBody.ToString)

    End Sub



End Class
