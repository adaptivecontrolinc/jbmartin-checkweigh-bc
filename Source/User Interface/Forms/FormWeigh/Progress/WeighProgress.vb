Public Class WeighProgress : Inherits UserControl

  Private splitter As SplitContainer

  Private progressText As WeighProgressText
  Private progressBars As WeighProgressBars

  Private defaultWidth As Integer = 400
  Private defaultHeight As Integer = 200

  Sub New()
    DoubleBuffered = True
    Size = New Size(defaultWidth, defaultHeight)

    SuspendLayout()
    AddControls()
    ResumeLayout(False)
  End Sub

  Private Sub AddControls()
    ' Use a split container to space the controls evenly
    splitter = New SplitContainer
    With splitter
      .Dock = DockStyle.Fill
      .Orientation = Orientation.Horizontal
      .SplitterWidth = 1

      AddHandler .SizeChanged, AddressOf splitter_SizeChanged
    End With
    Controls.Add(splitter)

    progressText = New WeighProgressText With {.Dock = DockStyle.Fill}
    splitter.Panel1.Controls.Add(progressText)

    progressBars = New WeighProgressBars With {.Dock = DockStyle.Fill}
    splitter.Panel2.Controls.Add(progressBars)
  End Sub

  Private Sub splitter_SizeChanged(sender As Object, e As EventArgs)
    ' Always make splitter distance = 50%
    splitter.SplitterDistance = splitter.Height \ 2
  End Sub

End Class
