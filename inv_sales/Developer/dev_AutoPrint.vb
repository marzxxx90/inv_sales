Imports Microsoft.Reporting.WinForms

Public Class dev_AutoPrint

    Private PRINTER_PT As String = GetOption("PRINTER")

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        PrintOR(TextBox1.Text)
    End Sub

    Private Sub PrintOR(ByVal docID As Integer)
        ' Check if able to print
        Dim printerName As String = PRINTER_PT
        If Not canPrint(printerName) Then Exit Sub

        ' Execute SQL
        Dim mySql As String = "SELECT * FROM SALES_OR WHERE DOCID = " & docID
        Dim ds As DataSet, fillData As String = "OR"
        ds = LoadSQL(mySql, fillData)

        ' Declare AutoPrint
        Dim autoPrint As Reporting
        Dim report As LocalReport = New LocalReport
        autoPrint = New Reporting

        ' Initialize Auto Print
        report.ReportPath = "Reports\OfficialReceipt.rdlc"
        report.DataSources.Add(New ReportDataSource(fillData, ds.Tables(fillData)))

        ' Assign Parameters
        Dim dic As New Dictionary(Of String, String)
        With ds.Tables(0).Rows(0)
            dic.Add("txtORNum", .Item("CODE"))
            dic.Add("txtPostingDate", .Item("DOCDATE"))
            dic.Add("txtCustomer", .Item("CUSTOMER"))
        End With

        ' Importer Parameters
        If Not dic Is Nothing Then
            For Each nPara In dic
                Dim tmpPara As New ReportParameter
                tmpPara.Name = nPara.Key
                tmpPara.Values.Add(nPara.Value)
                report.SetParameters(New ReportParameter() {tmpPara})
                Console.WriteLine(String.Format("{0}: {1}", nPara.Key, nPara.Value))
            Next
        End If

        ' Executing Auto Print
        autoPrint.Export(report)
        autoPrint.m_currentPageIndex = 0
        autoPrint.Print(printerName)

        'frmReport.ReportInit(mySql, "OR", "Reports\OfficialReceipt.rdlc", dic)
        'frmReport.Show()
    End Sub

    Private Function canPrint(ByVal printerName As String) As Boolean
        Try
            Dim printDocument As Drawing.Printing.PrintDocument = New Drawing.Printing.PrintDocument
            printDocument.PrinterSettings.PrinterName = printerName
            Return printDocument.PrinterSettings.IsValid
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub dev_AutoPrint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class