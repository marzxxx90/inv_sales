Public Class qrySelectiveDate

    Enum ReportType As Integer
        Sales = 0
    End Enum

    Friend SelectDate As ReportType

    Private Sub qrySelectiveDate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        Select Case SelectDate
            Case ReportType.Sales
                Sales_Report()
        End Select
    End Sub

    Private Sub Sales_Report()
        Dim mySql As String = "SELECT DISTINCT * FROM SALES_OR"
        mySql &= String.Format("WHERE DOCDATE BETWEEN '{0}' AND '{1}' ORDER BY ITEMCODE ASC", _
                               GetFirstDate(monSelect.SelectionStart), GetLastDate(monSelect.SelectionStart))

        frmReport.Show()
        frmReport.ReportInit(mySql, "SalesReport", "Reports\SalesReport.rdlc")
    End Sub
End Class