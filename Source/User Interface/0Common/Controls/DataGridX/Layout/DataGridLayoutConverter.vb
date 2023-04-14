' Save the layout of a DataGrid to a DataGridLayout so we can save it

Public Class DataGridLayoutConverter

  Private grid As DataGridX
  Private gridLayout As DataGridLayout

  Function Create(grid As DataGridX) As DataGridLayout
    Try
      If grid Is Nothing OrElse grid.DataSource Is Nothing Then Return Nothing

      Me.grid = grid
      Me.gridLayout = New DataGridLayout

      If Run(grid, gridLayout) Then Return gridLayout
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return Nothing
  End Function


  Function Run(grid As DataGridX, gridLayout As DataGridLayout) As Boolean
    Try
      If grid Is Nothing OrElse gridLayout Is Nothing Then Return False

      Me.grid = grid
      Me.gridLayout = gridLayout

      ' Get sort order and filter
      Dim gridDataView = DirectCast(grid.DataSource, DataView)
      If Not String.IsNullOrEmpty(gridDataView.Sort) Then gridLayout.Sort = gridDataView.Sort
      If Not String.IsNullOrEmpty(gridDataView.RowFilter) Then gridLayout.Filter = gridDataView.RowFilter

      ' Add all the columns in the data grid column collection
      AddColumns()

      Return True
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return False
  End Function

  Private Sub AddColumns()
    If gridLayout Is Nothing Then Exit Sub

    ' Clear existing column layout
    gridLayout.Columns.Clear()

    For Each column As DataGridViewColumn In grid.Columns
      Select Case GetDataGridLayoutColumnStyle(column)
        Case DataGridLayout.Column.Style.CheckBox : AddCheckBoxColumn(column)
        Case DataGridLayout.Column.Style.Color : AddColorColumn(column)
        Case DataGridLayout.Column.Style.ComboBox : AddComboBoxColumn(column)
        Case DataGridLayout.Column.Style.Currency : AddCurrencyColumn(column)
        Case DataGridLayout.Column.Style.Date : AddDateColumn(column)
        Case DataGridLayout.Column.Style.Number : AddNumberColumn(column)
        Case DataGridLayout.Column.Style.Text : AddTextBoxColumn(column)
      End Select
    Next

    ' Sort the layout columns by display order (DislayOrder = DisplayIndex on the data grid column)
    gridLayout.Columns.Sort()
  End Sub

  Private Sub AddCheckBoxColumn(gridColumn As DataGridViewColumn)
    Dim layoutColumn = New DataGridLayout.Column(DataGridLayout.Column.Style.CheckBox)
    SetLayoutColumnProperties(gridColumn, layoutColumn)

    gridLayout.Columns.Add(layoutColumn)
  End Sub

  Private Sub AddColorColumn(gridColumn As DataGridViewColumn)
    Dim layoutColumn = New DataGridLayout.Column(DataGridLayout.Column.Style.Color)
    SetLayoutColumnProperties(gridColumn, layoutColumn)

    gridLayout.Columns.Add(layoutColumn)
  End Sub

  Private Sub AddComboBoxColumn(gridColumn As DataGridViewColumn)
    Dim layoutColumn = New DataGridLayout.Column(DataGridLayout.Column.Style.ComboBox)
    SetLayoutColumnProperties(gridColumn, layoutColumn)

    gridLayout.Columns.Add(layoutColumn)
  End Sub

  Private Sub AddCurrencyColumn(gridColumn As DataGridViewColumn)
    Dim layoutColumn = New DataGridLayout.Column(DataGridLayout.Column.Style.Currency)
    SetLayoutColumnProperties(gridColumn, layoutColumn)

    gridLayout.Columns.Add(layoutColumn)
  End Sub

  Private Sub AddDateColumn(gridColumn As DataGridViewColumn)
    Dim layoutColumn = New DataGridLayout.Column(DataGridLayout.Column.Style.Date)
    SetLayoutColumnProperties(gridColumn, layoutColumn)

    gridLayout.Columns.Add(layoutColumn)
  End Sub

  Private Sub AddNumberColumn(gridColumn As DataGridViewColumn)
    Dim layoutColumn = New DataGridLayout.Column(DataGridLayout.Column.Style.Number)
    SetLayoutColumnProperties(gridColumn, layoutColumn)

    gridLayout.Columns.Add(layoutColumn)
  End Sub

  Private Sub AddTextBoxColumn(gridColumn As DataGridViewColumn)
    Dim layoutColumn = New DataGridLayout.Column(DataGridLayout.Column.Style.Text)
    SetLayoutColumnProperties(gridColumn, layoutColumn)

    gridLayout.Columns.Add(layoutColumn)
  End Sub

  Private Sub SetLayoutColumnProperties(gridColumn As DataGridViewColumn, layoutColumn As DataGridLayout.Column)
    With layoutColumn
      .Name = gridColumn.Name
      .Width = gridColumn.Width
      .HeaderText = gridColumn.HeaderText
      .DisplayOrder = gridColumn.DisplayIndex

      .CellAlignment = GetDataGridLayoutAlignment(gridColumn.DefaultCellStyle.Alignment)
      .HeaderAlignment = GetDataGridLayoutAlignment(gridColumn.DefaultCellStyle.Alignment)

      If Not String.IsNullOrEmpty(gridColumn.DefaultCellStyle.Format) Then .CellFormat = gridColumn.DefaultCellStyle.Format
    End With
  End Sub


  Private Sub SetDisplayOrder()
    Dim index As Integer = 0
    For Each column As DataGridLayout.Column In gridLayout.Columns
      column.DisplayOrder = index
      index += 1
    Next
  End Sub

  ' Get column style from the DataGridViewColumn tag property
  Private Function GetDataGridLayoutColumnStyle(column As DataGridViewColumn) As DataGridLayout.Column.Style
    If column.Tag Is Nothing Then Return DataGridLayout.Column.Style.Text

    Select Case CStr(column.Tag).ToLower
      Case "checkbox" : Return DataGridLayout.Column.Style.CheckBox
      Case "color" : Return DataGridLayout.Column.Style.Color
      Case "combobox" : Return DataGridLayout.Column.Style.ComboBox
      Case "currency" : Return DataGridLayout.Column.Style.Currency
      Case "date" : Return DataGridLayout.Column.Style.Date
      Case "number" : Return DataGridLayout.Column.Style.Number
      Case "text" : Return DataGridLayout.Column.Style.Text
    End Select

    ' Default
    Return DataGridLayout.Column.Style.Text
  End Function


  ' Convert DataGridView alignment to DataGridLayout alignment
  Private Function GetDataGridLayoutAlignment(alignment As DataGridViewContentAlignment) As DataGridLayout.Column.Alignment
    Select Case alignment
      Case DataGridViewContentAlignment.MiddleLeft : Return DataGridLayout.Column.Alignment.Left
      Case DataGridViewContentAlignment.MiddleCenter : Return DataGridLayout.Column.Alignment.Center
      Case DataGridViewContentAlignment.MiddleRight : Return DataGridLayout.Column.Alignment.Right
    End Select

    Return DataGridLayout.Column.Alignment.Right
  End Function

End Class
