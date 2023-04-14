Public Class BalanceTcp : Inherits BalanceBase

  Private threadRun As Threading.Thread
  Private threadCancel As Boolean

  Sub New()
    Name = "Tcp"
  End Sub

  Public Overrides Sub Start()
    ' Setup and start a background thread for the balance
    threadCancel = False
    threadRun = New System.Threading.Thread(AddressOf Run)
    With threadRun
      .Name = "BalanceTcp"
      .Priority = Threading.ThreadPriority.BelowNormal
      .Start()
    End With
  End Sub

  Public Overrides Sub Cancel()
    threadCancel = True
  End Sub

  Private Sub Run()
    OpenPort()
    Do
      Try
        ' Some code
      Catch ex As Exception
        RunError = ex.Message
      End Try
    Loop Until threadCancel
    ClosePort()
  End Sub

  Private Sub OpenPort()
  End Sub

  Private Sub ClosePort()
  End Sub

End Class
