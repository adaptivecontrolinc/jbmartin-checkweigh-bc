Imports Utilities.Sql

'Version 2022-01-21
Public Class CheckWeighManager : Inherits MarshalByRefObject

  ' For convenience
  Private parent As ACParent
  Private controlCode As ControlCode

  Private Const SqlWriteSleepTime As Integer = 50   'milliseconds

  Public Sub New(ByVal parent As ACParent, ByVal controlCode As ControlCode)

    Me.parent = parent
    Me.controlCode = controlCode
  End Sub

  Public Sub Start()
    'Prepare the background thread for the scheduler
    '  set the thread properties and start the thread
    With New System.Threading.Thread(AddressOf Run)
      .Name = "Manager"
      .Priority = Threading.ThreadPriority.BelowNormal
      .Start()
    End With
  End Sub

  Private Sub Run()
    With controlCode
      'True if this is the first time through the loop
      Static firstLoop As Boolean = True

      Try
        ' Startup - wait 10 seconds to make sure that local default database has been assembled, first
        If firstLoop Then Threading.Thread.Sleep(5000)

        'Run Startup Cleanup procedure
        '  Cleanup()

        'Make sure we have the correct program and program group setup
        CheckProgramExist()

      Catch ex As Exception
        parent.LogException(ex)
      End Try

      ' Use this to detect change of day
      Static dayCounter As Integer = 0

      ' Wait at least sixteen seconds before starting the loop 
      '    Batch Control seems to take a while to fully wake up so lets not overtax the dear
      Threading.Thread.Sleep(16000)

      ' Run this continually
      Do
        Try
          ' Run daily tasks here
          Dim dayOfYear = Date.Now.DayOfYear
          If dayCounter <> dayOfYear Then
            dayCounter = dayOfYear
            RunDailyTasks()
          End If

          ' Check Schedule and add a batch if necessary
          '  If .Parameters.RunBatches = 1 Then CheckSchedule()

        Catch ex As Exception
          'Catch exception so we don't kill the control system
          Threading.Thread.Sleep(20000)
        End Try

        ' Completed first loop
        firstLoop = False

        ' Sleep for a bit at the end of the loop
        Threading.Thread.Sleep((MinMax(.Parameters.NetworkSleepTime, 1000, 60000)))
      Loop
    End With
  End Sub

  Private Sub RunDailyTasks()
    Try
      ' No daily tasks ?
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Private Sub CheckSchedule()
    ' Make sure there is always one batch scheduled and unblocked 
    '   So we record histories
    Dim sql As String = ""
    Try
      '      sql = "SELECT * FROM Dyelots WHERE State Is Null"
      '      Dim dyelots = controlCode.Parent.DbGetDataTable(sql)

      '      If dyelots Is Nothing OrElse dyelots.Rows.Count <= 0 Then
      '        AddBatch()
      '      End If

    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Private Sub AddBatch()
    Dim sql As String = ""
    Try
      Dim dyelot As String
      If controlCode.Parent.IsProgramRunning Then
        ' Set to next day so dyelot = date batch run
        dyelot = Date.Now.AddDays(1).ToString("yyyyMMdd HHmm")
      Else
        ' Set to current day because no dyelot is running
        dyelot = Date.Now.ToString("yyyyMMdd HHmm")
      End If
      sql = "INSERT INTO Dyelots(Dyelot,ReDye,Machine,Program) VALUES('" & dyelot.ToString & "',0,'Local','1')"

      controlCode.Parent.DbExecute(sql)

    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Public Sub AddBatch(ByVal dyelot As String, ByVal stepNumber As String)
    Dim sql As String = ""
    Try
      If dyelot = "" Then
        AddBatch()
        Exit Sub
      End If

      sql = "INSERT INTO Dyelots(Dyelot,ReDye,Machine,Program) VALUES('" & dyelot.ToString & "-" & stepNumber & "',0,'Local','1')"
      controlCode.Parent.DbExecute(sql)

    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Private Sub CheckProgramExist()
    'Make sure there is a program 1 defined with the correct data
    Dim sql As String = Nothing
    Try
      'Make sure we have the 'CityWaterSystem' program group
      CheckProgramGroup()

      'First Test to see that Program 1 is installed for CW command - standard dispense
      CheckCommandCW()

    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Private Sub CheckProgramGroup()
    Dim sql As String = Nothing
    Try
      Dim dtProgramGroup As System.Data.DataTable
      Dim drProgramGroup As System.Data.DataRow
      sql = "SELECT * FROM ProgramGroups WHERE Name='CheckWeigh'"
      dtProgramGroup = parent.DbGetDataTable(sql)

      If dtProgramGroup IsNot Nothing Then
        drProgramGroup = dtProgramGroup.Rows(0)
      End If

      'If the program group does not exist insert it
      If drProgramGroup Is Nothing Then InsertProgramGroup()
    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Private Sub InsertProgramGroup()
    Dim sql As String = Nothing
    Try
      sql = "INSERT INTO ProgramGroups(Name) VALUES('CheckWeigh')"
      parent.DbExecute(sql)
    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Private Sub CheckCommandCW()
    Dim sql As String = Nothing
    Try
      Dim dtProgram As System.Data.DataTable
      Dim drProgram As System.Data.DataRow
      sql = "SELECT ProgramGroup,ProgramNumber,Name,Steps FROM Programs WHERE ProgramGroup='CheckWeigh' AND ProgramNumber=1"
      dtProgram = parent.DbGetDataTable(sql)

      If dtProgram IsNot Nothing Then
        If dtProgram.Rows.Count > 0 Then drProgram = dtProgram.Rows(0)
      End If

      If drProgram Is Nothing Then
        'No program exists so insert the steps
        InsertNewProgram("CheckWeigh", 1, "CW")
      Else
        If Not ((drProgram("Name").ToString = "CheckWeigh") And (drProgram("Steps").ToString = "CW")) Then
          'Program steps not correct - update with correct info
          UpdateProgram("CheckWeigh", 1, "CW")
        End If
      End If
    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Private Sub InsertNewProgram(ByVal programName As String, ByVal programNumber As Integer, ByVal programStep As String)
    Dim sql As String = Nothing
    Try
      sql = "INSERT INTO Programs(ProgramGroup,ProgramNumber,Name,Steps) VALUES('CheckWeigh', " & NullToZeroInteger(programNumber) & ",'" & NullToNothingString(programName) & "', '" & NullToNothingString(programStep) & "')"
      parent.DbExecute(sql)
    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Private Sub UpdateProgram(ByVal programName As String, ByVal programNumber As Integer, ByVal programStep As String)
    Dim sql As String = Nothing
    Try
      sql = "UPDATE Programs SET Name='" & NullToNothingString(programName) & "',Steps='" & NullToNothingString(programStep) & "' WHERE ProgramGroup='CheckWeigh' AND ProgramNumber=" & NullToZeroInteger(programNumber)
      parent.DbExecute(sql)
    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

End Class
