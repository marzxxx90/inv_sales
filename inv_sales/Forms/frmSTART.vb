Public Class frmSTART

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        CurrentDate = dtp_current.Value

        frmMain.Show()
        frmMain.tmr_Closer.Enabled = True
        Me.Close()
    End Sub
End Class