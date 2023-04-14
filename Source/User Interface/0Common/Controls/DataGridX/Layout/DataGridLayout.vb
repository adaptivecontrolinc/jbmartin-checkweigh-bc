' Sadly the DataGridViewColumn class is not serializable 
'  so we have To make our own column collection Class to save the grid layout 

Public Class DataGridLayout
  ' Store column layout, sort order and filter
  Property Name As String
  Property Columns As New List(Of Column)
  Property Sort As String
  Property Filter As String
  Property LockLayout As Boolean
  Property LockSort As Boolean
  Property LockFilter As Boolean

  Sub AddCheckBoxColumn(name As String, width As Integer, headerText As String)
    Dim newColumn = New Column With {.Name = name, .Width = width, .HeaderText = headerText}
    With newColumn
      .CellStyle = Column.Style.CheckBox
      .CellAlignment = Column.Alignment.Center

      ' Just in case
      If headerText Is Nothing Then .HeaderText = name
    End With
    Columns.Add(newColumn)
  End Sub

  Sub AddColorColumn(name As String, width As Integer, headerText As String)
    Dim newColumn = New Column With {.Name = name, .Width = width, .HeaderText = headerText}
    With newColumn
      .CellStyle = Column.Style.Color
      .CellAlignment = Column.Alignment.Center

      ' Just in case
      If headerText Is Nothing Then .HeaderText = name
    End With
    Columns.Add(newColumn)
  End Sub

  Sub AddComboBoxColumn(name As String, width As Integer, headerText As String)
    Dim newColumn = New Column With {.Name = name, .Width = width, .HeaderText = headerText}
    With newColumn
      .CellStyle = Column.Style.ComboBox
      .CellAlignment = Column.Alignment.Center

      ' Just in case
      If headerText Is Nothing Then .HeaderText = name
    End With
    Columns.Add(newColumn)
  End Sub

  Sub AddCurrencyColumn(name As String, width As Integer, headerText As String)
    Dim newColumn = New Column With {.Name = name, .Width = width, .HeaderText = headerText}
    With newColumn
      .CellStyle = Column.Style.Currency
      .CellAlignment = Column.Alignment.Right
      .HeaderAlignment = Column.Alignment.Right

      ' Just in case
      If headerText Is Nothing Then .HeaderText = name
    End With
    Columns.Add(newColumn)
  End Sub

  Sub AddDateColumn(name As String, width As Integer, headerText As String)
    Dim newColumn = New Column With {.Name = name, .Width = width, .HeaderText = headerText}
    With newColumn
      .CellStyle = Column.Style.Date
      .CellAlignment = Column.Alignment.Right
      .HeaderAlignment = Column.Alignment.Right

      ' Just in case
      If headerText Is Nothing Then .HeaderText = name
    End With
    Columns.Add(newColumn)
  End Sub

  Sub AddNumberColumn(name As String, width As Integer, headerText As String)
    AddNumberColumn(name, width, headerText, Column.Alignment.Right)
  End Sub

  Sub AddNumberColumn(name As String, width As Integer, headerText As String, alignment As DataGridLayout.Column.Alignment)
    Dim newColumn = New Column With {.Name = name, .Width = width, .HeaderText = headerText}
    With newColumn
      .CellStyle = Column.Style.Number
      .CellAlignment = alignment
      .HeaderAlignment = alignment

      ' Just in case
      If headerText Is Nothing Then .HeaderText = name
    End With
    Columns.Add(newColumn)
  End Sub

  Sub AddNumberColumn(name As String, width As Integer, headerText As String, alignment As DataGridLayout.Column.Alignment, format As String)
    Dim newColumn = New Column With {.Name = name, .Width = width, .HeaderText = headerText}
    With newColumn
      .CellStyle = Column.Style.Number
      .CellAlignment = alignment
      .HeaderAlignment = alignment
      .CellFormat = format

      ' Just in case
      If headerText Is Nothing Then .HeaderText = name
    End With
    Columns.Add(newColumn)
  End Sub

  Sub AddTextColumn(name As String, width As Integer, headerText As String)
    AddTextColumn(name, width, headerText, Column.Alignment.Left)
  End Sub

  Sub AddTextColumn(name As String, width As Integer, headerText As String, alignment As DataGridLayout.Column.Alignment)
    Dim newColumn = New Column With {.Name = name, .Width = width, .HeaderText = headerText}
    With newColumn
      .CellStyle = Column.Style.Text
      .CellAlignment = alignment
      .HeaderAlignment = alignment

      ' Just in case
      If headerText Is Nothing Then .HeaderText = name
    End With
    Columns.Add(newColumn)
  End Sub

  Function CopyLayout() As DataGridLayout
    ' Copy this layout to the target layout
    Dim newGridLayout = New DataGridLayout
    With newGridLayout
      .Sort = Sort
      .Filter = Filter
      .LockLayout = LockLayout
      .LockSort = LockSort
      .LockFilter = LockFilter

      For Each sourceColumn As DataGridLayout.Column In Columns
        Dim newColumn = sourceColumn.CopyColumn
        .Columns.Add(newColumn)
      Next
    End With
    Return newGridLayout
  End Function


  Class Column
    Implements IComparable(Of Column)
    Enum Alignment As Integer
      Left
      Center
      Right
    End Enum
    Enum Style As Integer
      Text
      Number
      [Date]
      Currency
      Color
      CheckBox
      ComboBox   ' TODO this is not fully implemented
    End Enum

    Property Name As String  ' Data Column Name - we map this
    Property Width As Integer = 64
    Property DisplayOrder As Integer = 99

    Property HeaderText As String
    Property HeaderAlignment As Alignment = Alignment.Center

    Property CellAlignment As Alignment = Alignment.Left
    Property CellFormat As String
    Property CellStyle As Style = Style.Text

    Public Sub New()
      'For overload
    End Sub
    Public Sub New(name As String)
      Me.Name = name
    End Sub
    Public Sub New(cellStyle As Style)
      Me.CellStyle = cellStyle
    End Sub

    Function CopyColumn() As DataGridLayout.Column
      ' Copy this column to the target column
      Dim newColumn = New DataGridLayout.Column
      With newColumn
        .Name = Name
        .Width = Width
        .DisplayOrder = DisplayOrder
        .HeaderText = HeaderText
        .HeaderAlignment = HeaderAlignment
        .CellAlignment = CellAlignment
        .CellFormat = CellFormat
        .CellStyle = CellStyle
      End With
      Return newColumn
    End Function

    Public Function CompareTo(compareColumn As Column) As Integer Implements IComparable(Of Column).CompareTo
      Return Me.DisplayOrder.CompareTo(compareColumn.DisplayOrder)
    End Function

  End Class

End Class

