Public Class ControlTextBox
  Private fillMessage As String

  Private Sub ControlTextBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Try

    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Public Sub AddUpdate(ByVal message As String)
    Dim maxLength As Integer = 8192
    Try

      Dim messageLog As String
      messageLog = TextBox1.Text
      fillMessage = message & "   " & FormatDateTime(Now, DateFormat.GeneralDate) & vbCrLf & messageLog

      If fillMessage.Length >= maxLength Then
        fillMessage = fillMessage.Substring(1, 8192)
      End If

      TextBox1.Text = fillMessage

    Catch ex As Exception
      Utilities.Log.LogError(ex, message)
    End Try
  End Sub

  Public Sub AddRead(ByVal message As String)
    Dim maxLength As Integer = 8192
    Try

      Dim messageLog As String
      messageLog = TextBox1.Text
      fillMessage = message & vbCrLf & messageLog

      If fillMessage.Length >= maxLength Then
        fillMessage = fillMessage.Substring(1, 8192)
      End If

      TextBox1.Text = fillMessage

    Catch ex As Exception
      Utilities.Log.LogError(ex, message)
    End Try
  End Sub

  Public Sub ClearText()
    TextBox1.Clear()
  End Sub

End Class