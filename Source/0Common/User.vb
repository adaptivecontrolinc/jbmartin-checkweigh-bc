Public Class User : Inherits MarshalByRefObject
  Private controlcode As ControlCode

  Friend UserData As New Utilities.UserXml  ' Declared Friend so we don't see the password values in Variables

  Property Supervisor As Integer
  Property Expert As Integer
  Property Shutdown As Integer

  Property AutoUpdateTimer As New Timer With {.Seconds = 32}

  Property AutoLogOffTimer As New Timer With {.Seconds = 240}

  Property AutoLogOffSeconds As Integer = 240

  Private name_ As String = "Operator"
  Property Name As String
    Get
      Return name_
    End Get
    Set(value As String)
      If value <> name_ Then
        name_ = value
        AutoLogOffTimer.Seconds = AutoLogOffSeconds
      End If
    End Set
  End Property

  Sub New(controlcode As ControlCode)
    Me.controlcode = controlcode
    Update()
  End Sub

  Sub Update()
    UserData.Load()

    ' A token effort to hide the passwords
    Supervisor = GetMaskedValue(UserData.Supervisor)
    Expert = GetMaskedValue(UserData.Expert)
    Shutdown = GetMaskedValue(UserData.Shutdown)
  End Sub

  Sub Run()
    With controlcode
      If AutoLogOffTimer.Finished Then Name = "Operator"

      ' Every 32 seconds if no program running
      If AutoUpdateTimer.Finished Then
        If .Parent.IsProgramRunning Then
          AutoUpdateTimer.Seconds = 8
        Else
          Update()
          AutoUpdateTimer.Seconds = 16
        End If
      End If
    End With
  End Sub

  Private Function GetMaskedValue(password As String) As Integer
    Dim tryInteger As Integer
    If Integer.TryParse(password, tryInteger) Then
      Return tryInteger * 88
    End If
    Return -1
  End Function

End Class