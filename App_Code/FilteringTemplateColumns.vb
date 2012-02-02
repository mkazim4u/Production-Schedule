Imports Microsoft.VisualBasic
Imports Telerik.Web.UI
Imports System.Web
Imports Telerik.OpenAccess

Partial Public Class MyCustomFilteringColumn

    Inherits GridBoundColumn

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffTariff As New FFDataLayer.FF_Tariff

    'RadGrid will cal this method when it initializes the controls inside the filtering item cells
    Protected Overrides Sub SetupFilterControls(ByVal cell As TableCell)
        MyBase.SetupFilterControls(cell)
        cell.Controls.RemoveAt(0)
        Dim combo As RadComboBox = New RadComboBox
        combo.ID = ("RadComboBox1" + Me.DataField)
        combo.ShowToggleImage = False
        combo.Skin = "Office2007"
        combo.EnableLoadOnDemand = True
        combo.AutoPostBack = True
        combo.MarkFirstMatch = True
        combo.Height = Unit.Pixel(100)
        AddHandler combo.ItemsRequested, AddressOf Me.list_ItemsRequested
        AddHandler combo.SelectedIndexChanged, AddressOf Me.list_SelectedIndexChanged
        cell.Controls.AddAt(0, combo)
        cell.Controls.RemoveAt(1)
    End Sub

    'RadGrid will cal this method when the value should be set to the filtering input control(s)
    Protected Overrides Sub SetCurrentFilterValueToControl(ByVal cell As TableCell)
        MyBase.SetCurrentFilterValueToControl(cell)
        Dim combo As RadComboBox = CType(cell.Controls(0), RadComboBox)
        If (Me.CurrentFilterValue <> String.Empty) Then
            combo.Text = Me.CurrentFilterValue
        End If
    End Sub

    'RadGrid will cal this method when the filtering value should be extracted from the filtering input control(s)
    Protected Overrides Function GetCurrentFilterValueFromControl(ByVal cell As TableCell) As String
        Dim combo As RadComboBox = CType(cell.Controls(0), RadComboBox)
        Return combo.Text
    End Function
    Private Sub list_ItemsRequested(ByVal o As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)

        Dim sql As String = "SELECT DISTINCT " & Convert.ToString(Me.DataField) & " FROM FF_Tariff WHERE " & Convert.ToString(Me.DataField) & " LIKE '" & Convert.ToString(e.Text) & "%'"
        'Dim sql As String = "SELECT DISTINCT " & Convert.ToString(Me.DataField) & " FROM FF_Tariff WHERE " & Convert.ToString(Me.DataField) & " = '" & Convert.ToString(e.Text) & "'"
        Dim dt As DataTable = DNNDB.Query(sql)
        CType(o, RadComboBox).DataTextField = Me.DataField
        CType(o, RadComboBox).DataValueField = Me.DataField
        CType(o, RadComboBox).DataSource = dt
        CType(o, RadComboBox).DataBind()

    End Sub
    Private Sub list_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        Dim filterItem As GridFilteringItem = CType(CType(o, RadComboBox).NamingContainer, GridFilteringItem)
        If (Me.UniqueName = "JobID") Then
            'this is filtering for integer column type 
            filterItem.FireCommandEvent("Filter", New Pair("EqualTo", Me.UniqueName))
        End If
        'filtering for string column type
        filterItem.FireCommandEvent("Filter", New Pair("Contains", Me.UniqueName))
    End Sub


End Class


