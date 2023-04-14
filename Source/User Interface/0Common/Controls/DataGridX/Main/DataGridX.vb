Class DataGridX : Inherits Windows.Forms.DataGridView
  ' Extend the standard WinForms DataGridView control to simplify setup, configuration and to add some custom columns (color / check box)

  Property GridLayout As DataGridLayout

  Sub New()
    DoubleBuffered = True
  End Sub

  Property Table As System.Data.DataTable
    Get
      'Usually a data table or a data view
      If TypeOf Me.DataSource Is System.Data.DataTable Then Return DirectCast(Me.DataSource, System.Data.DataTable)
      If TypeOf Me.DataSource Is System.Data.DataView Then Return DirectCast(Me.DataSource, System.Data.DataView).Table
      Return Nothing
    End Get
    Set(value As System.Data.DataTable)
      Me.DataSource = value
    End Set
  End Property

  Property row() As DataRow
    Get
      Return GetSelectedDataRow()
    End Get
    Set(value As DataRow)
      SetSelectedDataRow(value)
    End Set
  End Property

  Property SelectedID As Integer
    Get
      Return GetSelectedID()
    End Get
    Set(value As Integer)
      SetSelectedID(value)
    End Set
  End Property

  Private Function GetSelectedDataRow() As DataRow
    Try
      If Me.DataSource Is Nothing OrElse Me.RowCount <= 0 Then Return Nothing
      If Me.CurrentRow Is Nothing Then Return Nothing

      Dim gridDataRowView = TryCast(Me.CurrentRow.DataBoundItem, DataRowView)
      If gridDataRowView IsNot Nothing Then Return gridDataRowView.Row

    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return Nothing
  End Function

  Private Sub SetSelectedDataRow(targetRow As DataRow)
    Try
      ' Set datagrid selection to targetRow if found
      If targetRow Is Nothing OrElse Me.Rows.Count <= 0 Then Exit Sub

      For i As Integer = 0 To Me.Rows.Count - 1
        Dim gridDataRowView = TryCast(Me.Rows(i).DataBoundItem, DataRowView)
        If gridDataRowView IsNot Nothing Then
          If gridDataRowView.Row Is targetRow Then
            Me.ClearSelection()
            Me.CurrentCell = Me.Rows(i).Cells(0)
            Me.Rows(i).Selected = True
            Exit For
          End If
        End If
      Next
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Private Function GetSelectedID() As Integer
    Try
      Dim selectedRow = GetSelectedDataRow()
      If selectedRow Is Nothing Then Return -1
      If Not selectedRow.Table.Columns.Contains("ID") Then Return -1

      Return DirectCast(selectedRow("ID"), Integer)
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return -1
  End Function

  Private Sub SetSelectedID(targetID As Integer)
    Try
      ' Set datagrid selection to targetID if found
      If targetID <= 0 OrElse Me.Rows.Count <= 0 Then Exit Sub
      If Table Is Nothing OrElse Not Table.Columns.Contains("ID") Then Exit Sub

      For i As Integer = 0 To Me.Rows.Count - 1
        Dim gridDataRowView = TryCast(Me.Rows(i).DataBoundItem, DataRowView)
        If gridDataRowView IsNot Nothing Then
          Dim id As Integer = DirectCast(gridDataRowView.Row("ID"), Integer)
          If id = targetID Then
            Me.ClearSelection()
            Me.CurrentCell = Me.Rows(i).Cells(0)
            Me.Rows(i).Selected = True
            Exit For
          End If
        End If
      Next
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

End Class

