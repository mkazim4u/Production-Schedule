Imports System.IO
Imports System.Net

Partial Class SS_CopyImage
    Inherits System.Web.UI.UserControl
    Private Const productImages As String = "productimages"

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffGroup As New FFDataLayer.FF_Group

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then



        End If

    End Sub

    Protected Sub SaveData()

        Dim stxtName As String = txtGroupName.Text.Trim()


    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click



    End Sub
    Protected Sub CopyImageFromUrl()

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        'Dim sFolderRoot As String = ps.HomeDirectoryMapPath & "ProductionSchedule/"
        Dim DestinationPath As String = ps.HomeDirectoryMapPath & productImages

        Dim imageUrl As String = "http://www.sprintexpress.co.uk/common/prod_images/jpgs/15368.jpg"
        'Dim fileInfo As FileInfo
        'fileInfo.s()

        Dim url As String = "http://img510.imageshack.us/img510/5523/aspnet4jh.jpg"
        'Dim DestinationPath As String = "C:/temp"
        Dim filename As String = url.Substring(url.LastIndexOf("/"c) + 1)
        Dim bytes As Byte() = GetBytesFromUrl(url)
        WriteBytesToFile(DestinationPath + "/" + filename, bytes)

    End Sub

    Public Shared Sub WriteBytesToFile(fileName As String, content As Byte())
        Dim fs As New FileStream(fileName, FileMode.Create)
        Dim w As New BinaryWriter(fs)
        Try
            w.Write(content)
        Finally
            fs.Close()
            w.Close()
        End Try

    End Sub

    Public Shared Function GetBytesFromUrl(url As String) As Byte()
        Dim b As Byte()
        Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        Dim myResp As WebResponse = myReq.GetResponse()

        Dim stream As Stream = myResp.GetResponseStream()
        'int i;
        Using br As New BinaryReader(stream)
            'i = (int)(stream.Length);
            b = br.ReadBytes(500000)
            br.Close()
        End Using
        myResp.Close()
        Return b
    End Function

    Protected Sub TimerAutoSave_Tick(sender As Object, e As System.EventArgs) Handles TimerAutoSave.Tick

        Dim sGroupName As String = txtGroupName.Text.Trim()

        ffGroup.GroupName = sGroupName
        ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        ffGroup.CreatedOn = DateTime.Now
        dbContext.Add(ffGroup)
        dbContext.SaveChanges()

    End Sub

End Class
