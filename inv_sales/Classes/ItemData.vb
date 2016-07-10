Public Class ItemData

    Private mySql As String
    Private fillData As String = "ItemMaster"

#Region "Properties"
    Private _itemID As Integer
    Public Property ItemID() As Integer
        Get
            Return _itemID
        End Get
        Set(ByVal value As Integer)
            _itemID = value
        End Set
    End Property

    Private _ItemCode As String
    Public Property ItemCode() As String
        Get
            Return _ItemCode
        End Get
        Set(ByVal value As String)
            _ItemCode = value
        End Set
    End Property

    Private _Description As String
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(ByVal value As String)
            _Description = value
        End Set
    End Property

    Private _category As String
    Public Property Category() As String
        Get
            Return _category
        End Get
        Set(ByVal value As String)
            _category = value
        End Set
    End Property

    Private _subCategory As String
    Public Property SubCategory() As String
        Get
            Return _subCategory
        End Get
        Set(ByVal value As String)
            _subCategory = value
        End Set
    End Property

    Private _uom As String
    Public Property UnitOfMeasure() As String
        Get
            Return _uom
        End Get
        Set(ByVal value As String)
            _uom = value
        End Set
    End Property

    Private _unitPrice As Double
    Public Property UnitPrice() As Double
        Get
            Return _unitPrice
        End Get
        Set(ByVal value As Double)
            _unitPrice = value
        End Set
    End Property

    Private _salePrice As Double
    Public Property SalePrice() As Double
        Get
            Return _salePrice
        End Get
        Set(ByVal value As Double)
            _salePrice = value
        End Set
    End Property

    Private _minimumDeviation As Double
    Public Property MinimumDeviation() As Double
        Get
            Return _minimumDeviation
        End Get
        Set(ByVal value As Double)
            _minimumDeviation = value
        End Set
    End Property

    Private _isSale As Boolean
    Public Property isSaleable() As Boolean
        Get
            Return _isSale
        End Get
        Set(ByVal value As Boolean)
            _isSale = value
        End Set
    End Property

    Private _isInv As Boolean
    Public Property isInventoriable() As Boolean
        Get
            Return _isInv
        End Get
        Set(ByVal value As Boolean)
            _isInv = value
        End Set
    End Property

    Private _onHold As Boolean
    Public Property onHold() As Boolean
        Get
            Return _onHold
        End Get
        Set(ByVal value As Boolean)
            _onHold = value
        End Set
    End Property

    Private _remarks As String
    Public Property Remarks() As String
        Get
            Return _remarks
        End Get
        Set(ByVal value As String)
            _remarks = value
        End Set
    End Property

    'DocLines
    Private _itemQty As Double
    Public Property Qty() As Double
        Get
            Return _itemQty
        End Get
        Set(ByVal value As Double)
            _itemQty = value
        End Set
    End Property

    Public ReadOnly Property RowTotal() As Double
        Get
            Return _itemQty * _unitPrice
        End Get
    End Property

    Private _onHand As Double
    Public Property onHand() As Double
        Get
            Return _onHand
        End Get
        Set(ByVal value As Double)
            _onHand = value
        End Set
    End Property
#End Region

#Region "Procedures and Functions"
    Public Sub Save_ItemData()
        Dim isNew As Boolean = True
        mySql = "SELECT * FROM " & fillData
        mySql &= String.Format(" WHERE ITEMCODE = '{0}'", ItemCode)

        Dim ds As DataSet
        ds = LoadSQL(mySql, fillData)
        If ds.Tables(fillData).Rows.Count = 0 Then
            Dim dsNewRow As DataRow
            dsNewRow = ds.Tables(fillData).NewRow
            With dsNewRow
                .Item("ItemCode") = ItemCode
                .Item("Description") = Description
                .Item("Categories") = Category
                .Item("SubCat") = SubCategory
                .Item("UoM") = UnitOfMeasure
                .Item("UnitPrice") = UnitPrice
                .Item("SalePrice") = SalePrice
                .Item("MinDev") = MinimumDeviation
                .Item("isSale") = IIf(isSaleable, 1, 0)
                .Item("isInv") = IIf(isInventoriable, 1, 0)
                .Item("onHold") = IIf(onHold, 1, 0)
                .Item("Comments") = Remarks
            End With
            ds.Tables(fillData).Rows.Add(dsNewRow)
        Else
            With ds.Tables(fillData).Rows(0)
                .Item("ItemCode") = ItemCode
                .Item("Description") = Description
                .Item("Categories") = Category
                .Item("SubCat") = SubCategory
                .Item("UoM") = UnitOfMeasure
                .Item("UnitPrice") = UnitPrice
                .Item("SalePrice") = SalePrice
                .Item("MinDev") = MinimumDeviation
                .Item("isSale") = IIf(isSaleable, 1, 0)
                .Item("isInv") = IIf(isInventoriable, 1, 0)
                .Item("onHold") = IIf(onHold, 1, 0)
                .Item("Comments") = Remarks
            End With

            isNew = False
        End If

        database.SaveEntry(ds, isNew)
    End Sub

    Public Sub Load_Item(ByVal itemCode As String)
        Dim mySql As String = String.Format("SELECT * FROM {1} WHERE ITEMCODE = '{0}'", itemCode, fillData)
        Dim ds As DataSet = LoadSQL(mySql)

        If ds.Tables(0).Rows.Count = 0 Then Exit Sub
        With ds.Tables(0).Rows(0)
            _itemID = .Item("ITEMID")
            _ItemCode = .Item("ITEMCODE")
            _Description = .Item("DESCRIPTION")
            _category = .Item("CATEGORIES")
            _subCategory = .Item("SUBCAT")
            _uom = .Item("UOM")
            _unitPrice = .Item("UNITPRICE")
            _salePrice = .Item("SALEPRICE")
            _minimumDeviation = .Item("MINDEV")
            _isSale = IIf(.Item("ISSALE") = 1, True, False)
            _isInv = IIf(.Item("ISINV") = 1, True, False)
            _onHold = IIf(.Item("ONHOLD") = 1, True, False)
            _remarks = IIf(IsDBNull(.Item("COMMENTS")), "", .Item("COMMENTS"))

            _onHand = getOnHand()
        End With
    End Sub

    Private Function getOnHand() As Double
        Dim mySql As String = "SELECT * FROM ITEMMASTER WHERE ITEMID = " & _itemID
        Dim ds As DataSet = LoadSQL(mySql)

        Return ds.Tables(0).Rows(0).Item("ONHAND")
    End Function

    Public Sub LoadReader_Item(ByVal rd As IDataReader)
        On Error Resume Next

        With rd
            _itemID = .Item("ITEMID")
            _ItemCode = .Item("ITEMCODE")
            _Description = .Item("DESCRIPTION")
            _category = .Item("CATEGORIES")
            _subCategory = .Item("SUBCAT")
            _uom = .Item("UOM")
            _unitPrice = .Item("UNITPRICE")
            _salePrice = .Item("SALEPRICE")
            _minimumDeviation = .Item("MINDEV")
            _isSale = IIf(.Item("ISSALE") = 1, True, False)
            _isInv = IIf(.Item("ISINV") = 1, True, False)
            _onHold = IIf(.Item("ONHOLD") = 1, True, False)
            _remarks = .Item("COMMENTS")

            _onHand = getOnHand()
        End With
    End Sub
#End Region

End Class
