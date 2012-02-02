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


Imports DotNetNuke.Common.Globals
Imports System.Xml
Imports System.Reflection
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class ProductTemplate
        Implements ITemplate
        Protected _aryTempl As String()
        Protected _TabID As Integer
        Protected _PageIndex As Integer
        Protected _StoreInstallPath As String
        Protected _ModuleID As String
        Protected _ThumbSize As String
        Protected _blnHasModuleEditPermissions As Boolean
        Protected _BuyText As String
        Protected _CssBuyButton As String
        Protected _CatID As Integer
        Protected _GalleryThumbSize As String
        Protected _EditCtrlKey As String
        Protected _ProductTabID As Integer
        Protected _UserID As Integer
        Protected _ZeroPriceMsg As String
        Protected _SoldOutImg As String
        Protected _LockStockOnCart As Boolean
        Protected _QtyLimit As Integer
        Protected _UserInfo As DotNetNuke.Entities.Users.UserInfo
        Protected _NestedLevel As ArrayList
        Protected _FoundEscapeChar As Boolean = False

        Sub New(ByVal TabID As Integer, ByVal ModuleID As Integer, ByVal StoreInstallPath As String, ByVal ThumbSize As String, ByVal TemplateText As String, ByVal blnHasModuleEditPermissions As Boolean, ByVal BuyText As String, ByVal CssBuyButton As String, ByVal PageIndex As Integer, ByVal CatID As Integer, ByVal GalleryThumbSize As String, ByVal EditCtrlKey As String, ByVal ProductTabID As Integer, ByVal UserID As Integer, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, Optional ByVal ZeroPriceMsg As String = "", Optional ByVal SoldOutImg As String = "", Optional ByVal LockStockOnCart As Boolean = False, Optional ByVal QtyLimit As Integer = 999999)

            'use double sqr brqckets as escape char.
            _FoundEscapeChar = False
            If InStr(TemplateText, "[[") > 0 Or InStr(TemplateText, "]]") > 0 Then
                TemplateText = Replace(TemplateText, "[[", "**SQROPEN**")
                TemplateText = Replace(TemplateText, "]]", "**SQRCLOSE**")
                _FoundEscapeChar = True
            End If


            _aryTempl = ParseTemplateText(TemplateText)
            _TabID = TabID
            _PageIndex = PageIndex
            _StoreInstallPath = StoreInstallPath
            _ThumbSize = ThumbSize
            _blnHasModuleEditPermissions = blnHasModuleEditPermissions
            _ModuleID = ModuleID
            _CatID = CatID
            _GalleryThumbSize = GalleryThumbSize
            If _GalleryThumbSize = "" Then _GalleryThumbSize = "100"
            If _ThumbSize = "" Then _ThumbSize = "150"
            _BuyText = BuyText
            _CssBuyButton = CssBuyButton
            If _CssBuyButton = "" Then _CssBuyButton = "CommandButton"
            _EditCtrlKey = EditCtrlKey
            _ProductTabID = ProductTabID
            _UserInfo = UserInfo
            If _UserInfo Is Nothing Then
                _UserID = -1
            Else
                _UserID = _UserInfo.UserID
            End If
            _ZeroPriceMsg = ZeroPriceMsg
            _SoldOutImg = SoldOutImg
            _LockStockOnCart = LockStockOnCart
            _QtyLimit = QtyLimit
            _NestedLevel = New ArrayList
            _NestedLevel.Add(True)
        End Sub

        Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            Dim lc As Literal
            Dim hyp As HyperLink
            Dim img As Image
            Dim ddl As DropDownList
            Dim rbl As RadioButtonList
            Dim cmd As LinkButton
            Dim cmdB As ImageButton
            Dim chk As CheckBox
            Dim txt As TextBox
            Dim val As RangeValidator
            Dim rval As RequiredFieldValidator
            Dim hf As HiddenField
            Dim fu As FileUpload
            Dim lp As Integer


            For lp = 0 To _aryTempl.GetUpperBound(0)
                Select Case _aryTempl(lp).ToUpper
                    Case "TAG:PRODUCTID"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf ProductID_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:PRODUCTNAME"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf ProductName_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:PRODUCTREF"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf ProductRef_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:MANUFACTURER"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf Manufacturer_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:SUMMARY"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf Summary_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:SUMMARYHTML"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf SummaryHTML_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:DESCRIPTION"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf Description_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:LINK"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf Link_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:LINKSIMPLE"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf LinkSimple_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:FROMPRICE"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf FromPrice_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:FROMPRICECURRENCY"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf FromPriceCurrency_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:DEALERPRICE"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf DealerPrice_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:DEALERPRICECURRENCY"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf DealerPriceCurrency_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:CURRENCYSYMBOL"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf CurrencySymbol_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:CURRENCYISOCODE"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf CurrencyISOcode_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:QTYREMAINING"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf QtyRemaining_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:STOCKPERCENT"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf StockPercent_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:STOCKPERCENTSOLD"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf StockPercentSold_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:STOCKPERCENTINPROCESS"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf StockPercentInProcess_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:STOCKPERCENTACTUAL"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf StockPercentActual_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:SALEPRICE"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf SalePrice_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:SALEPRICECURRENCY"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf SalePriceCurrency_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:BESTPRICE"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf BestPrice_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:BESTPRICECURRENCY"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf BestPriceCurrency_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:IMAGE"
                        img = New Image
                        AddHandler img.DataBinding, AddressOf Image_DataBinding
                        container.Controls.Add(img)
                    Case "TAG:IMAGEURL"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf ImageURL_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:CATIMAGEURL"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf CatImageURL_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:CATNAME"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf CatName_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:THUMBURL"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf ThumbURL_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:IMAGELIGHTBOX"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf ImageLB_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:THUMBSIZE"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf ThumbSize_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:STOREINSTALLPATH"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf StoreInstallPath_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:SOLDOUTIMG"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf SoldOutImg_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:EDITLINK"
                        hyp = New HyperLink
                        AddHandler hyp.DataBinding, AddressOf EditLink_DataBinding
                        container.Controls.Add(hyp)
                    Case "TAG:ADDTOBASKET"
                        cmd = New LinkButton
                        AddHandler cmd.DataBinding, AddressOf AddToBasket_DataBinding
                        container.Controls.Add(cmd)
                        hf = New HiddenField
                        hf.ID = "hfModel"
                        container.Controls.Add(hf)
                    Case "TAG:WEBSITEURL"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf WebsiteURL_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:END"
                        lc = New Literal
                        lc.Text = ""
                        AddHandler lc.DataBinding, AddressOf Visible_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:PRICEBEST"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf PriceBest_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:PRICESALE"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf PriceSale_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:PRICEDEALER"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf PriceDealer_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:PRICEBESTCURRENCY"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf PriceBestCurrency_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:PRICESALECURRENCY"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf PriceSaleCurrency_DataBinding
                        container.Controls.Add(lc)
                    Case "TAG:PRICEDEALERCURRENCY"
                        lc = New Literal
                        AddHandler lc.DataBinding, AddressOf PriceDealerCurrency_DataBinding
                        container.Controls.Add(lc)
                    Case Else
                        If _aryTempl(lp).ToUpper.StartsWith("TAG:UPLOAD") Then
                            fu = New FileUpload
                            fu = assignProperties(_aryTempl(lp), fu, "fu")
                            AddHandler fu.DataBinding, AddressOf TestCtrlDisplay_DataBinding
                            container.Controls.Add(fu)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:ADDTOBASKET") Then
                            Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                            cmd = New LinkButton
                            AddHandler cmd.DataBinding, AddressOf AddToBasket_DataBinding
                            container.Controls.Add(cmd)
                            hf = New HiddenField
                            hf.ID = "hfModel"
                            hf.Value = TagAtt(2)
                            container.Controls.Add(hf)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:PURCHASE") Then
                            cmd = New LinkButton
                            Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                            cmd.Text = TagAtt(2)
                            AddHandler cmd.DataBinding, AddressOf Purchase_DataBinding
                            container.Controls.Add(cmd)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:GATEWAYRADIO") Then
                            rbl = New RadioButtonList
                            rbl = assignProperties(_aryTempl(lp), rbl)
                            rbl.ID = "rblGateway"
                            AddHandler rbl.DataBinding, AddressOf GatewayRadio_DataBinding
                            container.Controls.Add(rbl)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:GATEWAY") Then
                            Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                            lc = New Literal
                            lc.Text = TagAtt(2)
                            AddHandler lc.DataBinding, AddressOf GatewayButton_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:DATABIND") Then
                            Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                            lc = New Literal
                            lc.Text = TagAtt(2)
                            AddHandler lc.DataBinding, AddressOf ClassDataBind_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:SETTING") Then
                            Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                            lc = New Literal
                            lc.Text = TagAtt(2)
                            AddHandler lc.DataBinding, AddressOf Setting_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:TEST") Then
                            Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                            lc = New Literal
                            lc.Text = TagAtt(2)
                            AddHandler lc.DataBinding, AddressOf TokenTest_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:ADDRELATED") Then
                            cmd = New LinkButton
                            AddHandler cmd.DataBinding, AddressOf AddToRelated_DataBinding
                            cmd = assignProperties(_aryTempl(lp), cmd)
                            container.Controls.Add(cmd)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:ADDTOWISHLIST") Then
                            cmd = New LinkButton
                            AddHandler cmd.DataBinding, AddressOf AddToWishList_DataBinding
                            cmd = assignProperties(_aryTempl(lp), cmd)
                            container.Controls.Add(cmd)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:REMOVEFROMWISHLIST") Then
                            cmd = New LinkButton
                            AddHandler cmd.DataBinding, AddressOf RemoveFromWishList_DataBinding
                            cmd = assignProperties(_aryTempl(lp), cmd)
                            container.Controls.Add(cmd)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:QTY") Then
                            If _QtyLimit <= 20 Then
                                ddl = New DropDownList
                                ddl = assignProperties(_aryTempl(lp), ddl, "ddl")
                                For lp2 As Integer = 1 To _QtyLimit
                                    ddl.Items.Add(lp2.ToString)
                                Next
                                AddHandler ddl.DataBinding, AddressOf QtyDDL_DataBinding
                                container.Controls.Add(ddl)
                            Else
                                txt = New TextBox
                                txt.MaxLength = 6
                                txt.Text = "1"
                                txt.Width = System.Web.UI.WebControls.Unit.Parse("32px")
                                txt = assignProperties(_aryTempl(lp), txt, "txt")
                                AddHandler txt.DataBinding, AddressOf QtyInput_DataBinding
                                container.Controls.Add(txt)
                                val = New RangeValidator
                                val.ErrorMessage = "*"
                                val.Text = ""
                                val.MaximumValue = _QtyLimit
                                val.MinimumValue = 1
                                val = assignProperties(_aryTempl(lp), val, "val")
                                val.ControlToValidate = "txtqty"
                                AddHandler val.DataBinding, AddressOf QtyVal_DataBinding
                                container.Controls.Add(val)
                                rval = New RequiredFieldValidator
                                rval.ErrorMessage = "*"
                                rval.Text = ""
                                rval = assignProperties(_aryTempl(lp), rval, "rval")
                                rval.ControlToValidate = "txtqty"
                                AddHandler rval.DataBinding, AddressOf QtyRVal_DataBinding
                                container.Controls.Add(rval)
                            End If
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:OPTION") Then
                            If _aryTempl(lp).ToUpper.StartsWith("TAG:OPTIONNAME") Then
                                'option name display as literal
                                lc = New Literal
                                lc.ID = Replace(_aryTempl(lp).ToLower, "tag:", "")
                                AddHandler lc.DataBinding, AddressOf OptionName_DataBinding
                                container.Controls.Add(lc)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:OPTIONRADIO") Then
                                rbl = New RadioButtonList
                                rbl = assignProperties(_aryTempl(lp), rbl)
                                AddHandler rbl.DataBinding, AddressOf OptionRadio_DataBinding
                                container.Controls.Add(rbl)
                                chk = New CheckBox
                                chk = assignProperties(_aryTempl(lp), chk, "chk")
                                chk.Visible = False
                                container.Controls.Add(chk)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:OPTIONTEXT") Then
                                txt = New TextBox
                                txt = assignProperties(_aryTempl(lp), txt)
                                AddHandler txt.DataBinding, AddressOf OptionText_DataBinding
                                container.Controls.Add(txt)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:OPTIONVALRANGE") Then
                                Dim FieldVal As New RangeValidator
                                FieldVal = assignProperties(_aryTempl(lp), FieldVal)
                                AddHandler FieldVal.DataBinding, AddressOf TestCtrlDisplay_DataBinding
                                container.Controls.Add(FieldVal)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:OPTIONVALREGX") Then
                                Dim FieldVal As New RegularExpressionValidator
                                FieldVal = assignProperties(_aryTempl(lp), FieldVal)
                                AddHandler FieldVal.DataBinding, AddressOf TestCtrlDisplay_DataBinding
                                container.Controls.Add(FieldVal)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:OPTIONVALREQ") Then
                                Dim FieldVal As New RequiredFieldValidator
                                FieldVal = assignProperties(_aryTempl(lp), FieldVal)
                                AddHandler FieldVal.DataBinding, AddressOf TestCtrlDisplay_DataBinding
                                container.Controls.Add(FieldVal)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:OPTIONVALSUMMARY") Then
                                Dim FieldVal As New ValidationSummary
                                FieldVal = assignProperties(_aryTempl(lp), FieldVal)
                                AddHandler FieldVal.DataBinding, AddressOf TestCtrlDisplay_DataBinding
                                container.Controls.Add(FieldVal)
                            Else
                                'option ddl & chk
                                ddl = New DropDownList
                                ddl = assignProperties(_aryTempl(lp), ddl)
                                AddHandler ddl.DataBinding, AddressOf Option_DataBinding
                                container.Controls.Add(ddl)
                                chk = New CheckBox
                                chk = assignProperties(_aryTempl(lp), chk, "chk")
                                chk.Visible = False
                                container.Controls.Add(chk)
                            End If
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:MODELS") Then
                            If _aryTempl(lp).ToUpper.StartsWith("TAG:MODELSRADIO2") Then
                                rbl = New RadioButtonList
                                rbl = assignProperties(_aryTempl(lp), rbl)
                                rbl.ID = "rblModelsel"
                                AddHandler rbl.DataBinding, AddressOf ModelRadio2_DataBinding
                                container.Controls.Add(rbl)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:MODELSRADIO") Then
                                rbl = New RadioButtonList
                                rbl = assignProperties(_aryTempl(lp), rbl)
                                rbl.ID = "rblModel"
                                AddHandler rbl.DataBinding, AddressOf ModelRadio_DataBinding
                                container.Controls.Add(rbl)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:MODELSTABLE") Then
                                lc = New Literal
                                AddHandler lc.DataBinding, AddressOf ModelTable_DataBinding
                                container.Controls.Add(lc)
                            ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:MODELS2") Then
                                ddl = New DropDownList
                                ddl = assignProperties(_aryTempl(lp), ddl)
                                ddl.ID = "ddlModelsel"
                                AddHandler ddl.DataBinding, AddressOf Model2_DataBinding
                                container.Controls.Add(ddl)
                            Else
                                ddl = New DropDownList
                                ddl = assignProperties(_aryTempl(lp), ddl)
                                ddl.ID = "ddlModel"
                                AddHandler ddl.DataBinding, AddressOf Model_DataBinding
                                container.Controls.Add(ddl)
                            End If
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:XMLDATA") Then
                            Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                            lc = New Literal
                            lc.Text = Replace(TagAtt(2).ToLower, "(", "[")
                            lc.Text = Replace(lc.Text, ")", "]")
                            AddHandler lc.DataBinding, AddressOf XMLData_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:GALLERYTHUMBURL") Then
                            lc = New Literal
                            lc.ID = Replace(_aryTempl(lp).ToLower, "tag:", "")
                            AddHandler lc.DataBinding, AddressOf GalleryThumbUrl_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:GALLERYURL") Then
                            lc = New Literal
                            lc.ID = Replace(_aryTempl(lp).ToLower, "tag:", "")
                            AddHandler lc.DataBinding, AddressOf GalleryUrl_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:GALLERYID") Then
                            lc = New Literal
                            lc.ID = Replace(_aryTempl(lp).ToLower, "tag:", "")
                            AddHandler lc.DataBinding, AddressOf GalleryId_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAGXML") Then
                            lc = New Literal
                            lc.Text = Replace(_aryTempl(lp).ToLower, "tagxml:", "")
                            AddHandler lc.DataBinding, AddressOf XML_DataBinding
                            container.Controls.Add(lc)
                        ElseIf _aryTempl(lp).ToUpper.StartsWith("TAGEXTRA") Then
                            lc = New Literal
                            lc.Text = Replace(_aryTempl(lp).ToLower, "tagextra:", "")
                            AddHandler lc.DataBinding, AddressOf EXTRA_DataBinding
                            container.Controls.Add(lc)
                        Else
                            If _aryTempl(lp).ToUpper.StartsWith("TAG:GALLERY") Then
                                lc = New Literal
                                lc.ID = Replace(_aryTempl(lp).ToLower, "tag:", "")
                                AddHandler lc.DataBinding, AddressOf Gallery_DataBinding
                                container.Controls.Add(lc)
                            Else
                                If _aryTempl(lp).ToUpper.StartsWith("TAG:DOC") Then
                                    If _aryTempl(lp).ToUpper.StartsWith("TAG:DOCLINK") Or _aryTempl(lp).StartsWith("TAG:DOCPURCHASEDLINK") Then
                                        cmd = New LinkButton
                                        cmd = assignProperties(_aryTempl(lp), cmd)
                                        cmd.CausesValidation = False
                                        AddHandler cmd.DataBinding, AddressOf DocLink_DataBinding
                                        container.Controls.Add(cmd)
                                    ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:DOCDESC") Then
                                        lc = New Literal
                                        lc = assignProperties(_aryTempl(lp), lc)
                                        AddHandler lc.DataBinding, AddressOf DocDesc_DataBinding
                                        container.Controls.Add(lc)
                                    Else
                                        cmdB = New ImageButton
                                        cmdB = assignProperties(_aryTempl(lp), cmdB)
                                        cmdB.CausesValidation = False
                                        AddHandler cmdB.DataBinding, AddressOf Doc_DataBinding
                                        container.Controls.Add(cmdB)
                                    End If
                                Else
                                    lc = New Literal
                                    lc.Text = _aryTempl(lp)
                                    AddHandler lc.DataBinding, AddressOf VisibleMode_DataBinding
                                    container.Controls.Add(lc)
                                End If
                            End If
                        End If
                End Select

                If _FoundEscapeChar Then
                    _aryTempl(lp) = Replace(_aryTempl(lp), "**SQROPEN**", "[")
                    _aryTempl(lp) = Replace(_aryTempl(lp), "**SQRCLOSE**", "]")
                End If

            Next

        End Sub

        Private Sub Visible_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            lc.Visible = False
            If _NestedLevel.Count > 0 Then
                _NestedLevel.RemoveAt(_NestedLevel.Count - 1)
            End If
        End Sub

        Private Sub VisibleMode_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub


        Private Sub XMLData_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            Dim strData As String = DataBinder.Eval(container.DataItem, "XMLData")
            If strData = "" Then
                lc.Text = ""
            Else
                If lc.Text.Contains("/edt/") Then
                    'Is editor so decode html
                    lc.Text = System.Web.HttpUtility.HtmlDecode(getGenXMLvalue(DataBinder.Eval(container.DataItem, "XMLData"), lc.Text))
                Else
                    lc.Text = getGenXMLvalue(DataBinder.Eval(container.DataItem, "XMLData"), lc.Text)
                End If
            End If
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub TokenTest_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            Dim objTokenTest As New TokenTest
            assignByReflection(objTokenTest, lc.Text)
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim VisibleMode As Boolean
            VisibleMode = objTokenTest.getVisibleMode(PS.PortalId, _ModuleID, container, _UserInfo, CBool(_NestedLevel((_NestedLevel.Count - 1))))
            lc.Visible = False ' never display this token
            _NestedLevel.Add(VisibleMode)
        End Sub

        Private Sub GatewayButton_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)

            Dim objGateway As New GatewayWrapper
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings

            lc.Text = objGateway.GetButtonHtml(PS.PortalId, CInt(DataBinder.Eval(container.DataItem, "OrderID")), GetCurrentCulture, lc.Text)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))

        End Sub

        Private Sub ClassDataBind_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = DataBinder.Eval(container.DataItem, lc.Text).ToString
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub


        Private Sub Setting_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            lc.Text = GetStoreSetting(PS.PortalId, lc.Text, GetCurrentCulture)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub EXTRA_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            If Not TagExtraInterface.Instance() Is Nothing Then
                Dim lc As Literal
                lc = CType(sender, Literal)
                Dim container As DataListItem
                container = CType(lc.NamingContainer, DataListItem)
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings

                lc.Text = TagExtraInterface.Instance.getExtraHtml(PS.PortalId, container, lc.Text)

                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))

            End If

        End Sub


        Private Sub XML_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim DebugMode As Boolean = False
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)


            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            DebugMode = GetStoreSettingBoolean(PS.PortalId, "debug.mode", "None")
            Dim strCacheKey As String = ""
            Dim strCacheData As String = ""
            Try
                Dim TagAtt As String() = lc.Text.Split(":"c)
                Dim XMLFunction As String = ""
                Dim XSLFName As String = ""

                XSLFName = TagAtt(0)
                If TagAtt.GetUpperBound(0) >= 1 Then
                    XMLFunction = TagAtt(1)
                End If

                Dim XSLFileName As String = PS.HomeDirectoryMapPath & TagAtt(0)
                Dim k As String = ""
                Dim ProdID As String = ""

                strCacheKey = "dummykey"
                If Not XMLFunction.ToLower = "getorderxml" Then

                    If Not DataBinder.Eval(container.DataItem, "ProductID") Is Nothing Then
                        ProdID = DataBinder.Eval(container.DataItem, "ProductID")
                    End If

                    If Not DataBinder.Eval(container.DataItem, "Lang") Is Nothing Then
                        k = System.IO.Path.GetFileName(XSLFileName) & "_" & DataBinder.Eval(container.DataItem, "Lang") & "_" & XMLFunction & "_" & _CatID
                        strCacheKey = GetCacheKey(k, PS.PortalId, ProdID)
                    End If
                End If



                If (DataCache.GetCache(strCacheKey) Is Nothing And strCacheKey <> "") Or DebugMode Or XMLFunction.ToLower = "getorderxml" Then

                    Dim strXML As String = ""

                    If XMLFunction = "" Then
                        strXML = getTemplateXML(container)
                    ElseIf XMLFunction.ToLower = "xmlgetproductsincat" Then
                        Dim objPCtrl As New ProductController
                        strXML = objPCtrl.xmlGetProductsInCat(PS.PortalId, _CatID, DataBinder.Eval(container.DataItem, "Lang"), _ProductTabID)
                    ElseIf XMLFunction.ToLower = "getproductxml" Then
                        Dim objECtrl As New Export
                        strXML = objECtrl.GetProductXML(CInt(ProdID), DataBinder.Eval(container.DataItem, "Lang"))
                    ElseIf XMLFunction.ToLower = "getorderxml" Then
                        Dim objECtrl As New Export
                        strXML = "<root>"
                        strXML &= objECtrl.GetOrderXML(DataBinder.Eval(container.DataItem, "OrderID"))
                        strXML &= "<Stg2FormXML>" & DataBinder.Eval(container.DataItem, "Stg2FormXML") & "</Stg2FormXML>"
                        strXML &= "<Stg3FormXML>" & DataBinder.Eval(container.DataItem, "Stg3FormXML") & "</Stg3FormXML>"
                        strXML &= "</root>"
                    End If


                    Dim xslData As New XmlDocument
                    Dim xslDataStr As String = ""
                    Dim strCacheXSLKey As String = ""

                    strCacheXSLKey = GetCacheKey(System.IO.Path.GetFileName(XSLFileName), PS.PortalId)

                    If strCacheXSLKey <> "" Then
                        If DataCache.GetCache(strCacheXSLKey) Is Nothing Or DebugMode Then
                            'check for the xsl in the template settings.
                            xslDataStr = GetStoreSettingText(PS.PortalId, XSLFName, GetCurrentCulture, False, True)
                            If xslDataStr = "" Then
                                ' Not in settigns so load XSLT file
                                xslData.Load(XSLFileName)
                                xslDataStr = xslData.OuterXml
                            End If
                            DataCache.SetCache(strCacheXSLKey, xslDataStr, DateAdd(DateInterval.Day, 1, Now))
                        Else
                            xslDataStr = CType(DataCache.GetCache(strCacheXSLKey), String)
                        End If
                    End If

                    If DebugMode Then
                        xslData = New XmlDocument
                        xslData.LoadXml(strXML)
                        xslData.Save(PS.HomeDirectoryMapPath & "debugProduct_" & XMLFunction & ".xml")
                    End If

                    DataCache.SetCache(strCacheKey, XSLTransInMemory(strXML, xslDataStr), DateAdd(DateInterval.Day, 1, Now))
                End If

                lc.Text = DataCache.GetCache(strCacheKey)
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))

            Catch ex As Exception
                If DebugMode Then
                    lc.Text = ex.ToString
                End If
            End Try
        End Sub

        Private Sub WebsiteURL_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            lc.Text = PS.PortalAlias.HTTPAlias
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub ProductName_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = System.Web.HttpUtility.HtmlEncode(DataBinder.Eval(container.DataItem, "ProductName"))
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub ProductRef_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = System.Web.HttpUtility.HtmlEncode(DataBinder.Eval(container.DataItem, "ProductRef"))
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub ProductID_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = DataBinder.Eval(container.DataItem, "ProductID")
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub Manufacturer_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = System.Web.HttpUtility.HtmlEncode(DataBinder.Eval(container.DataItem, "Manufacturer"))
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub Summary_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = System.Web.HttpUtility.HtmlEncode(DataBinder.Eval(container.DataItem, "Summary"))
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub SummaryHTML_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = Replace(System.Web.HttpUtility.HtmlEncode(DataBinder.Eval(container.DataItem, "Summary")), vbLf, "<br/>")
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub Description_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = System.Web.HttpUtility.HtmlDecode(DataBinder.Eval(container.DataItem, "Description"))
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub Link_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)

            Dim strUrlName As String = GetStoreSetting(PS.PortalId, "urlname.column", "None")
            Try
                If strUrlName <> "" Then
                    strUrlName = DataBinder.Eval(container.DataItem, strUrlName)
                End If
            Catch ex As Exception
                strUrlName = ""
            End Try

            lc.Text = GetProductUrlByProductID(PS.PortalId, _ProductTabID, DataBinder.Eval(container.DataItem, "ProductID"), _CatID, strUrlName, False, GetCurrentCulture)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub LinkSimple_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = NavigateURL(_ProductTabID, "", "ProdID=" & DataBinder.Eval(container.DataItem, "ProductID"))
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub FromPrice_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getFromPrice(container, False)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub FromPriceCurrency_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getFromPrice(container, True)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub DealerPrice_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getFromDealerPriceOutput(container, False)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub DealerPriceCurrency_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getFromDealerPriceOutput(container, True)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub PriceDealer_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getFromDealerPrice(container, False)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub PriceDealerCurrency_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getFromDealerPrice(container, True)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub CurrencySymbol_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            lc.Text = getCurrencySymbol()
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub CurrencyISOcode_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            lc.Text = getCurrencyISOCode()
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub QtyRemaining_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = DataBinder.Eval(container.DataItem, "QtyRemaining")
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub StockPercent_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            Dim StockLevels As ProductStockLevels

            StockLevels = getProductStockLevels(container)
            If Not StockLevels Is Nothing Then
                lc.Text = StockLevels.Percent
            End If

            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub StockPercentSold_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            Dim StockLevels As ProductStockLevels

            StockLevels = getProductStockLevels(container)
            If Not StockLevels Is Nothing Then
                lc.Text = StockLevels.PercentSold
            End If
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub StockPercentInProcess_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            Dim StockLevels As ProductStockLevels

            StockLevels = getProductStockLevels(container)
            If Not StockLevels Is Nothing Then
                lc.Text = StockLevels.PercentInProgess
            End If
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub StockPercentActual_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            Dim StockLevels As ProductStockLevels

            StockLevels = getProductStockLevels(container)
            If Not StockLevels Is Nothing Then
                lc.Text = StockLevels.PercentActual
            End If
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub


        Private Sub SalePrice_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getSalePriceOutput(container, False)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub SalePriceCurrency_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getSalePriceOutput(container, True)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub PriceSale_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getSalePrice(container, False)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub PriceSaleCurrency_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getSalePrice(container, True)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub BestPrice_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getBestPriceOutput(container, False)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub BestPriceCurrency_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getBestPriceOutput(container, True)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub PriceBest_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getBestPrice(container, False, Nothing)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub PriceBestCurrency_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = getBestPrice(container, True, Nothing)
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub EditLink_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim hyp As HyperLink
            hyp = CType(sender, HyperLink)
            Dim container As DataListItem
            container = CType(hyp.NamingContainer, DataListItem)
            If _blnHasModuleEditPermissions Then
                hyp.ImageUrl = "~/images/edit.gif"
                hyp.NavigateUrl = NavigateURL(_TabID, _EditCtrlKey, "mid=" & _ModuleID, "ProdID=" & DataBinder.Eval(container.DataItem, "ProductID"), "RtnTab=" & _TabID.ToString, "PageIndex=" & _PageIndex.ToString, "CatID=" & _CatID.ToString, "SkinSrc=" & QueryStringEncode(DotNetNuke.Common.ResolveUrl("~/DesktopModules/NB_Store/Skins/Dark/Edit")))
            Else
                hyp.Visible = False
            End If

            If hyp.Visible Then
                hyp.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub Image_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ThumbW As String
            Dim ThumbH As String = "0"

            If Not IsNumeric(_ThumbSize) Then
                Dim ThumbSplit() As String
                ThumbSplit = _ThumbSize.Split("x"c)
                ThumbW = ThumbSplit(0)
                If ThumbSplit.GetUpperBound(0) = 1 Then
                    ThumbH = ThumbSplit(1)
                End If
            Else
                ThumbW = _ThumbSize
            End If

            Dim lc As Image
            lc = CType(sender, Image)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            Dim ImageID As Integer = DataBinder.Eval(container.DataItem, "ImageID")
            Dim PortalID As Integer = DataBinder.Eval(container.DataItem, "PortalID")
            If GetStoreSettingBoolean(PortalID, "diskthumbnails.flag") Then
                Dim objThumb As New ThumbFunctions
                Dim ImagePath As String = DataBinder.Eval(container.DataItem, "ImageURL")
                lc.ImageUrl = objThumb.GetThumbURLName(ImagePath, ThumbW, ThumbH)
                If lc.ImageUrl = "" Then
                    lc.ImageUrl = _StoreInstallPath & "makethumbnail.ashx?Image=" & ImageID.ToString & "&w=" & ThumbW & "&tabid=" & _TabID & "&h=" & ThumbH
                End If
            Else
                lc.ImageUrl = _StoreInstallPath & "makethumbnail.ashx?Image=" & ImageID.ToString & "&w=" & ThumbW & "&tabid=" & _TabID & "&h=" & ThumbH
            End If
            lc.AlternateText = DataBinder.Eval(container.DataItem, "ImageDesc")
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub ImageURL_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim container As DataListItem

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)
            Dim ImageURL As String = DataBinder.Eval(container.DataItem, "ImageURL")

            lc.Text = ImageURL
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub CatImageURL_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim container As DataListItem

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)
            Dim objCCtrl As New CategoryController
            Dim objCInfo As NB_Store_CategoriesInfo
            objCInfo = objCCtrl.GetCategory(_CatID, GetCurrentCulture)
            If Not objCInfo Is Nothing Then
                lc.Text = objCInfo.ImageURL
            Else
                lc.Text = ""
            End If
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))

        End Sub

        Private Sub CatName_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim container As DataListItem

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)
            Dim objCCtrl As New CategoryController
            Dim objCInfo As NB_Store_CategoriesInfo
            objCInfo = objCCtrl.GetCategory(_CatID, GetCurrentCulture)
            If Not objCInfo Is Nothing Then
                lc.Text = objCInfo.CategoryName
            Else
                lc.Text = ""
            End If
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))

        End Sub

        Private Sub ThumbURL_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim container As DataListItem
            Dim ThumbW As String
            Dim ThumbH As String = "0"

            If Not IsNumeric(_ThumbSize) Then
                Dim ThumbSplit() As String
                ThumbSplit = _ThumbSize.Split("x"c)
                ThumbW = ThumbSplit(0)
                If ThumbSplit.GetUpperBound(0) = 1 Then
                    ThumbH = ThumbSplit(1)
                End If
            Else
                ThumbW = _ThumbSize
            End If

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)

            Dim ImageURL As String
            Dim ImageID As Integer = DataBinder.Eval(container.DataItem, "ImageID")
            Dim PortalID As Integer = DataBinder.Eval(container.DataItem, "PortalID")
            If GetStoreSettingBoolean(PortalID, "diskthumbnails.flag") Then
                Dim objThumb As New ThumbFunctions
                Dim ImagePath As String = DataBinder.Eval(container.DataItem, "ImageURL")
                ImageURL = objThumb.GetThumbURLName(ImagePath, ThumbW, ThumbH)
                If ImageURL = "" Then
                    ImageURL = _StoreInstallPath & "makethumbnail.ashx?Image=" & ImageID.ToString & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH
                End If
            Else
                ImageURL = _StoreInstallPath & "makethumbnail.ashx?Image=" & ImageID.ToString & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH
            End If

            If ImageID <= 0 Then
                ImageURL = _StoreInstallPath & "makethumbnail.ashx?Image=" & QueryStringEncode(_StoreInstallPath & "img/noimage.png") & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH
            End If

            lc.Text = ImageURL
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub ThumbSize_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim container As DataListItem
            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = _ThumbSize
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub StoreInstallPath_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim container As DataListItem
            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = _StoreInstallPath
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub SoldOutImg_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim container As DataListItem
            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)

            lc.Text = ""

            If _LockStockOnCart Then
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))
                Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")

                Dim StockLevels As ProductStockLevels

                StockLevels = getProductStockLevels(container)
                If Not StockLevels Is Nothing Then
                    If StockLevels.PercentSold = 100 Then
                        lc.Text = System.Web.HttpUtility.HtmlDecode(_SoldOutImg)
                    End If
                Else
                    'Old code, left for double proof
                    Dim aryStock As ArrayList
                    aryStock = GetAvailableModelList(PS.PortalId, ProdID, Lang, IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))
                    If aryStock.Count = 0 Then
                        lc.Text = System.Web.HttpUtility.HtmlDecode(_SoldOutImg)
                    End If
                End If

            Else
                Dim qtyR As Integer = DataBinder.Eval(container.DataItem, "QtyRemaining")
                If qtyR = 0 Then
                    lc.Text = System.Web.HttpUtility.HtmlDecode(_SoldOutImg)
                End If
            End If

            If lc.Text = "" Then
                lc.Visible = False
            End If

            If lc.Visible Then
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub ImageLB_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ThumbW As String
            Dim ThumbH As String = "0"

            If Not IsNumeric(_ThumbSize) Then
                Dim ThumbSplit() As String
                ThumbSplit = _ThumbSize.Split("x"c)
                ThumbW = ThumbSplit(0)
                If ThumbSplit.GetUpperBound(0) = 1 Then
                    ThumbH = ThumbSplit(1)
                End If
            Else
                ThumbW = _ThumbSize
            End If

            Dim strData As String
            strData = "<A HREF=""[Product:ImageURL]"" rel=""GRP1"" class=""nyroModal"" title=""[Product:ImageDesc]""><img border=""0"" src=""[Product:ImageThumb]"" alt=""[Product:ImageDesc]""/></A>"

            Dim objSCtrl As New SettingsController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim objSInfo As NB_Store_SettingsInfo
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim lc As Literal
            Dim container As DataListItem

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)
            Dim ImageID As Integer = DataBinder.Eval(container.DataItem, "ImageID")
            Dim ImageURL As String = DataBinder.Eval(container.DataItem, "ImageURL")

            objSInfo = objSCtrl.GetSetting(PS.PortalId, "lightboxtemplate.name", DataBinder.Eval(container.DataItem, "Lang"))
            Dim lboxName As String = "nyromodal.template"
            If Not objSInfo Is Nothing Then
                lboxName = objSInfo.SettingValue
            End If
            objSTInfo = objSCtrl.GetSettingsText(PS.PortalId, lboxName, DataBinder.Eval(container.DataItem, "Lang"))
            If Not objSTInfo Is Nothing Then
                strData = System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText)
            End If

            If ImageURL = "" Then
                ImageURL = "noimage.png"
                strData = "<img border=""0"" src=""[Product:ImageThumb]"" alt=""[Product:ImageDesc]""/>"
                objSTInfo = objSCtrl.GetSettingsText(PS.PortalId, "noimage.template", DataBinder.Eval(container.DataItem, "Lang"))
                If Not objSTInfo Is Nothing Then
                    strData = System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText)
                End If
            End If

            strData = Replace(strData, "[Product:ImageURL]", ImageURL)
            strData = Replace(strData, "[Product:ImageDesc]", DataBinder.Eval(container.DataItem, "ImageDesc"))

            Dim PortalID As Integer = DataBinder.Eval(container.DataItem, "PortalID")
            If GetStoreSettingBoolean(PortalID, "diskthumbnails.flag") Then
                Dim objThumb As New ThumbFunctions
                Dim ImagePath As String = DataBinder.Eval(container.DataItem, "ImageURL")
                Dim imgUrlName As String = objThumb.GetThumbURLName(ImagePath, ThumbW, ThumbH)
                If imgUrlName = "" Then
                    strData = Replace(strData, "[Product:ImageThumb]", _StoreInstallPath & "makethumbnail.ashx?Image=" & ImageID.ToString & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH)
                Else
                    strData = Replace(strData, "[Product:ImageThumb]", imgUrlName)
                End If
            Else
                strData = Replace(strData, "[Product:ImageThumb]", _StoreInstallPath & "makethumbnail.ashx?Image=" & ImageID.ToString & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH)
            End If

            lc.Text = strData

            If lc.Visible Then
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub GalleryId_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim objPCtrl As New ProductController
            Dim objPIInfo As NB_Store_ProductImageInfo
            Dim lc As Literal
            Dim container As DataListItem
            Dim arylist As ArrayList
            Dim GalleryIndex As Integer = -1

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)

            If IsNumeric(Replace(lc.ID, "galleryid", "")) Then
                GalleryIndex = CInt(Replace(lc.ID, "galleryid", ""))
                GalleryIndex = GalleryIndex - 1
            End If

            lc.ID = ""

            If GalleryIndex >= 0 Then
                arylist = objPCtrl.GetProductImageList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If arylist.Count >= (GalleryIndex + 1) Then
                    objPIInfo = CType(arylist(GalleryIndex), NB_Store_ProductImageInfo)
                    lc.Text = objPIInfo.ImageID.ToString
                Else
                    lc.Text = ""
                End If
            Else
                lc.Text = ""
            End If
            If lc.Text = "" Then lc.Visible = False

            If lc.Visible Then
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub GalleryThumbUrl_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ThumbW As String
            Dim ThumbH As String = "0"

            If Not IsNumeric(_GalleryThumbSize) Then
                Dim ThumbSplit() As String
                ThumbSplit = _GalleryThumbSize.Split("x"c)
                ThumbW = ThumbSplit(0)
                If ThumbSplit.GetUpperBound(0) = 1 Then
                    ThumbH = ThumbSplit(1)
                End If
            Else
                ThumbW = _GalleryThumbSize
            End If

            Dim objPCtrl As New ProductController
            Dim objPIInfo As NB_Store_ProductImageInfo
            Dim lc As Literal
            Dim container As DataListItem
            Dim arylist As ArrayList
            Dim GalleryIndex As Integer = -1

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)

            If IsNumeric(Replace(lc.ID, "gallerythumburl", "")) Then
                GalleryIndex = CInt(Replace(lc.ID, "gallerythumburl", ""))
                GalleryIndex = GalleryIndex - 1
            End If

            lc.ID = ""

            If GalleryIndex >= 0 Then
                arylist = objPCtrl.GetProductImageList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If arylist.Count >= (GalleryIndex + 1) Then
                    objPIInfo = CType(arylist(GalleryIndex), NB_Store_ProductImageInfo)
                    Dim ImageURL As String = objPIInfo.ImageURL
                    Dim ImageID As Integer = objPIInfo.ImageID

                    Dim PortalID As Integer = DataBinder.Eval(container.DataItem, "PortalID")
                    If GetStoreSettingBoolean(PortalID, "diskthumbnails.flag") Then
                        Dim objThumb As New ThumbFunctions
                        ImageURL = objThumb.GetThumbURLName(ImageURL, ThumbW, ThumbH)
                        If ImageURL = "" Then
                            ImageURL = _StoreInstallPath & "makethumbnail.ashx?Image=" & ImageID.ToString & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH
                        End If
                    Else
                        ImageURL = _StoreInstallPath & "makethumbnail.ashx?Image=" & ImageID.ToString & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH
                    End If

                    lc.Text = ImageURL
                Else
                    lc.Text = ""
                End If
            Else
                lc.Text = ""
            End If
            If lc.Text = "" Then lc.Visible = False

            If lc.Visible Then
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub


        Private Sub GalleryUrl_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim objPCtrl As New ProductController
            Dim objPIInfo As NB_Store_ProductImageInfo
            Dim lc As Literal
            Dim container As DataListItem
            Dim arylist As ArrayList
            Dim GalleryIndex As Integer = -1
            Dim TagString As String = ""
            Dim ThumbSize As String = ""

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)

            TagString = lc.ID
            Dim TagAtt As String() = TagString.Split(":"c)
            If TagAtt.GetUpperBound(0) = 1 Then
                ThumbSize = TagAtt(1)
            End If

            If IsNumeric(Replace(TagAtt(0), "galleryurl", "")) Then
                GalleryIndex = CInt(Replace(TagAtt(0), "galleryurl", ""))
                GalleryIndex = GalleryIndex - 1
            End If

            lc.ID = ""

            If GalleryIndex >= 0 Then
                arylist = objPCtrl.GetProductImageList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If arylist.Count >= (GalleryIndex + 1) Then
                    objPIInfo = CType(arylist(GalleryIndex), NB_Store_ProductImageInfo)
                    Dim ImageURL As String = objPIInfo.ImageURL
                    If ThumbSize <> "" Then
                        Dim objThumb As New ThumbFunctions
                        ImageURL = objThumb.GetThumbURLName(ImageURL, objThumb.getThumbWidth(ThumbSize), objThumb.getThumbHeight(ThumbSize))
                    End If
                    lc.Text = ImageURL
                Else
                    lc.Text = ""
                End If
            Else
                lc.Text = ""
            End If
            If lc.Text = "" Then lc.Visible = False

            If lc.Visible Then
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub Gallery_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim strData As String
            Dim objSCtrl As New SettingsController
            Dim objPCtrl As New ProductController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim objSInfo As NB_Store_SettingsInfo
            Dim objPIInfo As NB_Store_ProductImageInfo
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim lc As Literal
            Dim container As DataListItem
            Dim arylist As ArrayList
            Dim GalleryIndex As Integer = -1
            Dim ThumbW As String
            Dim ThumbH As String = "0"

            If Not IsNumeric(_GalleryThumbSize) Then
                Dim ThumbSplit() As String
                ThumbSplit = _GalleryThumbSize.Split("x"c)
                ThumbW = ThumbSplit(0)
                If ThumbSplit.GetUpperBound(0) = 1 Then
                    ThumbH = ThumbSplit(1)
                End If
            Else
                ThumbW = _GalleryThumbSize
            End If

            strData = "<A HREF=""[Product:ImageURL]"" rel=""GRP1"" class=""nyroModal"" title=""[Product:ImageDesc]""><img border=""0"" src=""[Product:ImageThumb]"" alt=""[Product:ImageDesc]""/></A>"

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)


            objSInfo = objSCtrl.GetSetting(PS.PortalId, "lightboxgallerytemplate.name", DataBinder.Eval(container.DataItem, "Lang"))
            Dim lboxName As String = "nyromodalgallery.template"
            If Not objSInfo Is Nothing Then
                lboxName = objSInfo.SettingValue
            End If
            objSTInfo = objSCtrl.GetSettingsText(PS.PortalId, lboxName, DataBinder.Eval(container.DataItem, "Lang"))
            If Not objSTInfo Is Nothing Then
                strData = System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText)
            End If

            If IsNumeric(Replace(lc.ID, "gallery", "")) Then
                GalleryIndex = CInt(Replace(lc.ID, "gallery", ""))
                GalleryIndex = GalleryIndex - 1
            End If

            lc.ID = ""

            If GalleryIndex >= 0 Then
                arylist = objPCtrl.GetProductImageList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If arylist.Count >= (GalleryIndex + 1) Then
                    objPIInfo = CType(arylist(GalleryIndex), NB_Store_ProductImageInfo)
                    Dim ImageURL As String = objPIInfo.ImageURL
                    If ImageURL = "" Then
                        ImageURL = "noimage.png"
                        strData = "<img border=""0"" src=""[Product:ImageThumb]"" alt=""[Product:ImageDesc]""/>"
                        objSTInfo = objSCtrl.GetSettingsText(PS.PortalId, "noimagegallery.template", DataBinder.Eval(container.DataItem, "Lang"))
                        If Not objSTInfo Is Nothing Then
                            strData = System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText)
                        End If
                    End If
                    strData = Replace(strData, "[Product:ImageURL]", ImageURL)
                    strData = Replace(strData, "[Product:ImageDesc]", objPIInfo.ImageDesc)

                    Dim PortalID As Integer = DataBinder.Eval(container.DataItem, "PortalID")
                    If GetStoreSettingBoolean(PortalID, "diskthumbnails.flag") Then
                        Dim objThumb As New ThumbFunctions
                        Dim strURLThumbName As String = objThumb.GetThumbURLName(ImageURL, ThumbW, ThumbH)
                        If strURLThumbName = "" Then
                            strData = Replace(strData, "[Product:ImageThumb]", _StoreInstallPath & "makethumbnail.ashx?Image=" & objPIInfo.ImageID.ToString & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH)
                        Else
                            strData = Replace(strData, "[Product:ImageThumb]", strURLThumbName)
                        End If
                    Else
                        strData = Replace(strData, "[Product:ImageThumb]", _StoreInstallPath & "makethumbnail.ashx?Image=" & objPIInfo.ImageID.ToString & "&amp;w=" & ThumbW & "&amp;tabid=" & _TabID & "&amp;h=" & ThumbH)
                    End If

                    lc.Text = strData
                Else
                    lc.Text = ""
                End If
            Else
                lc.Text = ""
            End If
            If lc.Text = "" Then lc.Visible = False

            If lc.Visible Then
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub Doc_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim objPCtrl As New ProductController
            Dim cmd As ImageButton
            Dim arylist As ArrayList
            Dim objDInfo As NB_Store_ProductDocInfo
            Dim strButtonType As String = "doc"

            cmd = CType(sender, ImageButton)

            If cmd.ID.ToLower.StartsWith("docpurchased") Then
                strButtonType = "docpurchased"
            Else
                strButtonType = "doc"
            End If

            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)

            Dim DocIndex As Integer = -1
            If IsNumeric(Replace(cmd.ID, strButtonType, "")) Then
                DocIndex = CInt(Replace(cmd.ID, strButtonType, ""))
                DocIndex = DocIndex - 1
            End If
            cmd.Visible = False

            If DocIndex >= 0 Then

                arylist = objPCtrl.GetProductDocList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If arylist.Count >= (DocIndex + 1) Then
                    objDInfo = CType(arylist(DocIndex), NB_Store_ProductDocInfo)
                    cmd.CommandArgument = objDInfo.DocID
                    If strButtonType = "docpurchased" Then
                        cmd.CommandName = "DocPurchased"
                    Else
                        cmd.CommandName = "DocDownload"
                    End If
                    If cmd.ImageUrl = "" Then
                        Select Case objDInfo.FileExt.ToLower
                            Case ".pdf"
                                cmd.ImageUrl = _StoreInstallPath & "img/pdf.png"
                            Case ".zip"
                                cmd.ImageUrl = _StoreInstallPath & "img/zip.png"
                            Case Else
                                cmd.ImageUrl = _StoreInstallPath & "img/disk.png"
                        End Select
                    End If
                    If cmd.ToolTip = "" Then
                        cmd.ToolTip = objDInfo.DocDesc
                    End If
                    If strButtonType = "doc" Then
                        cmd.Visible = True
                    Else
                        If DocHasBeenPurchased(_UserID, objDInfo.ProductID) Then
                            cmd.Visible = True
                        Else
                            cmd.Visible = False
                        End If
                    End If
                End If
            End If

            If cmd.Visible Then
                cmd.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If


        End Sub

        Private Sub DocLink_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim objPCtrl As New ProductController
            Dim cmd As LinkButton
            Dim arylist As ArrayList
            Dim objDInfo As NB_Store_ProductDocInfo
            Dim strButtonType As String = "doclink"

            cmd = CType(sender, LinkButton)

            If cmd.ID.ToLower.StartsWith("doclink") Then
                strButtonType = "doclink"
            Else
                strButtonType = "docpurchasedlink"
            End If

            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)

            Dim DocIndex As Integer = -1
            If IsNumeric(Replace(cmd.ID, strButtonType, "")) Then
                DocIndex = CInt(Replace(cmd.ID, strButtonType, ""))
                DocIndex = DocIndex - 1
            End If
            cmd.Visible = False

            If DocIndex >= 0 Then

                arylist = objPCtrl.GetProductDocList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If arylist.Count >= (DocIndex + 1) Then
                    objDInfo = CType(arylist(DocIndex), NB_Store_ProductDocInfo)
                    cmd.CommandArgument = objDInfo.DocID
                    If strButtonType = "docpurchasedlink" Then
                        cmd.CommandName = "DocPurchased"
                    Else
                        cmd.CommandName = "DocDownload"
                    End If
                    If cmd.Text = "" Then
                        cmd.Text = objDInfo.DocDesc
                    End If
                    If cmd.ToolTip = "" Then
                        cmd.ToolTip = objDInfo.FileName
                    End If
                    If strButtonType = "doclink" Then
                        cmd.Visible = True
                    Else
                        If DocHasBeenPurchased(_UserID, objDInfo.ProductID) Then
                            cmd.Visible = True
                        Else
                            cmd.Visible = False
                        End If
                    End If
                End If
            End If

            If cmd.Visible Then
                cmd.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub DocDesc_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)

            Dim objPCtrl As New ProductController
            Dim arylist As ArrayList
            Dim objDInfo As NB_Store_ProductDocInfo
            Dim strButtonType As String = "docdesc"

            Dim DocIndex As Integer = -1
            If IsNumeric(Replace(lc.ID, strButtonType, "")) Then
                DocIndex = CInt(Replace(lc.ID, strButtonType, ""))
                DocIndex = DocIndex - 1
            End If

            If DocIndex >= 0 Then
                arylist = objPCtrl.GetProductDocList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If arylist.Count >= (DocIndex + 1) Then
                    objDInfo = CType(arylist(DocIndex), NB_Store_ProductDocInfo)
                    lc.Text = objDInfo.DocDesc
                End If
            End If

            If lc.Visible Then
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub Model_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ddl As DropDownList
            ddl = CType(sender, DropDownList)
            CreateModelDropDownList(ddl, False)
        End Sub


        Private Sub Model2_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ddl As DropDownList
            ddl = CType(sender, DropDownList)
            CreateModelDropDownList(ddl, True)
        End Sub

        Private Sub CreateModelDropDownList(ByVal ddl As DropDownList, ByVal AlwaysDisplay As Boolean)
            Dim container As DataListItem
            container = CType(ddl.NamingContainer, DataListItem)

            Dim objCtrl As New ProductController
            Dim objPromoCtrl As New PromoController
            Dim aryList As ArrayList
            Dim objInfo As New NB_Store_ModelInfo
            Dim li As ListItem
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim SalePrice As Double

            aryList = objCtrl.GetModelList(-1, CInt(DataBinder.Eval(container.DataItem, "ProductID")), DataBinder.Eval(container.DataItem, "Lang"), IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))

            If aryList.Count <= 1 And Not AlwaysDisplay Then
                ddl.Visible = False
            Else

                'check if all unit costs are the same
                Dim ShowCost As Boolean = False
                Dim HoldCost As Decimal = CType(aryList.Item(0), NB_Store_ModelInfo).UnitCost
                For Each objInfo In aryList
                    If objInfo.UnitCost <> HoldCost Then
                        ShowCost = True
                    End If
                Next

                Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))
                Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")
                aryList = GetAvailableModelList(PS.PortalId, ProdID, Lang, IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))

                If aryList.Count <= 0 Then
                    ddl.Visible = False
                End If

                For Each objInfo In aryList
                    li = New ListItem
                    If ShowCost Then
                        If IsDealer(PS.PortalId, _UserInfo, Lang) Then
                            If (objInfo.UnitCost > 0) And (objInfo.DealerCost <> objInfo.UnitCost) Then
                                li.Text = objInfo.ModelName & " (" & FormatToStoreCurrency(objInfo.UnitCost) & ") " & FormatToStoreCurrency(objInfo.DealerCost)
                            Else
                                li.Text = objInfo.ModelName & FormatToStoreCurrency(objInfo.DealerCost)
                            End If
                        Else
                            SalePrice = objPromoCtrl.GetSalePrice(objInfo, _UserInfo)
                            If (SalePrice > -1 And objInfo.UnitCost > 0) And (objInfo.UnitCost <> SalePrice) Then
                                li.Text = objInfo.ModelName & " (" & FormatToStoreCurrency(objInfo.UnitCost) & ") " & FormatToStoreCurrency(SalePrice)
                            Else
                                li.Text = objInfo.ModelName & " " & FormatToStoreCurrency(objInfo.UnitCost)
                            End If
                        End If
                    Else
                        li.Text = objInfo.ModelName
                    End If
                    li.Value = objInfo.ModelID
                    ddl.Items.Add(li)

                    If objInfo.QtyRemaining = 0 Then
                        Dim strOutMsg As String = GetStoreSettingText(PS.PortalId, "outofstockmodelmsg.text", Lang)
                        If strOutMsg <> "" Then
                            ddl.Items(ddl.Items.Count - 1).Text &= strOutMsg
                        End If
                    End If

                Next
                If aryList.Count > 0 Then
                    ddl.SelectedIndex = 0
                End If
            End If
            If ddl.Visible Then
                ddl.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If


        End Sub

        Private Sub ModelRadio2_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim rbl As RadioButtonList
            rbl = CType(sender, RadioButtonList)
            CreateModelRadioButList(rbl, True)
        End Sub

        Private Sub GatewayRadio_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim rbl As RadioButtonList
            rbl = CType(sender, RadioButtonList)
            CreateGatewayRadioButList(rbl, False)
        End Sub

        Private Sub CreateGatewayRadioButList(ByVal rbl As RadioButtonList, ByVal AlwaysDisplay As Boolean)
            Dim container As DataListItem
            container = CType(rbl.NamingContainer, DataListItem)
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim objGInfo As NB_Store_GatewayInfo
            Dim aryList As ArrayList
            Dim li As ListItem

            aryList = GetAvailableGateways(PS.PortalId)

            For Each objGInfo In aryList
                li = New ListItem
                li.Value = objGInfo.ref
                li.Text = objGInfo.gatewaymsg
                rbl.Items.Add(li)

            Next

        End Sub


        Private Sub ModelRadio_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim rbl As RadioButtonList
            rbl = CType(sender, RadioButtonList)
            CreateModelRadioButList(rbl, False)
        End Sub

        Private Sub CreateModelRadioButList(ByVal rbl As RadioButtonList, ByVal AlwaysDisplay As Boolean)
            Dim container As DataListItem
            container = CType(rbl.NamingContainer, DataListItem)

            Dim objCtrl As New ProductController
            Dim objPromoCtrl As New PromoController
            Dim aryList As ArrayList
            Dim objInfo As New NB_Store_ModelInfo
            Dim li As ListItem
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim SalePrice As Double

            aryList = objCtrl.GetModelList(-1, CInt(DataBinder.Eval(container.DataItem, "ProductID")), DataBinder.Eval(container.DataItem, "Lang"), IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))

            If aryList.Count <= 1 And Not AlwaysDisplay Then
                rbl.Visible = False
            Else
                'check if all unit costs are the same
                Dim ShowCost As Boolean = False
                Dim HoldCost As Decimal = CType(aryList.Item(0), NB_Store_ModelInfo).UnitCost
                For Each objInfo In aryList
                    If objInfo.UnitCost <> HoldCost Then
                        ShowCost = True
                    End If
                Next

                Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))
                Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")
                aryList = GetAvailableModelList(PS.PortalId, ProdID, Lang, IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))

                If aryList.Count <= 0 Then
                    rbl.Visible = False
                End If

                For Each objInfo In aryList
                    li = New ListItem
                    If ShowCost Then
                        If IsDealer(PS.PortalId, _UserInfo, Lang) Then
                            If (objInfo.UnitCost > 0) And (objInfo.DealerCost <> objInfo.UnitCost) Then
                                li.Text = objInfo.ModelName & " (" & FormatToStoreCurrency(objInfo.UnitCost) & ") " & FormatToStoreCurrency(objInfo.DealerCost)
                            Else
                                li.Text = objInfo.ModelName & FormatToStoreCurrency(objInfo.DealerCost)
                            End If
                        Else
                            SalePrice = objPromoCtrl.GetSalePrice(objInfo, _UserInfo)
                            If (SalePrice > -1 And objInfo.UnitCost > 0) And (objInfo.UnitCost <> SalePrice) Then
                                li.Text = objInfo.ModelName & " (" & FormatToStoreCurrency(objInfo.UnitCost) & ") " & FormatToStoreCurrency(SalePrice)
                            Else
                                li.Text = objInfo.ModelName & " " & FormatToStoreCurrency(objInfo.UnitCost)
                            End If
                        End If
                    Else
                        li.Text = objInfo.ModelName
                    End If
                    li.Value = objInfo.ModelID
                    rbl.Items.Add(li)
                    If objInfo.QtyRemaining = 0 Then
                        rbl.Items(rbl.Items.Count - 1).Enabled = False
                        Dim strOutMsg As String = GetStoreSettingText(PS.PortalId, "outofstockmodelmsg.text", Lang)
                        If strOutMsg <> "" Then
                            rbl.Items(rbl.Items.Count - 1).Text &= strOutMsg
                        End If
                    End If
                Next
                If aryList.Count > 0 Then
                    rbl.SelectedIndex = 0
                End If
            End If
            If rbl.Visible Then
                rbl.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If
        End Sub


        Private Sub ModelTable_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim objSCtrl As New SettingsController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)

            Dim objCtrl As New ProductController
            Dim objPromoCtrl As New PromoController
            Dim aryList As ArrayList
            Dim objInfo As New NB_Store_ModelInfo
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))
            Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")

            objSTInfo = objSCtrl.GetSettingsText(PS.PortalId, "modeltablerow.template", Lang)

            If Not objSTInfo Is Nothing Then
                Dim rowTemplate As String = objSTInfo.SettingText
                If rowTemplate.Length > 0 Then
                    objSTInfo = objSCtrl.GetSettingsText(PS.PortalId, "modeltable.template", Lang)
                    If Not objSTInfo Is Nothing Then
                        Dim tableTemplate As String = objSTInfo.SettingText
                        If tableTemplate.Length > 0 Then
                            Dim strRows As String = ""

                            'aryList = objCtrl.GetModelList(-1, ProdID, Lang)
                            aryList = GetAvailableModelList(PS.PortalId, ProdID, Lang, IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))

                            If aryList.Count > 0 Then
                                Dim r As New HtmlTableRow

                                For Each objInfo In aryList

                                    Dim objTR As New TokenStoreReplace(objInfo)
                                    strRows = strRows & objTR.DoTokenReplace(rowTemplate)
                                Next

                                tableTemplate = Replace(tableTemplate, "[TAG:TABLEROWS]", strRows)

                                lc.Text = System.Web.HttpUtility.HtmlDecode(tableTemplate)

                            End If

                        Else
                            lc.Text = "*** 'modeltable.template' is blank ***"
                        End If
                    Else
                        lc.Text = "*** No 'modeltable.template' exists ***"
                    End If
                Else
                    lc.Text = "*** 'modeltablerow.template' is blank ***"
                End If
            Else
                lc.Text = "*** No 'modeltablerow.template' exists ***"
            End If
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))

        End Sub

        Private Sub FileUpload_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cmd As FileUpload
            cmd = CType(sender, FileUpload)
            If cmd.Visible Then
                cmd.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If
        End Sub

        Private Sub TestCtrlDisplay_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ctrl As Control
            ctrl = CType(sender, Control)
            If ctrl.Visible Then
                ctrl.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If
        End Sub


        Private Sub AddToBasket_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cmd As LinkButton
            cmd = CType(sender, LinkButton)
            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)
            cmd.CommandArgument = DataBinder.Eval(container.DataItem, "ProductID")
            cmd.CommandName = "AddToBasket"
            cmd.CssClass = _CssBuyButton
            cmd.Text = _BuyText

            If DataBinder.Eval(container.DataItem, "QtyRemaining") = 0 Then
                cmd.Visible = False
            Else
                Dim objCtrl As New ProductController
                Dim aryList As ArrayList
                Dim objInfo As New NB_Store_ModelInfo
                Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))
                Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings

                If _LockStockOnCart Then
                    aryList = GetAvailableModelList(PS.PortalId, ProdID, Lang, IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))
                Else
                    aryList = objCtrl.GetModelList(-1, ProdID, Lang, IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))
                End If

                Dim hf As HiddenField
                hf = CType(container.FindControl("hfModel"), HiddenField)
                If Not hf Is Nothing Then
                    'get cmd property vales from value
                    If hf.Value <> "" Then
                        assignByReflection(cmd, hf.Value)
                    End If
                End If

                If aryList.Count >= 1 Then
                    objInfo = aryList(0)
                    'populate hidden field with modelid
                    If Not hf Is Nothing Then
                        hf.Value = objInfo.ModelID
                    End If
                Else
                    cmd.Visible = False
                End If
            End If

            If cmd.Visible Then
                cmd.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub Purchase_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cmd As LinkButton
            cmd = CType(sender, LinkButton)
            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)
            cmd.CommandName = "Purchase"

            assignByReflection(cmd, cmd.Text)

            If cmd.Text = "" Then cmd.Text = "Purchase >>"


            If cmd.Visible Then
                cmd.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub


        Private Sub AddToRelated_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cmd As LinkButton
            cmd = CType(sender, LinkButton)
            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)
            cmd.CommandArgument = DataBinder.Eval(container.DataItem, "ProductID")
            cmd.CommandName = "AddRelated"
            cmd.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub
        Private Sub AddToWishList_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cmd As LinkButton
            cmd = CType(sender, LinkButton)
            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)
            cmd.CommandArgument = DataBinder.Eval(container.DataItem, "ProductID")
            cmd.CommandName = "AddToWishList"
            cmd.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub RemoveFromWishList_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cmd As LinkButton
            cmd = CType(sender, LinkButton)
            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)
            cmd.CommandArgument = DataBinder.Eval(container.DataItem, "ProductID")
            cmd.CommandName = "RemoveFromWishList"
            cmd.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub QtyDDL_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ddl As DropDownList
            ddl = CType(sender, DropDownList)
            ddl.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub


        Private Sub QtyInput_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim txt As TextBox
            txt = CType(sender, TextBox)
            Dim container As DataListItem
            container = CType(txt.NamingContainer, DataListItem)

            If DataBinder.Eval(container.DataItem, "QtyRemaining") = 0 Then
                txt.Visible = False
            End If
            If txt.Visible Then
                txt.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub QtyVal_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim val As RangeValidator
            val = CType(sender, RangeValidator)
            val.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))            
        End Sub

        Private Sub QtyRVal_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim rval As RequiredFieldValidator
            rval = CType(sender, RequiredFieldValidator)
            rval.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub OptionName_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            Dim objCtrl As New ProductController
            Dim OptionIndex As Integer = -1
            Dim aryList As ArrayList
            Dim container As DataListItem

            lc = CType(sender, Literal)
            container = CType(lc.NamingContainer, DataListItem)

            If IsNumeric(Replace(lc.ID, "optionname", "")) Then
                OptionIndex = CInt(Replace(lc.ID, "optionname", ""))
                OptionIndex = OptionIndex - 1
            End If

            If OptionIndex >= 0 Then
                aryList = objCtrl.GetOptionList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If aryList.Count >= (OptionIndex + 1) Then
                    lc.Text = CType(aryList(OptionIndex), NB_Store_OptionInfo).OptionDesc
                Else
                    lc.Text = ""
                End If
            Else
                lc.Text = ""
            End If
            If lc.Text = "" Then lc.Visible = False
            If lc.Visible Then
                lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub OptionRadio_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim rbl As RadioButtonList = Nothing
            Dim objCtrl As New ProductController
            Dim OptionIndex As Integer = -1
            Dim aryList As ArrayList
            Dim container As DataListItem
            Dim objOInfo As NB_Store_OptionInfo
            Dim objOVInfo As NB_Store_OptionValueInfo
            Dim strDesc As String = ""

            rbl = CType(sender, RadioButtonList)
            rbl.Visible = False
            container = CType(rbl.NamingContainer, DataListItem)
            If IsNumeric(Replace(rbl.ID, "optionradio", "")) Then
                OptionIndex = CInt(Replace(rbl.ID, "optionradio", ""))
                OptionIndex = OptionIndex - 1
            End If

            If OptionIndex >= 0 Then
                aryList = objCtrl.GetOptionList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If aryList.Count >= (OptionIndex + 1) Then
                    Dim strID As String = "chk" & rbl.ID
                    Dim chk As CheckBox = CType(container.FindControl(strID), CheckBox)
                    If Not chk Is Nothing Then chk.Visible = False

                    objOInfo = CType(aryList(OptionIndex), NB_Store_OptionInfo)
                    aryList = objCtrl.GetOptionValueListWithInterface(objOInfo.OptionID, objOInfo.Lang)

                    If aryList.Count = 1 Then
                        rbl.Visible = False
                        Dim OptionValueInfo As NB_Store_OptionValueInfo = CType(aryList(0), NB_Store_OptionValueInfo)
                        If Not chk Is Nothing Then
                            strDesc = OptionValueInfo.OptionValueDesc
                            If OptionValueInfo.AddedCost > 0 Then
                                strDesc &= " +" & FormatToStoreCurrency(OptionValueInfo.AddedCost)
                            End If
                            If OptionValueInfo.AddedCost < 0 Then
                                strDesc &= " -" & FormatToStoreCurrency(OptionValueInfo.AddedCost)
                            End If

                            chk.Text = OptionValueInfo.OptionValueDesc
                            chk.Attributes.Add("OptionValueID", OptionValueInfo.OptionValueID)
                            chk.Visible = True
                        End If
                    Else
                        If aryList.Count > 1 Then
                            Dim li As ListItem
                            For Each objOVInfo In aryList
                                li = New ListItem
                                strDesc = objOVInfo.OptionValueDesc
                                If objOVInfo.AddedCost > 0 Then
                                    strDesc &= " +" & FormatToStoreCurrency(objOVInfo.AddedCost)
                                End If
                                If objOVInfo.AddedCost < 0 Then
                                    strDesc &= " -" & FormatToStoreCurrency(objOVInfo.AddedCost)
                                End If
                                li.Text = strDesc
                                li.Value = objOVInfo.OptionValueID
                                rbl.Items.Add(li)
                            Next
                            rbl.SelectedIndex = 0
                            rbl.Visible = True
                        End If
                    End If
                End If
            End If
            If rbl.Visible Then
                rbl.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Sub Option_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ddl As DropDownList = Nothing
            Dim objCtrl As New ProductController
            Dim OptionIndex As Integer = -1
            Dim aryList As ArrayList
            Dim container As DataListItem
            Dim objOInfo As NB_Store_OptionInfo
            Dim objOVInfo As NB_Store_OptionValueInfo
            Dim strDesc As String = ""

            ddl = CType(sender, DropDownList)
            ddl.Visible = False
            container = CType(ddl.NamingContainer, DataListItem)
            If IsNumeric(Replace(ddl.ID, "option", "")) Then
                OptionIndex = CInt(Replace(ddl.ID, "option", ""))
                OptionIndex = OptionIndex - 1
            End If

            If OptionIndex >= 0 Then
                aryList = objCtrl.GetOptionList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If aryList.Count >= (OptionIndex + 1) Then
                    Dim strID As String = "chk" & ddl.ID
                    Dim chk As CheckBox = CType(container.FindControl(strID), CheckBox)
                    If Not chk Is Nothing Then chk.Visible = False

                    objOInfo = CType(aryList(OptionIndex), NB_Store_OptionInfo)

                    aryList = objCtrl.GetOptionValueListWithInterface(objOInfo.OptionID, objOInfo.Lang)

                    If aryList.Count = 1 Then
                        ddl.Visible = False
                        Dim OptionValueInfo As NB_Store_OptionValueInfo = CType(aryList(0), NB_Store_OptionValueInfo)
                        If Not chk Is Nothing Then
                            strDesc = OptionValueInfo.OptionValueDesc
                            If OptionValueInfo.AddedCost > 0 Then
                                strDesc &= " +" & FormatToStoreCurrency(OptionValueInfo.AddedCost)
                            End If
                            If OptionValueInfo.AddedCost < 0 Then
                                strDesc &= " -" & FormatToStoreCurrency(OptionValueInfo.AddedCost)
                            End If

                            chk.Text = OptionValueInfo.OptionValueDesc
                            chk.Attributes.Add("OptionValueID", OptionValueInfo.OptionValueID)
                            chk.Visible = True
                            chk.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
                        End If

                    Else
                        If aryList.Count > 1 Then
                            Dim li As ListItem
                            For Each objOVInfo In aryList
                                li = New ListItem
                                strDesc = objOVInfo.OptionValueDesc
                                If objOVInfo.AddedCost > 0 Then
                                    strDesc &= " +" & FormatToStoreCurrency(objOVInfo.AddedCost)
                                End If
                                If objOVInfo.AddedCost < 0 Then
                                    strDesc &= " -" & FormatToStoreCurrency(objOVInfo.AddedCost)
                                End If
                                li.Text = strDesc
                                li.Value = objOVInfo.OptionValueID
                                ddl.Items.Add(li)
                            Next
                            ddl.SelectedIndex = 0
                            ddl.Visible = True
                            ddl.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
                        End If
                    End If
                End If
            End If

        End Sub

        Private Sub OptionText_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim txt As TextBox = Nothing
            Dim objCtrl As New ProductController
            Dim OptionIndex As Integer = -1
            Dim aryList As ArrayList
            Dim container As DataListItem
            Dim objOInfo As NB_Store_OptionInfo

            txt = CType(sender, TextBox)
            txt.Visible = False
            container = CType(txt.NamingContainer, DataListItem)

            If IsNumeric(Replace(txt.ID, "optiontext", "")) Then
                OptionIndex = CInt(Replace(txt.ID, "optiontext", ""))
                OptionIndex = OptionIndex - 1
            End If

            If OptionIndex >= 0 Then
                aryList = objCtrl.GetOptionList(DataBinder.Eval(container.DataItem, "ProductID"), DataBinder.Eval(container.DataItem, "Lang"))
                If aryList.Count >= (OptionIndex + 1) Then
                    txt.Visible = True
                    objOInfo = CType(aryList(OptionIndex), NB_Store_OptionInfo)
                    aryList = objCtrl.GetOptionValueListWithInterface(objOInfo.OptionID, objOInfo.Lang)
                    If aryList.Count >= 1 Then
                        Dim OptionValueInfo As NB_Store_OptionValueInfo = CType(aryList(0), NB_Store_OptionValueInfo)
                        txt.Text = OptionValueInfo.OptionValueDesc
                    End If
                    If Not OptionInterface.Instance() Is Nothing Then
                        Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                        txt.Text = OptionInterface.Instance.GetTextboxDefault(PS.PortalId, objOInfo)
                    End If

                End If
            End If
            If txt.Visible Then
                txt.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
            End If

        End Sub

        Private Function ParseTemplateText(ByVal TemplText As String) As String()
            Dim strOUT As String()
            Dim ParamAry As Char() = {"[", "]"}

            strOUT = TemplText.Split(ParamAry)

            Return strOUT
        End Function

        Private Function assignProperties(ByVal TagString As String, ByVal obj As Object, Optional ByVal IdPrefix As String = "") As Object
            Dim TagAtt As String() = TagString.Split(":"c)
            obj.ID = IdPrefix & TagAtt(1).ToLower
            If TagAtt.GetUpperBound(0) >= 2 Then
                obj = assignByReflection(obj, TagAtt(2))
            End If
            Return obj
        End Function

        Private Function getTemplateXML(ByVal Container As DataListItem) As String
            Dim strXML As String = ""

            strXML = "<root>"

            Dim dn As String

            dn = "EditLink" : strXML &= "<" & dn & "><![CDATA[" & NavigateURL(_TabID, _EditCtrlKey, "mid=" & _ModuleID, "ProdID=" & DataBinder.Eval(Container.DataItem, "ProductID"), "RtnTab=" & _TabID.ToString, "PageIndex=" & _PageIndex.ToString, "CatID=" & _CatID.ToString) & "]]></" & dn & ">"
            dn = "TabID" : strXML &= "<" & dn & "><![CDATA[" & _TabID.ToString & "]]></" & dn & ">"
            dn = "PageIndex" : strXML &= "<" & dn & "><![CDATA[" & _PageIndex.ToString & "]]></" & dn & ">"
            dn = "ModulePath" : strXML &= "<" & dn & "><![CDATA[" & _StoreInstallPath & "]]></" & dn & ">"
            dn = "ModuleID" : strXML &= "<" & dn & "><![CDATA[" & _ModuleID.ToString & "]]></" & dn & ">"
            dn = "ThumbSize" : strXML &= "<" & dn & "><![CDATA[" & _ThumbSize.ToString & "]]></" & dn & ">"
            dn = "ModuleEdit" : strXML &= "<" & dn & "><![CDATA[" & _blnHasModuleEditPermissions.ToString & "]]></" & dn & ">"
            dn = "GallerySize" : strXML &= "<" & dn & "><![CDATA[" & _GalleryThumbSize.ToString & "]]></" & dn & ">"
            dn = "EditCtrlKey" : strXML &= "<" & dn & "><![CDATA[" & _EditCtrlKey & "]]></" & dn & ">"
            dn = "CatID" : strXML &= "<" & dn & "><![CDATA[" & _CatID.ToString & "]]></" & dn & ">"
            dn = "ProductID" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "PortalID" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "Featured" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "Archived" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "IsDeleted" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "CreatedByUser" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "CreatedDate" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "ModifiedDate" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "ProductRef" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "Lang" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "Description" : strXML &= "<" & dn & "><![CDATA[" & System.Web.HttpUtility.HtmlDecode(DataBinder.Eval(Container.DataItem, dn)) & "]]></" & dn & ">"
            dn = "Summary" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "ImageURL" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "ImageDESC" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "ImageID" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "FromPrice" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "ProductName" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "QtyRemaining" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"
            dn = "QtyStockSet" : strXML &= "<" & dn & "><![CDATA[" & DataBinder.Eval(Container.DataItem, dn) & "]]></" & dn & ">"

            strXML &= DataBinder.Eval(Container.DataItem, "XMLdata")

            'Add any related product data
            strXML &= "<related>"

            Dim ProdID As Integer = DataBinder.Eval(Container.DataItem, "ProductID")
            Dim PortalID As Integer = DataBinder.Eval(Container.DataItem, "PortalID")
            Dim objCtrl As New ProductController
            Dim objRInfo As NB_Store_ProductRelatedInfo
            Dim objPInfo As NB_Store_ProductsInfo
            Dim aryList As ArrayList
            Dim objExport As New Export

            aryList = objCtrl.GetProductRelatedList(PortalID, ProdID, GetCurrentCulture, -1, False)

            For Each objRInfo In aryList
                objPInfo = objCtrl.GetProduct(objRInfo.RelatedProductID, GetCurrentCulture)
                strXML &= "<item>"

                strXML &= objCtrl.getProductLinkXML(objPInfo, _ProductTabID, _CatID)

                Dim fPrice As Double = objCtrl.GetFromPrice(objPInfo.PortalID, objPInfo.ProductID)

                strXML &= "<fromprice>"
                strXML &= fPrice.ToString
                strXML &= "</fromprice>"

                strXML &= "<frompricecurrency>"
                strXML &= FormatToStoreCurrency(fPrice)
                strXML &= "</frompricecurrency>"

                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objRInfo)
                strXML &= objExport.GetProductXML(objRInfo.RelatedProductID, GetCurrentCulture)
                strXML &= "</item>"
            Next

            strXML &= "</related>"


            strXML &= "</root>"

            Return strXML
        End Function



