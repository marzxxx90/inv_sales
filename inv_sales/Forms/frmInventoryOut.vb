Public Class frmInventoryIn

    Private _DOCNUM As Integer

    Enum FormChange
        GoodsReceipt = 1 'Inventory IN
        GoodsIssue = 0 'Inventory OUT
    End Enum

    Private Sub frmInventoryIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearFields()
        Generate_DocNum()
    End Sub

    Private Sub Generate_DocNum(Optional ByVal DocNum As Integer = 0)
        If DocNum = 0 Then
            _DOCNUM = GetOption("DOCNUM")
        Else
            _DOCNUM = DocNum
        End If

        txtCode.Text = String.Format("{0:000000}", _DOCNUM)
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

    Friend Sub AddItem(ByVal SelectedItem As ItemData, Optional ByVal Qty As Double = 1, Optional ByVal UnitPrice As Double = 0)
        Dim _unitPrice As Double = 0

        Dim lv As ListViewItem = lvInventory.Items.Add(SelectedItem.ItemCode)
        lv.SubItems.Add(SelectedItem.Description)
        lv.SubItems.Add(Qty)
        lv.SubItems.Add(SelectedItem.UnitOfMeasure)
        If UnitPrice = 0 Then
            lv.SubItems.Add(SelectedItem.UnitPrice)

            _unitPrice = SelectedItem.UnitPrice
        Else
            lv.SubItems.Add(UnitPrice)

            _unitPrice = UnitPrice
        End If

        'Total
        Dim total As Double = Qty * _unitPrice
        lv.SubItems.Add(total.ToString("#,#00.00"))
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        frmPLU.Show()

        If txtSearch.Text.Length > 0 Then frmPLU.SearchSelect(txtSearch.Text) : Exit Sub

        frmPLU.Load_PLU()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class