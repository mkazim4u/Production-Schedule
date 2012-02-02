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



    Partial Public Class AdminPromo
        Inherits BaseAdminModule

        Private _CodeType As String


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                'Sample code to get data
                If Not Request.QueryString("spg") Is Nothing Then
                    Select Case Request.QueryString("spg").ToUpper
                        Case "COU"
                            _CodeType = Request.QueryString("spg").ToUpper
                        Case "CAR"
                            _CodeType = Request.QueryString("spg").ToUpper
                        Case "STO"
                            _CodeType = Request.QueryString("spg").ToUpper
                    End Select


                    If Not Page.IsPostBack Then
                        cmdDelete.Visible = False
                        Select Case _CodeType.ToUpper
                            Case "COU"
                                pnlProduct.Visible = True
                            Case "CAR"
                                pnlProduct.Visible = False
                            Case "STO"
                                pnlProduct.Visible = False
                            Case Else
                                pnlEdit.Visible = False
                        End Select

                        Dim chk As String = GetStoreSetting(PortalId, "allowmultidiscount.flag", "None")
                        If chk = "" Then chk = "false"
                        chkAllowMultiDiscount.Checked = CBool(chk)

                        populateGV()

                    End If
                End If


            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNew.Click
            AddNewRecord(-1, "")
            Response.Redirect(Request.Url.ToString)
        End Sub

        Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
            populateGV()
        End Sub

        Private Sub chkAllowMultiDiscount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAllowMultiDiscount.CheckedChanged
            SetStoreSetting(PortalId, "allowmultidiscount.flag", chkAllowMultiDiscount.Checked.ToString, "None", False)
        End Sub


#Region "Procedures"

        Private Sub AddNewRecord(ByVal ObjectID As Integer, ByVal Description As String)
            Dim objCtrl As New PromoController
            Dim objInfo As New NB_Store_PromoInfo

            objInfo.PromoID = 0
            objInfo.ObjectID = ObjectID
            objInfo.Range1 = 0
            objInfo.Range2 = 0
            objInfo.PromoName = Localization.GetString("New_Promotion", LocalResourceFile)
            objInfo.PromoType = _CodeType.ToUpper
            objInfo.Disabled = False
            objInfo.PromoAmount = 0
            objInfo.PromoGroup = ""
            objInfo.PromoPercent = 0
            objInfo.PortalID = PortalId
            objInfo.RangeEndDate = DateAdd(DateInterval.Year, 10, Today).ToString
            objInfo.RangeStartDate = Today.ToString
            objInfo.PromoCode = ""
            objInfo.QtyRange1 = 0
            objInfo.QtyRange2 = 0
            objInfo.MaxUsagePerUser = 0
            objCtrl.UpdateObjPromo(objInfo)
            UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
        End Sub

#End Region

#Region "gvDiscount Methods - Events"