#Region "Price Methods"



#Region "General"

        Private Function ReplacePriceTokens(ByVal container As DataListItem, ByVal strInput As String) As String
            Dim rtnStr As String = strInput
            Dim PriceType As String = ""
            rtnStr = System.Web.HttpUtility.HtmlDecode(Replace(rtnStr, "[TAG:DEALERPRICECURRENCY]", getFromDealerPrice(container, True)))
            rtnStr = Replace(rtnStr, "[TAG:DEALERPRICE]", getFromDealerPrice(container, False))
            rtnStr = System.Web.HttpUtility.HtmlDecode(Replace(rtnStr, "[TAG:FROMPRICECURRENCY]", getFromPrice(container, True)))
            rtnStr = Replace(rtnStr, "[TAG:FROMPRICE]", getFromPrice(container, False))
            rtnStr = System.Web.HttpUtility.HtmlDecode(Replace(rtnStr, "[TAG:SALEPRICECURRENCY]", getSalePrice(container, True)))
            rtnStr = Replace(rtnStr, "[TAG:SALEPRICE]", getSalePrice(container, False))
            rtnStr = System.Web.HttpUtility.HtmlDecode(Replace(rtnStr, "[TAG:BESTPRICECURRENCY]", getBestPrice(container, True, PriceType)))
            rtnStr = Replace(rtnStr, "[TAG:BESTPRICE]", getBestPrice(container, False, PriceType))
            Return rtnStr
        End Function

