Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports NEvoWeb.Modules.NB_Store.WishList

Namespace NEvoWeb.Modules.NB_Store

    Public Class TokenTest

#Region "Private Members"
        Private _TestType As String = ""
        Private _Template As String = ""
        Private _ElseTemplate As String = ""
        Private _FieldXPath As String = ""
        Private _TestName As String = ""
        Private _TestValue As String = ""
        Private _Action As String = ""
        Private _TestOperator As String = ""

#End Region

#Region "Constructors"
        Public Sub New()
        End Sub
#End Region

#Region "Public Properties"
        Public Property TestType() As String
            Get
                Return _TestType
            End Get
            Set(ByVal Value As String)
                _TestType = Value
            End Set
        End Property

        Public Property Template() As String
            Get
                Return _Template
            End Get
            Set(ByVal Value As String)
                _Template = Value
            End Set
        End Property

        Public Property ElseTemplate() As String
            Get
                Return _ElseTemplate
            End Get
            Set(ByVal Value As String)
                _ElseTemplate = Value
            End Set
        End Property

        Public Property FieldXPath() As String
            Get
                Return _FieldXPath
            End Get
            Set(ByVal Value As String)
                _FieldXPath = Value
            End Set
        End Property

        Public Property TestName() As String
            Get
                Return _TestName
            End Get
            Set(ByVal Value As String)
                _TestName = Value
            End Set
        End Property

        Public Property TestValue() As String
            Get
                Return _TestValue
            End Get
            Set(ByVal Value As String)
                _TestValue = Value
            End Set
        End Property

        Public Property Action() As String
            Get
                Return _Action
            End Get
            Set(ByVal Value As String)
                _Action = Value
            End Set
        End Property

        Public Property TestOperator() As String
            Get
                Return _TestOperator
            End Get
            Set(ByVal Value As String)
                _TestOperator = Value
            End Set
        End Property


#End Region


