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

    Partial Public Class Clients
        Inherits Framework.UserControlBase

        Private _Filter As String = ""
        Private _PortalID As Integer
        Private _ResourceFile As String
        Public Event SelectedUser(ByVal UserID As Integer)

#Region "Public Properties"

        Public Property ResourceFile() As String
            Get
                Return _ResourceFile
            End Get
            Set(ByVal Value As String)
                _ResourceFile = Value
            End Set
        End Property

        Public Property PortalID() As String
            Get
                Return _PortalID
            End Get
            Set(ByVal Value As String)
                _PortalID = Value
            End Set
        End Property

#End Region


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                populateUserList()
            End If

        End Sub


        Private Sub populateUserList()
            Dim objCtrl As New ClientController
            Dim aryList As ArrayList

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgUserList, _ResourceFile)

            aryList = objCtrl.GetUsers(_PortalID, txtSearch.Text)

            dgUserList.DataSource = aryList
            dgUserList.DataBind()

        End Sub

        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            dgUserList.CurrentPageIndex = 0
            populateUserList()
        End Sub

        Private Sub dgUserList_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgUserList.EditCommand
            Dim UsrId As Integer = Int32.Parse(e.CommandArgument.ToString)
            RaiseEvent SelectedUser(UsrId)
        End Sub

        Private Sub dgUserList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgUserList.PageIndexChanged
            dgUserList.CurrentPageIndex = e.NewPageIndex
            populateUserList()
        End Sub
    End Class

End Namespace
