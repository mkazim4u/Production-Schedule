Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports FFDataLayer
Imports Telerik.Web.UI
Imports System.Reflection
Imports Telerik.OpenAccess

Partial Class PS_Quotation
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    'Private ffTariffItem As New FFDataLayer.FF_TariffItem
    Private ffItem As New FFDataLayer.FF_Item
    Private ffGroup As New FFDataLayer.FF_Group
    Private ffTariff As New FFDataLayer.FF_Tariff
    Private sGridTableName As String
    Private sQuotationName As String = "Quotation"
    Private QUERY As String = "select Tariff.ID, Tariff.TariffName, Tariff.TariffOrder, Tariff.RecordType, Tariff.IsDeleted, Tariff.IsCreated, Tariff.JobID, Tariff.CustomerID, Tariff.LockedDateTime, Tariff.CreatedOn, Tariff.CreatedBy from ff_tariff Tariff where Tariff.IsDeleted = 'False' and Tariff.IsCreated = 'True'"

    Property pnJobID() As Integer
        Get
            Dim o As Object = ViewState("JobID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("JobID") = Value
        End Set
    End Property

    Property psQuery() As String
        Get
            Dim o As Object = ViewState("Query")
            If o Is Nothing Then
                Return QUERY
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("Query") = Value
        End Set
    End Property


    Protected Function GetJobIDFromQueryString() As String
        GetJobIDFromQueryString = String.Empty
        If Request.Params.Count > 0 Then
            Try
                GetJobIDFromQueryString = Request.Params("job")
            Catch
            End Try
        End If
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            pnJobID = GetJobIDFromQueryString()

            'CreateColumns()
            'CalculateTotalCost()

        End If

    End Sub

    Protected Sub rgFilter_ItemCommand(ByVal source As Object, ByVal e As GridCommandEventArgs) Handles rgFilter.ItemCommand
        If (e.CommandName = "Filter") Then

            For Each column As GridColumn In e.Item.OwnerTableView.Columns
                column.CurrentFilterValue = String.Empty
                column.CurrentFilterFunction = GridKnownFunction.NoFilter
            Next

        ElseIf e.CommandName = "Copy" Then

            Copy(e)
            rgQuotation.Rebind()

        End If

    End Sub

    Protected Sub rgQuotation_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgQuotation.NeedDataSource

        Dim query = From Tariff In dbContext.FF_Tariffs
                    Where Tariff.JobID = pnJobID And Tariff.IsDeleted = False And Tariff.RecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION
                    Select Tariff
        'pdtTemQuotation = query.ToList
        rgQuotation.DataSource = query.ToList


    End Sub

    Protected Sub SetQuery()



    End Sub

    Protected Sub tbSearchByQuotationName_TextChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim tbSearchByQuotationName As RadTextBox = sender
        Dim nJobID As Integer
        If Integer.TryParse(tbSearchByQuotationName.Text.Trim(), nJobID) Then
            QUERY = QUERY + " And JobID = " + tbSearchByQuotationName.Text.Trim()
        Else
            QUERY = QUERY + " And TariffName = '" + tbSearchByQuotationName.Text.Trim() + "'"
        End If

        rgFilter.Rebind()

    End Sub

    Protected Sub btnGoSearchByQuotationName_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim tbSearchByQuotationName As RadTextBox = sender
        Dim nJobID As Integer
        If Integer.TryParse(tbSearchByQuotationName.Text.Trim(), nJobID) Then
            QUERY = QUERY + " And JobID = " + tbSearchByQuotationName.Text.Trim()
        Else
            QUERY = QUERY + " And TariffName = '" + tbSearchByQuotationName.Text.Trim() + "'"
        End If

        rgFilter.Rebind()


    End Sub

    Protected Sub BindFilterGrid()

        Dim sRecordType As String

        If rcbFilter.SelectedValue = "0" Then
            sRecordType = FF_GLOBALS.RECORD_TYPE_TARIFF
            'rgFilter.MasterTableView.Columns.FindByDataField("JobID").Visible = False
        ElseIf rcbFilter.SelectedValue = "1" Then
            sRecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION
            'rgFilter.MasterTableView.Columns.FindByDataField("JobID").Visible = True

        ElseIf rcbFilter.SelectedValue = "-1" Then
            sRecordType = FF_GLOBALS.RECORD_TYPE_TARIFF
            rcbFilter.SelectedValue = "Tariff"
        End If

        QUERY = "select Tariff.ID, Tariff.TariffName, Tariff.TariffOrder, Tariff.RecordType, Tariff.IsDeleted, Tariff.IsCreated, Tariff.JobID, Tariff.CustomerID, Tariff.LockedDateTime, Tariff.CreatedOn, Tariff.CreatedBy from ff_tariff Tariff where Tariff.IsDeleted = 'False' and Tariff.IsCreated = 'True' and Tariff.RecordType = '" & sRecordType & "'"


        Dim dt As DataTable = DNNDB.Query(QUERY)

        rgFilter.DataSource = dt

        If dt.Rows.Count <> 0 Then

            If sRecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION Then

                rgFilter.MasterTableView.Columns.FindByUniqueName("JobID").Visible = True
                rgFilter.MasterTableView.Columns.FindByUniqueName("Copy").Visible = True

            Else

                rgFilter.MasterTableView.Columns.FindByUniqueName("JobID").Visible = False
                rgFilter.MasterTableView.Columns.FindByUniqueName("Copy").Visible = True

            End If

            rgFilter.MasterTableView.Columns.FindByUniqueName("Copy").Visible = True

        Else

            rgFilter.MasterTableView.Columns.FindByUniqueName("JobID").Visible = False
            rgFilter.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False

        End If




    End Sub

    Protected Sub rgFilter_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgFilter.NeedDataSource

        BindFilterGrid()

        'Dim sql As String = "Select JobID, TariffName, LockedDateTime from FF_Tariff"

    End Sub

    Protected Sub rgFilter_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgFilter.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim lblLockedDate As Label = e.Item.FindControl("lblLockedDate")

            If lblLockedDate.Text.Trim <> String.Empty Then
                lblLockedDate.Text = FF_GLOBALS.IsValidDate(lblLockedDate.Text)
            End If


        End If


    End Sub



    Protected Sub rgQuotation_DetailTableDataBind(ByVal source As Object, ByVal e As GridDetailTableDataBindEventArgs) Handles rgQuotation.DetailTableDataBind

        Select Case e.DetailTableView.Name

            Case "Groups"

                Dim nTariffId As String = e.DetailTableView.ParentItem.GetDataKeyValue("ID").ToString()


                Dim IGroups = From Groups In dbContext.FF_Groups
                              Where Groups.TariffID = nTariffId And Groups.IsDeleted = False
                              Select GroupId = Groups.ID, Groups.GroupName, Groups.GroupOrder, Groups.TariffID, Groups.CreatedOn, Groups.CreatedBy
                              Order By GroupOrder

                e.DetailTableView.DataSource = IGroups.ToList


            Case "Items"

                Dim nGroupId As String = e.DetailTableView.ParentItem.GetDataKeyValue("GroupID").ToString()

                Dim IItems = From Items In dbContext.FF_Items
                            Where Items.GroupID = nGroupId And Items.IsDeleted = False
                            Order By Items.ItemOrder
                            Select Items.ID, Items.GroupID, Items.CostPrice, Items.SellingPrice, Items.ItemName, Items.Units, Items.ItemOrder, Items.Quantity

                e.DetailTableView.DataSource = IItems.ToList

                'CalculateTotalCost()


        End Select

    End Sub

    Protected Sub Delete(ByVal nID As Int64, ByVal tableName As String)

        If tableName = "Items" Then

            ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))
            ffItem.IsDeleted = True
            'dbContext.Delete(ffItem)
            dbContext.SaveChanges()



        ElseIf tableName = "Groups" Then


            ffGroup = dbContext.GetObjectByKey(New ObjectKey(ffGroup.GetType().Name, nID))
            ffGroup.IsDeleted = True
            'dbContext.Delete(ffItem)
            dbContext.SaveChanges()



        ElseIf tableName = "Quotation" Then

            ffTariff = dbContext.GetObjectByKey(New ObjectKey(ffTariff.GetType().Name, nID))
            ffTariff.IsDeleted = True
            'dbContext.Delete(ffItem)
            dbContext.SaveChanges()

        End If

    End Sub

    Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If (TypeOf e.Item Is GridDataInsertItem Or TypeOf e.Item Is Telerik.Web.UI.GridEditFormInsertItem) AndAlso e.Item.IsInEditMode Then

            If "Quotation".Equals(e.Item.OwnerTableView.Name) Then

                Dim txtTariffName As TextBox = e.Item.FindControl("txtTariffName")
                ffTariff.TariffName = txtTariffName.Text.Trim()
                ffTariff.RecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION
                ffTariff.IsDeleted = False
                ffTariff.LockedDateTime = FF_GLOBALS.BASE_DATE
                ffTariff.JobID = pnJobID
                ffTariff.CreatedOn = DateTime.Now
                ffTariff.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                dbContext.Add(ffTariff)
                dbContext.SaveChanges()

            ElseIf "Groups".Equals(e.Item.OwnerTableView.Name) Then

                Dim txtGroupName As TextBox = e.Item.FindControl("txtGroupName")
                Dim nGroupOrder As Int64 = e.Item.OwnerTableView.Items.Count + 1
                ffGroup.GroupOrder = nGroupOrder
                ffGroup.IsDeleted = False
                ffGroup.GroupName = txtGroupName.Text.Trim()
                ffGroup.CreatedOn = DateTime.Now
                ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                ffGroup.TariffID = e.Item.OwnerTableView.ParentItem.GetDataKeyValue("ID")
                dbContext.Add(ffGroup)
                dbContext.SaveChanges()

            ElseIf "Items".Equals(e.Item.OwnerTableView.Name) Then

                Dim txtItemName As TextBox = e.Item.FindControl("txtItemName")
                Dim rntbCostPrice As RadNumericTextBox = e.Item.FindControl("rntbCostPrice")
                Dim rntbSellingPrice As RadNumericTextBox = e.Item.FindControl("rntbSellingPrice")
                Dim rntbQuantity As RadNumericTextBox = e.Item.FindControl("rntbQuantity")
                Dim rntbRate As RadNumericTextBox = e.Item.FindControl("rntbRate")

                Dim nItemOrder As Int64 = e.Item.OwnerTableView.Items.Count + 1
                ffItem.ItemOrder = nItemOrder
                ffItem.IsDeleted = False
                ffItem.ItemName = txtItemName.Text.Trim()

                If rntbCostPrice.Text.Trim() <> String.Empty Then
                    ffItem.CostPrice = rntbCostPrice.Text.Trim()
                Else
                    ffItem.CostPrice = 0
                End If

                If rntbSellingPrice.Text.Trim() <> String.Empty Then
                    ffItem.SellingPrice = rntbSellingPrice.Text
                Else
                    ffItem.SellingPrice = 0
                End If

                If rntbQuantity.Text.Trim() <> String.Empty Then
                    ffItem.Quantity = rntbQuantity.Text.Trim()
                Else
                    ffItem.Quantity = 0
                End If

                If rntbRate.Text.Trim = String.Empty Then
                    ffItem.Units = 1
                Else
                    ffItem.Units = rntbRate.Text.Trim
                End If

                ffItem.CreatedOn = DateTime.Now
                ffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                ffItem.GroupID = e.Item.OwnerTableView.ParentItem.GetDataKeyValue("GroupID")
                dbContext.Add(ffItem)
                dbContext.SaveChanges()

            End If


        End If


        'Call CalculateTotalCost()
        'Call CalculateTotalCost()

    End Sub

    Protected Sub Update(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If TypeOf e.Item Is GridEditableItem AndAlso e.Item.IsInEditMode Then

            If "Quotation".Equals(e.Item.OwnerTableView.Name) Then

                Dim nID As Int64
                Dim data As GridEditFormItem = e.Item
                nID = Convert.ToInt64(data.ParentItem("ID").Text)

                Dim txtTariffName As TextBox = e.Item.FindControl("txtTariffName")

                ffTariff = dbContext.GetObjectByKey(New ObjectKey(ffTariff.GetType().Name, nID))
                ffTariff.TariffName = txtTariffName.Text.Trim()
                ffTariff.CreatedOn = DateTime.Now
                ffTariff.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                dbContext.SaveChanges()

            ElseIf "Groups".Equals(e.Item.OwnerTableView.Name) Then

                Dim nID As Int64
                Dim data As GridEditFormItem = e.Item
                nID = Convert.ToInt64(data.ParentItem("GroupID").Text)

                Dim txtGroupName As TextBox = e.Item.FindControl("txtGroupName")

                ffGroup = dbContext.GetObjectByKey(New ObjectKey(ffGroup.GetType().Name, nID))
                ffGroup.GroupName = txtGroupName.Text.Trim()
                ffGroup.CreatedOn = DateTime.Now
                ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                ffGroup.TariffID = e.Item.OwnerTableView.ParentItem.GetDataKeyValue("ID")
                dbContext.SaveChanges()

            ElseIf "Items".Equals(e.Item.OwnerTableView.Name) Then

                Dim nID As Int64

                Dim data As GridDataItem = e.Item
                'Dim data As GridEditFormItem = e.Item                      for editformitem
                'nID = Convert.ToInt64(data.ParentItem("ID").Text)

                nID = Convert.ToInt64(data.GetDataKeyValue("ID"))
                ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))


                Dim txtItemName As TextBox = e.Item.FindControl("txtItemName")
                Dim rntbCostPrice As RadNumericTextBox = e.Item.FindControl("rntbCostPrice")
                Dim rntbSellingPrice As RadNumericTextBox = e.Item.FindControl("rntbSellingPrice")
                Dim rntbRate As RadNumericTextBox = e.Item.FindControl("rntbRate")
                Dim rntbQuantity As RadNumericTextBox = e.Item.FindControl("rntbQuantity")

                'Dim nItemOrder As Int64 = e.Item.OwnerTableView.Items.Count + 1
                'ffItem.ItemOrder = nItemOrder
                ffItem.IsDeleted = False
                ffItem.ItemName = txtItemName.Text.Trim()
                If rntbCostPrice.Text.Trim() <> String.Empty Then
                    ffItem.CostPrice = rntbCostPrice.Text.Trim()
                Else
                    ffItem.CostPrice = 0
                End If

                If rntbSellingPrice.Text.Trim() <> String.Empty Then
                    ffItem.SellingPrice = rntbSellingPrice.Text
                Else
                    ffItem.SellingPrice = 0
                End If

                If rntbQuantity.Text.Trim() <> String.Empty Then
                    ffItem.Quantity = rntbQuantity.Text.Trim()
                Else
                    ffItem.Quantity = 0
                End If

                If rntbRate.Text.Trim = String.Empty Then
                    ffItem.Units = 1
                Else
                    ffItem.Units = rntbRate.Text.Trim
                End If
                ffItem.CreatedOn = DateTime.Now
                ffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                ffItem.GroupID = e.Item.OwnerTableView.ParentItem.GetDataKeyValue("GroupID")
                dbContext.SaveChanges()

            End If


        End If

        'CalculateTotalCost()



    End Sub

    Protected Sub CopyItem(ByVal nID As Int64, ByVal tableName As String)

        If tableName = "Items" Then

            ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))

            Dim ffNewItem As New FFDataLayer.FF_Item

            ffNewItem.CostPrice = ffItem.CostPrice
            ffNewItem.SellingPrice = ffItem.SellingPrice
            ffNewItem.Quantity = ffItem.Quantity
            ffNewItem.ItemOrder = ffItem.ItemOrder + 1
            ffNewItem.Units = ffItem.Units
            ffNewItem.ItemName = ffItem.ItemName
            ffNewItem.IsDeleted = ffItem.IsDeleted
            ffNewItem.GroupID = ffItem.GroupID
            ffNewItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffNewItem.CreatedOn = DateTime.Now
            dbContext.Add(ffNewItem)
            dbContext.SaveChanges()

        End If

    End Sub

    Protected Sub rcbItem_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

        Dim rcbItem As RadComboBox = o
        Dim nID As Int64 = Convert.ToInt64(rcbItem.SelectedValue)
        ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))

        Dim txtPrice As TextBox = rcbItem.NamingContainer.FindControl("txtPrice")
        txtPrice.Text = ffItem.CostPrice

    End Sub

    Protected Sub rgQuotation_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgQuotation.ItemDataBound

        If TypeOf e.Item Is GridDataItem And e.Item.OwnerTableView.Name = "Items" Then

            'Dim itemCost As Decimal = e.Item.OwnerTableView.Columns.FindByUniqueName("").t 

        ElseIf TypeOf e.Item Is GridFooterItem And e.Item.OwnerTableView.Name = "Groups" Then

            CalculateTotalCost()
            'CalculateTotalSelling()

        End If


    End Sub

    Protected Sub CalculateTotalCost(Optional ByVal e As Telerik.Web.UI.GridCommandEventArgs = Nothing)

        Dim dTotalCost As Decimal
        Dim dCostPrice As Decimal
        Dim nQuantity As Integer
        Dim nRate As Integer

        For Each gdiQuotation As GridDataItem In rgQuotation.MasterTableView.Items

            dTotalCost = 0

            For Each gdiGroups As GridDataItem In gdiQuotation.ChildItem.NestedTableViews(0).Items

                ''''''''''''''''''''''''''''''''''' This step is necessary for For First Item ................................

                If e IsNot Nothing Then

                    Dim count As Integer = gdiGroups.ChildItem.NestedTableViews(0).Items.Count

                    If (e.Item.IsInEditMode Or e.Item.OwnerTableView.IsItemInserted) And count = 0 Then

                        Dim rntbRate As RadNumericTextBox = e.Item.FindControl("rntbRate")
                        Dim rntbQuantity As RadNumericTextBox = e.Item.FindControl("rntbQuantity")
                        Dim rntbCostPrice As RadNumericTextBox = e.Item.FindControl("rntbCostPrice")


                        nRate = Convert.ToInt64(rntbRate.Text.Trim())
                        dCostPrice = Convert.ToDecimal(rntbCostPrice.Text.Trim())
                        nQuantity = Convert.ToInt64(rntbQuantity.Text.Trim())
                        dTotalCost = (dCostPrice * nQuantity) / nRate + dTotalCost

                        Dim nestedView As GridTableView = gdiQuotation.ChildItem.NestedTableViews(0)

                        For Each childfooter As GridFooterItem In nestedView.GetItems(GridItemType.Footer)

                            Dim lblTotalCost As Label = DirectCast(childfooter.FindControl("lblTotalCost"), Label)

                            lblTotalCost.Text = "Total C.P £" + dTotalCost.ToString()

                        Next

                    End If

                End If


                For i As Integer = 0 To gdiGroups.ChildItem.NestedTableViews(0).Items.Count - 1

                    Dim gdiItem As GridDataItem = gdiGroups.ChildItem.NestedTableViews(0).Items(i)

                    If TypeOf gdiItem Is GridDataItem And gdiItem.IsInEditMode Then

                        Dim rntbRate As RadNumericTextBox = gdiItem.FindControl("rntbRate")
                        Dim rntbQuantity As RadNumericTextBox = gdiItem.FindControl("rntbQuantity")
                        Dim rntbCostPrice As RadNumericTextBox = gdiItem.FindControl("rntbCostPrice")

                        nRate = Convert.ToInt64(rntbRate.Text.Trim())

                        If rntbCostPrice.Text.Trim <> String.Empty Then
                            dCostPrice = Convert.ToDecimal(rntbCostPrice.Text.Trim())
                        Else
                            dCostPrice = 0
                        End If


                        If rntbQuantity.Text.Trim <> String.Empty Then
                            nQuantity = Convert.ToInt64(rntbQuantity.Text.Trim())
                        Else
                            nQuantity = 0
                        End If

                        dTotalCost = (dCostPrice * nQuantity) / nRate + dTotalCost


                    ElseIf gdiItem.OwnerTableView.IsItemInserted Then

                        Dim rntbRate As RadNumericTextBox = e.Item.FindControl("rntbRate")
                        Dim rntbQuantity As RadNumericTextBox = e.Item.FindControl("rntbQuantity")
                        Dim rntbCostPrice As RadNumericTextBox = e.Item.FindControl("rntbCostPrice")


                        nRate = Convert.ToInt64(rntbRate.Text.Trim())

                        If rntbCostPrice.Text.Trim <> String.Empty Then
                            dCostPrice = Convert.ToDecimal(rntbCostPrice.Text.Trim())
                        Else
                            dCostPrice = 0
                        End If

                        If rntbQuantity.Text.Trim <> String.Empty Then
                            nQuantity = Convert.ToInt64(rntbQuantity.Text.Trim())
                        Else
                            nQuantity = 0
                        End If


                        dTotalCost = (dCostPrice * nQuantity) / nRate + dTotalCost

                        dTotalCost = dTotalCost + Convert.ToDecimal(gdiItem("TotalCostPrice").Text)

                    ElseIf TypeOf gdiItem Is GridDataItem Then

                        dTotalCost = dTotalCost + Convert.ToDecimal(gdiItem("TotalCostPrice").Text)

                    End If

                    Dim nestedView As GridTableView = gdiQuotation.ChildItem.NestedTableViews(0)

                    For Each childfooter As GridFooterItem In nestedView.GetItems(GridItemType.Footer)

                        Dim lblTotalCost As Label = DirectCast(childfooter.FindControl("lblTotalCost"), Label)

                        lblTotalCost.Text = "£" + dTotalCost.ToString()

                    Next



                Next

            Next

        Next

    End Sub

    Protected Sub CalculateTotalSelling(Optional ByVal e As Telerik.Web.UI.GridCommandEventArgs = Nothing)

        Dim dTotalSelling As Decimal
        Dim dSellingPrice As Decimal
        Dim nQuantity As Integer
        Dim nRate As Integer

        For Each gdiQuotation As GridDataItem In rgQuotation.MasterTableView.Items

            dTotalSelling = 0

            For Each gdiGroups As GridDataItem In gdiQuotation.ChildItem.NestedTableViews(0).Items

                ''''''''''''''''''''''''''''''''''' This step is necessary for For First Item ................................

                If e IsNot Nothing Then

                    Dim count As Integer = gdiGroups.ChildItem.NestedTableViews(0).Items.Count

                    If (e.Item.IsInEditMode Or e.Item.OwnerTableView.IsItemInserted) And count = 0 Then

                        Dim rntbRate As RadNumericTextBox = e.Item.FindControl("rntbRate")
                        Dim rntbQuantity As RadNumericTextBox = e.Item.FindControl("rntbQuantity")
                        Dim rntbSellingPrice As RadNumericTextBox = e.Item.FindControl("rntbSellingPrice")


                        nRate = Convert.ToInt64(rntbRate.Text.Trim())
                        dSellingPrice = Convert.ToDecimal(rntbSellingPrice.Text.Trim())
                        nQuantity = Convert.ToInt64(rntbQuantity.Text.Trim())
                        dTotalSelling = (dSellingPrice * nQuantity) / nRate + dTotalSelling

                        Dim nestedView As GridTableView = gdiQuotation.ChildItem.NestedTableViews(0)

                        For Each childfooter As GridFooterItem In nestedView.GetItems(GridItemType.Footer)

                            Dim lblTotalCost As Label = DirectCast(childfooter.FindControl("lblTotalCost"), Label)

                            lblTotalCost.Text = "Total S.P £" + dTotalSelling.ToString()

                        Next

                    End If

                End If


                For i As Integer = 0 To gdiGroups.ChildItem.NestedTableViews(0).Items.Count - 1

                    Dim gdiItem As GridDataItem = gdiGroups.ChildItem.NestedTableViews(0).Items(i)

                    If TypeOf gdiItem Is GridDataItem And gdiItem.IsInEditMode Then

                        Dim rntbRate As RadNumericTextBox = gdiItem.FindControl("rntbRate")
                        Dim rntbQuantity As RadNumericTextBox = gdiItem.FindControl("rntbQuantity")
                        Dim rntbCostPrice As RadNumericTextBox = gdiItem.FindControl("rntbSellingPrice")

                        nRate = Convert.ToInt64(rntbRate.Text.Trim())
                        dSellingPrice = Convert.ToDecimal(rntbCostPrice.Text.Trim())
                        nQuantity = Convert.ToInt64(rntbQuantity.Text.Trim())
                        dTotalSelling = (dSellingPrice * nQuantity) / nRate + dTotalSelling


                    ElseIf gdiItem.OwnerTableView.IsItemInserted Then

                        Dim rntbRate As RadNumericTextBox = e.Item.FindControl("rntbRate")
                        Dim rntbQuantity As RadNumericTextBox = e.Item.FindControl("rntbQuantity")
                        Dim rntbCostPrice As RadNumericTextBox = e.Item.FindControl("rntbSellingPrice")


                        nRate = Convert.ToInt64(rntbRate.Text.Trim())
                        dSellingPrice = Convert.ToDecimal(rntbCostPrice.Text.Trim())
                        nQuantity = Convert.ToInt64(rntbQuantity.Text.Trim())
                        dTotalSelling = (dSellingPrice * nQuantity) / nRate + dTotalSelling

                        dTotalSelling = dTotalSelling + Convert.ToDecimal(gdiItem("TotalSellingPrice").Text)

                    ElseIf TypeOf gdiItem Is GridDataItem Then

                        dTotalSelling = dTotalSelling + Convert.ToDecimal(gdiItem("TotalSellingPrice").Text)

                    End If

                    Dim nestedView As GridTableView = gdiQuotation.ChildItem.NestedTableViews(0)

                    For Each childfooter As GridFooterItem In nestedView.GetItems(GridItemType.Footer)

                        Dim lblTotalSelling As Label = DirectCast(childfooter.FindControl("lblTotalSelling"), Label)

                        lblTotalSelling.Text = "£" + dTotalSelling.ToString()

                    Next



                Next

            Next

        Next

    End Sub

    Protected Sub SetColor(ByVal costPrice As Decimal, ByVal sellingPrice As Decimal)

        If costPrice > sellingPrice Then
            WebMsgBox.Show("cost price is greater than selling price")
        End If

    End Sub

    Protected Sub rgQuotation_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgQuotation.ItemCommand

        If e.CommandName = "PerformInsert" Then

            Insert(e)
            CalculateTotalCost(e)
            'CalculateTotalSelling(e)

        ElseIf e.CommandName = "Update" Then
            Update(e)
            CalculateTotalCost()
            'CalculateTotalSelling(e)
        End If

    End Sub

    Protected Sub Copy(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        Dim hidTariffID As HiddenField = e.Item.FindControl("hidID")
        Dim nTariffID As Integer = hidTariffID.Value

        Dim sb As New StringBuilder

        sb.Append("select ff_group.id 'groupid',groupname,grouporder, ff_group.tariffid, ff_group.IsDeleted from ff_group inner join ff_tariff on ff_group.tariffid = ff_tariff.id ")
        sb.Append("where ff_group.tariffid = " & nTariffID)

        Dim sql As String = sb.ToString()
        Dim dtGroups As DataTable = DNNDB.Query(sql)


        ffTariff.TariffName = sQuotationName + " " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
        ffTariff.RecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION
        ffTariff.IsDeleted = False
        ffTariff.IsCreated = False
        ffTariff.JobID = pnJobID
        'ffTariff.
        ffTariff.LockedDateTime = FF_GLOBALS.BASE_DATE
        ffTariff.CreatedOn = DateTime.Now
        ffTariff.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        dbContext.Add(ffTariff)
        dbContext.SaveChanges()

        If dtGroups IsNot Nothing And dtGroups.Rows.Count <> 0 Then

            For Each dr As DataRow In dtGroups.Rows

                ffGroup = New FFDataLayer.FF_Group
                ffGroup.GroupOrder = Convert.ToInt64(dr("grouporder"))
                ffGroup.IsDeleted = Convert.ToBoolean(dr("IsDeleted"))
                ffGroup.GroupName = dr("groupname")
                ffGroup.CreatedOn = DateTime.Now
                ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                ffGroup.TariffID = ffTariff.ID
                dbContext.Add(ffGroup)
                dbContext.SaveChanges()

                Dim nGroupIdToCollectItems As Integer = dr("groupid")
                Dim nGroupId As Integer = ffGroup.ID

                sb = New StringBuilder()

                sb.Append("select ff_item.id, ff_item.itemname, ff_item.itemorder, ff_item.quantity, ff_item.units, ff_item.costprice, ff_item.sellingprice,FF_Item.GroupID, FF_Item.IsDeleted ")
                sb.Append("from ff_item inner join ff_group ")
                sb.Append("on ff_item.groupid = ff_group.id ")
                sb.Append("where ff_item.groupid = " & nGroupIdToCollectItems)

                sql = sb.ToString()

                Dim dtItems As DataTable = DNNDB.Query(sql)


                If dtItems IsNot Nothing And dtItems.Rows.Count <> 0 Then

                    For Each drItem As DataRow In dtItems.Rows

                        ffItem = New FF_Item
                        ffItem.ItemName = drItem("ItemName")
                        ffItem.ItemOrder = drItem("ItemOrder")
                        ffItem.Quantity = drItem("Quantity")
                        ffItem.CostPrice = drItem("CostPrice")
                        ffItem.SellingPrice = drItem("SellingPrice")
                        ffItem.CreatedOn = DateTime.Now
                        ffItem.Units = drItem("Units")
                        ffItem.GroupID = nGroupId
                        ffItem.IsDeleted = drItem("IsDeleted")
                        ffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                        dbContext.Add(ffItem)
                        dbContext.SaveChanges()

                    Next

                End If

            Next

        End If

    End Sub

    Protected Function ToggleButtons() As String

        Dim sText As String

        If rgQuotation.MasterTableView.IsItemInserted Or rgQuotation.MasterTableView.DetailTables(0).IsItemInserted Or rgQuotation.MasterTableView.DetailTables(1).IsItemInserted Then
            sText = "Insert"
        Else
            sText = "Update"
        End If

        Return sText

    End Function

    Protected Function ToggleCommand() As String

        Dim sText As String

        If rgQuotation.MasterTableView.IsItemInserted Or rgQuotation.MasterTableView.DetailTables(0).IsItemInserted Then
            sText = "PerformInsert"
        Else
            sText = "Update"
        End If

        Return sText

    End Function

    Protected Sub rcMenu_ItemClick(ByVal sender As Object, ByVal e As RadMenuEventArgs)

        Dim radGridClickedRowIndex As Integer
        Dim UId As String

        Dim indices As String() = Request.Form("radGridClickedRowIndex").Split("_"c)
        radGridClickedRowIndex = Convert.ToInt32(indices(indices.Length - 1))
        UId = Request.Form("radGridClickedTableId")

        Dim tableView As GridTableView

        Select Case e.Item.Text

            Case "Edit"
                tableView = TryCast(Me.Page.FindControl(UId), GridTableView)
                TryCast(TryCast(tableView, GridTableView).Items(radGridClickedRowIndex), GridItem).Edit = True
                tableView.Rebind()
                Exit Select

            Case "Add"
                tableView = TryCast(Me.Page.FindControl(UId), GridTableView)
                tableView.InsertItem()
                'TryCast(TryCast(tableView., GridTableView).Items(radGridClickedRowIndex), GridItem).Selected = True
                'rgQuotation.MasterTableView.IsItemInserted = True
                'tableView.Rebind()
                Exit Select

            Case "Delete"

                tableView = TryCast(Me.Page.FindControl(UId), GridTableView)

                If tableView.Name = "Quotation" Then
                    Dim nID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("ID"))
                    sGridTableName = "Quotation"
                    Delete(nID, sGridTableName)
                    tableView.Rebind()
                    CalculateTotalCost()
                    'CalculateTotalSelling()
                ElseIf tableView.Name = "Groups" Then
                    Dim nID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("GroupID"))
                    sGridTableName = "Groups"
                    Delete(nID, sGridTableName)
                    tableView.Rebind()
                    CalculateTotalCost()
                    'CalculateTotalSelling()
                ElseIf tableView.Name = "Items" Then
                    Dim nID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("ID"))
                    sGridTableName = "Items"
                    Delete(nID, sGridTableName)
                    tableView.Rebind()
                    CalculateTotalCost()
                    'CalculateTotalSelling()
                End If

                Exit Select

            Case "Copy"


                tableView = TryCast(Me.Page.FindControl(UId), GridTableView)

                If tableView.Name = "Items" Then

                    Dim nID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("ID"))
                    sGridTableName = "Items"
                    CopyItem(nID, sGridTableName)
                    tableView.Rebind()

                End If


                Exit Select

            Case "Move Up"

                tableView = TryCast(Me.Page.FindControl(UId), GridTableView)

                If tableView.Name = "Items" Then

                    Dim nClickedRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("ID"))
                    sGridTableName = "Items"

                    Row_MoveUp(nClickedRowID, sGridTableName)

                    If radGridClickedRowIndex > 0 Then

                        Dim nAboveRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex - 1).GetDataKeyValue("ID"))
                        Row_MoveDown(nAboveRowID, sGridTableName)

                    End If

                    tableView.Rebind()

                ElseIf tableView.Name = "Groups" Then

                    sGridTableName = "Groups"
                    Dim nClickedRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("GroupID"))

                    Row_MoveUp(nClickedRowID, sGridTableName)

                    If radGridClickedRowIndex > 0 Then

                        Dim nAboveRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex - 1).GetDataKeyValue("GroupID"))
                        Row_MoveDown(nAboveRowID, sGridTableName)

                    End If

                    tableView.Rebind()


                End If


                Exit Select

            Case "Move Down"

                tableView = TryCast(Me.Page.FindControl(UId), GridTableView)

                If tableView.Name = "Items" Then

                    Dim nClickedRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("ID"))
                    sGridTableName = "Items"


                    If radGridClickedRowIndex < tableView.Items.Count - 1 Then

                        Row_MoveDown(nClickedRowID, sGridTableName)

                        If radGridClickedRowIndex < tableView.Items.Count Then

                            Dim nBelowRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex + 1).GetDataKeyValue("ID"))
                            Row_MoveUp(nBelowRowID, sGridTableName)

                        End If

                    End If

                    tableView.Rebind()

                ElseIf tableView.Name = "Groups" Then

                    Dim nClickedRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("GroupID"))
                    sGridTableName = "Groups"
                    If radGridClickedRowIndex < tableView.Items.Count - 1 Then

                        Row_MoveDown(nClickedRowID, sGridTableName)

                        If radGridClickedRowIndex < tableView.Items.Count Then

                            Dim nBelowRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex + 1).GetDataKeyValue("GroupID"))
                            Row_MoveUp(nBelowRowID, sGridTableName)

                        End If

                    End If

                    tableView.Rebind()


                End If

                Exit Select


        End Select
    End Sub

    Protected Sub Row_MoveDown(ByVal nID As Int64, ByVal tableName As String)

        If tableName = "Items" Then

            ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))
            ffItem.ItemOrder = ffItem.ItemOrder + 1
            dbContext.SaveChanges()

        ElseIf tableName = "Groups" Then

            ffGroup = dbContext.GetObjectByKey(New ObjectKey(ffGroup.GetType().Name, nID))
            ffGroup.GroupOrder = ffGroup.GroupOrder + 1
            dbContext.SaveChanges()

        End If


    End Sub

    Protected Sub Row_MoveUp(ByVal nID As Int64, ByVal tableName As String)

        If tableName = "Items" Then

            ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))
            Dim nPosition As Integer = ffItem.ItemOrder
            If nPosition > 1 Then
                ffItem.ItemOrder = ffItem.ItemOrder - 1
                dbContext.SaveChanges()
            End If

        ElseIf tableName = "Groups" Then

            ffGroup = dbContext.GetObjectByKey(New ObjectKey(ffGroup.GetType().Name, nID))
            Dim nPosition As Integer = ffGroup.GroupOrder
            If nPosition > 1 Then
                ffGroup.GroupOrder = ffGroup.GroupOrder - 1
                dbContext.SaveChanges()
            End If

        End If



    End Sub

    Protected Sub rcbFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbFilter.SelectedIndexChanged

        Dim sRecType As String = String.Empty
        If rcbFilter.SelectedValue = "0" Then
            sRecType = FF_GLOBALS.RECORD_TYPE_TARIFF
            rgFilter.MasterTableView.Columns.FindByUniqueName("JobID").Visible = False
            rgFilter.MasterTableView.Columns.FindByUniqueName("Copy").Visible = True
            rgFilter.MasterTableView.Columns.FindByDataField("TariffName").HeaderText = "Tariff"
        ElseIf rcbFilter.SelectedValue = "1" Then
            sRecType = FF_GLOBALS.RECORD_TYPE_QUOTATION
            rgFilter.MasterTableView.Columns.FindByDataField("JobID").Visible = True
            rgFilter.MasterTableView.Columns.FindByUniqueName("Copy").Visible = True
            rgFilter.MasterTableView.Columns.FindByDataField("TariffName").HeaderText = "Quotation"
        End If

        Dim IList = From Tariff In dbContext.FF_Tariffs
                            Where Tariff.RecordType = sRecType And Tariff.IsDeleted = False And Tariff.IsCreated = True
                            Order By Tariff.CreatedOn
                            Select Tariff

        rgFilter.DataSource = IList
        rgFilter.DataBind()

    End Sub

    Protected Sub btnAddQoutation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveQuotation.Click

        Dim IQuotations = (From Quotation In dbContext.FF_Tariffs
                          Where Quotation.JobID = pnJobID And Quotation.IsDeleted = False
                          Select Quotation).ToList

        For Each Quotation As FFDataLayer.FF_Tariff In IQuotations


            ffTariff = dbContext.GetObjectByKey(New ObjectKey(ffTariff.GetType().Name, Quotation.ID))
            Quotation.IsCreated = True
            Quotation.CreatedOn = DateTime.Now
            Quotation.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            dbContext.SaveChanges()


        Next

        'WebMsgBox.Show("Tariffs Saved.")

        'For Each Quotation As dbc In IQuotations

        '    Quotation.IsCreated = True

        'Next

        'Dim sQueryParams(0) As String
        'sQueryParams(0) = "job=" & pnJobID
        'Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)


    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Dim IQuotations = (From Quotation In dbContext.FF_Tariffs
                         Where Quotation.JobID = pnJobID And Quotation.IsDeleted = False And Quotation.IsCreated = False
                         Select Quotation).ToList

        For Each Quotation As FFDataLayer.FF_Tariff In IQuotations


            ffTariff = dbContext.GetObjectByKey(New ObjectKey(ffTariff.GetType().Name, Quotation.ID))
            Quotation.IsDeleted = True
            dbContext.SaveChanges()


        Next

        Dim sQueryParams(0) As String
        sQueryParams(0) = "job=" & pnJobID
        Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)

    End Sub

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)

        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))

    End Sub

    Public Function IsValidDate(ByVal dtDate As DateTime) As String

        Dim dt As Date = dtDate.Date
        If dtDate = FF_GLOBALS.BASE_DATE_MONTH_FORMAT Then
            IsValidDate = String.Empty
        Else

            If dt = Date.Today Then
                IsValidDate = "Today"
            ElseIf dt > DateTime.Now.AddDays(7).Date Or dt < DateTime.Now.AddDays(-7).Date Then
                IsValidDate = dt.ToString("d-MMM-yyyy")
            ElseIf dt = DateTime.Now.AddDays(-1).Date Then
                IsValidDate = "Yesterday"
            ElseIf dt = DateTime.Now.AddDays(1).Date Then
                IsValidDate = "Tomorrow"
            Else
                IsValidDate = sWithinCurrentWeek(dtDate)
            End If


        End If

        Return IsValidDate

    End Function

    Private Function sWithinCurrentWeek(ByVal dtDate As DateTime) As String
        sWithinCurrentWeek = String.Empty
        Dim dtTargetDate = dtDate.Date
        Dim nTargetDayOfWeek As Int32 = dtTargetDate.DayOfWeek
        Dim dtToday As DateTime = Date.Today.Date
        Dim nTodayDayOfWeek As Int32 = dtToday.DayOfWeek

        If dtTargetDate > dtToday Then
            If nTargetDayOfWeek <= nTodayDayOfWeek Then
                sWithinCurrentWeek = "Next " & dtTargetDate.ToString("dddd")
            Else
                sWithinCurrentWeek = dtTargetDate.ToString("dddd")
            End If
        Else
            sWithinCurrentWeek = "Last " & dtTargetDate.ToString("dddd")
        End If
    End Function

