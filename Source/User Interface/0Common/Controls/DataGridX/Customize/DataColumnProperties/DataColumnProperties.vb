Public Class DataColumnProperties : Inherits UserControl

  Private Const groupHeight = 150
  Private Const labelWidth = 84

  Private groupHeader As GroupBox
  Private headerText As New SqlTextBoxOld("Text:", labelWidth, ELabelPosition.Left)
  Private headerWidth As New SqlTextBoxOld("Width:", labelWidth, ELabelPosition.Left)
  Private headerAlign As New SqlComboBox("Alignment:", labelWidth, ELabelPosition.Left)

  Private groupColumn As GroupBox
  Private columnAlign As New SqlComboBox("Alignment:", labelWidth, ELabelPosition.Left)
  Private columnStyle As New SqlComboBox("Format:", labelWidth, ELabelPosition.Left)
  Private columnFormat As New SqlTextBoxOld("Format string:", labelWidth, ELabelPosition.Left)


  Sub New()
    DoubleBuffered = True
    Font = Defaults.DefaultFont
    BackColor = Color.Transparent

    AddControls()
    Utilities.Translations.Translate(Me)
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    headerAlign.ComboBox.Items.AddRange([Enum].GetNames(GetType(DataGridLayout.Column.Alignment)))
    columnAlign.ComboBox.Items.AddRange([Enum].GetNames(GetType(DataGridLayout.Column.Alignment)))
    columnStyle.ComboBox.Items.AddRange([Enum].GetNames(GetType(DataGridLayout.Column.Style)))

    groupHeader = New GroupBox
    With groupHeader
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
      .Text = "Header"

      .Size = New Size(Me.Width, groupHeight)
      .Location = New Point(0, 0)
    End With
    Me.Controls.Add(groupHeader)

    AddControlToGroup(groupHeader, headerText, 28)
    AddControlToGroup(groupHeader, headerWidth, 60)
    AddControlToGroup(groupHeader, headerAlign, 92)

    groupColumn = New GroupBox
    With groupColumn
      .Anchor = AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
      .Text = "Column"

      .Size = New Size(Me.Width, groupHeight)
      .Location = New Point(0, Me.Height - groupHeight)
    End With
    Me.Controls.Add(groupColumn)

    AddControlToGroup(groupColumn, columnAlign, 28)
    AddControlToGroup(groupColumn, columnStyle, 60)
    AddControlToGroup(groupColumn, columnFormat, 92)

    ResumeLayout(False)
  End Sub

  Private Sub AddControlToGroup(targetGroupBox As GroupBox, control As UserControl, top As Integer)
    With control
      .BackColor = Color.Transparent
      .Location = New Point(16, top)

      .Height = 24
      .Width = targetGroupBox.Width - 32
    End With
    targetGroupBox.Controls.Add(control)
  End Sub

End Class
