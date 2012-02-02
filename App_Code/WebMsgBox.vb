Imports Microsoft.VisualBasic
Imports System.Collections

Public Class WebMsgBox
    Protected Shared htHandlerPages As Hashtable = New Hashtable

    Public Shared Sub Show(ByVal sMessage As String)
        Dim qMessageQueue As Queue
        If Not htHandlerPages.Contains(HttpContext.Current.Handler) Then
            Dim pgCurrentPage As Page = HttpContext.Current.Handler
            If pgCurrentPage IsNot Nothing Then
                qMessageQueue = New Queue
                qMessageQueue.Enqueue(sMessage)
                htHandlerPages.Add(HttpContext.Current.Handler, qMessageQueue)
                'pgCurrentPage. ' AddHandler here
                AddHandler pgCurrentPage.Unload, AddressOf CurrentPageUnload
            Else
                qMessageQueue = htHandlerPages(HttpContext.Current.Handler)
                qMessageQueue.Enqueue(sMessage)
            End If
        End If
    End Sub

    Private Shared Sub CurrentPageUnload(ByVal sender As Object, ByVal e As EventArgs)
        Dim qQueue As Queue = htHandlerPages(HttpContext.Current.Handler)
        If qQueue IsNot Nothing Then
            Dim sbBuilder As New StringBuilder
            Dim nMessageCount As Integer = qQueue.Count
            sbBuilder.Append("<script language='javascript'>")
            Dim sMsg As String
            While nMessageCount > 0
                nMessageCount -= 1
                sMsg = qQueue.Dequeue.ToString
                Dim sQuote As String = """"
                sMsg = sMsg.Replace(squote, "'")
                sbBuilder.Append("alert(" & sQuote & sMsg & sQuote & ");")
            End While
            sbBuilder.Append("</script>")
            htHandlerPages.Remove(HttpContext.Current.Handler)
            HttpContext.Current.Response.Write(sbBuilder.ToString())
        End If
    End Sub

End Class
