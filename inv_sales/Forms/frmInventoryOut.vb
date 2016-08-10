Public Class frmInventoryIn

    Private _DOCNUM As Integer
    Friend DocumentType As DocType

    Enum DocType As Integer
        GoodsIssue = 0
        GoodsReceipt = 1
    End Enum


    Friend Sub Load_GoodsReceipt()
        Me.Text = "Inventory In - Goods Receipt"
        DocumentType = DocType.GoodsReceipt
    End Sub

    Private Sub frmInventoryIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearFields()
        Generate_DocNum()
    End Sub

    Private Sub Generate_DocNum(Optional ByVal DocNum As Integer = 0)
        If DocNum = 0 Then

            Dim OptionKey As String = ""
            If DocumentType = DocType.GoodsReceipt Then OptionKey = "STINUM"
            If DocumentType = DocType.GoodsIssue Then OptionKey = "STONUM"

            _DOCNUM = GetOption(OptionKey)
        Else
            _DOCNUM = DocNum
        End If

        txtCode.Text = String.Format("{0:000000}", _DOCNUM)
        dtpDocDate.Value = CurrentDate
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

        _unitPrice = InputBox("Price", "Custom Unit Price", SelectedItem.UnitPrice)

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

        frmPLU.From_Inventories()
        frmPLU.Load_PLU()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class