End Class








'Dim items = (From quotation In dbContext.FF_Tariffs
'            Where quotation.FF_Item.GroupID = ItemID _
'            Select quotation.FF_Item.ID, quotation.FF_Item.ItemName, quotation.FF_Item.Price, quotation.FF_Item.Rate, quotation.Quantity, quotation.TotalCost).ToList()



'Dim sb As New StringBuilder
'sb.Append("select ff_group.id 'groupid',groupname, ff_group.tariffid from ff_group inner join ff_tariff on ff_group.tariffid = ff_tariff.id ")
'sb.Append("where ff_group.tariffid  = " & nTariffId & "and ff_group.IsDeleted = 0 order by grouporder asc")

'Dim sql As String = sb.ToString
'Dim dt As DataTable = DNNDB.Query(sql)

'If dt IsNot Nothing Then



'End If

'Dim IGroups As IQueryable(Of FFDataLayer.FF_Group) = (From Groups In dbContext.FF_Groups
'              Where Groups.TariffID = nTariffID
'              Select Groups.ID, Groups.GroupName, Groups.GroupOrder, Groups.IsDeleted, Groups.TariffID, Groups.CreatedOn, Groups.CreatedBy).ToList

'Dim IGroupList As IList(Of FFDataLayer.FF_Group) = IGroups


