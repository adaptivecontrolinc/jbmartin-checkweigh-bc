Public Class ProductClass

  Public Dyelot As String
  Public Redye As Integer
  Public StepID As Integer                  'DyelotsBulkedRecipe.ID
  Public StepNumber As Integer              'DyelotsBulkedRecipe.StepNumber
  Public AddSequence As Integer             'DyelotsBulkedRecipe.AddNumber

  Public Name As String                     'DyelotsBulkedRecipe.StepCode
  Public ProductCode As String              'DyelotsBulkedRecipe.StepCode
  Public ProductName As String              'DyelotsBulkedRecipe.StepDescription

  Public ProductID As Integer               'DyelotsBulkedRecipe.StepID
  Public ProductTypeID As Integer           'DyelotsBulkedRecipe.StepTypeID

  Public ReadOnly Property ProductType As EProductType
    Get
      Return CType(ProductTypeID, EProductType)
    End Get
  End Property
  Public Enum EProductType
    Null = 0
    Liquid = 2
    Powder = 3
    Active = 4
    Obsolete = 5
  End Enum

  Public Amount As Double                   'DyelotsBulkedRecipe.Amount
  Public DAmount As Double                  'DyelotsBulkedRecipe.DispenseGrams
  Public Units As String                    'DyelotsBulkedRecipe.UnitCode
  Public Grams As Double                    'DyelotsBulkedRecipe.Grams
  Public DGrams As Double                   'DyelotsBulkedRecipe.DispenseGrams
  Public DState As Integer                  'DyelotsBulkedRecipe.DispenseState

  Public IsGhost As Integer
  Public IsChemical As Boolean
  Public IsDye As Boolean
  Public Barcode As String
  Public LotNumber As String

  Public PreWeighedGrams As Double
  Public NewContainerGrams As Double

  Public Tolerance As Double = 0.0025
  Public ToleranceMinGrams As Double = 1
  Public ToleranceMaxGrams As Double = 100

  Friend State As ProductState
  Private pState As ProductState
  Private pTargetAmount As Double
  Private pMaxErrorAmount As Double

  Public Function CheckWeight(ByVal currentGrams As Double) As Boolean
    If Math.Abs(currentGrams - Grams) <= ToleranceGrams Then
      Return True
    Else
      Return False
    End If
  End Function

  Public ReadOnly Property ToleranceGrams() As Double
    Get
      Dim ReturnValue As Double = Grams * Tolerance
      If ReturnValue < ToleranceMinGrams Then ReturnValue = ToleranceMinGrams
      If ReturnValue > ToleranceMaxGrams Then ReturnValue = ToleranceMaxGrams
      Return ReturnValue
    End Get
  End Property

  Public ReadOnly Property DisplayProduct() As String
    Get
      Return ProductCode & "  " & ProductName
    End Get
  End Property

End Class