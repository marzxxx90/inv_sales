﻿Imports System.Data.Odbc

Friend Module database
    Public con As OdbcConnection
    Public ReaderCon As OdbcConnection
    Friend dbName As String = "INVENTORY.FDB"
    Friend fbUser As String = "SYSDBA"
    Friend fbPass As String = "masterkey"
    Friend fbDataSet As New DataSet
    Friend conStr As String = String.Empty

    Private DBversion As String = "1.0.9"
    Private language() As String = _
        {"Connection error failed."}

    Public Sub dbOpen()
        conStr = "DRIVER=Firebird/InterBase(r) driver;User=" & fbUser & ";Password=" & fbPass & ";Database=" & dbName & ";"

        con = New OdbcConnection(conStr)
        Try
            con.Open()
        Catch ex As Exception
            con.Dispose()
            MsgBox(language(0) + vbCrLf + ex.Message.ToString, vbCritical, "Connecting Error")
            Log_Report(ex.Message.ToString)
            Log_Report(String.Format("User: {0}", fbUser))
            Log_Report(String.Format("Database: {0}", dbName))
            Exit Sub
        End Try
    End Sub

    Public Sub dbClose()
        con.Close()
    End Sub

    Friend Function isReady() As Boolean
        Dim ready As Boolean = False
        Try
            dbOpen()
            ready = True
        Catch ex As Exception
            Console.WriteLine("[ERROR] " & ex.Message.ToString)
            Return False
        End Try

        Return ready
    End Function

    ''' <summary>
    ''' Module 001
    ''' Save the Dataset to the database
    ''' </summary>
    ''' <param name="dsEntry">Database with Table Name as Database Table Name</param>
    ''' <returns>Boolean: Success Result</returns>
    ''' <remarks></remarks>
    Friend Function SaveEntry(ByVal dsEntry As DataSet, Optional ByVal isNew As Boolean = True) As Boolean
        If dsEntry Is Nothing Then
            Return False
        End If

        dbOpen()

        Dim da As OdbcDataAdapter
        Dim ds As New DataSet, mySql As String, fillData As String
        ds = dsEntry

        'Save all tables in the dataset
        For Each dsTable As DataTable In dsEntry.Tables
            fillData = dsTable.TableName
            mySql = "SELECT * FROM " & fillData
            If Not isNew Then
                Dim colName As String = dsTable.Columns(0).ColumnName
                Dim idx As Integer = dsTable.Rows(0).Item(0)
                mySql &= String.Format(" WHERE {0} = {1}", colName, idx)

                Console.WriteLine("ModifySQL: " & mySql)
            End If

            da = New OdbcDataAdapter(mySql, con)
            Dim cb As New OdbcCommandBuilder(da) 'Required in Saving/Update to Database
            da.Update(ds, fillData)
        Next

        dbClose()
        Return True
    End Function

    Friend Function DBCompatibilityCheck() As Boolean
        Console.WriteLine("Checking database compatibility...")
        Dim strDB As String = GetOption("DBVersion")

        If DBversion = strDB Then
            Console.WriteLine("Success!")
            Return True
        Else
            Console.WriteLine("Database Version didn't match... " & strDB)
            Return False
        End If
    End Function

    ' Module 002
    Friend Function LoadSQL(ByVal mySql As String, Optional ByVal tblName As String = "QuickSQL") As DataSet
        dbOpen()

        Dim da As OdbcDataAdapter
        Dim ds As New DataSet, fillData As String = tblName

        da = New OdbcDataAdapter(mySql, con)
        da.Fill(ds, fillData)

        dbClose()

        Return ds
    End Function

    Friend Function LoadSQL_byDataReader(ByVal mySql As String) As OdbcDataReader
        Dim myCom As OdbcCommand = New OdbcCommand(mySql, ReaderCon)
        Dim reader As OdbcDataReader = myCom.ExecuteReader()

        Return reader
    End Function

    Public Sub dbReaderOpen()
        conStr = "DRIVER=Firebird/InterBase(r) driver;User=" & fbUser & ";Password=" & fbPass & ";Database=" & dbName & ";"

        ReaderCon = New OdbcConnection(conStr)
        Try
            ReaderCon.Open()
        Catch ex As Exception
            ReaderCon.Dispose()
            MsgBox(language(0) + vbCrLf + ex.Message.ToString, vbCritical, "Connecting Error")
            Log_Report(ex.Message.ToString)
            Exit Sub
        End Try
    End Sub

    Public Sub dbReaderClose()
        ReaderCon.Close()
    End Sub

    Friend Function GetOption(ByVal keys As String) As String
        Dim mySql As String = "SELECT * FROM tblmaintenance WHERE opt_keys = '" & keys & "'"
        Dim ret As String
        Try
            Dim ds As DataSet = LoadSQL(mySql)
            ret = ds.Tables(0).Rows(0).Item("opt_values")
        Catch ex As Exception
            ret = 0
        End Try

        Return ret
    End Function

    Friend Sub UpdateOptions(ByVal key As String, ByVal value As String)
        Dim mySql As String = "SELECT * FROM tblMaintenance WHERE opt_keys = '" & key & "'"
        Dim fillData As String = "tblMaintenance"
        Dim ds As DataSet = LoadSQL(mySql, fillData)

        If ds.Tables(fillData).Rows.Count = 0 Then
            Dim dsNewRow As DataRow
            dsNewRow = ds.Tables(fillData).NewRow
            With dsNewRow
                .Item("opt_keys") = key
                .Item("opt_values") = value
            End With
            ds.Tables(fillData).Rows.Add(dsNewRow)
            SaveEntry(ds)
        Else
            ds.Tables(0).Rows(0).Item("opt_values") = value
            SaveEntry(ds, False)
        End If

        If key = "RevolvingFund" Then
            mySql = "SELECT * FROM TBLCASH WHERE TRANSNAME = 'Revolving Fund'"
            fillData = "tblCash"

            ds = LoadSQL(mySql, fillData)
            ds.Tables(fillData).Rows(0).Item("SAPACCOUNT") = value
            SaveEntry(ds, False)
        End If
        Console.WriteLine("Option updated. " & key)
    End Sub
End Module
