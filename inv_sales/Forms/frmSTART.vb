Public Class frmSTART

    Private isClosing As Boolean = True
    Private EXPIRELIMIT As Date = #8/2/2016#
    Private DEMOMSG As String = "THIS IS ONLY A DEMO" + vbCrLf + "IT WILL EXPIRED WITHIN {0} {1}"

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click
        CurrentDate = dtp_current.Value
        isClosing = False

        CheckExpired()

        frmMain.Show()
        frmMain.WindowState = FormWindowState.Maximized
        frmMain.tmr_Closer.Enabled = True
        Me.Close()
    End Sub

    Private Sub CheckExpired()
        Dim diff = EXPIRELIMIT - CurrentDate

        If CurrentDate > EXPIRELIMIT Then
            MsgBox("DEMO HAS EXPIRED", vbCritical, "E X P I R E D")
            End
        Else
            If diff.Days = 0 Then
                MsgBox("THIS IS THE LAST DAY OF THE DEMO" + vbCrLf + "PLEASE BE ADVICE", vbCritical, "EXPIRING...")
                Exit Sub
            End If

            MsgBox(String.Format(DEMOMSG, diff.Days, IIf(diff.Days > 1, "days", "day")), vbInformation, "EXPIRATION")
        End If
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