<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AdminPromo.ascx.vb" Inherits="NEvoWeb.Modules.NB_Store.AdminPromo" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table class="NBright_ContentDiv"><tr><td>
<asp:Panel ID="pnlEdit" runat="server">
<div class="NBright_ButtonDiv">
<asp:linkbutton id="cmdNew" runat="server" resourcekey="cmdNew" cssclass="NBright_AddButton" >Add New</asp:linkbutton>&nbsp;
<asp:linkbutton id="cmdDelete" runat="server" resourcekey="cmdDelete" cssclass="NBright_DeleteButton" >Delete</asp:linkbutton>
</div>
<asp:CheckBox ID="chkAllowMultiDiscount" runat="server" AutoPostBack="true" /><dnn:label id="plAllowMultiDiscount" runat="server" controlname="chkAllowMultiDiscount" suffix=""></dnn:label>
<asp:Panel id="pnlProduct" runat="server">
<div class="NBright_SelectDiv">
    <asp:TextBox ID="txtSearch" runat="server" Width="169px"></asp:TextBox>
    <asp:LinkButton ID="cmdSearch" cssclass="NBright_CommandButton" resourcekey="cmdSearch" runat="server">Search</asp:LinkButton>
</div>
</asp:Panel><asp:Panel id="pnlRange" runat="server">

