' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2010 SARL NevoWeb.  www.nevoweb.com. BSD License.
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

Imports System.Data.SqlTypes
Imports DotNetNuke.UI.WebControls
Imports DotNetNuke.Common.Utilities
Imports System.Xml
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    <ToolboxData("<{0}:GenTextEditor runat=server></{0}:GenTextEditor>")> _
Public Class GenTextEditor
        Inherits System.Web.UI.UserControl

        Public Sub New(ByVal xmlNod As XmlNode)
            Dim oControl As New System.Web.UI.Control
            oControl = CType(LoadControl("~/controls/TextEditor.ascx"), DotNetNuke.UI.UserControls.TextEditor)
            oControl = assignByReflection(oControl, xmlNod)
            Me.Controls.Add(oControl)
        End Sub
    End Class

End Namespace




