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


Imports System.Reflection

Namespace NEvoWeb.Modules.NB_Store

    Public Class FormTemplateBuilder
        Private Inputs As System.Collections.Specialized.NameValueCollection = New System.Collections.Specialized.NameValueCollection
        Private Displays As System.Collections.Specialized.NameValueCollection = New System.Collections.Specialized.NameValueCollection
        Private _Title As String = ""
#Region "Add controls"

        Public Sub AddControl(ByVal ctrl As Object, ByVal DisplayLabel As String)
            Dim newID As String
            newID = ctrl.ID & Inputs.Keys.Count.ToString
            AddControl(newID, ctrl, DisplayLabel)
        End Sub

        Public Sub AddControl(ByVal id As String, ByVal ctrl As Object, ByVal DisplayLabel As String)
            Dim strTemp As String = getFieldTemplate(id, ctrl)
            If strTemp <> "" Then
                Inputs.Add(id, strTemp)
                Displays.Add(id, DisplayLabel)
            End If
        End Sub

        Public Sub AddTitle(ByVal Title As String)
            AddTitle(Title, False)
        End Sub

        Public Sub AddTitle(ByVal Title As String, ByVal IsHTML As Boolean)
            If IsHTML Then
                _Title = Title
            Else
                _Title = "<h1>" & Title & "</h1><hr/>"
            End If
        End Sub

#End Region

#Region "Mothods"

        Public Function GetFormTemplate() As String
            Dim strHtml As String = _Title

            strHtml &= "<table>"

            Dim i As Integer
            For i = 0 To Inputs.Keys.Count - 1 Step i + 1

                strHtml &= "<tr>"
                strHtml &= "<td align=""right"">"

                strHtml &= Displays(Displays.Keys(i))

                strHtml &= "</td>"
                strHtml &= "<td>"

                strHtml &= Inputs(Inputs.Keys(i))

                strHtml &= "</td>"
                strHtml &= "</tr>"

            Next

            strHtml &= "</table>"

            Return strHtml
        End Function

        Public Function getFieldTemplate(ByVal ctrl As Object) As String
            Return getFieldTemplate(ctrl.id, ctrl)
        End Function

        Public Function getFieldTemplate(ByVal id As String, ByVal ctrl As Object) As String
            Dim strValue As String = "[<tag "

            If TypeOf ctrl Is NBright_TextBox Then
                strValue &= " type=""textbox"""
            ElseIf TypeOf ctrl Is NBright_dateEditControl Then
                strValue &= " type=""dateeditcontrol"""
            ElseIf TypeOf ctrl Is NBright_DropDownList Then
                strValue &= " type=""dropdownlist"""
            ElseIf TypeOf ctrl Is NBright_CheckBox Then
                strValue &= " type=""checkbox"""
            ElseIf TypeOf ctrl Is NBright_RadioButtonList Then
                strValue &= " type=""radiobuttonlist"""
            ElseIf TypeOf ctrl Is RequiredFieldValidator Then
                strValue &= " type=""rfvalidator"""
            ElseIf TypeOf ctrl Is RegularExpressionValidator Then
                strValue &= " type=""revalidator"""
            ElseIf TypeOf ctrl Is ValidationSummary Then
                strValue &= " type=""validationsummary"""
            Else
                Return ""
            End If

            Dim blnID As Boolean = False
            For Each PropertyItem As PropertyInfo In ctrl.GetType().GetProperties()
                Dim strToAdd As String = ""
                If Not PropertyItem.GetValue(ctrl, Nothing) Is Nothing Then
                    strToAdd = PropertyItem.GetValue(ctrl, Nothing).ToString()
                End If
                If strToAdd <> "" Then
                    If PropertyItem.Name.ToLower = "id" Then
                        strValue &= " " & PropertyItem.Name & "=""" & id & """"
                        blnID = True
                    Else
                        strValue &= " " & PropertyItem.Name & "=""" & strToAdd & """"
                    End If
                End If
            Next

            If Not blnID Then
                'no id assign so create
                strValue &= " id=""" & id & """"
            End If

            strValue &= " />]"

            Return strValue
        End Function

#End Region

    End Class

End Namespace
