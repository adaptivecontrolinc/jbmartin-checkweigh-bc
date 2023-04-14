Namespace Utilities

  Partial Public NotInheritable Class Text

    Public Shared Function SeparateWords(ByVal text As String) As String
      'String has to have at least two characters
      If text.Length <= 1 Then Return text

      'Loop through the string and insert a space in front of all capital letters except the first
      Dim test As String
      Dim textNew As String = text.Substring(0, 1)
      For i As Integer = 1 To text.Length - 1
        test = text.Substring(i, 1)
        'Check to see if the letter is upper case
        If test.ToUpper = test Then test = " " & test
        textNew &= test
      Next i
      Return textNew

    End Function


  End Class

End Namespace