'For Each Group As FFDataLayer.FF_Group In IGroups

'    ffGroup.GroupOrder = Group.GroupOrder
'    ffGroup.IsDeleted = False
'    ffGroup.GroupName = Group.GroupName
'    ffGroup.CreatedOn = DateTime.Now
'    ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
'    ffGroup.TariffID = ffTariff.ID
'    dbContext.Add(ffGroup)
'    dbContext.SaveChanges()

'    Dim IItems As IQueryable(Of FFDataLayer.FF_Item) = From Item In dbContext.FF_Items
'                 Where Item.GroupID = ffGroup.ID
'                 Select Item.ItemName, Item.CostPrice, Item.SellingPrice, Item.Units, Item.Quantity, Item.ItemOrder, Item.IsDeleted, Item.CreatedOn, Item.CreatedBy, Item.GroupID, Item.ID



'    For Each Item As FFDataLayer.FF_Item In IItems.ToList

'        ffItem.ItemName = Item.ItemName
'        ffItem.ItemOrder = Item.ItemOrder
'        ffItem.Quantity = Item.Quantity
'        ffItem.CostPrice = Item.CostPrice
'        ffItem.SellingPrice = Item.SellingPrice
'        ffItem.CreatedOn = DateTime.Now
'        ffItem.GroupID = ffGroup.ID
'        ffItem.IsDeleted = Item.IsDeleted
'        ffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID

