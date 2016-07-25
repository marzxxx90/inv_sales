Module mod_system

#Region "Global Variables"
    Public DEV_MODE As Boolean = False
    Public ADS_ESKIE As Boolean = True

    Public CurrentDate As Date = Now
#End Region

    Friend Function GetUserID() As Integer
        'TODO: GET USERID

        Return 0
    End Function

    Friend Function GetOption(ByVal key As String) As String
        Dim MySql As String = String.Format("SELECT * FROM MAINTENANCE WHERE SYS_KEY = '{0}'", key)
        Dim ds As DataSet = LoadSQL(MySql)

        If ds.Tables(0).Rows.Count = 0 Then
            Return "N/A"
        End If

        Return ds.Tables(0).Rows(0).Item("SYS_VALUE")
    End Function

    Friend Sub UpdateOption(ByVal key As String, ByVal value As String)
        Dim isNew As Boolean = False
        Dim MySql As String = String.Format("SELECT * FROM MAINTENANCE WHERE SYS_KEY = '{0}'", key)
        Dim ds As DataSet = LoadSQL(MySql, "MAINTENANCE")

        If ds.Tables(0).Rows.Count = 0 Then
            Dim dsNewRow As DataRow : isNew = True
            dsNewRow = ds.Tables(0).NewRow

            With dsNewRow
                .Item("SYS_KEY") = key
                .Item("SYS_VALUE") = value
                .Item("REMARKS") = "AUTOREMARKS"
            End With
            ds.Tables(0).Rows.Add(dsNewRow)
        Else
            ds.Tables(0).Rows(0).Item("SYS_VALUE") = value
        End If

        database.SaveEntry(ds, isNew)
    End Sub

    Public Function CommandPrompt(ByVal app As String, ByVal args As String) As String
        Dim oProcess As New Process()
        Dim oStartInfo As New ProcessStartInfo(app, args)
        oStartInfo.UseShellExecute = False
        oStartInfo.RedirectStandardOutput = True
        oStartInfo.WindowStyle = ProcessWindowStyle.Hidden
        oStartInfo.CreateNoWindow = True
        oProcess.StartInfo = oStartInfo

        oProcess.Start()

        Dim sOutput As String
        Using oStreamReader As System.IO.StreamReader = oProcess.StandardOutput
            sOutput = oStreamReader.ReadToEnd()
        End Using

        Return sOutput
    End Function

    ''' <summary>
    ''' Function use to input only numbers
    ''' </summary>
    ''' <param name="e">Keypress Event</param>
    ''' <remarks>Use the Keypress Event when calling this function</remarks>
    Friend Function DigitOnly(ByVal e As System.Windows.Forms.KeyPressEventArgs, Optional ByVal isWhole As Boolean = False)
        Console.WriteLine("char: " & e.KeyChar & " -" & Char.IsDigit(e.KeyChar))
        If e.KeyChar <> ControlChars.Back Then
            If isWhole Then
                e.Handled = Not (Char.IsDigit(e.KeyChar))
            Else
                e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ".")
            End If

        End If

        Return Not (Char.IsDigit(e.KeyChar))
    End Function

    Friend Function checkNumeric(ByVal txt As TextBox) As Boolean
        If IsNumeric(txt.Text) Then
            Return True
        End If

        Return False
    End Function

    Friend Function DreadKnight(ByVal str As String, Optional ByVal special As String = Nothing) As String
        str = str.Replace("'", "''")
        str = str.Replace("""", """""")

        If special <> Nothing Then
            str = str.Replace(special, "")
        End If

        Return str
    End Function

    ''' <summary>
    ''' Identify if the KeyPress is enter
    ''' </summary>
    ''' <param name="e">KeyPressEventArgs</param>
    ''' <returns>Boolean</returns>
    Friend Function isEnter(ByVal e As KeyPressEventArgs) As Boolean
        If Asc(e.KeyChar) = 13 Then
            Return True
        End If
        Return False
    End Function

    Friend Function GetCurrentAge(ByVal dob As Date) As Integer
        Dim age As Integer
        age = Today.Year - dob.Year
        If (dob > Today.AddYears(-age)) Then age -= 1
        Return age
    End Function

    ''' <summary>
    ''' Use to verify entry
    ''' </summary>
    ''' <param name="txtBox">TextBox of the Money</param>
    ''' <returns>Boolean</returns>
    Friend Function isMoney(ByVal txtBox As TextBox) As Boolean
        Dim isGood As Boolean = False

        If Double.TryParse(txtBox.Text, 0.0) Then
            isGood = True
        End If

        Return isGood
    End Function

    Friend Function GetFirstDate(ByVal curDate As Date) As Date
        Dim firstDay = DateSerial(curDate.Year, curDate.Month, 1)
        Return firstDay
    End Function

    Friend Function GetLastDate(ByVal curDate As Date) As Date
        Dim original As DateTime = curDate  ' The date you want to get the last day of the month for
        Dim lastOfMonth As DateTime = original.Date.AddDays(-(original.Day - 1)).AddMonths(1).AddDays(-1)

        Return lastOfMonth
    End Function

#Region "Log Module"
    Const LOG_FILE As String = "-log.txt"
    Private Sub CreateLog()
        Dim fsEsk As New System.IO.FileStream(Now.ToString("MMddyyyy") & LOG_FILE, IO.FileMode.CreateNew)
        fsEsk.Close()
    End Sub

    Friend Sub Log_Report(ByVal str As String)
        If Not System.IO.File.Exists(Now.ToString("MMddyyyy") & LOG_FILE) Then CreateLog()

        Dim recorded_log As String = _
            String.Format("[{0}] " & str, Now.ToString("MM/dd/yyyy HH:mm:ss"))

        Dim fs As New System.IO.FileStream(Now.ToString("MMddyyyy") & LOG_FILE, IO.FileMode.Append, IO.FileAccess.Write)
        Dim fw As New System.IO.StreamWriter(fs)
        fw.WriteLine(recorded_log)
        fw.Close()
        fs.Close()
        Console.WriteLine("Recored")
    End Sub
#End Region
End Module
