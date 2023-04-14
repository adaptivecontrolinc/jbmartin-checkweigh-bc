Public Class SqlDateBox : Inherits UserControl : Implements ISqlControl
  ' label above a text box
  Public Label As New Label
  Public TextBox As New TextBox

  Property LabelPosition As ELabelPosition = ELabelPosition.Top ' label on top or to the left
  Property LabelWidth As Integer  ' really only used if labelPosition = left

  Property SqlAutoFill As Boolean = True Implements ISqlControl.SqlAutoFill

  Property SqlColumnName As String Implements ISqlControl.SqlColumnName ' column name if mapping to a table

  Property SqlValueType As ESqlValueType = ESqlValueType.Date Implements ISqlControl.SqlValueType

  Property SqlValue() As Object Implements ISqlControl.SqlValue
    Get
      Dim tryDate As Date
      If Date.TryParse(Value, tryDate) Then Return tryDate
      Return DBNull.Value
    End Get
    Set(value As Object)
      If value Is DBNull.Value Then
        Me.Value = Nothing
      Else
        Me.Value = value.ToString
      End If
    End Set
  End Property

  Property Value As String
    Get
      Return TextBox.Text
    End Get
    Set(value As String)
      TextBox.Text = value
    End Set
  End Property

  Property [ReadOnly] As Boolean
    Get
      Return TextBox.ReadOnly
    End Get
    Set(value As Boolean)
      TextBox.ReadOnly = value
    End Set
  End Property

  Public Sub New()
    Label.Text = "TextBox"
    NewBase()
  End Sub

  Public Sub New(label As String)
    Me.Label.Text = label
    NewBase()
  End Sub

  Public Sub New(label As String, sqlColumnName As String)
    Me.Label.Text = label
    Me.SqlColumnName = sqlColumnName
    NewBase()
  End Sub

  Public Sub New(label As String, labelWidth As Integer, labelPosition As ELabelPosition)
    Me.Label.Text = label
    Me.LabelWidth = labelWidth
    Me.LabelPosition = labelPosition
    NewBase()
  End Sub

  Private Sub NewBase()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent
    Label.ForeColor = LabelForeColor

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Private Sub AddControls()
    If LabelPosition = ELabelPosition.Top Then AddControlsVertical()
    If LabelPosition = ELabelPosition.Left Then AddControlsHorizontal()
  End Sub

  Private Sub AddControlsVertical()
    SuspendLayout()
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    With Label
      .AutoSize = False
      .BackColor = Color.Transparent
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .Location = New Point(-3, 0)
      .Size = New Size(Me.Width + 3, 20)
      .TextAlign = ContentAlignment.MiddleLeft
    End With

    With TextBox
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      '.BackColor = Color.LightGray
      '.BorderStyle = BorderStyle.None
      .Location = New Point(0, Label.Height)
      .Size = New Size(Me.Width, 24)
      .TextAlign = HorizontalAlignment.Left
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(TextBox)

    ResizeControlVertical()

    ResumeLayout(False)
  End Sub

  Private Sub AddControlsHorizontal()
    SuspendLayout()
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    If LabelWidth = 0 Then LabelWidth = CInt(Math.Floor(Me.Width / 2))
    With Label
      .AutoSize = False
      .BackColor = Color.Transparent
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top
      .Location = New Point(0, 0)
      .Size = New Size(LabelWidth, 20)
      .TextAlign = ContentAlignment.MiddleLeft
    End With

    With TextBox
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      '.BackColor = Color.LightGray
      '.BorderStyle = BorderStyle.None
      .Location = New Point(LabelWidth, 0)
      .Size = New Size(Me.Width - LabelWidth, 20)
      .TextAlign = HorizontalAlignment.Left
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(TextBox)

    'ResizeControlHorizontal()
    ResumeLayout(False)
  End Sub

  Private Sub Me_Resize(sender As Object, e As EventArgs)
    If LabelPosition = ELabelPosition.Top Then ResizeControlVertical()
    If LabelPosition = ELabelPosition.Left Then ResizeControlHorizontal()
  End Sub

  Private Sub ResizeControlVertical()
    Me.Height = Label.Height + TextBox.Height
  End Sub

  Private Sub ResizeControlHorizontal()
    Label.Height = TextBox.Height
    Me.Height = TextBox.Height
  End Sub


End Class
