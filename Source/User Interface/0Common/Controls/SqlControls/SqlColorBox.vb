Public Class SqlColorBox : Inherits UserControl : Implements ISqlControl
  ' Colors in hex are AARRGGBB
  '   Add FF000000 to the RGB value to get a fully opaque ARGB value

  Public Label As New Label
  Public WithEvents LabelColor As New Label

  ' Set to true if we want to enable auto fill
  Property SqlAutoFill As Boolean = True Implements ISqlControl.SqlAutoFill
  ' Column name if mapping to a table
  Property SqlColumnName As String Implements ISqlControl.SqlColumnName
  ' This is always an integer in this case
  Property SqlValueType As ESqlValueType = ESqlValueType.Integer Implements ISqlControl.SqlValueType

  ' This has to be as Object so it can hold DBNull.Value
  '   Note SqlValue will not return DBNull.Value so it will always set the mapped column to a value
  Property SqlValue() As Object Implements ISqlControl.SqlValue
    Get
      Return Value
    End Get
    Set(value As Object)
      Me.Value = Utilities.Sql.NullToZeroInteger(value)
    End Set
  End Property

  Public Property Value As Integer
    Get
      Return LabelColor.BackColor.ToArgb
    End Get
    Set(value As Integer)
      ' Just in case we have some old OLE RGB colors hangin around
      LabelColor.BackColor = Utilities.Conversions.OleRgbOrArgbToColor(value)
    End Set
  End Property

  Public Property ValueOleRGB As Integer
    Get
      Return Utilities.Conversions.ColorToOleRgb(Value)
    End Get
    Set(value As Integer)

    End Set
  End Property

  Public Sub New()
    NewBase()
  End Sub

  Public Sub New(sqlColumnName As String)
    Me.SqlColumnName = sqlColumnName
    NewBase()
  End Sub

  Public Sub New(sqlColumnName As String, useRGB As Boolean)
    Me.SqlColumnName = sqlColumnName
    NewBase()
  End Sub

  Sub NewBase()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent
    Label.ForeColor = LabelForeColor

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Private Sub AddControls()
    SuspendLayout()
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
    Me.Height = 80 'default height

    With Label
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .AutoSize = False
      .BackColor = Color.Transparent
      .Location = New Point(-3, 0)
      .Size = New Size(Me.Width + 3, 20)
      .Text = "Display Color..."
      .TextAlign = ContentAlignment.MiddleLeft
    End With
    Me.Controls.Add(Label)

    With LabelColor
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom
      .AutoSize = False
      .BackColor = Color.White
      .Location = New Point(0, Label.Height)
      .Size = New Size(Me.Width, Me.Height - Label.Height)
    End With
    Me.Controls.Add(LabelColor)

    ResumeLayout(False)
  End Sub

  Private Sub OpenColorDialog()
    'Open the ColorDialog component and set the color selected by user
    With New ColorDialog
      .FullOpen = True
      .ShowHelp = True
      .SolidColorOnly = True
      If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        LabelColor.BackColor = .Color
      End If
    End With
  End Sub

  Private Sub Label_Click(sender As Object, e As EventArgs) Handles LabelColor.Click
    OpenColorDialog()
  End Sub

End Class
