Module language

    Enum TitleSign As Integer
        Information = 0
        Warning = 1
        Critical = 2
    End Enum

    Friend Sub AlertBox(ByVal str As String, ByVal msgBtn As MsgBoxStyle, Optional ByVal title As TitleSign = TitleSign.Information)
        Dim titleBar As String = "U N K N O W N"
        Select Case title
            Case 0
                titleBar = "INFORMATION"
            Case 1
                titleBar = "W A R N I N G"
            Case 2
                titleBar = "E R R O R"
        End Select
        MsgBox(str, msgBtn, titleBar)
    End Sub

End Module