'    Next

'Next


'rgQuotation.DataSource = query.c


'Protected Sub rgFilter_ColumnCreating(ByVal sender As Object, ByVal e As GridColumnCreatingEventArgs)

'    If (e.ColumnType = GetType(MyCustomFilteringColumn).Name) Then
'        e.Column = New MyCustomFilteringColumn
'    End If

'End Sub



'Protected Sub CreateColumns()

'    'Dim query = From Tariff In dbContext.FF_Tariffs
'    '              Select Tariff.JobID, Tariff.TariffName, Tariff.LockedDateTime


'    'Dim dt As DataTable = FF_GLOBALS.ConvertToDataTable(query)

'    Dim sql As String = "Select JobID, TariffName, LockedDateTime from FF_Tariff"
'    Dim dt As DataTable = DNNDB.Query(sql)

'    'rgFilter.MasterTableView.Columns.Clear()

'    For Each dataColumn As DataColumn In dt.Columns

'        Dim gridColumn As New MyCustomFilteringColumn()
'        Me.rgFilter.MasterTableView.Columns.Add(gridColumn)

'        If dataColumn.ColumnName = "LockedDateTime" Then

'            gridColumn.DataFormatString = "{0:dd-MMM-yyyy}"
'            gridColumn.DataField = dataColumn.ColumnName
'            gridColumn.UniqueName = dataColumn.ColumnName
'            gridColumn.HeaderText = "Locked Date Time"

