Public Class InventoryChange

    Private fillData As String = "DOC"

    Enum DocType As Integer
        DocOut = 0
        DocIn = 1
    End Enum

#Region "Properties"
    Private _docNum As Integer
    Public Property DocNum() As Integer
        Get
            Return _docNum
        End Get
        Set(ByVal value As Integer)
            _docNum = value
        End Set
    End Property

    Private _docType As DocType
    Public Property DocumentType() As DocType
        Get
            Return _docType
        End Get
        Set(ByVal value As DocType)
            _docType = value
        End Set
    End Property

    Private _docDate As Date
    Public Property DocDate() As Date
        Get
            Return _docDate
        End Get
        Set(ByVal value As Date)
            _docDate = value
        End Set
    End Property

    Private _businessPartner As String
    Public Property BusinessPartner() As String
        Get
            Return _businessPartner
        End Get
        Set(ByVal value As String)
            _businessPartner = value
        End Set
    End Property

    Private _docTotal As Double
    Public Property DocTotal() As Double
        Get
            Return _docTotal
        End Get
        Set(ByVal value As Double)
            _docTotal = value
        End Set
    End Property

    Private _comments As String
    Public Property Remarks() As String
        Get
            Return _comments
        End Get
        Set(ByVal value As String)
            _comments = value
        End Set
    End Property

    Private _itemList As CollectionItemData
    Public Property ItemList() As CollectionItemData
        Get
            Return _itemList
        End Get
        Set(ByVal value As CollectionItemData)
            _itemList = value
        End Set
    End Property

    Private _userID As Integer
    Public Property UserID() As Integer
        Get
            Return _userID
        End Get
        Set(ByVal value As Integer)
            _userID = value
        End Set
    End Property

#End Region

    Public Sub SaveDocument()
        Dim ds As DataSet
        Dim mySql As String = _
            "SELECT * FROM " & fillData & _
            " ROWS 1"

        'Saving Document
        Dim dsNewRow As DataRow
        ds = LoadSQL(mySql, fillData)
        dsNewRow = ds.Tables(fillData).NewRow
        With dsNewRow
            .Item("DOCTYPE") = _docType
            .Item("CODE") = _docNum
            .Item("DOCDATE") = _docDate
            .Item("DOCTOTAL") = _docTotal
            .Item("REMARKS") = _comments
            .Item("USERID") = _userID
        End With
        ds.Tables(fillData).Rows.Add(dsNewRow)
        database.SaveEntry(ds)

        'Saving DocumentLines
        mySql = "SELECT * FROM DOCLINES ROWS 1"
        Dim LastID As Integer = GetLastID()
        Dim docLines As DataSet
        docLines = LoadSQL(mySql, fillData)
        For Each item As ItemData In _itemList
            dsNewRow = docLines.Tables(fillData).NewRow
            With dsNewRow
                .Item("DOCID") = LastID
                .Item("ITEMCODE") = item.ItemCode
                .Item("DESCRIPTION") = item.Description
                .Item("QTY") = item.Qty
                .Item("UNITPRICE") = item.UnitPrice
                .Item("SALEPRICE") = item.SalePrice
                .Item("ROWTOTAL") = item.RowTotal
                .Item("UOM") = item.UnitOfMeasure
            End With
            docLines.Tables(fillData).Rows.Add(dsNewRow)
        Next

        database.SaveEntry(docLines)
    End Sub

    Private Function GetLastID() As Integer
        Dim mySql As String = "SELECT * FROM " & fillData
        mySql &= " ORDER BY DOCID DESC ROWS 1"

        Dim ds As DataSet = LoadSQL(mySql)
        If ds.Tables(0).Rows.Count = 0 Then Return 0
        Return ds.Tables(0).Rows(0).Item("DOCID")
    End Function

    Public Sub LoadDocument()

    End Sub
End Class
