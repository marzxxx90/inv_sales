Imports Microsoft.Reporting.WinForms

Public Class frmReport

    Private Sub frmReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Me.rv_display.RefreshReport()
    End Sub

    Friend Sub ReportInit(ByVal mySql As String, ByVal dsName As String, ByVal rptUrl As String, _
                          Optional ByVal addPara As Dictionary(Of String, String) = Nothing, Optional ByVal hasUser As Boolean = True)
        Try
            Dim ds As DataSet = LoadSQL(mySql, dsName)
            If ds Is Nothing Then Exit Sub

            Console.WriteLine("SQL: " & mySql)
            Console.WriteLine("Max: " & ds.Tables(dsName).Rows.Count)
            Console.WriteLine("Report is Existing? " & System.IO.File.Exists(Application.StartupPath & "\" & rptUrl))

            With rv_display
                .ProcessingMode = ProcessingMode.Local
                .LocalReport.ReportPath = rptUrl
                .LocalReport.DataSources.Clear()

                .LocalReport.DataSources.Add(New ReportDataSource(dsName, ds.Tables(dsName)))

                If Not addPara Is Nothing Then
                    For Each nPara In addPara
                        Dim tmpPara As New ReportParameter
                        tmpPara.Name = nPara.Key
                        tmpPara.Values.Add(nPara.Value)
                        .LocalReport.SetParameters(New ReportParameter() {tmpPara})
                    Next
                End If

                .RefreshReport()
            End With
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical, "REPORT GENERATE ERROR")
            Log_Report("REPORT - " & ex.ToString)
        End Try
    End Sub
End Class