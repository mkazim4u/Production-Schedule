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
Imports System.Web.UI.TemplateControl

Namespace NEvoWeb.Modules.NB_Store

    Public Class GenXMLTemplate
        Implements ITemplate

        Protected _aryTempl As String()
        Protected _NestedLevel As ArrayList
        Protected _UserInfo As DotNetNuke.Entities.Users.UserInfo
        Protected _FoundEscapeChar As Boolean = False

        Sub New(ByVal TemplateText As String, ByVal UsrInfo As DotNetNuke.Entities.Users.UserInfo)
            _UserInfo = UsrInfo
            _aryTempl = ParseTemplateText(TemplateText)
            _NestedLevel = New ArrayList
            _NestedLevel.Add(True)
        End Sub

        Sub New(ByVal TemplateText As String)
            _aryTempl = ParseTemplateText(TemplateText)
            _NestedLevel = New ArrayList
            _NestedLevel.Add(True)
        End Sub

        Sub New(ByVal ResourceKey As String, ByVal ResourceFile As String)
            Dim TemplateText As String = DotNetNuke.Services.Localization.Localization.GetString(ResourceKey, ResourceFile)
            If TemplateText = "" Then TemplateText = "---- NO TEMPLATE FOUND ---- [" & ResourceFile & "," & ResourceKey & "]"
            _aryTempl = ParseTemplateText(TemplateText)
            _NestedLevel = New ArrayList
            _NestedLevel.Add(True)
        End Sub

        Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            Dim lc As Literal
            Dim lp As Integer
            Dim strXML As String = ""
            Dim xmlNod As XmlNode
            Dim xmlDoc As New XmlDocument
            Dim ctrltype As String = ""

            For lp = 0 To _aryTempl.GetUpperBound(0)
                If System.Web.HttpUtility.HtmlDecode(_aryTempl(lp)).ToLower.StartsWith("<tag") Then

                    strXML = System.Web.HttpUtility.HtmlDecode(_aryTempl(lp))
                    strXML = "<root>" & strXML & "</root>"

                    xmlDoc.LoadXml(strXML)
                    xmlNod = xmlDoc.SelectSingleNode("root/tag")

                    If Not xmlNod.Attributes("type") Is Nothing Then
                        ctrltype = xmlNod.Attributes("type").InnerXml.ToLower
                    End If

                    If Not xmlNod.Attributes("ctrltype") Is Nothing Then
                        ctrltype = xmlNod.Attributes("ctrltype").InnerXml.ToLower
                    End If

                    If ctrltype <> "" Then

                        Select Case ctrltype
                            Case "linkbutton"
                                createLinkButton(container, xmlNod)
                            Case "textbox"
                                createTextbox(container, xmlNod)
                            Case "dateeditcontrol"
                                createDateEditControl(container, xmlNod)
                            Case "dropdownlist"
                                createDropDownList(container, xmlNod)
                            Case "checkbox"
                                createCheckBox(container, xmlNod)
                            Case "radiobuttonlist"
                                createRadioButtonList(container, xmlNod)
                            Case "checkboxlist"
                                createCheckBoxList(container, xmlNod)
                            Case "rvalidator"
                                createRangeValidator(container, xmlNod)
                            Case "rfvalidator"
                                createRequiredFieldValidator(container, xmlNod)
                            Case "revalidator"
                                createRegExValidator(container, xmlNod)
                            Case "validationsummary"
                                createValidationSummary(container, xmlNod)
                            Case "dnntexteditor"
                                createTextEditor(container, xmlNod)
                            Case Else

                                If _FoundEscapeChar Then
                                    _aryTempl(lp) = Replace(_aryTempl(lp), "**SQROPEN**", "[")
                                    _aryTempl(lp) = Replace(_aryTempl(lp), "**SQRCLOSE**", "]")
                                End If

                                lc = New Literal
                                lc.Text = strXML
                                AddHandler lc.DataBinding, AddressOf General_DataBinding
                                container.Controls.Add(lc)
                        End Select
                    End If
                ElseIf _aryTempl(lp).ToUpper = "TAG:END" Then
                    lc = New Literal
                    lc.Text = ""
                    AddHandler lc.DataBinding, AddressOf Visible_DataBinding
                    container.Controls.Add(lc)
                ElseIf _aryTempl(lp).ToUpper.StartsWith("TAG:TEST") Then
                    Dim TagAtt As String() = _aryTempl(lp).Split(":"c)
                    lc = New Literal
                    lc.Text = TagAtt(2)
                    AddHandler lc.DataBinding, AddressOf TokenTest_DataBinding
                    container.Controls.Add(lc)
                Else
                    If _FoundEscapeChar Then
                        _aryTempl(lp) = Replace(_aryTempl(lp), "**SQROPEN**", "[")
                        _aryTempl(lp) = Replace(_aryTempl(lp), "**SQRCLOSE**", "]")
                    End If

                    lc = New Literal
                    lc.Text = _aryTempl(lp)
                    container.Controls.Add(lc)
                End If
            Next

        End Sub

