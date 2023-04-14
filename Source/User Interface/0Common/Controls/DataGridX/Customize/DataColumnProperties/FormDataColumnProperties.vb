Public Class FormDataColumnProperties : Inherits Windows.Forms.Form

  Protected WithEvents mainControl As DataColumnProperties
  Protected WithEvents buttonOK As Button
  Protected WithEvents buttonCancel As Button

  Private Const formWidth = 300
  Private Const formHeight = 380

  Private Const controlPadLeft = 16
  Private Const controlPadTop = 16

  Private Const buttonWidth = 100
  Private Const buttonHeight = 32

  Private formSize As New Size(formWidth, formHeight)
  Private controlSize As New Size(formWidth - (controlPadLeft * 2), formHeight - (controlPadTop * 2) - buttonHeight - 8)
  Private buttonSize As New Size(buttonWidth, buttonHeight)

  Private buttonX, buttonY As Integer

  Public Sub New()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Defaults.BackColor

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    MyBase.ClientSize = formSize
    ControlBox = False
    MaximizeBox = False
    MinimizeBox = False
    Name = "FormDataColumnProperties"
    ShowIcon = False
    ShowInTaskbar = False
    SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Text = "Column Properties"

    mainControl = New DataColumnProperties
    With mainControl
      .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
      .AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
      .BackColor = System.Drawing.Color.Transparent
      .TabIndex = 0

      .Size = controlSize
      .Location = New System.Drawing.Point(controlPadLeft, controlPadTop)
    End With
    Controls.Add(mainControl)

    buttonX = mainControl.Left - 1
    buttonY = mainControl.Top + mainControl.Height + 8

    buttonOK = New Button
    With buttonOK
      .Anchor = AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
      .Name = "buttonOK"
      .TabIndex = 1
      .Text = "OK"
      .UseVisualStyleBackColor = True

      .Size = buttonSize
      .Location = New System.Drawing.Point(buttonX, buttonY)
    End With
    Controls.Add(buttonOK)

    buttonX = mainControl.Left + mainControl.Width - buttonWidth + 1

    buttonCancel = New Button
    With buttonCancel
      .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
      .Name = "buttonCancel"
      .TabIndex = 2
      .Text = "Cancel"
      .UseVisualStyleBackColor = True

      .Size = buttonSize
      .Location = New System.Drawing.Point(buttonX, buttonY)
    End With
    Controls.Add(buttonCancel)

    ResumeLayout(False)
  End Sub



  Sub Connect(column As DataGridLayout.Column)
    mainControl.Connect(column)
  End Sub

  Private Sub buttonOK_Click(sender As Object, e As EventArgs) Handles buttonOK.Click
    mainControl.SaveData()
    DialogResult = DialogResult.OK
    Close()
  End Sub

  Private Sub buttonCancel_Click(sender As Object, e As EventArgs) Handles buttonCancel.Click
    DialogResult = DialogResult.Cancel
    Close()
  End Sub

  ReadOnly Property GridColumn As DataGridLayout.Column
    Get
      Return mainControl.GridColumn
    End Get
  End Property


End Class
