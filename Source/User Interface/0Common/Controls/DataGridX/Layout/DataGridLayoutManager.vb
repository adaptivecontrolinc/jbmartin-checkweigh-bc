Public Class DataGridLayoutManager

  ' Use the xml serializer to save the DataGridLayout to an xml file
  Function SaveLayoutToFile(ByRef layout As DataGridLayout, file As String) As Boolean
    Try
      If layout Is Nothing OrElse file Is Nothing Then Return False

      ' Use xml serializer to write collection to 
      Dim sw As New System.IO.StreamWriter(file, False)
      Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(DataGridLayout))
      writer.Serialize(sw, layout)

      Return True
    Catch ex As Exception
      ' Ignore errors
    End Try
    Return False
  End Function

  ' Use the xml serializer to update a DataGridLayout from an xml file
  Function LoadLayoutFromFile(ByRef layout As DataGridLayout, file As String) As Boolean
    Try
      If layout Is Nothing OrElse file Is Nothing Then Return False
      If Not System.IO.File.Exists(file) Then Return False

      Using sr As New System.IO.StreamReader(file)
        Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(DataGridLayout))

        layout = CType(reader.Deserialize(sr), DataGridLayout)
      End Using
      Return True
    Catch ex As Exception
      ' Ignore errors
    End Try
    Return False
  End Function

  ' Use the xml serializer to create a DataGridLayout from an xml file
  Function GetLayoutFromFile(file As String) As DataGridLayout
    Try
      If file Is Nothing Then Return Nothing
      If Not System.IO.File.Exists(file) Then Return Nothing

      Dim gridLayout As DataGridLayout = Nothing
      Using sr As New System.IO.StreamReader(file)
        Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(DataGridLayout))

        gridLayout = CType(reader.Deserialize(sr), DataGridLayout)
      End Using
      Return gridLayout
    Catch ex As Exception
      ' Ignore errors
    End Try
    Return Nothing
  End Function

End Class
