' Custom color column for the data grid
'   Displays a color rectangle based on an integer (RGB) value

Partial Public Class DataGridX

  Class XColumnColor
    Inherits DataGridViewColumn

    Public Sub New()
      MyBase.New(New XCellColor())
      Me.ReadOnly = True
    End Sub

    Overrides Property CellTemplate() As DataGridViewCell
      Get
        Return MyBase.CellTemplate
      End Get
      Set(ByVal value As DataGridViewCell)
        ' Ensure that the cell used for the template is a ColorCell.
        If (value IsNot Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(XCellColor)) Then
          Throw New InvalidCastException("Must be a ColorCell")
        End If
        MyBase.CellTemplate = value

      End Set
    End Property
  End Class

  Class XCellColor
    Inherits DataGridViewCell

    Protected Overrides Sub Paint(ByVal graphics As System.Drawing.Graphics, ByVal clipBounds As System.Drawing.Rectangle, ByVal cellBounds As System.Drawing.Rectangle, ByVal rowIndex As Integer, ByVal cellState As System.Windows.Forms.DataGridViewElementStates, ByVal value As Object, ByVal formattedValue As Object, ByVal errorText As String, ByVal cellStyle As System.Windows.Forms.DataGridViewCellStyle, ByVal advancedBorderStyle As System.Windows.Forms.DataGridViewAdvancedBorderStyle, ByVal paintParts As System.Windows.Forms.DataGridViewPaintParts)
      Try
        'Fill in with a white background
        Using pBack As New System.Drawing.Pen(Color.White, 1)
          graphics.FillRectangle(pBack.Brush, cellBounds)
        End Using

        'Set fill color
        ' This could be a .Net ARGB or an old VB6 circa OLE color
        Dim fillColor = Utilities.Conversions.OleRgbOrArgbToColor(CType(value, Integer))
        Using pFill As New System.Drawing.Pen(fillColor, 1)
          'Dim x As Integer = cellBounds.Location.X + ((cellBounds.Width - 16) \ 2) - 1
          'Dim y As Integer = cellBounds.Location.Y + ((cellBounds.Height - 16) \ 2)

          Dim x As Integer = cellBounds.Location.X + 4
          Dim y As Integer = cellBounds.Location.Y + 4
          Dim width As Integer = cellBounds.Width - 8 - 1
          Dim height As Integer = cellBounds.Height - 8 - 1

          Dim rColor As New System.Drawing.Rectangle(x, y, width, height)
          graphics.FillRectangle(pFill.Brush, rColor)
        End Using

        'Finally draw grid lines
        Using pBorder As New System.Drawing.Pen(Me.DataGridView.GridColor, 1)
          Dim pt1 As New System.Drawing.Point(cellBounds.Location.X, cellBounds.Location.Y + cellBounds.Height - 1)  'bottom left
          Dim pt2 As New System.Drawing.Point(pt1.X + cellBounds.Width - 1, pt1.Y)                                   'bottom right
          Dim pt3 As New System.Drawing.Point(pt2.X, cellBounds.Location.Y)                                          'top right
          graphics.DrawLine(pBorder, pt1, pt2)
          graphics.DrawLine(pBorder, pt2, pt3)
        End Using
      Catch ex As Exception
        ' Ignore errors
      End Try
    End Sub

    ' Make sure we always return an integer, stops "formatted cell has wrong type" errors with DBNull values
    Protected Overrides Function GetValue(rowIndex As Integer) As Object
      Dim value = MyBase.GetValue(rowIndex)
      If TypeOf value Is Integer Then Return value
      Return Color.White.ToArgb
    End Function
  End Class

End Class
