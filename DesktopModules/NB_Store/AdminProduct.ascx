<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminProduct.ascx.vb"
    Inherits="NEvoWeb.Modules.NB_Store.AdminProduct" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="nbs" TagName="ProdList" Src="AdminProductList.ascx" %>
<%@ Register TagPrefix="nbs" TagName="ProdDetail" Src="AdminProductDetail.ascx" %>
<%@ Register TagPrefix="nbs" TagName="ProdImage" Src="AdminProductImage.ascx" %>
<%@ Register TagPrefix="nbs" TagName="ProdDoc" Src="AdminProductDoc.ascx" %>
<%@ Register TagPrefix="nwb" TagName="SelectLang" Src="controls/SelectLang.ascx" %>
<%@ Register TagPrefix="nwb" TagName="ShowSelectLang" Src="controls/ShowSelectLang.ascx" %>
<%@ Register TagPrefix="dnn" TagName="DualList" Src="~/controls/DualListControl.ascx" %>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlList" runat="server">
    <nbs:ProdList id="productlist" runat="server"></nbs:ProdList>
</asp:Panel>
<asp:PlaceHolder ID="plhMsg" runat="server"></asp:PlaceHolder>
<asp:Panel ID="pnlLang" runat="server">
    <nwb:SelectLang ID="selectlang" runat="server"></nwb:SelectLang>
    <div class="NBright_ButtonDiv">
        <asp:LinkButton CssClass="NBright_SaveButton" ID="cmdUpdate" runat="server" Text="Save"></asp:LinkButton>&nbsp;
        <asp:LinkButton CssClass="NBright_CancelButton" ID="cmdCancel" resourcekey="cmdCancel"
            runat="server" Text="Cancel" CausesValidation="False"></asp:LinkButton>
    </div>
