Public Class Flasher

  Private startTickCount As UInt32

    Property TimeOn As Integer       ' In milliseconds
    Property TimeOff As Integer
    Property Period As Integer

    Sub New()
      NewBase(1000, 1000)
    End Sub
    Sub New(timeOn As Integer)
      NewBase(timeOn, timeOn)
    End Sub
    Sub New(timeOn As Integer, timeOff As Integer)
      NewBase(timeOn, timeOff)
    End Sub
    Private Sub NewBase(timeOn As Integer, timeOff As Integer)
      Me.TimeOn = timeOn
      Me.TimeOff = timeOff
      Me.Period = timeOn + timeOff
      startTickCount = TickCount
    End Sub

    ReadOnly Property [On] As Boolean
      Get
        Dim elapsed = TickCount - startTickCount
        Dim x As UInt32 = elapsed Mod CType(Period, UInt32)
        Return (x < TimeOn)
      End Get
    End Property


#If 0 Then
  ' From Support .vb
  
Public Class Flasher
  Private startTime_ As UInt32

  Public Sub Flash(ByRef variable As Boolean, ByVal onMilliSeconds As Integer)
    Flash(variable, onMilliSeconds, onMilliSeconds)
  End Sub

  Public Sub Flash(ByRef variable As Boolean, ByVal onMilliSeconds As Integer, ByVal offMilliSeconds As Integer)
    ' Initialise StartTime
    Dim elapsed As UInt32
    If startTime_ = 0 Then
      startTime_ = TickCount
    Else
      elapsed = TickCount - startTime_
    End If

    ' Generate a number that ranges from 0 to the sum
    Dim x As UInt32 = elapsed Mod CType(onMilliSeconds + offMilliSeconds, UInt32)
    variable = (x < onMilliSeconds)
  End Sub
End Class

#End If
End Class
