<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInvSelect
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnIMD = New System.Windows.Forms.Button()
        Me.btnGR = New System.Windows.Forms.Button()
        Me.btnGI = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnIMD
        '
        Me.btnIMD.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnIMD.Location = New System.Drawing.Point(74, 12)
        Me.btnIMD.Name = "btnIMD"
        Me.btnIMD.Size = New System.Drawing.Size(116, 70)
        Me.btnIMD.TabIndex = 0
        Me.btnIMD.Text = "Item Master Data"
        Me.btnIMD.UseVisualStyleBackColor = True
        '
        'btnGR
        '
        Me.btnGR.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGR.Location = New System.Drawing.Point(12, 88)
        Me.btnGR.Name = "btnGR"
        Me.btnGR.Size = New System.Drawing.Size(116, 70)
        Me.btnGR.TabIndex = 1
        Me.btnGR.Text = "Goods Receipt"
        Me.btnGR.UseVisualStyleBackColor = True
        '
        'btnGI
        '
        Me.btnGI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGI.Location = New System.Drawing.Point(134, 88)
        Me.btnGI.Name = "btnGI"
        Me.btnGI.Size = New System.Drawing.Size(116, 70)
        Me.btnGI.TabIndex = 2
        Me.btnGI.Text = "Goods Issue"
        Me.btnGI.UseVisualStyleBackColor = True
        '
        'frmInvSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(267, 172)
        Me.Controls.Add(Me.btnGI)
        Me.Controls.Add(Me.btnGR)
        Me.Controls.Add(Me.btnIMD)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmInvSelect"
        Me.Text = "Inventory"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnIMD As System.Windows.Forms.Button
    Friend WithEvents btnGR As System.Windows.Forms.Button
    Friend WithEvents btnGI As System.Windows.Forms.Button
End Class
