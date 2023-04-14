Public Class LabelListBox : Inherits UserControl

  Property Label As New Label
  Property ListBox As New ListBox

  Public Sub New()
    Label.Text = "ListBox"
    NewBase()
  End Sub

  Sub New(label As String)
    Me.Label.Text = label
    NewBase()
  End Sub

  Sub New(label As String, list As String)
    Me.Label.Text = label
    NewBase()
  End Sub

  Private Sub NewBase()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent

    AddControls()
  End Sub

  Private Sub AddControls()

    With Label
      .AutoSize = False
      .BackColor = Color.Transparent
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .Location = New Point(-3, 0)
      .Size = New Size(Me.Width + 3, 20)
      .TextAlign = ContentAlignment.MiddleLeft
    End With

    With ListBox
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom
      '.BackColor = Color.LightGray
      '.BorderStyle = BorderStyle.None
      .Location = New Point(0, Label.Height)
      .Size = New Size(Me.Width, Me.Height - 20)
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(ListBox)
  End Sub

End Class