#Region "gvDiscount Events"

        Private Sub gvPromo_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvPromo.RowCancelingEdit
            gvPromo.EditIndex = -1
            populateGV()
        End Sub

        Private Sub gvDiscount_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvPromo.RowEditing
            gvPromo.EditIndex = e.NewEditIndex
            populateGV()
            setupFields()
        End Sub

        Private Sub gvDiscount_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPromo.RowDataBound
            Dim row As GridViewRow = e.Row
            If row.RowType = DataControlRowType.DataRow Then

                Dim cmd As LinkButton
                cmd = e.Row.FindControl("cmdDelete")
                cmd.Text = "<img src=""" + StoreInstallPath + "img/delete.png"" border=""0"" />"
                cmd.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdDelete", LocalResourceFile) & "');")

                If row.RowIndex = gvPromo.EditIndex Then

                    Dim hyp As HyperLink
                    Dim txt As TextBox

                    hyp = e.Row.FindControl("hypDateRange1")
                    If Not hyp Is Nothing Then
                        txt = e.Row.FindControl("txtDateRange1")
                        If Not txt Is Nothing Then
                            hyp.Text = "<img src=""" + DotNetNuke.Common.Globals.ApplicationPath + "/images/calendar.png"" border=""0"" />"
                            hyp.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txt)
                        End If
                    End If

                    hyp = e.Row.FindControl("hypDateRange2")
                    If Not hyp Is Nothing Then
                        txt = e.Row.FindControl("txtDateRange2")
                        If Not txt Is Nothing Then
                            hyp.Text = "<img src=""" + DotNetNuke.Common.Globals.ApplicationPath + "/images/calendar.png"" border=""0"" />"
                            hyp.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txt)
                        End If
                    End If

                    Dim ddl As DropDownList
                    ddl = e.Row.FindControl("ddlCategories")
                    If Not ddl Is Nothing Then
                        populateCategoryList(PortalId, ddl, -1, "All Categories", CType(row.DataItem, NB_Store_PromoInfo).ObjectID.ToString)
                    End If

                    Dim ddl2 As DropDownList
                    ddl2 = e.Row.FindControl("ddlPromoGroup")
                    If Not ddl2 Is Nothing Then
                        populateRoleList(ddl2, CType(row.DataItem, NB_Store_PromoInfo).PromoGroup)
                    End If

                End If

            End If

        End Sub

        Private Sub populateRoleList(ByVal ddl As DropDownList, ByVal SelectedRole As String)
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim PromoRoles As String()
            Dim rtnPrice As Double = -1
            Dim strList As String = ""

            ddl.Items.Clear()
            objSInfo = objSCtrl.GetSetting(PortalId, "promo.roles", "None")
            If Not objSInfo Is Nothing Then
                strList = "," & objSInfo.SettingValue
                PromoRoles = strList.Split(","c)
                For lp As Integer = 0 To PromoRoles.GetUpperBound(0)
                    ddl.Items.Add(PromoRoles(lp))
                Next
                If ddl.Items.Count = 1 Then
                    ddl.Enabled = False
                End If
            Else
                ddl.Enabled = False
            End If

            If Not ddl.Items.FindByValue(SelectedRole) Is Nothing Then
                ddl.ClearSelection()
                ddl.Items.FindByValue(SelectedRole).Selected = True
            End If

        End Sub

        Private Sub gvDiscount_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvPromo.RowUpdating
            If Page.IsValid Then
                Dim row As GridViewRow = gvPromo.Rows(e.RowIndex)
                Dim SDate As Date = CDate(getGridViewValueDate(row, "txtDateRange1"))
                Dim EDate As Date = CDate(getGridViewValueDate(row, "txtDateRange2"))
                updateGVrow(e)
                gvPromo.EditIndex = -1
                populateGV()
                If _CodeType.ToUpper = "STO" Then
                    'recalulate salepricetable if active today
                    If SDate <= Today And EDate >= Today Then
                        Dim objCtrl As New PromoController
                        objCtrl.createSalePriceTable(PortalId)
                    End If
                End If
            End If
        End Sub


        Private Sub gvPromo_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvPromo.RowDeleting
            Dim row As GridViewRow = gvPromo.Rows(e.RowIndex)
            Dim SDate As Date = CDate(getGridViewValueDate(row, "txtDateRange1"))
            Dim EDate As Date = CDate(getGridViewValueDate(row, "txtDateRange2"))
            deleteGVrow(e)
            gvPromo.EditIndex = -1
            populateGV()
            If _CodeType.ToUpper = "STO" Then
                'recalulate salepricetable if active today
                If SDate <= Today And EDate >= Today Then
                    Dim objCtrl As New PromoController
                    objCtrl.createSalePriceTable(PortalId)
                End If
            End If
        End Sub



#End Region