<asp:GridView ID="gvPromo" runat="server" AllowPaging="False" 
    AllowSorting="True" AutoGenerateColumns="False" width="100%" gridlines="None" 
            cellpadding="2" PageSize="25" >
            <HeaderStyle cssclass="NBright_HeaderStyle" />
			<FooterStyle cssclass="NBright_FooterStyle"/>
            <EditRowStyle cssclass="NBright_EditItemStyle" />
            <SelectedRowStyle  cssclass="NBright_SelectedItemStyle"/>
			<PagerStyle cssclass="NBright_PagerStyle"/>
			<AlternatingRowStyle cssclass="NBright_AlternatingItemStyle" />
            <RowStyle cssclass="NBright_ItemStyle" />
    <Columns>
       <asp:CommandField ShowEditButton="True" buttontype="Image" CancelImageUrl="~/Desktopmodules/NB_Store/img/cancel.png" 
            EditImageUrl="~/images/edit.gif" UpdateImageUrl="~/Desktopmodules/NB_Store/img/save.png" />
   <asp:TemplateField ShowHeader="False">
     <ItemTemplate>
       <asp:LinkButton ID="cmdDelete" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete"/>
     </ItemTemplate>
   </asp:TemplateField>
        <asp:TemplateField HeaderText="Name">
            <EditItemTemplate>
                <asp:HiddenField ID="PromoID" runat="server" Value='<%# Bind("PromoID") %>' />
                <asp:HiddenField ID="ObjectID" runat="server" Value='<%# Bind("ObjectID") %>' />
                <asp:HiddenField ID="PromoType" runat="server" Value='<%# Bind("PromoType") %>' />
            <dnn:label id="plPromoName" runat="server" controlname="txtPromoName" suffix=":"></dnn:label>
                <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("PromoName") %>' 
                    Width="250px"></asp:TextBox>
                <br />
                <dnn:label id="plPromoGroup" runat="server" controlname="txtPromoGroup" suffix=":"></dnn:label>
                <asp:DropDownList ID="ddlPromoGroup" runat="server" DataValueField='<%# Bind("PromoGroup") %>'>
                </asp:DropDownList>
                <br />
                <dnn:label id="plPromoUser" runat="server" controlname="txtPromoUser" suffix=":"></dnn:label>
                <asp:TextBox ID="txtPromoUser" runat="server" Text='<%# Bind("PromoUser") %>'></asp:TextBox>
                <br />
                <dnn:label id="plPromoEmail" runat="server" controlname="txtPromoEmail" suffix=":"></dnn:label>
                <asp:TextBox ID="txtPromoEmail" runat="server" Text='<%# Bind("PromoEmail") %>' Width="250px"></asp:TextBox>
                <br />
                <dnn:label id="plCategories" runat="server" controlname="ddlCategories" suffix=":"></dnn:label>
                <asp:DropDownList ID="ddlCategories" runat="server" Width="250px" DataValueField='<%# Bind("ObjectID") %>'>
                </asp:DropDownList>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:HiddenField ID="PromoID" runat="server" Value='<%# Bind("PromoID") %>' />
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("PromoName") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date">
            <EditItemTemplate>
                <dnn:label id="plDateRange" runat="server" controlname="txtDateRange1" suffix=":"></dnn:label>
                <asp:TextBox ID="txtDateRange1" runat="server" Width="90px" Text='<%# Bind("RangeStartDate","{0:d}") %>' ></asp:TextBox><asp:HyperLink ID="hypDateRange1" runat="server"></asp:HyperLink>
                <asp:CompareValidator ID="revDateRange1" runat="server" 
                ControlToValidate="txtDateRange1" Display="Dynamic" ErrorMessage="*" 
                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>                &nbsp;-&nbsp;
                <asp:TextBox ID="txtDateRange2" runat="server" Width="90px" Text='<%# Bind("RangeEndDate","{0:d}") %>' ></asp:TextBox><asp:HyperLink ID="hypDateRange2" runat="server"></asp:HyperLink>
                <asp:CompareValidator ID="revDateRange2" runat="server" 
                ControlToValidate="txtDateRange2" Display="Dynamic" ErrorMessage="*" 
                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                <br />
                <dnn:label id="plQtyRange" runat="server" controlname="txtQtyRange1" suffix=":"></dnn:label>
                <asp:TextBox ID="txtQtyRange1" runat="server" Width="60px" Text='<%# Bind("QtyRange1") %>'></asp:TextBox>
                <asp:CompareValidator ID="revQtyRange1" runat="server" 
                ControlToValidate="txtQtyRange1" Display="Dynamic" ErrorMessage="*" 
                Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>   
                <asp:Label ID="lblQtySep" runat="server" Text="&nbsp;-&nbsp;"></asp:Label>             
                <asp:TextBox ID="txtQtyRange2" runat="server" Width="60px" Text='<%# Bind("QtyRange2") %>'></asp:TextBox>
                <asp:CompareValidator ID="revQtyRange2" runat="server" 
                ControlToValidate="txtQtyRange2" Display="Dynamic" ErrorMessage="*" 
                Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>    
                <br />
                <dnn:label id="plRange" runat="server" controlname="txtRange1" suffix=":"></dnn:label>
                <asp:TextBox ID="txtRange1" runat="server" Width="60px" Text='<%# Bind("Range1") %>'></asp:TextBox>
                <asp:CompareValidator ID="revRange1" runat="server" 
                ControlToValidate="txtRange1" Display="Dynamic" ErrorMessage="*" 
                Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
                <asp:Label ID="lblPriceSep" runat="server" Text="&nbsp;-&nbsp;"></asp:Label>  
                <asp:TextBox ID="txtRange2" runat="server" Width="60px" Text='<%# Bind("Range2") %>'></asp:TextBox>
                <asp:CompareValidator ID="revRange2" runat="server" 
                ControlToValidate="txtRange2" Display="Dynamic" ErrorMessage="*" 
                Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>            
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label3" runat="server" Text='<%# Bind("RangeStartDate","{0:d}") %>'></asp:Label>&nbsp;-&nbsp;
                <asp:Label ID="Label4" runat="server" Text='<%# Bind("RangeEndDate","{0:d}") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <EditItemTemplate>
            </EditItemTemplate>
            <ItemTemplate>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <EditItemTemplate>
            <dnn:label id="plPromoCode" runat="server" controlname="txtPromoCode" suffix=":"></dnn:label>
                <asp:TextBox ID="txtPromoCode" runat="server" Text='<%# Bind("PromoCode") %>'></asp:TextBox>
                <br />
                <dnn:label id="plMaxUsage" runat="server" controlname="txtMaxUsage" suffix=":" Text="Max."></dnn:label>
                <asp:TextBox ID="txtMaxUsage" runat="server" Text='<%# Bind("MaxUsage") %>'></asp:TextBox>                
                <br />
                <dnn:label id="plMaxUsagePerUser" runat="server" controlname="txtMaxUsagePerUser" suffix=":" Text="Max."></dnn:label>
                <asp:TextBox ID="txtMaxUsagePerUser" runat="server" Text='<%# Bind("MaxUsagePerUser") %>'></asp:TextBox>                
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text='<%# Bind("PromoCode") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Amt">
            <EditItemTemplate>
            <dnn:label id="plAmount" runat="server" controlname="txtAmount" suffix=":"></dnn:label>
                <asp:TextBox ID="txtAmount" runat="server" Text='<%# Bind("PromoAmount") %>' width="50px"></asp:TextBox>
                <asp:CompareValidator ID="revAmount" runat="server" 
                ControlToValidate="txtAmount" Display="Dynamic" ErrorMessage="*" 
                Operator="DataTypeCheck" Type="Double"></asp:CompareValidator>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("PromoAmount") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="%">
            <EditItemTemplate>
            <dnn:label id="plPercent" runat="server" controlname="txtPercent" suffix=":"></dnn:label>
                <asp:TextBox ID="txtPercent" runat="server" Text='<%# Bind("PromoPercent") %>' width="50px"></asp:TextBox>
                <asp:CompareValidator ID="revPercent" runat="server" ControlToValidate="txtPercent" Display="Dynamic" ErrorMessage="*" Operator="DataTypeCheck" Type="Integer"  ></asp:CompareValidator>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="lblPercent" runat="server" Text='<%# Bind("PromoPercent") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Enabled">
            <EditItemTemplate>
                <asp:CheckBox ID="chkDisable" runat="server" 
                    Checked='<%# not(DataBinder.Eval(Container.DataItem, "Disabled")) %>' />            
            </EditItemTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkDisable2" runat="server" 
                    Checked='<%# not(DataBinder.Eval(Container.DataItem, "Disabled")) %>' Enabled="False"/>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Panel>
</asp:Panel>
</td></tr>
</table>

