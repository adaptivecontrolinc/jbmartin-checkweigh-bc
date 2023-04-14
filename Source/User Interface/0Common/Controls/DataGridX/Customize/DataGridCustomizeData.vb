Public Class DataGridCustomizeData
  Property GridLayout As DataGridLayout
  Property TableSchema As System.Data.DataTable

  Private sourceDataGrid As DataGridX
  Private sourceGridLayout As DataGridLayout

  Sub Connect(dataGrid As DataGridX)
    sourceDataGrid = dataGrid
    sourceGridLayout = dataGrid.GridLayout

    ' Copy the grid layout for editing
    GridLayout = sourceGridLayout.CopyLayout

    ' Get the table schema so we can populate the source column list
    TableSchema = dataGrid.Table.Clone
  End Sub

  Sub AddSourceColumn(name As String)
    AddColumnToGridLayout(name)
    ResetDisplayOrder()
  End Sub

  Sub AddAllSourceColumns()
    For Each col As DataColumn In TableSchema.Columns
      AddColumnToGridLayout(col.ColumnName)
    Next
    ResetDisplayOrder()
  End Sub

  Sub RemoveTargetColumn(name As String)
    Dim item = GetTargetColumn(name)
    If item IsNot Nothing Then GridLayout.Columns.Remove(item)
    ResetDisplayOrder()
  End Sub

  Sub RemoveAllTargetColumns()
    GridLayout.Columns.Clear()
  End Sub

  Sub ReplaceTargetColumn(originalColumn As DataGridLayout.Column, newColumn As DataGridLayout.Column)
    Dim index = GridLayout.Columns.IndexOf(originalColumn)
    GridLayout.Columns.Item(index) = newColumn
  End Sub


  Sub MoveTargetColumnUp(name As String)
    Dim item = GetTargetColumn(name)
    If item Is Nothing Then Exit Sub

    Dim index = GridLayout.Columns.IndexOf(item)
    If index > 0 Then
      GridLayout.Columns.Remove(item)
      GridLayout.Columns.Insert(index - 1, item)
    End If

    ResetDisplayOrder()
  End Sub

  Sub MoveTargetColumnDown(name As String)
    Dim item = GetTargetColumn(name)
    If item Is Nothing Then Exit Sub

    Dim index = GridLayout.Columns.IndexOf(item)
    If index < GridLayout.Columns.Count - 1 Then
      GridLayout.Columns.Remove(item)
      GridLayout.Columns.Insert(index + 1, item)
    End If

    ResetDisplayOrder()
  End Sub


  Private Sub AddColumnToGridLayout(name As String)
    If TargetColumnExists(name) Then Exit Sub
    GridLayout.Columns.Add(New DataGridLayout.Column(name))
  End Sub


  Private Sub ResetDisplayOrder()
    If GridLayout.Columns Is Nothing OrElse GridLayout.Columns.Count <= 0 Then Exit Sub
    Dim maxNumber As Integer = GridLayout.Columns.Count

    Dim displayOrder As Integer = 1
    For Each col In GridLayout.Columns
      col.DisplayOrder = displayOrder
      displayOrder += 1
    Next
  End Sub


  Public Function TargetColumnExists(name As String) As Boolean
    ' Make sure there are columns in the collection
    If GridLayout.Columns Is Nothing OrElse GridLayout.Columns.Count <= 0 Then Return False
    ' Check to see if the item is already in the target list
    For Each col In GridLayout.Columns
      If col.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) Then Return True
    Next
    Return False
  End Function

  Public Function GetTargetColumn(name As String) As DataGridLayout.Column
    ' Make sure there are columns in the collection
    If GridLayout.Columns Is Nothing OrElse GridLayout.Columns.Count <= 0 Then Return Nothing
    ' Return the matching layout column from the collection
    For Each col In GridLayout.Columns
      If col.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) Then Return col
    Next
    Return Nothing
  End Function

End Class
