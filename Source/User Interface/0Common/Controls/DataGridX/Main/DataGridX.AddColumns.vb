Partial Class DataGridX

  Sub AddCheckBoxIntegerColumn(ByVal name As String, ByVal width As Integer, ByVal headerText As String)
    Dim column As New XColumnCheckBox
    With column
      .DataPropertyName = name
      '      .DefaultCellStyle = Nothing
      .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
      .HeaderText = headerText
      .Name = name
      .ReadOnly = True
      .Width = width
      .Tag = "CheckBox"
    End With
    MyBase.Columns.Add(column)
  End Sub

  Sub AddColorColumn(ByVal name As String, ByVal width As Integer, ByVal headerText As String)
    Dim column As New XColumnColor
    With column
      .DataPropertyName = name
      .DefaultCellStyle = Nothing
      .HeaderText = headerText
      .Name = name
      .ReadOnly = True
      .Width = width
      .Tag = "Color"
    End With
    MyBase.Columns.Add(column)
  End Sub

  Sub AddComboColumn(ByVal name As String, ByVal width As Integer, ByVal headerText As String, ByVal table As DataTable, ByVal valueMember As String, ByVal displayMember As String)
    AddComboColumn(name, width, headerText, table, valueMember, displayMember, name)
  End Sub

  Sub AddComboColumn(ByVal name As String, ByVal width As Integer, ByVal headerText As String, ByVal table As DataTable, ByVal valueMember As String, ByVal displayMember As String, ByVal dataPropertyName As String)
    Dim column As New System.Windows.Forms.DataGridViewComboBoxColumn
    With column
      .DataPropertyName = dataPropertyName
      .DataSource = table
      .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
      .DisplayMember = displayMember
      .DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
      .HeaderText = headerText
      .Name = name
      .ReadOnly = Me.ReadOnly
      .ValueMember = valueMember
      .Width = width
      .Tag = "ComboBox"
    End With
    MyBase.Columns.Add(column)
  End Sub

  Sub AddCurrencyColumn(ByVal name As String, ByVal width As Integer, headerText As String)
    Dim column As New DataGridViewTextBoxColumn
    With column
      .DataPropertyName = name
      .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
      .DefaultCellStyle.Format = "c"
      .HeaderText = headerText
      .Name = name
      .ReadOnly = Me.ReadOnly
      .Width = width
      .Tag = "Currency"
    End With
    Me.Columns.Add(column)
  End Sub

  Sub AddDateColumn(ByVal name As String, ByVal width As Integer, headerText As String)
    AddDateColumn(name, width, headerText, DataGridViewContentAlignment.MiddleRight)
  End Sub

  Sub AddDateColumn(ByVal name As String, ByVal width As Integer, headerText As String, ByVal alignment As DataGridViewContentAlignment)
    AddDateColumn(name, width, headerText, DataGridViewContentAlignment.MiddleRight, "d")
  End Sub

  Sub AddDateColumn(ByVal name As String, ByVal width As Integer, headerText As String, ByVal alignment As DataGridViewContentAlignment, ByVal format As String)
    Dim column As New DataGridViewTextBoxColumn
    With column
      .DataPropertyName = name
      .DefaultCellStyle.Alignment = alignment
      .DefaultCellStyle.Format = format
      .HeaderText = headerText
      .Name = name
      .ReadOnly = Me.ReadOnly
      .Width = width
      .Tag = "Date"
    End With
    Me.Columns.Add(column)
  End Sub

  Sub AddImageColumn(ByVal name As String, ByVal width As Integer, headerText As String)
    AddImageColumn(name, width, headerText, DataGridViewContentAlignment.MiddleCenter)
  End Sub

  Sub AddImageColumn(ByVal name As String, ByVal width As Integer, headerText As String, ByVal alignment As DataGridViewContentAlignment)
    Dim column As New DataGridViewImageColumn
    With column
      .ValuesAreIcons = False
      .DefaultCellStyle.DataSourceNullValue = DBNull.Value
      .DataPropertyName = name
      .DefaultCellStyle = Nothing
      .DefaultCellStyle.Alignment = alignment
      .DefaultCellStyle.NullValue = Nothing
      .HeaderText = headerText
      .Name = name
      .ReadOnly = True
      .Width = width
      .Tag = "Image"
    End With
    Me.Columns.Add(column)
  End Sub

  Sub AddNumberColumn(ByVal name As String, ByVal width As Integer, headerText As String)
    AddNumberColumn(name, width, headerText, DataGridViewContentAlignment.MiddleRight)
  End Sub

  Sub AddNumberColumn(ByVal name As String, ByVal width As Integer, headerText As String, ByVal alignment As DataGridViewContentAlignment)
    AddNumberColumn(name, width, headerText, alignment, Nothing)
  End Sub

  Sub AddNumberColumn(ByVal name As String, ByVal width As Integer, ByVal headerText As String, ByVal alignment As DataGridViewContentAlignment, ByVal format As String)
    Dim column As New DataGridViewTextBoxColumn
    With column
      .DataPropertyName = name
      .DefaultCellStyle.Alignment = alignment
      .DefaultCellStyle.Format = format
      .HeaderText = headerText
      .Name = name
      .ReadOnly = Me.ReadOnly
      .Width = width
      .Tag = "Number"
    End With
    Me.Columns.Add(column)
  End Sub

  Sub AddRecipeStepImageColumn(ByVal name As String, dataPropertyName As String, ByVal width As Integer, headerText As String)
    AddRecipeStepImageColumn(name, dataPropertyName, width, headerText, DataGridViewContentAlignment.MiddleCenter)
  End Sub

  Sub AddRecipeStepImageColumn(ByVal name As String, dataPropertyName As String, ByVal width As Integer, headerText As String, ByVal alignment As DataGridViewContentAlignment)
    Dim column As New XColumnRecipeStepImage
    With column
      .DefaultCellStyle.DataSourceNullValue = DBNull.Value
      .DataPropertyName = dataPropertyName
      .DefaultCellStyle = Nothing
      .DefaultCellStyle.Alignment = alignment
      .DefaultCellStyle.NullValue = Nothing
      .HeaderText = headerText
      .Name = name
      .ReadOnly = True
      .Width = width
      .Tag = "RecipeStepImage"
    End With
    Me.Columns.Add(column)
  End Sub

  Sub AddTextColumn(ByVal name As String, ByVal width As Integer, ByVal headerText As String)
    AddTextColumn(name, width, headerText, DataGridViewContentAlignment.MiddleLeft)
  End Sub

  Sub AddTextColumn(ByVal name As String, ByVal width As Integer, ByVal headerText As String, ByVal alignment As DataGridViewContentAlignment)
    Dim column As New DataGridViewTextBoxColumn
    With column
      .DataPropertyName = name
      .DefaultCellStyle.Alignment = alignment
      .HeaderText = headerText
      .Name = name
      .ReadOnly = Me.ReadOnly
      .Width = width
      .Tag = "Text"
    End With
    Me.Columns.Add(column)
  End Sub

End Class
