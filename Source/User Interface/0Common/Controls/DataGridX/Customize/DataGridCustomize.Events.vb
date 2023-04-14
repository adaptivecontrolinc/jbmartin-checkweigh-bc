Partial Public Class DataGridCustomize

  Private Sub listSource_DoubleClick(sender As Object, e As EventArgs)
    AddItem()
  End Sub

  Private Sub listTarget_DoubleClick(sender As Object, e As EventArgs)
    RemoveItem()
  End Sub

  Private Sub buttonAdd_Click(sender As Object, e As EventArgs) Handles buttonAdd.Click
    AddItem()
  End Sub

  Private Sub buttonAddAll_Click(sender As Object, e As EventArgs) Handles buttonAddAll.Click
    AddAll()
  End Sub

  Private Sub buttonRemove_Click(sender As Object, e As EventArgs) Handles buttonRemove.Click
    RemoveItem()
  End Sub

  Private Sub buttonRemoveAll_Click(sender As Object, e As EventArgs) Handles buttonRemoveAll.Click
    RemoveAll()
  End Sub

  Private Sub buttonProperties_Click(sender As Object, e As EventArgs) Handles buttonProperties.Click
    DataColumnProperties()
  End Sub

  Private Sub buttonMoveUp_Click(sender As Object, e As EventArgs) Handles buttonMoveUp.Click
    MoveItemUp()
  End Sub

  Private Sub buttonMoveDown_Click(sender As Object, e As EventArgs) Handles buttonMoveDown.Click
    MoveItemDown()
  End Sub


  Private Sub AddItem()
    With listSource.ListBox
      If .SelectedItem Is Nothing Then Exit Sub
      Dim name = TryCast(.SelectedItem, String)
      If name IsNot Nothing Then
        Manager.AddSourceColumn(name)
        FillTargetList()
      End If
    End With
  End Sub

  Private Sub AddAll()
    Manager.AddAllSourceColumns()
    FillTargetList()
  End Sub

  Private Sub RemoveItem()
    With listTarget.ListBox
      If .SelectedItem Is Nothing Then Exit Sub
      Dim name = TryCast(.SelectedItem, String)
      If name IsNot Nothing Then
        Manager.RemoveTargetColumn(name)
        FillTargetList()
      End If
    End With
  End Sub

  Private Sub RemoveAll()
    Manager.RemoveAllTargetColumns()
    FillTargetList()
  End Sub

  Private Sub DataColumnProperties()
    Dim name = TryCast(listTarget.ListBox.SelectedItem, String)
    Dim column = Manager.GetTargetColumn(name)

    If column Is Nothing Then Exit Sub

    Using newform As New FormDataColumnProperties
      newform.Connect(column)
      If newform.ShowDialog(Me) = DialogResult.OK Then
        Manager.ReplaceTargetColumn(column, newform.GridColumn)
      End If
    End Using

  End Sub


  Private Sub MoveItemUp()
    With listTarget.ListBox
      If .SelectedItem Is Nothing Then Exit Sub
      Dim name = TryCast(.SelectedItem, String)
      If name IsNot Nothing Then
        Manager.MoveTargetColumnUp(name)
        FillTargetList()
        SetTargetSelection(name)
      End If
    End With
  End Sub

  Private Sub MoveItemDown()
    With listTarget.ListBox
      If .SelectedItem Is Nothing Then Exit Sub
      Dim name = TryCast(.SelectedItem, String)
      If name IsNot Nothing Then
        Manager.MoveTargetColumnDown(name)
        FillTargetList()
        SetTargetSelection(name)
      End If
    End With
  End Sub

  Private Sub SetTargetSelection(name As String)
    With listTarget.ListBox
      For Each item In .Items
        Dim currentItem = TryCast(item, String)
        If currentItem IsNot Nothing Then
          If currentItem.Equals(name, StringComparison.InvariantCultureIgnoreCase) Then
            .SelectedItem = item
            Exit Sub
          End If
        End If
      Next
    End With
  End Sub

End Class
