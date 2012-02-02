Imports System
Imports System.Collections
'Imports System.Data
'Imports DotNetNuke.Common.Utilities
'Imports DotNetNuke.Entities.Users
'Imports DotNetNuke.Security.Membership.Data
'Imports DotNetNuke.Security.Roles
'Imports DotNetNuke.Services.Exceptions
Imports FFDataLayer
Imports Telerik.Web.UI
Imports Telerik.OpenAccess

Partial Class PS_TariffItem
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    'Private ffTariffItem As New FFDataLayer.FF_TariffItem
    Private ffItem As New FFDataLayer.FF_Item
    Private ffGroup As New FFDataLayer.FF_Group
    Private ffTariff As New FFDataLayer.FF_Tariff
    Private sGridTableName As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then


        End If

    End Sub

    Protected Sub rgTariffItem_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgTariffItem.NeedDataSource

        Dim query = dbContext.GetAll(Of FFDataLayer.FF_Tariff)()
        rgTariffItem.DataSource = query.ToList

    End Sub


    Protected Sub rgTariffItem_DetailTableDataBind(ByVal source As Object, ByVal e As GridDetailTableDataBindEventArgs) Handles rgTariffItem.DetailTableDataBind

        Dim dataItem As GridDataItem = e.DetailTableView.ParentItem

        Select Case e.DetailTableView.Name

            Case "Groups"

                Dim nTariffId As String = e.DetailTableView.ParentItem.GetDataKeyValue("ID").ToString()

                Dim sb As New StringBuilder
                sb.Append("select ff_group.id 'groupid',groupname, ff_group.tariffid from ff_group inner join ff_tariff on ff_group.tariffid = ff_tariff.id ")
                sb.Append("where ff_group.tariffid  = " & nTariffId & "and ff_group.IsDeleted = 0 order by grouporder asc")

                Dim sql As String = sb.ToString
                Dim dt As DataTable = DNNDB.Query(sql)

                If dt IsNot Nothing Then

                    e.DetailTableView.DataSource = dt

                End If


            Case "Items"

                Dim nGroupId As String = dataItem.GetDataKeyValue("GroupID").ToString()

                Dim IItems = From Items In dbContext.FF_Items
                            Where Items.GroupID = nGroupId And Items.IsDeleted = False
                            Order By Items.ItemOrder
                            Select Items.ID, Items.GroupID, Items.CostPrice, Items.SellingPrice, Items.ItemName, Items.Units, Items.ItemOrder

                e.DetailTableView.DataSource = IItems.ToList


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


        End If

    End Sub

    Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

            If "Tariff".Equals(e.Item.OwnerTableView.Name) Then

                Dim txtTariffName As TextBox = e.Item.FindControl("txtTariffName")
                ffTariff.TariffName = txtTariffName.Text.Trim()
                ffTariff.RecordType = FF_GLOBALS.RECORD_TYPE_TARIFF
                ffTariff.IsDeleted = False
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
                Dim txtRate As TextBox = e.Item.FindControl("txtRate")

                Dim nItemOrder As Int64 = e.Item.OwnerTableView.Items.Count + 1
                ffItem.ItemOrder = nItemOrder
                ffItem.IsDeleted = False
                ffItem.ItemName = txtItemName.Text.Trim()
                ffItem.CostPrice = rntbCostPrice.Text.Trim()
                ffItem.SellingPrice = rntbSellingPrice.Text.Trim()
                ffItem.Units = txtRate.Text.Trim
                ffItem.CreatedOn = DateTime.Now
                ffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                ffItem.GroupID = e.Item.OwnerTableView.ParentItem.GetDataKeyValue("GroupID")
                dbContext.Add(ffItem)
                dbContext.SaveChanges()

            End If


        End If

    End Sub

    Protected Sub Update(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

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
                Dim data As GridEditFormItem = e.Item
                nID = Convert.ToInt64(data.ParentItem("ID").Text)

                Dim txtItemName As TextBox = e.Item.FindControl("txtItemName")
                Dim rntbCostPrice As RadNumericTextBox = e.Item.FindControl("rntbCostPrice")
                Dim rntbSellingPrice As RadNumericTextBox = e.Item.FindControl("rntbSellingPrice")
                Dim txtRate As TextBox = e.Item.FindControl("txtRate")

                ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))

                ffItem.Units = txtRate.Text.Trim
                ffItem.ItemName = txtItemName.Text.Trim()
                ffItem.CostPrice = rntbCostPrice.Text.Trim()
                ffItem.SellingPrice = rntbSellingPrice.Text.Trim()
                ffItem.CreatedOn = DateTime.Now
                ffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                ffItem.GroupID = e.Item.OwnerTableView.ParentItem.GetDataKeyValue("GroupID")
                dbContext.SaveChanges()

            End If


        End If



    End Sub

    Protected Sub rcbItem_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

        Dim rcbItem As RadComboBox = o
        Dim nID As Int64 = Convert.ToInt64(rcbItem.SelectedValue)
        ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))

        Dim txtPrice As TextBox = rcbItem.NamingContainer.FindControl("txtPrice")
        txtPrice.Text = ffItem.CostPrice

    End Sub



    Protected Sub rgTariffItem_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgTariffItem.ItemDataBound

        If e.Item.IsInEditMode Then

            'BindTariff(e)
            'BindGroup(e)
            'BindItem(e)

        End If

    End Sub

    Protected Sub rgTariffItem_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgTariffItem.ItemCommand

        If e.CommandName = "PerformInsert" Then
            Insert(e)
        Else
            Update(e)
        End If


    End Sub

    Protected Function ToggleButtons() As String

        Dim sText As String

        If rgTariffItem.MasterTableView.IsItemInserted Or rgTariffItem.MasterTableView.DetailTables(0).IsItemInserted Or rgTariffItem.MasterTableView.DetailTables(1).IsItemInserted Then
            sText = "Insert"
        Else
            sText = "Update"
        End If

        Return sText

    End Function

    Protected Function ToggleCommand() As String

        Dim sText As String

        If rgTariffItem.MasterTableView.IsItemInserted Or rgTariffItem.MasterTableView.DetailTables(0).IsItemInserted Then
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
                TryCast(TryCast(tableView, GridTableView).Items(radGridClickedRowIndex), GridItem).Selected = True
                rgTariffItem.MasterTableView.IsItemInserted = True
                tableView.Rebind()
                Exit Select

            Case "Delete"

                tableView = TryCast(Me.Page.FindControl(UId), GridTableView)

                If tableView.Name = "Tariff" Then
                    Dim nID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("ID"))
                    sGridTableName = "Tariff"
                    Delete(nID, sGridTableName)
                    tableView.Rebind()
                ElseIf tableView.Name = "Groups" Then
                    Dim nID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("GroupID"))
                    sGridTableName = "Groups"
                    Delete(nID, sGridTableName)
                    tableView.Rebind()
                ElseIf tableView.Name = "Items" Then
                    Dim nID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("ID"))
                    sGridTableName = "Items"
                    Delete(nID, sGridTableName)
                    tableView.Rebind()
                End If

                Exit Select

            Case "Move Up"

                tableView = TryCast(Me.Page.FindControl(UId), GridTableView)

                If tableView.Name = "Items" Then

                    Dim nClickedRowID As Integer = Convert.ToInt64(tableView.Items(radGridClickedRowIndex).GetDataKeyValue("ID"))
                    sGridTableName = "Items"

                    Row_MoveUp(nClickedRowID, String.Empty)

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

    Protected Sub rapPSMain_AjaxRequest(ByVal sender As Object, ByVal e As Telerik.Web.UI.AjaxRequestEventArgs)



    End Sub


