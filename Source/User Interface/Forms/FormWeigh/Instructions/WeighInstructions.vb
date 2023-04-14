Public Class WeighInstructions : Inherits UserControl

  Private flowPanel As FlowLayoutPanel
  Private currentStep As Label

  Private defaultWidth As Integer = 400
  Private defaultHeight As Integer = 200

  Public Enum EState
    Start
    Scan
    Tare
    Weigh
    Done
    Skip
    Abort
  End Enum

  Sub New()
    DoubleBuffered = True
    Size = New Size(defaultWidth, defaultHeight)

    SuspendLayout()
    AddControls()
    ResumeLayout(False)
  End Sub

  Private Sub AddControls()
    flowPanel = New FlowLayoutPanel
    With flowPanel
      .Dock = System.Windows.Forms.DockStyle.Fill
      .FlowDirection = System.Windows.Forms.FlowDirection.TopDown
      .WrapContents = False
    End With
    Controls.Add(flowPanel)
  End Sub


  Sub Start(product As String)

  End Sub

  Sub Run(state As EState, message As String)

  End Sub


  Private Sub AddInstruction(product As String)
    Dim newLabel = New ImageLabel
    With newLabel


    End With



  End Sub

End Class
