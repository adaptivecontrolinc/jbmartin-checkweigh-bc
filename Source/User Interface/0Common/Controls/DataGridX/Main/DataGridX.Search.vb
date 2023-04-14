Partial Public Class DataGridX

  Sub Search(text As String)
    Search(text, "Code")
  End Sub

  Sub Search(text As String, columnName As String)
    ' Make sure the grid has data to search
    If Me.Table Is Nothing OrElse Me.Table.Rows.Count <= 0 Then Exit Sub

    ' If no current row (selected row ?) then search all the rows from the beginning
    If Me.CurrentRow Is Nothing Then
      SearchAll(text, columnName)
      Exit Sub
    End If

    ' If we have a current row check to see if it matches the search text if it does search forward (search Again)
    Dim tableRow = DirectCast(Me.CurrentRow.DataBoundItem, DataRowView).Row
    Dim formulaName = tableRow(columnName).ToString
    If formulaName.StartsWith(text, StringComparison.InvariantCultureIgnoreCase) Then
      SearchForward(text, columnName)
    Else
      SearchAll(text, columnName)
    End If
  End Sub

  Private Sub SearchAll(text As String, columnName As String)
    ' Search all grid rows starting at the first row
    For Each gridRow As DataGridViewRow In Me.Rows
      Dim tableRow = DirectCast(gridRow.DataBoundItem, DataRowView).Row
      Dim formulaName = tableRow(columnName).ToString
      If formulaName.StartsWith(text, StringComparison.InvariantCultureIgnoreCase) Then
        Me.ClearSelection()
        Me.CurrentCell = gridRow.Cells(0)
        gridRow.Selected = True
        Exit Sub
      End If
    Next
  End Sub

  Private Sub SearchForward(text As String, columnName As String)
    ' Search forward from the row after the current row
    Dim currentRowIndex = Me.Rows.IndexOf(Me.CurrentRow)
    ' Make sure we don't go out of bounds
    If currentRowIndex >= Me.Rows.Count - 1 Then Exit Sub
    ' Loop through the rows starting with the row after the current row
    For i As Integer = currentRowIndex + 1 To Me.Rows.Count - 1
      Dim gridRow = Me.Rows(i)
      Dim tableRow = DirectCast(gridRow.DataBoundItem, DataRowView).Row
      Dim formulaName = tableRow(columnName).ToString
      If formulaName.StartsWith(text, StringComparison.InvariantCultureIgnoreCase) Then
        Me.ClearSelection()
        Me.CurrentCell = gridRow.Cells(0)
        gridRow.Selected = True
        Exit Sub
      End If
    Next
  End Sub

End Class
