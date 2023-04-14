Public Class SqlTextBox : Inherits UserControl : Implements ISqlControl
  Public Event ValueChanged(sender As Object, text As String)

  ' label above a text box
  Public LabelLeft As New Label   ' main text on the left
  Public LabelRight As New Label   ' additional info on the right, typically max min limits
  Public TextBox As New TextBox

  Private labelHeight As Integer = 20      ' TODO resize based on Font Size
  Private textboxHeight As Integer = 24

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
    LabelLeft.Text = "TextBox"
    NewBase()
  End Sub

  Public Sub New(label As String)
    Me.LabelLeft.Text = label
    NewBase()
  End Sub

  Public Sub New(label As String, sqlColumnName As String)
    Me.LabelLeft.Text = label
    Me.SqlColumnName = sqlColumnName
    NewBase()
  End Sub

  Public Sub New(label As String, sqlColumnName As String, sqlValueType As ESqlValueType)
    Me.LabelLeft.Text = label
    Me.SqlColumnName = sqlColumnName
    Me.SqlValueType = sqlValueType
    NewBase()
  End Sub

  Private Sub NewBase()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    Height = labelHeight + textboxHeight
    Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    With LabelLeft
      .Size = New Size(Me.Width + 3, labelHeight)
      .Location = New Point(-3, 3)
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

      .AutoSize = True
      .BackColor = Color.Transparent
      .ForeColor = LabelForeColor
      .TextAlign = ContentAlignment.MiddleLeft

      AddHandler .Resize, AddressOf ResizeControl
    End With

    With LabelRight
      .Size = New Size(Me.Width + 3, labelHeight)
      .Location = New Point(-3, 3)
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

      .AutoSize = True
      .BackColor = Color.Transparent
      .ForeColor = LabelForeColor
      .TextAlign = ContentAlignment.MiddleRight

      AddHandler .Resize, AddressOf ResizeControl
    End With

    With TextBox
      .Size = New Size(Me.Width, textboxHeight)
      .Location = New Point(0, labelHeight)
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

      .TextAlign = HorizontalAlignment.Left
      .Visible = True

      AddHandler .Leave, AddressOf TextBox_Leave
      AddHandler .KeyDown, AddressOf TextBox_KeyDown
    End With

    Controls.Add(LabelRight)
    Controls.Add(LabelLeft)
    Controls.Add(TextBox)

    ResumeLayout(False)
  End Sub

  Overrides Property Text As String
    Get
      Return TextLeft
    End Get
    Set(value As String)
      TextLeft = value
    End Set
  End Property

  Property TextLeft As String
    Get
      Return LabelLeft.Text
    End Get
    Set(value As String)
      If value <> LabelLeft.Text Then
        LabelLeft.Text = value
      End If
    End Set
  End Property

  Property TextRight As String
    Get
      Return LabelRight.Text
    End Get
    Set(value As String)
      If value <> LabelRight.Text Then
        LabelRight.Text = value
      End If
    End Set
  End Property


  Private Sub TextBox_Leave(sender As Object, e As EventArgs)
    RaiseEventValueChanged()
  End Sub

  Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs)
    If e.KeyCode = Keys.Enter Then RaiseEventValueChanged()
  End Sub

  Private Sub RaiseEventValueChanged()
    RaiseEvent ValueChanged(Me, TextBox.Text)
  End Sub


  Private Sub Me_Resize(sender As Object, e As EventArgs)
    ResizeControl(sender, e)
  End Sub

  Private Sub ResizeControl(sender As Object, e As EventArgs)
    Height = TextBox.Top + TextBox.Height + 2  ' pad this a bit so it matches the old default spacing

    LabelRight.Left = Width - LabelRight.Width
    LabelRight.SendToBack()
  End Sub

End Class
