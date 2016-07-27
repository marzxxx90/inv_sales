Public Class frmMaintenance

    Private Sub frmMaintenance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Load_Printer()
        Load_Data()
    End Sub

    Private Sub Load_Data()
        Dim or_number As Double = GetOption("ORNUM")
        txtOR.Text = String.Format("{0:0000000}", or_number)

        If GetOption("PRINTER") = "" Then Exit Sub
        cboPrinter.Text = GetOption("PRINTER")
    End Sub

    Private Sub Load_Printer()
        Dim tmpPrinterName As String

        cboPrinter.Items.Clear()
        For Each tmpPrinterName In Printing.PrinterSettings.InstalledPrinters
            cboPrinter.Items.Add(tmpPrinterName)
        Next
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub txtOR_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtOR.KeyPress
        DigitOnly(e)
        isEnter(e)
    End Sub

    Private Sub btnPost_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPost.Click
        UpdateOption("PRINTER", cboPrinter.Text)
        UpdateOption("ORNUM", txtOR.Text)

        MsgBox("Settings Updated", MsgBoxStyle.Information)
        Me.Close()
    End Sub

    Private Sub isEnter(ByVal e As KeyPressEventArgs)
        If Asc(e.KeyChar) = 13 Then btnPost.PerformClick()
    End Sub

    Private Sub txtOR_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOR.TextChanged

    End Sub
End Class