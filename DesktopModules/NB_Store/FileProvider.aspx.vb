Option Strict On

Imports System
Imports System.Web
Imports System.Web.Services
Imports System.IO
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public MustInherit Class FileProvider
        Implements System.Web.IHttpHandler

#Region "Event Handlers"
        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest
            Dim key As String = getURLParam(context, "key")
            Dim DetailID As String = getURLParam(context, "did")
            Dim PortalID As String = getURLParam(context, "PortalID")

            If IsNumeric(DetailID) Then
                Dim strXML As String = ""
                Dim objCtrl As New OrderController
                Dim objDInfo As NB_Store_OrderDetailsInfo


                objDInfo = objCtrl.GetOrderDetail(CInt(DetailID))

                If objDInfo.CartXMLInfo <> "" Then
                    Dim DocPath As String
                    Dim FileExt As String
                    Dim xmlDoc As New Xml.XmlDataDocument
                    xmlDoc.LoadXml(objDInfo.CartXMLInfo)

                    Try
                        If key = "" Then
                            'get default fileupload data
                            DocPath = xmlDoc.SelectSingleNode("root/fuupload").InnerText
                        Else
                            DocPath = xmlDoc.SelectSingleNode("root/" & key).InnerText
                        End If
                    Catch ex As Exception
                        DocPath = ""
                    End Try

                    If DocPath <> "" Then

                        FileExt = Right(DocPath, 3) ' just use last 3 chars for extension, should match most files.

                        Dim useragent As String = context.Request.Headers("User-Agent")
                        If useragent.Contains("MSIE 7.0") Or useragent.Contains("MSIE 8.0") Then
                            context.Response.AppendHeader("content-disposition", "attachment; filename=""" & Replace(objDInfo.ItemDesc, " ", "_") & "." & FileExt & """")
                        Else
                            context.Response.AppendHeader("content-disposition", "attachment; filename=""" & System.Web.HttpUtility.UrlDecode(Replace(objDInfo.ItemDesc, " ", "_")) & "." & FileExt & """")
                        End If
                        context.Response.ContentType = "application/octet-stream"
                        context.Response.WriteFile(DocPath)
                        context.Response.End()
                    End If

                End If

            End If

        End Sub
#End Region

#Region "Private Methods"




#End Region

    End Class

End Namespace
