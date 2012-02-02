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

    Partial Public Class AdminShipMethod
        Inherits BaseAdminModule

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    populateList()
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            Try
                UpdateList()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNew.Click
            Try
                Dim objCtrl As New ShipController
                Dim objInfo As New NB_Store_ShippingMethodInfo

                objInfo.PortalID = PortalId
                objInfo.ShipMethodID = -1
                objInfo.MethodDesc = ""
                objInfo.MethodName = ""
                objInfo.SortOrder = 1
                objInfo.TemplateName = ""
                objInfo.URLtracker = ""

                objCtrl.UpdateObjShippingMethod(objInfo)

                populateList()

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub populateList()
            Dim objCtrl As New ShipController

            DotNetNuke.Services.Localization.Localization.LocalizeDataGrid(dgList, LocalResourceFile)

            ' get content
            Dim aryList As ArrayList

            aryList = objCtrl.GetShippingMethodList(PortalId)

            dgList.DataSource = aryList
            dgList.DataBind()
        End Sub

        Private Sub UpdateList()
            If Page.IsValid = True Then

                Dim objRow As DataGridItem
                Dim objCtrl As New ShipController
                Dim objInfo As NB_Store_ShippingMethodInfo

                For Each objRow In dgList.Items
                    objInfo = objCtrl.GetShippingMethod(CInt(objRow.Cells(1).Text))
                    If Not objInfo Is Nothing Then

                        objInfo.MethodName = CType(objRow.FindControl("txtMethodName"), TextBox).Text
                        objInfo.MethodDesc = CType(objRow.FindControl("txtMethodDesc"), TextBox).Text
                        objInfo.TemplateName = CType(objRow.FindControl("ddlTemplateName"), DropDownList).SelectedValue
                        objInfo.URLtracker = CType(objRow.FindControl("ddlTrackerTemplate"), DropDownList).SelectedValue
                        objInfo.Disabled = CType(objRow.FindControl("chkDisabled"), CheckBox).Checked
                        objInfo.SortOrder = CType(objRow.FindControl("txtSortOrder"), TextBox).Text
                        objInfo.PortalID = PortalId
                        objCtrl.UpdateObjShippingMethod(objInfo)

                    End If
                Next

                populateList()

            End If
        End Sub

        Private Sub dgList_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgList.DeleteCommand
            Dim item As DataGridItem = e.Item
            Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
            Dim objCtrl As New ShipController
            Dim objMInfo As NB_Store_ShippingMethodInfo
            objMInfo = objCtrl.GetShippingMethod(ItemId)
            If Not objMInfo Is Nothing Then
                If objMInfo.Disabled Then
                    'restore deleted
                    objCtrl.DeleteShippingMethod(objMInfo.ShipMethodID)
                End If
            End If

            populateList()

        End Sub

        Private Sub dgList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgList.ItemCommand
            If e.CommandName = "Copy" Then
                Dim item As DataGridItem = e.Item
                Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
                Dim objCtrl As New ShipController
                objCtrl.CopyShippingMethod(ItemId)
                populateList()
            End If
        End Sub

        Private Sub dgList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgList.ItemDataBound
            Dim item As DataGridItem = e.Item
            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                Dim imgColumnControl As Control = item.Controls(0).Controls(0)
                If TypeOf imgColumnControl Is ImageButton Then
                    Dim remImage As ImageButton = CType(imgColumnControl, ImageButton)
                    If item.ItemIndex = 0 Or Not CBool(DataBinder.Eval(item.DataItem, "Disabled")) Then
                        remImage.Visible = False
                        If item.ItemIndex = 0 Then CType(item.FindControl("chkDisabled"), CheckBox).Visible = False
                    Else
                        remImage.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdDeleteMsg", LocalResourceFile) & "');")
                        remImage.ToolTip = Localization.GetString("cmdDeleteTip", LocalResourceFile)
                    End If
                End If

                Dim dll As DropDownList = item.FindControl("ddlTemplateName")
                If Not dll Is Nothing Then
                    populateTemplateList(PortalId, dll, ".shiptemplate", "NONE", Localization.GetString("None", LocalResourceFile), DataBinder.Eval(item.DataItem, "TemplateName"))
                End If

                Dim imgColumnControl2 As Control = item.Controls(8).Controls(0)
                If TypeOf imgColumnControl2 Is ImageButton Then
                    Dim remImage2 As ImageButton = CType(imgColumnControl2, ImageButton)
                    remImage2.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdCopyMsg", LocalResourceFile) & "');")
                    remImage2.ToolTip = Localization.GetString("cmdCopyTip", LocalResourceFile)
                End If

                Dim dllT As DropDownList = item.FindControl("ddlTrackerTemplate")
                If Not dllT Is Nothing Then
                    populateTemplateList(PortalId, dllT, ".tracktemplate", "NONE", Localization.GetString("None", LocalResourceFile), DataBinder.Eval(item.DataItem, "URLTracker"))
                End If

            End If


        End Sub
    End Class

End Namespace
