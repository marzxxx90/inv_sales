Module Seeder

    Friend Sub ItemMasterData()
        Dim mySql As String = "DELETE FROM ITEMMASTER WHERE ITEMID > 0"
        RunCommand(mySql) 'Empty the database

        Dim seed As New ItemData
        With seed
            .ItemCode = "CEL 00001"
            .Description = "SAMSUNG J1 2013"
            .UnitPrice = 4500
            .SalePrice = 5600
            .Category = "GADGET"
            .SubCategory = "CELLPHONE"
            .UnitOfMeasure = "PCS"
            .MinimumDeviation = 3
            .isSaleable = True
            .isInventoriable = True
            .Remarks = "From seed module"

            .Save_ItemData()
        End With

        seed = New ItemData
        With seed
            .ItemCode = "CEL 00002"
            .Description = "ASUS ZENFONE GO 5"
            .UnitPrice = 4300
            .SalePrice = 4500
            .Category = "GADGET"
            .SubCategory = "CELLPHONE"
            .UnitOfMeasure = "PCS"
            .MinimumDeviation = 5
            .isSaleable = True
            .isInventoriable = True
            .Remarks = "From seed module"

            .Save_ItemData()
        End With

        seed = New ItemData
        With seed
            .ItemCode = "BAT 00002"
            .Description = "BLC 510"
            .UnitPrice = 50
            .SalePrice = 150
            .Category = "BATTERY"
            '.SubCategory = "CELLPHONE"
            .UnitOfMeasure = "PCS"
            .MinimumDeviation = 13
            .isSaleable = True
            .isInventoriable = True
            .Remarks = "From seed module"

            .Save_ItemData()
        End With
    End Sub

End Module
