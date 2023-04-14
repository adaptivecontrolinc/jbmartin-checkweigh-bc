Namespace Utilities

  Public NotInheritable Class File

    'Return just the path from the full file name (path + file)
    Public Shared Function GetPath(file As String) As String
      Try
        Dim lastBackSlash As Integer = file.LastIndexOf("\")
        Dim path As String = file.Substring(0, lastBackSlash)

        Return path
      Catch ex As Exception
        'Ignore errors
      End Try
      Return file
    End Function
  End Class

End Namespace
