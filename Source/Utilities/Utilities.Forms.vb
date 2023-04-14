Namespace Utilities

  Partial Public NotInheritable Class Forms

    Public Shared Function ShowDataError(message As String) As Boolean
      MessageBox.Show(Translations.Translate(message), "Adaptive", MessageBoxButtons.OK)
      Return False
    End Function

    Public Shared Function ShowWarning(warning As String) As DialogResult
      Return MessageBox.Show(Translations.Translate(warning), "Adaptive", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Function

    Public Shared Function ShowQuestion(question As String) As DialogResult
      Return MessageBox.Show(Translations.Translate(question), "Adaptive", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
    End Function


    Public Shared Function ErrorMessage(message As String) As Boolean
      Return ErrorMessage(message, "Adaptive")
    End Function

    Public Shared Function ErrorMessage(message As String, caption As String) As Boolean
      MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
      Return False
    End Function

    Public Shared Function FontInstalled(fontName As String) As Boolean
      Dim installedFonts = FontFamily.Families()
      For Each Font In installedFonts
        If Font.Name.ToLower = fontName.ToLower Then Return True
      Next
      Return False
    End Function

  End Class

End Namespace
