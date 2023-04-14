Public Class BalanceControl : Inherits UserControl

  Private balance As BalanceBase

  Private splitter As SplitContainer
  Private data As TextBox
  Private settings As PropertyGrid

  Sub New()
    DoubleBuffered = True
    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    splitter = New SplitContainer
    With splitter
      .Dock = DockStyle.Fill
      .Orientation = Orientation.Vertical

      .SplitterWidth = 1
      .SplitterDistance = Me.Width \ 2
    End With
    Controls.Add(splitter)

    data = New TextBox
    With data
      .Multiline = True
      .Dock = DockStyle.Fill
    End With
    splitter.Panel1.Controls.Add(data)

    settings = New PropertyGrid
    With settings
      .Dock = DockStyle.Fill
    End With
    splitter.Panel2.Controls.Add(settings)

    ResumeLayout(False)
  End Sub


  Sub Connect(balance As BalanceBase)
    Me.balance = balance
    Me.settings.SelectedObject = balance

    Requery()
  End Sub

  Sub Requery()
    If balance Is Nothing Then
      data.Text = Date.Now.ToString
    Else
      Dim text = data.Text
      data.Text = balance.Data & Environment.NewLine & text
    End If
  End Sub

End Class
