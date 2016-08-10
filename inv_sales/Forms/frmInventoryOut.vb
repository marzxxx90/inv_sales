Public Class frmInventoryIn

    Private _DOCNUM As Integer
    Private DOC_TOTAL As Double = 0
    Friend DocumentType As DocType

    Enum DocType As Integer
        GoodsIssue = 0
        GoodsReceipt = 1
    End Enum


    Private Sub DisplayGrandTotal(ByVal total As Double)
        txtTotal.Text = total.ToString("Php #,##0.00")
    End Sub

    Friend Sub Load_GoodsReceipt()
        Me.Text = "Inventory In - Goods Receipt"
        DocumentType = DocType.GoodsReceipt
    End Sub

    Private Sub frmInventoryIn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearFields()
        Generate_DocNum()
        DisplayGrandTotal(0)
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
        Dim _unitPrice As Double = 0, ItemAmount As Double = 0
        Dim hasSelected As Boolean = False, total As Double

        For Each AddedItems As ListViewItem In lvInventory.Items
            If AddedItems.Text = SelectedItem.ItemCode Then
                hasSelected = True
                Exit For
            End If
        Next

        If hasSelected Then
            With lvInventory.FindItemWithText(SelectedItem.ItemCode)
                .SubItems(3).Text += Qty
                ItemAmount = (UnitPrice * Qty)
                Dim curTotal As Double = .SubItems(6).Text
                curTotal += ItemAmount
                DOC_TOTAL += ItemAmount

                .SubItems(6).Text = curTotal.ToString("#,##0.00")
            End With
        Else
            Dim lv As ListViewItem = lvInventory.Items.Add(SelectedItem.ItemCode)
            lv.SubItems.Add(SelectedItem.Description)
            lv.SubItems.Add(SelectedItem.Category)
            lv.SubItems.Add(Qty)
            lv.SubItems.Add(SelectedItem.UnitOfMeasure)
            If UnitPrice = 0 Then
                lv.SubItems.Add(SelectedItem.UnitPrice.ToString("#,##0.00"))

                _unitPrice = SelectedItem.UnitPrice
            Else
                lv.SubItems.Add(UnitPrice.ToString("#,##0.00"))

                _unitPrice = UnitPrice
            End If

            'Total
            total = Qty * _unitPrice
            lv.SubItems.Add(total.ToString("#,#00.00"))
        End If


        DOC_TOTAL += total
        DisplayGrandTotal(DOC_TOTAL)
    End Sub

    Friend Sub ClearSearch()
        txtSearch.Text = ""
        txtSearch.Focus()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        frmPLU.Show()
        frmPLU.From_Inventories()

        If txtSearch.Text.Length > 0 Then frmPLU.SearchSelect(txtSearch.Text) : Exit Sub

        frmPLU.Load_PLU()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If isEnter(e) Then btnSearch.PerformClick()
    End Sub

    Private Sub lvInventory_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvInventory.KeyDown
        If e.KeyCode = Keys.Delete Then

            Dim idx As Integer = lvInventory.FocusedItem.Index
            Dim totalRow As Double = lvInventory.FocusedItem.SubItems(6).Text
            lvInventory.Items(idx).Remove()

            DOC_TOTAL -= totalRow
            DisplayGrandTotal(DOC_TOTAL)

        End If
    End Sub

    Private Sub btnPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPost.Click
        If Not isVerified() Then
            txtSearch.Focus()
            Exit Sub
        End If

        Dim fillData As String, mySql As String, DocPrefix As String
        Select Case DocumentType
            Case DocType.GoodsReceipt
                DocPrefix = "STI#"
            Case DocType.GoodsIssue
                DocPrefix = "STO#"
            Case Else
                DocPrefix = "DOC#"
        End Select

        ' Creating Document
        Console.WriteLine("DocNum: " & _DOCNUM)
        fillData = "GOODSRECEIPT"

        mySql = String.Format("SELECT * FROM {0} ROWS 1", fillData)
        Dim ds As DataSet = LoadSQL(mySql, fillData)
        Dim dsNewRow As DataRow
        dsNewRow = ds.Tables(fillData).NewRow

        With dsNewRow
            .Item("DOCNUM") = DocPrefix & _DOCNUM.ToString("000000")
            .Item("DOCDATE") = dtpDocDate.Value
            .Item("PARTNER") = txtBPartner.Text
            .Item("GRANDTOTAL") = DOC_TOTAL
            .Item("REMARKS") = txtRemarks.Text
        End With

        ds.Tables(fillData).Rows.Add(dsNewRow)
        database.SaveEntry(ds)

        ' Create Document Lines
        Dim DOCID As Integer = GetLastID()
        fillData = "RECEIPTLINES"
        mySql = String.Format("SELECT * FROM {0} ROWS 1", fillData)

        ds = LoadSQL(mySql, fillData)

        For Each lv As ListViewItem In lvInventory.Items
            Dim ItemCode As String = lv.Text

            Dim saveItm As New ItemData
            saveItm.Load_Item(ItemCode)

            dsNewRow = ds.Tables(fillData).NewRow
            With dsNewRow
                .Item("DOCID") = DOCID
                .Item("ITEMCODE") = saveItm.ItemCode
                .Item("DESCRIPTION") = saveItm.Description
                .Item("QTY") = CDbl(lv.SubItems(3).Text)
                .Item("UNITPRICE") = CDbl(lv.SubItems(5).Text)
                .Item("UOM") = saveItm.UnitOfMeasure
                .Item("ROWTOTAL") = CDbl(lv.SubItems(6).Text)
            End With

            ds.Tables(fillData).Rows.Add(dsNewRow)
            database.SaveEntry(ds)

            saveItm.onHand += CDbl(lv.SubItems(3).Text)
            saveItm.Save_ItemData()
        Next

        InventoryPosted()
        MsgBox("Inventory Updated", MsgBoxStyle.Information, "Update")
        Me.Close()
    End Sub

    Private Sub InventoryPosted()
        _DOCNUM += 1
        Generate_DocNum(_DOCNUM)

        UpdateOption("STINUM", _DOCNUM)
    End Sub

    Private Function GetLastID() As Integer
        Dim mySql As String, fillData As String = ""

        If DocumentType = DocType.GoodsReceipt Then fillData = "GOODSRECEIPT"
        If DocumentType = DocType.GoodsIssue Then fillData = "GOODSISSUE"

        mySql = "SELECT * FROM " & fillData
        mySql &= " ORDER BY DOCID DESC ROWS 1"

        Dim ds As DataSet = LoadSQL(mySql, fillData)
        If ds.Tables(fillData).Rows.Count = 0 Then Return 0

        Return ds.Tables(fillData).Rows(0).Item("DOCID")
    End Function

    Private Function isVerified() As Boolean
        Dim ver As Boolean = True

        If txtBPartner.Text = "" Then txtBPartner.Text = "One-Time Vendor"
        If lvInventory.Items.Count = 0 Then ver = False

        Return ver
    End Function
End Class