Public Class ControlCode : Inherits MarshalByRefObject : Implements ACControlCode

  Public Parent As ACParent
  Public Parameters As Parameters
  Public IO As IO
  Public Alarms As Alarms
  Public User As User

  Public SystemShutdown As Boolean                ' Shutdown flag clear IO when set

  Public ComputerName As String

  Property Settings As Settings
  Property DemoMode As Boolean                     ' If true simulate a scale
  Property DemoTimer As New Timer

  Property Balance As BalanceBase                  ' The active balance - this can change depending on setup and weight ranges
  Property BalanceList As New List(Of BalanceBase) ' List of all balances 

  ' Tank Monitoring Management
  Friend Manager As CheckWeighManager

  Public Sub New(ByVal parent As ACParent)
    Try
      Me.Parent = parent

      Parameters = New Parameters(Me)
      IO = New IO(Me)
      Alarms = New Alarms(Me)
      User = New User(Me)

      ' Save the computer name locally so we don't keep querying the OS
      Me.ComputerName = My.Computer.Name

      'Load local Settings.xml
      Dim settingsXmlLoader = New SettingsXml
      Settings = settingsXmlLoader.GetSettings

      ' Set demo property
      DemoMode = Settings.DemoMode

      ' Set the active balance to balance 0 initially
      Balance = IO.BalanceList(0)

      ' Finally start the manager
      Me.Manager = New CheckWeighManager(parent, Me)
      Manager.Start()

      ' Interface tweaks
      CustomizeOperatorToolStrip()
      CustomizeExpertToolStrip()

    Catch ex As Exception
      ' Do nothing
    End Try
  End Sub

  Public Sub Run() Implements ACControlCode.Run
    '------------------------------------------------------------------------------------------
    ' Run calibration for analogs
    '------------------------------------------------------------------------------------------
    ' TODO - something with the scale?

    '------------------------------------------------------------------------------------------
    ' Demo Mode
    '------------------------------------------------------------------------------------------
    If (Parent.Mode = Mode.Test) OrElse (Parent.Mode = Mode.Debug) Then
      If DemoMode Then
        If DemoTimer.Finished Then

          DemoTimer.Milliseconds = 100
        End If
      End If
    End If

    '------------------------------------------------------------------------------------------
    ' Flashers for alarms and prepare lamps
    '------------------------------------------------------------------------------------------
    Static FlasherSlow As New Flasher(800) : FlashSlow = FlasherSlow.On
    Static FlasherFast As New Flasher(400) : FlashFast = FlasherFast.On


    '------------------------------------------------------------------------------------------
    ' Run alarms, delays, utilities, parameters, sleep, hibernate
    '------------------------------------------------------------------------------------------
    Alarms.Run()


    '------------------------------------------------------------------------------------------
    ' Run remote push buttons
    '------------------------------------------------------------------------------------------
    Parent.PressButtons(False, False, False, False, False)

    '------------------------------------------------------------------------------------------
    ' Flag first code scan done 
    '------------------------------------------------------------------------------------------
    FirstCodeScanDone = True
  End Sub

  Public Sub ShutDown() Implements ACControlCode.ShutDown
    ' Close comms for all balances 
    For Each item In BalanceList : item.Cancel() : Next item

    ' Delay shutdown to allow ports to close
    Using shutdownForm As New FormShutdown
      shutdownForm.ShowDialog()
    End Using

    SystemShutdown = True
  End Sub

  Public Sub WriteOutputs(dout() As Boolean, anout() As Short) Implements ACControlCode.WriteOutputs
    ' Do Nothing
  End Sub

  Public Sub DrawScreen(screen As Integer, row() As String) Implements ACControlCode.DrawScreen
    ' Do Nothing
  End Sub

  Public Sub ProgramStart() Implements ACControlCode.ProgramStart
    ' Do Nothing
  End Sub

  Public Sub ProgramStop() Implements ACControlCode.ProgramStop
    ' Do Nothing

    ' TODO - maybe close all communications?
    ' Close comms for all balances 
    '    For Each item In BalanceList : item.Cancel() : Next item

  End Sub

  Public Function ReadInputs(dinp() As Boolean, aninp() As Short, temp() As Short) As Boolean Implements ACControlCode.ReadInputs
    ' TODO
  End Function

  Public Sub StartUp() Implements ACControlCode.StartUp
    ProgramIdleTimer.Start()
  End Sub

  Private Sub SetStatusColor(color As Color)
    StatusColor = color.ToArgb And 16777215
  End Sub

  Public ReadOnly Property Status() As String
    Get
      If String.IsNullOrEmpty(Parent.Signal) Then
        Return "" ' TODO CheckWeigh.Status
      Else
        Return Parent.Signal
      End If

      If Not Parent.IsProgramRunning Then
        Return "Machine Idle: " & ProgramIdleTimer.ToString
      End If

      Return Nothing
    End Get
  End Property

End Class
