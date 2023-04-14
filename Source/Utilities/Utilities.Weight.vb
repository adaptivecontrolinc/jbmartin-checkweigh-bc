
Namespace Utilities
  Partial Public Class Weight

    Public Shared Function FormatWeight(ByVal weight As Double, ByVal weightUnits As EBalanceUnits, displayUnits As EBalanceUnits, displayFormat As String) As String
      Dim displayWeight = ConvertWeight(weight, weightUnits, displayUnits)
      Return FormatWeight(displayWeight, displayUnits, displayFormat)
    End Function

    Public Shared Function FormatWeight(ByVal weight As Double, ByVal weightUnits As EBalanceUnits, displayFormat As String) As String
      If displayFormat Is Nothing Then Return FormatWeight(weight, weightUnits)

      Select Case weightUnits
        Case EBalanceUnits.Grams
          Return weight.ToString(displayFormat) & " g"

        Case EBalanceUnits.Kilograms
          Return weight.ToString(displayFormat) & " kg"

        Case EBalanceUnits.Ounces
          Return weight.ToString(displayFormat) & " oz"

        Case EBalanceUnits.Pounds
          Return weight.ToString(displayFormat) & " lb"

        Case Else
          Return "na"

      End Select
    End Function

    Public Shared Function FormatWeight(ByVal weight As Double, ByVal weightUnits As EBalanceUnits) As String
      Select Case weightUnits
        Case EBalanceUnits.Grams
          Return FormatWeight(weight, weightUnits, "#0")

        Case EBalanceUnits.Kilograms
          Return FormatWeight(weight, weightUnits, "#0.000")

        Case EBalanceUnits.Ounces
          Return FormatWeight(weight, weightUnits, "#0.0")

        Case EBalanceUnits.Pounds
          Return FormatWeight(weight, weightUnits, "#0.000")

        Case Else
          Return "na"

      End Select
    End Function


    Public Shared Function ConvertWeight(weight As Double, weightUnits As EBalanceUnits, returnUnits As EBalanceUnits) As Double
      Select Case returnUnits
        Case EBalanceUnits.Grams : Return GetGrams(weight, weightUnits)
        Case EBalanceUnits.Kilograms : Return GetKilograms(weight, weightUnits)
        Case EBalanceUnits.Ounces : Return GetOunces(weight, weightUnits)
        Case EBalanceUnits.Pounds : Return GetPounds(weight, weightUnits)
      End Select
      Return -1
    End Function

    Public Shared Function GetGrams(weight As Double, units As EBalanceUnits) As Double
      Select Case units
        Case EBalanceUnits.Grams : Return weight
        Case EBalanceUnits.Kilograms : Return weight * 1000
        Case EBalanceUnits.Ounces : Return weight * Utilities.Conversions.OuncesToGrams
        Case EBalanceUnits.Pounds : Return weight * Utilities.Conversions.PoundsToGrams
      End Select
      Return -1
    End Function

    Public Shared Function GetKilograms(weight As Double, units As EBalanceUnits) As Double
      Select Case units
        Case EBalanceUnits.Grams : Return weight / 1000
        Case EBalanceUnits.Kilograms : Return weight
        Case EBalanceUnits.Ounces : Return weight * Utilities.Conversions.OuncesToKilograms
        Case EBalanceUnits.Pounds : Return weight * Utilities.Conversions.PoundsToKilograms
      End Select
      Return -1
    End Function

    Public Shared Function GetOunces(weight As Double, units As EBalanceUnits) As Double
      Select Case units
        Case EBalanceUnits.Grams : Return weight * Utilities.Conversions.GramsToOunces
        Case EBalanceUnits.Kilograms : Return weight * Utilities.Conversions.KilogramsToOunces
        Case EBalanceUnits.Ounces : Return weight
        Case EBalanceUnits.Pounds : Return weight * 16
      End Select
      Return -1
    End Function

    Public Shared Function GetPounds(weight As Double, units As EBalanceUnits) As Double
      Select Case units
        Case EBalanceUnits.Grams : Return weight * Utilities.Conversions.GramsToPounds
        Case EBalanceUnits.Kilograms : Return weight * Utilities.Conversions.KilogramsToPounds
        Case EBalanceUnits.Ounces : Return weight / 16
        Case EBalanceUnits.Pounds : Return weight
      End Select
      Return -1
    End Function

  End Class

End Namespace
