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
        'UNDERCONSTRUCTION()
        loadChild(frmInvSelect)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Sub MaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaintenanceToolStripMenuItem.Click
        frmMaintenance.Show()
    End Sub

    Private Sub tmr_Closer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmr_Closer.Tick
        tssCurrentDate.Text = CurrentDate.ToLongDateString & " " & Now.ToLongTimeString
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        frmSTART.ShowDialog()
    End Sub

    Private Sub SalesReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesReportToolStripMenuItem.Click
        qrySelectiveDate.ViewReport = qrySelectiveDate.ReportType.Sales
        qrySelectiveDate.Show()
    End Sub

    Private Sub OutstandingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutstandingToolStripMenuItem.Click
        Dim mySql As String = "SELECT * FROM ITEMMASTER WHERE onHold <> 1 AND onHand <> 0 AND isInv = 1 ORDER BY ITEMCODE ASC"
        Dim ds As DataSet = LoadSQL(mySql)

        frmReport.ReportInit(mySql, "dsOutstanding", "Reports\Outstanding.rdlc")
        frmReport.Show()
    End Sub
End Class