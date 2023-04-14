Imports System.ComponentModel

Public Class Settings
  ' Version 2023-04-05

  Public Shared ReadOnly Property DefaultCulture() As System.Globalization.CultureInfo
    Get
      Return System.Globalization.CultureInfo.InvariantCulture
    End Get
  End Property



  ' System Settings

  <DescriptionAttribute("Set Check Weigh to Simulation mode"), CategoryAttribute("System Settings")>
  Property DemoMode As Boolean

  <DescriptionAttribute("Customer name"), CategoryAttribute("System Settings")>
  Property Customer As String

  <DescriptionAttribute("Display units for the data grids"), CategoryAttribute("System Settings")>
  Property DisplayUnits As EUnits



  ' Form Main

  <DescriptionAttribute("Disable Select Batch button on Form Main"), CategoryAttribute("Form Main")>
  Property DisableSelectButton As Boolean = False

  <DescriptionAttribute("Disable Refresh button on Form Main"), CategoryAttribute("Form Main")>
  Property DisableRefreshButton As Boolean = False

  <DescriptionAttribute("Show or hide Customize button on Form Main"), CategoryAttribute("Form Main")>
  Property ShowGridButton As Boolean = True

  <DescriptionAttribute("Show or hide Settings button on Form Main"), CategoryAttribute("Form Main")>
  Property ShowSettingsButton As Boolean = True

  <DescriptionAttribute("Show or hide Balance button on Form Main"), CategoryAttribute("Form Main")>
  Property ShowBalanceButton As Boolean = True

  <DescriptionAttribute("Show or hide About button on Form Main"), CategoryAttribute("Form Main")>
  Property ShowAboutButton As Boolean = True

  <DescriptionAttribute("Show or hide Exit button on Form Main"), CategoryAttribute("Form Main")>
  Property ShowExitButton As Boolean = True



  ' Form Weigh
  <DescriptionAttribute("Enable or disable PreWeigh button on Form Weigh"), CategoryAttribute("Form Weigh")>
  Property ButtonPreWeighEnable As Boolean = False

  <DescriptionAttribute("Enable or disable New Container button on Form Weigh"), CategoryAttribute("Form Weigh")>
  Property ButtonNewContainerEnable As Boolean = True

  <DescriptionAttribute("Enable or disable Skip Product button on Form Weigh"), CategoryAttribute("Form Weigh")>
  Property ButtonSkipProductEnable As Boolean = True

  <DescriptionAttribute("Enable or disable Lot Number button on Form Weigh"), CategoryAttribute("Form Weigh")>
  Property ButtonLotNumberEnable As Boolean = True

  <DescriptionAttribute("Enable or disable No Barcode button on Form Weigh"), CategoryAttribute("Form Weigh")>
  Property ButtonNoBarcodeEnable As Boolean = True

  <DescriptionAttribute("Skip user scan on Form Weigh"), CategoryAttribute("Form Weigh")>
  Property SkipScanUser As Boolean

  <DescriptionAttribute("Skip barcode check on Form Weigh"), CategoryAttribute("Form Weigh")>
  Property SkipScanCode As Boolean

  <DescriptionAttribute("Skip lot number entry on Form Weigh"), CategoryAttribute("Form Weigh")>
  Property SkipScanLotNumber As Boolean

  <DescriptionAttribute("Print step ticket after all the products in the step have been weighed"), CategoryAttribute("Form Weigh")>
  Property PrintStepTicket As Boolean

  <DescriptionAttribute("Print product ticket after each product is weighed"), CategoryAttribute("Form Weigh")>
  Property PrintProductTicket As Boolean



  ' Barcode Settings
  <DescriptionAttribute("Barcode font to use on all printouts"), CategoryAttribute("Barcode Settings")>
  Property BarcodeFontName As String = "Free 3 of 9"

  <DescriptionAttribute("Barcode font size to use on all printouts"), CategoryAttribute("Barcode Settings")>
  Property BarcodeFontSize As Integer = 24

  <DescriptionAttribute("Barcode separator used to split text into Dyelot and Redye"), CategoryAttribute("Barcode Settings")>
  Public Shared Property BarcodeSeparator As String = "$"




  ' Batch List Settings
  <DescriptionAttribute("Batch list show scheduled only"), CategoryAttribute("Batch List")>
  Property ScheduledOnly As Boolean = False  ' only showed dyelots scheduled to machines

  <DescriptionAttribute("Batch list row filter"), CategoryAttribute("Batch List")>
  Property BatchListRowFilter As String


  ' Product Settings
  <DescriptionAttribute("Check weigh dyes only"), CategoryAttribute("Product Settings")>
  Property DyesOnly As Boolean = False

  <DescriptionAttribute("Check weigh chemicals only"), CategoryAttribute("Product Settings")>
  Property ChemsOnly As Boolean = False

  <DescriptionAttribute("Check weigh powders only"), CategoryAttribute("Product Settings")>
  Property PowderOnly As Boolean = False

  <DescriptionAttribute("Only check weigh dyes and chemicals with CheckWeighEnable set to true"), CategoryAttribute("Product Settings")>
  Property CheckWeighEnableOnly As Boolean = False

  <DescriptionAttribute("Exclude steps that have already been weighed"), CategoryAttribute("Product Settings")>
  Property ExcludeWeighed As Boolean = True


  ' Sql Settings
  <DescriptionAttribute("Connection string for the database"), CategoryAttribute("Sql Settings")>
  Property SqlConnection As String = "Data Source=adaptive-server;Initial Catalog=BatchDyeingCentral;User ID=sa;Password=Control;app=CheckWeigh"
  '     "Data Source=DALEHOPKINS-PC2\SQLEXPRESS2019;Initial Catalog=JBMartin.BatchDyeingCentral;User ID=sa;Password=Control;app=CheckWeigh"

  ' %WHERE will be replaced in BatchListData
  <DescriptionAttribute("Select statement for Batch List"), CategoryAttribute("Sql Settings")>
  Property SqlSelectBatches As String =
    "SELECT ID, Dyelot, Redye, StartTime, EndTime, Machine, Blocked, Color FROM Dyelots WITH (NOLOCK)" &
    " %WHERE" &
    " ORDER BY StartTime"

  ' %WHERE will be replaced in StepListData
  <DescriptionAttribute("Select statement for Batch Step List"), CategoryAttribute("Sql Settings")>
  Property SqlSelectBatchSteps As String =
    "SELECT *, (Grams / 1000) AS Kilograms, (DispenseGrams / 1000) AS DispenseKilograms FROM DyelotsBulkedRecipe" &
    " %WHERE" &
    " ORDER BY StepNumber,Grams"

  ' %WHERE will be replaced in WeighListData
  <DescriptionAttribute("Select statement for Batch Step List"), CategoryAttribute("Sql Settings")>
  Property SqlSelectWeighSteps As String =
    "SELECT *, (Grams / 1000) AS Kilograms, (DispenseGrams / 1000) AS DispenseKilograms FROM DyelotsBulkedRecipe" &
    " %WHERE" &
    " ORDER BY StepNumber,Grams"



End Class

