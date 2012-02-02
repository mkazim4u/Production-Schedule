Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class SelectLang
        Inherits System.Web.UI.UserControl

        Public Event AfterChange(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs, ByVal PreviousEditLang As String)
        Public Event BeforeChange(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs, ByVal NewEditLang As String)
        Public Event ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)

#Region "Private Members"
        Private _SelectedFlagUrl As String
        Private _RepeatColumns As Integer = 0
        Private _DisplayNoLang As Boolean = False
        Private _Visible As Boolean = False
        Private PS As Entities.Portals.PortalSettings
#End Region


#Region "Public Properties"

        Public Property EditedLang() As String
            Get
                Return getStoreCookieValue(PS.PortalId, "AdminLang", "AdminEditedCulture")
            End Get
            Set(ByVal value As String)
                setStoreCookieValue(PS.PortalId, "AdminLang", "AdminEditedCulture", value, 1)
            End Set
        End Property


        Public Property SelectedLang() As String
            Get
                Dim cookieLang As String = getStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture")
                If Not _DisplayNoLang And cookieLang = "None" Then
                    Return GetCurrentCulture()
                End If
                Return cookieLang
            End Get
            Set(ByVal value As String)
                setStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture", value, 1)
            End Set
        End Property

        Public Property SelectedFlagUrl() As String
            Get
                Return _SelectedFlagUrl
            End Get
            Set(ByVal value As String)
                _SelectedFlagUrl = value
            End Set
        End Property

        Public Property RepeatColumns() As Integer
            Get
                Return _RepeatColumns
            End Get
            Set(ByVal value As Integer)
                _RepeatColumns = value
            End Set
        End Property

        Public Property DisplayNoLang() As Boolean
            Get
                Return _DisplayNoLang
            End Get
            Set(ByVal value As Boolean)
                _DisplayNoLang = value
            End Set
        End Property


#End Region

#Region "Event Handlers"

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            PS = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim cookieLang As String = getStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture")
            If cookieLang = "" Then
                setStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture", "None", 1)
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                buildMenu()
            End If
        End Sub

        Private Sub dlLanguages_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlLanguages.ItemCommand
            Dim item As DataListItem = e.Item
            Dim hid As HiddenField
            Dim PrevLang As String = ""
            Select Case e.CommandName
                Case "Change"
                    If Page.IsValid Then
                        hid = e.Item.FindControl("hidCultureCode")
                        If Not hid Is Nothing Then
                            RaiseEvent BeforeChange(source, e, hid.Value)
                            PrevLang = getStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture")
                            setStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture", hid.Value, 1)
                            buildMenu()
                            RaiseEvent AfterChange(source, e, PrevLang)
                        End If
                    End If
                Case Else
                    RaiseEvent ItemCommand(source, e)
            End Select

        End Sub

        Private Sub dlLanguages_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlLanguages.ItemDataBound
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

                Dim cmd As LinkButton
                Dim hid As HiddenField
                cmd = e.Item.FindControl("cmdFlagLang")
                hid = e.Item.FindControl("hidCultureCode")
                Dim cookieLang As String = getStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture")

                If Not cmd Is Nothing Then
                    hid.Value = e.Item.DataItem.ToString
                    cmd.ToolTip = e.Item.DataItem.ToString
                    If e.Item.DataItem.ToString = cookieLang Then
                        If hid.Value = "None" Then
                            cmd.Text = "<div style=""padding:1px;border:#ABABAB solid 1px;""><div style=""border:0px;width: 18px;height:12px;background:#dddddd;""></div></div>"
                        Else
                            _SelectedFlagUrl = DotNetNuke.Common.ResolveUrl("~/images/flags/" & e.Item.DataItem.ToString & ".gif")
                            cmd.Text = "<div style=""padding:1px;border:#ABABAB solid 1px;""><img src=""" & _SelectedFlagUrl & """  height=""12"" width=""18"" border=""0""/></div>"
                        End If
                    Else
                        If hid.Value = "None" Then
                            cmd.Text = "<div style=""padding:1px;border:white solid 1px;""><div style=""border:0px;width: 18px;height:12px;background:#dddddd;""></div></div>"
                        Else
                            cmd.Text = "<div style=""padding:1px;border:white solid 1px;""><img src=""" & DotNetNuke.Common.ResolveUrl("~/images/flags/" & e.Item.DataItem.ToString & ".gif") & """ height=""12"" width=""18"" border=""0"" /></div>"
                        End If
                    End If
                End If
            End If

        End Sub


#End Region

#Region "Methods"

        Public Sub Refresh()
            buildMenu()
        End Sub

        Private Sub buildMenu()
            Dim Locales As LocaleCollection = GetValidLocales()
            If Locales.Count > 1 Or _DisplayNoLang Then
                Dim LocalCol As New LocaleCollection

                If _DisplayNoLang Then
                    LocalCol.Add("None", "None")
                    For lp As Integer = 0 To (Locales.Count - 1)
                        LocalCol.Add(Locales.Item(lp).Key, Locales.Item(lp).Value)
                    Next
                Else
                    LocalCol = Locales
                End If

                If _RepeatColumns > 0 Then
                    dlLanguages.RepeatColumns = _RepeatColumns
                End If

                _Visible = True

                dlLanguages.DataSource = LocalCol
                dlLanguages.DataBind()
            Else
                setStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture", "None", 1)
                dlLanguages.Visible = False
                _Visible = False
            End If

        End Sub

#End Region

    End Class

End Namespace
