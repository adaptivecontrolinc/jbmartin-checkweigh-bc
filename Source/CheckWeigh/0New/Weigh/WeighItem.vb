Public Class WeighItem

  Property Row As DataRow
  Property StartTime As Date
  Property EndTime As Date

  Property UserName As String
  Property BalanceName As String

  Property Barcode As String

  Property GramsPreWeighed As Double   ' Amount of product pre-weighed before we start
  Property GramsNewContainer As Double    ' Amount of product weighed in previous containers (on this step)
  Property GramsBalance As Double      ' Current grams weighed on the balance

  Function CheckBarcode() As Boolean
    If Row Is Nothing Then Return False
    If Row("StepCode").ToString.Equals(Barcode, StringComparison.InvariantCultureIgnoreCase) Then Return True
  End Function

  ReadOnly Property Product As String
    Get
      If Row Is Nothing Then Return Nothing
      Return Row("StepCode").ToString & " " & Row("StepDescription").ToString
    End Get
  End Property

  ReadOnly Property CurrentGrams As Double
    Get
      Return GramsBalance + GramsNewContainer + GramsPreWeighed
    End Get
  End Property

  ReadOnly Property TargetGrams As Double
    Get
      If Row Is Nothing Then Return -1
      If Row.IsNull("Grams") Then Return -1
      Return Utilities.Sql.NullToZeroDouble(Row("Grams"))
    End Get
  End Property

End Class


