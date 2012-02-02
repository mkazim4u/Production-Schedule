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


Namespace NEvoWeb.Modules.NB_Store

    Public Class CountryLists

        Public Function getCountryList(ByVal Lang As String) As DotNetNuke.Common.Lists.ListEntryInfoCollection
            Dim ctlEntry As DotNetNuke.Common.Lists.ListController = New DotNetNuke.Common.Lists.ListController

            'Use Bespoke country list if exists
            Dim entryCollection As DotNetNuke.Common.Lists.ListEntryInfoCollection = ctlEntry.GetListEntryInfoCollection("StoreCountry." & Lang)
            If entryCollection.Count = 0 Then
                entryCollection = ctlEntry.GetListEntryInfoCollection("Country")
            End If

            Return entryCollection
        End Function

        Public Function getCountryEntryInfo(ByVal CountryCode As String, ByVal Lang As String) As DotNetNuke.Common.Lists.ListEntryInfo
            Dim ctlEntry As DotNetNuke.Common.Lists.ListController = New DotNetNuke.Common.Lists.ListController
            Dim Key As String = CountryCode

            'Use Bespoke country list if exists
            Dim entryCollection As DotNetNuke.Common.Lists.ListEntryInfoCollection = ctlEntry.GetListEntryInfoCollection("StoreCountry." & Lang)
            If entryCollection.Count = 0 Then
                entryCollection = ctlEntry.GetListEntryInfoCollection("Country")
                Key = "Country:" & CountryCode
            Else
                Key = "StoreCountry." & Lang & ":" & CountryCode
            End If

            Return entryCollection.Item(Key)
        End Function

        Public Function getCountryName(ByVal Key As String, ByVal Lang As String) As String
            Dim cEntry As DotNetNuke.Common.Lists.ListEntryInfo = getCountryEntryInfo(Key, Lang)
            If cEntry Is Nothing Then
                Return ""
            Else
                Return cEntry.Text
            End If
        End Function

    End Class

End Namespace
