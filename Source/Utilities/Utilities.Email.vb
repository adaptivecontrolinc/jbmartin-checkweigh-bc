Namespace Utilities

  Public Class Email

    Private Shared smtpHost_ As String
    Public Shared Property smtpHost() As String
      Get
        Return smtpHost_
      End Get
      Set(ByVal value As String)
        smtpHost_ = value
      End Set
    End Property

    Private Shared from_ As String
    Public Shared Property From() As String
      Get
        Return from_
      End Get
      Set(ByVal value As String)
        from_ = value
      End Set
    End Property

    Private Shared subjectSave As String
    Private Shared messageSave As String

    Public Shared Sub SendSync(ByVal subject As String, ByVal message As String)
      Try
        subjectSave = subject
        messageSave = message

        SendEmail()
      Catch ex As Exception
        Utilities.Log.LogError("Utilities.Email.SendSync:  " & ex.Message)
      End Try
    End Sub

    Public Shared Sub SendAsync(ByVal subject As String, ByVal message As String)
      Try
        subjectSave = subject
        messageSave = message

        Dim backgroundThread As New System.Threading.Thread(AddressOf SendEmail)
        backgroundThread.Start()
      Catch ex As Exception
        Utilities.Log.LogError("Utilities.Email.SendAysnc:  " & ex.Message)
      End Try
    End Sub

    Private Shared Sub SendEmail()
      Try
        Dim email As New System.Net.Mail.MailMessage
        With email
          .From = New System.Net.Mail.MailAddress(Utilities.Email.From)
          .To.Add("email@adaptivecontrol.com")
          '.To.Add("acmichaellynch@adaptivecontrol.com")
          '.CC.Add("bryan.evans@colaik.com")
          '.CC.Add("iceman.betten@colaik.com")
          .Subject = subjectSave
          .Body = messageSave
        End With

        Dim smtp As New System.Net.Mail.SmtpClient
        With smtp
          .Host = Utilities.Email.smtpHost
          .Send(email)
        End With
      Catch ex As Exception
        Utilities.Log.LogError("Utilities.Email.SendEmail:  " & ex.Message)
      End Try

    End Sub

  End Class

End Namespace