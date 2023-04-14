Public Class FormSettings : Inherits Form
  Private controlCode As ControlCode

  Private settings As SettingsControl

  Private defaultWidth As Integer = 880
  Private defaultHeight As Integer = 660

  Sub New(ByVal controlCode As ControlCode)
    ClientSize = New Size(defaultWidth, defaultHeight)
    Text = "Settings"

    Me.controlCode = controlCode

    ControlBox = True
    FormBorderStyle = FormBorderStyle.Sizable
    MaximizeBox = False
    MinimizeBox = False

    SizeGripStyle = Windows.Forms.SizeGripStyle.Hide
    StartPosition = FormStartPosition.CenterParent
    WindowState = FormWindowState.Normal

    ShowIcon = False

    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    settings = New SettingsControl(controlCode)
    With settings
      .Dock = DockStyle.Fill
    End With
    Controls.Add(settings)

    ResumeLayout(False)
  End Sub

End Class

