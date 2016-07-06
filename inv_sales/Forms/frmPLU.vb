Public Class frmPLU

    Friend Sub From_Sales()
        txtCode.Select()
    End Sub

    Private Sub frmPLU_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Load_PLU()
    End Sub

    Private Sub Load_PLU()
        Dim mySql As String = "SELECT * FROM ITEMMASTER WHERE onHold = 0 ORDER BY ITEMCODE ASC"

        dbReaderOpen()
        Dim dsR = LoadSQL_byDataReader(mySql)
        While dsR.Read

            Dim itmReader As New ItemData
            itmReader.LoadReader_Item(dsR)
            AddItem(itmReader)

        End While
        dbReaderClose()
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
        mySql &= String.Format("WHERE ITEMCODE LIKE '%{0}%' OR DESCRIPTION LIKE '%{0}%'", DreadKnight(unsec_src))

        Dim ds As DataSet = LoadSQL(mySql)
        If ds.Tables(0).Rows.Count = 0 Then AlertBox("ITEM NOT FOUND", MsgBoxStyle.Information) : Exit Sub

        lvItem.Items.Clear()
        For Each dr As DataRow In ds.Tables(0).Rows

            Dim foundItem As New ItemData
            foundItem.Load_Item(dr("ITEMCODE"))

            AddItem(foundItem)
            Application.DoEvents()
        Next
    End Sub
End Class