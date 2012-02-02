Imports Telerik.Web.UI

Partial Class SNR_HomePage
    Inherits System.Web.UI.UserControl

    Private dbContext As New SNRDentonDBLayerDataContext

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then


            PopulateCountry()
            LoadProgressPanelImage()

        End If

    End Sub

    Private Sub LoadProgressPanelImage()

        Dim img As System.Web.UI.WebControls.Image = UpdateProgressHome.FindControl("imgAjaxImage")
        img.ImageUrl = "~/Portals/3/Images/CircleAjaxLoading.gif"

    End Sub

    Private Sub PopulateCountry()

        Dim country = (From con In dbContext.SNR_Countries
                       Select con).ToList

        rcbCountry.DataSource = country
        rcbCountry.DataBind()



    End Sub
    Protected Sub rbAmerica_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Threading.Thread.Sleep(2000)

        Dim strFilter As String = String.Empty

        Dim ICountryList = (From country In dbContext.SNR_COUNTRies
                            Where country.Source = FF_GLOBALS.USA_PORTAL And country.Importance = 1
                            Select CountryKey = country.CountryKey, countryName = country.CountryName, source = country.Source).ToList


        rcbCountry.DataSource = ICountryList

        rcbCountry.DataBind()

    End Sub

    Protected Sub rbAsia_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Threading.Thread.Sleep(2000)


        Dim ICountryList = (From country In dbContext.SNR_COUNTRies
                            Where country.Source = FF_GLOBALS.ME_PORTAL And country.Importance = 1
                            Select CountryKey = country.CountryKey, countryName = country.CountryName, source = country.Source).ToList


        rcbCountry.DataSource = ICountryList

        rcbCountry.DataBind()

    End Sub


    Protected Sub rbEurope_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Threading.Thread.Sleep(2000)

        Dim strFilter As String = String.Empty

        Dim ICountryList = (From country In dbContext.SNR_COUNTRies
                            Where country.Source = FF_GLOBALS.UK_PORTAL And country.Importance = 1
                            Select CountryKey = country.CountryKey, countryName = country.CountryName, source = country.Source).ToList


        rcbCountry.DataSource = ICountryList

        rcbCountry.DataBind()


    End Sub
    Protected Sub rbAllCountries_CheckChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Threading.Thread.Sleep(2000)

        Dim strFilter As String = String.Empty

        Dim ICountryList = (From country In dbContext.SNR_COUNTRies
                            Select CountryKey = country.CountryKey, countryName = country.CountryName, source = country.Source).ToList


        rcbCountry.DataSource = ICountryList

        rcbCountry.DataBind()


    End Sub
    Protected Sub rcbCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As RadComboBoxSelectedIndexChangedEventArgs)

        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController

        Dim nCountryKey As Integer = Convert.ToInt64(rcbCountry.SelectedValue)

        Dim country = (From con In dbContext.SNR_COUNTRies Where con.CountryKey = nCountryKey
                       Select con).SingleOrDefault

        If country.Source = FF_GLOBALS.UK_PORTAL Then

            Response.Redirect(FF_GLOBALS.UK_PORTAL_URL)

        ElseIf country.Source = FF_GLOBALS.USA_PORTAL Then

            Response.Redirect(FF_GLOBALS.USA_PORTAL_URL)


        ElseIf country.Source = FF_GLOBALS.ME_PORTAL Then

            Response.Redirect(FF_GLOBALS.ME_PORTAL_URL)

        End If

    End Sub

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)

        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))

    End Sub



End Class
