Namespace Utilities

  Public Class UserXml

    Property Document As Xml.XmlDocument

    Property Supervisor As String
    Property Expert As String
    Property Shutdown As String

    Sub Load()
      Dim fileName = My.Application.Info.DirectoryPath & "\User.xml"
      Dim document = New Xml.XmlDocument : document.Load(fileName)

      For Each rootElement As Xml.XmlElement In document.DocumentElement.ChildNodes
        If rootElement.Name = "MainView" Then
          For Each subElement As Xml.XmlElement In rootElement.ChildNodes
            Select Case subElement.Name
              Case "SupervisorPassword" : Supervisor = subElement.FirstChild.Value
              Case "ExpertPassword" : Expert = subElement.FirstChild.Value
              Case "ShutdownPassword" : Shutdown = subElement.FirstChild.Value
            End Select
          Next subElement
        End If
      Next rootElement
    End Sub

  End Class

End Namespace