#End Region

#Region "From Price Method"


        Private Function getFromPrice(ByVal container As DataListItem, ByVal FormatAsCurrency As Boolean) As String
            Dim fromPrice As Double = DataBinder.Eval(container.DataItem, "FromPrice")
            If fromPrice > 0 Or _ZeroPriceMsg = "" Then
                If FormatAsCurrency Then
                    Return FormatToStoreCurrency(fromPrice)
                Else
                    Return Format(fromPrice, "0.00")
                End If
            Else
                Return _ZeroPriceMsg
            End If
        End Function

        Private Function getFromPriceOutput(ByVal container As DataListItem, ByVal FormatAsCurrency As Boolean) As String

            Dim rtnStr As String
            Dim strCacheKey As String = ""
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings

            Dim objSTCtrl As New SettingsController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")

            objSTInfo = objSTCtrl.GetSettingsText(PS.PortalId, "fromprice.template", Lang)
            If objSTInfo Is Nothing Then
                rtnStr = getFromPrice(container, FormatAsCurrency)
            Else
                rtnStr = ReplacePriceTokens(container, objSTInfo.SettingText)
            End If

            Return rtnStr
        End Function

#End Region


#Region "Sale Price Method"

        Private Function getSalePrice(ByVal container As DataListItem, ByVal FormatAsCurrency As Boolean) As String
            Dim rtnStr As String
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim strCacheKey As String = ""

            'cache this for speed
            strCacheKey = GetCacheKey("getSalePrice_" & FormatAsCurrency.ToString & _UserInfo.UserID.ToString, PS.PortalId, DataBinder.Eval(container.DataItem, "ProductID"))
            'Don't use cache for users, so role promo works.
            If (DataCache.GetCache(strCacheKey) Is Nothing And strCacheKey <> "") Or GetStoreSettingBoolean(PS.PortalId, "debug.mode", "None") Or _UserInfo.UserID >= 0 Then
                Dim SalePrice As Double = 0
                Dim ModelPrice As Double
                Dim objCCtrl As New CartController
                Dim objPCtrl As New ProductController
                Dim objPromoCtrl As New PromoController
                Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))
                Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")
                Dim aryList As ArrayList

                aryList = objPCtrl.GetModelList(PS.PortalId, ProdID, Lang, IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))

                ModelPrice = 9999999
                For Each objInfo As NB_Store_ModelInfo In aryList
                    If objInfo.UnitCost < ModelPrice Then
                        ModelPrice = objInfo.UnitCost
                        SalePrice = objPromoCtrl.GetSalePrice(objInfo, _UserInfo)
                    End If
                Next
                If SalePrice <= 0 Or ModelPrice <= 0 Then
                    rtnStr = ""
                Else
                    If FormatAsCurrency Then
                        rtnStr = FormatToStoreCurrency(SalePrice)
                    Else
                        rtnStr = Format(SalePrice, "0.00")
                    End If
                End If

                If _UserInfo.UserID = -1 Then
                    'only set cache for non-users
                    DataCache.SetCache(strCacheKey, rtnStr, DateAdd(DateInterval.Day, 1, Now))
                End If
            Else
                rtnStr = DataCache.GetCache(strCacheKey)
            End If

            Return rtnStr
        End Function

        Private Function getSalePriceOutput(ByVal container As DataListItem, ByVal FormatAsCurrency As Boolean) As String

            Dim rtnStr As String
            Dim strCacheKey As String = ""
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings

            'cache this for speed
            strCacheKey = GetCacheKey("getSalePriceOutput_" & FormatAsCurrency.ToString, PS.PortalId, DataBinder.Eval(container.DataItem, "ProductID"))
            'Don't use cache for users, so role promo works.
            If (DataCache.GetCache(strCacheKey) Is Nothing And strCacheKey <> "") Or GetStoreSettingBoolean(PS.PortalId, "debug.mode", "None") Or _UserInfo.UserID >= 0 Then

                Dim objSTCtrl As New SettingsController
                Dim objSTInfo As NB_Store_SettingsTextInfo
                Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")

                rtnStr = getSalePrice(container, FormatAsCurrency)

                If rtnStr <> "" Then
                    objSTInfo = objSTCtrl.GetSettingsText(PS.PortalId, "saleprice.template", Lang)
                    If Not objSTInfo Is Nothing Then
                        rtnStr = ReplacePriceTokens(container, objSTInfo.SettingText)
                    End If
                End If

                If _UserInfo.UserID = -1 Then
                    'only set cache for non-users
                    DataCache.SetCache(strCacheKey, rtnStr, DateAdd(DateInterval.Day, 1, Now))
                End If
            Else
                rtnStr = DataCache.GetCache(strCacheKey)
            End If

            Return rtnStr
        End Function

