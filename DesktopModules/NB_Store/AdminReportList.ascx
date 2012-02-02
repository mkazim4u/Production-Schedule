<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminAdminReportList.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminReportList" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<div class="NBright_ButtonDiv">
                <asp:LinkButton ID="cmdAdd" runat="server" 
                    cssclass="NBright_AddButton" resourcekey="cmdAdd" text="Add"></asp:LinkButton>
</div>
<div class="NBright_SelectDiv">
                <asp:TextBox ID="txtSearch" runat="server" Width="169px"></asp:TextBox>
                <asp:LinkButton ID="cmdSearch" cssclass="NBright_CommandButton" resourcekey="cmdSearch" runat="server">Search</asp:LinkButton>
</div>
    <asp:DataGrid id="dgReports" runat="server" AutoGenerateColumns="False" 
            CellPadding="2" GridLines="None" PageSize="25" Width="100%" AllowPaging="True">
			  <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
			<Columns>
				<dnntc:ImageCommandColumn KeyField="ReportID" headerText="" ShowImage="True" ImageURL="img/edit.png" CommandName="EditRep"
					EditMode="Command" >
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
			<asp:BoundColumn DataField="ReportID" HeaderText="ID" Visible="false"></asp:BoundColumn>
			<asp:BoundColumn DataField="ReportName" HeaderText="Name"></asp:BoundColumn>
			<asp:BoundColumn DataField="SchedulerFlag" HeaderText="Scheduled" Visible="false"></asp:BoundColumn>
				<dnntc:ImageCommandColumn KeyField="ReportID" ShowImage="True" headerText="" ImageURL="img/copy.png" CommandName="Copy"
					EditMode="Command">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
				<dnntc:ImageCommandColumn KeyField="ReportID" ShowImage="True" ImageURL="img/view.png" CommandName="View"
					EditMode="Command" headerText="View">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
				<dnntc:ImageCommandColumn KeyField="ReportID" ShowImage="True" ImageURL="img/save.png" CommandName="Export"
					EditMode="Command">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
				<dnntc:ImageCommandColumn KeyField="ReportID" ShowImage="True" ImageURL="img/email.png" CommandName="Email"
					EditMode="Command">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<EditItemTemplate></EditItemTemplate>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
			</Columns>
			<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
		</asp:DataGrid>