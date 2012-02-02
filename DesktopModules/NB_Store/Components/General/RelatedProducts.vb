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
Imports System.Configuration
Imports System.Data
Imports System.Xml
Imports System.Web
Imports System.Collections.Generic

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class RelatedProducts

#Region "Public Methods"

        Public Shared Sub AddProductFromWishList(ByVal PortalID As Integer, ByVal ProductID As String)
            Dim aryList As ArrayList
            Dim objCtrl As New ProductController

            aryList = WishList.GetList(PortalID)
            For Each objI As ProductListInfo In aryList
                AddProduct(PortalID, ProductID, objI.ProductID)
            Next
            WishList.ClearList(PortalID)
        End Sub

        Public Shared Sub AddProduct(ByVal PortalID As Integer, ByVal ProductID As String, ByVal RelatedProductID As String)
            Dim objCtrl As New ProductController
            Dim objRInfo As NB_Store_ProductRelatedInfo

            objRInfo = New NB_Store_ProductRelatedInfo
            objRInfo.RelatedID = -1
            objRInfo.PortalID = PortalID
            objRInfo.ProductID = ProductID
            objRInfo.RelatedProductID = RelatedProductID
            objRInfo.RelatedType = 1
            objRInfo.BiDirectional = True
            objRInfo.Disabled = False
            objRInfo.DiscountAmt = 0
            objRInfo.DiscountPercent = 0
            objRInfo.MaxQty = 0
            objRInfo.NotAvailable = False
            objRInfo.ProductQty = 0
            objCtrl.UpdateObjProductRelated(objRInfo)

        End Sub


#End Region


    End Class

End Namespace
