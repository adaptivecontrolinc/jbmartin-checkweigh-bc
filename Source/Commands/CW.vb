' Main check weigh command
Imports System.ComponentModel

<Command("Check Weigh", "", "", "", "16"), Description("Dispense the current job"), Category("Dispense Commands")>
Public Class CW : Inherits MarshalByRefObject : Implements ACCommand
  ' Keep a local copy for convenience
  Private ReadOnly controlCode As ControlCode

  Public Enum EState
    Off
    CheckWeigh
    Abort
    Done
  End Enum
  Property State As EState
  Property Status As String
  Property Timer As New Timer
  Property OverrunTimer As New Timer

  Sub New(ByVal controlCode As ControlCode)
    Me.controlCode = controlCode
  End Sub

  Public Sub ParametersChanged(ParamArray param() As Integer) Implements ACCommand.ParametersChanged
    ' Do nothing
  End Sub

  Public Function Start(ParamArray param() As Integer) As Boolean Implements ACCommand.Start
    With Me.controlCode
      .User.Update()  ' keep it updated

      If State = EState.Off Then
        Timer.Seconds = 60 ' 
        OverrunTimer.Minutes = .Parameters.StandardTimeCheckWeigh
        State = EState.CheckWeigh
      Else
        ' TODO what now??
      End If
    End With
    ' Foreground command
    Return False
  End Function

  Public Function Run() As Boolean Implements ACCommand.Run
    With Me.controlCode

      Select Case State
        Case EState.Off
          ' TODO - add this??
          ' Should never happen but just in case 
          'If Job IsNot Nothing Then CloseDispenseJob()
          Status = ""

        Case EState.CheckWeigh
          Status = "Check Weigh " & Timer.ToString


        Case EState.Abort
          Status = "Aborting " & Timer.ToString
          If Timer.Finished Then Cancel()


        Case EState.Done
          If Timer.Finished Then Cancel()
          Status = "Completing " & Timer.ToString


      End Select
    End With
    Return False
  End Function

  Public Sub Cancel() Implements ACCommand.Cancel
    State = EState.Off
    Status = ""
  End Sub

  ReadOnly Property IsOn() As Boolean Implements ACCommand.IsOn
    Get
      Return (State <> EState.Off)
    End Get
  End Property

  ReadOnly Property IsOff() As Boolean
    Get
      Return (State = EState.Off)
    End Get
  End Property

  ReadOnly Property IsDelayed() As Boolean
    Get
      Return (State = EState.CheckWeigh) AndAlso OverrunTimer.Finished
    End Get
  End Property

End Class

Partial Public Class ControlCode
  Public CW As New CW(Me)
End Class
