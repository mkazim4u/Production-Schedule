<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_StoreProducts.ascx.vb"
    Inherits="SNR_StoreProducts" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    .viewWrap
    {
        padding: 15px;
        background: #2291b5 0 0 url(Img/bluegradient.gif) repeat-x;
    }
    
    .contactWrap
    {
        padding: 10px 15px 15px 15px;
        background: #fff;
        color: #333;
    }
    
    .contactWrap td
    {
        padding: 0 20px 0 0;
    }
    
    .contactWrap td td
    {
        padding: 3px 20px 3px 0;
    }
    
    .contactWrap img
    {
        border: 1px solid #05679d;
    }
</style>
<script type="text/javascript">

    
</script>
<telerik:RadFormDecorator ID="rfd" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
<table id="tblStoreProducts" cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td width="15%" valign="top" align="left">
            <telerik:RadTreeView runat="server" ID="tvCategory" DataTextField="CategoryName"
                DataValueField="CategoryID">
            </telerik:RadTreeView>
        </td>
        <td width="80%">
            <telerik:RadListView ID="rlvProducts" runat="server" ItemPlaceholderID="ProductsContainer"
                PageSize="6" AllowPaging="true" DataKeyNames="ProductID">
                <LayoutTemplate>
                    <fieldset style="padding: 5px; width: 750px">
                        <legend>Products</legend>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Button ID="btnAddProduct" runat="server" CommandName="Insert" Visible="<%#Container.InsertItemPosition = RadListViewInsertItemPosition.None %>"
                                        Text="Add new product" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="ProductsContainer" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadDataPager ID="RadDataPager1" runat="server" PagedControlID="rlvProducts"
                                        PageSize="6">
                                        <Fields>
                                            <telerik:RadDataPagerButtonField FieldType="FirstPrev" />
                                            <telerik:RadDataPagerButtonField FieldType="Numeric" />
                                            <telerik:RadDataPagerButtonField FieldType="NextLast" />
                                            <telerik:RadDataPagerPageSizeField PageSizeText="Page size: " />
                                            <telerik:RadDataPagerTemplatePageField>
                                                <PagerTemplate>
                                                    <div style="float: right">
                                                        <b>Items
                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.Owner.StartRowIndex+1%>" />
                                                            to
                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# IIF(Container.Owner.TotalRowCount > (Container.Owner.StartRowIndex+Container.Owner.PageSize), Container.Owner.StartRowIndex+Container.Owner.PageSize, Container.Owner.TotalRowCount) %>" />
                                                            of
                                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.Owner.TotalRowCount%>" />
                                                            <br />
                                                        </b>
                                                    </div>
                                                </PagerTemplate>
                                            </telerik:RadDataPagerTemplatePageField>
                                        </Fields>
                                    </telerik:RadDataPager>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </LayoutTemplate>
                <ItemTemplate>
                    <fieldset style="float: left; width: 226px; height: 230px; padding: 5px; margin: 5px">
                        <legend style="padding: 5px;"><b>
                            <%#Eval("ProductName") %></b></legend>
                        <asp:Image ID="imgProduct" runat="server" ImageAlign="AbsBottom" Width="225px" Height="150px" />
                        <%--                        <telerik:RadToolTip ID="rttImage" runat="server" ManualClose="false" TargetControlID="imgProduct"
                            IsClientID="false" ShowEvent="OnMouseOver" RelativeTo="Element" HideDelay="4000"
                            HideEvent="LeaveTargetAndToolTip" ShowCallout="true" Position="TopRight" Width="200px"
                            Height="200px">
                            <asp:Image ID="imgThubnailView" runat="server" ImageAlign="AbsBottom" Width="200px"
                                Height="200px" />
                        </telerik:RadToolTip>--%>
                        <br />
                        <br />
                        <b>Price</b>
                        <asp:Label ID="lblPrice" Text='<%# Eval("UnitCost","{0:C2}") %>' runat="server"></asp:Label>
                        <br />
                        <b>Quantity</b>
                        <asp:Label ID="lblQuantity" Text='<%# Bind("QtyRemaining") %>' runat="server"></asp:Label>
                        <%--<b>Description</b>--%>
                        <%--<span style="font-weight: bold;">Description:</span>--%>
                        <%--<asp:Label ID="lblDescription" Text='<%# Bind("Description") %>' runat="server" Visible="false"></asp:Label>--%>
                        <div style="padding-bottom: 1px">
                            <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="True" CommandName="Edit"
                                CommandArgument='<%# Eval("ProductID")%>'>
                                <asp:Image ID="imgEdit" runat="server" ImageAlign="Right" />
                            </asp:LinkButton>
                        </div>
                        <asp:HiddenField ID="hidImageUrl" runat="server" Value='<%# Bind("ImagePath")%>' />
                        <asp:HiddenField ID="hidCategoryID" runat="server" Value='<%# Bind("CategoryID")%>' />
                        <asp:HiddenField ID="hidProductID" runat="server" Value='<%# Bind("ProductID")%>' />
                        <asp:HiddenField ID="hidProductRef" runat="server" Value='<%# Bind("ProductRef")%>' />
                    </fieldset>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <fieldset style="float: left; width: 226px; height: 230px; padding: 5px; margin: 5px">
                        <legend style="padding: 5px;"><b>
                            <%#Eval("ProductName") %></b></legend>
                        <asp:Image ID="imgProduct" runat="server" ImageAlign="AbsBottom" Width="225px" Height="150px" />
                        <%--                        <telerik:RadToolTip ID="rttImage" runat="server" ManualClose="false" TargetControlID="imgProduct"
                            IsClientID="false" ShowEvent="OnMouseOver" RelativeTo="Element" HideDelay="4000"
                            HideEvent="LeaveTargetAndToolTip" ShowCallout="true" Position="TopRight" Width="200px"
                            Height="200px">
                            <asp:Image ID="imgThubnailView" runat="server" ImageAlign="AbsBottom" Width="200px"
                                Height="200px" />
                        </telerik:RadToolTip>--%>
                        <br />
                        <br />
                        <b>Price</b>
                        <asp:Label ID="lblPrice" Text='<%# Eval("UnitCost","{0:C2}") %>' runat="server"></asp:Label>
                        <br />
                        <b>Quantity</b>
                        <asp:Label ID="lblQuantity" Text='<%# Bind("QtyRemaining") %>' runat="server"></asp:Label>
                        <%--<b>Description</b>--%>
                        <%--<span style="font-weight: bold;">Description:</span>--%>
                        <%--<asp:Label ID="lblDescription" Text='<%# Bind("Description") %>' runat="server" Visible="false"></asp:Label>--%>
                        <div style="padding-bottom: 1px">
                            <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="True" CommandName="Edit"
                                CommandArgument='<%# Eval("ProductID")%>'>
                                <asp:Image ID="imgEdit" runat="server" ImageAlign="Right" />
                            </asp:LinkButton>
                        </div>
                        <asp:HiddenField ID="hidImageUrl" runat="server" Value='<%# Bind("ImagePath")%>' />
                        <asp:HiddenField ID="hidCategoryID" runat="server" Value='<%# Bind("CategoryID")%>' />
                        <asp:HiddenField ID="hidProductID" runat="server" Value='<%# Bind("ProductID")%>' />
                        <asp:HiddenField ID="hidProductRef" runat="server" Value='<%# Bind("ProductRef")%>' />
                    </fieldset>
                </AlternatingItemTemplate>
            </telerik:RadListView>
        </td>
    </tr>
