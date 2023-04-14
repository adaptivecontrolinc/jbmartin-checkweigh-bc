Partial Public Class DataColumnProperties

  Property GridColumn As DataGridLayout.Column

  Sub Connect(column As DataGridLayout.Column)
    GridColumn = column.CopyColumn
    FillControls()
  End Sub

  Private Sub FillControls()
    SuspendLayout()
    With GridColumn
      If String.IsNullOrEmpty(.HeaderText) Then
        headerText.TextBox.Text = .Name
      Else
        headerText.TextBox.Text = .HeaderText
      End If

      headerWidth.TextBox.Text = .Width.ToString
      headerAlign.ComboBox.Text = .HeaderAlignment.ToString

      columnAlign.ComboBox.Text = .CellAlignment.ToString
      columnStyle.ComboBox.Text = .CellStyle.ToString
      columnFormat.TextBox.Text = .CellFormat
    End With
    ResumeLayout(False)
  End Sub

  Public Sub SaveData()
    Dim tryInteger As Integer
    With GridColumn
      .HeaderText = headerText.TextBox.Text
      If Integer.TryParse(headerWidth.TextBox.Text, tryInteger) Then .Width = tryInteger
      .HeaderAlignment = CType(headerAlign.ComboBox.SelectedIndex, DataGridLayout.Column.Alignment)

      .CellStyle = CType(columnStyle.ComboBox.SelectedIndex, DataGridLayout.Column.Style)
      .CellAlignment = CType(columnAlign.ComboBox.SelectedIndex, DataGridLayout.Column.Alignment)
      .CellFormat = columnFormat.TextBox.Text
    End With
  End Sub

End Class
