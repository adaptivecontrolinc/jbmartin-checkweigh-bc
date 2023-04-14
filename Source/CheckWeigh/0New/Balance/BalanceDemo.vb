' This is a balance simulation so we will use the start and cancel calls to simulate a scale - maybe...
Public Class BalanceDemo : Inherits BalanceBase

  Private threadRun As Threading.Thread
  Private threadCancel As Boolean

  Sub New()
    Name = "Demo"
  End Sub

  Public Overrides Sub Start()
    ' To satisfy inheritance rules
  End Sub

  Public Overrides Sub Cancel()
    ' To satisfy inheritance rules
  End Sub

End Class
