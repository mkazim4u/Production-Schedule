Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports NEvoWeb.Modules.NB_Store
Imports System.Web.UI.WebControls

Namespace NEvoWeb.Provider.NB_Store.TagExtra

    Public Class TagExtra
        Inherits TagExtraInterface


        Public Overrides Function getExtraHtml(ByVal PortalID As Integer, ByVal container As DataListItem, ByVal CtrlData As String) As String
            'The return from this function is rendered directly 
            'on the product list and therefore must be html compliant.
            Dim strHtml As String = ""

            Select Case CtrlData.ToLower
                Case "1"
                    strHtml = "Hello World !"
                Case "2"
                    strHtml = "ByeBye World !"
                Case "productid"
                    Dim objPInfo As ProductListInfo
                    objPInfo = DirectCast(container.DataItem, ProductListInfo)

                    strHtml = objPInfo.ProductID.ToString

            End Select

            Return strHtml
        End Function


    End Class

End Namespace
