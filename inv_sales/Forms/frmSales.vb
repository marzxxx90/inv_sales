﻿Public Class frmSales

    Enum TransType As Integer
        Cash = 0
        Check = 1
    End Enum

    Friend TransactionMode As TransType
    Friend ht_BroughtItems As New Hashtable

    Private ORNUM As Double = GetOption("ORNUM")
    Private VAT As Double = 0
    Private DOC_TYPE As Integer = 0 '0 - SALES
    Private DOC_NOVAT As Double = 0
    Private DOC_VATTOTAL As Double = 0
    Private DOC_TOTAL As Double = 0

    Private canTransact As Boolean = True

    Private Sub frmSales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Seeder.ItemMasterData()

        ClearField()
        txtSearch.Select()

        'VAT SETUP
        If hasVAT() Then
            lblNoVat.Visible = True
        Else
            lblNoVat.Visible = False
        End If

        CheckOR()
    End Sub

    Private Sub CheckOR()
        Dim mySql As String = "SELECT * FROM DOC WHERE CODE = "
        mySql &= String.Format("'OR#{0:000000}'", ORNUM)

        Dim ds As DataSet = LoadSQL(mySql)
        If ds.Tables(0).Rows.Count >= 1 Then
            canTransact = False
            MsgBox("OR NUMBER ALREADY EXISTED" + vbCrLf + "PLEASE BE ADVICED", MsgBoxStyle.Critical)
        End If
    End Sub

#Region "Function"
    ''' <summary>
    ''' Get the current VAT
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Private Function hasVAT() As Boolean

        'VAT = 12 / 100
        If VAT = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Friend Sub AddItem(ByVal itm As ItemData, ByVal qty As Double)
        Dim ItemAmount As Double
        Dim hasSelected As Boolean = False

        For Each AddedItems As ListViewItem In lvSale.Items
            If AddedItems.Text = itm.ItemCode Then
                hasSelected = True
                Exit For
            End If
        Next

        If hasSelected Then
            With lvSale.FindItemWithText(itm.ItemCode)
                .SubItems(2).Text += qty
                ItemAmount = (itm.SalePrice * qty)
                .SubItems(4).Text += ItemAmount
            End With
        Else
            'If NEW
            Dim lv As ListViewItem = lvSale.Items.Add(itm.ItemCode)
            lv.SubItems.Add(itm.Description)
            lv.SubItems.Add(qty)
            lv.SubItems.Add(itm.SalePrice.ToString("#,#00.00"))
            ItemAmount = (itm.SalePrice * qty)
            lv.SubItems.Add(ItemAmount.ToString("#,#00.00"))
        End If

        If ht_BroughtItems.ContainsKey(itm.ItemCode) Then
            ht_BroughtItems.Item(itm.ItemCode) += qty
        Else
            ht_BroughtItems.Add(itm.ItemCode, qty)
        End If

        DOC_TOTAL += ItemAmount
        Display_Total(DOC_TOTAL)
    End Sub

    Friend Sub ClearSearch()
        txtSearch.Text = ""
        txtSearch.Focus()
    End Sub

    Private Sub ForcePosting()
        btnPost.PerformClick()
    End Sub

    Private Sub Form_HotKeys(ByVal e As System.Windows.Forms.KeyEventArgs)
        Select Case e.KeyCode
            Case 112 'F1
                frmIMD.Show()
            Case 113 'F2
                frmPLU.Show()
            Case 114 'F3
                tsbCustomer.PerformClick()
            Case 116 'F5
                tsbCash.PerformClick()
            Case 117 'F6
                tsbCheck.PerformClick()
            Case 120 'F9
                ForcePosting()
        End Select
    End Sub
#End Region

#Region "GUI"
    Private Sub ClearField()
        lvSale.Items.Clear()
        lblCustomer.Text = "One-Time Customer"
        txtSearch.Text = ""
        lblNoVat.Text = Display_NoVat(0)
        Display_Total(0)
    End Sub

    Private Sub Load_asCash()
        lblMode.Text = "CASH"
        TransactionMode = TransType.Cash
    End Sub

    Private Sub Load_asCheck()
        lblMode.Text = "CHECK"
        TransactionMode = TransType.Check
    End Sub

    Private Function Display_Total(ByVal tot As Double) As Double
        Dim final As Double = tot.ToString("##000.00")

        Dim TOTALVAT As Double = final * VAT
        Display_NoVat(final)
        DOC_VATTOTAL = TOTALVAT

        final += TOTALVAT
        lblTotal.Text = String.Format("Php {0:#,##0.00}", final)
        DOC_TOTAL = final

        Return final
    End Function

    Private Function Display_NoVat(ByVal tot As Double) As Double
        lblNoVat.Text = String.Format("Php {0:#,##0.00}", tot)
        DOC_NOVAT = tot

        Return tot
    End Function
