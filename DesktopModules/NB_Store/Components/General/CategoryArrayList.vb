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


Imports System

Namespace NEvoWeb.Modules.NB_Store

    Public Class CategoryArrayList

        Protected CategoryArray As ArrayList

        Public Sub New(ByVal PortalID As Integer)
            Dim objCatCtrl As New CategoryController
            Dim aryList As New ArrayList
            Dim supportedLanguages As LocaleCollection = NEvoWeb.Modules.NB_Store.SharedFunctions.GetValidLocales()
            Dim objCatInfo As NB_Store_CategoriesInfo

            CategoryArray = New ArrayList

            For Each Lang As String In supportedLanguages
                aryList = objCatCtrl.GetCategories(PortalID, Lang)
                For Each objCatInfo In aryList
                    objCatInfo.CategoryName = objCatInfo.CategoryName
                    CategoryArray.Add(objCatInfo)
                Next
            Next

        End Sub

        Public Sub New(ByVal xmlDoc As Xml.XmlDataDocument)
            Dim aryList As New ArrayList
            Dim xmlNodList As Xml.XmlNodeList
            Dim xmlNod As Xml.XmlNode
            Dim objCatInfo As New NB_Store_CategoriesInfo

            Dim supportedLanguages As LocaleCollection = NEvoWeb.Modules.NB_Store.SharedFunctions.GetValidLocales()

            For Each Lang As String In supportedLanguages
                xmlNodList = xmlDoc.SelectNodes("root/categories/" & Lang & "/NB_Store_CategoriesInfo")
                For Each xmlNod In xmlNodList
                    objCatInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlNod.OuterXml, objCatInfo.GetType)
                    If Not objCatInfo Is Nothing Then
                        objCatInfo.CategoryName = objCatInfo.CategoryName
                        aryList.Add(objCatInfo)
                    End If
                Next
            Next

            CategoryArray = aryList

        End Sub

        Public Function GetCat(ByVal CatID As Integer, ByVal Lang As String) As NB_Store_CategoriesInfo
            Dim objCInfo As NB_Store_CategoriesInfo
            For Each objCInfo In CategoryArray
                If objCInfo.CategoryID = CatID And objCInfo.Lang = Lang Then
                    Return objCInfo
                End If
            Next
            Return Nothing
        End Function

        Public Function GetCatByName(ByVal CatQualifiedName As String, ByVal Lang As String) As NB_Store_CategoriesInfo
            Dim objCInfo As NB_Store_CategoriesInfo
            For Each objCInfo In CategoryArray
                If GetCatQualifiedName(objCInfo.CategoryID, Lang) = CatQualifiedName Then
                    Return objCInfo
                End If
            Next
            Return Nothing
        End Function

        Public Function GetCatQualifiedName(ByVal CategoryID As Integer, ByVal Lang As String) As String
            Dim objCInfo As NB_Store_CategoriesInfo
            For Each objCInfo In CategoryArray
                If objCInfo.Lang = Lang And objCInfo.CategoryID = CategoryID Then
                    If objCInfo.ParentCategoryID > 0 And objCInfo.ParentCategoryID <> CategoryID Then
                        Return GetCatQualifiedName(objCInfo.ParentCategoryID, Lang) & "/" & Left(objCInfo.CategoryName, 50)
                    Else
                        Return Left(objCInfo.CategoryName, 50)
                    End If
                End If
            Next
            Return ""
        End Function

    End Class
End Namespace
