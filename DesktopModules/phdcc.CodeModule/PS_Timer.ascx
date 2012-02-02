<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PS_Timer.ascx.vb" Inherits="PS_Timer" %>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>

<script language="javascript" type="text/javascript">

    $(document).ready(function () {

        $("#<%= txtGroupName.ClientID %>").blur(function (e) {
            var pageUrl = '<%=ResolveUrl("~/Timer.aspx")%>'
            alert(pageUrl);
            $.ajax({
                type: "POST",
                url: pageUrl + "/IsExist",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: AjaxSuccess,
                failure: function (response) {
                    alert(response);
                },
                error: function (msg) {
                    alert("error" + msg.d);
                }

            });

        });

    });

    function AjaxSuccess(response) {
        alert(response);
    }


</script>
<style type="text/css">
    .water
    {
        font-family: Tahoma, Arial, sans-serif;
        font-size: 75%;
        color: gray;
    }
</style>
<div>
<%--    <telerik:RadAjaxPanel ID="rap" runat="server">--%>
        <asp:Label ID="lblGroupName" runat="server" Text="Group Name" CssClass="fieldsetControlStyle"></asp:Label>
        &nbsp;&nbsp;
        <asp:TextBox ID="txtGroupName" runat="server" Text="" CssClass="water"></asp:TextBox>
        <asp:Button ID="btnSave" runat="server" Text="Save" />
    <%--</telerik:RadAjaxPanel>--%>
</div>


<%--<asp:Timer ID="TimerAutoSave" runat="server" OnTick="TimerAutoSave_Tick" Interval="5000" />--%>
