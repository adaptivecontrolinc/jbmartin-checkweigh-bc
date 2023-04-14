Public Class SqlComboBox : Inherits UserControl : Implements ISqlControl
  Public Event ValueChanged(sender As Object, index As Integer, text As String)
  Public Event RowChanged(sender As Object, row As DataRow)


  ' Label above a combo box
  Public Label As New Label
  Public WithEvents ComboBox As New ComboBox

  Property LabelPosition As ELabelPosition = ELabelPosition.Top
  Property LabelWidth As Integer

  Property SqlAutoFill As Boolean = True Implements ISqlControl.SqlAutoFill

  Property SqlColumnName As String Implements ISqlControl.SqlColumnName ' column name if mapping to a table

  Property SqlValueType As ESqlValueType = ESqlValueType.Integer Implements ISqlControl.SqlValueType

  ' This has to be as Object so it can hold DBNull.Value
  Property SqlValue() As Object Implements ISqlControl.SqlValue
    Get
      If SelectedValue Is Nothing Then Return DBNull.Value
      If TypeOf SelectedValue Is Integer Then Return DirectCast(SelectedValue, Integer)
      If TypeOf SelectedValue Is String Then Return DirectCast(SelectedValue, String)
      Return DBNull.Value
    End Get
    Set(value As Object)
      If value Is DBNull.Value Then
        ' Do nothing ??
      Else
        Me.SelectedValue = value.ToString
      End If
    End Set
  End Property

  Sub SqlSetup(dataSource As System.Data.DataTable, DisplayMember As String, ValueMember As String)
    With ComboBox
      .AutoCompleteMode = AutoCompleteMode.SuggestAppend
      .AutoCompleteSource = AutoCompleteSource.ListItems
      .DataSource = dataSource
      .DisplayMember = DisplayMember
      .ValueMember = ValueMember
      .Text = Nothing
    End With
  End Sub

  Sub SqlSetup(dataSource As System.Data.DataTable, DisplayMember As String, ValueMember As String, sortOrder As String)
    With ComboBox
      .AutoCompleteMode = AutoCompleteMode.SuggestAppend
      .AutoCompleteSource = AutoCompleteSource.ListItems
      .DataSource = New DataView(dataSource) With {.Sort = sortOrder}
      .DisplayMember = DisplayMember
      .ValueMember = ValueMember
      .Text = Nothing
    End With
  End Sub

  Property SelectedIndex As Integer
    Get
      Return ComboBox.SelectedIndex
    End Get
    Set(value As Integer)
      ComboBox.SelectedIndex = value
    End Set
  End Property

  Property SelectedValue As Object
    Get
      Return ComboBox.SelectedValue
    End Get
    Set(value As Object)
      ComboBox.SelectedValue = value
    End Set
  End Property

  Property SelectedID As Integer
    Get
      If TypeOf SelectedValue Is Integer Then Return DirectCast(SelectedValue, Integer)
      Return -1
    End Get
    Set(value As Integer)
      SelectedValue = value
    End Set
  End Property

  Property SelectedText As String
    Get
      If TypeOf SelectedValue Is String Then Return DirectCast(SelectedValue, String)
      Return Nothing
    End Get
    Set(value As String)
      SelectedValue = value
    End Set
  End Property

  Property SelectedItem As Object
    Get
      Return ComboBox.SelectedItem
    End Get
    Set(value As Object)
      ComboBox.SelectedItem = value
    End Set
  End Property

  ReadOnly Property SelectedDataRow As DataRow
    Get
      If Not TypeOf SelectedItem Is DataRowView Then Return Nothing
      Return DirectCast(SelectedItem, DataRowView).Row
    End Get
  End Property

  Sub New()
    Label.Text = "ComboBox"
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

  Public Sub New(label As String, labelWidth As Integer, orientation As ELabelPosition)
    Me.Label.Text = label
    Me.LabelPosition = orientation
    Me.LabelWidth = labelWidth
    NewBase()
  End Sub

  Private Sub NewBase()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent
    Label.ForeColor = LabelForeColor

    AddControls()
    Utilities.Translations.Translate(Me)
    Me.ActiveControl = Label
  End Sub

  Private Sub AddControls()
    Me.BackColor = Color.Transparent
    If LabelPosition = ELabelPosition.Top Then AddControlsVertical()
    If LabelPosition = ELabelPosition.Left Then AddControlsHorizontal()
  End Sub

  Private Sub AddControlsVertical()
    SuspendLayout()
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    With Label
      .AutoSize = False
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .Location = New Point(-3, 0)
      .Size = New Size(Me.Width + 3, 20)
      .TextAlign = ContentAlignment.MiddleLeft
    End With

    With ComboBox
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .Location = New Point(0, Label.Height)
      .Size = New Size(Me.Width, 20)

      '.DrawMode = DrawMode.OwnerDrawVariable
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(ComboBox)

    ResizeControl()
    ResumeLayout(False)
  End Sub

  Private Sub AddControlsHorizontal()
    SuspendLayout()
    Me.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

    If LabelWidth = 0 Then LabelWidth = CInt(Math.Floor(Me.Width / 2))
    With Label
      .AutoSize = False
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top
      .Location = New Point(0, 0)
      .Size = New Size(LabelWidth, 24)
      .TextAlign = ContentAlignment.MiddleLeft
    End With

    With ComboBox
      .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
      .Location = New Point(LabelWidth, 0)
      .Size = New Size(Me.Width - LabelWidth, 24)
    End With

    Me.Controls.Add(Label)
    Me.Controls.Add(ComboBox)

    ResizeControl()
    ResumeLayout(False)
  End Sub

  Private Sub Me_Resize(sender As Object, e As EventArgs) Handles Me.Resize
    ResizeControl()
  End Sub

  Private Sub ResizeControl()
    If LabelPosition = ELabelPosition.Top Then ResizeControlVertical()
    If LabelPosition = ELabelPosition.Left Then ResizeControlHorizontal()
  End Sub

  Private Sub ResizeControlVertical()
    Me.Height = Label.Height + ComboBox.Height + 1
  End Sub

  Private Sub ResizeControlHorizontal()
    Label.Height = ComboBox.Height
    Me.Height = ComboBox.Height
  End Sub

  Private Sub ComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox.SelectedIndexChanged
    Dim index = ComboBox.SelectedIndex
    Dim text = ComboBox.Text
    RaiseEvent ValueChanged(Me, index, text)
    RaiseEvent RowChanged(Me, SelectedDataRow)
  End Sub

  Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
    MyBase.OnPaint(e)
    Me.ComboBox.SelectionLength = 0
  End Sub

End Class
