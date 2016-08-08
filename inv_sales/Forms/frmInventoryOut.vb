Public Class frmInventoryIn

    Enum FormChange
        GoodsReceipt = 1 'Inventory IN
        GoodsIssue = 0 'Inventory OUT
    End Enum


    Private Sub frmInventoryIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearFields()
    End Sub

    Private Sub ClearFields()
        txtCode.Text = ""
        txtBPartner.Text = ""
        dtpDocDate.Value = CurrentDate

        lvInventory.Items.Clear()
        txtSearch.Text = ""
        txtTotal.Text = ""

        txtRemarks.Text = ""
    End Sub

    Friend Sub AddItem(ByVal SelectedItem As ItemData, Optional ByVal Qty As Double = 1)
        Dim lv As ListViewItem = lvInventory.Items.Add(SelectedItem.ItemCode)
        lv.SubItems.Add(SelectedItem.Description)
        lv.SubItems.Add(SelectedItem.Qty)
        lv.SubItems.Add(SelectedItem.UnitOfMeasure)
        'Unit Price
        'Total
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

    End Sub
End Class