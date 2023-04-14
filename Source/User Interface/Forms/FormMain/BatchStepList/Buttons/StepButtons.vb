Public Class StepButtons : Inherits UserControl
  Private controlCode As ControlCode

  Public Event ButtonClick(sender As Object, button As Button)

  Private buttonWeigh As New Button With {.Name = "Main", .Text = "Weigh Step"}
  Private buttonReturn As New Button With {.Name = "Return", .Text = "Return"}
  Private buttonRefresh As New Button With {.Name = "Refresh", .Text = "Refresh"}

  Private buttonSettings As New Button With {.Name = "Settings", .Text = "Settings"}
  Private buttonBalance As New Button With {.Name = "Balance", .Text = "Balance"}
  Private buttonGrid As New Button With {.Name = "Grid", .Text = "Step List"}
  Private buttonAbout As New Button With {.Name = "About", .Text = "About"}
  Private buttonExit As New Button With {.Name = "Exit", .Text = "Exit"}

  Private defaultWidth As Integer = 180
  Private defaultHeight As Integer = 480

  Sub New(ByVal controlCode As ControlCode)
    Me.controlCode = controlCode

    DoubleBuffered = True
    Size = New Size(defaultWidth, defaultHeight)

    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    AddButtonMain(buttonWeigh, My.Resources.Scale24x24)
    AddButtonBottom(buttonReturn, My.Resources.ArrowLeft24x24)
    AddButtonBottom(buttonRefresh, My.Resources.Refresh24x24)

    With controlCode
      If .Settings.ShowGridButton Then AddButtonBottom(buttonGrid, My.Resources.TableProperties24x24)
      If .Settings.ShowBalanceButton Then AddButtonBottom(buttonBalance, My.Resources.ScaleProperties24x24)
      If .Settings.ShowSettingsButton Then AddButtonBottom(buttonSettings, My.Resources.Gear24x24)
      If .Settings.ShowAboutButton Then AddButtonBottom(buttonAbout, My.Resources.Info24x24)
      If .Settings.ShowExitButton Then AddButtonBottom(buttonExit, Nothing)
    End With

    ResumeLayout(False)
  End Sub

  Private Sub AddButtonMain(button As Button, image As Image)
    With button
      .Font = New Font("Tahoma", 12)
      .ForeColor = Color.FromArgb(32, 72, 144)
      .Dock = DockStyle.Top
      .Height = 72
      .Image = image
      .ImageAlign = ContentAlignment.MiddleLeft
      AddHandler .Click, AddressOf Buttons_ButtonClick
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
      AddHandler .Click, AddressOf Buttons_ButtonClick
    End With
    Controls.Add(button)
  End Sub

  Private Sub Buttons_ButtonClick(sender As Object, e As EventArgs)
    Dim button = TryCast(sender, Button)
    If button IsNot Nothing Then RaiseEvent ButtonClick(Me, button)
  End Sub

End Class


