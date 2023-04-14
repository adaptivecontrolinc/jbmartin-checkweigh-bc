Partial Public Class DataGridX

  Public Sub XSetupReadOnly()
    AllowUserToAddRows = False
    AllowUserToDeleteRows = False
    AllowUserToOrderColumns = False
    AllowUserToResizeColumns = False
    AllowUserToResizeRows = False
    AutoGenerateColumns = False
    AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
    BackgroundColor = Color.Silver
    BorderStyle = System.Windows.Forms.BorderStyle.None
    CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single
    ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
    ColumnHeadersHeight = 24
    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
    EditMode = DataGridViewEditMode.EditProgrammatically
    MultiSelect = False
    [ReadOnly] = True
    RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
    RowHeadersWidth = 28
    RowTemplate.Height = 20
    ScrollBars = ScrollBars.Vertical
    SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
  End Sub

  Public Sub XSetupEdit()
    AllowUserToAddRows = False
    AllowUserToDeleteRows = True
    AllowUserToOrderColumns = False
    AllowUserToResizeColumns = False
    AllowUserToResizeRows = False
    AutoGenerateColumns = False
    AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
    BackgroundColor = Color.Silver
    BorderStyle = System.Windows.Forms.BorderStyle.None
    CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single
    ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
    ColumnHeadersHeight = 24
    ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
    EditMode = DataGridViewEditMode.EditOnEnter
    MultiSelect = False
    [ReadOnly] = False
    RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
    RowHeadersWidth = 28
    RowTemplate.Height = 20
    ScrollBars = ScrollBars.Vertical
    SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
  End Sub

  'Setup alternating color
  Public Sub XSetupAlternatingColor()
    Dim AlternatingRowColor As New System.Drawing.Color
    AlternatingRowColor = System.Drawing.Color.FromArgb(255, 224, 255, 255)
    AlternatingRowsDefaultCellStyle.BackColor = AlternatingRowColor
  End Sub

  'Setup and set the alternating color
  Public Sub XSetupAlternatingColor(ByVal AlternatingRowColor As System.Drawing.Color)
    AlternatingRowsDefaultCellStyle.BackColor = AlternatingRowColor
  End Sub

  'Set selection bar to be standard colors so we don't see it
  Public Sub XHideSelectionBar()
    SelectionMode = DataGridViewSelectionMode.CellSelect
    DefaultCellStyle.SelectionBackColor = DefaultCellStyle.BackColor
    DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor
  End Sub

  'Set selection bar to be standard colors so we don't see it
  Public Sub XHideSelectionBar(backColor As System.Drawing.Color)
    SelectionMode = DataGridViewSelectionMode.CellSelect
    DefaultCellStyle.BackColor = backColor
    DefaultCellStyle.SelectionBackColor = DefaultCellStyle.BackColor
    DefaultCellStyle.SelectionForeColor = DefaultCellStyle.ForeColor
  End Sub

  Public Sub XSetBackColor(backColor As System.Drawing.Color)
    BackgroundColor = backColor
    DefaultCellStyle.BackColor = backColor
  End Sub

  Public Sub XSetForeColor(foreColor As System.Drawing.Color)
    foreColor = foreColor
    DefaultCellStyle.ForeColor = foreColor
  End Sub

  Public Sub XDisableUserSorting()
    For Each col As DataGridViewColumn In Me.Columns
      col.SortMode = DataGridViewColumnSortMode.Programmatic
    Next
  End Sub

  Private Sub XConvertDatesToLocalTime()
    If Table Is Nothing OrElse Table.Rows.Count <= 0 Then Exit Sub
    Try
      For Each row As System.Data.DataRow In Table.Rows
        For Each column As System.Data.DataColumn In Table.Columns
          If column.DataType Is GetType(Date) Then
            If Not row.IsNull(column) Then row(column) = DirectCast(row(column), Date).ToLocalTime
          End If
        Next
      Next
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Private xAutosizeColumn_ As String
  Public Property XAutosizeColumn() As String
    Get
      Return xAutosizeColumn_
    End Get
    Set(ByVal value As String)
      For Each column As System.Windows.Forms.DataGridViewColumn In Me.Columns
        If column.Name.ToLower = value.ToLower Then
          xAutosizeColumn_ = column.Name
          column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
      Next
    End Set
  End Property

  Private xBorderColor_ As System.Drawing.Color = Color.DarkGray
  Public Property XBorderColor() As System.Drawing.Color
    Get
      Return xBorderColor_
    End Get
    Set(ByVal value As System.Drawing.Color)
      xBorderColor_ = value
    End Set
  End Property

End Class
