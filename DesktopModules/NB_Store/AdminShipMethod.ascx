<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminShipMethod.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminShipMethod" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<table class="NBright_ContentDiv"><tr><td>
<div class="NBright_ButtonDiv">
<asp:linkbutton id="cmdNew" runat="server" resourcekey="cmdNew" cssclass="NBright_AddButton" >Add New</asp:linkbutton>&nbsp;
<asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" cssclass="NBright_SaveButton" >Update</asp:linkbutton>&nbsp;
</div>
	<p>
		<asp:datagrid id="dgList" runat="server" width="100%" gridlines="None" cellpadding="2" autogeneratecolumns="False">
            <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
            <Columns>
    			<dnntc:ImageCommandColumn KeyField="ShipMethodID" ShowImage="True" ImageURL="~/images/delete.gif" CommandName="Delete"
					EditMode="Command">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<HeaderTemplate></HeaderTemplate>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
                <asp:BoundColumn DataField="ShipMethodID" HeaderText="Key" Visible="False">
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="MethodName">
                    <ItemTemplate>
                        <asp:TextBox ID="txtMethodName" runat="server"  Width="100"  MaxLength="50"
                            Text='<%# DataBinder.Eval(Container, "DataItem.MethodName") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvMethodName" runat="server" ErrorMessage="*" ControlToValidate="txtMethodName"></asp:RequiredFieldValidator>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="MethodDesc">
                    <ItemTemplate>
                        <asp:TextBox ID="txtMethodDesc" runat="server"  Width="200"  MaxLength="256"
                            Text='<%# DataBinder.Eval(Container, "DataItem.MethodDesc") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="TemplateName">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlTemplateName" runat="server"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="URLTracker">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlTrackerTemplate" runat="server"></asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Disabled">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDisabled" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.Disabled") %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="SortOrder">
                    <ItemTemplate>
                        <asp:TextBox ID="txtSortOrder" runat="server"  Width="30"  MaxLength="3"
                            Text='<%# DataBinder.Eval(Container, "DataItem.SortOrder") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
				<dnntc:ImageCommandColumn KeyField="ShipMethodID" ShowImage="True" ImageURL="~/images/copy.gif" CommandName="Copy"
					EditMode="Command">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>                
            </Columns>
		</asp:datagrid>
	</p>
</td></tr>
</table>
