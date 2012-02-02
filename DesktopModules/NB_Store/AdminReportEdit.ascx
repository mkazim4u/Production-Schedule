<%@ Control language="vb"  Inherits="NEvoWeb.Modules.NB_Store.AdminReportEdit" AutoEventWireup="false"  Codebehind="AdminReportEdit.ascx.vb" %>
<%@ Register TagPrefix="dnntc" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="DotNetNuke" namespace="DotNetNuke.Security.Permissions.Controls" tagprefix="cc1" %>

<div class="NBright_ButtonDiv">
    <asp:linkbutton cssclass="NBright_SaveButton" id="cmdUpdate" resourcekey="cmdUpdate" runat="server" text="Update"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="NBright_CancelButton" id="cmdCancel" resourcekey="cmdCancel" runat="server" text="Cancel" causesvalidation="False"></asp:linkbutton>&nbsp;
    <asp:linkbutton cssclass="NBright_DeleteButton" id="cmdDelete" resourcekey="cmdDelete" runat="server" text="Delete" causesvalidation="False"></asp:linkbutton>&nbsp;
</div>
<table width="650" cellspacing="2" cellpadding="2" border="0" summary="Edit Table">
	<tr>
	<td  colspan="8"><dnn:label id="plReportName" runat="server" controlname="txtReportName" suffix=""></dnn:label><asp:TextBox ID="txtReportName" runat="server" Width="100%"  MaxLength="50"></asp:TextBox>
	</td>
	</tr>
	<tr>
	<td  colspan="8"><dnn:label id="plReportRef" runat="server" controlname="txtReportRef" suffix=""></dnn:label><asp:TextBox ID="txtReportRef" runat="server" Width="100px" MaxLength="20"></asp:TextBox>
	</td>
	</tr>

	<tr>
	<td colspan="8">
	<hr />
	<div class="NBright_ButtonDiv">
                <asp:LinkButton ID="cmdAddParam" runat="server" 
                    cssclass="NBright_AddButton" resourcekey="cmdAdd" text="Add"></asp:LinkButton>&nbsp;
<asp:TextBox ID="txtAddParams" Runat="server" Width="20" MaxLength="2" CssClass="NormalTextBox" Text="1"></asp:TextBox>
&nbsp;<asp:Label ID="Label1" runat="server" resourcekey="cmdAddParam" Text="Label">Params</asp:Label>
	</div>
    <asp:DataGrid id="dgParam" runat="server"  AutoGenerateColumns="False" 
            CellPadding="2" GridLines="None" PageSize="25" Width="100%" AllowPaging="True">
			  <HeaderStyle CssClass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditItemStyle cssclass="NBright_EditItemStyle" />
            <SelectedItemStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle" Mode="NumericPages"/>
			<AlternatingItemStyle cssclass="NBright_AlternatingItemStyle" />
            <ItemStyle cssclass="NBright_ItemStyle" />
			<Columns>
			<asp:BoundColumn DataField="ReportParamID" HeaderText="ID" Visible="false"></asp:BoundColumn>
				<dnntc:ImageCommandColumn KeyField="ReportParamID" ShowImage="True" ImageURL="~/images/delete.gif" CommandName="Delete"
					EditMode="Command">
					<HeaderStyle Font-Size="10pt" Font-Names="Tahoma, Verdana, Arial" Font-Bold="True" HorizontalAlign="Center"></HeaderStyle>
					<ItemStyle HorizontalAlign="Center"></ItemStyle>
					<HeaderTemplate></HeaderTemplate>
					<ItemTemplate></ItemTemplate>
				</dnntc:ImageCommandColumn>
			    <asp:TemplateColumn HeaderText="Name">
                    <ItemTemplate>
                        <asp:TextBox ID="txtParamName" runat="server" Width="250" MaxLength="20"
                            Text='<%# DataBinder.Eval(Container, "DataItem.ParamName") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Type">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlType" runat="server" 
                            SelectedValue='<%# DataBinder.Eval(Container, "DataItem.ParamType") %>'>
                            <asp:ListItem>nvarchar(50)</asp:ListItem>
                            <asp:ListItem>int</asp:ListItem>
                            <asp:ListItem>datetime</asp:ListItem>
                            <asp:ListItem>nvarchar(MAX)</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Source">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlSource" runat="server" 
                            SelectedValue='<%# DataBinder.Eval(Container, "DataItem.ParamSource") %>'>
                            <asp:ListItem Value="1">Static Value</asp:ListItem>
                            <asp:ListItem Value="2">QueryString Param</asp:ListItem>
                            <asp:ListItem Value="3">Form Post Param</asp:ListItem>
                            <asp:ListItem Value="4">Today</asp:ListItem>
                            <asp:ListItem Value="5">PortalID</asp:ListItem>
                            <asp:ListItem Value="6">UserID</asp:ListItem>
                            <asp:ListItem Value="7">Current Culture</asp:ListItem>
                            <asp:ListItem Value="8">Form Textbox</asp:ListItem>
                            <asp:ListItem Value="9">Form Date Input</asp:ListItem>
                            <asp:ListItem Value="10">Form DropDown</asp:ListItem>
                            <asp:ListItem Value="11">Form RadioButton</asp:ListItem>
                            <asp:ListItem Value="12">Form CheckBox</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Value">
                    <ItemTemplate>
                        <asp:TextBox ID="txtParamValue" runat="server" Width="250"  MaxLength="50"
                            Text='<%# DataBinder.Eval(Container, "DataItem.ParamValue") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
