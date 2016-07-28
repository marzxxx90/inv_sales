Public Class frmSTART

    Private isClosing As Boolean = True

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        CurrentDate = dtp_current.Value
        isClosing = False

        frmMain.Show()
        frmMain.tmr_Closer.Enabled = True
        Me.Close()
    End Sub

    Private Sub frmSTART_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        If isClosing Then End
    End Sub

    Private Sub dtp_current_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtp_current.KeyPress
        If isEnter(e) Then
            btnStart.PerformClick()
        End If
    End Sub
End Class