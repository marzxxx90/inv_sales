Public Class frmSales

    Enum TransType As Integer
        Cash = 0
        Check = 1
    End Enum

    Friend TransactionMode As TransType
    Friend BoughtItems As CollectionItemData

    Private Sub frmSales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Seeder.ItemMasterData()

        ClearField()
        txtSearch.Select()
    End Sub

#Region "Function"
    Friend Sub AddItem(ByVal itm As ItemData)
        Dim lv As ListViewItem = lvSale.Items

    End Sub
#End Region

#Region "GUI"
    Private Sub ClearField()
        lvSale.Items.Clear()
        lblCustomer.Text = "One-Time Customer"
        txtSearch.Text = ""
        Display_Total(0)
    End Sub

    Private Sub Load_asCash()
        lblMode.Text = "CASH"
    End Sub

    Private Sub Load_asCheck()
        lblMode.Text = "CHECK"
    End Sub

    Private Function Display_Total(ByVal tot As Double) As Double
        Dim final As Double = tot.ToString("##000.00")
        lblTotal.Text = String.Format("Php {0:#,##0.00}", final)

        Return final
    End Function
#End Region

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class