</asp:Panel>
<asp:Panel ID="pnlProductTabs" runat="server">
    <asp:PlaceHolder ID="phPadding" runat="server"></asp:PlaceHolder>
    <div id="tabs" runat="server" class="NBright_tabs">
            <a id="buttontabs-1" href="#"><asp:Label ID="lblGeneral" runat="server" Text="General" resourcekey="lblGeneral"></asp:Label></a>
            <a id="buttontabs-2" href="#"><asp:Label ID="lblCategories" runat="server" Text="Categories" resourcekey="lblCategories"></asp:Label></a>
            <a id="buttontabs-3" href="#"><asp:Label ID="lblModels" runat="server" Text="Models" resourcekey="lblModels"></asp:Label></a>
            <a id="buttontabs-4" href="#"><asp:Label ID="lblImages" runat="server" Text="Images" resourcekey="lblImages"></asp:Label></a>
            <a id="buttontabs-5" href="#"><asp:Label ID="lblDocuments" runat="server" Text="Documents" resourcekey="lblDocuments"></asp:Label></a>
            <a id="buttontabs-6" href="#"><asp:Label ID="lblOptions" runat="server" Text="Options" resourcekey="lblOptions"></asp:Label></a>
            <a id="buttontabs-7" href="#"><asp:Label ID="lblRelatedProducts" runat="server" Text="Related Products" resourcekey="lblRelatedProducts"></asp:Label></a>
            <a id="buttontabs-8" href="#"><asp:Label ID="lblAll" runat="server" Text="All" resourcekey="lblAll"></asp:Label></a>
    </div>
        <div id="tabs-1">
            <asp:Panel ID="pnlProduct" runat="server">
                <nbs:ProdDetail id="productdetail" runat="server"></nbs:ProdDetail>
            </asp:Panel>
        </div>
        <div id="tabs-2">
            <asp:Panel ID="pnlCategories" runat="server">
            <dnn:label id="labelCategory" runat="server" controlname="labelCategory" suffix=":"></dnn:label>
            <dnn:DualList id="dlCategories" runat="server" ListBoxWidth="450" ListBoxHeight="300" DataValueField="Value" DataTextField="Text" />
            </asp:Panel>
        </div>
        <div id="tabs-3">
            <asp:Panel ID="pnlModel" runat="server">
                <div class="NBright_ButtonDiv">
                    <asp:LinkButton ID="cmdAddModel" runat="server" CssClass="NBright_AddButton" resourcekey="cmdAdd"
                        Text="Add"></asp:LinkButton>&nbsp;
                    <asp:TextBox ID="txtAddModels" runat="server" Width="20" MaxLength="2" CssClass="NormalTextBox"
                        Text="1"></asp:TextBox>
                    &nbsp;<asp:Label runat="server" resourcekey="cmdAddModel" Text="Label">Models</asp:Label>
                </div>
                <asp:DataGrid ID="dgModel" runat="server" AutoGenerateColumns="False" Width="100%"
                    CellPadding="1" GridLines="None">
                    <HeaderStyle CssClass="NBright_HeaderStyle" />
                    <FooterStyle CssClass="NBright_FooterStyle" />
                    <EditItemStyle CssClass="NBright_EditItemStyle" />
                    <SelectedItemStyle CssClass="NBright_SelectedItemStyle" />
                    <PagerStyle CssClass="NBright_PagerStyle" Mode="NumericPages" />
                    <AlternatingItemStyle CssClass="NBright_AlternatingItemStyle" />
                    <ItemStyle CssClass="NBright_ItemStyle" />
                    <Columns>
                        <asp:BoundColumn DataField="ModelID" HeaderText="ID" Visible="false"></asp:BoundColumn>
                        <dnntc:ImageCommandColumn KeyField="ModelID" ShowImage="True" ImageURL="~/images/delete.gif"
                            CommandName="Delete" EditMode="Command"><headerstyle font-size="10pt" font-names="Tahoma, Verdana, Arial" font-bold="True"
                                horizontalalign="Center"></headerstyle><edititemtemplate></edititemtemplate><itemstyle horizontalalign="Center"></itemstyle><headertemplate></headertemplate><itemtemplate></itemtemplate></dnntc:ImageCommandColumn>
                        <asp:TemplateColumn HeaderText="Ref">
                            <ItemTemplate>
                                <asp:TextBox ID="txtModelRef" runat="server" Width="80" MaxLength="20" Text='<%# DataBinder.Eval(Container, "DataItem.ModelRef") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Name">
                            <HeaderTemplate>
                                <nwb:ShowSelectLang ID="ShowSelectLang" runat="server"></nwb:ShowSelectLang>
                                <asp:Label ID="nlName" runat="server" Text="Name : "></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtModelName" runat="server" Width="250" MaxLength="50" CssClass="textexp_name"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.ModelName") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Extra">
                            <HeaderTemplate>
                                <nwb:ShowSelectLang ID="ShowSelectLang2" runat="server"></nwb:ShowSelectLang>
                                <asp:Label ID="nlExtra" runat="server" Text="Extra : "></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="txtExtra" runat="server" Width="50" MaxLength="50" CssClass="textexp_extra"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.Extra") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="BarCode">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBarCode" runat="server" Width="50" MaxLength="20" CssClass="textexp_code"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.BarCode") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Weight">
                            <ItemTemplate>
                                <asp:TextBox ID="txtWeight" runat="server" Width="50" MaxLength="7" Text='<%# DataBinder.Eval(Container, "DataItem.Weight") %>'></asp:TextBox>
                                <asp:CompareValidator ID="validatorWeight" runat="server" ErrorMessage="Error! Please enter a valid weight."
                                    resourcekey="validatorWeight" Type="Double" ControlToValidate="txtWeight" Operator="DataTypeCheck"
                                    Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="UnitCost">
                            <ItemTemplate>
                                <asp:TextBox ID="txtUnitCost" runat="server" Width="50" MaxLength="15" Text='<%# DataBinder.Eval(Container, "DataItem.UnitCost") %>'></asp:TextBox>
                                <asp:CompareValidator ID="validatorUnitPrice" runat="server" ErrorMessage="Error! Please enter a valid price."
                                    resourcekey="validatorUnitPrice" Type="Double" ControlToValidate="txtUnitCost"
                                    Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Dealer">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDealerCost" runat="server" Width="50" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.DealerCost") %>'></asp:TextBox>
                                <asp:CompareValidator ID="validatorDealerCost" runat="server" ErrorMessage="Error! Please enter a valid price."
                                    resourcekey="validatorDealerCost" Type="Double" ControlToValidate="txtDealerCost"
                                    Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="DealerOnly">
                            <HeaderTemplate>
                                <dnn:label id="labelDealerOnly" runat="server" controlname="labelDealerOnly" suffix="">
                                </dnn:label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDealerOnly" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "DealerOnly") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Cost">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPurchaseCost" runat="server" Width="50" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.PurchaseCost") %>'></asp:TextBox>
                                <asp:CompareValidator ID="validatorPurchaseCost" runat="server" ErrorMessage="Error! Please enter a valid price."
                                    resourcekey="validatorPurchaseCost" Type="Double" ControlToValidate="txtPurchaseCost"
                                    Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Stock - Max">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQtyRemaining" runat="server" Width="30" MaxLength="6" Text='<%# DataBinder.Eval(Container, "DataItem.QtyRemaining") %>'></asp:TextBox><asp:CompareValidator ID="revQtyRemaining" runat="server" ControlToValidate="txtQtyRemaining" Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
                                -
                                <asp:TextBox ID="txtMaxStock" runat="server" Width="30" MaxLength="6" Text='<%# DataBinder.Eval(Container, "DataItem.QtyStockSet") %>'></asp:TextBox><asp:CompareValidator ID="revMaxStock" runat="server" ControlToValidate="txtMaxStock" Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Allow">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAllow" runat="server" Width="30" MaxLength="6" Text='<%# DataBinder.Eval(Container, "DataItem.Allow") %>'></asp:TextBox><asp:CompareValidator ID="revAllow" runat="server" ControlToValidate="txtAllow" Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="ListOrder">
                            <ItemTemplate>
                                <asp:TextBox ID="txtListOrder" runat="server" Width="30" MaxLength="3" Text='<%# DataBinder.Eval(Container, "DataItem.ListOrder") %>'></asp:TextBox>
                                <asp:CompareValidator ID="revListOrder" runat="server" ControlToValidate="txtListOrder" Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </asp:Panel>
        </div>
        <div id="tabs-4">
            <asp:Panel ID="pnlImageCtrl" runat="server">
                <nbs:ProdImage id="productimage" runat="server"></nbs:ProdImage>
            </asp:Panel>
        </div>
        <div id="tabs-5">
            <asp:Panel ID="pnlDocCtrl" runat="server">
                <nbs:ProdDoc id="productdoc" runat="server"></nbs:ProdDoc>
            </asp:Panel>
        </div>
        <div id="tabs-6">
            <asp:Panel ID="pnlOptions" runat="server">
                <table id="tbloptions" runat="server" cellpadding="1" width="100%">
                    <tr>
                        <td valign="top" width="50%">
                            <div>
                                <p class="NormalBold">
                                    <asp:LinkButton ID="cmdAddOption" runat="server" CssClass="NBright_CommandButton"
                                        resourcekey="cmdAdd" Text="Add"></asp:LinkButton>&nbsp;
                                    <asp:TextBox ID="txtAddOption" runat="server" Width="20" MaxLength="2" CssClass="NormalTextBox"
                                        Text="1"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblAddOption" runat="server" resourcekey="cmdAddOption" Text="Label">Options</asp:Label>
                                </p>
                                <asp:DataGrid ID="dgOption" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CellPadding="1" GridLines="None">
                                    <HeaderStyle CssClass="NBright_HeaderStyle" />
                                    <FooterStyle CssClass="NBright_FooterStyle" />
                                    <EditItemStyle CssClass="NBright_EditItemStyle" />
                                    <SelectedItemStyle CssClass="NBright_SelectedItemStyle" />
                                    <PagerStyle CssClass="NBright_PagerStyle" Mode="NumericPages" />
                                    <AlternatingItemStyle CssClass="NBright_AlternatingItemStyle" />
                                    <ItemStyle CssClass="NBright_ItemStyle" />
                                    <Columns>
                                        <asp:BoundColumn DataField="OptionID" HeaderText="ID" Visible="false"></asp:BoundColumn>
                                        <dnntc:ImageCommandColumn KeyField="OptionID" ShowImage="True" ImageURL="~/images/edit.gif"
                                            CommandName="Edit" EditMode="Command"><headerstyle font-size="10pt" font-names="Tahoma, Verdana, Arial" font-bold="True"
                                                horizontalalign="Center"></headerstyle><edititemtemplate></edititemtemplate><itemstyle horizontalalign="Center"></itemstyle><headertemplate></headertemplate><itemtemplate></itemtemplate></dnntc:ImageCommandColumn>
                                        <dnntc:ImageCommandColumn KeyField="OptionID" ShowImage="True" ImageURL="~/images/delete.gif"
                                            CommandName="Delete" EditMode="Command"><headerstyle font-size="10pt" font-names="Tahoma, Verdana, Arial" font-bold="True"
                                                horizontalalign="Center"></headerstyle><edititemtemplate></edititemtemplate><itemstyle horizontalalign="Center"></itemstyle><headertemplate></headertemplate><itemtemplate></itemtemplate></dnntc:ImageCommandColumn>
                                        <asp:TemplateColumn HeaderText="Name">
                                            <HeaderTemplate>
                                                <asp:Label ID="nlName" runat="server" Text="Name."></asp:Label>
                                                <nwb:ShowSelectLang ID="ShowSelectLang" runat="server"></nwb:ShowSelectLang>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOptionDesc" runat="server" Width="200" MaxLength="50" Text='<%# DataBinder.Eval(Container, "DataItem.OptionDesc") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="ListOrder">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtListOrder" runat="server" Width="30" MaxLength="3" Text='<%# DataBinder.Eval(Container, "DataItem.ListOrder") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </td>
                        <td valign="top" width="50%">
                            <div id="optionvaluesdiv" runat="server">
                                <p class="NormalBold">
                                    <asp:LinkButton ID="cmdAddOptionValue" runat="server" CssClass="NBright_CommandButton"
                                        resourcekey="cmdAdd" Text="Add"></asp:LinkButton>&nbsp;
                                    <asp:TextBox ID="txtAddOptionValue" runat="server" Width="20" MaxLength="2" CssClass="NormalTextBox"
                                        Text="1"></asp:TextBox>
                                    &nbsp;<asp:Label ID="lblAddOptionValue" runat="server" resourcekey="cmdAddOptionValue"
                                        Text="Label">Options</asp:Label>
                                    <asp:Label ID="lblSelectedOptionID" runat="server" Text="" Visible="false"></asp:Label>
                                </p>
                                <asp:DataGrid ID="dgOptionEdit" runat="server" AutoGenerateColumns="False" Width="100%"
                                    CellPadding="1" GridLines="None">
                                    <HeaderStyle CssClass="NBright_HeaderStyle" />
                                    <FooterStyle CssClass="NBright_FooterStyle" />
                                    <EditItemStyle CssClass="NBright_EditItemStyle" />
                                    <SelectedItemStyle CssClass="NBright_SelectedItemStyle" />
                                    <PagerStyle CssClass="NBright_PagerStyle" Mode="NumericPages" />
                                    <AlternatingItemStyle CssClass="NBright_AlternatingItemStyle" />
                                    <ItemStyle CssClass="NBright_ItemStyle" />
                                    <Columns>
                                        <asp:BoundColumn DataField="OptionValueID" HeaderText="ID" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="OptionID" HeaderText="ID" Visible="false"></asp:BoundColumn>
                                        <dnntc:ImageCommandColumn KeyField="OptionValueID" ShowImage="True" ImageURL="~/images/delete.gif"
                                            CommandName="Delete" EditMode="Command"><headerstyle font-size="10pt" font-names="Tahoma, Verdana, Arial" font-bold="True"
                                                horizontalalign="Center"></headerstyle><edititemtemplate></edititemtemplate><itemstyle horizontalalign="Center"></itemstyle><headertemplate></headertemplate><itemtemplate></itemtemplate></dnntc:ImageCommandColumn>
                                        <asp:TemplateColumn HeaderText="Name">
                                            <HeaderTemplate>
                                                <asp:Label ID="nlName" runat="server" Text="Name."></asp:Label>
                                                <nwb:ShowSelectLang ID="ShowSelectLang" runat="server"></nwb:ShowSelectLang>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOptionValueDesc" runat="server" Width="150" MaxLength="50" Text='<%# DataBinder.Eval(Container, "DataItem.OptionValueDesc") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="AddedCost">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAddedCost" runat="server" Width="80" MaxLength="15" Text='<%# DataBinder.Eval(Container, "DataItem.AddedCost") %>'></asp:TextBox>
                                                <asp:CompareValidator ID="validatorUnitPrice2" runat="server" ErrorMessage="Error! Please enter a valid price."
                                                    resourcekey="validatorUnitPrice" Type="Double" ControlToValidate="txtAddedCost"
                                                    Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="ListOrder">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtListOrder" runat="server" Width="30" MaxLength="3" Text='<%# DataBinder.Eval(Container, "DataItem.ListOrder") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <div id="tabs-7">
            <asp:Panel ID="pnlRelated" runat="server">
                <div class="NBright_ButtonDiv">
                <asp:LinkButton ID="cmdAddSelected" runat="server" CssClass="NBright_CommandButton" Text="Add"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="cmdClearSelected" runat="server" CssClass="NBright_CommandButton" resourcekey="cmdClearSelected" Text="Clear"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="cmdSelectRelated" runat="server" CssClass="NBright_CommandButton" resourcekey="cmdSelectRelated" Text="Select"></asp:LinkButton>&nbsp;
                </div>
                <asp:DataGrid ID="dgRelated" runat="server" AutoGenerateColumns="False" Width="100%"
                    CellPadding="1" GridLines="None">
                    <HeaderStyle CssClass="NBright_HeaderStyle" />
                    <FooterStyle CssClass="NBright_FooterStyle" />
                    <EditItemStyle CssClass="NBright_EditItemStyle" />
                    <SelectedItemStyle CssClass="NBright_SelectedItemStyle" />
                    <PagerStyle CssClass="NBright_PagerStyle" Mode="NumericPages" />
                    <AlternatingItemStyle CssClass="NBright_AlternatingItemStyle" />
                    <ItemStyle CssClass="NBright_ItemStyle" />
                    <Columns>
                        <asp:BoundColumn DataField="RelatedID" HeaderText="ID" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="ProductID" HeaderText="ProductID" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="RelatedProductID" HeaderText="RelatedProductID" Visible="false"></asp:BoundColumn>
                        <dnntc:ImageCommandColumn KeyField="RelatedID" ShowImage="True" ImageURL="~/images/delete.gif"
                            CommandName="Delete" EditMode="Command"><headerstyle font-size="10pt" font-names="Tahoma, Verdana, Arial" font-bold="True"
                                horizontalalign="Center"></headerstyle><edititemtemplate></edititemtemplate><itemstyle horizontalalign="Center"></itemstyle><headertemplate></headertemplate><itemtemplate></itemtemplate></dnntc:ImageCommandColumn>
                        <asp:TemplateColumn HeaderText="Ref">
                            <ItemTemplate>
                                <asp:Label ID="lblRef" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RelatedProductRef") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Name">
                            <ItemTemplate>
                                 <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RelatedProductName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateColumn>


                        
                        <asp:TemplateColumn HeaderText="Amt" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDiscountAmt" runat="server" Width="60" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.DiscountAmt") %>'></asp:TextBox>
                                <asp:CompareValidator ID="validatorPurchaseCost" runat="server" ErrorMessage="*"
                                    resourcekey="validatorDiscountAmt" Type="Double" ControlToValidate="txtDiscountAmt"
                                    Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Per" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDiscountPercent" runat="server" Width="60" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.DiscountPercent") %>'></asp:TextBox>
                                                <asp:CompareValidator ID="revPercent" runat="server" 
                ControlToValidate="txtDiscountPercent" Display="Dynamic" ErrorMessage="*" 
                Operator="DataTypeCheck" Type="Double" ></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Qty" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProductQty" runat="server" Width="60" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.ProductQty") %>'></asp:TextBox>
                                <asp:CompareValidator ID="validatorProductQty" runat="server" ErrorMessage="*"
                                    resourcekey="validatorDiscountAmt" Type="Integer" ControlToValidate="txtProductQty"
                                    Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Max" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMaxQty" runat="server" Width="60" MaxLength="10" Text='<%# DataBinder.Eval(Container, "DataItem.MaxQty") %>'></asp:TextBox>
                                <asp:CompareValidator ID="validatorMaxQty" runat="server" ErrorMessage="*"
                                    resourcekey="validatorDiscountAmt" Type="Integer" ControlToValidate="txtMaxQty"
                                    Operator="DataTypeCheck" Display="Dynamic"></asp:CompareValidator>
                            </ItemTemplate>
                        </asp:TemplateColumn>

                        <asp:TemplateColumn HeaderText="Type">
                            <ItemTemplate>
                            <asp:DropDownList ID="ddlRelatedType" runat="server" DataValueField='<%# DataBinder.Eval(Container.DataItem, "RelatedType") %>'>
                                        <asp:ListItem Enabled="True" Text="Manual Link" Value="1" resourcekey="ManualLink" />
                                        <asp:ListItem Enabled="True" Text="Also Purchased" Value="2"  resourcekey="AlsoPurchased" />
                            </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        
                        
                        <asp:TemplateColumn HeaderText="BiDirectional">
                            <HeaderTemplate>
                                <dnn:label id="plBiDirectional" runat="server" controlname="plBiDirectional" suffix="" Text="BiDirection">
                                </dnn:label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkBiDirectional" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "BiDirectional") %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="Enabled">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkEnabled" runat="server" Checked='<%# not(DataBinder.Eval(Container.DataItem, "Disabled")) %>' />
                            </ItemTemplate>
                        </asp:TemplateColumn>

                    </Columns>
                </asp:DataGrid>
            </asp:Panel>



        </div>
</asp:Panel>
</td></tr>
</table>