#End Region

#Region "Best Price Method"

        Private Function getBestPriceOutput(ByVal container As DataListItem, ByVal FormatAsCurrency As Boolean) As String
            Dim rtnStr As String
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")

            Dim strCacheKey As String = ""
            Dim strText As String = ""
            Dim PriceType As String = ""

            strText = GetStoreSettingText(PS.PortalId, "bestprice.template", Lang)
            If strText = "" Then
                rtnStr = getBestPrice(container, FormatAsCurrency, PriceType)
                Select Case PriceType
                    Case "FROM"
                        rtnStr = getFromPriceOutput(container, FormatAsCurrency)
                    Case "SALE"
                        rtnStr = getSalePriceOutput(container, FormatAsCurrency)
                    Case "DEALER"
                        rtnStr = getFromDealerPriceOutput(container, FormatAsCurrency)
                End Select
            Else
                rtnStr = ReplacePriceTokens(container, strText)
            End If

            Return rtnStr
        End Function

        Private Function getBestPrice(ByVal container As DataListItem, ByVal FormatAsCurrency As Boolean, ByRef PriceType As String) As String
            Dim rtnStr As String
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")
            Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))

            If IsDealer(PS.PortalId, _UserInfo, Lang) Then
                rtnStr = getFromDealerPrice(container, FormatAsCurrency)
                PriceType = "DEALER"
            Else
                If getSalePrice(container, False) = "" Then
                    rtnStr = getFromPrice(container, FormatAsCurrency)
                    PriceType = "FROM"
                Else
                    rtnStr = getSalePrice(container, FormatAsCurrency)
                    PriceType = "SALE"
                End If
            End If

            Return rtnStr
        End Function

