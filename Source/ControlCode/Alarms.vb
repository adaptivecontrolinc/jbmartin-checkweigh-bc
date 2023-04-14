Public Class Alarms : Inherits MarshalByRefObject
  Private ReadOnly controlcode As ControlCode

  '<Description("Batch Control Operating Mode - Non Critical")>
  Public Alarms_ModeTestSelected As Boolean

  '<Description("Batch Control Operating Mode - Non Critical")>
  Public Alarms_ModeOverrideSelected As Boolean

  '<Description("Batch Control Operating Mode - Non Critical")>
  Public Alarms_ModeDebugSelected As Boolean

  '<Description("Batch Control Operating Mode - Non Critical")>
  Public Alarms_Balance1NotResponding As Boolean

  '<Description("Emergency Stop Button Pressed - Critical")>
  Public Alarms_EmergencyStop As Boolean


  Sub New(controlcode As ControlCode)
    Me.controlcode = controlcode
  End Sub

  Sub Run()
    With controlcode

      ' Give the system some time to get going before we start checking alarms
      Static PowerUpTimer As New Timer With {.Seconds = 32}
      If Not PowerUpTimer.Finished Then Exit Sub

      'Control Code Modes
      Alarms_ModeTestSelected = (.Parent.Mode = Mode.Test)
      Alarms_ModeOverrideSelected = (.Parent.Mode = Mode.Override)
      Alarms_ModeDebugSelected = (.Parent.Mode = Mode.Debug) ' TODO And Not Settings.DemoMode

      ' Communications Loss
      Alarms_Balance1NotResponding = False ' TODO (Parent.Mode <> Mode.Debug) AndAlso .IO.Balance1.IsAlarm

      ' TODO - Need this?
      Alarms_EmergencyStop = False


    End With
  End Sub

End Class
