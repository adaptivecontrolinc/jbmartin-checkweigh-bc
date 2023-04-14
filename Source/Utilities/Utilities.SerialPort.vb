
Namespace Utilities
  Partial Public Class SerialPort

    ' ComSettings Format = "COM1,9600,N,8,1"

    Public Shared Function ComPort(comSettings As String) As String
      Try
        Dim settings = comSettings.Split(",".ToCharArray)
        If settings.Length = 5 Then Return settings(0)
      Catch ex As Exception
        ' Ignore errors
      End Try
      Return "COM1" ' default
    End Function

    Public Shared Function BaudRate(comSettings As String) As Integer
      Try
        Dim settings = comSettings.Split(",".ToCharArray)
        If settings.Length = 5 Then
          Dim tryInteger As Integer
          If Integer.TryParse(settings(1), tryInteger) Then Return tryInteger
        End If
      Catch ex As Exception
        ' Ignore errors
      End Try
      Return 9600 ' default
    End Function

    Public Shared Function Parity(comSettings As String) As System.IO.Ports.Parity
      Try
        Dim settings = comSettings.Split(",".ToCharArray)
        If settings.Length = 5 Then
          Select Case settings(2).ToUpper.Trim
            Case "N" : Return System.IO.Ports.Parity.None
            Case "E" : Return System.IO.Ports.Parity.Even
            Case "O" : Return System.IO.Ports.Parity.Odd
            Case Else
              Return System.IO.Ports.Parity.None
          End Select
        End If
      Catch ex As Exception
        ' Ignore errors
      End Try
      Return System.IO.Ports.Parity.None
    End Function

    Public Shared Function DataBits(comSettings As String) As Integer
      Try
        Dim settings = comSettings.Split(",".ToCharArray)
        If settings.Length = 5 Then
          Dim tryInteger As Integer
          If Integer.TryParse(settings(3), tryInteger) Then Return tryInteger
        End If
      Catch ex As Exception
        ' Ignore errors
      End Try
      Return 8 ' default
    End Function

    Public Shared Function StopBits(comSettings As String) As System.IO.Ports.StopBits
      Try
        Dim settings = comSettings.Split(",".ToCharArray)
        If settings.Length = 5 Then
          Select Case settings(4).ToUpper.Trim
            Case "0" : Return System.IO.Ports.StopBits.None
            Case "1" : Return System.IO.Ports.StopBits.One
            Case "1.5" : Return System.IO.Ports.StopBits.OnePointFive
            Case "2" : Return System.IO.Ports.StopBits.Two
            Case Else
              Return System.IO.Ports.StopBits.One
          End Select
        End If
      Catch ex As Exception
        ' Ignore errors
      End Try
      Return System.IO.Ports.StopBits.One
    End Function

    Public Shared Function PortSettings(ByVal name As String, ByVal baudRate As Integer,
                                      ByVal parity As System.IO.Ports.Parity, ByVal dataBits As Integer,
                                      ByVal stopBits As System.IO.Ports.StopBits) As String
      Try
        Dim settings As String = name & "," & baudRate.ToString

        'Add Parity Setting
        Select Case parity
          Case System.IO.Ports.Parity.None
            settings += "," & "N"
          Case System.IO.Ports.Parity.Even
            settings += "," & "E"
          Case System.IO.Ports.Parity.Odd
            settings += "," & "O"
          Case Else
            settings += "," & "N"
        End Select

        'Add DataBits setting
        settings += "," & dataBits.ToString

        'Add StopBit setting
        Select Case stopBits
          Case System.IO.Ports.StopBits.None
            settings += "," & "0"
          Case System.IO.Ports.StopBits.One
            settings += "," & "1"
          Case System.IO.Ports.StopBits.OnePointFive
            settings += "," & "1.5"
          Case System.IO.Ports.StopBits.Two
            settings += "," & "2"
          Case Else
            settings += "," & "0"
        End Select


        'Final Return Value
        Return settings

      Catch ex As Exception
        ' Ignore errors
      End Try
      Return ""
    End Function

  End Class

End Namespace
