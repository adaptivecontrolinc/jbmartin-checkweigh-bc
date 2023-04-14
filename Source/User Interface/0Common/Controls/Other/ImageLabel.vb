Public Class ImageLabel : Inherits Label

  Private imageWidth As Integer = 24

  Sub New()
    DoubleBuffered = True
  End Sub

  Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
    Dim width = e.ClipRectangle.Width
    Dim height = e.ClipRectangle.Height

    Dim imageRect = New Rectangle(0, 0, imageWidth, height)
    Dim textRect = New Rectangle(imageWidth, 0, width - imageWidth, height)

    DrawLabelImage(e.Graphics, imageRect)
    DrawLabelText(e.Graphics, textRect)
  End Sub

  Private Sub DrawLabelImage(g As Graphics, drawRect As Rectangle)
    If Me.Image Is Nothing Then Exit Sub

    Dim imageHeight = Me.Image.Size.Height
    Dim imageY = (drawRect.Height - imageHeight) \ 2

    g.DrawImage(Me.Image, 0, imageY)
  End Sub

  Private Sub DrawLabelText(g As Graphics, drawRect As Rectangle)
    If Me.Text Is Nothing Then Exit Sub

    Dim stringFormat = New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}

    Using brush As New SolidBrush(Me.ForeColor)
      g.DrawString(Me.Text, Me.Font, brush, drawRect, stringFormat)
    End Using
  End Sub

End Class
