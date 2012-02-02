

Partial Class PS_Group
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then


            'Dim obj_group As New FFDataLibrary.FFDataLibrary.FF_GroupName
            'obj_group.GroupName = ""
            'Dim db As New FFDataLibrary.FFDataLibrary.FFDBContext
            'db.Add(obj_group)

        End If
    End Sub


End Class
