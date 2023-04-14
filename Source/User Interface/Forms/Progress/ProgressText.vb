Imports Utilities.Weight

Public Class ProgressText
  Inherits System.Windows.Forms.UserControl

  Public CurrentValue As Double
  Public TargetValue As Double
  Public MaxError As Double

  Property DisplayUnits As EBalanceUnits = EBalanceUnits.Grams
  Property DisplayFormat As String = "#0"

  Public ColorOk As Color = Color.Green
  Public ColorLow As Color = Color.Blue
  Public ColorHigh As Color = Color.Red
  Public ColorBorder As Color = Color.DimGray

  Public Sub InitializeControl()
    DoubleBuffered = True
    SetLanguage()
  End Sub

  Sub Clear()
    LabelCurrentValue.Text = Nothing
    LabelTargetValue.Text = Nothing
    LabelToleranceValue.Text = Nothing
  End Sub

  Sub Progress(ByVal currentGrams As Double, ByVal targetGrams As Double, ByVal toleranceGrams As Double)

    Me.CurrentValue = currentGrams   ' These used for color change
    Me.TargetValue = targetGrams     '
    Me.MaxError = toleranceGrams

    LabelCurrentValue.Text = Utilities.Weight.FormatWeight(currentGrams, EBalanceUnits.Grams, DisplayUnits, DisplayFormat)
    LabelTargetValue.Text = Utilities.Weight.FormatWeight(targetGrams, EBalanceUnits.Grams, DisplayUnits, DisplayFormat)
    LabelToleranceValue.Text = Utilities.Weight.FormatWeight(toleranceGrams, EBalanceUnits.Grams, DisplayUnits, DisplayFormat)

    SetForeColor()
  End Sub

  Sub SetForeColor()
    Try
      'Decide which color to use for text
      Dim TextColor As Color
      Dim CurrentError As Double = TargetValue - CurrentValue

      Select Case CurrentError
        Case Is > MaxError : TextColor = ColorLow
        Case Is < -MaxError : TextColor = ColorHigh
        Case -MaxError To +MaxError : TextColor = ColorOk
        Case Else : TextColor = ColorBorder
      End Select
      If TargetValue = 0 Then TextColor = ColorBorder

      'Update foreground color
      Dim MyControl As Windows.Forms.Control
      For Each MyControl In Me.Controls : MyControl.ForeColor = TextColor : Next
    Catch Ex As Exception
      'Ignore Errors
    End Try
  End Sub

  Private Sub ProgressText_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
    SetForeColor()
  End Sub

  Private Sub ProgressText_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
    SetForeColor()
  End Sub

#Region " Language "

  Private LabelTargetTitleText As String
  Private LabelCurrentTitleText As String
  Private Sub SetLanguage()

    LabelTargetTitleText = "Target Weight:"
    LabelCurrentTitleText = "Current Weight:"

    LabelTargetTitle.Text = LabelTargetTitleText
    LabelCurrentTitle.Text = LabelCurrentTitleText
  End Sub
