
Public Class SqlCheckBox : Inherits UserControl : Implements ISqlControl

  Public CheckBox As New CheckBox

  Property SqlAutoFill As Boolean = True Implements ISqlControl.SqlAutoFill

  Property SqlColumnName As String Implements ISqlControl.SqlColumnName

  Property SqlValueType As ESqlValueType = ESqlValueType.Boolean Implements ISqlControl.SqlValueType

  ' This has to be as Object so it can hold DBNull.Value
  '   Note SqlValue will not return DBNull.Value so it will always set the mapped column to a value
  Property SqlValue As Object Implements ISqlControl.SqlValue
    Get
      Return Value
    End Get
    Set(value As Object)
      Me.Value = Utilities.Sql.NullToFalseBoolean(value)
    End Set
  End Property

  Property Value As Boolean
    Get
      Return CheckBox.Checked
    End Get
    Set(value As Boolean)
      CheckBox.Checked = value
    End Set
  End Property

  Property Checked As Boolean
    Get
      Return CheckBox.Checked
    End Get
    Set(value As Boolean)
      CheckBox.Checked = value
    End Set
  End Property

  Sub New()
    CheckBox.Text = "CheckBox"
    NewBase()
  End Sub

  Public Sub New(label As String)
    CheckBox.Text = label
    NewBase()
  End Sub

  Public Sub New(label As String, sqlColumnName As String)
    CheckBox.Text = label
    Me.SqlColumnName = sqlColumnName
    NewBase()
  End Sub

  Public Sub New(label As String, sqlColumnName As String, autoFill As Boolean)
    CheckBox.Text = label
    Me.SqlColumnName = sqlColumnName
    Me.SqlAutoFill = autoFill
    NewBase()
  End Sub

  Private Sub NewBase()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent
    CheckBox.ForeColor = LabelForeColor

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Private Sub AddControls()
    SuspendLayout()
    Me.Height = 20
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    With CheckBox
      .AutoSize = False
      .BackColor = Color.Transparent

      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
      .Location = New Point(0, 0)
      .Size = New Size(Me.Width, 20)

      .CheckAlign = ContentAlignment.MiddleLeft
      .TextAlign = ContentAlignment.MiddleLeft
    End With
    Me.Controls.Add(CheckBox)

    ResumeLayout(False)
  End Sub

  Private Sub SqlCheckBox_Resize(sender As Object, e As EventArgs) Handles Me.Resize
    Me.Height = CheckBox.Height
  End Sub
End Class
