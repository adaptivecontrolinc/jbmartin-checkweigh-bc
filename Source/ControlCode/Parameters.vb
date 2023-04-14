Imports System.ComponentModel

Public Class Parameters : Inherits MarshalByRefObject
  'Save a local reference for convenience
  Private controlCode As ControlCode

  Public Sub New(controlCode As ControlCode)
    Me.controlCode = controlCode
  End Sub





#Region " Standard Time "
  Private Const section_StandardTime As String = "Standard Time"

  <Parameter(0, 1000), Category(section_StandardTime),
  Description("Time, in minutes, to allow operators to complete a check weigh before setting delay conditions.")>
  Public StandardTimeCheckWeigh As Integer

#End Region

#Region " System "

  <Parameter(0, 9999), Category("System"), Description("Set system to demo mode to simulate dispensing (9387 = on)")>
  Public DemoMode As Integer

  <Parameter(0, 60000), Category("System"), Description("Sleep time between network sockets auto refresh in milliseconds")>
  Public NetworkSleepTime As Integer

  '  <Parameter(0, 1), Category("System"), Description("Run daily batches to log histories (1 = yes, 0 = no)")>
  '  Public RunBatches As Integer

#End Region



End Class