#End Region

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call
    InitializeControl()
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
  Friend WithEvents LabelToleranceValue As System.Windows.Forms.Label
  Friend WithEvents LabelTargetTitle As System.Windows.Forms.Label
  Friend WithEvents LabelTargetValue As System.Windows.Forms.Label
  Friend WithEvents LabelCurrentTitle As System.Windows.Forms.Label
  Friend WithEvents LabelCurrentValue As System.Windows.Forms.Label
  Friend WithEvents LabelMaxErrorTitle As System.Windows.Forms.Label

  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.LabelTargetTitle = New System.Windows.Forms.Label()
    Me.LabelTargetValue = New System.Windows.Forms.Label()

    Me.LabelCurrentTitle = New System.Windows.Forms.Label()
    Me.LabelCurrentValue = New System.Windows.Forms.Label()

    Me.LabelMaxErrorTitle = New System.Windows.Forms.Label()
    Me.LabelToleranceValue = New System.Windows.Forms.Label()
    Me.SuspendLayout()
    '
    'LabelTargetTitle
    '
    Me.LabelTargetTitle.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelTargetTitle.ForeColor = System.Drawing.Color.DimGray
    Me.LabelTargetTitle.Location = New System.Drawing.Point(0, 0)
    Me.LabelTargetTitle.Name = "LabelTargetTitle"
    Me.LabelTargetTitle.Size = New System.Drawing.Size(168, 32)
    Me.LabelTargetTitle.TabIndex = 5
    Me.LabelTargetTitle.Text = "Target Weight:"
    '
    'LabelTargetValue
    '
    Me.LabelTargetValue.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelTargetValue.ForeColor = System.Drawing.Color.DimGray
    Me.LabelTargetValue.Location = New System.Drawing.Point(168, 0)
    Me.LabelTargetValue.Name = "LabelTargetValue"
    Me.LabelTargetValue.Size = New System.Drawing.Size(160, 32)
    Me.LabelTargetValue.TabIndex = 6
    Me.LabelTargetValue.Text = "0.0g"
    '
    'LabelCurrentTitle
    '
    Me.LabelCurrentTitle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.LabelCurrentTitle.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelCurrentTitle.ForeColor = System.Drawing.Color.DimGray
    Me.LabelCurrentTitle.Location = New System.Drawing.Point(0, 32)
    Me.LabelCurrentTitle.Name = "LabelCurrentTitle"
    Me.LabelCurrentTitle.Size = New System.Drawing.Size(168, 32)
    Me.LabelCurrentTitle.TabIndex = 7
    Me.LabelCurrentTitle.Text = "Current Weight:"
    '
    'LabelCurrentValue
    '
    Me.LabelCurrentValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
    Me.LabelCurrentValue.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelCurrentValue.ForeColor = System.Drawing.Color.DimGray
    Me.LabelCurrentValue.Location = New System.Drawing.Point(168, 32)
    Me.LabelCurrentValue.Name = "LabelCurrentValue"
    Me.LabelCurrentValue.Size = New System.Drawing.Size(160, 32)
    Me.LabelCurrentValue.TabIndex = 8
    Me.LabelCurrentValue.Text = "0.0g"
    '
    'LabelMaxErrorTitle
    '
    Me.LabelMaxErrorTitle.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.LabelMaxErrorTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.LabelMaxErrorTitle.ForeColor = System.Drawing.Color.DimGray
    Me.LabelMaxErrorTitle.Location = New System.Drawing.Point(322, 21)
    Me.LabelMaxErrorTitle.Name = "LabelMaxErrorTitle"
    Me.LabelMaxErrorTitle.Size = New System.Drawing.Size(68, 17)
    Me.LabelMaxErrorTitle.TabIndex = 10
    Me.LabelMaxErrorTitle.Text = "Max Error"
    Me.LabelMaxErrorTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'LabelMaxErrorValue
    '
    Me.LabelToleranceValue.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.LabelToleranceValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
    Me.LabelToleranceValue.ForeColor = System.Drawing.Color.DimGray
    Me.LabelToleranceValue.Location = New System.Drawing.Point(322, 38)
    Me.LabelToleranceValue.Name = "LabelMaxErrorValue"
    Me.LabelToleranceValue.Size = New System.Drawing.Size(67, 16)
    Me.LabelToleranceValue.TabIndex = 9
    Me.LabelToleranceValue.Text = "0 g"
    Me.LabelToleranceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'ProgressText
    '
    Me.Controls.Add(Me.LabelMaxErrorTitle)
    Me.Controls.Add(Me.LabelToleranceValue)
    Me.Controls.Add(Me.LabelTargetTitle)
    Me.Controls.Add(Me.LabelTargetValue)
    Me.Controls.Add(Me.LabelCurrentTitle)
    Me.Controls.Add(Me.LabelCurrentValue)
    Me.ForeColor = System.Drawing.Color.DimGray
    Me.Name = "ProgressText"
    Me.Size = New System.Drawing.Size(400, 64)
    Me.ResumeLayout(False)

  End Sub
#End Region

End Class
