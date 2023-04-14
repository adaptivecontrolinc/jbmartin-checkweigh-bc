Module Defaults

  Property DefaultFont As Font = New Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Property DefaultFontPlus As Font = New Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

  Property DefaultSmall As Font = New Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Property DefaultFontLarge As Font = New Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

  Property BackColor As Color = Color.FromArgb(236, 243, 246)

  Property BorderColor As Color = Color.FromArgb(190, 214, 224)

  Property LabelForeColor As Color = Color.FromArgb(32, 72, 144)

  Property MessagesBackColor As Color = SystemColors.Info
  Property MessagesForeColor As Color = Color.FromArgb(32, 72, 144)

  Property StepColorOdd As Color = Color.FromArgb(192, 255, 255)
  Property StepColorEven As Color = Color.FromArgb(192, 255, 192)


  Property DefaultMimicBackColor As Color = Color.FromArgb(192, 255, 255)
  Property DefaultMimicSize As Size = New Size(1280, 1024 - 92)             ' Expert mode no scroll bar
  Property DefaultMimicPageSize As Size = New Size(1280, 1024 - 92 - 48)    ' Expert mode no scroll bar + page buttons

  Property VerticalSpacing As Integer = -2
  Property VerticalSpacingGroup As Integer = 11

  Property Comparison As StringComparison = StringComparison.InvariantCultureIgnoreCase

  Property Culture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.InvariantCulture


  Function TimeStamp() As String
    Return Date.Now.ToString("HHmmss")
  End Function

  Function TimeStampUtc() As String
    Return Date.UtcNow.ToString("HHmmss")
  End Function

  Function DateAndTimeStamp() As String
    Return Date.Now.ToString("yyMMddHHmmss")
  End Function

  Function DateAndTimeStampUtc() As String
    Return Date.UtcNow.ToString("yyMMddHHmmss")
  End Function

End Module