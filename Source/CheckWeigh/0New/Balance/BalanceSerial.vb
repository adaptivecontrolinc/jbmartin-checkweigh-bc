Public Class BalanceSerial : Inherits BalanceBase

  Private threadRun As Threading.Thread
  Private threadCancel As Boolean

  Private serialPort As System.IO.Ports.SerialPort

  Sub New()
    Name = "Serial"
  End Sub

  Overrides Sub Start()
    ' Setup and start a background thread for the balance
    threadCancel = False
    threadRun = New System.Threading.Thread(AddressOf Run)
    With threadRun
      .Name = "BalanceSerial"
      .Priority = Threading.ThreadPriority.BelowNormal
      .Start()
    End With
  End Sub

  Overrides Sub Cancel()
    threadCancel = True
  End Sub

  Private Sub Run()
    OpenPort()
    Do
      Try
        Data = serialPort.ReadLine  ' Blocks until we have data - also strips out new line character
        DataLastRead = Date.Now
        UpdateWeight(Data)
      Catch ex As TimeoutException
        ClosePort()
        Threading.Thread.Sleep(2000)
        OpenPort()
      Catch ex As Exception
        RunError = ex.Message
      End Try
    Loop Until threadCancel
    ClosePort()
  End Sub

#If 0 Then
  ' Polartec ML version
  Private Sub UpdateWeight(data As String)
    Try
      Dim values = data.Split(" ".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
      If values Is Nothing OrElse values.Length <= 0 Then Exit Sub

      Dim reading As String = Nothing
      Dim prefix, suffix As String
      Dim isNegative As Boolean = False

      Select Case values.Length
        Case 1
          reading = values(0).Trim
        Case 2
          reading = values(0).Trim
          suffix = values(1).Trim
        Case 3
          prefix = values(0).Trim
          reading = values(1).Trim
          suffix = values(2).Trim
      End Select

      If Not String.IsNullOrEmpty(reading) Then
        'If prefix IsNot Nothing Then
        '  If prefix.Contains("-") Then
        '    isNegative = True
        '  End If
        'End If


        Dim tryDouble As Double
        If Double.TryParse(reading, tryDouble) Then
          Weight = tryDouble
          If isNegative Then Weight = Weight * -1
          WeightLastRead = Date.Now
        End If
      End If

    Catch ex As Exception
      Utilities.Log.LogError(ex.ToString, data)
    End Try
  End Sub

#End If


  '#If 0 Then
  Private Sub UpdateWeight(data As String)  ',NT-     58,g (linefeed)

    Try
      Dim endpos = data.LastIndexOf(DataString)
      Dim startPos = endpos - (DataLength)   'include search string

      Dim reading As String = Nothing

      If startPos > 0 Then
        reading = data.Substring(startPos, DataLength)
      End If

      If Not String.IsNullOrEmpty(reading) Then
        Dim tryDouble As Double
        If Double.TryParse(reading, tryDouble) Then
          Weight = tryDouble
          WeightLastRead = Date.Now
        End If
      End If

    Catch ex As Exception
      Utilities.Log.LogError(ex.ToString, data)
    End Try
  End Sub
  '#End If

  Private Sub OpenPort()
    ' e.g. "COM1,19200,N,8,1" or "COM1,19200,N,8,1,13" to terminate line with Carriage Return
    Try
      Dim parameters = Connection.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
      If parameters Is Nothing OrElse parameters.Length < 5 Then Exit Sub

      ' Initialize serial port if necessary
      If serialPort Is Nothing Then
        serialPort = New System.IO.Ports.SerialPort
      Else
        serialPort.Close()
      End If

      ' Set port parameters and open the port
      Utilities.Log.LogEvent("OpenPort for " & Name & ": " & Connection)
      With serialPort
        .PortName = parameters(0).Trim
        .BaudRate = Integer.Parse(parameters(1).Trim)
        .Parity = GetParity(parameters(2).Trim)
        .DataBits = Integer.Parse(parameters(3).Trim)
        .StopBits = GetStopBits(parameters(4).Trim)
        .Handshake = System.IO.Ports.Handshake.None
        .ReadTimeout = 2000

        ' Optionally set new line character
        If parameters.GetUpperBound(0) >= 5 Then
          Dim terminator = Integer.Parse(parameters(5).Trim)
          .NewLine = Chr(terminator).ToString
        End If

        .Open()
      End With

    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Private Sub ClosePort()
    If serialPort IsNot Nothing Then serialPort.Close()
  End Sub

  Private Function GetParity(value As String) As System.IO.Ports.Parity
    If value.ToUpper = "E" Then Return System.IO.Ports.Parity.Even
    If value.ToUpper = "O" Then Return System.IO.Ports.Parity.Odd
    If value.ToUpper = "M" Then Return System.IO.Ports.Parity.Mark
    If value.ToUpper = "S" Then Return System.IO.Ports.Parity.Space

    Return System.IO.Ports.Parity.None
  End Function

  Private Function GetStopBits(value As String) As System.IO.Ports.StopBits
    Dim stopBits As Double
    If Double.TryParse(value, stopBits) Then
      If stopBits = 1 Then Return System.IO.Ports.StopBits.One
      If stopBits = 1.5 Then Return System.IO.Ports.StopBits.OnePointFive
      If stopBits = 2 Then Return System.IO.Ports.StopBits.Two
    End If

    Return System.IO.Ports.StopBits.None
  End Function

End Class