#End Region

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        frmPLU.Show()
        frmPLU.From_Sales()

        If txtSearch.Text.Length > 0 Then frmPLU.SearchSelect(txtSearch.Text) : Exit Sub

        frmPLU.Load_PLU()
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        Form_HotKeys(e)
    End Sub

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        If Asc(e.KeyChar) = 13 Then btnSearch.PerformClick()
    End Sub

    Private Sub tsbIMD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbIMD.Click
        frmIMD.Show()
    End Sub

    Private Sub tsbPLU_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPLU.Click
        frmPLU.Show()
        frmPLU.Load_PLU()
    End Sub

    Private Sub tsbCustomer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCustomer.Click
        Dim defaultName As String = "One-Time Customer"
        Dim promptName As String = InputBox("Customer's Name", "Customer", defaultName)

        If promptName = "" Then lblCustomer.Text = defaultName : Exit Sub
        lblCustomer.Text = promptName
    End Sub

    Private Sub lvSale_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvSale.KeyDown
        If e.KeyCode = Keys.Delete Then

            Dim idx As Integer = lvSale.FocusedItem.Index

            If Not IsNumeric(lvSale.Items(idx).SubItems(2).Text) Then
                Log_Report(String.Format("[SALES DELETE] {0} have an NON-NUMERIC QTY", lvSale.Items(idx).Text))
                MsgBox(String.Format("{0} has NON-NUMERIC Quantity", lvSale.Items(idx).Text), MsgBoxStyle.Critical, "INVALID ITEMCODE")
                Exit Sub
            End If

            Console.WriteLine("Removing " & lvSale.Items(idx).Text)

            Dim itm As New ItemData
            itm.Load_Item(lvSale.Items(idx).Text)

            DOC_TOTAL -= itm.SalePrice * CDbl(lvSale.Items(idx).SubItems(2).Text)
            ht_BroughtItems.Remove(itm.ItemCode)
            lvSale.Items(idx).Remove()

            Display_Total(DOC_TOTAL)
        End If
    End Sub

    Private Sub tsbCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCheck.Click
        Load_asCheck()
    End Sub

    Private Sub tsbCash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCash.Click
        Load_asCash()
    End Sub

    Private Sub btnPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPost.Click
        If Not MsgBox("Do you want to POST?", MsgBoxStyle.YesNo + MsgBoxStyle.Information + vbDefaultButton2, "POSTING...") = vbYes Then
            Exit Sub
        End If

        CheckOR()
        If Not canTransact Then Exit Sub

        If lvSale.Items.Count = 0 Then Exit Sub

        Dim mySql As String, fillData As String

        'Creating Document
        mySql = "SELECT * FROM DOC ROWS 1"
        fillData = "DOC"
        Dim unsec_Customer As String = lblCustomer.Text

        Dim ds As DataSet = LoadSQL(mySql, fillData)
        Dim dsNewRow As DataRow
        dsNewRow = ds.Tables(fillData).NewRow

        With dsNewRow
            .Item("DOCTYPE") = DOC_TYPE
            .Item("CODE") = String.Format("OR#{0:000000}", ORNUM)
            .Item("MOP") = GetModesOfPayment(TransactionMode)
            .Item("CUSTOMER") = unsec_Customer
            .Item("DOCDATE") = CurrentDate
            .Item("NOVAT") = DOC_NOVAT
            .Item("VATRATE") = VAT
            .Item("VATTOTAL") = DOC_VATTOTAL
            .Item("DOCTOTAL") = DOC_TOTAL
            .Item("USERID") = GetUserID()
        End With
        ds.Tables(fillData).Rows.Add(dsNewRow)

        database.SaveEntry(ds)
        Dim DOCID As Integer = 0

        mySql = "SELECT * FROM DOC ORDER BY DOCID DESC ROWS 1"
        ds = LoadSQL(mySql, fillData)
        DOCID = ds.Tables(fillData).Rows(0).Item("DOCID")

        Console.Write("Loading")
        While DOCID = 0
            Application.DoEvents()
            Console.Write(".")
        End While
        Console.WriteLine()

        'Creating DocumentLines
        mySql = "SELECT * FROM DOCLINES ROWS 1"
        fillData = "DOCLINES"
        ds = LoadSQL(mySql, fillData)

        For Each ht As DictionaryEntry In ht_BroughtItems
            Dim itm As New ItemData
            itm.Load_Item(ht.Key)

            dsNewRow = ds.Tables(fillData).NewRow
            With dsNewRow
                .Item("DOCID") = DOCID
                .Item("ITEMCODE") = itm.ItemCode
                .Item("DESCRIPTION") = itm.Description
                .Item("QTY") = ht.Value
                .Item("UNITPRICE") = itm.UnitPrice
                .Item("SALEPRICE") = itm.SalePrice
                .Item("ROWTOTAL") = itm.SalePrice * ht.Value
                .Item("UOM") = itm.UnitOfMeasure
            End With
            ds.Tables(fillData).Rows.Add(dsNewRow)

            database.SaveEntry(ds)

            itm.onHand -= 1
            itm.Save_ItemData()
        Next

        ItemPosted()
    End Sub

    Private Function GetModesOfPayment(ByVal x As TransType)
        Select Case x
            Case TransType.Cash
                Return "C"
            Case TransType.Check
                Return "Q"
        End Select

        Return "0"
    End Function

    Private Sub ItemPosted()
        ORNUM += 1 'INCREMENT ORNUMBER
        UpdateOption("ORNUM", ORNUM)

        MsgBox("ITEM POSTED", MsgBoxStyle.Information)
        ClearField()
    End Sub

    Private Sub tsbRefund_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRefund.Click
        MsgBox("FUNCTION UNDER CONSTRUCTION", MsgBoxStyle.Information)
    End Sub

    Private Sub tsbSalesReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSalesReturn.Click
        MsgBox("FUNCTION UNDER CONSTRUCTION", MsgBoxStyle.Information)
    End Sub
End Class