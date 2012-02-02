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


Imports DotNetNuke.Common.Globals
Imports System.Xml
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class BaseDisplayTemplate
        Implements ITemplate

        Protected _aryTempl As String()
        Protected _TemplateText As String

        Sub New(ByVal TemplateText As String)
            _aryTempl = ParseTemplateText(TemplateText)

            _TemplateText = TemplateText
        End Sub

        Public Overridable Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            Dim lc As Literal
            Dim cmd As LinkButton

            For lp = 0 To _aryTempl.GetUpperBound(0)

                If _aryTempl(lp).ToUpper.StartsWith("TAG:LINKBUTTON") Then
                    cmd = New LinkButton
                    assignProperties(_aryTempl(lp), cmd)
                    AddHandler cmd.DataBinding, AddressOf LinkButtonDataBind_DataBinding
                    container.Controls.Add(cmd)
                ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:DATABIND") Then
                    Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                    lc = New Literal
                    lc.Text = TagAtt(2)
                    AddHandler lc.DataBinding, AddressOf ClassDataBind_DataBinding
                    container.Controls.Add(lc)
                Else
                    lc = New Literal
                    lc.Text = _aryTempl(lp)
                    lc.EnableViewState = False
                    container.Controls.Add(lc)
                End If

            Next

        End Sub

        Private Sub LinkButtonDataBind_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cmd As LinkButton
            cmd = CType(sender, LinkButton)
            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)
            cmd.CommandArgument = DataBinder.Eval(container.DataItem, cmd.CommandArgument).ToString
        End Sub

        Private Sub ClassDataBind_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            lc.Text = DataBinder.Eval(container.DataItem, lc.Text).ToString
        End Sub


        Friend Function ParseTemplateText(ByVal TemplText As String) As String()
            Dim strOUT As String()
            Dim ParamAry As Char() = {"[", "]"}

            strOUT = TemplText.Split(ParamAry)

            Return strOUT
        End Function

        Friend Function assignProperties(ByVal TagString As String, ByVal obj As Object, Optional ByVal IdPrefix As String = "") As Object
            Dim TagAtt As String() = TagString.Split(":"c)
            obj.ID = IdPrefix & TagAtt(1).ToLower
            If TagAtt.GetUpperBound(0) >= 2 Then
                obj = assignByReflection(obj, TagAtt(2))
            End If
            Return obj
        End Function

    End Class

End Namespace
