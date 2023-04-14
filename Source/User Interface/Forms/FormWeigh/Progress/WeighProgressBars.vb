Public Class WeighProgressBars : Inherits UserControl

  Private splitter As SplitContainer

  Private barTarget As ProgressBar
  Private barCurrent As ProgressBar

  Private defaultWidth As Integer = 400
  Private defaultHeight As Integer = 100

  Sub New()
    DoubleBuffered = True
    Size = New Size(defaultWidth, defaultHeight)

    SuspendLayout()
    AddControls()
    ResumeLayout(False)
  End Sub

  Private Sub AddControls()
    ' Use a split container to space the bars evenly
    splitter = New SplitContainer
    With splitter
      .Dock = DockStyle.Fill
      .Orientation = Orientation.Horizontal
      .SplitterWidth = 1

      .Panel1.Padding = New Padding(4, 8, 4, 2)
      .Panel2.Padding = New Padding(4, 2, 4, 8)

      AddHandler .SizeChanged, AddressOf splitter_SizeChanged
    End With
    Controls.Add(splitter)

    barTarget = New ProgressBar
    With barTarget
      .Dock = DockStyle.Fill

      .Style = ProgressBarStyle.Continuous
      .Value = 75
    End With
    splitter.Panel1.Controls.Add(barTarget)

    barCurrent = New ProgressBar
    With barCurrent
      .Dock = DockStyle.Fill

      .Style = ProgressBarStyle.Continuous
      .Value = 75
    End With
    splitter.Panel2.Controls.Add(barCurrent)
  End Sub

  Private Sub splitter_SizeChanged(sender As Object, e As EventArgs)
    ' Always make splitter distance = 50%
    splitter.SplitterDistance = splitter.Height \ 2
  End Sub

End Class
