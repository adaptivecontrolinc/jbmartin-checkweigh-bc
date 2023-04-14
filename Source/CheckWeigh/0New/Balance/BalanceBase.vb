Imports System.ComponentModel

Public MustInherit Class BalanceBase

  MustOverride Sub Start()
  MustOverride Sub Cancel()

  <Browsable(False)>
  Property RunError As String

  <DescriptionAttribute("Balance name."), CategoryAttribute("General Settings")>
  Property Name As String = "Base"


  <DescriptionAttribute("Connection parameters."), CategoryAttribute("Comm Settings")>
  Property Connection As String                   ' Connection string for Serial or TCP 

  <DescriptionAttribute("Data string parameters."), CategoryAttribute("Comm Settings")>
  Public Property DataString As String = "g"
  Public Property DataLength As Integer = 8

  <DescriptionAttribute("The units of the value transmitted by the balance."), CategoryAttribute("Comm Settings")>
  Property WeightUnits As EBalanceUnits = EBalanceUnits.Kilograms   ' The units of the weight transmitted by the balance (grams, kilograms, ounces or pounds)


  <DescriptionAttribute("The units to use when displaying the balance weight."), CategoryAttribute("Display Settings")>
  Property DisplayUnits As EBalanceUnits = EBalanceUnits.Kilograms   ' The units on the balance display

  <DescriptionAttribute("The number format to use when displaying the balance weight."), CategoryAttribute("Display Settings")>
  Property DisplayFormat As String = Nothing                         ' The number format to use when displaying the balance weight on the forms (nothing will show default formats)


  <DescriptionAttribute("Minimum grams this balance can weigh."), CategoryAttribute("Balance Settings")>
  Property MinGrams As Double                    ' Use these values to determine which scale to use if we have multiple scales

  <DescriptionAttribute("Maximum grams this balance can weigh."), CategoryAttribute("Balance Settings")>
  Property MaxGrams As Double                    '

  <DescriptionAttribute("Balance is tared if the weight is less than or equal to this value."), CategoryAttribute("Balance Settings")>
  Property TareGrams As Double



  <DescriptionAttribute("Weigh tolerance % of target weight."), CategoryAttribute("Tolerance Settings")>
  Property Tolerance As Double = 0.0025

  <DescriptionAttribute("Weigh tolerance minimum grams absolute."), CategoryAttribute("Tolerance Settings")>
  Property ToleranceMinGrams As Double = 1

  <DescriptionAttribute("Weigh tolerance maximum grams absolute."), CategoryAttribute("Tolerance Settings")>
  Property ToleranceMaxGrams As Double = 100


  <BrowsableAttribute(False)>
  Property Data As String                         ' The data packet from the balance

  <BrowsableAttribute(False)>
  Property DataLastRead As Date                   ' The last time we received data from the balance

  <BrowsableAttribute(False)>
  Property Weight As Double                       ' The weight value transmitted by the balance (grams, kilograms, ounces or pounds)

  <BrowsableAttribute(False)>
  Property WeightLastRead As Date                 ' The last time we succesfully read the weight from the balance

  <BrowsableAttribute(False)>
  Overridable ReadOnly Property Tared As Boolean
    Get
      Return Math.Abs(Grams) <= TareGrams
    End Get
  End Property

  <BrowsableAttribute(False)>
  ReadOnly Property DisplayWeight As String
    Get
      Return Utilities.Weight.FormatWeight(Weight, WeightUnits, DisplayUnits, DisplayFormat)
    End Get
  End Property

  <BrowsableAttribute(False)>
  ReadOnly Property Grams As Double
    Get
      Return Utilities.Weight.ConvertWeight(Weight, WeightUnits, EBalanceUnits.Grams)
    End Get
  End Property

  <BrowsableAttribute(False)>
  ReadOnly Property Kilograms As Double
    Get
      Return Utilities.Weight.ConvertWeight(Weight, WeightUnits, EBalanceUnits.Kilograms)
    End Get
  End Property

  <BrowsableAttribute(False)>
  ReadOnly Property Ounces As Double
    Get
      Return Utilities.Weight.ConvertWeight(Weight, WeightUnits, EBalanceUnits.Ounces)
    End Get
  End Property

  <BrowsableAttribute(False)>
  ReadOnly Property Pounds As Double
    Get
      Return Utilities.Weight.ConvertWeight(Weight, WeightUnits, EBalanceUnits.Pounds)
    End Get
  End Property

End Class

