Public Class DataGridCustomizePanels : Inherits UserControl

  Private splitContainer1 As SplitContainer    'Left panel and container for middle panel and right panel
  Private splitContainer2 As SplitContainer    'Middle panel and right panel

  Property MiddleWidth As Integer = 120

  Sub New()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    splitContainer1 = New SplitContainer
    With splitContainer1
      .Dock = DockStyle.Fill
      .SplitterWidth = 1
      .SplitterDistance = Math.Max(CInt(Me.Width / 2) - MiddleWidth, 0)
    End With
    Controls.Add(splitContainer1)

    splitContainer2 = New SplitContainer
    With splitContainer2
      .Dock = DockStyle.Fill
      .SplitterWidth = 1
      .SplitterDistance = MiddleWidth
    End With
    splitContainer1.Panel2.Controls.Add(splitContainer2)

    PanelLeft.BorderStyle = BorderStyle.FixedSingle
    PanelMiddle.BorderStyle = BorderStyle.FixedSingle
    PanelRight.BorderStyle = BorderStyle.FixedSingle

    ResumeLayout(False)
  End Sub

  Private Sub ResizePanels()
    With splitContainer1
      .SplitterDistance = Math.Max(CInt(Me.Width / 2 - MiddleWidth / 2), 0)
    End With
    With splitContainer2
      .SplitterDistance = MiddleWidth
    End With
  End Sub

  Private Sub DataGridCustomizePanels_Resize(sender As Object, e As EventArgs) Handles Me.Resize
    ResizePanels()
  End Sub

  Public ReadOnly Property PanelLeft As SplitterPanel
    Get
      Return splitContainer1.Panel1
    End Get
  End Property

  Public ReadOnly Property PanelMiddle As SplitterPanel
    Get
      Return splitContainer2.Panel1
    End Get
  End Property

  Public ReadOnly Property PanelRight As SplitterPanel
    Get
      Return splitContainer2.Panel2
    End Get
  End Property

End Class
