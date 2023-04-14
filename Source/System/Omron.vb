Namespace Ports
  Public Class Omron
    Private state_ As State, asyncResult_ As IAsyncResult, result_ As Result   ' valid when state_ = Complete
    Private Structure WorkData
      Dim UnitNumber As Integer, Address As String, IsRead As Boolean ' , Count As Integer
    End Structure
    Private work_ As WorkData
    ReadOnly writeOptimisation_ As New WriteOptimisation

    ReadOnly stm_ As Stream
    Private rx_() As Byte
    Private oks_, faults_, hwFaults_ As Integer

    Private Enum State
      Idle
      TX
      RX
      Complete
    End Enum

    Public Enum Result
      Busy
      OK
      Fault
      HwFault
    End Enum

    Sub New(stm As System.IO.Stream)
      stm.ReadTimeout = 300
      stm_ = New Stream(stm)
    End Sub

    Friend ReadOnly Property BaseStream() As Stream
      Get
        Return stm_
      End Get
    End Property

    ' The first character of the address corresponds to a letter to use in the tx request
    Private Shared Function GetLetter(address As String) As Char
      Select Case address.Chars(0)
        Case "I"c : Return "R"c     'RR = CIO Area
        Case "H"c : Return "H"c     'RH = HR Area
        Case "A"c : Return "J"c     'RJ = AR Area
        Case "L"c : Return "L"c     'RL = LR Area
        Case "C"c : Return "C"c     'RC = Timer / Counter 
        Case Else : Return "D"c     'RD = DM Area
      End Select
    End Function

    Private Shared Function HexToInt32(x As Byte) As Integer
      If x >= 48 AndAlso x <= 57 Then Return x - 48
      If x >= 65 AndAlso x <= 70 Then Return x - 65 + 10
      Return 0
    End Function
    Private Shared Function Hex2ToInt32(str() As Byte, startIndex As Integer) As Integer
      Return HexToInt32(str(startIndex + 0)) * 16 + HexToInt32(str(startIndex + 1))
    End Function
    Private Shared Function Hex4ToInt32(str() As Byte, startIndex As Integer) As Integer
      Return ((HexToInt32(str(startIndex + 0)) * 16 + HexToInt32(str(startIndex + 1))) * 16 + _
               HexToInt32(str(startIndex + 2))) * 16 + HexToInt32(str(startIndex + 3))
    End Function

    Private Shared Function AppendOmronFCS(str As String) As String
      Dim cs As Integer
      For Each ch As Char In str
        cs = (cs Xor Convert.ToInt32(ch)) And 255
      Next ch
      Return str & cs.ToString("X2", System.Globalization.CultureInfo.InvariantCulture) & "*" & Convert.ToChar(13)
    End Function

    Private Sub StartRequest(tx As String)
      tx = AppendOmronFCS(tx)  ' add the FCS    
      state_ = State.TX : stm_.Flush()
      ' Transmit as ascii bytes
      asyncResult_ = stm_.BeginWrite(System.Text.ASCIIEncoding.ASCII.GetBytes(tx), 0, tx.Length, Nothing, Nothing)
    End Sub

    Function SetToMonitorMode(unitNumber As Integer) As Result
      If state_ = State.Idle Then
        With work_ : .UnitNumber = unitNumber : .Address = "SC" : End With
        With New System.Text.StringBuilder
          .Append("@"c)
          .Append(unitNumber.ToString("00", System.Globalization.CultureInfo.InvariantCulture))
          .Append("SC02")
          rx_ = New Byte(11 - 1) {}
          StartRequest(.ToString)
        End With
      End If

      ' See if we're finished
      RunStateMachine()

      ' Maybe it's someone else's job
      If state_ <> State.Complete OrElse work_.UnitNumber <> unitNumber _
         OrElse work_.Address <> "SC" Then Return Result.Busy ' not yet 
      state_ = State.Idle ' the end of this job
      Return result_
    End Function

    Function Read(unitNumber As Integer, address As String, values As Array) As Result
      ' Start a completely new task
      If state_ = State.Idle Then
        With work_
          .UnitNumber = unitNumber : .Address = address : .IsRead = True
        End With
        Dim isBits As Boolean = (values.GetType.GetElementType Is GetType(Boolean)), _
            uShortCount As Integer = values.Length - 1  ' one less because we ignore element 0
        If isBits Then uShortCount = (uShortCount + 15) \ 16

        With New System.Text.StringBuilder
          .Append("@"c)
          .Append(unitNumber.ToString("00", System.Globalization.CultureInfo.InvariantCulture))
          .Append("R"c)
          .Append(GetLetter(address))
          .Append(Integer.Parse(address.Substring(2), Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture) _
                                .ToString("0000", System.Globalization.CultureInfo.InvariantCulture))
          .Append(uShortCount.ToString("0000", System.Globalization.CultureInfo.InvariantCulture))
          rx_ = New Byte(11 + 4 * uShortCount - 1) {}
          StartRequest(.ToString)
        End With
      End If

      ' See if we're finished
      RunStateMachine()

      ' Maybe it's someone else's job
      If state_ <> State.Complete OrElse work_.UnitNumber <> unitNumber _
         OrElse work_.Address <> address OrElse work_.IsRead <> True Then Return Result.Busy ' not yet 
      state_ = State.Idle ' the end of this job

      ' Decode the values returned
      If result_ = Result.OK Then
        Dim isBits As Boolean = (values.GetType.GetElementType Is GetType(Boolean)), _
            uShortCount As Integer = values.Length - 1  ' one less because we ignore element 0
        If isBits Then uShortCount = (uShortCount + 15) \ 16

        ' Convert the 4 byte hex received to 16 bits
        Dim data() As Integer = New Integer(uShortCount - 1) {}
        For i As Integer = 0 To uShortCount - 1
          data(i) = Hex4ToInt32(rx_, 7 + i * 4)
        Next i
        ' Store the results back in the array we were given
        If isBits Then
          Dim bools() As Boolean = DirectCast(values, Boolean())
          For i As Integer = 0 To values.Length - 2
            bools(1 + i) = ((data(i \ 16) And (1 << (i And 15))) <> 0)
          Next i
        Else
          Dim shorts() As Short = DirectCast(values, Short())
          For i = 0 To values.Length - 2
            shorts(1 + i) = CType(data(i), Short)
          Next i
        End If
      End If
      Return result_
    End Function

    Function Write(unitNumber As Integer, address As String, values As Array, writeMode As WriteMode) As Result
      If state_ = State.Idle Then
        ' Optionally, do write-optimisation, meaning we usually do not write the same values to the same
        ' registers in the same slave.
        'ML 9/9/19 If writeMode = Ports.WriteMode.Optimised AndAlso writeOptimisation_.RecentlyWritten(values, unitNumber, address) Then Return Result.OK

        With work_
          .UnitNumber = unitNumber : .Address = address : .IsRead = False
        End With

        Dim isBits As Boolean = (values.GetType.GetElementType Is GetType(Boolean)), _
            uShortCount As Integer = values.Length - 1  ' one less because we ignore element 0
        If isBits Then uShortCount = (uShortCount + 15) \ 16

        ' Maybe squash the data as bits into words
        Dim data() As Integer = New Integer(uShortCount - 1) {}
        If isBits Then
          Dim bools() As Boolean = DirectCast(values, Boolean())
          For i As Integer = 0 To values.Length - 2
            If bools(1 + i) Then data(i \ 16) += (1 << (i And 15))
          Next i
        Else
          Array.Copy(values, 1, data, 0, values.Length - 1)
        End If

        ' Append the values as 4 byte hex
        With New System.Text.StringBuilder
          .Append("@"c)
          .Append(unitNumber.ToString("00", System.Globalization.CultureInfo.InvariantCulture))
          .Append("W"c)
          .Append(GetLetter(address))
          .Append(Integer.Parse(address.Substring(2), Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture) _
                                .ToString("0000", System.Globalization.CultureInfo.InvariantCulture))
          For i As Integer = 0 To uShortCount - 1
            .Append(data(i).ToString("X4", System.Globalization.CultureInfo.InvariantCulture))
          Next i
          rx_ = New Byte(11 - 1) {}
          StartRequest(.ToString)
        End With
      End If

      ' See if we're finished
      RunStateMachine()

      ' Maybe it's someone else's job - TODO: timeout if no-one comes for it for a long time
      If state_ <> State.Complete OrElse work_.UnitNumber <> unitNumber OrElse work_.Address <> address _
          OrElse work_.IsRead <> False Then Return Result.Busy ' not yet 
      state_ = State.Idle ' the end of this job
      Return result_
    End Function

    Private Sub RunStateMachine()
      Select Case state_
        Case State.TX
          ' Wait for the tx to complete
          If Not asyncResult_.IsCompleted Then Exit Sub
          stm_.EndWrite(asyncResult_)  ' tidy up

          ' Start reading
          asyncResult_ = stm_.BeginRead(rx_, 0, rx_.Length, Nothing, Nothing)
          state_ = State.RX : GoTo stateRx ' go straight on to the next state

        Case State.RX