End Class

'Protected Sub rcbGroup_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)

'    Dim rcbGroup As RadComboBox = o
'    Dim nGroupId As Int64 = Convert.ToInt64(rcbGroup.SelectedValue)

'    Dim rcbItems As RadComboBox = rcbGroup.NamingContainer.FindControl("rcbItem")

'    Dim items = From item In dbContext.FF_Items
'               Where (item.GroupID = nGroupId)

'    Dim IItems As IList = items.ToList

'    For Each item As FFDataLayer.FF_Item In IItems

'        Dim rcbItem As New RadComboBoxItem(item.ItemName, item.ID)
'        rcbItems.Items.Add(rcbItem)

'    Next

'    rcbItems.Items.Insert(0, New RadComboBoxItem("- Select -", -1))

'End Sub
'Protected Sub BindTariff(ByVal e As Telerik.Web.UI.GridItemEventArgs)

'    Dim rcbTariff As RadComboBox = e.Item.FindControl("rcbTariff")

'    Dim ITariffs As IList = dbContext.FF_Tariffs.ToList


'    For Each item As FFDataLayer.FF_Tariff In ITariffs

'        Dim rcbItem As New RadComboBoxItem(item.TariffName, item.ID)
'        rcbTariff.Items.Add(rcbItem)

'    Next

'    rcbTariff.Items.Insert(0, New RadComboBoxItem("- Select -", -1))

'End Sub

'Protected Sub BindItem(ByVal e As Telerik.Web.UI.GridItemEventArgs)

'    Dim rcbItem As RadComboBox = e.Item.FindControl("rcbItem")


'    Dim IItems As IList = dbContext.FF_Items.ToList


'    For Each item As FFDataLayer.FF_Item In IItems

'        Dim rcbNewItem As New RadComboBoxItem(item.ItemName, item.ID)
'        rcbItem.Items.Add(rcbNewItem)

'    Next

'    rcbItem.Items.Insert(0, New RadComboBoxItem("- Select -", -1))

'End Sub

'Protected Sub BindGroup(ByVal e As Telerik.Web.UI.GridItemEventArgs)

'    Dim rcbGroup As RadComboBox = e.Item.FindControl("rcbGroup")


'    Dim IGroups As IList = dbContext.FF_Groups.ToList


'    For Each item As FFDataLayer.FF_Group In IGroups

'        Dim rcbItem As New RadComboBoxItem(item.GroupName, item.ID)
'        rcbGroup.Items.Add(rcbItem)

'    Next

'    rcbGroup.Items.Insert(0, New RadComboBoxItem("- Select -", -1))

'End Sub


'Dim txtTariffPrice As TextBox = e.Item.FindControl("txtPrice")
'Dim rcbTariff As RadComboBox = e.Item.FindControl("rcbTariff")
'Dim rcbItem As RadComboBox = e.Item.FindControl("rcbItem")

'Dim nPrice As Integer = Convert.ToDecimal(txtTariffPrice.Text.Trim())

'Dim data As GridEditFormItem = e.Item
'Dim nID As Integer = Convert.ToInt64(data.ParentItem("ID").Text)

'ffTariffItem = dbContext.GetObjectByKey(New ObjectKey(ffTariff.GetType().Name, nID))

'ffTariffItem.TariffID = Convert.ToInt64(rcbTariff.SelectedValue)
'ffTariffItem.ItemID = Convert.ToInt64(rcbItem.SelectedValue)
'ffTariffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
'ffTariffItem.CreatedOn = DateTime.Now
'dbContext.Add(ffTariffItem)
'dbContext.SaveChanges()

