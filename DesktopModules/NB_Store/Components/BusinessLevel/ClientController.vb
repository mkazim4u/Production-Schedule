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


Imports System
Imports System.Configuration
Imports System.Data
Imports System.Xml
Imports System.Web
Imports System.Collections.Generic

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class ClientController


        Public Function GetUsers(ByVal PortalID As Integer, ByVal Filter As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_GetUsers(PortalID, Filter), GetType(NB_StoreUsersInfo))
        End Function

        Public Sub ResetPassword(ByVal PortalID As Integer, ByVal UserId As Integer)
            Dim objUserInfo As Users.UserInfo
            objUserInfo = Users.UserController.GetUserById(PortalID, UserId)
            If Not objUserInfo Is Nothing Then
                Users.UserController.ResetPassword(objUserInfo, "")
                'Send Notification to User
                DotNetNuke.Services.Mail.Mail.SendMail(objUserInfo, DotNetNuke.Services.Mail.MessageType.PasswordReminder, CType(HttpContext.Current.Items("PortalSettings"), DotNetNuke.Entities.Portals.PortalSettings))
            End If
        End Sub

        Public Sub updateEmail(ByVal PortalID As Integer, ByVal UserID As Integer, ByVal Email As String)
            Dim objUserInfo As Users.UserInfo

            objUserInfo = Users.UserController.GetUserById(PortalID, UserID)
            If Not objUserInfo Is Nothing Then
                If objUserInfo.Email <> Email Then
                    objUserInfo.Email = Email
                    Users.UserController.UpdateUser(PortalID, objUserInfo)
                End If
            End If

        End Sub

        Public Sub UnlockUser(ByVal Portalid As Integer, ByVal UserId As Integer)
            Dim objUserInfo As Users.UserInfo

            objUserInfo = Users.UserController.GetUserById(Portalid, UserId)
            If Not objUserInfo Is Nothing Then
                Users.UserController.UnLockUser(objUserInfo)
            End If

        End Sub

        Public Sub LockUser(ByVal Portalid As Integer, ByVal UserId As Integer)
            Dim objUserInfo As Users.UserInfo

            objUserInfo = Users.UserController.GetUserById(Portalid, UserId)
            If Not objUserInfo Is Nothing Then
                objUserInfo.Membership.Approved = False
                Users.UserController.UpdateUser(Portalid, objUserInfo)
            End If

        End Sub

    End Class

End Namespace