stateRx:
          If Not asyncResult_.IsCompleted Then Exit Sub ' it'll be completed soon

          Dim rxCount As Integer = rx_.Length, red As Integer = stm_.EndRead(asyncResult_) : asyncResult_ = Nothing
          If red = -1 Then                     ' some hardware error
            SetResult(Result.HwFault)
          ElseIf red <> rxCount Then           ' not enough bytes ?
            SetResult(Result.Fault)
          ElseIf HexToInt32(rx_(5)) <> 0 Then  ' response code indicates an error
            SetResult(Result.Fault)
          Else
            ' TODO: check the received FCS
            SetResult(Result.OK)
          End If
          state_ = State.Complete
      End Select
    End Sub

    Friend ReadOnly Property OKs() As Integer
      Get
        Return oks_
      End Get
    End Property
    ReadOnly Property Faults() As Integer
      Get
        Return faults_
      End Get
    End Property
    ReadOnly Property HwFaults() As Integer
      Get
        Return hwFaults_
      End Get
    End Property
    Private Sub SetResult(value As Result)
      result_ = value
      Select Case value
        Case Result.OK : oks_ += 1
        Case Result.Fault : faults_ += 1
        Case Else : hwFaults_ += 1
      End Select
    End Sub
  End Class
End Namespace