'        ElseIf dataColumn.ColumnName = "JobID" Then

'            gridColumn.DataField = dataColumn.ColumnName
'            gridColumn.UniqueName = dataColumn.ColumnName
'            gridColumn.HeaderText = "Job No"
'            'gridColumn.Visible = False

'        ElseIf dataColumn.ColumnName = "TariffName" Then

'            gridColumn.DataField = dataColumn.ColumnName
'            gridColumn.UniqueName = dataColumn.ColumnName
'            gridColumn.HeaderText = "Tariff Name"

'        End If

'    Next


'    'gridColumn.DataField = dataColumn.ColumnName
'    'gridColumn.HeaderText = dataColumn.ColumnName


'End Sub

'Protected Sub clrFilters_Click(ByVal sender As Object, ByVal e As EventArgs) Handles clrFilters.Click

'    For Each column As GridColumn In rgFilter.MasterTableView.Columns

'        column.CurrentFilterFunction = GridKnownFunction.NoFilter
'        column.CurrentFilterValue = String.Empty

'    Next

'    rgFilter.MasterTableView.FilterExpression = String.Empty
'    rgFilter.MasterTableView.Rebind()

'End Sub

'Dim sRecordType As String

'If rcbFilter.SelectedValue = "0" Then
'    sRecordType = FF_GLOBALS.RECORD_TYPE_TARIFF
'    'rgFilter.MasterTableView.Columns.FindByDataField("JobID").Visible = False
'ElseIf rcbFilter.SelectedValue = "1" Then
'    sRecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION
'    'rgFilter.MasterTableView.Columns.FindByDataField("JobID").Visible = True
'Else
'    sRecordType = String.Empty
'End If


