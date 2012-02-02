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

    Partial Public Class AdminProductDetail
        Inherits Framework.UserControlBase

#Region "Private Members"
        Private ResourceFile As String
        Private _PortalID As Integer
#End Region

        Public Event HideUpdate()

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim objSTCtrl As New SettingsController
            Dim objSTInfo As NB_Store_SettingsTextInfo

            objSTInfo = objSTCtrl.GetSettingsText(CType(Me.Parent.Parent.Parent, BaseAdminModule).PortalId, "productxmldata.template", GetCurrentCulture)
            If Not objSTInfo Is Nothing Then
                If objSTInfo.SettingText <> "" Then
                    dlXMLData.ItemTemplate = New GenXMLTemplate(Server.HtmlDecode(objSTInfo.SettingText))
                Else
                    dlXMLData.Visible = False
                End If
            Else
                dlXMLData.Visible = False
            End If

        End Sub


        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ResourceFile = Services.Localization.Localization.GetResourceFile(Me, Me.GetType().BaseType.Name + ".ascx")

            Dim ProdID As Integer = -1
            If Not (Request.QueryString("ProdID") Is Nothing) Then
                If IsNumeric(Request.QueryString("ProdID")) Then
                    ProdID = CInt(Request.QueryString("ProdID"))
                End If
            End If


        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            ShowSelectLang.Refresh()
            ShowSelectLang1.Refresh()
            ShowSelectLang2.Refresh()
            ShowSelectLang3.Refresh()
            ShowSelectLang5.Refresh()
            ShowSelectLang4.Refresh()
        End Sub


        Public Sub populateDetails(ByVal objInfo As NB_Store_ProductsInfo)

            If Not objInfo Is Nothing Then

                populateCategoryList(PortalSettings.PortalId, cmbCategory, "-1", " ", objInfo.TaxCategoryID.ToString)
                CType(txtDescription, DotNetNuke.UI.UserControls.TextEditor).Text = objInfo.Description
                txtManufacturer.Text = objInfo.Manufacturer
                txtSummary.Text = objInfo.Summary
                txtProductName.Text = objInfo.ProductName
                txtSEOName.Text = objInfo.SEOName
                txtProductRef.Text = objInfo.ProductRef
                chkArchived.Checked = objInfo.Archived
                chkFeatured.Checked = objInfo.Featured
                chkDeleted.Checked = objInfo.IsDeleted
                chkIsHidden.Checked = objInfo.IsHidden
                txtTagWords.Text = objInfo.TagWords

                If dlXMLData.Visible Then
                    DisplayXMLData(objInfo, dlXMLData)
                End If

            Else
                populateCategoryList(PortalSettings.PortalId, cmbCategory)
            End If

        End Sub

        Public Function UpdateDetails(ByVal Lang As String, ByVal UsrId As Integer, ByVal ProdID As Integer) As NB_Store_ProductsInfo
            Dim objCtrl As New ProductController
            Dim objInfo As New NB_Store_ProductsInfo

            If ProdID > 0 Then
                objInfo = objCtrl.GetProduct(ProdID, Lang)
            End If
            If objInfo Is Nothing Then
                objInfo = New NB_Store_ProductsInfo
            End If

            objInfo.Archived = chkArchived.Checked
            If IsNumeric(cmbCategory.SelectedValue) Then
                objInfo.TaxCategoryID = CInt(cmbCategory.SelectedValue)
            Else
                objInfo.TaxCategoryID = -1
            End If
            objInfo.CreatedByUser = UsrId
            If objInfo.CreatedDate = Null.NullDate Then
                objInfo.CreatedDate = Now
            End If
            objInfo.Description = CType(txtDescription, DotNetNuke.UI.UserControls.TextEditor).Text
            objInfo.Featured = chkFeatured.Checked
            objInfo.IsDeleted = chkDeleted.Checked
            objInfo.IsHidden = chkIsHidden.Checked
            objInfo.Lang = Lang
            objInfo.Manufacturer = txtManufacturer.Text
            objInfo.PortalID = PortalSettings.PortalId
            objInfo.ProductID = ProdID
            objInfo.Summary = txtSummary.Text
            objInfo.ProductName = txtProductName.Text
            objInfo.SEOName = txtSEOName.Text
            objInfo.ProductRef = txtProductRef.Text
            If dlXMLData.Visible Then
                objInfo.XMLData = getGenXML(dlXMLData)
            Else
                objInfo.XMLData = ""
            End If
            objInfo.TagWords = txtTagWords.Text

            objInfo = objCtrl.UpdateObjProduct(objInfo)

            Return objInfo
        End Function





    End Class

End Namespace
