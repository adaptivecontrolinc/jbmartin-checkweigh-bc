﻿Public Class FormDataGridCustomize : Inherits Windows.Forms.Form

  Protected WithEvents mainControl As DataGridCustomize
  Protected WithEvents buttonOK As Button
  Protected WithEvents buttonCancel As Button

  Private Const formWidth = 640
  Private Const formHeight = 498

  Private Const controlPadLeft = 16
  Private Const controlPadTop = 16

  Private Const buttonWidth = 100
  Private Const buttonHeight = 32

  Private formSize As New Size(formWidth, formHeight)
  Private controlSize As New Size(formWidth - (controlPadLeft * 2), formHeight - (controlPadTop * 2) - buttonHeight - 8)
  Private buttonSize As New Size(buttonWidth, buttonHeight)

  Private buttonX, buttonY As Integer

  ReadOnly Property GridLayout As DataGridLayout
    Get
      Return mainControl.Manager.GridLayout
    End Get
  End Property

  Public Sub New()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Defaults.BackColor

    ClientSize = New Size(formWidth, formHeight)
    Text = "Customize Data Grid"

    ControlBox = True
    FormBorderStyle = FormBorderStyle.Sizable
    MaximizeBox = False
    MinimizeBox = False

    SizeGripStyle = Windows.Forms.SizeGripStyle.Hide
    StartPosition = FormStartPosition.CenterScreen
    WindowState = FormWindowState.Normal

    ShowIcon = False

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Private Sub AddControls()
    SuspendLayout()


    mainControl = New DataGridCustomize
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

    buttonX = mainControl.Left + mainControl.Width - buttonWidth

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


  Sub Connect(grid As DataGridX)
    mainControl.Connect(grid)
  End Sub

  Private Sub buttonOK_Click(sender As Object, e As EventArgs) Handles buttonOK.Click
    DialogResult = DialogResult.OK
    Close()
  End Sub

  Private Sub buttonCancel_Click(sender As Object, e As EventArgs) Handles buttonCancel.Click
    DialogResult = DialogResult.Cancel
    Close()
  End Sub

End Class
