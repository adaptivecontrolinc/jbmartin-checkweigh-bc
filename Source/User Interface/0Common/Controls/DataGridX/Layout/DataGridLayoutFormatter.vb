' Apply a DataGridLayout to a DataGird

Public Class DataGridLayoutFormatter

  Private grid As DataGridX
  Private table As System.Data.DataTable
  Private layout As DataGridLayout

  Sub Run(grid As DataGridX, table As System.Data.DataTable)
    Run(grid, table, grid.GridLayout)
  End Sub

  Sub Run(grid As DataGridX, table As System.Data.DataTable, layout As DataGridLayout)
    If grid Is Nothing OrElse table Is Nothing OrElse layout Is Nothing Then Exit Sub

    Me.grid = grid
    Me.table = table
    Me.layout = layout

    If SetDatasource() Then
      grid.GridLayout = layout
      grid.AutoGenerateColumns = False
      grid.Columns.Clear()
      AddColumns()
    End If
  End Sub

  Private Function SetDatasource() As Boolean
    Try
      Dim dv As System.Data.DataView = table.DefaultView

      With layout
        If Not String.IsNullOrEmpty(.Sort) Then dv.Sort = .Sort
        If Not String.IsNullOrEmpty(.Filter) Then dv.RowFilter = .Filter
      End With

      grid.DataSource = dv

      Return True
    Catch ex As Exception
      ' Ignore errors
    End Try
    Return False
  End Function

  Private Function AddColumns() As Boolean
    Try
      If layout Is Nothing Then Return False

      For Each column As DataGridLayout.Column In layout.Columns
        Select Case column.CellStyle
          Case DataGridLayout.Column.Style.Text : AddTextColumn(column)
          Case DataGridLayout.Column.Style.Number : AddNumberColumn(column)
          Case DataGridLayout.Column.Style.Date : AddDateColumn(column)
          Case DataGridLayout.Column.Style.Currency : AddCurrencyColumn(column)
          Case DataGridLayout.Column.Style.Color : AddColorColumn(column)
          Case DataGridLayout.Column.Style.CheckBox
        End Select
      Next

      Return True
    Catch ex As Exception
      ' Ignore errors
    End Try
    Return False
  End Function

  Private Sub AddTextColumn(column As DataGridLayout.Column)
    grid.AddTextColumn(column.Name, column.Width, column.HeaderText, GetDataGridViewAlignment(column.CellAlignment))
  End Sub

  Private Sub AddNumberColumn(column As DataGridLayout.Column)
    If String.IsNullOrEmpty(column.CellFormat) Then
      grid.AddNumberColumn(column.Name, column.Width, column.HeaderText, GetDataGridViewAlignment(column.CellAlignment))
    Else
      grid.AddNumberColumn(column.Name, column.Width, column.HeaderText, GetDataGridViewAlignment(column.CellAlignment), column.CellFormat)
    End If
  End Sub

  Private Sub AddDateColumn(column As DataGridLayout.Column)
    If String.IsNullOrEmpty(column.CellFormat) Then
      grid.AddDateColumn(column.Name, column.Width, column.HeaderText, GetDataGridViewAlignment(column.CellAlignment))
    Else
      grid.AddDateColumn(column.Name, column.Width, column.HeaderText, GetDataGridViewAlignment(column.CellAlignment), column.CellFormat)
    End If
  End Sub

  Private Sub AddCurrencyColumn(column As DataGridLayout.Column)
    grid.AddCurrencyColumn(column.Name, column.Width, column.HeaderText)
  End Sub

  Private Sub AddColorColumn(column As DataGridLayout.Column)
    grid.AddColorColumn(column.Name, column.Width, column.HeaderText)
  End Sub

  Private Function GetDataGridViewAlignment(alignment As DataGridLayout.Column.Alignment) As DataGridViewContentAlignment
    If alignment = DataGridLayout.Column.Alignment.Left Then Return DataGridViewContentAlignment.MiddleLeft
    If alignment = DataGridLayout.Column.Alignment.Center Then Return DataGridViewContentAlignment.MiddleCenter
    If alignment = DataGridLayout.Column.Alignment.Right Then Return DataGridViewContentAlignment.MiddleRight

    Return DataGridViewContentAlignment.MiddleRight
  End Function

End Class
