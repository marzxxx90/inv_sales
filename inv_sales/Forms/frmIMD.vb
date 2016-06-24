Public Class frmIMD

    Private Sub frmIMD_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ClearFields()
    End Sub

    Private Sub ClearFields()
        txtCode.Text = ""
        txtDescription.Text = ""
        chkHold.Checked = False
        cboCat.Items.Clear()
        cboSubCat.Items.Clear()
        txtUoM.Text = ""
        txtPrice.Text = ""
        chkInv.Checked = False
        txtSale.Text = ""
        txtDev.Text = ""
        txtRemarks.Text = ""
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class
