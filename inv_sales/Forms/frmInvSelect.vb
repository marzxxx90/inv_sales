Public Class frmInvSelect

    Private Sub btnIMD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIMD.Click
        frmIMD.Show()
    End Sub

    Private Sub btnGR_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGR.Click
        frmInventoryIn.Load_GoodsReceipt()
        frmInventoryIn.Show()
    End Sub

    Private Sub btnGI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGI.Click
        frmInventoryIn.Load_GoodsIssue()
        frmInventoryIn.Show()
    End Sub
End Class