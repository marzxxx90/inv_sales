Public Class frmPLU

    Private qtyItm As Double = 1
    Private queued_IMD As New CollectionItemData

    Friend Sub From_Sales()
        txtCode.Select()
    End Sub

    Private Sub frmPLU_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        For Each itm As ItemData In queued_IMD
            Console.WriteLine(itm.ItemCode)
        Next
    End Sub

    Private Sub frmPLU_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearField()
    End Sub

    Private Sub ClearFields()
        txtCode.Text = ""
        lvItem.Items.Clear()
    End Sub

    Friend Sub Load_PLU()
        Dim mySql As String = "SELECT * FROM ITEMMASTER WHERE onHold = 0 ORDER BY ITEMCODE ASC"

        If Not txtCode.Text = "" Then Exit Sub
        dbReaderOpen()
        Dim dsR = LoadSQL_byDataReader(mySql)
        While dsR.Read

            Dim itmReader As New ItemData
            itmReader.LoadReader_Item(dsR)
            queued_IMD.Add(itmReader)
            AddItem(itmReader)

        End While
        dbReaderClose()
    End Sub

    Friend Sub SearchSelect(ByVal unsecured_String As String)
        Dim qty As String = ""

        If Not unsecured_String.Contains("*") Then
            txtCode.Text = unsecured_String
        Else
            qty = unsecured_String.Split("*")(0)
            qtyItm = If(IsNumeric(qty), qty, 0)
            txtCode.Text = unsecured_String.Split("*")(1)
        End If

        btnSearch.PerformClick()
    End Sub

#Region "GUI"
    Private Sub AddItem(ByVal itm As ItemData)
        Dim lv As ListViewItem = lvItem.Items.Add(itm.ItemCode)
        lv.SubItems.Add(itm.Description)
        lv.SubItems.Add(itm.Category)
        lv.SubItems.Add(itm.onHand)
    End Sub

    Private Sub ClearField()
        txtCode.Text = ""
        lvItem.Items.Clear()
    End Sub
#End Region

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim unsec_src As String = txtCode.Text
        Dim mySql As String = "SELECT * FROM ITEMMASTER "
        mySql &= String.Format("WHERE LOWER(ITEMCODE) LIKE '%{0}%' OR LOWER(DESCRIPTION) LIKE '%{0}%'", DreadKnight(unsec_src).ToLower)

        Dim ds As DataSet = LoadSQL(mySql)
        If ds.Tables(0).Rows.Count = 0 Then AlertBox("ITEM NOT FOUND", MsgBoxStyle.Information) : Exit Sub

        ' Clear in Collections
        lvItem.Items.Clear()
        queued_IMD.Clear()

        dbReaderOpen()

        Dim dsR = LoadSQL_byDataReader(mySql)
        While dsR.Read
            Dim lv As ListViewItem = lvItem.Items.Add(dsR("ITEMCODE"))
            lv.SubItems.Add(dsR("DESCRIPTION"))
            lv.SubItems.Add(dsR("CATEGORIES"))
            lv.SubItems.Add(dsR("ONHAND"))
        End While

        dbReaderClose()
    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        Console.WriteLine(lvItem.SelectedItems(0).Index)
        Dim idx As Integer = lvItem.SelectedItems(0).Index

        Dim selected_Itm As New ItemData
        selected_Itm.Load_Item(lvItem.SelectedItems(0).Text)

        frmSales.AddItem(selected_Itm, qtyItm)
        frmSales.ClearSearch()
        Me.Close()
    End Sub

    Private Sub lvItem_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvItem.DoubleClick
        btnSelect.PerformClick()
    End Sub

    Private Sub lvItem_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles lvItem.KeyPress
        If Asc(e.KeyChar) = 13 Then
            btnSelect.PerformClick()
        End If
    End Sub

    Private Sub txtCode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCode.KeyPress
        If Asc(e.KeyChar) = 13 Then btnSearch.PerformClick()
    End Sub
End Class