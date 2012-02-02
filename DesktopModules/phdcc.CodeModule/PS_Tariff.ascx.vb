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
Imports Telerik.OpenAccess

Partial Class PS_Tariff
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    'Private ffTariffItem As New FFDataLayer.FF_TariffItem
    Private ffItem As New FFDataLayer.FF_Item
    Private ffGroup As New FFDataLayer.FF_Group
    Private ffTariff As New FFDataLayer.FF_Tariff
    Private sGridTableName As String
    Private sQuotationName As String = "Quotation"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then



        End If
    End Sub
    Protected Sub rgQuotation_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgQuotation.NeedDataSource

        Dim query = From Tariff In dbContext.FF_Tariffs
                    Where Tariff.IsDeleted = False And Tariff.RecordType = FF_GLOBALS.RECORD_TYPE_TARIFF
                    Select Tariff
        'pdtTemQuotation = query.ToList
        rgQuotation.DataSource = query.ToList


    End Sub

    Protected Sub rgQuotation_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgQuotation.ItemDataBound

        If TypeOf e.Item Is GridDataItem And e.Item.OwnerTableView.Name = "Items" Then

            'e.Item.r()
            'Dim itemCost As Decimal = e.Item.OwnerTableView.Columns.FindByUniqueName("").t 

        ElseIf TypeOf e.Item Is GridFooterItem And e.Item.OwnerTableView.Name = "Groups" Then

            'Dim item As GridFooterItem = e.Item
            'item("GroupName").BackColor = Color.Purple
            'item("EditCommandColumn").BackColor = Color.Purple
            'item("ShowTotal").BackColor = Color.Purple
            'CalculateTotalCost()
            'CalculateTotalSelling()

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

                        If rntbRate IsNot Nothing And rntbQuantity IsNot Nothing And rntbCostPrice IsNot Nothing Then

                            If rntbRate.Text.Trim() <> String.Empty Then
                                nRate = Convert.ToInt64(rntbRate.Text.Trim())
                            Else
                                nRate = 1
                            End If

                            If rntbCostPrice.Text.Trim() <> String.Empty Then
                                dCostPrice = Convert.ToDecimal(rntbCostPrice.Text.Trim())
                            Else
                                dCostPrice = 0
                            End If

                            If rntbQuantity.Text.Trim() <> String.Empty Then
                                nQuantity = Convert.ToInt64(rntbQuantity.Text.Trim())
                            Else
                                nQuantity = 0
                            End If

                            dTotalCost = (dCostPrice * nQuantity) / nRate + dTotalCost

                            Dim nestedView As GridTableView = gdiQuotation.ChildItem.NestedTableViews(0)

                            For Each childfooter As GridFooterItem In nestedView.GetItems(GridItemType.Footer)

                                Dim lblTotalCost As Label = DirectCast(childfooter.FindControl("lblTotalCost"), Label)

                                lblTotalCost.Text = "Total C.P £" + dTotalCost.ToString()

                            Next


                        End If


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
                ffTariff.RecordType = FF_GLOBALS.RECORD_TYPE_TARIFF
                ffTariff.IsDeleted = False
                ffTariff.LockedDateTime = FF_GLOBALS.BASE_DATE
                ffTariff.IsCreated = True
                'ffTariff.JobID = pnJobID
                ffTariff.JobID = -1
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

            If "Tariff".Equals(e.Item.OwnerTableView.Name) Then

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

End Class
