
Public Class Timer : Inherits MarshalByRefObject
  Private interval_, startTickCount_, pauseTickCount_ As UInt32, paused_, finished_ As Boolean

  Friend Property TimeRemainingMs() As Integer
    Get
      If Finished Then Return 0
      Dim t As UInt32
      If paused_ Then
        t = pauseTickCount_
      Else
        t = TickCount
      End If
      Return CType(interval_ - (t - startTickCount_), Integer)
    End Get
    Set(ByVal value As Integer)
      interval_ = CType(value, UInt32)
      startTickCount_ = TickCount
      paused_ = False : finished_ = False
    End Set
  End Property

  Public Property TimeRemaining() As Integer
    Get
      Return (TimeRemainingMs + 500) \ 1000
    End Get
    Set(ByVal value As Integer)
      TimeRemainingMs = value * 1000
    End Set
  End Property

  ' TimeRemainingMs and MilliSeconds are the same
  Friend Property Milliseconds As Integer
    Get
      Return TimeRemainingMs
    End Get
    Set(ByVal value As Integer)
      TimeRemainingMs = value
    End Set
  End Property

  ' TimeRemaining and Seconds are the same
  Friend Property Seconds As Integer
    Get
      Return TimeRemaining
    End Get
    Set(ByVal value As Integer)
      TimeRemaining = value
    End Set
  End Property

  Friend Property Minutes() As Integer
    Get
      Return (TimeRemainingMs + 30000) \ 60000
    End Get
    Set(ByVal value As Integer)
      TimeRemainingMs = value * 60000
    End Set
  End Property

  Public ReadOnly Property Finished() As Boolean
    Get
      If Not finished_ AndAlso Not paused_ AndAlso TickCount - startTickCount_ >= interval_ Then finished_ = True
      Return finished_
    End Get
  End Property

  Private Sub CheckFinished()
    If Not paused_ AndAlso TickCount - startTickCount_ >= interval_ Then finished_ = True
  End Sub

  Public Sub Pause()
    'If we're already finshed or paused then ignore
    If Finished OrElse paused_ Then Exit Sub
    paused_ = True : pauseTickCount_ = TickCount
  End Sub

  Public Sub Restart()
    'If we're not paused then ignore
    If finished_ OrElse Not paused_ Then Exit Sub
    Dim lostTime As UInt32 = TickCount - pauseTickCount_
    startTickCount_ += lostTime
    paused_ = False
  End Sub

  Public Sub Cancel()
    TimeRemainingMs = 0
  End Sub

  Public ReadOnly Property Paused() As Boolean
    Get
      Return paused_
    End Get
  End Property

  Public Overrides Function ToString() As String
    With TimeSpan.FromTicks(TimeRemainingMs * TimeSpan.TicksPerMillisecond)
      Select Case .TotalSeconds
        Case Is >= 86400
          Return .Days.ToString("00") & ":" & .Hours.ToString("00") & "h"
        Case 3600 To 86399
          Return .Hours.ToString("00") & ":" & .Minutes.ToString("00") & "m"
        Case 1 To 3599
          Return .Minutes.ToString("00") & ":" & .Seconds.ToString("00") & "s"
        Case Else
          Return "00:00s"
      End Select
    End With
  End Function



#If 0 Then
  
  Property StartTimeUtc As Date = Date.UtcNow
  Property EndTimeUtc As Date = Date.UtcNow

  Property PauseTimeUtc As Date
  Property PauseTimeRemaining As TimeSpan

  Property Seconds() As Integer
    Get
      If PauseTimeUtc = Nothing Then
        Return Math.Max(CInt(EndTimeUtc.Subtract(Date.UtcNow).TotalSeconds), 0)
      Else
        Return Math.Max(CInt(PauseTimeRemaining.TotalSeconds), 0)
      End If
    End Get
    Set(ByVal value As Integer)
      StartTimeUtc = Date.UtcNow
      EndTimeUtc = StartTimeUtc.AddSeconds(value)
    End Set
  End Property

  ReadOnly Property Finished() As Boolean
    Get
      If PauseTimeUtc = Nothing Then
        Return (Date.UtcNow >= EndTimeUtc)
      End If
      Return False
    End Get
  End Property

  ReadOnly Property Paused() As Boolean
    Get
      Return (PauseTimeUtc <> Nothing)
    End Get
  End Property

  Sub Pause()
    If PauseTimeUtc = Nothing Then
      PauseTimeUtc = Date.UtcNow
      PauseTimeRemaining = EndTimeUtc.Subtract(PauseTimeUtc)
    End If
  End Sub

  Sub Restart()
    EndTimeUtc.Add(PauseTimeRemaining)
    PauseTimeUtc = Nothing
  End Sub

  Public Overloads Function ToString(padSpaces As Integer) As String
    If padSpaces > 0 Then
      Dim timerString As String = Me.ToString
      Return timerString.PadLeft(timerString.Length + padSpaces)
    End If
    Return Me.ToString
  End Function

  Public Overrides Function ToString() As String
    Try
      Dim s As Integer = Me.Seconds
      Dim ts As New TimeSpan(0, 0, s)
      Select Case ts.TotalSeconds
        Case Is >= 86400
          Return ts.Days.ToString("00") & ":" & ts.Hours.ToString("00") & "h"
        Case 3600 To 86399
          Return ts.Hours.ToString("00") & ":" & ts.Minutes.ToString("00") & "m"
        Case 1 To 3599
          Return ts.Minutes.ToString("00") & ":" & ts.Seconds.ToString("00") & "s"
        Case Else
          Return "00:00s"
      End Select
    Catch ex As Exception
      'TODO Log Error
    End Try
    Return Nothing
  End Function

#End If
End Class
