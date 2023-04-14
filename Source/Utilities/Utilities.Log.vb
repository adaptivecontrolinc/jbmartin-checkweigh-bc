Namespace Utilities

  Public NotInheritable Class Log

    Private Shared MaxLogFileSize As Integer = 8000000 '8MB ish
    Public Shared Property LogFileName As String = "Log.txt"
    Public Shared Property LogArchiveFileName As String = "LogArchive.txt"

    Public Shared Sub LogErrorShowMessage(ByVal message As String)

      LogError(message)
      MessageBox.Show(message, "Adaptive Control", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Shared Sub LogErrorShowMessage(ByVal message As String, ByVal detail As String)
      LogError(message, detail)
      MessageBox.Show(message, "Adaptive Control", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Shared Sub LogErrorShowMessage(ByVal err As Exception)
      LogError(err)
      MessageBox.Show(err.Message, "Adaptive Control", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Shared Sub LogErrorShowMessage(ByVal err As Exception, ByVal sql As String)
      LogError(err, sql)
      MessageBox.Show(err.Message, "Adaptive Control", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Shared Sub LogError(ByVal message As String)
      Try
        Dim applicationPath As String = My.Application.Info.DirectoryPath
        Dim logFilePath As String = applicationPath & "\" & LogFileName

        'Check log file size if it's too big archive the current one and start anew
        If MaxLogFileSizeExceeded(logFilePath) Then ArchiveLogFile(applicationPath, logFilePath)

        WriteToLogFile(logFilePath, message)
      Catch Ex As Exception
        'Ignore
      End Try
    End Sub

    Public Shared Sub LogError(ByVal message As String, ByVal detail As String)
      Try
        Dim applicationPath As String = My.Application.Info.DirectoryPath
        Dim logFilePath As String = applicationPath & "\" & LogFileName

        'Check log file size if it's too big archive the current one and start anew
        If MaxLogFileSizeExceeded(logFilePath) Then ArchiveLogFile(applicationPath, logFilePath)

        WriteToLogFile(logFilePath, message, detail)
      Catch Ex As Exception
        'Ignore
      End Try
    End Sub

    Public Shared Sub LogError(ByVal err As Exception)
      Try
        Dim applicationPath As String = My.Application.Info.DirectoryPath
        Dim logFilePath As String = applicationPath & "\" & LogFileName

        'Check log file size if it's too big archive the current one and start anew
        If MaxLogFileSizeExceeded(logFilePath) Then ArchiveLogFile(applicationPath, logFilePath)

        WriteToLogFile(logFilePath, err.Message, err.StackTrace)
      Catch Ex As Exception
        'Ignore
      End Try
    End Sub

    Public Shared Sub LogError(ByVal err As Exception, ByVal sql As String)
      Try
        Dim applicationPath As String = My.Application.Info.DirectoryPath
        Dim logFilePath As String = applicationPath & "\" & LogFileName

        'Check log file size if it's too big archive the current one and start anew
        If MaxLogFileSizeExceeded(logFilePath) Then ArchiveLogFile(applicationPath, logFilePath)

        WriteToLogFile(logFilePath, err.Message, err.StackTrace, sql)
      Catch Ex As Exception
        'Ignore
      End Try
    End Sub

    Public Shared Sub LogEvent(ByVal message As String)
      Try
        Dim applicationPath As String = My.Application.Info.DirectoryPath
        Dim logFilePath As String = applicationPath & "\" & LogFileName

        'Check log file size if it's too big archive the current one and start anew
        If MaxLogFileSizeExceeded(logFilePath) Then ArchiveLogFile(applicationPath, logFilePath)

        WriteToLogFile(logFilePath, message)
      Catch ex As Exception
        'Some code
      End Try
    End Sub

    Private Shared Sub WriteToLogFile(ByVal logFilePath As String, ByVal message As String)
      Try
        Using sw As New System.IO.StreamWriter(logFilePath, True)
          sw.WriteLine()
          sw.WriteLine()
          sw.WriteLine(Date.Now.ToString("yyyy-MM-dd hh:mm:ss"))
          sw.WriteLine("-------------------")
          sw.WriteLine(message)
        End Using
      Catch ex As Exception
        'Some code
      End Try
    End Sub

    Private Shared Sub WriteToLogFile(ByVal logFilePath As String, ByVal message As String, ByVal detail As String)
      Try
        Using sw As New System.IO.StreamWriter(logFilePath, True)
          sw.WriteLine()
          sw.WriteLine()
          sw.WriteLine(Date.Now.ToString("yyyy-MM-dd hh:mm:ss"))
          sw.WriteLine("-------------------")
          sw.WriteLine(message)
          sw.WriteLine(detail)
        End Using
      Catch ex As Exception
        'Some code
      End Try
    End Sub

    Private Shared Sub WriteToLogFile(ByVal logFilePath As String, ByVal message As String, ByVal detail As String, ByVal sql As String)
      Try
        Using sw As New System.IO.StreamWriter(logFilePath, True)
          sw.WriteLine()
          sw.WriteLine()
          sw.WriteLine(Date.Now.ToString("yyyy-MM-dd hh:mm:ss"))
          sw.WriteLine("-------------------")
          sw.WriteLine(message)
          sw.WriteLine(detail)
          sw.WriteLine(sql)
        End Using
      Catch ex As Exception
        'Some code
      End Try
    End Sub

    Private Shared Function MaxLogFileSizeExceeded(ByVal logFilePath As String) As Boolean
      'Check log file size if it exceeds 8MB (ish) then archive it and create a new one
      Try
        If System.IO.File.Exists(logFilePath) Then
          Dim fileInfo As System.IO.FileInfo = New System.IO.FileInfo(logFilePath)
          If fileInfo.Length > MaxLogFileSize Then Return True
        End If
      Catch ex As Exception
        'Some code
      End Try
      Return False
    End Function

    Private Shared Sub ArchiveLogFile(ByVal applicationPath As String, ByVal logFilePath As String)
      Try
        'Delete existing log file archive if there is one
        Dim logArchiveFilePath As String = applicationPath & LogArchiveFileName
        If System.IO.File.Exists(logArchiveFilePath) Then
          System.IO.File.Delete(logArchiveFilePath)
        End If

        'Rename current log file to "LogArchive.txt"
        If System.IO.File.Exists(logFilePath) Then
          My.Computer.FileSystem.RenameFile(logFilePath, LogArchiveFileName)
        End If

        'If everything has gone according to plan there will be no log file so create one by writing a new log message
        If Not System.IO.File.Exists(logFilePath) Then
          WriteToLogFile(logFilePath, "Log File Archived")
        End If
      Catch ex As Exception
        'Some code
      End Try
    End Sub

  End Class

End Namespace