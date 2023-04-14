Public Class SettingsControl : Inherits UserControl
  Private controlCode As ControlCode

  Private settingsGrid As PropertyGrid

  Sub New(ByVal controlCode As ControlCode)
    Me.controlCode = controlCode

    DoubleBuffered = True
    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    settingsGrid = New PropertyGrid
    With settingsGrid
      .Dock = DockStyle.Fill
      .SelectedObject = controlCode.Settings
    End With
    Controls.Add(settingsGrid)

    ResumeLayout(False)
  End Sub

End Class
