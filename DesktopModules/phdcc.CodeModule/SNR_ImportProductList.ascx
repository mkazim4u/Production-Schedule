<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SNR_ImportProductList.ascx.vb"
    Inherits="SNR_ImportProductList" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="DNN" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<fieldset id="fsManageCustomerAndPortal" runat="server">
    <telerik:RadAjaxLoadingPanel ID="alpImportProducts" runat="server" Height="75px"
        MinDisplayTime="5" Width="75px">
        <asp:Image ID="Image1" runat="server" AlternateText="Loading..." ImageUrl="~/Portals/0/Images/LoadingAjax.gif" />
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="rapImportProducts" RequestQueueSize="5" runat="server"
        Width="100%" EnableOutsideScripts="True" HorizontalAlign="NotSet" ScrollBars="None"
        LoadingPanelID="alpImportProducts">
        <legend>Manage Customer / Portals</legend>
        <br />
        <DNN:SectionHead runat="server" ID="shCustomerPortal" IsExpanded="false" Section="divCustomerPortalMapping"
            Text="Customer / Portal" IncludeRule="true" Visible="true" />
        <asp:ValidationSummary ID="vs" ValidationGroup="vgCP" runat="server" ForeColor="Red" />
        <div id="divCustomerPortalMapping" runat="server">
            <asp:Label ID="Label2" Text="Customer" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadComboBox ID="rcbCustomer" runat="server" AutoPostBack="true" Height="100px"
                OnSelectedIndexChanged="rcbCustomer_SelectedIndexChanged">
            </telerik:RadComboBox>
            <asp:RequiredFieldValidator ID="rfvrcbCustomer" runat="server" ErrorMessage="Please Select Customer"
                InitialValue="- Select Customer -" Display="None" ControlToValidate="rcbCustomer"
                ValidationGroup="vgCP" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            <asp:Label ID="lblPortal" Text="Portals" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadComboBox ID="rcbPortals" runat="server" OnSelectedIndexChanged="rcbPortals_SelectedIndexChanged">
            </telerik:RadComboBox>
            <asp:RequiredFieldValidator ID="rfvrcbPortals" runat="server" ErrorMessage="Please Select Shop"
                InitialValue="- Select Shop -" Display="None" ControlToValidate="rcbPortals"
                ValidationGroup="vgCP" />
            <br />
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="vgCP" />
            &nbsp;&nbsp;
            <asp:Button ID="btnCancel" runat="server" Text="Clear" CausesValidation="false" ValidationGroup="vgCP" />
            <br />
            <br />
            <asp:GridView ID="gvShop" runat="server" CellPadding="2" AutoGenerateColumns="false"
                ShowHeader="true" AllowSorting="true" Width="100%" AllowPaging="true" CssClass="gridviewSpacing">
                <Columns>
                    <asp:TemplateField ShowHeader="false">
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle Font-Bold="false" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnEdit" runat="server" OnClick="lnkbtnEdit_Click" ToolTip="Edit"
                                CommandArgument='<%# Container.DataItemIndex %>' CommandName="edit">
                                <asp:Image ID="imgEdit" runat="server" /></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="lnkbtnRemove" runat="server" OnClick="lnkbtnRemove_Click"
                                ToolTip="Delete" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this Shop ?');">
                                <asp:Image ID="imgDelete" runat="server" />
                            </asp:LinkButton>
                            <asp:HiddenField ID="hidID" runat="server" Value='<%# Container.DataItem("ID")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                                ToolTip="Update" Text="">
                                <asp:Image ID="imgUpdate" runat="server" /></asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                ToolTip="Cancel" Text="">
                                <asp:Image ID="imgCancel" runat="server" />
                            </asp:LinkButton>
                            <asp:HiddenField ID="hidID" runat="server" Value='<%# Container.DataItem("ID")%>' />
                            <%--<asp:HiddenField ID="hidName" runat="server" Value='<%# Container.DataItem("TeamName")%>' />--%>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer">
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle Font-Bold="false" />
                        <ItemTemplate>
                            <asp:Label ID="lblCustomer" Font-Bold="true" runat="server" Text='<%# Bind("Customer") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="gvrcbCustomer" runat="server" AutoPostBack="true" Height="100px">
                            </telerik:RadComboBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnShopName" runat="server" OnClick="lnkbtnShopName_Click">Portal</asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblShopName" runat="server" Text='<%# Bind("portal") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadComboBox ID="gvrcbPortals" runat="server" OnSelectedIndexChanged="gvrcbPortals_SelectedIndexChanged">
                            </telerik:RadComboBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField HeaderText="Team Name" DataField="RoleName" Visible="true" ReadOnly="true" />--%>
                    <%--                    <asp:TemplateField HeaderText="Created On" SortExpression="CreatedOn">
                        <HeaderTemplate>
                            <asp:LinkButton ID="lnkbtnCreatedColumn" runat="server" OnClick="lnkbtnCreatedColumn_Click">Created</asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCreatedOn" runat="server" Text='<%# FF_Globals.IsValidDate(Eval("CreatedOn")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <%--                                            <asp:BoundField HeaderText="Created On" DataField="CreatedOnDate" Visible="true"
                                                ReadOnly="true" DataFormatString="{0:d-MMM-yyyy}" />
                    --%>
                    <%--<asp:BoundField HeaderText="Created By" DataField="UserName" Visible="true" ReadOnly="true" />--%>
                    <%--<asp:Label ID="Created" Font-Bold="true" runat="server" Text='<%# Bind("CreatedOnDate") %>' />--%>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmptyMessage" runat="server" ForeColor="Red" Text="No Record Found"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
            <br />
            <br />
        </div>
        <DNN:SectionHead runat="server" ID="shStockNbStore" IsExpanded="false" Section="divImportProducts"
            Text="Stock / NBStore" IncludeRule="true" Visible="true" />
        <div id="divImportProducts" runat="server">
            <asp:Label ID="lblCustomer" Text="Customer" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadComboBox ID="rcbCustomerImport" runat="server" AutoPostBack="true" Height="100px"
                OnSelectedIndexChanged="rcbCustomerImport_SelectedIndexChanged">
            </telerik:RadComboBox>
            <br />
            <asp:Label ID="lblCategory" Text="Category" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadComboBox ID="rcbCategory" runat="server" AutoPostBack="true" Height="100px">
            </telerik:RadComboBox>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            <asp:Label ID="lblCustomerPortal" Text="" runat="server"></asp:Label>
            <%--    <asp:Label ID="Label1" Text="Portals" runat="server"></asp:Label>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <telerik:RadComboBox ID="rcbPortals" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rcbPortals_SelectedIndexChanged">
    </telerik:RadComboBox>
            --%>
            <br />
            <br />
            <asp:Label ID="lblStockProducts" Text="Stock Products" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblNBStoreProducts" Text="NBStore Products" runat="server"></asp:Label>
            <br />
            <br />
            <telerik:RadListBox runat="server" ID="rlbStockProducts" Height="200px" Width="200px"
                AllowTransferDuplicates="false" SelectionMode="Multiple" AllowTransfer="true"
                TransferToID="rlbNBStoreProducts">
            </telerik:RadListBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadListBox runat="server" ID="rlbNBStoreProducts" Height="200px" Width="200px"
                AllowDelete="true" TransferToID="rlbStockProducts">
            </telerik:RadListBox>
            <br />
            <br />
            <asp:Button ID="btnImportProducts" runat="server" Text="Import Products" />
        </div>
        <DNN:SectionHead runat="server" ID="shExportOrders" IsExpanded="false" Section="divExportOrders"
            Text="Export Orders" IncludeRule="true" Visible="true" />
        <div id="divExportOrders" runat="server">
            <asp:Button ID="btnExportOrders" runat="server" Text="Export Orders" CausesValidation="false" />
            <asp:GridView ID="gvOrders" runat="server" CellPadding="2" AutoGenerateColumns="false"
                AllowSorting="true" Width="100%" AllowPaging="true" CssClass="gridviewSpacing">
                <Columns>
                    <asp:TemplateField ShowHeader="false">
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle Font-Bold="false" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chkOrder" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Order Id" DataField="OrderId" />
                    <asp:BoundField HeaderText="Portal Id" DataField="PortalId" />
                    <asp:BoundField HeaderText="Order Number" DataField="OrderNumber" />
                    <asp:BoundField HeaderText="Order Status Id" DataField="OrderStatusId" />
                </Columns>
            </asp:GridView>
        </div>
        <DNN:SectionHead runat="server" ID="shCountryList" IsExpanded="false" Section="divCountryList"
            Text="Country List" IncludeRule="true" Visible="true" />
        <div id="divCountryList" runat="server">
            <asp:Button ID="btnImportCountry" runat="server" Text="Import Country" />
            <telerik:RadGrid ID="rgCountryList" runat="server" Width="95%" ShowStatusBar="true" Skin="Vista"
                EnableEmbeddedSkins="false" CssClass="gridviewSpacing" AutoGenerateColumns="False"
                PageSize="10" AllowSorting="True" AllowMultiRowSelection="False" AllowPaging="True"
                GridLines="Both">
                <PagerStyle Mode="NumericPages"></PagerStyle>
                <MasterTableView Width="100%" DataKeyNames="EntryID" AllowMultiColumnSorting="True" Name="Country">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="ID" DataField="EntryID" UniqueName="EntryID"
                            Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Country ID" DataField="Value" UniqueName="CountryID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Country Name" DataField="Text" UniqueName="CountryName">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
    </telerik:RadAjaxPanel>
</fieldset>