#Region "Create controls"

        Private Sub createLinkButton(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim cmd As LinkButton
            cmd = New LinkButton

            cmd = assignByReflection(cmd, xmlNod)

            If Not xmlNod.Attributes("src") Is Nothing Then
                cmd.Text = "<img src=""" & xmlNod.Attributes("src").InnerXml & """ border=""0"" />" & cmd.Text
            End If

            If Not xmlNod.Attributes("confirm") Is Nothing Then
                If xmlNod.Attributes("confirm").InnerXml <> "" Then
                    cmd.Attributes.Add("onClick", "javascript:return confirm('" & xmlNod.Attributes("confirm").InnerXml & "');")
                End If
            End If

            AddHandler cmd.DataBinding, AddressOf LinkButton_DataBinding
            container.Controls.Add(cmd)
        End Sub

        Private Sub createTextEditor(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim te As New GenTextEditor(xmlNod)
            If Not xmlNod.Attributes("id") Is Nothing Then
                te.ID = "gte" & xmlNod.Attributes("id").InnerXml

                If Not xmlNod.Attributes("databind") Is Nothing Then
                    te.Attributes.Add("databind", xmlNod.Attributes("databind").InnerXml)
                End If

                AddHandler te.DataBinding, AddressOf TextEditor_DataBinding
                container.Controls.Add(te)
            End If
        End Sub

        Private Sub createRangeValidator(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim rfv As RangeValidator
            rfv = New RangeValidator
            rfv.Text = "*" 'Set default

            rfv = assignByReflection(rfv, xmlNod)

            container.Controls.Add(rfv)
        End Sub

        Private Sub createRequiredFieldValidator(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim rfv As RequiredFieldValidator
            rfv = New RequiredFieldValidator
            rfv.Text = "*" 'Set default

            rfv = assignByReflection(rfv, xmlNod)

            container.Controls.Add(rfv)
        End Sub

        Private Sub createDateEditControl(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim calpop As DateEditControl
            calpop = New DateEditControl
            If Not xmlNod.Attributes("id") Is Nothing Then
                calpop.ID = xmlNod.Attributes("id").InnerXml & "dctrl"
            End If
            Dim txt As TextBox
            txt = getcreateTextbox(container, xmlNod)
            txt.Attributes.Add("datatype", "date")
            calpop.dateField = txt
            AddHandler calpop.DataBinding, AddressOf Date_DataBinding
            container.Controls.Add(calpop)
        End Sub

        Private Sub createRegExValidator(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim rfv As RegularExpressionValidator
            rfv = New RegularExpressionValidator

            rfv.Text = "*" 'Set default
            rfv = assignByReflection(rfv, xmlNod)

            container.Controls.Add(rfv)
        End Sub

        Private Sub createValidationSummary(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim rfv As ValidationSummary
            rfv = New ValidationSummary

            rfv = assignByReflection(rfv, xmlNod)

            container.Controls.Add(rfv)
        End Sub

        Private Sub createRadioButtonList(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim rbl As New RadioButtonList

            rbl = assignByReflection(rbl, xmlNod)

            Dim DataTyp As String = ""
            If Not xmlNod.Attributes("datatype") Is Nothing Then
                DataTyp = xmlNod.Attributes("datatype").InnerXml
                rbl.Attributes.Add("datatype", DataTyp)
            End If

            If Not xmlNod.Attributes("databind") Is Nothing Then
                rbl.Attributes.Add("databind", xmlNod.Attributes("databind").InnerXml)
            End If

            If Not xmlNod.Attributes("data") Is Nothing Then
                Dim strList As String()
                Dim strListValue As String()
                If Not xmlNod.Attributes("datavalue") Is Nothing Then
                    strListValue = xmlNod.Attributes("datavalue").InnerXml.Split(";"c)
                Else
                    strListValue = xmlNod.Attributes("data").InnerXml.Split(";"c)
                End If
                strList = xmlNod.Attributes("data").InnerXml.Split(";"c)
                Dim li As ListItem
                For lp As Integer = 0 To strList.GetUpperBound(0)
                    li = New ListItem
                    Select Case DataTyp.ToLower
                        Case "double"
                            li.Text = FormatFromSave(strList(lp), TypeCode.Double)
                            li.Value = FormatFromSave(strListValue(lp), TypeCode.Double)
                        Case Else
                            li.Text = strList(lp)
                            li.Value = strListValue(lp)
                    End Select
                    rbl.Items.Add(li)
                Next

            End If

            AddHandler rbl.DataBinding, AddressOf RBL_DataBinding
            container.Controls.Add(rbl)
        End Sub

        Private Sub createCheckBoxList(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim chk As New CheckBoxList
            Dim DefaultValue As Boolean = False

            chk = assignByReflection(chk, xmlNod)

            Dim DataTyp As String = ""
            If Not xmlNod.Attributes("datatype") Is Nothing Then
                DataTyp = xmlNod.Attributes("datatype").InnerXml
                chk.Attributes.Add("datatype", DataTyp)
            End If

            If Not xmlNod.Attributes("data") Is Nothing Then

                Dim strList As String()
                Dim strListValue As String()
                If Not xmlNod.Attributes("datavalue") Is Nothing Then
                    strListValue = xmlNod.Attributes("datavalue").InnerXml.Split(";"c)
                Else
                    strListValue = xmlNod.Attributes("data").InnerXml.Split(";"c)
                End If
                strList = xmlNod.Attributes("data").InnerXml.Split(";"c)
                Dim li As ListItem
                For lp As Integer = 0 To strList.GetUpperBound(0)
                    li = New ListItem
                    Select Case DataTyp.ToLower
                        Case "double"
                            li.Text = FormatFromSave(strList(lp), TypeCode.Double)
                            li.Value = FormatFromSave(strListValue(lp), TypeCode.Double)
                        Case Else
                            li.Text = strList(lp)
                            li.Value = strListValue(lp)
                    End Select
                    chk.Items.Add(li)
                Next

                'set default
                If Not xmlNod.Attributes("default") Is Nothing Then
                    DefaultValue = CBool(xmlNod.Attributes("default").InnerText)
                End If
                If DefaultValue Then
                    For lp As Integer = 0 To (chk.Items.Count - 1)
                        chk.Items(lp).Selected = True
                    Next
                End If

            End If

            AddHandler chk.DataBinding, AddressOf ChkB_DataBinding
            container.Controls.Add(chk)
        End Sub

        Private Sub createCheckBox(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim chk As New CheckBox

            chk = assignByReflection(chk, xmlNod)

            AddHandler chk.DataBinding, AddressOf ChkBox_DataBinding
            container.Controls.Add(chk)
        End Sub

        Private Sub createDropDownList(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim ddl As DropDownList
            ddl = New DropDownList

            ddl = assignByReflection(ddl, xmlNod)

            Dim DataTyp As String = ""
            If Not xmlNod.Attributes("datatype") Is Nothing Then
                DataTyp = xmlNod.Attributes("datatype").InnerXml
                ddl.Attributes.Add("datatype", DataTyp)
            End If

            If Not xmlNod.Attributes("databind") Is Nothing Then
                ddl.Attributes.Add("databind", xmlNod.Attributes("databind").InnerXml)
            End If

            If Not xmlNod.Attributes("data") Is Nothing Then
                Dim strList As String()
                Dim strListValue As String()
                If Not xmlNod.Attributes("datavalue") Is Nothing Then
                    strListValue = xmlNod.Attributes("datavalue").InnerXml.Split(";"c)
                Else
                    strListValue = xmlNod.Attributes("data").InnerXml.Split(";"c)
                End If
                strList = xmlNod.Attributes("data").InnerXml.Split(";"c)
                Dim li As ListItem
                For lp As Integer = 0 To strList.GetUpperBound(0)
                    li = New ListItem
                    Select Case DataTyp.ToLower
                        Case "double"
                            li.Text = FormatFromSave(strList(lp), TypeCode.Double)
                            li.Value = FormatFromSave(strListValue(lp), TypeCode.Double)
                        Case Else
                            li.Text = strList(lp)
                            li.Value = strListValue(lp)
                    End Select
                    ddl.Items.Add(li)
                Next
            End If

            AddHandler ddl.DataBinding, AddressOf DDList_DataBinding
            container.Controls.Add(ddl)
        End Sub


        Private Function getcreateTextbox(ByVal container As Control, ByVal xmlNod As XmlNode) As TextBox
            Dim txt As TextBox
            txt = New TextBox
            txt.Text = ""

            txt = assignByReflection(txt, xmlNod)

            If Not xmlNod.Attributes("datatype") Is Nothing Then
                txt.Attributes.Add("datatype", xmlNod.Attributes("datatype").InnerXml)
            End If

            If Not xmlNod.Attributes("databind") Is Nothing Then
                txt.Attributes.Add("databind", xmlNod.Attributes("databind").InnerXml)
            End If

            AddHandler txt.DataBinding, AddressOf Text_DataBinding
            Return txt
        End Function

        Private Sub createTextbox(ByVal container As Control, ByVal xmlNod As XmlNode)
            Dim txt As TextBox
            txt = getcreateTextbox(container, xmlNod)
            container.Controls.Add(txt)
        End Sub
#End Region

#Region "databind controls"

        Private Sub LinkButton_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cmd As LinkButton
            cmd = CType(sender, LinkButton)
            Dim container As DataListItem
            container = CType(cmd.NamingContainer, DataListItem)
            Try
                If Not DataBinder.Eval(container.DataItem, cmd.CommandArgument) Is Nothing Then
                    'dataitem value matching commandarg name 
                    cmd.CommandArgument = DataBinder.Eval(container.DataItem, cmd.CommandArgument)
                Else
                    'no value in dataitem matching commandarg name so search xml values
                    Dim nod As XmlNode = Nothing
                    nod = getXMLnode(cmd.ID, cmd.Text, DataBinder.Eval(container.DataItem, "XMLData"))
                    If Not nod Is Nothing Then
                        cmd.CommandArgument = nod.InnerXml
                    Else
                        nod = getXMLnode(DataBinder.Eval(container.DataItem, "XMLData"), cmd.Text)
                        If Not nod Is Nothing Then
                            cmd.CommandArgument = nod.InnerXml
                        Else
                            cmd.CommandArgument = ""
                        End If
                    End If
                End If

            Catch ex As Exception
                'do nothing
            End Try
        End Sub

        Private Sub TextEditor_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim gte As GenTextEditor
            Dim te As DotNetNuke.UI.UserControls.TextEditor
            gte = CType(sender, GenTextEditor)
            te = DirectCast(gte.Controls(0), DotNetNuke.UI.UserControls.TextEditor)
            Dim container As DataListItem
            container = CType(gte.NamingContainer, DataListItem)
            Try
                If Not gte.Attributes.Item("databind") Is Nothing Then
                    te.Text = DataBinder.Eval(container.DataItem, gte.Attributes.Item("databind"))
                Else
                    te.Text = getXMLvalue(te.ID, "edt", DataBinder.Eval(container.DataItem, "XMLData"))
                End If

            Catch ex As Exception
                'do nothing
            End Try
        End Sub

        Private Sub Visible_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            lc.Visible = False
            If _NestedLevel.Count > 0 Then
                _NestedLevel.RemoveAt(_NestedLevel.Count - 1)
            End If
        End Sub

        Private Sub TokenTest_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)
            Dim objTokenTest As New TokenTest
            assignByReflection(objTokenTest, lc.Text)
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim VisibleMode As Boolean = True
            If Not _UserInfo Is Nothing Then
                VisibleMode = objTokenTest.getVisibleMode(PS.PortalId, -1, container, _UserInfo, CBool(_NestedLevel((_NestedLevel.Count - 1))))
            End If
            lc.Visible = False ' never display this token
            _NestedLevel.Add(VisibleMode)
        End Sub

        Private Sub General_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim lc As Literal
            lc = CType(sender, Literal)
            Dim container As DataListItem
            container = CType(lc.NamingContainer, DataListItem)

            Try
                lc.Text = DataBinder.Eval(container.DataItem, lc.Text)
            Catch ex As Exception
                'do nothing
            End Try
            lc.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub Date_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim dte As DateEditControl
            dte = CType(sender, DateEditControl)
            Dim container As DataListItem
            container = CType(dte.NamingContainer, DataListItem)
            Try
                dte.dateField.Text = getXMLvalue(dte.ID.Substring(0, (dte.ID.Length - 5)), "textbox", DataBinder.Eval(container.DataItem, "XMLData"))
            Catch ex As Exception
                'do nothing
            End Try
            dte.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub RBL_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim rbl As RadioButtonList
            rbl = CType(sender, RadioButtonList)
            Dim container As DataListItem
            container = CType(rbl.NamingContainer, DataListItem)

            Try

                Dim strValue As String = getXMLvalue(rbl.ID, "radiobuttonlist", DataBinder.Eval(container.DataItem, "XMLData"))
                If Not rbl.Items.FindByValue(strValue) Is Nothing Then
                    rbl.SelectedValue = strValue
                End If
            Catch ex As Exception
                'do nothing
            End Try
            rbl.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub ChkB_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim chk As CheckBoxList
            chk = CType(sender, CheckBoxList)
            Dim container As DataListItem
            container = CType(chk.NamingContainer, DataListItem)

            Try
                Dim xmlNod As XmlNode
                xmlNod = getXMLnode(chk.ID, "checkboxlist", DataBinder.Eval(container.DataItem, "XMLData"))

                For Each xmlNoda As XmlNode In xmlNod.SelectNodes("./chk")
                    Dim FindName As String = FormatFromSave(xmlNoda.InnerText)
                    If Not chk.Items.FindByText(FindName).Value Is Nothing Then
                        chk.Items.FindByText(FindName).Selected = CBool(xmlNoda.Attributes("value").Value)
                    End If
                Next

            Catch ex As Exception
                'do nothing
            End Try
            chk.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub ChkBox_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim chk As CheckBox
            chk = CType(sender, CheckBox)
            Dim container As DataListItem
            container = CType(chk.NamingContainer, DataListItem)

            Try
                chk.Checked = CType(getXMLvalue(chk.ID, "checkbox", DataBinder.Eval(container.DataItem, "XMLData")), Boolean)
            Catch ex As Exception
                'do nothing
            End Try
            chk.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub DDList_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim ddl As DropDownList
            ddl = CType(sender, DropDownList)
            Dim container As DataListItem
            container = CType(ddl.NamingContainer, DataListItem)
            Try
                Dim strValue As String = getXMLvalue(ddl.ID, "dropdownlist", DataBinder.Eval(container.DataItem, "XMLData"))
                If Not ddl.Items.FindByValue(strValue) Is Nothing Then
                    ddl.SelectedValue = strValue
                End If
            Catch ex As Exception
                'do nothing
            End Try
            ddl.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

        Private Sub Text_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim txt As TextBox
            txt = CType(sender, TextBox)
            Dim container As DataListItem
            container = CType(txt.NamingContainer, DataListItem)
            Try
                If Not txt.Attributes.Item("databind") Is Nothing Then
                    txt.Text = DataBinder.Eval(container.DataItem, txt.Attributes.Item("databind"))
                Else
                    txt.Text = getXMLvalue(txt.ID, "textbox", DataBinder.Eval(container.DataItem, "XMLData"))
                End If
            Catch ex As Exception
                'do nothing
            End Try
            txt.Visible = CBool(_NestedLevel((_NestedLevel.Count - 1)))
        End Sub

#End Region

        Private Function getXMLvalue(ByVal ctrlID As String, ByVal CtrlType As String, ByVal DataXML As String) As String
            Return getGenXMLvalue(DataXML, "genxml/" & CtrlType & "/" & ctrlID.ToLower)
        End Function

        Private Function getXMLvalue(ByVal DataXML As String, ByVal XPath As String) As String
            Return getGenXMLvalue(DataXML, XPath)
        End Function

        Private Function getXMLnode(ByVal ctrlID As String, ByVal CtrlType As String, ByVal DataXML As String) As XmlNode
            If ctrlID Is Nothing Or CtrlType Is Nothing Or DataXML Is Nothing Then
                Return Nothing
            Else
                Return getGenXMLnode(DataXML, "genxml/" & CtrlType & "/" & ctrlID.ToLower)
            End If
        End Function

        Private Function getXMLnode(ByVal DataXML As String, ByVal XPath As String) As XmlNode
            Return getGenXMLnode(DataXML, XPath)
        End Function

        Private Function ParseTemplateText(ByVal TemplText As String) As String()
            Dim strOUT As String()
            Dim ParamAry As Char() = {"[", "]"}

            'use double sqr brqckets as escape char.
            _FoundEscapeChar = False
            If InStr(TemplText, "[[") > 0 Or InStr(TemplText, "]]") > 0 Then
                TemplText = Replace(TemplText, "[[", "**SQROPEN**")
                TemplText = Replace(TemplText, "]]", "**SQRCLOSE**")
                _FoundEscapeChar = True
            End If

            strOUT = TemplText.Split(ParamAry)

            Return strOUT
        End Function

    End Class

End Namespace
