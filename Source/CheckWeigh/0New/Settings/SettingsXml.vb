Public Class SettingsXml

  Public Function GetSettings() As Settings
    Try
      ' The xml file should be in the application folder
      Dim appFolder As String = My.Application.Info.DirectoryPath
      Dim file = appFolder & "\Settings.xml"

      Dim newSettings As Settings = Nothing

      ' If we have no settings file just return the default (and save the file so we have something to edit the next time)
      If Not System.IO.File.Exists(file) Then
        newSettings = New Settings
        SaveToXml(file, newSettings)
      Else
        newSettings = GetSettingsFromXml(file)
      End If

      Return newSettings
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return Nothing
  End Function


  Private Function GetSettingsFromXml(file As String) As Settings
    Try
      ' Use serialization to create the settings class from the settings.xml file
      Dim sr As New System.IO.StreamReader(file)
      Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(Settings))

      Return CType(serializer.Deserialize(sr), Settings)
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return Nothing
  End Function


  Public Sub SaveToXml(file As String, settings As Settings)
    If settings Is Nothing Then Exit Sub

    ' Use xml serializer to write collection to 
    Dim sw As New System.IO.StreamWriter(file, False)
    Dim serializer As New System.Xml.Serialization.XmlSerializer(GetType(Settings))
    serializer.Serialize(sw, settings)
  End Sub

End Class
