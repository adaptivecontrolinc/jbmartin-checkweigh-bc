Public Class DataGridCustomize : Inherits UserControl

  Property Manager As New DataGridCustomizeData

  Sub New()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Sub Connect(dataGrid As DataGridX)
    Manager.Connect(dataGrid)
    LoadData()
  End Sub

  Private Sub LoadData()
    FillSourceList()
    FillTargetList()

    With Manager.GridLayout
      checkboxLockLayout.Checked = .LockLayout
      checkBoxLockSort.Checked = .LockSort
      checkBoxLockFilter.Checked = .LockFilter
    End With
  End Sub

  Private Sub FillSourceList()
    With listSource.ListBox
      ' Clear and add all the columns from the table schema
      .Items.Clear()
      For Each col As DataColumn In Manager.TableSchema.Columns
        .Items.Add(col.ColumnName)
      Next
    End With
  End Sub

  Private Sub FillTargetList()
    With listTarget.ListBox
      ' Clear and add all the columns from the layout
      .Items.Clear()
      For Each col As DataGridLayout.Column In Manager.GridLayout.Columns
        .Items.Add(col.Name)
      Next
    End With
  End Sub

End Class
