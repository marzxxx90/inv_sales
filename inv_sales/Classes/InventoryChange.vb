Public Class InventoryChange

    Enum DocType As Integer
        DocOut = 0
        DocIn = 1
    End Enum

    'Private _docNum As Integer
    'Private _docDate As Date
    'Private _businessPartner As String
    'Private _itemList As CollectionItemData

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

    Private _itemList As CollectionItemData
    Public Property ItemList() As CollectionItemData
        Get
            Return _itemList
        End Get
        Set(ByVal value As CollectionItemData)
            _itemList = value
        End Set
    End Property
#End Region

End Class
