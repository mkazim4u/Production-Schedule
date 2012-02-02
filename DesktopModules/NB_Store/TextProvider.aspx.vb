Option Strict On

Imports System
Imports System.Web
Imports System.Web.Services
Imports System.IO
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public MustInherit Class TextProvider
        Implements System.Web.IHttpHandler

#Region "Event Handlers"
        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
            Dim strText As String = ""

            strText = GetFeederReportText(context, False)

            If strText <> "" Then

                context.Response.ContentType = "text/plain"
                context.Response.ContentEncoding = System.Text.Encoding.UTF8
                context.Response.Cache.SetAllowResponseInBrowserHistory(True)
                context.Response.Write(strText)

            End If

        End Sub
#End Region

#Region "Private Methods"




#End Region

    End Class

End Namespace
