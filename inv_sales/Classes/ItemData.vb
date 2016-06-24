Public Class ItemData

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

#End Region

End Class
