' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. BSD License.
' Author: D.C.Lee
' ------------------------------------------------------------------------
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' ------------------------------------------------------------------------
' This copyright notice may NOT be removed, obscured or modified without written consent from the author.
' --- End copyright notice --- 



Namespace NEvoWeb.Modules.NB_Store

    Public Class RemotePost
        Private Inputs As System.Collections.Specialized.NameValueCollection = New System.Collections.Specialized.NameValueCollection
        Public Url As String = ""
        Public Method As String = "post"
        Public FormName As String = "form"
        Public Sub Add(ByVal name As String, ByVal value As String)
            Inputs.Add(name, value)
        End Sub
        Public Function GetPostHtml(ByVal LoadingImageURL As String) As String
            Dim SipsHtml As String = ""

            SipsHtml = "<html><head>"
            SipsHtml += "</head><body onload=""document." & FormName & ".submit()"">"
            SipsHtml += "<form name=""" & FormName & """ method=""" & Method & """ action=""" & Url & """>"
            Dim i As Integer
            For i = 0 To Inputs.Keys.Count - 1 Step i + 1
                SipsHtml += "<input type=""hidden"" name=""" & Inputs.Keys(i) & """ value=""" & Inputs(Inputs.Keys(i)) & """ />"
            Next
            SipsHtml += "</form>"

            SipsHtml += "  <table border=""0"" cellspacing=""0"" cellpadding=""0"" width=""100%"" height=""100%"">"
            SipsHtml += "<tr><td width=""100%"" height=""100%"" valign=""middle"" align=""center"">"
            SipsHtml += "<font style=""font-family: Trebuchet MS, Verdana, Helvetica;font-size: 14px;letter-spacing: 1px;font-weight: bold;"">"
            SipsHtml += "Processing..."
            SipsHtml += "</font><br /><br /><img src=""" & LoadingImageURL & """ />     "
            SipsHtml += "</td></tr>"
            SipsHtml += "</table>"

            SipsHtml += "</body></html>"

            Return SipsHtml

        End Function

    End Class

End Namespace
