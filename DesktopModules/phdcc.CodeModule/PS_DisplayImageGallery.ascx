<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_DisplayImageGallery.ascx.vb"
    Inherits="PS_DisplayImageGallery" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="DNN" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="Portal" Namespace="DotNetNuke.Security.Permissions.Controls"
    Assembly="DotNetNuke" %>
<style type="text/css">
    .previewPane
    {
        background-color: #fff;
        background-image: url(Images/previewPaneBg.gif);
        background-repeat: no-repeat;
    }
    
    .product-image
    {
        width: 40px;
        height: 40px;
        float: left;
        border-color: Orange;         
        
    }
    .product-title
    {
        position: absolute;
        margin-top: 10px;
        margin-bottom: 10px;
        margin-left: 5px;
        color: #3d84ca;
        font: 11px 'Segoe UI' , Arial, sans-serif;
    }
</style>
<fieldset id="fsManageCarouselImages" runat="server">
    <legend>Manage Carousel Images</legend>
    <telerik:RadAjaxLoadingPanel ID="alpDisplayCarouselImages" runat="Server" Transparency="30"
        EnableSkinTransparency="false" BackColor="#E0E0E0">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="rapDisplayCarouselImages" runat="server" LoadingPanelID="alpDisplayCarouselImages">
        <div>
            <asp:Label ID="Label2" Text="Customer" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadComboBox ID="rcbCustomer" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rcbCustomer_SelectedIndexChanged">
            </telerik:RadComboBox>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            <br />
            <asp:Label ID="lblImages" Text="Images" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblCarouselImages" Text="CarouselImages" runat="server"></asp:Label>
            <br />
            <br />
            <telerik:RadListBox runat="server" ID="rlbImages" Height="200px" Width="200px" AllowTransferDuplicates="false" AutoPostbackOnTransfer="true" EnableViewState="true"  
                SelectionMode="Multiple" AllowTransfer="true" TransferToID="rlbCarouselImages" DataTextField="sImageUrl" >
                <ItemTemplate>
                    <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("sImageUrl") %>' ImageAlign="Middle"
                        CssClass="product-image" BorderStyle="Solid" BorderWidth="1" />
                    <span class="product-title">
                       <asp:Label ID="lblImgTitle" runat="server" Text='<%# Eval("sfileName") %>'></asp:Label>
                    </span>
                </ItemTemplate>
            </telerik:RadListBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <telerik:RadListBox runat="server" ID="rlbCarouselImages" Height="200px" Width="200px" AllowDelete="true" AllowReorder="true" DataTextField="sImageUrl" OnTransferred="rlbCarouselImages_Transferred" 
               >
                <ItemTemplate>
                    <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("sImageUrl") %>'  ImageAlign="Middle"
                        CssClass="product-image" BorderStyle="Solid" BorderWidth="1" />
                    <span class="product-title">
                        <asp:Label ID="lblImgTitle" runat="server" Text='<%# Eval("sfileName") %>'></asp:Label>
                    </span>
                </ItemTemplate>
            </telerik:RadListBox>
            <br />
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" />
        </div>
    </telerik:RadAjaxPanel>
</fieldset>
