Public Class LabelComboBoxTextBox :: Inherits UserControl
  Public Event ValueUpdated(sender As Object, index As Integer, text As String)

  ' Label above a combo box
  Public Label As New Label
  Public ComboBox As New ComboBox
  Public TextBox As New TextBox

  Property Orientation As ELabelPosition = ELabelPosition.Top
  Property LabelWidth As Integer

  Property SqlColumnName As String  ' column name if mapping to a table

  Public Sub New()
    Label.Text = "ComboBox"
    NewBase()
  End Sub

  Public Sub New(label As String)
    Me.Label.Text = label
    NewBase()
  End Sub

  Public Sub New(label As String, labelWidth As Integer, orientation As ELabelPosition)
    Me.Label.Text = label
    Me.Orientation = orientation
    Me.LabelWidth = labelWidth
    NewBase()
  End Sub

  Private Sub NewBase()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent
    AddControls()
  End Sub

  Private Sub AddControls()
    If Orientation = ELabelPosition.Top Then AddControlsVertical()
    If Orientation = ELabelPosition.Left Then AddControlsHorizontal()
  End Sub

  Private Sub AddControlsVertical()
    SuspendLayout()
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    With Label
      .AutoSize = False
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .Location = New Point(-3, 0)
      .Size = New Size(Me.Width + 3, 20)
      .TextAlign = ContentAlignment.MiddleLeft
    End With

    With ComboBox
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .Location = New Point(0, Label.Height)
      .Size = New Size(Me.Width, 20)
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(ComboBox)

    ResizeControl()
    ResumeLayout(False)
  End Sub

  Private Sub AddControlsHorizontal()
    SuspendLayout()
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    If LabelWidth = 0 Then LabelWidth = CInt(Math.Floor(Me.Width / 2))
    With Label
      .AutoSize = False
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top
      .Location = New Point(0, 0)
      .Size = New Size(LabelWidth, 24)
      .TextAlign = ContentAlignment.MiddleLeft
    End With

    With ComboBox
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .Location = New Point(LabelWidth, 0)
      .Size = New Size(Me.Width - LabelWidth, 24)
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(ComboBox)

    ResizeControl()
    ResumeLayout(False)
  End Sub

  Private Sub Me_Resize(sender As Object, e As EventArgs) Handles Me.Resize
    ResizeControl()
  End Sub

  Private Sub ResizeControl()
    If Orientation = ELabelPosition.Top Then ResizeControlVertical()
    If Orientation = ELabelPosition.Left Then ResizeControlHorizontal()
  End Sub

  Private Sub ResizeControlVertical()
    Me.Height = Label.Height + ComboBox.Height + 1
  End Sub

  Private Sub ResizeControlHorizontal()
    Label.Height = ComboBox.Height
    Me.Height = ComboBox.Height
  End Sub

End Class
