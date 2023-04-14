Module Images

  Property RecipeStepDyeImageBytes As Byte()
  Property RecipeStepChemicalImageBytes As Byte()
  Property RecipeStepMessageImageBytes As Byte()
  Property RecipeStepTreatmentImageBytes As Byte()

  Sub New()
    recipeStepDyeImageBytes = GetRecipeStepImageBytes(ERecipeStepType.Dye)
    recipeStepChemicalImageBytes = GetRecipeStepImageBytes(ERecipeStepType.Chemical)
    recipeStepMessageImageBytes = GetRecipeStepImageBytes(ERecipeStepType.Message)
    recipeStepTreatmentImageBytes = GetRecipeStepImageBytes(ERecipeStepType.Treatment)
  End Sub

  Public Function GetRecipeStepImage(stepType As ERecipeStepType) As Image
    Select Case stepType
      Case ERecipeStepType.Dye : Return My.Resources.Dyes16x16
      Case ERecipeStepType.Chemical : Return My.Resources.Chemicals16x16
      Case ERecipeStepType.Message : Return My.Resources.Instructions16x16
      Case ERecipeStepType.Treatment : Return My.Resources.TreatmentsGreenBlue16x16
      Case Else
        Return Nothing
    End Select
  End Function

  Public Function GetRecipeStepImageBytes(stepType As ERecipeStepType) As Byte()
    Dim image = GetRecipeStepImage(stepType)
    If image Is Nothing Then Return Nothing

    Dim iConverter As New ImageConverter
    Return CType(iConverter.ConvertTo(image, GetType(Byte())), Byte())
  End Function

End Module
