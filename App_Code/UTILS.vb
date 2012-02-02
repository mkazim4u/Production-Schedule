Imports Microsoft.VisualBasic

Public Class UTILS

    Public Shared Function ButtonFactory(ByVal sLegend As String) As String
        Dim sTextSize As String = "13" ' 13
        Dim sTextCOlour As String = "ffffff" ' ffffff
        Dim sButtonHeight As String = "20" ' 22
        Dim sButtonWidth As String = "110" ' 110
        Dim sTopBackgrounColour As String = "47c"
        Dim sBottomBackgrounColour As String = "238" ' 238
        Dim sBorderSize As String = "0" ' 0
        Dim sBorderColour As String = "000000" ' 000000
        ButtonFactory = "http://dabuttonfactory.com/b?t=" & sLegend & "&f=Calibri-Bold&ts=" & sTextSize & "&tc=" & sTextCOlour & "&tshs=1&tshc=222222&it=png&c=8&bgt=gradient&bgc=" & sTopBackgrounColour & "&ebgc=" & sBottomBackgrounColour & "&bs=" & sBorderSize & "&bc=" & sBorderColour & "&w=" & sButtonWidth & "&h=" & sButtonHeight
    End Function

End Class
