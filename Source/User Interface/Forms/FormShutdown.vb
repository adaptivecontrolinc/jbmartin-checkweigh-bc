Public Class FormShutdown : Inherits Form

  Private labelMain As Label
  Private timerMain As Windows.Forms.Timer

  Private marginLeft As Integer = 8
  Private marginRight As Integer = 8

  Private marginTop As Integer = 8
  Private marginBottom As Integer = 8

  Private defaultWidth As Integer = 280
  Private defaultHeight As Integer = 100

  Private shutdownSeconds As Integer = 5
  Private startTime As Date
  Private endTime As Date

  Sub New()
    ClientSize = New Size(defaultWidth, defaultHeight)
    Text = "Adaptive Check Weigh"

    ControlBox = False
    FormBorderStyle = FormBorderStyle.Sizable
    MaximizeBox = False
    MinimizeBox = False
    SizeGripStyle = Windows.Forms.SizeGripStyle.Hide
    StartPosition = FormStartPosition.CenterScreen
    TopMost = True
    WindowState = FormWindowState.Normal

    ShowIcon = False
    Icon = My.Resources.Weigh

    AddControls()
    Start()
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    labelMain = New Label
    With labelMain
      .AutoSize = False
      .Dock = DockStyle.Fill
      .Font = New Font("Tahoma", 12)
      .ForeColor = Color.RoyalBlue
      .Text = "Shutdown in " & shutdownSeconds & " seconds"
      .TextAlign = ContentAlignment.MiddleCenter
    End With
    Me.Controls.Add(labelMain)

    timerMain = New Windows.Forms.Timer
    With timerMain
      .Interval = 500
      .Enabled = False
      AddHandler .Tick, AddressOf TimerMainTick
    End With

    ResumeLayout(False)
  End Sub

  Sub Start()
    startTime = Date.UtcNow
    endTime = startTime.AddSeconds(shutdownSeconds)
    timerMain.Enabled = True
  End Sub

  Private Sub CheckTimeRemaining()
    Dim timeRemaining = endTime.Subtract(Date.UtcNow)
    Dim secondsRemaining = timeRemaining.Seconds

    If secondsRemaining > 0 Then
      labelMain.Text = "Shutdown in " & timeRemaining.Seconds.ToString & " seconds"
    Else
      Close()
    End If
  End Sub

  Private Sub TimerMainTick(sender As Object, e As EventArgs)
    timerMain.Enabled = False
    CheckTimeRemaining()
    timerMain.Enabled = True
  End Sub

End Class