#Region "Methods"

        Public Function getVisibleMode(ByVal PortalID As Integer, ByVal ModuleID As Integer, ByVal container As DataListItem, ByVal UsrInfo As DotNetNuke.Entities.Users.UserInfo, ByVal CurrentVisibleFlag As Boolean) As Boolean
            Dim strTestData As String = ""
            Dim strReturn As Boolean = True
            Dim ProductID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))
            Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")

            If Not CurrentVisibleFlag Then
                'If the current flag is telling us that the display is hidden
                ' the TAG:TEST token must be in a hidden area, therefore don't display it.
                Return False
            End If

            Dim _strTestType() As String
            Dim _strFieldXPath() As String
            Dim _strTestName() As String
            Dim _strTestValue() As String
            Dim blnOrTest As Boolean = False

            _strTestType = Split(_TestType, "|"c)
            _strTestName = Split(_TestName, "|"c)
            _strTestValue = Split(_TestValue, "|"c)
            _strFieldXPath = Split(_FieldXPath, "|"c)

            If _strTestType.GetUpperBound(0) > 0 Then blnOrTest = True

            For lp As Integer = 0 To _strTestType.GetUpperBound(0)

                Select Case _strTestType(lp).ToLower
                    Case "valueof"
                        If _strTestName(lp) <> "" Then
                            strTestData = DataBinder.Eval(container.DataItem, _strTestName(lp))
                            If strTestData Is Nothing Then
                                'try stock levels data class
                                Dim StockLevels As ProductStockLevels
                                StockLevels = getProductStockLevels(container)
                                If Not StockLevels Is Nothing Then
                                    strTestData = DataBinder.Eval(StockLevels, _strTestName(lp))
                                End If
                            End If
                        End If
                        If _strFieldXPath(lp) <> "" Then
                            strTestData = getGenXMLvalue(DataBinder.Eval(container.DataItem, "XMLData"), _strFieldXPath(lp))
                        End If

                        strReturn = getResult(strTestData, _strTestValue(lp))

                    Case "date"
                        If _strTestName(lp) <> "" Then
                            strTestData = DataBinder.Eval(container.DataItem, _strTestName(lp))
                        End If
                        If _strFieldXPath(lp) <> "" Then
                            strTestData = getGenXMLvalue(DataBinder.Eval(container.DataItem, "XMLData"), _strFieldXPath(lp))
                        End If

                        If _strTestValue(lp) = "Now" Then
                            _strTestValue(lp) = Now.ToString
                        End If

                        If _strTestValue(lp) = "Today" Then
                            _strTestValue(lp) = Today.ToString
                        End If

                        If IsDate(strTestData) And IsDate(_strTestValue(lp)) Then
                            strReturn = getResultDate(CType(strTestData, DateTime), CType(_strTestValue(lp), DateTime))
                        End If

                    Case "stock"
                        If _strTestName(lp) <> "" Then
                            'try stock levels data class
                            Dim StockLevels As ProductStockLevels
                            StockLevels = getProductStockLevels(container)
                            If Not StockLevels Is Nothing Then
                                strTestData = DataBinder.Eval(StockLevels, _strTestName(lp))
                            End If
                        End If
                        If _strFieldXPath(lp) <> "" Then
                            strTestData = getGenXMLvalue(DataBinder.Eval(container.DataItem, "XMLData"), _strFieldXPath(lp))
                        End If

                        If IsNumeric(strTestData) And IsNumeric(_strTestValue(lp)) Then
                            strReturn = getResultDouble(CType(strTestData, Double), CType(_strTestValue(lp), Double))
                        End If

                    Case "isinrole"
                        If UsrInfo.IsInRole(_strTestValue(lp)) Then
                            strReturn = getActionBoolean()
                        Else
                            strReturn = Not getActionBoolean()
                        End If
                    Case "setting"
                        Dim strData As String = GetStoreSetting(PortalID, _strTestName(lp), Lang)
                        Try
                            If CBool(_strTestValue(lp)) = CBool(strData) Then
                                strReturn = getActionBoolean()
                            Else
                                strReturn = Not getActionBoolean()
                            End If
                        Catch ex As Exception
                            If _strTestValue(lp) = strData Then
                                strReturn = getActionBoolean()
                            Else
                                strReturn = Not getActionBoolean()
                            End If
                        End Try
                    Case "modulesetting"
                        Dim hTable As Hashtable
                        Dim objCtrl As New DotNetNuke.Entities.Modules.ModuleController
                        hTable = objCtrl.GetModuleSettings(ModuleID)
                        Dim strData As String = hTable(_strTestName(lp)).ToString
                        Try
                            If CBool(_strTestValue(lp)) = CBool(strData) Then
                                strReturn = getActionBoolean()
                            Else
                                strReturn = Not getActionBoolean()
                            End If
                        Catch ex As Exception
                            If _strTestValue(lp) = strData Then
                                strReturn = getActionBoolean()
                            Else
                                strReturn = Not getActionBoolean()
                            End If
                        End Try
                    Case "hasmodels"
                        Dim objCtrl As New ProductController
                        Dim aryList As ArrayList
                        aryList = objCtrl.GetModelList(-1, ProductID, Lang, IsDealer(PortalID, UsrInfo, GetCurrentCulture))

                        If aryList.Count > 1 Then
                            strReturn = getActionBoolean()
                        Else
                            strReturn = Not getActionBoolean()
                        End If
                    Case "hasoptions"
                        Dim objCtrl As New ProductController
                        Dim aryList As ArrayList

                        aryList = objCtrl.GetOptionList(ProductID, Lang)
                        strReturn = GreaterOrEqual(aryList.Count)
                    Case "hasimages"
                        Dim objCtrl As New ProductController
                        Dim aryList As ArrayList

                        aryList = objCtrl.GetProductImageList(ProductID, Lang)
                        strReturn = GreaterOrEqual(aryList.Count)
                    Case "hasdocuments"
                        Dim objCtrl As New ProductController
                        Dim aryList As ArrayList

                        aryList = objCtrl.GetProductDocList(ProductID, Lang)
                        strReturn = GreaterOrEqual(aryList.Count)
                    Case "isinwishlist"
                        If IsInWishlist(PortalID, ProductID) Then
                            strReturn = getActionBoolean()
                        Else
                            strReturn = Not getActionBoolean()
                        End If
                    Case "hasrelateditems"
                        Dim objCtrl As New ProductController
                        Dim aryList As ArrayList
                        aryList = objCtrl.GetProductRelatedList(PortalID, ProductID, Lang, -1, False)
                        strReturn = GreaterOrEqual(aryList.Count)
                    Case "isproductlist"
                        If container.Parent.ID = "dlProductList" Then
                            strReturn = getActionBoolean()
                        Else
                            strReturn = Not getActionBoolean()
                        End If
                    Case "isproductdetail"
                        If container.Parent.ID = "dlProductDetail" Then
                            strReturn = getActionBoolean()
                        Else
                            strReturn = Not getActionBoolean()
                        End If
                    Case "isincategory"
                        Dim objPCtrl As New ProductController
                        Dim strCacheKey As String = ""

                        strCacheKey = GetCacheKey("isincategory_TEST_" & _Action, PortalID, ProductID)

                        If (DataCache.GetCache(strCacheKey) Is Nothing And strCacheKey <> "") Or GetStoreSettingBoolean(PortalID, "debug.mode", "None") Then
                            If objPCtrl.IsInCategory(ProductID, TestValue) Then
                                strReturn = getActionBoolean()
                            Else
                                strReturn = Not getActionBoolean()
                            End If
                        Else
                            strReturn = CBool(DataCache.GetCache(strCacheKey))
                        End If
                    Case "isonsale"
                        Dim strCacheKey As String = ""

                        strCacheKey = GetCacheKey("isonsale_TEST_" & _Action, PortalID, ProductID)
                        'Don't use cache for users, so role promo works.
                        If (DataCache.GetCache(strCacheKey) Is Nothing And strCacheKey <> "") Or GetStoreSettingBoolean(PortalID, "debug.mode", "None") Or UsrInfo.UserID >= 0 Then
                            Dim SalePrice As Double = 0
                            Dim ModelPrice As Double
                            Dim objCCtrl As New CartController
                            Dim objPCtrl As New ProductController
                            Dim objPromoCtrl As New PromoController
                            Dim aryList As ArrayList

                            aryList = objPCtrl.GetModelList(PortalID, ProductID, Lang, IsDealer(PortalID, UsrInfo, GetCurrentCulture))

                            ModelPrice = 9999999
                            For Each objInfo As NB_Store_ModelInfo In aryList
                                If objInfo.UnitCost < ModelPrice Then
                                    ModelPrice = objInfo.UnitCost
                                    SalePrice = objPromoCtrl.GetSalePrice(objInfo, UsrInfo)
                                End If
                            Next
                            If SalePrice <= 0 Or ModelPrice <= 0 Then
                                strReturn = Not getActionBoolean()
                            Else
                                strReturn = getActionBoolean()
                            End If

                            If UsrInfo.UserID = -1 Then ' only set cache for non-users
                                DataCache.SetCache(strCacheKey, strReturn.ToString, DateAdd(DateInterval.Day, 1, Now))
                            End If
                        Else
                            strReturn = CBool(DataCache.GetCache(strCacheKey))
                        End If

                End Select

                If blnOrTest And strReturn And _Action.ToLower = "show" Then
                    Return strReturn
                End If

                If blnOrTest And Not strReturn And _Action.ToLower = "hide" Then
                    Return strReturn
                End If

            Next

            Return strReturn
        End Function

        Private Function getResult(ByVal TestData As String, ByVal TestValue As String) As Boolean
            Select Case _TestOperator
                Case "<"
                    If TestData < TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case ">"
                    If TestData > TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case ">="
                    If TestData >= TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case "<="
                    If TestData <= TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case "<>"
                    If TestData <> TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case Else
                    If TestData = TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
            End Select

        End Function

        Private Function getResultDate(ByVal TestData As DateTime, ByVal TestValue As DateTime) As Boolean
            Select Case _TestOperator
                Case "<"
                    If TestData < TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case ">"
                    If TestData > TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case ">="
                    If TestData >= TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case "<="
                    If TestData <= TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case "<>"
                    If TestData <> TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case Else
                    If TestData = TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
            End Select

        End Function

        Private Function getResultDouble(ByVal TestData As Double, ByVal TestValue As Double) As Boolean
            Select Case _TestOperator
                Case "<"
                    If TestData < TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case ">"
                    If TestData > TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case ">="
                    If TestData >= TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case "<="
                    If TestData <= TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case "<>"
                    If TestData <> TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
                Case Else
                    If TestData = TestValue Then
                        Return getActionBoolean()
                    Else
                        Return Not getActionBoolean()
                    End If
            End Select

        End Function


        Private Function getActionBoolean() As Boolean
            If _Action.ToLower = "show" Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function GreaterOrEqual(ByVal itemCount As Integer) As Boolean
            If Not IsNumeric(_TestValue) Then ' no testvalue so check if exists
                If itemCount > 0 Then
                    Return getActionBoolean()
                Else
                    Return Not getActionBoolean()
                End If
            Else
                If itemCount >= CInt(_TestValue) Then
                    Return getActionBoolean()
                Else
                    Return Not getActionBoolean()
                End If
            End If
        End Function
#End Region


    End Class

End Namespace


