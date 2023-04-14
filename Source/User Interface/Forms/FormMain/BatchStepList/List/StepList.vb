Public Class StepList : Inherits UserControl
  Private controlCode As ControlCode

  Property Data As StepListData

  Private dataGrid As DataGridX
  Private dataGridXmlFile As String     ' file path to layout xml file 

  Sub New(controlcode As ControlCode)
    Me.controlCode = controlcode

    Data = New StepListData(controlcode)

    DoubleBuffered = True
    dataGridXmlFile = My.Application.Info.DirectoryPath & "\StepList.xml"
    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    dataGrid = New DataGridX
    With dataGrid
      .XSetupReadOnly()

      .AutoGenerateColumns = False
      .AllowUserToOrderColumns = False
      .AllowUserToResizeColumns = False

      .ColumnHeadersHeight = 32
      .RowTemplate.Height = 32

      .XAutosizeColumn = "StepDescription"

      .BorderStyle = BorderStyle.None
      .Dock = DockStyle.Fill
    End With
    Controls.Add(dataGrid)

    ResumeLayout(False)
  End Sub

  Sub Requery()
    SuspendLayout()

    ' Save scroll position 
    Dim scrollRowIndex As Integer = -1
    If dataGrid.RowCount > 0 Then scrollRowIndex = dataGrid.FirstDisplayedScrollingRowIndex

    ' Save ID of selected row 
    Dim selectedRowID As Integer = dataGrid.SelectedID

    ' Requery data
    Data.Requery()
    dataGrid.DataSource = Data.BatchSteps
    If dataGrid.GridLayout Is Nothing Then LayoutDataGrid()

    ' Restore scroll position
    If scrollRowIndex > 0 Then dataGrid.FirstDisplayedScrollingRowIndex = scrollRowIndex

    ' Restore selected row using the ID
    If selectedRowID > 0 Then dataGrid.SelectedID = selectedRowID

    ResumeLayout(False)
  End Sub


  Function GetSelectedDataRow() As DataRow
    If dataGrid Is Nothing Then Return Nothing
    Return dataGrid.row
  End Function

  Function SetSelectedRow(targetStepNumber As String) As Boolean
    If dataGrid Is Nothing Then Return False
    For i As Integer = 0 To dataGrid.Rows.Count - 1
      Dim stepNumber = dataGrid.Rows(i).Cells("StepNumber").Value.ToString
      If stepNumber.Equals(targetStepNumber, StringComparison.InvariantCultureIgnoreCase) Then
        dataGrid.Rows(i).Selected = True
        dataGrid.CurrentCell = dataGrid.Rows(i).Cells(0)
        Return True
      End If
    Next
  End Function

  Sub Connect(batchRow As DataRow)
    Data.Connect(batchRow)
    dataGrid.DataSource = Data.BatchSteps
    If dataGrid.GridLayout Is Nothing Then LayoutDataGrid()
  End Sub


  Private Sub LayoutDataGrid()
    Try
      ' Get the grid layout from the xml file
      Dim gridLayout = LoadGridLayoutFromXml()

      ' If there is no xml file use the default layout
      If gridLayout Is Nothing Then gridLayout = GetDefaultGridLayout()

      ApplyGridLayout(gridLayout)
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Private Sub ApplyGridLayout(gridLayout As DataGridLayout)
    dataGrid.GridLayout = gridLayout
    Dim gridFormatter As New DataGridLayoutFormatter
    gridFormatter.Run(dataGrid, Data.BatchSteps)
  End Sub

  Private Function GetDefaultGridLayout() As DataGridLayout
    If controlCode.Settings.DisplayUnits = EUnits.Imperial Then
      Return GetDefaultGridLayoutImperial()
    Else
      Return GetDefaultGridLayoutMetric()
    End If
  End Function

  Private Function GetDefaultGridLayoutMetric() As DataGridLayout
    Dim gridLayout = New DataGridLayout
    With gridLayout
      .Name = "StepList"

      .AddTextColumn("StepNumber", 72, "Step")
      .AddTextColumn("StepCode", 72, "Code")
      .AddTextColumn("StepDescription", 128, "Description")

      .AddTextColumn("Amount", 72, "Amount", DataGridLayout.Column.Alignment.Center)
      .AddTextColumn("UnitCode", 72, "Units", DataGridLayout.Column.Alignment.Center)

      .AddNumberColumn("Kilograms", 72, "Kilograms", DataGridLayout.Column.Alignment.Right, "#0.000")
      .AddNumberColumn("DispenseKilograms", 72, "Dispensed", DataGridLayout.Column.Alignment.Right, "#0.000")
      .AddTextColumn("DispenseTime", 128, "Time", DataGridLayout.Column.Alignment.Right)
    End With
    Return gridLayout
  End Function

  Private Function GetDefaultGridLayoutImperial() As DataGridLayout
    Dim gridLayout = New DataGridLayout
    With gridLayout
      .Name = "StepList"

      .AddTextColumn("StepNumber", 72, "Step")
      .AddTextColumn("StepCode", 72, "Code")
      .AddTextColumn("StepDescription", 128, "Description")

      .AddTextColumn("Amount", 72, "Amount", DataGridLayout.Column.Alignment.Center)
      .AddTextColumn("UnitCode", 72, "Units", DataGridLayout.Column.Alignment.Center)

      .AddNumberColumn("Pounds", 72, "Pounds", DataGridLayout.Column.Alignment.Right, "#0.000")
      .AddNumberColumn("DispensePounds", 72, "Dispensed", DataGridLayout.Column.Alignment.Right, "#0.000")
      .AddTextColumn("DispenseTime", 128, "Time", DataGridLayout.Column.Alignment.Right)
    End With
    Return gridLayout
  End Function

  Private Function LoadGridLayoutFromXml() As DataGridLayout
    ' Try to load the layout from the xml file
    Dim gridManager = New DataGridLayoutManager
    Dim gridlayout = gridManager.GetLayoutFromFile(dataGridXmlFile)

    Return gridlayout
  End Function

  Private Sub SaveGridLayoutToXml()
    Dim gridManager = New DataGridLayoutManager
    gridManager.SaveLayoutToFile(dataGrid.GridLayout, dataGridXmlFile)
  End Sub

  Sub CustomizeGrid()
    If dataGrid Is Nothing Then Exit Sub
    If dataGrid.GridLayout Is Nothing Then Exit Sub

    Using newForm As New FormDataGridCustomize
      newForm.Connect(dataGrid)
      If newForm.ShowDialog(Me) = DialogResult.OK Then
        ApplyGridLayout(newForm.GridLayout)
        SaveGridLayoutToXml()
      End If
    End Using
  End Sub

End Class
