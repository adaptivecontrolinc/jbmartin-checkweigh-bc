' Display an image based on an image list

Imports System.ComponentModel

Partial Public Class DataGridX

  Class XColumnRecipeStepImage : Inherits DataGridViewColumn

    Sub New()
      MyBase.New(New XCellRecipeStepImage())
      Me.ReadOnly = True
    End Sub

    Overrides Property CellTemplate() As DataGridViewCell
      Get
        Return MyBase.CellTemplate
      End Get
      Set(ByVal value As DataGridViewCell)
        ' Ensure that the cell used for the template is the correct type
        If (value IsNot Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(XCellRecipeStepImage)) Then
          Throw New InvalidCastException("Must be a recipe step image Cell")
        End If
        MyBase.CellTemplate = value
      End Set
    End Property
  End Class

  Class XCellRecipeStepImage : Inherits DataGridViewImageCell

    Protected Overrides Function GetValue(rowIndex As Integer) As Object
      Dim value = MyBase.GetValue(rowIndex)
      If Not (TypeOf value Is Integer) Then Return DBNull.Value

      Select Case CInt(value)
        Case 1 : Return recipeStepDyeImageBytes
        Case 2 : Return recipeStepChemicalImageBytes
        Case 4 : Return recipeStepTreatmentImageBytes
      End Select

      Return DBNull.Value
    End Function

  End Class

End Class
