Namespace Utilities

  Public NotInheritable Class Draw

    Public Shared RgbBlack As Integer = 0
    Public Shared RgbWhite As Integer = 16777215

    Public Shared Function ConvertRgbToColor(ByVal rgb As Integer) As System.Drawing.Color

      Dim red As Integer = rgb And 255
      Dim green As Integer = (rgb \ 256) And 255
      Dim blue As Integer = (rgb \ 65536) And 255

      Return System.Drawing.Color.FromArgb(red, green, blue)
    End Function

    Public Shared Function ConvertColorToRgb(ByVal color As System.Drawing.Color) As Integer

      Dim red As Integer = color.R                 ' 
      Dim green As Integer = color.G * 256         ' 2^8
      Dim blue As Integer = color.B * 65536        ' 2^16

      Return red + green + blue
    End Function

    Public Shared Sub DrawRoundedRectangle(ByVal g As Graphics, ByVal r As Rectangle, ByVal p As Pen)

      Dim ArcDiameter As Integer = 7
      Dim ArcPen As New System.Drawing.Pen(p.Color, 1)
      Dim ArcRectangle As New Rectangle(r.X, r.Y, ArcDiameter, ArcDiameter)

      'top left Arc
      ArcRectangle.Location = New System.Drawing.Point(r.Left, r.Top)
      g.DrawArc(ArcPen, ArcRectangle, 180, 90)

      'bottom left arc
      ArcRectangle.Location = New System.Drawing.Point(r.Left, r.Bottom - ArcDiameter)
      g.DrawArc(ArcPen, ArcRectangle, 90, 90)

      'top right arc
      ArcRectangle.Location = New System.Drawing.Point(r.Right - ArcDiameter, r.Top)
      ArcRectangle.X = r.Right - ArcDiameter
      g.DrawArc(ArcPen, ArcRectangle, 270, 90)

      'bottom right arc
      ArcRectangle.Location = New System.Drawing.Point(r.Right - ArcDiameter, r.Bottom - ArcDiameter)
      g.DrawArc(ArcPen, ArcRectangle, 0, 90)

      'top line
      g.DrawLine(p, r.Left + CInt(ArcDiameter / 2), r.Top, CInt(r.Right - (ArcDiameter / 2)), r.Top)

      'bottom line
      g.DrawLine(p, r.Left + CInt(ArcDiameter / 2), r.Bottom, CInt(r.Right - (ArcDiameter / 2)), r.Bottom)

      'left line
      g.DrawLine(p, r.Left, r.Top + CInt(ArcDiameter / 2), r.Left, r.Bottom - CInt(ArcDiameter / 2))

      'right line
      g.DrawLine(p, r.Right, r.Top + CInt(ArcDiameter / 2), r.Right, r.Bottom - CInt(ArcDiameter / 2))

    End Sub

    Public Shared Sub DrawFilledRoundedRectangle(ByVal g As Graphics, ByVal r As Rectangle, ByVal fillColor As Drawing.Color)

      Dim ArcDiameter As Integer = 7
      Dim ArcPen As New Pen(fillColor, 1)
      Dim ArcRectangle As New Rectangle(r.X, r.Y, ArcDiameter, ArcDiameter)

      'top left circle
      ArcRectangle.Location = New System.Drawing.Point(r.Left, r.Top)
      g.DrawArc(ArcPen, ArcRectangle, 180, 90)
      g.FillEllipse(ArcPen.Brush, ArcRectangle)

      'bottom left circle
      ArcRectangle.Location = New System.Drawing.Point(r.Left, r.Bottom - ArcDiameter)
      g.DrawArc(ArcPen, ArcRectangle, 90, 90)
      g.FillEllipse(ArcPen.Brush, ArcRectangle)

      'top right circle
      ArcRectangle.Location = New System.Drawing.Point(r.Right - ArcDiameter, r.Top)
      ArcRectangle.X = r.Right - ArcDiameter
      g.DrawArc(ArcPen, ArcRectangle, 270, 90)
      g.FillEllipse(ArcPen.Brush, ArcRectangle)

      'bottom right circle
      ArcRectangle.Location = New System.Drawing.Point(r.Right - ArcDiameter, r.Bottom - ArcDiameter)
      g.DrawArc(ArcPen, ArcRectangle, 0, 90)
      g.FillEllipse(ArcPen.Brush, ArcRectangle)


      'top rectangle
      Dim topRectangle As New Rectangle(r.Left + CInt(ArcDiameter / 2), r.Top, r.Width - ArcDiameter, ArcDiameter)
      g.FillRectangle(ArcPen.Brush, topRectangle)

      'mid rectangle
      Dim midRectangle As New Rectangle(r.Left, r.Top + CInt(ArcDiameter / 2), r.Width, r.Height - ArcDiameter)
      g.FillRectangle(ArcPen.Brush, midRectangle)

      'bottom rectangle
      Dim bottomRectangle As New Rectangle(r.Left + CInt(ArcDiameter / 2), r.Bottom - ArcDiameter, r.Width - ArcDiameter, ArcDiameter)
      g.FillRectangle(ArcPen.Brush, bottomRectangle)

      'top line
      g.DrawLine(ArcPen, r.Left + CInt(ArcDiameter / 2), r.Top, CInt(r.Right - (ArcDiameter / 2)), r.Top)

      'bottom line
      g.DrawLine(ArcPen, r.Left + CInt(ArcDiameter / 2), r.Bottom, CInt(r.Right - (ArcDiameter / 2)), r.Bottom)

      'left line
      g.DrawLine(ArcPen, r.Left, r.Top + CInt(ArcDiameter / 2), r.Left, r.Bottom - CInt(ArcDiameter / 2))

      'right line
      g.DrawLine(ArcPen, r.Right, r.Top + CInt(ArcDiameter / 2), r.Right, r.Bottom - CInt(ArcDiameter / 2))

    End Sub

    Public Shared Sub DrawClippedRectangle(ByVal g As Graphics, ByVal r As Rectangle, ByVal p As Pen)

      Dim cornerSize As Integer = 2

      'top left corner
      g.DrawLine(p, r.Left, r.Top + cornerSize, r.Left + cornerSize, r.Top)

      'bottom left corner
      g.DrawLine(p, r.Left, r.Bottom - cornerSize, r.Left + cornerSize, r.Bottom)

      'top right corner
      g.DrawLine(p, r.Right - cornerSize, r.Top, r.Right, r.Top + cornerSize)

      'bottom right corner
      g.DrawLine(p, r.Right - cornerSize, r.Bottom, r.Right, r.Bottom - cornerSize)

      'top line
      g.DrawLine(p, r.Left + cornerSize, r.Top, r.Right - cornerSize, r.Top)

      'bottom line
      g.DrawLine(p, r.Left + cornerSize, r.Bottom, r.Right - cornerSize, r.Bottom)

      'left line
      g.DrawLine(p, r.Left, r.Top + cornerSize, r.Left, r.Bottom - cornerSize)

      'right line
      g.DrawLine(p, r.Right, r.Top + cornerSize, r.Right, r.Bottom - cornerSize)

    End Sub

    Public Shared Sub DrawFilledClippedRectangle(ByVal g As Graphics, ByVal r As Rectangle, ByVal p As Pen)

      Dim cornerSize As Integer = 2

      'top left corner
      'Dim topleftRectangle As New Rectangle(r.Left, r.Top + cornerSize, r.Left + cornerSize, r.Top, cornerSize)
      'g.FillRectangle(p.Brush, topleftRectangle)
      g.DrawLine(p, r.Left, r.Top + cornerSize, r.Left + cornerSize, r.Top)

      'bottom left corner
      g.DrawLine(p, r.Left, r.Bottom - cornerSize, r.Left + cornerSize, r.Bottom)

      'top right corner
      g.DrawLine(p, r.Right - cornerSize, r.Top, r.Right, r.Top + cornerSize)

      'bottom right corner
      g.DrawLine(p, r.Right - cornerSize, r.Bottom, r.Right, r.Bottom - cornerSize)


      'top rectangle
      Dim topRectangle As New Rectangle(r.Left + cornerSize, r.Top, r.Width - (cornerSize * 2), cornerSize)
      g.FillRectangle(p.Brush, topRectangle)

      'mid rectangle
      Dim midRectangle As New Rectangle(r.Left, r.Top + cornerSize, r.Width, r.Height - (cornerSize * 2))
      g.FillRectangle(p.Brush, midRectangle)

      'bottom rectangle
      Dim bottomRectangle As New Rectangle(r.Left + cornerSize, r.Bottom - cornerSize, r.Width - (cornerSize * 2), cornerSize)
      g.FillRectangle(p.Brush, bottomRectangle)

    End Sub

    Public Shared Sub SetDoubleBuffered(ByVal control As Control)

      GetType(Control).InvokeMember("DoubleBuffered", Reflection.BindingFlags.SetProperty Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic, Nothing, control, New Object() {True})

    End Sub

  End Class

End Namespace
