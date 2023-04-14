Public Class SqlTextBoxOld : Inherits UserControl : Implements ISqlControl
  Public Event ValueChanged(sender As Object, text As String)

  ' label above a text box
  Public Label As New Label
  Public TextBox As New TextBox

  Property LabelPosition As ELabelPosition = ELabelPosition.Top ' label on top or to the left
  Property LabelWidth As Integer  ' really only used if labelPosition = left

  Property SqlAutoFill As Boolean = True Implements ISqlControl.SqlAutoFill

  Property SqlColumnName As String Implements ISqlControl.SqlColumnName ' column name if mapping to a table

  Property SqlValueType As ESqlValueType Implements ISqlControl.SqlValueType

  Property SqlValue() As Object Implements ISqlControl.SqlValue
    Get
      Dim tryInteger As Integer, tryDouble As Double, tryDate As Date
      Select Case SqlValueType
        Case ESqlValueType.Integer
          If Integer.TryParse(Value, tryInteger) Then Return tryInteger
        Case ESqlValueType.Double
          If Double.TryParse(Value, tryDouble) Then Return tryDouble
        Case ESqlValueType.Date
          If Date.TryParse(Value, tryDate) Then Return tryDate
        Case Else
          If Not String.IsNullOrEmpty(Value) Then Return Value
      End Select
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
      If TextBox.Text <> value Then
        TextBox.Text = value
        RaiseEventValueChanged()
      End If
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

  Public Sub New(label As String, sqlColumnName As String, sqlValueType As ESqlValueType)
    Me.Label.Text = label
    Me.SqlColumnName = sqlColumnName
    Me.SqlValueType = sqlValueType
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
    AddEventHandlers()
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

  Private Sub AddEventHandlers()
    AddHandler TextBox.Leave, AddressOf TextBox_Leave
    AddHandler TextBox.KeyDown, AddressOf TextBox_KeyDown
    ' Didn't handle keypress 'cos I don't like testing for KeyChar - it's ugly
  End Sub

  Private Sub TextBox_Leave(sender As Object, e As EventArgs)
    RaiseEventValueChanged()
  End Sub

  Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs)
    If e.KeyCode = Keys.Enter Then RaiseEventValueChanged()
  End Sub

  Private Sub RaiseEventValueChanged()
    RaiseEvent ValueChanged(Me, TextBox.Text)
  End Sub

End Class
