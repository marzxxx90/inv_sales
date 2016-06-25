Public Class ItemData

    Private mySql As String
    Private fillData As String = "ItemMaster"

#Region "Properties"
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

#End Region

#Region "Procedures and Functions"
    Public Sub Save_ItemData()
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
        End If

    End Sub
#End Region

End Class