#Region "gvDiscount Methods"

        Private Sub populateGV()
            Dim objCtrl As New PromoController
            Dim list As ArrayList

            DotNetNuke.Services.Localization.Localization.LocalizeGridView(gvPromo, LocalResourceFile)

            list = objCtrl.GetPromoList(PortalId, _CodeType.ToUpper, txtSearch.Text)

            gvPromo.DataSource = list
            gvPromo.DataBind()

        End Sub


        Private Sub updateGVrow(ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)
            Dim objCtrl As New PromoController
            Dim objInfo As New NB_Store_PromoInfo
            Dim row As GridViewRow = gvPromo.Rows(e.RowIndex)

            objInfo.Disabled = Not (getGridViewValueBool(row, "chkDisable"))
            objInfo.PromoAmount = getGridViewValueDbl(row, "txtAmount")
            objInfo.PromoCode = getGridViewValue(row, "txtPromoCode")
            objInfo.PromoGroup = getGridViewValue(row, "ddlPromoGroup")
            objInfo.PromoUser = getGridViewValue(row, "txtPromoUser")
            objInfo.PromoID = getGridViewValueInt(row, "PromoID")
            objInfo.PromoName = getGridViewValue(row, "txtName")
            objInfo.PromoPercent = getGridViewValueDbl(row, "txtPercent")
            objInfo.PromoType = getGridViewValue(row, "PromoType")
            objInfo.ObjectID = getGridViewValueInt(row, "ObjectID")
            objInfo.PortalID = PortalId
            objInfo.Range1 = getGridViewValueDbl(row, "txtRange1")
            objInfo.Range2 = getGridViewValueDbl(row, "txtRange2")
            objInfo.RangeEndDate = getGridViewValueDate(row, "txtDateRange2")
            objInfo.RangeStartDate = getGridViewValueDate(row, "txtDateRange1")
            objInfo.QtyRange1 = getGridViewValueInt(row, "txtQtyRange1")
            objInfo.QtyRange2 = getGridViewValueInt(row, "txtQtyRange2")
            objInfo.PromoEmail = getGridViewValue(row, "txtPromoEmail")
            objInfo.ObjectID = getGridViewValueInt(row, "ddlCategories")
            objInfo.MaxUsagePerUser = getGridViewValueInt(row, "txtMaxUsagePerUser")
            objInfo.MaxUsage = getGridViewValueInt(row, "txtMaxUsage")

            objCtrl.UpdateObjPromo(objInfo)
            UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))

        End Sub

        Private Sub deleteGVrow(ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
            Dim objCtrl As New PromoController
            Dim objInfo As New NB_Store_PromoInfo
            Dim row As GridViewRow = gvPromo.Rows(e.RowIndex)
            objCtrl.DeletePromo(getGridViewValueInt(row, "PromoID"))
            UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
        End Sub

        Private Sub setupFields()
            Dim row As GridViewRow = gvPromo.Rows(gvPromo.EditIndex)
            Dim strList As String = ""

            Select Case _CodeType.ToUpper
                Case "COU"
                    'strList = "plCategories,"
                    'strList &= "ddlCategories"
                Case "CAR"
                    strList = "plPromoCode,"
                    strList &= "plCategories,"
                    strList &= "txtPromoCode,"
                    strList &= "ddlCategories,"
                    'hide MaxUsagePerUser controls for category discounts
                    strList &= "plMaxUsagePerUser,"
                    strList &= "txtMaxUsagePerUser,"
                    strList &= "plMaxUsage,"
                    strList &= "txtMaxUsage"
                Case "STO"
                    strList = "plPromoCode,"
                    strList &= "lblQtySep,"
                    strList &= "lblPriceSep,"
                    strList &= "plPromoUser,"
                    strList &= "plPromoCode,"
                    strList &= "plPromoUser,"
                    strList &= "plQtyRange,"
                    strList &= "plRange,"
                    strList &= "plPromoEmail,"
                    strList &= "txtPromoCode,"
                    strList &= "txtPromoUser,"
                    strList &= "txtPromoCode,"
                    strList &= "txtPromoUser,"
                    strList &= "txtQtyRange1,"
                    strList &= "txtQtyRange2,"
                    strList &= "txtRange1,"
                    strList &= "txtRange2,"
                    strList &= "txtPromoEmail,"
                    'hide MaxUsagePerUser controls for cart discounts
                    strList &= "plMaxUsagePerUser,"
                    strList &= "txtMaxUsagePerUser,"
                    strList &= "plMaxUsage,"
                    strList &= "txtMaxUsage"
                Case Else
                    pnlEdit.Visible = False
            End Select

            If strList <> "" Then
                setGridViewVisible(row, strList, False)
            End If


        End Sub

#End Region


#End Region


    End Class

End Namespace
