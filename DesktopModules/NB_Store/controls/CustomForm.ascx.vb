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


Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports System.Xml

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class CustomForm
        Inherits Framework.UserControlBase

#Region "Private Members"
        Private _DisplayTemplateName As String
        Private _ReportForm As Boolean = False
        Private _XMLData As String = ""
        Private _GenXMLrun As Boolean = False
        Protected _UserInfo As DotNetNuke.Entities.Users.UserInfo

        Protected WithEvents dlCustomForm As Global.System.Web.UI.WebControls.DataList

#End Region

#Region "Public Properties"

        Public WriteOnly Property DisplayTemplateName() As String
            Set(ByVal Value As String)
                _DisplayTemplateName = Value
            End Set
        End Property

        Public WriteOnly Property ReportForm() As String
            Set(ByVal Value As String)
                Try
                    _ReportForm = CBool(Value)
                Catch ex As Exception
                    _ReportForm = False
                End Try
            End Set
        End Property

        Public Property XMLData() As String
            Get
                _XMLData = getGenXML(dlCustomForm)
                _GenXMLrun = True
                Return _XMLData
            End Get
            Set(ByVal Value As String)
                _XMLData = Value
            End Set
        End Property

        Public WriteOnly Property UsrInfo() As DotNetNuke.Entities.Users.UserInfo
            Set(ByVal value As DotNetNuke.Entities.Users.UserInfo)
                _UserInfo = value
            End Set
        End Property


#End Region



        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            dlCustomForm = New Global.System.Web.UI.WebControls.DataList

            If Not _DisplayTemplateName Is Nothing Then
                Dim strData As String = GetStoreSettingText(PortalSettings.PortalId, _DisplayTemplateName, GetCurrentCulture)

                If strData <> "" Then
                    dlCustomForm.ItemTemplate = New GenXMLTemplate(Server.HtmlDecode(strData), _UserInfo)
                    dlCustomForm.Visible = True
                Else
                    dlCustomForm.Visible = False
                End If
            End If

            Me.Controls.Add(dlCustomForm)

            If _ReportForm Then

                Dim RepID As Integer = -1
                If Not (Request.QueryString("RepID") Is Nothing) Then
                    If IsNumeric(Request.QueryString("RepID")) Then
                        RepID = CInt(Request.QueryString("RepID"))
                    End If
                End If

                Dim frm As Boolean = False
                If Not (Request.QueryString("frm") Is Nothing) Then
                    frm = True
                End If

                If RepID >= 0 And frm And Not dlCustomForm Is Nothing Then
                    Dim objRep As New SQLReportController
                    Dim strData As String = objRep.BuildReportInputForm(RepID)

                    If strData <> "" Then
                        dlCustomForm.ItemTemplate = New GenXMLTemplate(strData)
                        dlCustomForm.Visible = True
                    Else
                        dlCustomForm.Visible = False
                    End If
                End If
            End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not Page.IsPostBack Then
                PopualteCustomForm()
            End If

        End Sub

        Private Sub PopualteCustomForm()
            'If Not _XMLData Is Nothing Then
            Dim objInfo As New XMLDataInfo

            If _XMLData Is Nothing Then _XMLData = "" ' never set data to nothing, may cause error

            objInfo.XMLData = _XMLData

            Dim aryList As New ArrayList
            aryList.Add(objInfo)

            _GenXMLrun = False

            dlCustomForm.DataSource = aryList
            dlCustomForm.DataBind()
            'End If
        End Sub

        Public Sub ReplaceField(ByVal ctrlID As String, ByVal CtrlType As String, ByVal NewValue As String, Optional ByVal cdata As Boolean = True)
            Dim xmlDoc As New XmlDataDocument
            Dim XPath As String = ""

            If Not ctrlID Is Nothing And Not _XMLData Is Nothing Then
                XPath = "genxml/" & CtrlType.ToString & "/" & ctrlID.ToLower
                If _XMLData = "" Then
                    'blank so populate
                    PopualteCustomForm()
                    _XMLData = getGenXML(dlCustomForm)
                End If

                xmlDoc.LoadXml(_XMLData)
                ReplaceXMLNode(xmlDoc, XPath, NewValue, cdata)
                _XMLData = xmlDoc.OuterXml
            End If

        End Sub

        Public Function getField(ByVal ctrlID As String, ByVal CtrlType As String) As String
            Dim xmlDoc As New XmlDataDocument
            Dim XPath As String = ""

            If Not _GenXMLrun Then
                If dlCustomForm.Controls.Count = 0 Then 'not poplulated so populate form
                    PopualteCustomForm()
                End If
                _XMLData = getGenXML(dlCustomForm)
                _GenXMLrun = True
            End If

            If ctrlID Is Nothing Or _XMLData Is Nothing Then
                Return ""
            Else
                XPath = "genxml/" & CtrlType.ToString & "/" & ctrlID.ToLower
            End If

            Dim strRtn As String = ""

            Try
                strRtn = SharedFunctions.getGenXMLvalue(_XMLData, XPath)
            Catch ex As Exception
                'could be blank XML so ignore.
            End Try

            Return strRtn
        End Function

        Private Class XMLDataInfo

#Region "Private Members"
            Private _XMLData As String
#End Region

#Region "Constructors"
            Public Sub New()
            End Sub
#End Region

#Region "Public Properties"
            Public Property XMLData() As String
                Get
                    Return _XMLData
                End Get
                Set(ByVal Value As String)
                    _XMLData = Value
                End Set
            End Property
#End Region

        End Class



    End Class


End Namespace
