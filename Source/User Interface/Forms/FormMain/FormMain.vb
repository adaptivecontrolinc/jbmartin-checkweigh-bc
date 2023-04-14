Public Class FormMain : Inherits BaseControl2 ' Inherits Form
  Private controlCode As ControlCode

  Private batches As ControlBatches
  Private steps As ControlSteps
  Private statusBarMain As StatusBar

  Private formTimerRequery As Windows.Forms.Timer
  Private formTimerScale As Windows.Forms.Timer

  Private marginLeft As Integer = 8
  Private marginRight As Integer = 8

  Private marginTop As Integer = 8
  Private marginBottom As Integer = 8

  Private defaultWidth As Integer = 800 ' 1024
  Private defaultHeight As Integer = 600 ' 768

  Private statusBarHeight As Integer = 32

  Sub New(ByVal controlcode As ControlCode)
    Me.controlCode = controlcode

    ClientSize = New Size(defaultWidth, defaultHeight)
    Text = "Adaptive Check Weigh: " & controlcode.Settings.Customer

    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    batches = New ControlBatches(controlCode)
    With batches
      .Dock = DockStyle.Fill
      .Visible = True

      AddHandler .BatchSelected, AddressOf BatchSelected
      AddHandler .ButtonClick, AddressOf ButtonClick
    End With
    Controls.Add(batches)

    steps = New ControlSteps(controlCode)
    With steps
      .Dock = DockStyle.Fill
      .Visible = False

      AddHandler .StepSelected, AddressOf StepSelected
      AddHandler .ButtonClick, AddressOf ButtonClick
    End With
    Controls.Add(steps)

    AddStatusBar()
    AddTimers()

    ResumeLayout(False)
  End Sub

  Private Sub AddStatusBar()
    statusBarMain = New StatusBar
    With statusBarMain
      .Height = 32
      .Dock = DockStyle.Bottom
      .ShowPanels = True
      .SizingGrip = False
    End With

    Dim panelMain = New StatusBarPanel
    With panelMain
      .Alignment = HorizontalAlignment.Left
      .AutoSize = StatusBarPanelAutoSize.Spring
      .Name = "PanelMain"
      .Text = "Database"
      .Width = 300
    End With
    statusBarMain.Panels.Add(panelMain)

    ' Add a status bar panel for each balance
    For i As Integer = 0 To controlCode.IO.BalanceList.Count - 1
      AddBalancePanel(i)
    Next

    Controls.Add(statusBarMain)
  End Sub

  Private Sub AddBalancePanel(balanceNumber As Integer)
    ' Add a status bar panel for each balance
    Dim panelBalance = New StatusBarPanel
    With panelBalance
      .Alignment = HorizontalAlignment.Center
      .Name = "Balance " & balanceNumber
      .Tag = balanceNumber
      .Text = .Name
      .Width = 240
    End With
    statusBarMain.Panels.Add(panelBalance)
  End Sub

  Private Sub AddTimers()
    formTimerRequery = New Windows.Forms.Timer
    With formTimerRequery
      .Interval = 4000
      .Enabled = True
      AddHandler .Tick, AddressOf FormTimerRequery_Tick
    End With

    formTimerScale = New Windows.Forms.Timer
    With formTimerScale
      .Interval = 4000
      .Enabled = True
      AddHandler .Tick, AddressOf FormTimerScale_Tick
    End With
  End Sub


  '--------------------------------------------------------------------------------------------
  ' Requery database
  '--------------------------------------------------------------------------------------------
  Private Sub FormTimerRequery_Tick(sender As Object, e As EventArgs)
    formTimerRequery.Enabled = False
    Requery()
    formTimerRequery.Interval = 8000
    formTimerRequery.Enabled = True
  End Sub

  Private Sub Requery()
    If batches.Visible Then batches.Requery()
    If steps.Visible Then steps.Requery()
  End Sub

  Private Sub UpdateMainPanel()
    If statusBarMain Is Nothing Then Exit Sub
    If statusBarMain.Panels.Count < 1 Then Exit Sub

    statusBarMain.Panels(0).Text = " Last database read:  " & Date.Now.ToString
  End Sub


  '--------------------------------------------------------------------------------------------
  ' Update balance panels with current weights
  '--------------------------------------------------------------------------------------------
  Private Sub FormTimerScale_Tick(sender As Object, e As EventArgs)
    formTimerScale.Enabled = False
    UpdateBalancePanels()
    formTimerScale.Interval = 1000
    formTimerScale.Enabled = True
  End Sub

  Private Sub UpdateBalancePanels()
    If statusBarMain Is Nothing Then Exit Sub
    If statusBarMain.Panels.Count < 2 Then Exit Sub

    For i As Integer = 1 To statusBarMain.Panels.Count - 1
      Dim balanceNumber = CInt(statusBarMain.Panels(i).Tag)
      If balanceNumber <= controlCode.IO.BalanceList.Count - 1 Then
        statusBarMain.Panels(i).Text = controlCode.IO.BalanceList(balanceNumber).DisplayWeight
      End If
    Next
  End Sub


  '--------------------------------------------------------------------------------------------
  ' Control events
  '--------------------------------------------------------------------------------------------
  Private Sub BatchSelected(sender As Object, batchRow As DataRow)
    If batchRow Is Nothing Then Exit Sub
    ConnectBatch(batchRow)
  End Sub

  Private Sub StepSelected(sender As Object, batchRow As DataRow, stepRow As DataRow)
    If batchRow Is Nothing OrElse stepRow Is Nothing Then Exit Sub
    ConnectStep(batchRow, stepRow)
  End Sub

  Private Sub ConnectBatch(batchRow As DataRow)
    If batchRow Is Nothing Then Exit Sub
    steps.Connect(batchRow)
    ShowSteps()
  End Sub

  Private Sub ConnectStep(batchRow As DataRow, stepRow As DataRow)
    If batchRow Is Nothing OrElse stepRow Is Nothing Then Exit Sub

    'Using newform As New FormWeighNew
    '  newform.ShowDialog(Me)
    'End Using

    Using newform As New FormWeigh(controlCode)
      newform.Start(batchRow, stepRow)
      newform.ShowDialog(Me)
    End Using
  End Sub


  '--------------------------------------------------------------------------------------------
  ' Button events
  '--------------------------------------------------------------------------------------------
  Private Sub ButtonClick(sender As Object, button As Button)
    Select Case button.Name
      Case "Return" : ShowBatches()
      Case "Refresh" : Requery()
      Case "Balance" : ShowBalance()
      Case "Settings" : ShowSettings()
      Case "About" : ShowAbout()
      Case "Exit" : Shutdown()
    End Select
  End Sub

  Sub ShowBatches()
    If steps.Visible Then
      ' TODO - Cancel active batch
      If controlCode.CW.IsOn Then controlCode.CW.Cancel()
      batches.Visible = True
      steps.Visible = False
    End If
  End Sub

  Sub ShowSteps()
    If batches.Visible Then
      steps.Visible = True
      batches.Visible = False
    End If
  End Sub

  Sub ShowCustomize()

  End Sub

  Sub ShowSettings()
    Using newform As New FormSettings(controlCode)
      newform.ShowDialog(Me)
    End Using
  End Sub

  Sub ShowBalance()
    Using newform As New FormBalance(Me.controlCode)
      newform.ShowDialog(Me)
    End Using
  End Sub

  Private Sub ShowAbout()
    Using newform As New FormSplash
      newform.ShowDialog(Me)
    End Using
  End Sub

  Private Sub Shutdown()
    Dim question = "Are you sure you want to close Check Weigh?"
    If MessageBox.Show(question, "Adaptive", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
      formTimerRequery.Enabled = False
      ' TODO - remove this button
      '     Me.Close()
    End If
  End Sub

End Class

