Public Class ProductCollection
  Inherits System.Collections.CollectionBase

  Public Overloads Sub Add(ByRef Product As ProductClass)
    'Invokes Add method of the List object to add the item
    List.Add(Product)
  End Sub

  Public Overloads Sub Add(ByVal Name As String)
    'Invokes Add method of the List object to add the item
    Dim Product As New ProductClass() : Product.Name = Name
    List.Add(Product)
  End Sub

  Public Overloads Sub Add(ByVal Index As Integer)
    'Invokes Add method of the List object to add the item
    Dim Product As New ProductClass() : Product.Name = "Product" & Index.ToString
    List.Insert(Index, Product)
  End Sub

  Public Overloads Sub Remove(ByRef Product As ProductClass)
    'Make sure the item it in the list and remove
    Dim i As Integer = GetIndexOf(Product.Name)
    If i > -1 Then List.RemoveAt(i)
  End Sub

  Public Overloads Sub Remove(ByVal Name As String)
    'Make sure the Name is valid and remove the item
    Dim i As Integer = GetIndexOf(Name)
    If i > -1 Then List.RemoveAt(i)
  End Sub

  Public Overloads Sub Remove(ByVal Index As Integer)
    'Make sure the index is valid and remove the item
    If Index > 0 And Index < (Count - 1) Then
      List.RemoveAt(Index)
    End If
  End Sub

  Default Public Overloads ReadOnly Property Item(ByVal Name As String) As ProductClass
    Get
      'The appropriate item is retrieved from the List object,
      '  explicitly cast to the item type, then returned to the caller
      Return CType(List.Item(GetIndexOf(Name)), ProductClass)
    End Get
  End Property

  Default Public Overloads ReadOnly Property Item(ByVal Index As Integer) As ProductClass
    Get
      'The appropriate item is retrieved from the List object,
      '  explicitly cast to the item type, then returned to the caller
      Return CType(List.Item(Index), ProductClass)
    End Get
  End Property

  Public Overloads Function GetIndexOf(ByRef Product As ProductClass) As Integer
    'Return index of Product in list
    Return List.IndexOf(Product)
  End Function

  Public Overloads Function GetIndexOf(ByVal Name As String) As Integer
    'Return index of Product in list
    Return List.IndexOf(Me(Name))
  End Function

  Public Function FirstByIndex() As ProductClass
    Return CType(List.Item(0), ProductClass)
  End Function

  Public Function LastByIndex() As ProductClass
    Return CType(List.Item(List.Count - 1), ProductClass)
  End Function

End Class