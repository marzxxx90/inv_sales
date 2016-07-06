Module Seeder

    Friend Sub ItemMasterData()
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
    End Sub

End Module
