' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. BSD License.
' Author: D.C.Lee
' ------------------------------------------------------------------------
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' ------------------------------------------------------------------------
' This copyright notice may NOT be removed, obscured or modified without written consent from the author.
' --- End copyright notice --- 


Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class AdminClients
        Inherits BaseAdminModule

        Private _UsrID As Integer

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                If Not (Request.QueryString("uid") Is Nothing) Then
                    If IsNumeric(Request.QueryString("uid")) Then
                        _UsrID = CInt(Request.QueryString("uid"))
                        pnlList.Visible = False
                        pnlEdit.Visible = True
                        populateEdit()
                    End If
                Else
                    pnlList.Visible = True
                    pnlEdit.Visible = False
                End If
            End If

            'always assign values to sub controls
            Clients1.PortalID = PortalId
            Clients1.ResourceFile = LocalResourceFile

        End Sub

        Private Sub Clients1_SelectedUser(ByVal UserID As Integer) Handles Clients1.SelectedUser
            Response.Redirect(EditUrl("uid", UserID.ToString, "AdminClients"))
        End Sub

        Private Sub populateEdit()
            Try

                Dim objUsrInfo As Users.UserInfo

                objUsrInfo = Users.UserController.GetUser(PortalId, _UsrID, False)
                If objUsrInfo Is Nothing Then
                    pnlEdit.Visible = False
                Else
                    If objUsrInfo.IsInRole("Administrators") Or objUsrInfo.IsSuperUser Then
                        pnlEdit.Visible = False
                    Else
                        cmdResetPass.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdResetPassMsg", LocalResourceFile) & "');")
                        cmdUnlock.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdUnlockMsg", LocalResourceFile) & "');")
                        lblViewDisplayName.Text = objUsrInfo.DisplayName
                        lblViewFirstName.Text = objUsrInfo.FirstName
                        lblViewLastName.Text = objUsrInfo.LastName
                        lblViewUserID.Text = _UsrID
                        lblViewUserName.Text = objUsrInfo.Username
                        txtEmail.Text = objUsrInfo.Email
                    End If
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            Try
                Dim objCtrl As New ClientController
                If Regex.IsMatch(txtEmail.Text, "[^""\r\n]*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") Then
                    objCtrl.updateEmail(PortalId, CInt(lblViewUserID.Text), txtEmail.Text)
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdResetPass_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdResetPass.Click
            Try
                Dim objCtrl As New ClientController
                objCtrl.ResetPassword(PortalId, CInt(lblViewUserID.Text))
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdReturn2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn2.Click
            Response.Redirect(EditUrl("AdminClients"))
        End Sub

        Private Sub cmdUnlock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUnlock.Click
            Try
                Dim objCtrl As New ClientController
                objCtrl.UnlockUser(PortalId, CInt(lblViewUserID.Text))
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdViewOrders_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdViewOrders.Click
            Response.Redirect(EditUrl("uid", Request.QueryString("uid"), "AdminOrders", "rtnctl=" & "AdminClients"))
        End Sub

        Private Sub cmdCreateOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCreateOrder.Click
            Dim objOCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo

            If IsNumeric(Request.QueryString("uid")) Then
                objOInfo = objOCtrl.CreateEmptyOrder(PortalId, CInt(Request.QueryString("uid")))
                Response.Redirect(EditUrl("OrdId", objOInfo.OrderID.ToString, "AdminOrders", "uid=" & Request.QueryString("uid"), "ed=1"))
            End If

        End Sub

    End Class

End Namespace
