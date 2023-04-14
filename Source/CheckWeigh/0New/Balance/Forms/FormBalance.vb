Public Class FormBalance : Inherits Form
  Private controlCode As ControlCode

  Private balanceControls As New List(Of BalanceControl)

  Private tabControl As TabControl
  Private timerMain As Windows.Forms.Timer

  Private defaultWidth As Integer = 640
  Private defaultHeight As Integer = 480

  Sub New(controlcode As ControlCode)
    ClientSize = New Size(defaultWidth, defaultHeight)
    Text = "Balance"

    Me.controlCode = controlcode

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

    tabControl = New TabControl
    With tabControl
      .Dock = DockStyle.Fill
    End With
    Controls.Add(tabControl)

    timerMain = New Windows.Forms.Timer
    With timerMain
      .Interval = 1000
      .Enabled = True

      AddHandler .Tick, AddressOf TimerMain_Tick
    End With

    AddTabPages()

    ResumeLayout(False)
  End Sub

  Private Sub AddTabPages()
    For Each Balance As BalanceBase In controlCode.IO.BalanceList
      AddTabPage(Balance)
    Next
  End Sub

  Private Sub AddTabPage(balance As BalanceBase)
    ' Create a new tab page for the balance
    Dim newTabPage = New TabPage(balance.Name)
    tabControl.TabPages.Add(newTabPage)

    ' Create a new balance control and add it to the list
    Dim newBalanceControl As New BalanceControl With {.Dock = DockStyle.Fill}
    balanceControls.Add(newBalanceControl)

    ' Add the control to the tab page and connect the balance
    newTabPage.Controls.Add(newBalanceControl)
    newBalanceControl.Connect(balance)
  End Sub

  Private Sub TimerMain_Tick(sender As Object, e As EventArgs)
    For Each balancecontrol In balanceControls
      balancecontrol.Requery()
    Next
  End Sub
End Class


