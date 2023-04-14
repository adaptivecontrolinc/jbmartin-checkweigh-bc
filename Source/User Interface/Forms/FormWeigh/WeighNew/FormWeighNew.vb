Public Class FormWeighNew : Inherits Windows.Forms.Form
  Private ReadOnly controlCode As ControlCode

  Private weigh As Weigh

  Private groupBoxInstructions As GroupBox
  Private groupBoxProgress As GroupBox
  Private groupBoxButtons As GroupBox

  Private instructions As WeighInstructions
  Private progress As WeighProgress
  Private buttons As WeighButtons

  Private marginLeft As Integer = 8
  Private marginRight As Integer = 8

  Private marginTop As Integer = 8
  Private marginBottom As Integer = 8

  Private defaultWidth As Integer = 700
  Private defaultHeight As Integer = 525

  Private controlSpacing As Integer = 4
  Private buttonsWidth As Integer = 200
  Private progressHeight As Integer = 200

  Sub New(controlcode As ControlCode)
    Me.controlCode = controlcode

    weigh = New Weigh(controlcode)

    ClientSize = New Size(defaultWidth, defaultHeight)
    Text = "Adaptive Check Weigh"

    ControlBox = True
    FormBorderStyle = FormBorderStyle.Sizable
    MaximizeBox = False
    MinimizeBox = False
    ShowIcon = False
    SizeGripStyle = Windows.Forms.SizeGripStyle.Hide
    StartPosition = FormStartPosition.CenterScreen
    WindowState = FormWindowState.Normal

    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()
    AddInstructions()
    AddProgress()
    AddButtons()
    ResumeLayout(False)
  End Sub

  Private Sub AddInstructions()
    Dim x = marginLeft
    Dim y = marginTop

    Dim width = defaultWidth - marginLeft - marginRight - controlSpacing - buttonsWidth
    Dim height = defaultHeight - marginTop - marginBottom - controlSpacing - progressHeight

    groupBoxInstructions = New GroupBox
    With groupBoxInstructions
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Padding = New Padding(12, 8, 8, 8)

      .Location = New Point(x, y)
      .Size = New Size(width, height)

      .Text = "Please follow the instructions below"
    End With
    Controls.Add(groupBoxInstructions)

    instructions = New WeighInstructions With {.Dock = DockStyle.Fill, .BorderStyle = BorderStyle.FixedSingle}
    groupBoxInstructions.Controls.Add(instructions)
  End Sub

  Private Sub AddProgress()
    Dim x = marginLeft
    Dim y = defaultHeight - marginBottom - progressHeight

    Dim width = defaultWidth - marginLeft - marginRight - controlSpacing - buttonsWidth
    Dim height = progressHeight

    groupBoxProgress = New GroupBox
    With groupBoxProgress
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Padding = New Padding(8, 8, 8, 8)

      .Location = New Point(x, y)
      .Size = New Size(width, height)

      .Text = "Please weigh the amount below on the scale"
    End With
    Controls.Add(groupBoxProgress)

    progress = New WeighProgress With {.Dock = DockStyle.Fill}
    groupBoxProgress.Controls.Add(progress)
  End Sub

  Private Sub AddButtons()
    Dim x = defaultWidth - marginRight - buttonsWidth
    Dim y = marginTop

    Dim width = buttonsWidth
    Dim height = defaultHeight - marginTop - marginBottom

    groupBoxButtons = New GroupBox
    With groupBoxButtons
      .Anchor = AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Padding = New Padding(8)

      .Location = New Point(x, y)
      .Size = New Size(width, height)

      .Text = "Actions"
    End With
    Controls.Add(groupBoxButtons)

    buttons = New WeighButtons With {.Dock = DockStyle.Fill, .BorderStyle = BorderStyle.FixedSingle}
    groupBoxButtons.Controls.Add(buttons)
  End Sub

End Class
