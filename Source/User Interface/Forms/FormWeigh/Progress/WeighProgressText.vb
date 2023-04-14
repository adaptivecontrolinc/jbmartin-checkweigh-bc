Public Class WeighProgressText : Inherits UserControl

  Private splitter As SplitContainer

  Private labelTarget As New Label
  Private labelCurrent As New Label
  Private labelTolerance As New Label

  Private valueTarget As New Label
  Private valueCurrent As New Label

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
    splitter = New SplitContainer
    With splitter
      .Dock = DockStyle.Fill
      .Orientation = Orientation.Horizontal
      .SplitterWidth = 1

      .Panel1.Padding = New Padding(0)
      .Panel2.Padding = New Padding(0, 0, 4, 0)

      AddHandler .SizeChanged, AddressOf splitter_SizeChanged
    End With
    Controls.Add(splitter)

    AddControlsPanel1()
    AddControlsPanel2()
  End Sub

  Private Sub AddControlsPanel1()
    With valueTarget
      .AutoSize = False
      .Dock = DockStyle.Left
      .Width = 128

      .Font = New Font("Tahoma", 24)
      .ForeColor = Color.Gray

      .Text = "0.0 g"
      .TextAlign = ContentAlignment.MiddleRight
    End With
    splitter.Panel1.Controls.Add(valueTarget)

    With labelTarget
      .AutoSize = False
      .Dock = DockStyle.Left
      .Width = 132

      .Font = New Font("Tahoma", 24)
      .ForeColor = Color.Gray

      .Text = "Target: "
      .TextAlign = ContentAlignment.MiddleLeft
    End With
    splitter.Panel1.Controls.Add(labelTarget)
  End Sub

  Private Sub AddControlsPanel2()
    With valueCurrent
      .AutoSize = False
      .Dock = DockStyle.Left
      .Width = 128

      .Font = New Font("Tahoma", 24)
      .ForeColor = Color.Gray

      .Text = "0.0 g"
      .TextAlign = ContentAlignment.MiddleRight
    End With
    splitter.Panel2.Controls.Add(valueCurrent)

    With labelCurrent
      .AutoSize = False
      .Dock = DockStyle.Left
      .Width = 132

      .Font = New Font("Tahoma", 24)
      .ForeColor = Color.Gray

      .Text = "Current: "
      .TextAlign = ContentAlignment.MiddleLeft
    End With
    splitter.Panel2.Controls.Add(labelCurrent)

    With labelTolerance
      .AutoSize = False
      .Dock = DockStyle.Right
      .Width = 48

      .Font = New Font("Tahoma", 8)
      .ForeColor = Color.Gray

      .Text = "+/- 5g"
      .TextAlign = ContentAlignment.MiddleRight
    End With
    splitter.Panel2.Controls.Add(labelTolerance)
  End Sub

  Private Sub splitter_SizeChanged(sender As Object, e As EventArgs)
    ' Always make splitter distance = 50%
    splitter.SplitterDistance = splitter.Height \ 2
  End Sub

End Class
