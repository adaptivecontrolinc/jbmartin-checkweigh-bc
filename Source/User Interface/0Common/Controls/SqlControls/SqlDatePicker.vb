Public Class SqlDatePicker : Inherits UserControl : Implements ISqlControl
  Public Event ValueChanged(sender As Object, text As String)

  ' label above a text box
  Public Label As New Label
  Public DatePicker As New DateTimePicker

  Property LabelPosition As ELabelPosition = ELabelPosition.Top ' label on top or to the left
  Property LabelWidth As Integer  ' really only used if labelPosition = left

  Property SqlAutoFill As Boolean = True Implements ISqlControl.SqlAutoFill

  Property SqlColumnName As String Implements ISqlControl.SqlColumnName ' column name if mapping to a table

  Property SqlValueType As ESqlValueType = ESqlValueType.Date Implements ISqlControl.SqlValueType

  Property SqlValue() As Object Implements ISqlControl.SqlValue
    Get
      ' Assume date picker date is local, convert to store in DB
      If Me.Value = Nothing Then Return DBNull.Value
      Return Me.Value.ToUniversalTime
    End Get
    Set(value As Object)
      ' Assume DB date is UTC, convert to local to store
      If TypeOf value IsNot Date Then
        Me.Value = Nothing
      Else
        Me.Value = DirectCast(value, Date).ToLocalTime
      End If
    End Set
  End Property

  Property Value As Date
    Get
      Return DatePicker.Value
    End Get
    Set(value As Date)
      If value = Nothing Then
        ' Show blank date picker
        DatePicker.Format = DateTimePickerFormat.Custom
        DatePicker.CustomFormat = " "
      Else
        DatePicker.Format = DateTimePickerFormat.Long
        DatePicker.Value = value
      End If
    End Set
  End Property

  Property [ReadOnly] As Boolean
    Get
      Return Not DatePicker.Enabled
    End Get
    Set(value As Boolean)
      DatePicker.Enabled = Not value
    End Set
  End Property

  Public Sub New()
    Label.Text = "DatePicker"
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

    With DatePicker
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      '.BackColor = Color.LightGray
      '.BorderStyle = BorderStyle.None
      .Location = New Point(0, Label.Height)
      .Size = New Size(Me.Width, 24)
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(DatePicker)

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

    With DatePicker
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      '.BackColor = Color.LightGray
      '.BorderStyle = BorderStyle.None
      .Location = New Point(LabelWidth, 0)
      .Size = New Size(Me.Width - LabelWidth, 20)
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(DatePicker)

    'ResizeControlHorizontal()
    ResumeLayout(False)
  End Sub

  Private Sub Me_Resize(sender As Object, e As EventArgs)
    If LabelPosition = ELabelPosition.Top Then ResizeControlVertical()
    If LabelPosition = ELabelPosition.Left Then ResizeControlHorizontal()
  End Sub

  Private Sub ResizeControlVertical()
    Me.Height = Label.Height + DatePicker.Height
  End Sub

  Private Sub ResizeControlHorizontal()
    Label.Height = DatePicker.Height
    Me.Height = DatePicker.Height
  End Sub

  Private Sub AddEventHandlers()
    AddHandler DatePicker.ValueChanged, AddressOf DatePicker_ValueChanged
    AddHandler DatePicker.Leave, AddressOf DatePicker_Leave
    AddHandler DatePicker.KeyDown, AddressOf DatePicker_KeyDown
    ' Didn't handle keypress 'cos I don't like testing for KeyChar - it's ugly
  End Sub

  Private Sub DatePicker_ValueChanged(sender As Object, e As EventArgs)
    If DatePicker.Value = Nothing Then
      DatePicker.Format = DateTimePickerFormat.Custom
      DatePicker.CustomFormat = " "
    Else
      DatePicker.Format = DateTimePickerFormat.Long
    End If
  End Sub

  Private Sub DatePicker_Leave(sender As Object, e As EventArgs)
    RaiseEventValueChanged()
  End Sub

  Private Sub DatePicker_KeyDown(sender As Object, e As KeyEventArgs)
    If e.KeyCode = Keys.Enter Then RaiseEventValueChanged()
  End Sub

  Private Sub RaiseEventValueChanged()
    RaiseEvent ValueChanged(Me, DatePicker.Text)
  End Sub

End Class