''Dim sql As String = "Select JobID, TariffName, LockedDateTime from FF_Tariff"
'Dim sql As String = "select Tariff.ID, Tariff.TariffName, Tariff.TariffOrder, Tariff.RecordType, Tariff.IsDeleted, Tariff.IsCreated, Tariff.JobID, Tariff.CustomerID, Tariff.LockedDateTime, Tariff.CreatedOn, Tariff.CreatedBy from ff_tariff Tariff where Tariff.IsDeleted = 'False' and Tariff.IsCreated = 'True' and Tariff.RecordType = '" & sRecordType & "'"

'Dim dt As DataTable = DNNDB.Query(sql)

'rgFilter.DataSource = dt

'If dt.Rows.Count <> 0 Then

'    If sRecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION Then

'        rgFilter.MasterTableView.Columns.FindByUniqueName("JobID").Visible = True

'    End If

'    rgFilter.MasterTableView.Columns.FindByUniqueName("Copy").Visible = True

'Else

'    rgFilter.MasterTableView.Columns.FindByUniqueName("JobID").Visible = False
'    rgFilter.MasterTableView.Columns.FindByUniqueName("Copy").Visible = False

'End If

'where FF_Tariff.RecordType = '" & FF_GLOBALS.RECORD_TYPE_QUOTATION & "'"
'Dim IQuotations = (From Tariff In dbContext.FF_Tariffs
'            Where Tariff.IsDeleted = False And Tariff.IsCreated = True And Tariff.RecordType = sRecordType
'            Select Tariff).ToList
'CreateColumns()
'Dim sql As String = "select Tariff.ID, Tariff.TariffName, Tariff.TariffOrder, Tariff.RecordType, Tariff.IsDeleted, Tariff.IsCreated, Tariff.JobID, Tariff.CustomerID, Tariff.LockedDateTime, Tariff.CreatedOn, Tariff.CreatedBy from ff_tariff Tariff where Tariff.IsDeleted = 'False' and Tariff.IsCreated = 'True' and Tariff.RecordType = '" & FF_GLOBALS.RECORD_TYPE_QUOTATION & "'" & sqlJobID


'MyCustomFilteringColumn.pnTempJobID = 0


'rgFilter.DataSource = IQuotations
