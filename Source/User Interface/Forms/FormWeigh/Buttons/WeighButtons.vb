Public Class WeighButtons : Inherits UserControl
  Public Event ButtonPress(sender As Object, button As Button)

  Private buttonDone As New Button With {.Name = "Done", .Text = "Done"}

  Private buttonNextProduct As New Button With {.Name = "NextProduct", .Text = "Next Product"}
  Private buttonPartialWeigh As New Button With {.Name = "PartialWeigh", .Text = "PartialWeigh"}
  Private buttonSkipProduct As New Button With {.Name = "SkipProduct", .Text = "Skip Product"}
  Private buttonNoBarcode As New Button With {.Name = "NoBarcode", .Text = "No Barcode"}
  Private buttonAbort As New Button With {.Name = "Abort", .Text = "Abort"}

  Private defaultWidth As Integer = 180
  Private defaultHeight As Integer = 480

  Sub New()
    DoubleBuffered = True
    Size = New Size(defaultWidth, defaultHeight)

    AddControls()
  End Sub

  Private Sub AddControls()

  End Sub


  Private Sub AddButtonMain(button As Button, image As Image)
    With button
      .Font = New Font("Tahoma", 12)
      .ForeColor = Color.FromArgb(32, 72, 144)
      .Dock = DockStyle.Top
      .Height = 72
      .Image = image
      .ImageAlign = ContentAlignment.MiddleLeft
      AddHandler .Click, AddressOf Button_Click
    End With
    Controls.Add(button)
  End Sub

  Private Sub AddButtonTop(button As Button, image As Image)
    With button
      .Font = New Font("Tahoma", 10)
      .Dock = DockStyle.Top
      .Height = 40
      .Image = image
      .ImageAlign = ContentAlignment.MiddleLeft
      AddHandler .Click, AddressOf Button_Click
    End With
    Controls.Add(button)
  End Sub

  Private Sub AddButtonBottom(button As Button, image As Image)
    With button
      .Font = New Font("Tahoma", 10)
      .Dock = DockStyle.Bottom
      .Height = 40
      .Image = image
      .ImageAlign = ContentAlignment.MiddleLeft
      AddHandler .Click, AddressOf Button_Click
    End With
    Controls.Add(button)
  End Sub

  Private Sub Button_Click(sender As Object, e As EventArgs)
    Dim button = TryCast(sender, Button)
    If button IsNot Nothing Then RaiseEvent ButtonPress(Me, button)
  End Sub

End Class
