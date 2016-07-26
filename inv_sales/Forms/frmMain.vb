Public Class frmMain

    Private Sub tsbtnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnExit.Click
        End
    End Sub

    Private Sub tsbtnSales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnSales.Click
        loadChild(frmSales)
    End Sub

    Private Sub loadChild(ByVal f As Form)
        f.MdiParent = Me
        f.Show()
    End Sub

    Private Sub tsbtnInv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtnInv.Click
        loadChild(frmInvSelect)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub MaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaintenanceToolStripMenuItem.Click

    End Sub
End Class