' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. BSD License.
' Author: D.C.Lee, Romain Doidy
' ------------------------------------------------------------------------
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' ------------------------------------------------------------------------
' This copyright notice may NOT be removed, obscured or modified without written consent from the author.
' --- End copyright notice --- 
'
'
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class AdminReportList
        Inherits Framework.UserControlBase

#Region "Private Members"
        Private ResourceFile As String
        Private _IsEditable As Boolean

        Public Property IsEdit() As Boolean
            Get
                Return _IsEditable
            End Get
            Set(ByVal value As Boolean)
                _IsEditable = value
            End Set
        End Property

#End Region

        Public Event AddButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event ReturnButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Public Event EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Public Event SearchButton(ByVal sender As Object, ByVal e As System.EventArgs)



        Public Sub populateList()
            Dim objCtrl As New SQLReportController

            If Not _IsEditable Then
                cmdAdd.Visible = False
            End If

            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")
            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgReports, ResourceFile)

            Dim aryList As ArrayList
            aryList = objCtrl.GetSQLAdminReportList(PortalSettings.PortalId, _IsEditable, txtSearch.Text)

            dgReports.DataSource = aryList
            dgReports.DataBind()

        End Sub

        'Private Sub dgReports_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReports.EditCommand
        '    RaiseEvent EditCommand(source, e)
        'End Sub

        Private Sub dgReports_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReports.ItemCommand
            Select Case e.CommandName
                Case "Copy"
                    Dim item As DataGridItem = e.Item
                    Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
                    Dim objCtrl As New SQLReportController
                    objCtrl.CopyReport(ItemId)
            End Select
            RaiseEvent ItemCommand(source, e)
        End Sub

        Private Sub dgReports_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReports.ItemDataBound
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim objRInfo As NB_Store_SQLReportInfo
                Dim imgColumnControl As Control
                objRInfo = e.Item.DataItem


                imgColumnControl = e.Item.Controls(0).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    If Not _IsEditable Then
                        remImage.Visible = False
                    End If
                End If

                imgColumnControl = e.Item.Controls(4).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdCopyReport", ResourceFile) & "');")
                    If Not _IsEditable Then
                        remImage.Visible = False
                    End If
                End If
                If Not objRInfo.AllowExport Then
                    imgColumnControl = e.Item.Controls(6).Controls(0)
                    If TypeOf imgColumnControl Is ImageButton Then
                        Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                        remImage.Visible = False
                    End If
                End If
                If Not objRInfo.AllowDisplay Then
                    imgColumnControl = e.Item.Controls(5).Controls(0)
                    If TypeOf imgColumnControl Is ImageButton Then
                        Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                        remImage.Visible = False
                    End If
                End If
                imgColumnControl = e.Item.Controls(7).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdEmailReport", ResourceFile) & "');")
                    If Not objRInfo.EmailResults Then
                        remImage.Visible = False
                    End If
                End If

            End If
        End Sub

        Private Sub dgProducts_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgReports.PageIndexChanged
            dgReports.CurrentPageIndex = e.NewPageIndex
            populateList()
        End Sub


        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            dgReports.CurrentPageIndex = 0
            RaiseEvent SearchButton(sender, e)
        End Sub

        Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
            RaiseEvent AddButton(sender, e)
        End Sub

    End Class

End Namespace
