﻿Public Class qrySelectiveDate

    Enum ReportType As Integer
        Sales = 0
    End Enum

    Friend ViewReport As ReportType

    Private Sub btnSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelect.Click
        Select Case ViewReport
            Case ReportType.Sales
                Sales_Report()
        End Select
    End Sub

    Private Sub Sales_Report()
        Dim mySql As String = "SELECT * FROM SALES_OR "
        mySql &= String.Format("WHERE DOCDATE BETWEEN '{0}' AND '{1}' ORDER BY CODE ASC", _
                               GetFirstDate(monSelect.SelectionStart).ToShortDateString, GetLastDate(monSelect.SelectionStart).ToShortDateString)

        Console.WriteLine(mySql)

        frmReport.Show()
        frmReport.ReportInit(mySql, "SalesReport", "Reports\SalesReport.rdlc")
    End Sub
End Class