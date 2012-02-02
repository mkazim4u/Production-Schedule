<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_StockProducts.ascx.vb"
    Inherits="SNR_StockProducts" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style type="text/css">
    div.RadGrid_Vista, div.RadGrid_Vista .rgMasterTable, div.RadGrid_Vista .rgDetailTable, div.RadGrid_Vista .rgGroupPanel table, div.RadGrid_Vista .rgCommandRow table, div.RadGrid_Vista .rgEditForm table, div.RadGrid_Vista .rgPager table, span.GridToolTip_Vista
    {
        font-family: Verdana;
        font-size: xx-small;
    }
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
<telerik:RadFormDecorator ID="rfd" runat="server" DecoratedControls="All" />
<table id="tblValidation" cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td>
            <asp:ValidationSummary ID="vs" ValidationGroup="vg" runat="server" />
        </td>
    </tr>
</table>
<table id="tblFilter" cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td>
            Select Product Type
        </td>
        <td>
            <telerik:RadComboBox ID="rcbProductsType" Height="200px" runat="server" AutoPostBack="true"
                Width="200px" Skin="Vista">
            </telerik:RadComboBox>
        </td>
        <td align="right">
            <asp:Button ID="btnUpdateUKProducts" runat="server" Text="Update UK Products" />
            <asp:Button ID="btnUpdateUSProducts" runat="server" Text="Update US Products" />
            <asp:Button ID="btnUpdateMEProducts" runat="server" Text="Update ME Products" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td width="15%">
            Select Category
        </td>
        <td>
            <telerik:RadComboBox ID="rcbCategory" Height="200px" runat="server" DataTextField="CategoryName"
                ValidationGroup="vg" Width="200px" Skin="Vista" DataValueField="CategoryID">
            </telerik:RadComboBox>
            <asp:RequiredFieldValidator runat="server" ID="rfvCateogry" ValidationGroup="vg"
                ControlToValidate="rcbCategory" Display="None" ErrorMessage="Please Select Category"
                InitialValue="- Select Category -" />
        </td>
    </tr>
</table>
<br />
<table id="tblstcProducts" cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td>
            <telerik:RadGrid ID="rgStockProducts" runat="server" AutoGenerateColumns="False"
                Font-Names="Verdana" Skin="Vista" AllowSorting="false" AllowPaging="True" PageSize="10"
                GridLines="None" ShowGroupPanel="false">
                <MasterTableView AllowMultiColumnSorting="false" Name="StockProducts" DataKeyNames="SprintProductKey"
                    AlternatingItemStyle-BackColor="#F2F2F2">
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkExportProduct" runat="server" AutoPostBack="true" OnCheckedChanged="chkExportProduct_CheckedChanged"
                                    CausesValidation="true" ValidationGroup="vg" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="SprintProductKey" HeaderText="ID" SortExpression="SprintProductKey"
                            HeaderStyle-HorizontalAlign="Center" UniqueName="SprintProductKey">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Product Code" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblProductCode" Text='<%# Bind("ProductCode") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Category" HeaderText="Category" SortExpression="Category"
                            ReadOnly="true" UniqueName="Category" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SubCategory" HeaderText="Sub Category" SortExpression="SubCategory"
                            ReadOnly="true" UniqueName="SubCategory" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Price" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblPrice" Text='<%# Bind("Price") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblQuantity" Text='<%# Bind("QuantityAvailable") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Image" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgProduct" runat="server" ImageAlign="AbsBottom" Width="50px" Height="50px" />
                                <telerik:RadToolTip ID="RadToolTipJobStatus" runat="server" ManualClose="false" TargetControlID="imgProduct"
                                    IsClientID="false" ShowEvent="OnMouseOver" RelativeTo="Element" HideDelay="4000"
                                    HideEvent="LeaveTargetAndToolTip" ShowCallout="true" Position="TopRight" Width="400px"
                                    Height="400px">
                                    <asp:Image ID="imgThubnailView" runat="server" ImageAlign="AbsBottom" Width="400px"
                                        Height="400px" />
                                </telerik:RadToolTip>
                                <asp:HiddenField ID="hidSprintProductKey" runat="server" Value='<%# Bind("SprintProductKey")%>' />
                                <asp:HiddenField ID="hidImage" runat="server" Value='<%# Bind("Image")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </td>
    </tr>
</table>
