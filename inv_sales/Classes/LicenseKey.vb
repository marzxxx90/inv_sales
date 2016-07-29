Imports Microsoft.Win32
Imports encapsulatedserialkey

Public Class LicenseKey

    Dim RegLicense As String = "SOFTWARE\cdt-S0ft\InventorySales\"

    Friend Function GetHardwareKey() As String
        Dim hwkey As New clsSerialize, hardwareKey As String = "", errInfo As String = ""
        hardwareKey = RegValue(RegistryHive.LocalMachine, RegLicense, "Hardware", errInfo)

        If hardwareKey = "" Or errInfo <> "" Then
            hardwareKey = hwkey.GetHardwareKey
            If errInfo <> "" Then
                Log_Report("[HardwareKey] " & errInfo)
                Exit Function
            End If

            If Not WriteToRegistry(RegistryHive.LocalMachine, RegLicense, "Hardware", hardwareKey) Then
                MsgBox("Please use your ADMIN account", MsgBoxStyle.Critical, "Permission Error")
                Exit Function
            End If
        End If

        If hardwareKey <> hwkey.GetHardwareKey Then
            MsgBox("Registry has been tampered" + vbCr + "Contact your supplier", MsgBoxStyle.Critical, "Data Tampering")
            End
        End If

        Return hardwareKey
    End Function

    Friend Function ActivatingLicense(ByVal LicenseKey As String, ByVal RegisteredTo As String, ByVal HardwareKey As String) As Boolean
        Dim eskey As New clsSerialize
        If LicenseKey = eskey.GetLicenseKey(HardwareKey, RegisteredTo) Then
            Dim lKstat As Boolean, regStat As Boolean
            lKstat = WriteToRegistry(RegistryHive.LocalMachine, RegLicense, "License", LicenseKey)
            regStat = WriteToRegistry(RegistryHive.LocalMachine, RegLicense, "Registered", RegisteredTo)

            Return True
        Else
            Return False
        End If
    End Function

#Region "System Function"

    ''' <summary>
    ''' Check if the System is Licensed
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks>If Failed, software is not license</remarks>
    Private Function e001() As Boolean
        Dim serialKeys As New clsSerialize
        Dim errInfo As String, licKey As String, hardKey As String, regTo As String

        licKey = RegValue(RegistryHive.LocalMachine, "SOFTWARE\StealthAssassin\ArchiveSystem\", "License", errInfo)
        If errInfo <> "" Then
            MsgBox("Error: " & errInfo, MsgBoxStyle.Critical, "Error")
            Return False
        End If
        hardKey = RegValue(RegistryHive.LocalMachine, "SOFTWARE\StealthAssassin\ArchiveSystem\", "Hardware", errInfo)
        regTo = RegValue(RegistryHive.LocalMachine, "SOFTWARE\StealthAssassin\ArchiveSystem\", "Registered", errInfo)

        If hardKey = "" Or regTo = "" Then
            Return False
        End If
        If hardKey <> serialKeys.GetHardwareKey Or licKey <> serialKeys.GetLicenseKey(hardKey, regTo) Then
            Return False
        End If

        Return True
    End Function

    Private Function WriteToRegistry(ByVal _
    ParentKeyHive As RegistryHive, _
    ByVal SubKeyName As String, _
    ByVal ValueName As String, _
    ByVal Value As Object) As Boolean

        Dim objSubKey As RegistryKey
        Dim sException As String
        Dim objParentKey As RegistryKey
        Dim bAns As Boolean


        Try
            Select Case ParentKeyHive
                Case RegistryHive.ClassesRoot
                    objParentKey = Registry.ClassesRoot
                Case RegistryHive.CurrentConfig
                    objParentKey = Registry.CurrentConfig
                Case RegistryHive.CurrentUser
                    objParentKey = Registry.CurrentUser
                Case RegistryHive.DynData
                    objParentKey = Registry.DynData
                Case RegistryHive.LocalMachine
                    objParentKey = Registry.LocalMachine
                Case RegistryHive.PerformanceData
                    objParentKey = Registry.PerformanceData
                Case RegistryHive.Users
                    objParentKey = Registry.Users
            End Select

            'Open 
            objSubKey = objParentKey.OpenSubKey(SubKeyName, True)
            'create if doesn't exist
            If objSubKey Is Nothing Then
                objSubKey = objParentKey.CreateSubKey(SubKeyName)
            End If

            objSubKey.SetValue(ValueName, Value)
            bAns = True
        Catch ex As Exception
            bAns = False
        End Try

        Return True
    End Function

    Private Function RegValue(ByVal Hive As RegistryHive, _
           ByVal Key As String, ByVal ValueName As String, _
            Optional ByVal ErrInfo As String = "") As String

        Dim objParent As RegistryKey
        Dim objSubkey As RegistryKey
        Dim sAns As String
        Select Case Hive
            Case RegistryHive.ClassesRoot
                objParent = Registry.ClassesRoot
            Case RegistryHive.CurrentConfig
                objParent = Registry.CurrentConfig
            Case RegistryHive.CurrentUser
                objParent = Registry.CurrentUser
            Case RegistryHive.DynData
                objParent = Registry.DynData
            Case RegistryHive.LocalMachine
                objParent = Registry.LocalMachine
            Case RegistryHive.PerformanceData
                objParent = Registry.PerformanceData
            Case RegistryHive.Users
                objParent = Registry.Users

        End Select

        Try
            objSubkey = objParent.OpenSubKey(Key)
            'if can't be found, object is not initialized
            If Not objSubkey Is Nothing Then
                sAns = (objSubkey.GetValue(ValueName))
            End If

        Catch ex As Exception

            ErrInfo = ex.Message
        Finally

            'if no error but value is empty, populate errinfo
            If ErrInfo = "" And sAns = "" Then
                ErrInfo = _
                   "No value found for requested registry key"
            End If
        End Try
        Return sAns
    End Function
#End Region

End Class