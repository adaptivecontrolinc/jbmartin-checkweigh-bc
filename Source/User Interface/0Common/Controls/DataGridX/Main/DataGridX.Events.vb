Partial Public Class DataGridX

  Public Event RowDoubleClick(ByVal sender As Object, ByVal row As System.Data.DataRow)

  Private Sub ControlDataGridViewX_DataBindingComplete(sender As Object, e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles Me.DataBindingComplete
    ' Stops first row being selected by default
    'Me.ClearSelection()
  End Sub

  Private Sub ControlDataGridViewX_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Me.CellDoubleClick
    ' Make sure we have some data
    If Me.DataSource Is Nothing Then Exit Sub

    ' It has to be data table or a dataview 'cos we're gonna pass a data row
    If (Not TypeOf Me.DataSource Is DataTable) AndAlso (Not TypeOf Me.DataSource Is DataView) Then Exit Sub

    ' Make sure the row index is valid
    If e.RowIndex < 0 Or e.RowIndex >= Me.Rows.Count Then Exit Sub

    ' The DataGrid event returns a row index - we're gonna get the underlying data row
    If Me.Rows(e.RowIndex) IsNot Nothing Then
      'Get the underlying DataRowView from the row index 
      Dim drv As System.Data.DataRowView = DirectCast(Me.Rows(e.RowIndex).DataBoundItem, System.Data.DataRowView)
      RaiseEventRowDoubleClick(drv.Row)
    End If
  End Sub

  Private Sub ControlDataGridViewX_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    ' Do this first to eliminate unnecessary tests
    If e.KeyCode <> Keys.Enter Then Exit Sub

    ' Make sure we data
    If Me.DataSource Is Nothing Then Exit Sub

    ' It has to be data table or a data view 'cos we're gonna pass a data row
    If (Not TypeOf Me.DataSource Is DataTable) AndAlso (Not TypeOf Me.DataSource Is DataView) Then Exit Sub

    'Catch enter key
    If e.KeyCode = Keys.Enter Then
      If Me.SelectedRows IsNot Nothing Then
        If Me.SelectedRows.Count >= 1 Then
          'Get the underlying DataRowView from the selected row
          Dim drv As System.Data.DataRowView = DirectCast(Me.SelectedRows(0).DataBoundItem, System.Data.DataRowView)
          RaiseEventRowDoubleClick(drv.Row)
        End If
      End If
      'e.Handled = True
    End If
  End Sub


  Private Sub RaiseEventRowDoubleClick(row As System.Data.DataRow)
    ' Just a little wrapper so we can easily redirect this if necessary
    RaiseEvent RowDoubleClick(Me, row)
  End Sub


  ' Stop data formatting errors 
  Private Sub ControlDataGridViewX_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Me.DataError
    Utilities.Log.LogError(e.Exception)
    e.Cancel = True
  End Sub

End Class