#End Region

#Region "Dealer Method"

        Private Function getFromDealerPriceOutput(ByVal container As DataListItem, ByVal FormatAsCurrency As Boolean) As String
            Dim rtnStr As String
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")

            If IsDealer(PS.PortalId, _UserInfo, Lang) Then
                Dim strCacheKey As String = ""
                Dim objSTCtrl As New SettingsController
                Dim objSTInfo As NB_Store_SettingsTextInfo

                objSTInfo = objSTCtrl.GetSettingsText(PS.PortalId, "dealerprice.template", Lang)
                If objSTInfo Is Nothing Then
                    rtnStr = getFromDealerPrice(container, FormatAsCurrency)
                Else
                    rtnStr = ReplacePriceTokens(container, objSTInfo.SettingText)
                End If
            Else
                rtnStr = ""
            End If

            Return rtnStr
        End Function

        Private Function getFromDealerPrice(ByVal container As DataListItem, ByVal FormatAsCurrency As Boolean) As String
            Dim objPCtrl As New ProductController
            Dim aryList As ArrayList
            Dim rtnPrice As Double = -1
            Dim DealerPrice As Double = 0
            Dim rtnStr As String
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim strCacheKey As String = ""
            Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")
            Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))

            'cache this for speed
            strCacheKey = GetCacheKey("getFromDealerPrice_" & FormatAsCurrency.ToString, PS.PortalId, DataBinder.Eval(container.DataItem, "ProductID"))
            If (DataCache.GetCache(strCacheKey) Is Nothing And strCacheKey <> "") Or GetStoreSettingBoolean(PS.PortalId, "debug.mode", "None") Then

                aryList = objPCtrl.GetModelList(PS.PortalId, ProdID, Lang, IsDealer(PS.PortalId, _UserInfo, GetCurrentCulture))
                For Each objMInfo As NB_Store_ModelInfo In aryList

                    If objMInfo.DealerCost < rtnPrice Then
                        rtnPrice = objMInfo.DealerCost
                    Else
                        If rtnPrice = -1 Then
                            rtnPrice = objMInfo.DealerCost
                        End If
                    End If
                Next

                If FormatAsCurrency Then
                    rtnStr = FormatToStoreCurrency(rtnPrice)
                Else
                    rtnStr = Format(rtnPrice, "0.00")
                End If
            Else
                rtnStr = DataCache.GetCache(strCacheKey)
            End If

            Return rtnStr
        End Function

#End Region

#End Region

    End Class

End Namespace