<hr />
	</td>
	</tr>
	<tr>
        <td  colspan="8"><dnn:label id="plSQLText" runat="server" controlname="txtSQLText" suffix=""></dnn:label>
            <asp:TextBox ID="txtSQLText" runat="server" Width="100%" Height="500px" TextMode="MultiLine"></asp:TextBox>
        </td>        
	</tr>
	<tr>
	<td align="right"><dnn:label id="plSchedulerFlag" runat="server" controlname="" suffix=""></dnn:label></td>
	<td><asp:CheckBox ID="chkSchedulerFlag" runat="server" /></td>
	<td align="right"><dnn:label id="plSchStartHour" runat="server" controlname="" suffix=""></dnn:label>
	</td>
	<td>
	<asp:DropDownList ID="ddlSchStartHour" runat="server">
            <asp:ListItem Value="0">00</asp:ListItem>
            <asp:ListItem Value="1">01</asp:ListItem>
            <asp:ListItem Value="2">02</asp:ListItem>
            <asp:ListItem Value="3">03</asp:ListItem>
            <asp:ListItem Value="4">04</asp:ListItem>
            <asp:ListItem Value="5">05</asp:ListItem>
            <asp:ListItem Value="6">06</asp:ListItem>
            <asp:ListItem Value="7">07</asp:ListItem>
            <asp:ListItem Value="8">08</asp:ListItem>
            <asp:ListItem Value="9">09</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
            <asp:ListItem>13</asp:ListItem>
            <asp:ListItem>14</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>16</asp:ListItem>
            <asp:ListItem>17</asp:ListItem>
            <asp:ListItem>18</asp:ListItem>
            <asp:ListItem>19</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>21</asp:ListItem>
            <asp:ListItem>22</asp:ListItem>
            <asp:ListItem>23</asp:ListItem>
        </asp:DropDownList>
        </td>
        <td align="right"><dnn:label id="plSchStartMins" runat="server" controlname="" suffix=""></dnn:label>
        </td>
        <td><asp:DropDownList ID="ddlSchStartMins" runat="server">
            <asp:ListItem>00</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
            <asp:ListItem>45</asp:ListItem>
        </asp:DropDownList>
        </td>
        <td align="right"><dnn:label id="plSchReRunMins" runat="server" controlname="" suffix=""></dnn:label>
        </td>
        <td><asp:DropDownList ID="ddlSchReRunMins" runat="server">
            <asp:ListItem Value="15">15 mins</asp:ListItem>
            <asp:ListItem Value="30">30 mins</asp:ListItem>
            <asp:ListItem Value="60">1 hour</asp:ListItem>
            <asp:ListItem Value="120">2 hours</asp:ListItem>
            <asp:ListItem Value="240">4 hours</asp:ListItem>
            <asp:ListItem Value="480">8 hours</asp:ListItem>
            <asp:ListItem Value="720">12 hours</asp:ListItem>
            <asp:ListItem Value="1440">1 Day</asp:ListItem>
            <asp:ListItem Value="10080">7 Days</asp:ListItem>
            <asp:ListItem Value="20160">14 Days</asp:ListItem>
            <asp:ListItem Value="43200">30 Days</asp:ListItem>
        </asp:DropDownList>
        </td>
	</tr>
	<tr>
	
	<td align="right"><dnn:label id="plEmailResults" runat="server" controlname="" suffix=""></dnn:label>
	</td>
	<td><asp:CheckBox ID="chkEmailResults" runat="server" />
	</td>
	<td align="right"><dnn:label id="plEmailFrom" runat="server" controlname="" suffix=""></dnn:label>
	</td>
	<td colspan="5"><asp:TextBox ID="txtEmailFrom" runat="server" Width="100%"></asp:TextBox>
	</td>
	</tr>
	<tr>
	<td></td>
	<td></td>	
	<td align="right"><dnn:label id="plEmailTo" runat="server" controlname="" suffix=""></dnn:label>
	</td>
	<td colspan="5"><asp:TextBox ID="txtEmailTo" runat="server"  Width="100%"></asp:TextBox>
	</td>	

	</tr>	
	<tr>
	<td  colspan="8"><dnn:label id="plReportTitle" runat="server" controlname="txtReportTitle" suffix=""></dnn:label><asp:TextBox ID="txtReportTitle" runat="server" Width="100%"  MaxLength="255"></asp:TextBox>
	</td>
	</tr>
	
	<tr>
	
	</tr>	
	<tr>
	<td align="right"><dnn:label id="plAllowExport" runat="server" controlname="" suffix=""></dnn:label>
	</td>
	<td><asp:CheckBox ID="chkAllowExport" runat="server" />
	</td>
	<td align="right"><dnn:label id="plAllowDisplay" runat="server" controlname="" suffix=""></dnn:label>
	</td>
	<td><asp:CheckBox ID="chkAllowDisplay" runat="server" />
	</td>
	<td align="right"><dnn:label id="plDisplayInLine" runat="server" controlname="" suffix=""></dnn:label>
	</td>
	<td><asp:CheckBox ID="chkDisplayInLine" runat="server" />
	</td>
	<td align="right"><dnn:label id="plShowSQL" runat="server" controlname="" suffix=""></dnn:label>
	</td>
	<td><asp:CheckBox ID="chkShowSQL" runat="server" />
	</td>
	</tr>
    <tr>
	<td>
    </td>
	<td colspan="7">
	<asp:TextBox ID="txtFieldDelimeter" runat="server" Width="15" MaxLength="1" Text=','></asp:TextBox>
    <asp:Label ID="lblFieldDelimeter" runat="server" Text="FieldDelimeter" resourcekey="lblFieldDelimeter"></asp:Label>
    </td>
    </tr>
    <tr>
	<td>
    </td>
	<td colspan="7">
	<asp:TextBox ID="txtFieldQualifier" runat="server" Width="15" MaxLength="1" Text=''></asp:TextBox>
    <asp:Label ID="lblFieldQualifier" runat="server" Text="FieldQualifier" resourcekey="lblFieldQualifier"></asp:Label>
    </td>
    </tr>
</table>
<PagerStyle HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
