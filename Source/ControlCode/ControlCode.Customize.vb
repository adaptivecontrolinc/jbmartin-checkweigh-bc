Partial Public Class ControlCode

  Private Sub CustomizeOperatorToolStrip()
    'Create custom Operator tool strip - bottom tool strip
    With Parent
      'Custom button to add to tool strip
      ' Dim buttonWorkList As New Windows.Forms.ToolStripButton("Work List", My.Resources.WorkList16x16)

      '  .AddButton(buttonWorkList, ButtonPosition.Operator, New WorklistDispenser(Me))
      '  .AddStandardButton(StandardButton.Mimic, ButtonPosition.Operator)
      '  .AddStandardButton(StandardButton.Graph, ButtonPosition.Operator)
      '  .AddStandardButton(StandardButton.History, ButtonPosition.Operator)


      Dim checkWeighButton As New Windows.Forms.ToolStripButton("Check Weigh", My.Resources.Scale16x16)
      .AddButton(checkWeighButton, ButtonPosition.Operator, New FormMain(Me))

      ' Testing to confirm history created - leave on checkweigh or not
      .AddStandardButton(StandardButton.History, ButtonPosition.Operator)

    End With
  End Sub

  Private Sub CustomizeExpertToolStrip()
    'Create custom Expert tool strip - shown after expert button press
    With Parent
      'Custom buttons to add to tool strip
      ' Dim buttonRecipes As New ToolStripButton("Recipes", My.Resources.Recipes16x16)
      ' Dim buttonProducts As New ToolStripButton("Products", My.Resources.Products16x16)
      ' Dim buttonDestinations As New ToolStripButton("Destinations", My.Resources.Destinations16x16)

      .AddStandardButton(StandardButton.IO, ButtonPosition.Expert)
      .AddStandardButton(StandardButton.Variables, ButtonPosition.Expert)
      .AddStandardButton(StandardButton.Programs, ButtonPosition.Expert)
      .AddStandardButton(StandardButton.Parameters, ButtonPosition.Expert)
      '  .AddButton(buttonRecipes, ButtonPosition.Expert, New Recipes)
      '  .AddButton(buttonProducts, ButtonPosition.Expert, New Products(Me))
      '  .AddButton(buttonDestinations, ButtonPosition.Expert, New Destinations(Me))


    End With
  End Sub

End Class
