Public Class ProgressBars
  Inherits System.Windows.Forms.UserControl

  Public MaxError As Double
  Public TargetValue As Double
  Public CurrentValue As Double

  Public ColorOk As Color = Color.Green
  Public ColorLow As Color = Color.Blue
  Public ColorHigh As Color = Color.Red
  Public ColorBorder As Color = Color.DimGray

  Public ProgressBarGap As Integer = 8
  Public ProgressBarLimitsActive As Boolean = True

  Sub Clear()
    Progress(0, 0, 0)
  End Sub

  Sub Progress(ByVal pCurrentValue As Double, ByVal pTargetValue As Double, ByVal pMaxError As Double)
    Try
      Dim NeedRedraw As Boolean = False
      If CurrentValue <> pCurrentValue Then
        CurrentValue = pCurrentValue
        NeedRedraw = True
      End If
      If TargetValue <> pTargetValue Then
        TargetValue = pTargetValue
        NeedRedraw = True
      End If
      If MaxError <> pMaxError Then
        MaxError = pMaxError
        NeedRedraw = True
      End If
      If NeedRedraw Then Redraw()
    Catch ex As Exception
      Utilities.Log.LogError("progress bars: " & ex.Message)
    End Try
  End Sub

  Public Sub Redraw()
    Try

      Dim BarWidth As Integer = Me.Width - 1
      Dim BarHeight As Integer = CType((Me.Height - ProgressBarGap) / 2, Integer)

      Dim TargetBarY As Integer = 0
      Dim CurrentBarY As Integer = Me.Height - BarHeight - 1

      Dim g As Graphics = Me.CreateGraphics

      'Clear graphics first
      g.Clear(Me.BackColor)

      'Draw the borders of each bar - separate them by ProgressBarGap pixels
      Dim PenBorder As New Pen(ColorBorder, 1)
      g.DrawRectangle(PenBorder, 0, TargetBarY, BarWidth, BarHeight)
      g.DrawRectangle(PenBorder, 0, CurrentBarY, BarWidth, BarHeight)

      'If we have no target value then just show empty bars
      If TargetValue = 0 Then Exit Sub

      'Decide which color to use for the bars
      Dim BarColor As Color
      Dim CurrentError As Double = TargetValue - CurrentValue
      Select Case CurrentError
        Case Is > MaxError : BarColor = ColorLow
        Case -MaxError To +MaxError : BarColor = ColorOk
        Case Is < -MaxError : BarColor = ColorHigh
        Case Else : BarColor = Me.BackColor
      End Select
      Dim Brush As New SolidBrush(BarColor)

      'Draw the inside of the target bar
      Dim TargetWidth As Integer = CType(BarWidth * 0.75, Integer)
      If TargetWidth > BarWidth - 1 Then TargetWidth = BarWidth - 1
      If TargetWidth < 0 Then TargetWidth = 0
      g.FillRectangle(Brush, 1, TargetBarY + 1, TargetWidth, BarHeight - 1)

      'Draw the inside of the current bar
      Dim CurrentWidth As Integer = CType(BarWidth * ((CurrentValue / TargetValue) * 0.75), Integer)
      If CurrentWidth > BarWidth - 1 Then CurrentWidth = BarWidth - 1
      If CurrentWidth < 0 Then CurrentWidth = 0
      g.FillRectangle(Brush, 1, CurrentBarY + 1, CurrentWidth, BarHeight - 1)


      'Draw max error limit bars 
      If ProgressBarLimitsActive Then
        Dim PenLimitLow As New Pen(ColorLow, 1)
        Dim PenLimitHigh As New Pen(ColorHigh, 1)
        Dim limitOffset As Double = 0
        If MaxError < TargetValue Then limitOffset = (MaxError / TargetValue) * TargetWidth
        Dim LimitLowX As Integer = CInt(TargetWidth - limitOffset)
        Dim LimitHighX As Integer = Math.Min(CInt(TargetWidth + limitOffset), BarWidth - 10)

        g.DrawRectangle(PenLimitLow, LimitLowX, CurrentBarY - 2, 2, BarHeight + 4)
        g.DrawRectangle(PenLimitHigh, LimitHighX, CurrentBarY - 2, 2, BarHeight + 4)
      End If

    Catch Ex As Exception
      'Ignore error
    End Try
  End Sub

  Private Sub ProgressBars_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
    Redraw()
  End Sub

  Private Sub ProgressBars_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
    Redraw()
  End Sub

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call

  End Sub

  'UserControl overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    '
    'ProgressBars
    '
    Me.Name = "ProgressBars"
    Me.Size = New System.Drawing.Size(400, 64)

  End Sub

#End Region

End Class
