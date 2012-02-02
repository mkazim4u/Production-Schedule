Imports DotNetNuke.Entities.Modules

Partial Public Class View

    Inherits UserControl
    Public Property SilverlightApplication() As String
        Get
            Return m_SilverlightApplication
        End Get
        Set(value As String)
            m_SilverlightApplication = value
        End Set
    End Property
    Private m_SilverlightApplication As String
    Public Property SilverlightInitParams() As String
        Get
            Return m_SilverlightInitParams
        End Get
        Set(value As String)
            m_SilverlightInitParams = value
        End Set
    End Property
    Private m_SilverlightInitParams As String
    Protected Sub Page_Load(sender As Object, e As EventArgs)
        ' Register Silverlight.js file
        Page.ClientScript.RegisterClientScriptInclude(Me.[GetType](), "SilverlightJS", (Convert.ToString(Me.TemplateSourceDirectory) & "/Silverlight.js"))

        ' Set the Web Service URL
        'Dim strWebServiceURL As String = [String].Format("http://{0}/{1}", DNN.GetPMB(Me).PortalAlias.HTTPAlias, "DesktopModules//Service.svc")

        ' Set the path to the .xap file
        SilverlightApplication = [String].Format("{0}{1}", TemplateSourceDirectory, "/ClientBin/PSSilverLightDemo.xap")
        ' Pass the Initialization Parameters to the Silverlight Control
        'SilverlightInitParams = String.Format("WebServiceURL={0}", strWebServiceURL)
    End Sub
End Class

