Public Class LabelColorBox : Inherits UserControl

  Public Label As New Label
  Public WithEvents Button As New Button

  Private buttonSize As New Size(75, 25)

  Property SqlColumnName As String  ' column name if mapping to a table

  Public Property Value As Integer
    Get
      Return Utilities.Conversions.ColorToOleRgb(Label.BackColor)
    End Get
    Set(value As Integer)
      Label.BackColor = Utilities.Conversions.OleRgbToColor(value)
    End Set
  End Property

  Public Sub New()
    NewBase()
  End Sub

  Public Sub New(sqlColumnName As String)
    Me.SqlColumnName = sqlColumnName
    NewBase()
  End Sub

  Sub NewBase()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent

    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    With Label
      .AutoSize = False
      .BackColor = Color.White
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom
      .Location = New Point(0, 0)
      .Size = New Size(Me.Width, Me.Height - buttonSize.Height - 4)
    End With

    Button = New Button
    With Button
      .Anchor = AnchorStyles.Left Or AnchorStyles.Bottom

      .Image = My.Resources.ChooseColor16x16
      .ImageAlign = ContentAlignment.MiddleRight
      '.TextImageRelation = TextImageRelation.TextBeforeImage
      .TextAlign = ContentAlignment.MiddleLeft

      .Left = 0
      .Top = Me.Height - buttonSize.Height
      .Size = buttonSize

      .Text = "Color..."
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(Button)

    ResumeLayout(False)
  End Sub

  Private Sub button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button.Click
    'Open the ColorDialog component and set the color selected by user
    With New ColorDialog
      .SolidColorOnly = True
      .ShowHelp = True
      If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
        Label.BackColor = .Color
      End If
    End With
  End Sub

End Class
