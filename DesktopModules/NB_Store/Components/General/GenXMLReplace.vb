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


Imports System.Xml
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class GenXMLReplace

        Private Inputs As System.Collections.Specialized.NameValueCollection = New System.Collections.Specialized.NameValueCollection

        Public Sub AddNodeReplacement(ByVal XPath As String, ByVal value As String, Optional ByVal cdata As Boolean = True)
            If cdata Then
                value = "<![CDATA[" & value & "]]>"
            End If
            Inputs.Add(XPath, value)
        End Sub

        Public Function DoNodeReplacement(ByVal XMLinfo As String) As String
            Dim xmlDoc As New XmlDataDocument

            xmlDoc.LoadXml(XMLinfo)


            Dim i As Integer
            For i = 0 To Inputs.Keys.Count - 1 Step i + 1
                ReplaceXMLNode(xmlDoc, Inputs.Keys(i), Inputs(Inputs.Keys(i)))
            Next

            Return xmlDoc.OuterXml
        End Function


    End Class

End Namespace
