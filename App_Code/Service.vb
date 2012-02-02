Imports FFDataLayer
Imports DotNetNuke

Public Class Service
    Implements IService

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffUser As New FFDataLayer.FF_User

    Public Function GetUsers() As DataTable Implements IService.GetUsers

        Dim sql As String = "select * from users"
        Dim dt As DataTable = DNNDB.Query(sql)
        Return dt

    End Function

    Public Function GetUserNameabc(ByVal str As String) As String Implements IService.GetUserNameabc

        Dim strUsername As String = "World!"

        'For Each user As FFDataLayer.FF_Tariff In dbContext.FF_Tariffs

        '    strUsername = user.TariffName

        'Next

        strUsername = strUsername + str

        Return strUsername

    End Function

End Class
