Public Class BatchList : Inherits UserControl
  Private ReadOnly controlCode As ControlCode

  Property Data As BatchListData

  Private dataGrid As DataGridX
  Private dataGridXmlFile As String     ' file path to layout xml file 

  Sub New(controlCode As ControlCode)
    Data = New BatchListData(controlCode)
    DoubleBuffered = True
    dataGridXmlFile = My.Application.Info.DirectoryPath & "\BatchList.xml"
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

      .BorderStyle = BorderStyle.None
      .Dock = DockStyle.Fill

      .ScrollBars = ScrollBars.Both

      .XAutosizeColumn = "Dyelot"
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
    dataGrid.DataSource = Data.Batches
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

  Function SetSelectedRow(targetBatch As String) As Boolean
    If dataGrid Is Nothing Then Return False
    For i As Integer = 0 To dataGrid.Rows.Count - 1
      Dim batch = dataGrid.Rows(i).Cells("Dyelot").Value.ToString
      If batch.Equals(targetBatch, StringComparison.InvariantCultureIgnoreCase) Then
        dataGrid.Rows(i).Selected = True
        dataGrid.CurrentCell = dataGrid.Rows(i).Cells(0)
        Return True
      End If
    Next
  End Function

  Function SetSelectedRow(targetBatch As String, targetRedye As Integer) As Boolean
    If dataGrid Is Nothing Then Return False
    For i As Integer = 0 To dataGrid.Rows.Count - 1
      Dim batch = dataGrid.Rows(i).Cells("Dyelot").Value.ToString
      Dim redye = Utilities.Sql.NullToZeroInteger(dataGrid.Rows(i).Cells("Redye"))
      If batch.Equals(targetBatch, StringComparison.InvariantCultureIgnoreCase) AndAlso (redye = targetRedye) Then
        dataGrid.Rows(i).Selected = True
        dataGrid.CurrentCell = dataGrid.Rows(i).Cells(0)
        Return True
      End If
    Next
  End Function

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
    gridFormatter.Run(dataGrid, Data.Batches)
  End Sub

  Private Function GetDefaultGridLayout() As DataGridLayout
    Dim gridLayout = New DataGridLayout
    With gridLayout
      .Name = "BatchList"

      .AddColorColumn("Color", 64, " ")
      .AddTextColumn("Machine", 96, "Machine")
      .AddTextColumn("Dyelot", 128, "Dyelot")
      .AddNumberColumn("ReDye", 64, "ReDye", DataGridLayout.Column.Alignment.Center)
      .AddTextColumn("StartTime", 150, "StartTime")
      .AddNumberColumn("StepCount", 80, "Steps", DataGridLayout.Column.Alignment.Center)
      .AddNumberColumn("DyeCount", 80, "Dyes", DataGridLayout.Column.Alignment.Center)
      .AddNumberColumn("DyeDispenseCount", 80, "Dispensed", DataGridLayout.Column.Alignment.Center)
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
