Partial Class ControlCode

  ' Flags for first code and IO scans
  Public FirstCodeScanDone As Boolean
  Public FirstIOScanDone As Boolean

  ' Hibernate
  Public SystemHibernate As Boolean
  Public SystemHibernateTimer As New Timer

  ' Program idle and run time
  Public ProgramIdleTime As Integer
  Public ProgramIdleTimer As New TimerUp
  Public ProgramRunTime As Integer
  Public ProgramRunTimer As New TimerUp

  ' Flash variables  - friend so they don't log in the histories
  Friend FlashSlow As Boolean
  Friend FlashFast As Boolean

  ' Pump and reel switch enable - public for debug so we can see the values
  Public SwitchEnable As Boolean
  Public SwitchEnableTimer As New Timer

  ' Plant Explorer host status color
  Public StatusColor As Integer





End Class
