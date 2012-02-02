<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SS_CopyImage.ascx.vb"
    Inherits="SS_CopyImage" %>
<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
    rel="stylesheet" type="text/css" />
<style type="text/css">
    .demoheading
    {
        padding-bottom: 20px;
        color: #5377A9;
        font-family: Arial, Sans-Serif;
        font-weight: bold;
        font-size: 1.5em;
    }
    .water
    {
        font-family: Tahoma, Arial, sans-serif;
        font-size: 75%;
        color: gray;
    }
    
    .text-label
    {
        color: #cdcdcd;
        font-weight: bold;
    }
</style>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
<script language="javascript" type="text/javascript">
    $(function () {
        $("#accordion").accordion({ animated: 'bounceslide', event: 'mouseover', collapsible: true });
    });


    $(document).ready(function () {
        $("#<%= txtGroupName.ClientID %>").blur(function () {

            alert("blur");

            $.ajax(
         {
             url: 'CopyImages.aspx',
             type: 'POST',
             data: 'name=John&location=Boston',
             success: function (msg) {
                 if (msg) {
                     alert("Success" + msg)
                 }
                 else {
                     alert("Failure" + msg)
                 }
             }

         });



        });


    });

        
        


   

    







</script>
<div id="accordion">
    <h3>
        <a href="#">Section 1</a></h3>
    <div>
        <p>
            kazim
        </p>
    </div>
    <h3>
        <a href="#">Section 2</a></h3>
    <div>
        <p>
            Sed non urna. Donec et ante. Phasellus eu ligula. Vestibulum sit amet purus. Vivamus
            hendrerit, dolor at aliquet laoreet, mauris turpis porttitor velit, faucibus interdum
            tellus libero ac justo. Vivamus non quam. In suscipit faucibus urna.
        </p>
    </div>
    <h3>
        <a href="#">Section 3</a></h3>
    <div>
        <p>
            Nam enim risus, molestie et, porta ac, aliquam ac, risus. Quisque lobortis. Phasellus
            pellentesque purus in massa. Aenean in pede. Phasellus ac libero ac tellus pellentesque
            semper. Sed ac felis. Sed commodo, magna quis lacinia ornare, quam ante aliquam
            nisi, eu iaculis leo purus venenatis dui.
        </p>
        <ul>
            <li>List item one</li>
            <li>List item two</li>
            <li>List item three</li>
        </ul>
    </div>
    <h3>
        <a href="#">Section 4</a></h3>
    <div>
        <p>
            Cras dictum. Pellentesque habitant morbi tristique senectus et netus et malesuada
            fames ac turpis egestas. Vestibulum ante ipsum primis in faucibus orci luctus et
            ultrices posuere cubilia Curae; Aenean lacinia mauris vel est.
        </p>
        <p>
            Suspendisse eu nisl. Nullam ut libero. Integer dignissim consequat lectus. Class
            aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos.
        </p>
    </div>
</div>
<div>
    <telerik:RadAjaxPanel ID="rap" runat="server">
        <asp:Panel ID="pnlTimer" runat="server">
            <asp:Timer ID="TimerAutoSave" runat="server" OnTick="TimerAutoSave_Tick" Interval="5000" />
        </asp:Panel>
    </telerik:RadAjaxPanel>
</div>
<div>
    <asp:Label ID="lblGroupName" runat="server" Text="Group Name" CssClass="fieldsetControlStyle"></asp:Label>
    &nbsp;&nbsp;
    <asp:TextBox ID="txtGroupName" runat="server" Text="" CssClass="water"></asp:TextBox>
    <br />
    <br />
    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CausesValidation="True" CssClass="lnkButton"
        ToolTip="Update">
        <asp:Image ID="imgUpdate" runat="server" ImageUrl="~/Portals/0/Images/Update.gif" /></asp:LinkButton>
    &nbsp;<asp:LinkButton ID="lnkbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
        CssClass="lnkButton" ToolTip="Cancel">
        <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Portals/0/Images/Cancel.gif" /></asp:LinkButton>
</div>
<asp:Button ID="btnSave" runat="server" Text="Save Group" />