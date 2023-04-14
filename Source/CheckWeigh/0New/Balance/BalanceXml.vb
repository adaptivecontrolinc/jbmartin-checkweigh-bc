' Read the balance.xml file and load the configured defined scales
Imports System.IO
Public Class BalanceXml
  Private ReadOnly controlcode As ControlCode

  Public Sub New(ByVal controlCode As ControlCode)
    Me.controlcode = controlCode

  End Sub

  Public Sub LoadFromXml(balanceList As List(Of BalanceBase))
    If balanceList Is Nothing Then Exit Sub
    Try
      Dim appFolder = My.Application.Info.DirectoryPath

      Dim loadDemo As New LoadDemo(appFolder) : loadDemo.Load(balanceList)
      Dim loadSerial As New LoadSerial(appFolder) : loadSerial.Load(balanceList)
      Dim loadTcp As New LoadTcp(appFolder) : loadTcp.Load(balanceList)

      ' If no balances are defined create a default demo balance and save it to an xml file so we have a xml template for a balance
      If balanceList.Count <= 0 Then
        Dim newBalance = New BalanceDemo
        balanceList.Add(newBalance)
        SaveToXml(appFolder & "\BalanceDemo.xml", balanceList(0))
        controlcode.DemoMode = True
      End If

    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Private Sub SaveToXml(file As String, balance As BalanceBase)
    Try
      If balance Is Nothing Then Exit Sub

      ' Use xml serializer to save this instance of the balance demo object to a file
      Dim sw As New System.IO.StreamWriter(file, False)
      Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(BalanceDemo))
      writer.Serialize(sw, balance)
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Private Class LoadDemo
    Property Folder As String
    Sub New(folder As String)
      Me.Folder = folder
    End Sub

    Public Sub Load(balanceList As List(Of BalanceBase))
      If balanceList Is Nothing Then Exit Sub
      Try
        ' Get balance serial xml files
        Dim files = System.IO.Directory.GetFiles(Folder, "BalanceDemo*.xml")
        If files Is Nothing OrElse files.Length <= 0 Then Exit Sub

        ' Loop through the file list and load each balance
        For Each File In files
          Dim newBalance = LoadFromXml(File)
          If newBalance IsNot Nothing Then balanceList.Add(newBalance)
        Next

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    Private Function LoadFromXml(file As String) As BalanceDemo
      Try
        Dim sr As New System.IO.StreamReader(file)
        Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(BalanceDemo))

        Dim newBalance = CType(reader.Deserialize(sr), BalanceDemo)
        If newBalance IsNot Nothing Then Return newBalance

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return Nothing
    End Function
  End Class


  Private Class LoadSerial
    Property Folder As String
    Sub New(folder As String)
      Me.Folder = folder
    End Sub

    Public Sub Load(balanceList As List(Of BalanceBase))
      If balanceList Is Nothing Then Exit Sub
      Try
        ' Get balance serial xml files
        Dim files = System.IO.Directory.GetFiles(Folder, "BalanceSerial*.xml")
        If files Is Nothing OrElse files.Length <= 0 Then Exit Sub

        ' Loop through the file list and load each balance
        For Each file In files
          Dim newBalance = LoadFromXml(file)
          If newBalance IsNot Nothing Then balanceList.Add(newBalance)
        Next

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    Private Function LoadFromXml(file As String) As BalanceSerial
      Try
        Dim sr As New System.IO.StreamReader(file)
        Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(BalanceSerial))

        Dim newBalance = CType(reader.Deserialize(sr), BalanceSerial)
        If newBalance IsNot Nothing Then Return newBalance

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return Nothing
    End Function
  End Class


  Private Class LoadTcp
    Property Folder As String
    Sub New(folder As String)
      Me.Folder = folder
    End Sub

    Public Sub Load(balanceList As List(Of BalanceBase))
      If balanceList Is Nothing Then Exit Sub
      Try
        ' Get balance serial xml files
        Dim files = System.IO.Directory.GetFiles(Folder, "BalanceTcp*.xml")
        If files Is Nothing OrElse files.Length <= 0 Then Exit Sub

        ' Loop through the file list and load each balance
        For Each file In files
          Dim newBalance = LoadFromXml(file)
          If newBalance IsNot Nothing Then balanceList.Add(newBalance)
        Next

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    Private Function LoadFromXml(file As String) As BalanceTcp
      Try
        Dim sr As New System.IO.StreamReader(file)
        Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(BalanceTcp))

        Dim newBalance = CType(reader.Deserialize(sr), BalanceTcp)
        If newBalance IsNot Nothing Then Return newBalance

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return Nothing
    End Function
  End Class

End Class
