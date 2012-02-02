Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports NEvoWeb.Modules.NB_Store

Namespace NEvoWeb.Provider.NB_Store.TokenExtra

    Public Class TokenExtra
        Inherits TokenExtraInterface

        Public Overrides Function DoExtraReplace(ByVal PortalID As Integer, ByVal SourceText As String, ByVal Lang As String, ByVal OrderID As Integer) As String
            Dim rtnText As String = ""

            rtnText = Replace(SourceText, "[TokenExtra:Test]", "TOKEN EXTRA TEST...HELLO WORLD!!")

            Return rtnText
        End Function


    End Class

End Namespace
