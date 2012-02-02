<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_JobSummary.ascx.vb"
    Inherits="PS_JobSummary" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<link href="module.css" rel="stylesheet" type="text/css" />
<div style="overflow: auto; width: 100%;">
    <asp:DetailsView ID="dvJob" runat="server" AutoGenerateRows="False" DataKeyNames="Id"
        align="center" Width="100%">
        <HeaderTemplate>
            <asp:Label ID="lblJobDetail" Text="Job Summary" runat="server"></asp:Label>
        </HeaderTemplate>
        <HeaderStyle Font-Bold="true" BackColor="LightGray" HorizontalAlign="Center" />
        <Fields>
            <asp:BoundField DataField="Id" HeaderText="Job No" SortExpression="Id" HeaderStyle-Font-Bold="true" />
            <asp:BoundField DataField="JobName" HeaderText="Job Name" ReadOnly="True" SortExpression="JobName"
                HeaderStyle-Font-Bold="true" />
            <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" SortExpression="CustomerCode"
                HeaderStyle-Font-Bold="true" />
            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" SortExpression="CustomerName"
                HeaderStyle-Font-Bold="true" />
            <asp:TemplateField HeaderText="Created On" HeaderStyle-Font-Bold="true">
                <ItemTemplate>
                    <asp:Label ID="lblJobCreatedOn" Text='<%# IsValidDate(Eval("CreatedOn")) %>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Collateral due On" HeaderStyle-Font-Bold="true">
                <ItemTemplate>
                    <asp:Label ID="lblCollateralDueOn" Text='<%# IsValidDate(Eval("CollateralDueOn")) %>'
                        runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Deadline On" HeaderStyle-Font-Bold="true">
                <ItemTemplate>
                    <asp:Label ID="lblDeadlineOn" Text='<%# IsValidDate(Eval("DeadlineOn")) %>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Completed On" HeaderStyle-Font-Bold="true">
                <ItemTemplate>
                    <asp:Label ID="lblCompletedOn" Text='<%# IsValidDate(Eval("CompletedOn")) %>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Instructions" HeaderText="Instructions" SortExpression="Instructions"
                HeaderStyle-Font-Bold="true" />
            <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note" HeaderStyle-Font-Bold="true" />
            <asp:BoundField DataField="AccountHandler" HeaderText="Account Handler" SortExpression="AccountHandler"
                HeaderStyle-Font-Bold="true" />
            <asp:BoundField DataField="MaterialFromExternalSource" HeaderText="Materials" SortExpression="MaterialFromExternalSource"
                HeaderStyle-Font-Bold="true" />
            <asp:BoundField DataField="ProductionCost" HeaderText="Production Cost" SortExpression="ProductionCost"
                HeaderStyle-Font-Bold="true" />
            <asp:BoundField DataField="DistributionCost" HeaderText="Distribution Cost" SortExpression="DistributionCost"
                HeaderStyle-Font-Bold="true" />
            <asp:TemplateField HeaderText="Job Stages" HeaderStyle-Font-Bold="true">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:GridView ID="gvJobStates" runat="server" AutoGenerateColumns="False" Width="100%"
                        ShowHeader="false" GridLines="None">
                        <Columns>
                            <asp:TemplateField SortExpression="JobStateName">
                                <ItemTemplate>
                                    <asp:Label ID="lblJobStateName" runat="server" Text='<%# Bind("JobStateName") %>'
                                        Font-Bold='<%# Eval("IsCompleted")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Job Files" HeaderStyle-Font-Bold="true">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:GridView ID="gvJobFiles" runat="server" AutoGenerateColumns="true" Width="100%" AllowSorting="true" 
                        ShowHeader="false" GridLines="None">
                        <Columns>
<%--                            <asp:TemplateField >
                                <ItemTemplate>
                                    <asp:Label ID="lblJobFileName" runat="server" Text="" />
                                </ItemTemplate>
                            </asp:TemplateField>
--%>                        </Columns>
                    </asp:GridView>
                </ItemTemplate>
            </asp:TemplateField>
        </Fields>
        <AlternatingRowStyle BackColor="LightGray" />
        <EmptyDataTemplate>
            <div>
                <asp:Label ID="lblEmptyRecord" Text="No Record Found" runat="server" Font-Bold="true"
                    align="center" ForeColor="red"></asp:Label>
            </div>
        </EmptyDataTemplate>
    </asp:DetailsView>
</div>
