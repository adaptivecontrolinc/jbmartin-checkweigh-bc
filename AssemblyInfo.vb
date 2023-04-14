Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Permissions

' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes
<Assembly: ComVisible(False)>
<Assembly: CLSCompliant(True)>
<Assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution:=True)>

<Assembly: AssemblyTitle("Adaptive Check Weigh")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("Adaptive Incorporated")>
<Assembly: AssemblyProduct("Check Weigh")>
<Assembly: AssemblyCopyright("Copyright © Adaptive Incorporated, all rights reserved.")>
<Assembly: AssemblyTrademark("")>

'The following GUID is for the ID of the typelib if this project is exposed to COM
<Assembly: Guid("0a391813-3612-4533-bc2d-c0172c0f19c2")>

' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:
' <Assembly: AssemblyVersion("1.0.*")> 

<Assembly: AssemblyVersion("1.2.*")>
<Assembly: AssemblyFileVersion("1.2")>

'******************************************************************************************************
'****************                Code Version Notes -- Most Recent At Top              ****************
'******************************************************************************************************


' Version 1.2 [2023-04-013] DH
' - Add barcode separator setting used with Recipe Maker to create Batch Ticket barcode.


' Version 1.1 [2022-01-21] DH
' - CheckWeigh JB Marting Batch Contro version #1
' - Began with MW's Southfork v1.2 [2021-07-13] mw

' V1.2 [2021-07-13] mw
' - tweaked to work at delta.

' V1.1 [2020-11-05] DH
' - Second cut of CheckWeigh application integrated into BatchControl
' - Starting with ML's Polartec V4.118 - notes below:


'V4.118 [2019-11-04] ML
' Added DisableSelectButton and DisableRefreshButton to settings

'V4.117 [2019-02-20] ML
' Changed default sort order on SqlSelectBatches (Machine,StartTime -> StartTime)

'V4.116 [2018-11-28] ML
'  Updated progress text so we could set weight format to match the balance in use

'V4.118 [2019-11-04] ML
' Added DisableSelectButton and DisableRefreshButton to settings


'V4.115 [2018-11-28] ML
'  Changed Skip Product and enabled from ScanCode to Weigh
'  Use DispenseGrams to check for dispense complete rather than DispenseTime (which is now set on skip product)
'  Added DispenseBy to PrintProduct

'V4.114 [2018-11-27] ML
'  Changed to scale selection rather than scale summing
'  Supports multiple demo scales

'V4.112 [2018-11-27] ML
'  Added option to set newline on serial port

'V4.111 [2018-11-19] ML
'  Added BatchListRowFilter so we can drop batches by DyeDispenseDone / ChemicalDispenseDone

'V4.110 [2018-11-19] ML
'  Polartec version 

'V4.109 [2018-03-09] ML
'  Tidied up building of SQL Select statements for BatchList, StepList and FormWeigh
'  Added LotNumber to Form Weigh with eanable options and Lot Number button on FormWeigh

'V4.108 [2018-03-02] ML
'  Added CheckWeighEnable to select statements
'  Added ExcludeWeighed to old FormWeigh

'V4.107 [2018-01-17] ML
'  Took out error logging in balance run section and set a public property so it can be displayed in the balance form

'V4.106 [2017-12-28] ML
' Fixed a bug with the grid layout xml save, a bit of shoddy code meant the xml files were being locked when they were 
'   read so we couldn't write the new config (DataGridlayoutManager.GetLayoutFromFile(file As String) As DataGridLayout)

'V4.105 [2017-12-28] ML
' Added code so the select statements for the batch list and step list can be changed in settings.xml

'V4.104 [2017-12-20] ML
' Added property grid for main settings and balance settings

'V4.103 [2017-12-15] ML
' Added balance display to status bar panels

'V4.102 [2017-12-07] ML
' Added new balance handling using inheritance from base class BalanceBase and serialization to load from xml files

'V4.101 [2017-11-05] ML
' New version to work with Recipe Maker Add In
