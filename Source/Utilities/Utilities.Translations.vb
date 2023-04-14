Imports System.Globalization
Imports System.Xml

Namespace Utilities

  Partial Public Class Translations

    ' TODO copy translations table to a dictionary to improve performance

    Public Shared Property TranslationsTable() As System.Data.DataTable

    Public Shared Property Translations() As Dictionary(Of String, String)

    Public Shared Property CultureName() As String


    Public Shared Sub Load()
      ' Load the translations.xml file
      Try
        ' Get application and file path
        Dim appPath As String = My.Application.Info.DirectoryPath
        Dim filePath As String = appPath & "\translations.xml"

        'If the file exists load the translations
        If System.IO.File.Exists(filePath) Then
          'Read the settings into a dataset
          Dim data As New System.Data.DataSet
          data.ReadXml(filePath)

          'Use the first table in the dataset - should only be one table actually...
          If data.Tables IsNot Nothing AndAlso data.Tables.Count > 0 Then
            TranslationsTable = data.Tables(0)
          End If
        End If

        ' Set Culture Name to OS culture name if not already set
        If CultureName Is Nothing Then CultureName = CultureInfo.CurrentCulture.Name

        ' TODO - load relevant translations based on currrent culture into a dictionary for faster searching


      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub


    Public Shared Function Translate(ByVal text As String) As String
      Try
        ' Export text to translate - use this to generate a list of strings to translate
        'ExportText(text)  

        ' Make sure we have loaded the translations file
        If TranslationsTable Is Nothing Then Load()

        ' If there are still no translations then quit
        If TranslationsTable Is Nothing OrElse TranslationsTable.Rows.Count <= 0 Then Return text

        'Make sure we have translations for this culture
        If Not TranslationsTable.Columns.Contains(CultureName) Then Return text

        'Remove any leading or trailing spaces before trying to match the string
        Dim textToMatch As String = text.Trim

        'Loop through the translations table and return the first match
        For Each row As System.Data.DataRow In TranslationsTable.Rows
          If row("text").ToString.ToLower = textToMatch.ToLower Then
            If Not row.IsNull(CultureName) Then Return row(CultureName).ToString
          End If
        Next

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      'If no match or an error return original text
      Return text
    End Function


    Public Shared Sub Translate(ByVal control As Windows.Forms.Control)
      Try
        'Translate the text property of the control
        control.Text = Translate(control.Text)

        'Loop through all the controls in this control and translate the text property of appropriate objects
        For Each childControl As Windows.Forms.Control In control.Controls
          Dim controlType = childControl.GetType
          If TranslateControl(controlType) Then
            Translate(childControl)
          ElseIf controlType Is GetType(Windows.Forms.TabControl) Then
            For Each page As Windows.Forms.TabPage In DirectCast(childControl, Windows.Forms.TabControl).TabPages
              Translate(page)
            Next page
          ElseIf controlType Is GetType(Windows.Forms.ToolStrip) Then
            For Each item As ToolStripItem In DirectCast(childControl, Windows.Forms.ToolStrip).Items
              item.Text = Translate(item.Text)
            Next item
          End If
        Next childControl
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub


    Private Shared Function TranslateControl(ByVal controlType As System.Type) As Boolean
      If controlType Is GetType(Windows.Forms.Label) OrElse controlType Is GetType(Windows.Forms.CheckBox) OrElse controlType Is GetType(Windows.Forms.GroupBox) _
       OrElse controlType Is GetType(Windows.Forms.Button) OrElse controlType Is GetType(Windows.Forms.RadioButton) Then
        Return True
      End If
      Return False
    End Function

    Private Shared Sub ExportText(text As String)
      Dim exportFilePath As String = My.Application.Info.DirectoryPath & "\TranslationExport.txt"
      Using sw As New System.IO.StreamWriter(exportFilePath, True)
        sw.WriteLine(text)
      End Using
    End Sub

  End Class

End Namespace

