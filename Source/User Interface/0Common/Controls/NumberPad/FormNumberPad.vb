Public Class FormNumberPad : Inherits Windows.Forms.Form

  Private textBoxValue As TextBox
  Private numberPad As NumberPad

  Private buttonOK As Button
  Private buttonCancel As Button

  Private buttonWidth As Integer = 104
  Private buttonHeight As Integer = 32

  Private defaultWidth As Integer = 240
  Private defaultHeight As Integer = 264

  ReadOnly Property Value As Double
    Get
      Dim tryDouble As Double
      If Double.TryParse(textBoxValue.Text, tryDouble) Then
        Return tryDouble
      Else
        Return 0
      End If
    End Get
  End Property

  Sub New()
    ClientSize = New Size(defaultWidth, defaultHeight)

    ControlBox = False
    FormBorderStyle = FormBorderStyle.Sizable
    MaximizeBox = False
    MinimizeBox = False
    SizeGripStyle = Windows.Forms.SizeGripStyle.Hide
    StartPosition = FormStartPosition.CenterScreen
    WindowState = FormWindowState.Normal

    ShowIcon = False
    Icon = My.Resources.Weigh

    AddControls()
  End Sub

  Private Sub AddControls()
    Dim x = 2
    Dim y = 2

    textBoxValue = New TextBox
    With textBoxValue
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
      .Font = New Font("Tahoma", 16)
      .Location = New Point(x, y)
      .Width = defaultWidth - 4

      .TabStop = False
      .TextAlign = HorizontalAlignment.Center
    End With
    Controls.Add(textBoxValue)

    numberPad = New NumberPad
    With numberPad
      y = textBoxValue.Top + textBoxValue.Height + 2
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Location = New Point(x, y)
      .Width = defaultWidth - 4
      .Height = defaultHeight - textBoxValue.Height - 4 - buttonHeight - 8

      AddHandler .NumberClick, AddressOf NumberPad_NumberClick
      AddHandler .ControlClick, AddressOf NumberPad_ControlClick
    End With
    Controls.Add(numberPad)

    buttonWidth = (defaultWidth - 8) \ 2

    buttonOK = New Button
    With buttonOK
      y = defaultHeight - buttonHeight - 2
      .Anchor = AnchorStyles.Left Or AnchorStyles.Bottom
      .Location = New Point(x, y)
      .Size = New Size(buttonWidth, buttonHeight)
      .Text = "OK"

      AddHandler .Click, AddressOf ButtonOK_Click
    End With
    Controls.Add(buttonOK)

    buttonCancel = New Button
    With buttonCancel
      x = defaultWidth - buttonWidth - 2
      y = defaultHeight - buttonHeight - 2
      .Anchor = AnchorStyles.Left Or AnchorStyles.Bottom
      .Location = New Point(x, y)
      .Size = New Size(buttonWidth, buttonHeight)
      .Text = "Cancel"

      AddHandler .Click, AddressOf ButtonCancel_Click
    End With
    Controls.Add(buttonCancel)
  End Sub

  Private Sub ButtonOK_Click(sender As Object, e As EventArgs)
    Me.DialogResult = DialogResult.OK
    Me.Close()
  End Sub

  Private Sub ButtonCancel_Click(sender As Object, e As EventArgs)
    Me.DialogResult = DialogResult.Cancel
    Me.Close()
  End Sub

  Private Sub NumberPad_NumberClick(text As String)
    If textBoxValue.Text Is Nothing Then
      textBoxValue.Text = text
    Else
      textBoxValue.Text &= text
    End If
  End Sub

  Private Sub NumberPad_ControlClick(text As String)
    If text.ToUpper = "DELETE" Then textBoxValue.Text = Nothing
  End Sub

End Class