</table>
<telerik:RadWindow ID="rwEditProduct" runat="server" Title="Editing record" Height="450px"
    EnableShadow="true" OpenerElementID="lnkbtnEdit" Width="350px" ReloadOnShow="true"
    ShowContentDuringLoad="false" Behaviors="Move,Pin,Resize" Visible="false" DestroyOnClose="true">
    <ContentTemplate>
        <asp:Button ID="btnClose" runat="server" Text="Close Window" CausesValidation="False" />
        <table id="tblRw" cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td width="25%">
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Label ID="lblProductName" Text="Name" runat="server"></asp:Label>
                    <asp:HiddenField ID="hidProductID" runat="server" />
                    <asp:HiddenField ID="hidCategoryID" runat="server" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr id="trImage" runat="server" visible="true">
                <td colspan="2" align="center">
                    <asp:Image ID="imgProduct" runat="server" ImageAlign="AbsBottom" Width="100px" Height="100px" />
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblName" Text="Name" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtProductName" MaxLength="50" Width="150px" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPrice" Text="Price" runat="server"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="rntbPrice" MinValue="0" runat="server" MaxValue="9999"
                        Width="150px" Type="Currency" Culture="English (United Kingdom)">
                    </telerik:RadNumericTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblQuantity" Text="Quantity" runat="server"></asp:Label>
                </td>
                <td>
                    <telerik:RadNumericTextBox ID="rntbQuantity" MinValue="0" runat="server" MaxValue="9999"
                        IncrementSettings-InterceptMouseWheel="true" IncrementSettings-InterceptArrowKeys="true"
                        Width="150px" Type="Number" Culture="English (United Kingdom)">
                    </telerik:RadNumericTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblUploadImage" Text="Image" runat="server"></asp:Label>
                </td>
                <td>
                    <telerik:RadAsyncUpload runat="server" ID="rauImage" AllowedFileExtensions="jpg,jpeg,png,gif"
                        OnFileUploaded="rauImage_FileUploaded" Width="150px" MaxFileInputsCount="1" MultipleFileSelection="Disabled"
                        MaxFileSize="524288">
                    </telerik:RadAsyncUpload>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" />
                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="false"
                        CommandName="Cancel" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</telerik:RadWindow>
