Imports Utilities.Sql

Public Class FormWeigh : Inherits System.Windows.Forms.Form
  Private controlCode As ControlCode

  Property SqlConnection As String
  Property SqlSelect As String

  Property BatchID As Integer
  Property Machine As String

  Property Dyelot As String
  Property ReDye As Integer
  Property StepNumber As Integer
  Property AddNumber As Integer

  Property Products As ProductCollection                         ' Collection of all products to weigh for this dyelot
  Property CurrentProduct As ProductClass                        ' Product we are currently weighing
  Property CurrentBalance As BalanceBase                         ' Balance we are using to weigh

  Property Users As DataTable                                    ' All users (for badge scan)
  Property UserFound As Boolean
  Property UserBadge As String
  Property UserID As String
  Property UserName As String

  Private FormNewContainer As New FormNewContainer
  Private FormNewDyeDrop As New FormNewDyeDrop

  Friend WithEvents ButtonPreWeigh As Button
  Friend WithEvents ButtonLotNumber As Button

  Private Const ImageScan As Integer = 0
  Private Const ImageWeigh As Integer = 1
  Private Const ImageDone As Integer = 2
  Private Const ImageSkipped As Integer = 5

  Property DemoMode As Boolean
  Property DemoGrams As Double
  Property DemoTimer As New Timer

  Public Function Start(batchRow As System.Data.DataRow, stepRow As System.Data.DataRow) As Boolean
    Try
      'Make sure we have some data
      If batchRow Is Nothing OrElse stepRow Is Nothing Then Return False

      Me.BatchID = NullToZeroInteger(batchRow("ID"))
      Me.Machine = batchRow("Machine").ToString

      Me.Dyelot = batchRow("Dyelot").ToString
      Me.ReDye = NullToZeroInteger(batchRow("ReDye"))

      Me.StepNumber = NullToZeroInteger(stepRow("StepNumber"))
      Me.AddNumber = NullToZeroInteger(stepRow("AddNumber"))

      Me.Text = "Dyelot " & Me.Dyelot & " - Step: " & Me.StepNumber.ToString
      If Me.ReDye > 0 Then Me.Text = "Dyelot " & Me.Dyelot & "@" & Me.AddNumber.ToString & " - Step: " & Me.StepNumber.ToString

      Dim StepID As Integer = NullToZeroInteger(stepRow("ID"))
      Dim stepCode As String = NullToNothingString(stepRow("StepCode"))

      SqlConnection = controlCode.Settings.SqlConnection
      SqlSelect = GetSqlSelect()

      Dim stepsTable = GetDataTable(SqlConnection, SqlSelect, "DyelotsBulkedRecipe")
      If stepsTable Is Nothing OrElse (stepsTable.Rows.Count <= 0) Then
        MsgBox("No products to weigh please select another dyelot", MsgBoxStyle.Exclamation, "Adaptive Control")
        Exit Function
      End If

      ' Made it this far - create a batch
      controlCode.Manager.AddBatch(Dyelot, StepNumber.ToString)

      ' TODO - NOTE: Polartec Database uses a stored procedure to obtain the MesUsers table from another Server & table "Manufacturing"
      '              I created a local MesUsers1 table for testing here only
      If controlCode.Settings.SkipScanUser = False Then
        Users = GetDataTable(SqlConnection, "SELECT * FROM MesUsers1") ' "SELECT * FROM MesUsers"
      End If

      Products = New ProductCollection
      For Each row As System.Data.DataRow In stepsTable.Rows
        Dim newProduct As New ProductClass
        With newProduct
          .StepID = NullToZeroInteger(row("ID"))
          .StepNumber = NullToZeroInteger(row("StepNumber"))
          .AddSequence = NullToZeroInteger(row("DisplayOrder"))

          .Name = CType(row("StepCode"), String)
          .ProductCode = CType(row("StepCode"), String)
          .ProductName = NullToNothingString(row("StepDescription"))

          .ProductID = NullToZeroInteger(row("StepID"))
          .ProductTypeID = NullToZeroInteger(row("StepTypeID"))

          .IsDye = (.ProductTypeID = 1)                             ' 2020-11-04 ProductTypeID field is not defined with Polartec.BatchDyeingCentral database
          .IsChemical = (.ProductTypeID = 2)

          .Amount = NullToZeroDouble(row("Amount"))
          .Units = NullToNothingString(row("UnitCode"))

          .Grams = NullToZeroDouble(row("Grams"))

          .DAmount = NullToZeroInteger(row("DispenseGrams"))
          .DGrams = NullToZeroInteger(row("DispenseGrams"))

          .Dyelot = Dyelot
          .Redye = ReDye

          ' Update product DState value, used with filtering
          .DState = NullToZeroInteger(row("DispenseState"))

        End With
        Products.Add(newProduct)
      Next
      CurrentProduct = Products.FirstByIndex

      'Hide all the controls in the Instructions group - except the barcode text box
      Dim MyControl As Control
      For Each MyControl In GroupInstructions.Controls : MyControl.Visible = False : Next
      LabelBarcode.Visible = True
      TextboxBarcode.Text = ""
      TextboxBarcode.Visible = True

      'Set timer and make an initial run through code
      TimerMain.Enabled = True
      Run()
      Return True

    Catch ex As Exception
      'Display any errors
      Dim ErrorText As String = "FormWeigh_Start(): " & ex.Message
      Utilities.Log.LogError(ErrorText)
      MsgBox(ErrorText, MsgBoxStyle.Critical, "Adaptive Control")
      Return False
    End Try
  End Function

  Private Function GetSqlSelect() As String
    Dim sql As String = Nothing
    Try
      sql = controlCode.Settings.SqlSelectWeighSteps
      Dim sqlWhere As String = "WHERE (DyelotID=" & BatchID.ToString & ") AND (StepNumber=" & StepNumber.ToString & ") AND ((StepTypeID=1) OR (StepTypeID=2))"

      If controlCode.Settings.DyesOnly Then sqlWhere = "WHERE (DyelotID=" & BatchID.ToString & ") AND (StepNumber=" & StepNumber.ToString & ") AND (StepTypeID=1)"
      If controlCode.Settings.ChemsOnly Then sqlWhere = "WHERE (DyelotID=" & BatchID.ToString & ") AND (StepNumber=" & StepNumber.ToString & ") AND (StepTypeID=2)"

      If controlCode.Settings.CheckWeighEnableOnly Then sqlWhere &= " AND (CheckWeighEnable=1)"
      If controlCode.Settings.ExcludeWeighed Then sqlWhere &= " AND (DispenseGrams Is Null)"

      Return sql.Replace("%WHERE", sqlWhere)
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return sql
  End Function

  Private Function UpdateResponseString(ByVal existingResponse As String, ByVal newResponse As String) As String
    Dim returnValue As String = ""
    Try
      If existingResponse <> "" Then
        returnValue = existingResponse & Environment.NewLine & newResponse
      Else
        returnValue = newResponse
      End If

    Catch ex As Exception
    End Try
    ' Return New Value
    Return returnValue
  End Function

  Private Sub TimerMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerMain.Tick
    Run()
  End Sub

  Public Sub Run()
    Try
      ' If the timer is not enabled quit
      If TimerMain.Enabled = False Then Exit Sub

      ' Disable timer then renable at the end to ensure timer.interval between calls
      TimerMain.Enabled = False

      ' If all goes according to plan Current Product Index = Line Number of instruction
      Dim Index As Integer = Products.GetIndexOf(CurrentProduct)

      ' Enable DemoMode button TODO Add more complexity?
      ButtonDemoWeigh.Enabled = (controlCode.Parent.Mode = Mode.Test) OrElse (controlCode.Parent.Mode = Mode.Debug)


      ' Do all this with the active product in the products collection
      With CurrentProduct
        ' Loop until all state changes are done
        Dim prevState As ProductState
        Do
          prevState = .State

          ' Simulate a scale if we're in Demo mode
          If (controlCode.DemoMode) Then RunBalanceSimulation()


          ' Good old state machine - how comforting
          Select Case .State

            Case ProductState.Setup
              ' Disable buttons
              ButtonDone.Enabled = False
              ButtonNextProduct.Enabled = False
              ButtonPreWeigh.Enabled = False
              ButtonNewContainer.Enabled = False
              ButtonSkipProduct.Enabled = False
              ButtonNoBarcode.Enabled = False
              ButtonAbort.Enabled = True
              If controlCode.Parent.Mode = Mode.Debug Then ButtonDemoWeigh.Enabled = True
              ' Hide buttons 
              If Products.Count <= 1 Then ButtonNextProduct.Visible = False

              ' Setup barcode textbox
              TextboxBarcode.Text = ""
              TextboxBarcode.Focus()
              .State = ProductState.ScanUser
              ' Clear progress display
              ProgressText.Clear()
              ProgressBars.Clear()
              GroupWeigh.Text = "Please weigh the amount below "

            Case ProductState.ScanUser
              ' Set mask on text box
              TextboxBarcode.PasswordChar = "*"c
              ' Tell the operator to scan id badge
              SetInstruction(Index, ImageScan, "Scan User Badge")
              ' Skip this step if user already scanned
              If UserFound Then .State = ProductState.ScanCode
              ' Skip this step if option set
              If controlCode.Settings.SkipScanUser Then .State = ProductState.ScanCode
              ' Clear progress display
              ProgressText.Clear()
              ProgressBars.Clear()

            Case ProductState.ScanCode
              ' Clear mask on text box
              TextboxBarcode.PasswordChar = Nothing
              ' Tell the operator to scan the product barcode
              SetInstruction(Index, ImageScan, "Scan:" & "   " & .DisplayProduct)
              ' Skip this step if option set
              If controlCode.Settings.SkipScanCode Then .State = ProductState.ScanLotNumber
              ' Enable no barcode button if allowed
              If controlCode.Settings.ButtonNoBarcodeEnable Then ButtonNoBarcode.Enabled = True
              ' Enable skip product button if allowed
              If controlCode.Settings.ButtonSkipProductEnable Then ButtonSkipProduct.Enabled = True
              ' Clear progress display
              ProgressText.Clear()
              ProgressBars.Clear()

            Case ProductState.ScanLotNumber
              ' Clear mask on text box
              TextboxBarcode.PasswordChar = Nothing
              ' Tell the operator to scan the lot number
              SetInstruction(Index, ImageScan, "Scan Lot:" & "   " & .DisplayProduct)
              ' Skip this step if option set
              If controlCode.Settings.SkipScanLotNumber Then .State = ProductState.SelectScale
              ' Enable lot number button if allowed
              If controlCode.Settings.ButtonLotNumberEnable Then ButtonLotNumber.Enabled = True
              ' Enable no barcode button if allowed
              If controlCode.Settings.ButtonNoBarcodeEnable Then ButtonNoBarcode.Enabled = True
              ' Enable skip product button if allowed
              If controlCode.Settings.ButtonSkipProductEnable Then ButtonSkipProduct.Enabled = True
              ' Clear progress display
              ProgressText.Clear()
              ProgressBars.Clear()


            Case ProductState.SelectScale
              SetBalance(CurrentProduct)
              ' Set tolerance based on the scale being used
              If CurrentBalance IsNot Nothing Then
                .Tolerance = CurrentBalance.Tolerance
                .ToleranceMinGrams = CurrentBalance.ToleranceMinGrams
                .ToleranceMaxGrams = CurrentBalance.ToleranceMaxGrams
                ' Set unit format on progress text
                ProgressText.DisplayUnits = CurrentBalance.DisplayUnits
                ProgressText.DisplayFormat = CurrentBalance.DisplayFormat
              Else
                SetInstruction(Index, ImageScan, "  Balance not defined.  Check configuration")
              End If
              ' Clear progress display
              ProgressText.Clear()
              ProgressBars.Clear()
              ' Show the user which balance to use
              ShowBalanceSelection()
              ' Tarte the balance
              .State = ProductState.Tare

            Case ProductState.Tare
              ' Signal to tare all scales
              SetInstruction(Index, ImageWeigh, "Tare:  Please clear all scales.")
              'Disable lot number button
              ButtonLotNumber.Enabled = False
              'Disable no barcode button
              ButtonNoBarcode.Enabled = False
              'Check to see if all active scales are tared
              If BalanceTared Then
                .State = ProductState.Weigh
                If controlCode.Settings.ButtonPreWeighEnable Then ButtonPreWeigh.Enabled = True
                If controlCode.Settings.ButtonNewContainerEnable Then ButtonNewContainer.Enabled = True
                If controlCode.Settings.ButtonSkipProductEnable Then ButtonSkipProduct.Enabled = True
              End If
              'Update progress display
              ProgressText.Progress(TotalGrams, 0, 0)
              ProgressBars.Progress(TotalGrams, 0, 0)

            Case ProductState.Weigh
              'Tell the operator to Weigh the product
              SetInstruction(Index, ImageWeigh, "Weigh  " & .DisplayProduct)
              'Check to see if target weight is reached
              If .CheckWeight(TotalGrams) Then
                ButtonPreWeigh.Enabled = False
                ButtonNewContainer.Enabled = False
                ButtonSkipProduct.Enabled = False

                ButtonNextProduct.Enabled = True
                ButtonNextProduct.Visible = True
              Else
                If controlCode.Settings.ButtonPreWeighEnable Then ButtonPreWeigh.Enabled = True
                If controlCode.Settings.ButtonNewContainerEnable Then ButtonNewContainer.Enabled = True
                If controlCode.Settings.ButtonSkipProductEnable Then ButtonSkipProduct.Enabled = True
                ButtonNextProduct.Enabled = False
              End If
              'Update progress display
              ProgressText.Progress(TotalGrams, .Grams, .ToleranceGrams)
              ProgressBars.Progress(TotalGrams, .Grams, .ToleranceGrams)

            Case ProductState.NewContainer
              'Update progress display
              ProgressText.Progress(.NewContainerGrams, .Grams, .ToleranceGrams)
              ProgressBars.Progress(.NewContainerGrams, .Grams, .ToleranceGrams)

            Case ProductState.SkipProduct
              'Update database and move on to next product
              SetInstruction(Index, ImageSkipped, "Skipped   " & .DisplayProduct)
              ButtonNextProduct.Enabled = False

              'Update Product as skipped 
              UpdateDatabaseState(CurrentProduct, DispenseState.Skipped)

              If Index < Products.Count - 1 Then
                Dim OldProduct As ProductClass = CurrentProduct
                CurrentProduct = Products(Index + 1)
                If OldProduct.StepNumber <> CurrentProduct.StepNumber Then
                  FormNewDyeDrop.ShowDialog()
                End If
                CurrentProduct.State = ProductState.Setup

                If controlCode.Settings.PrintProductTicket Then PrintProduct(OldProduct)             ' Print this product if the option is set
              Else
                ButtonDone.Enabled = True
                TimerMain.Enabled = False
                .State = ProductState.Off

                If controlCode.Settings.PrintProductTicket Then PrintProduct(CurrentProduct)             ' Print this product if the option is set
                If controlCode.Settings.PrintStepTicket Then PrintStep(CurrentProduct)                   ' Print this step if the option is set
              End If
              'Update progress display
              ProgressText.Progress(TotalGrams, 0, 0)
              ProgressBars.Progress(TotalGrams, 0, 0)

            Case ProductState.Done
              'Update database and move on to next product
              SetInstruction(Index, ImageDone, "Done   " & .DisplayProduct)
              ButtonNextProduct.Enabled = False

              'Update the Product Details
              If TotalGrams >= 0 Then
                UpdateDatabaseAmounts(CurrentProduct, TotalGrams, DispenseState.Manual)
              Else
                UpdateDatabaseState(CurrentProduct, DispenseState.Error)
              End If

              'Is this the last product
              If (Index < Products.Count - 1) Then
                Dim OldProduct As ProductClass = CurrentProduct
                CurrentProduct = Products(Index + 1)
                If OldProduct.StepNumber <> CurrentProduct.StepNumber Then
                  FormNewDyeDrop.ShowDialog()
                End If
                CurrentProduct.State = ProductState.Setup

                If controlCode.Settings.PrintProductTicket Then PrintProduct(OldProduct)             ' Print this product if the option is set
              Else
                ButtonDone.Enabled = True
                TimerMain.Enabled = False
                .State = ProductState.Off

                If controlCode.Settings.PrintProductTicket Then PrintProduct(CurrentProduct)         ' Print this product if the option is set
                If controlCode.Settings.PrintStepTicket Then PrintStep(CurrentProduct)               ' Print this Step If the Option Is Set
              End If

              'Update progress display
              ProgressText.Progress(TotalGrams, 0, 0)
              ProgressBars.Progress(TotalGrams, 0, 0)
            Case Else
              'Some code
          End Select
        Loop Until (prevState = .State)
      End With
    Catch ex As Exception
      Utilities.Log.LogError("FormWeigh Run: " & ex.Message)
    End Try
    'Re-enable timer
    TimerMain.Enabled = True
  End Sub

  Private Sub SetInstruction(ByVal LineNumber As Integer, ByVal ImageNumber As Integer, ByVal Text As String)
    Try
      'Update image and text
      Dim MyControl As Control, Picturebox As PictureBox, Label As Label
      For Each MyControl In Me.GroupInstructions.Controls
        If MyControl.Name = "PictureboxProduct" & LineNumber Then
          Picturebox = CType(MyControl, PictureBox)
          Picturebox.Image = ImageList16.Images(ImageNumber)
          Picturebox.Visible = True
        End If
        If MyControl.Name = "LabelProduct" & LineNumber Then
          Label = CType(MyControl, Label)
          Label.Text = Text
          Label.Visible = True
        End If
      Next
    Catch ex As Exception
      Utilities.Log.LogError("FormWeigh set instructions: " & ex.Message)
    End Try
  End Sub


  Private Sub SetBalance(product As ProductClass)
    ' Pick the best balance for this step based on the deisred weight and the balance range
    For Each balance As BalanceBase In controlCode.IO.BalanceList
      If product.Grams > balance.MinGrams AndAlso product.Grams <= balance.MaxGrams Then
        CurrentBalance = balance
        Exit Sub
      End If
    Next

    ' Pick the largest balance if the step is not in any range
    Dim maxGrams As Double = -1
    For Each balance As BalanceBase In controlCode.IO.BalanceList
      If controlCode.IO.Balance.MaxGrams > maxGrams Then
        maxGrams = balance.Grams
        CurrentBalance = balance
      End If
    Next
  End Sub

  Private Sub UpdateDatabaseAmounts(ByVal product As ProductClass, ByVal grams As Double, state As DispenseState)
    Dim sql As String = Nothing
    Try
      With product
        Dim computerName As String = System.Environment.MachineName
        Dim pounds As Double = Math.Round(grams * Utilities.Conversions.GramsToPounds, 3)

        'Set Update String
        sql = "UPDATE DyelotsBulkedRecipe SET" &
               "  DispenseGrams=" & grams.ToString &
               ", DispensePounds=" & pounds.ToString &
               ", DispenseTime=" & SqlDateString(Date.UtcNow) &
               ", DispenseBy=" & SqlString(UserID) &
               ", DispenseState=" & CInt(state) &
               ", DispenseSource=" & SqlString(computerName) &
               ", LotNumber=" & SqlString(.LotNumber) &
               " WHERE ID=" & .StepID.ToString
        Utilities.Sql.SqlUpdate(controlCode.Settings.SqlConnection, sql)
      End With

    Catch ex As Exception
      'Show error message
      Dim ErrorText As String = "FormWeigh_UpdateDatabase(): " & ex.Message
      Utilities.Log.LogError(ErrorText, sql)
      MsgBox(ErrorText, MsgBoxStyle.Critical, "Adaptive Control")
    End Try
  End Sub

  Private Sub UpdateDatabaseState(ByVal product As ProductClass, state As DispenseState)
    Dim sql As String = Nothing
    Try
      With product
        Dim computerName As String = System.Environment.MachineName

        'Set Update String
        sql = "UPDATE DyelotsBulkedRecipe SET" &
              "  DispenseTime=" & SqlDateString(Date.UtcNow) &
              ", DispenseBy=" & SqlString(UserID) &
              ", DispenseState=" & CInt(state) &
              ", DispenseSource=" & SqlString(computerName) &
              " WHERE ID=" & .StepID.ToString
        Utilities.Sql.SqlUpdate(controlCode.Settings.SqlConnection, sql)
      End With

    Catch ex As Exception
      'Show error message
      Dim ErrorText As String = "FormWeigh_UpdateDatabase(): " & ex.Message
      Utilities.Log.LogError(ErrorText, sql)
      MsgBox(ErrorText, MsgBoxStyle.Critical, "Adaptive Control")
    End Try
  End Sub

  Private Sub PrintProduct(product As ProductClass)
    Dim dyelot = product.Dyelot
    Dim redye = product.Redye
    Dim stepNumber = product.StepNumber
    Dim stepID = product.StepID

    Using dialogPrint As New System.Windows.Forms.PrintDialog
      With dialogPrint
        .AllowPrintToFile = False
        .AllowSelection = False
        .AllowSomePages = False

        .Document = (New PrintProduct(controlCode, dyelot, redye, stepNumber, stepID)).Document
        If .Document IsNot Nothing Then dialogPrint.Document.Print()
      End With
    End Using
  End Sub

  Private Sub PrintStep(product As ProductClass)
    Dim dyelot = product.Dyelot
    Dim redye = product.Redye
    Dim stepNumber = product.StepNumber

    Using dialogPrint As New System.Windows.Forms.PrintDialog
      With dialogPrint
        .AllowPrintToFile = False
        .AllowSelection = False
        .AllowSomePages = False

        .Document = (New PrintStep(controlCode, dyelot, redye, stepNumber)).Document
        If .Document IsNot Nothing Then dialogPrint.Document.Print()
      End With
    End Using
  End Sub

  Private Sub TextboxBarcode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextboxBarcode.KeyPress
    Select Case CurrentProduct.State
      Case ProductState.ScanUser
        If e.KeyChar = Chr(Keys.Enter) Then CheckScanUser()
      Case ProductState.ScanCode
        If e.KeyChar = Chr(Keys.Enter) Then CheckScanCode()
      Case ProductState.ScanLotNumber
        If e.KeyChar = Chr(Keys.Enter) Then CheckScanLotNumber()
      Case Else
        CurrentProduct.Barcode = ""
        TextboxBarcode.Text = ""
    End Select
  End Sub

  Private Sub CheckScanUser()
    If CurrentProduct.State <> ProductState.ScanUser Then Exit Sub
    If UserBadgeFound(TextboxBarcode.Text) Then
      UserFound = True
      CurrentProduct.State = ProductState.ScanCode
      TextboxBarcode.PasswordChar = Nothing
      TextboxBarcode.Text = Nothing
    Else
      UserFound = False
      MsgBox("User badge not found", MsgBoxStyle.Exclamation, "Adaptive Control")
      TextboxBarcode.PasswordChar = Nothing
      TextboxBarcode.Text = Nothing
    End If
  End Sub

  Private Function UserBadgeFound(badgeNumber As String) As Boolean
    If Users IsNot Nothing Then
      For Each row As DataRow In Users.Rows
        Dim userBadgeNumber = row("BadgeNo").ToString
        If userBadgeNumber = badgeNumber Then
          UserBadge = badgeNumber
          UserID = row("UserID").ToString
          UserName = row("FullName").ToString
          Return True
        End If
      Next
    Else
      MsgBox("User table not found", MsgBoxStyle.Exclamation, "Adaptive Control")
      Return False
    End If
    Return False
  End Function

  Private Sub CheckScanCode()
    If CurrentProduct.State <> ProductState.ScanCode Then Exit Sub
    If UCase(TextboxBarcode.Text) = UCase(CurrentProduct.Name) Then
      CurrentProduct.Barcode = TextboxBarcode.Text
      CurrentProduct.State = ProductState.ScanLotNumber
      TextboxBarcode.PasswordChar = Nothing
      TextboxBarcode.Text = Nothing
    Else
      MsgBox("Barcode does not match product code", MsgBoxStyle.Exclamation, "Adaptive Control")
      TextboxBarcode.PasswordChar = Nothing
      TextboxBarcode.Text = Nothing
    End If
  End Sub

  Private Sub CheckScanLotNumber()
    If CurrentProduct.State <> ProductState.ScanLotNumber Then Exit Sub
    If Not String.IsNullOrEmpty(TextboxBarcode.Text) Then
      CurrentProduct.LotNumber = TextboxBarcode.Text
      CurrentProduct.State = ProductState.SelectScale
      TextboxBarcode.PasswordChar = Nothing
      TextboxBarcode.Text = Nothing
    End If
  End Sub

  Private Sub ShowBalanceSelection()
    If CurrentBalance Is Nothing Then Exit Sub
    Using newForm As New FormMessage
      newForm.Connect(CurrentBalance)
      newForm.ShowDialog(Me)
    End Using
  End Sub

  Private Sub ButtonDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDone.Click
    If CurrentProduct.State = ProductState.Weigh Then
      CurrentProduct.State = ProductState.Done
    ElseIf CurrentProduct.State = ProductState.Off Then
      Done()
    End If
    SetFocus()
  End Sub

  Private Sub ButtonDemo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDemoWeigh.Click
    controlCode.DemoMode = Not controlCode.DemoMode
  End Sub

  Private Sub ButtonNextProduct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNextProduct.Click
    If CurrentProduct.State = ProductState.Weigh Then
      CurrentProduct.State = ProductState.Done
    End If
    SetFocus()
  End Sub

  Private Sub ButtonPreWeigh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPreWeigh.Click
    If CurrentProduct.State = ProductState.Weigh Then
      Dim Reply As MsgBoxResult
      Dim Style As MsgBoxStyle = CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Microsoft.VisualBasic.MsgBoxStyle)
      Reply = MsgBox("Are you sure you want to enter a pre-weighed amount", Style, "Adaptive Control")
      If Reply = MsgBoxResult.Yes Then
        Using newForm As New FormNumberPad
          newForm.Text = "Enter Pre-Weighed Amount (kg)"
          If newForm.ShowDialog(Me) = DialogResult.OK Then
            CurrentProduct.PreWeighedGrams = newForm.Value * 1000
          End If
        End Using
      End If
    End If
  End Sub

  Private Sub ButtonNewContainer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNewContainer.Click
    If CurrentProduct.State = ProductState.Weigh Then
      Dim Reply As MsgBoxResult
      Dim Style As MsgBoxStyle = CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Microsoft.VisualBasic.MsgBoxStyle)
      Reply = MsgBox("Are you sure you want to use a new container", Style, "Adaptive Control")
      If Reply = MsgBoxResult.Yes Then
        CurrentProduct.NewContainerGrams = TotalGrams
        CurrentProduct.State = ProductState.NewContainer
        FormNewContainer.ShowDialog()
        CurrentProduct.State = ProductState.Tare
      End If
    End If
    SetFocus()
  End Sub

  Private Sub ButtonSkipProduct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSkipProduct.Click
    If CurrentProduct.State >= ProductState.ScanCode AndAlso CurrentProduct.State <= ProductState.Weigh Then
      Dim Reply As MsgBoxResult
      Dim Style As MsgBoxStyle = CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Microsoft.VisualBasic.MsgBoxStyle)
      Reply = MsgBox("Are you sure you want to skip this product", Style, "Adaptive Control")
      If Reply = MsgBoxResult.Yes Then
        CurrentProduct.State = ProductState.SkipProduct
      End If
    End If
    SetFocus()
  End Sub

  Private Sub ButtonProductLotNumber_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLotNumber.Click
    If CurrentProduct.State <> ProductState.ScanLotNumber Then Exit Sub

    Using newForm As New FormNumberPad
      newForm.Text = "Enter Lot Number"
      If newForm.ShowDialog(Me) = DialogResult.OK Then
        TextboxBarcode.Text = newForm.Value.ToString
        CheckScanLotNumber()
      End If
    End Using
  End Sub

  Private Sub ButtonNoBarcode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNoBarcode.Click
    Select Case CurrentProduct.State
      Case ProductState.ScanCode : NoBarcodeScanCode()
      Case ProductState.ScanLotNumber : NoBarcodeScanLotNumber()
    End Select
  End Sub

  Private Sub NoBarcodeScanCode()
    Dim Reply As MsgBoxResult
    Dim Style As MsgBoxStyle = CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Reply = MsgBox("Are you sure you want to continue without scanning the product barcode", Style, "Adaptive Control")
    If Reply = MsgBoxResult.Yes Then
      If CurrentProduct.State = ProductState.ScanCode Then
        CurrentProduct.State = ProductState.ScanLotNumber
      End If
    End If
    SetFocus()
  End Sub

  Private Sub NoBarcodeScanLotNumber()
    Dim Reply As MsgBoxResult
    Dim Style As MsgBoxStyle = CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Reply = MsgBox("Are you sure you want to continue without scanning the product lot number", Style, "Adaptive Control")
    If Reply = MsgBoxResult.Yes Then
      If CurrentProduct.State = ProductState.ScanLotNumber Then
        CurrentProduct.State = ProductState.SelectScale
      End If
    End If
    SetFocus()
  End Sub

  Private Sub ButtonAbort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAbort.Click
    Dim Reply As MsgBoxResult
    Dim Style As MsgBoxStyle = CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, MsgBoxStyle)
    Reply = MsgBox("Are you sure you want to abort", Style, "Adaptive Control")
    If Reply = MsgBoxResult.Yes Then Done()
    SetFocus()
  End Sub

  Private Sub SetFocus()
    TextboxBarcode.SelectionLength = 0
    TextboxBarcode.SelectionStart = Len(TextboxBarcode.Text)
    TextboxBarcode.Focus()
  End Sub


  '--------------------------------------------------------------------------------------------
  ' Balance properties - use CurrentBalance
  '--------------------------------------------------------------------------------------------

  ' Includes preweigh and new container
  Private ReadOnly Property TotalGrams() As Double
    Get
      If CurrentBalance Is Nothing Then Return 0
      Return CurrentBalance.Grams + CurrentProduct.PreWeighedGrams + CurrentProduct.NewContainerGrams
    End Get
  End Property

  Private ReadOnly Property BalanceGrams As Double
    Get
      If CurrentBalance Is Nothing Then Return 0
      Return CurrentBalance.Grams
    End Get
  End Property

  Private ReadOnly Property BalanceTared() As Boolean
    Get
      If CurrentBalance Is Nothing Then Return False
      Return CurrentBalance.Tared
    End Get
  End Property

  Private Sub RunBalanceSimulation()
    If CurrentBalance Is Nothing Then Exit Sub

    Static startTime As Date
    If (CurrentProduct.State = ProductState.Weigh) OrElse (CurrentProduct.State = ProductState.Done) Then
      ' Calculate elapsed time since weigh start (time span)
      If startTime = Nothing Then startTime = Date.UtcNow()
      Dim elapsedTime = Date.UtcNow.Subtract(startTime)

      ' Make the weigh simulation take 10 seconds regardless of target weight
      Dim simGrams = Math.Min(CurrentProduct.Grams * (elapsedTime.TotalSeconds / 10), CurrentProduct.Grams)
      Dim simWeight = Utilities.Weight.ConvertWeight(simGrams, EBalanceUnits.Grams, CurrentBalance.WeightUnits)

      ' Set balance weight in the units specified by the balance 
      CurrentBalance.Weight = simWeight
    Else
      startTime = Nothing
      CurrentBalance.Weight = 0
    End If
  End Sub

  Private Sub Done()
    TimerMain.Enabled = False
    'FormMain.ButtonWeigh.Visible = False
    If Me.controlCode.CW.IsOn Then Me.controlCode.CW.Cancel()
    Me.Close()
  End Sub


#Region " Windows Form Designer generated code "

  Public Sub New(controlCode As ControlCode)
    MyBase.New()

    Me.controlCode = controlCode

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call


  End Sub
  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents ImageList16 As System.Windows.Forms.ImageList
  Friend WithEvents ButtonDone As System.Windows.Forms.Button
  Friend WithEvents ButtonAbort As System.Windows.Forms.Button
  Friend WithEvents GroupInstructions As System.Windows.Forms.GroupBox
  Friend WithEvents ButtonNoBarcode As System.Windows.Forms.Button
  Friend WithEvents ButtonNextProduct As System.Windows.Forms.Button

  Friend WithEvents ButtonDemoWeigh As System.Windows.Forms.Button

  Friend WithEvents GroupWeigh As System.Windows.Forms.GroupBox
  Friend WithEvents ProgressText As ProgressText
  Friend WithEvents ButtonNewContainer As System.Windows.Forms.Button
  Friend WithEvents LabelBarcode As System.Windows.Forms.Label
  Friend WithEvents TextboxBarcode As System.Windows.Forms.TextBox
  Friend WithEvents LabelProduct0 As System.Windows.Forms.Label
  Friend WithEvents LabelProduct1 As System.Windows.Forms.Label
  Friend WithEvents LabelProduct2 As System.Windows.Forms.Label
  Friend WithEvents LabelProduct3 As System.Windows.Forms.Label
  Friend WithEvents PictureboxProduct3 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureboxProduct2 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureboxProduct1 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureboxProduct0 As System.Windows.Forms.PictureBox
  Friend WithEvents ProgressBars As ProgressBars
  Friend WithEvents TimerMain As System.Windows.Forms.Timer
  Friend WithEvents LabelProduct4 As System.Windows.Forms.Label
  Friend WithEvents LabelProduct5 As System.Windows.Forms.Label
  Friend WithEvents LabelProduct6 As System.Windows.Forms.Label
  Friend WithEvents LabelProduct7 As System.Windows.Forms.Label
  Friend WithEvents ButtonSkipProduct As System.Windows.Forms.Button
  Friend WithEvents PictureboxProduct7 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureboxProduct6 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureboxProduct5 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureboxProduct4 As System.Windows.Forms.PictureBox
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormWeigh))
    Me.ButtonDone = New System.Windows.Forms.Button()
    Me.ImageList16 = New System.Windows.Forms.ImageList(Me.components)
    Me.ButtonAbort = New System.Windows.Forms.Button()
    Me.LabelProduct0 = New System.Windows.Forms.Label()
    Me.LabelProduct1 = New System.Windows.Forms.Label()
    Me.LabelProduct2 = New System.Windows.Forms.Label()
    Me.GroupInstructions = New System.Windows.Forms.GroupBox()
    Me.PictureboxProduct7 = New System.Windows.Forms.PictureBox()
    Me.LabelProduct7 = New System.Windows.Forms.Label()
    Me.PictureboxProduct6 = New System.Windows.Forms.PictureBox()
    Me.LabelProduct6 = New System.Windows.Forms.Label()
    Me.PictureboxProduct5 = New System.Windows.Forms.PictureBox()
    Me.LabelProduct5 = New System.Windows.Forms.Label()
    Me.PictureboxProduct4 = New System.Windows.Forms.PictureBox()
    Me.LabelProduct4 = New System.Windows.Forms.Label()
    Me.LabelBarcode = New System.Windows.Forms.Label()
    Me.TextboxBarcode = New System.Windows.Forms.TextBox()
    Me.PictureboxProduct3 = New System.Windows.Forms.PictureBox()
    Me.PictureboxProduct2 = New System.Windows.Forms.PictureBox()
    Me.PictureboxProduct1 = New System.Windows.Forms.PictureBox()
    Me.PictureboxProduct0 = New System.Windows.Forms.PictureBox()
    Me.LabelProduct3 = New System.Windows.Forms.Label()
    Me.ButtonNoBarcode = New System.Windows.Forms.Button()
    Me.TimerMain = New System.Windows.Forms.Timer(Me.components)
    Me.ButtonNextProduct = New System.Windows.Forms.Button()
    Me.GroupWeigh = New System.Windows.Forms.GroupBox()
    Me.ProgressBars = New ProgressBars()
    Me.ProgressText = New ProgressText()
    Me.ButtonNewContainer = New System.Windows.Forms.Button()
    Me.ButtonSkipProduct = New System.Windows.Forms.Button()
    Me.ButtonPreWeigh = New System.Windows.Forms.Button()
    Me.ButtonLotNumber = New System.Windows.Forms.Button()
    Me.ButtonDemoWeigh = New System.Windows.Forms.Button
    Me.GroupInstructions.SuspendLayout()
    CType(Me.PictureboxProduct7, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureboxProduct6, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureboxProduct5, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureboxProduct4, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureboxProduct3, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureboxProduct2, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureboxProduct1, System.ComponentModel.ISupportInitialize).BeginInit()
    CType(Me.PictureboxProduct0, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.GroupWeigh.SuspendLayout()
    Me.SuspendLayout()
    '
    'ButtonDone
    '
    Me.ButtonDone.BackColor = System.Drawing.Color.Silver
    Me.ButtonDone.Enabled = False
    Me.ButtonDone.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonDone.ForeColor = System.Drawing.Color.Green
    Me.ButtonDone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonDone.ImageIndex = 2
    Me.ButtonDone.ImageList = Me.ImageList16
    Me.ButtonDone.Location = New System.Drawing.Point(432, 16)
    Me.ButtonDone.Name = "ButtonDone"
    Me.ButtonDone.Size = New System.Drawing.Size(152, 56)
    Me.ButtonDone.TabIndex = 2
    Me.ButtonDone.TabStop = False
    Me.ButtonDone.Text = "Done"
    Me.ButtonDone.UseVisualStyleBackColor = False
    '
    'ImageList16
    '
    Me.ImageList16.ImageStream = CType(resources.GetObject("ImageList16.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.ImageList16.TransparentColor = System.Drawing.Color.Transparent
    Me.ImageList16.Images.SetKeyName(0, "")
    Me.ImageList16.Images.SetKeyName(1, "")
    Me.ImageList16.Images.SetKeyName(2, "")
    Me.ImageList16.Images.SetKeyName(3, "")
    Me.ImageList16.Images.SetKeyName(4, "")
    Me.ImageList16.Images.SetKeyName(5, "")
    Me.ImageList16.Images.SetKeyName(6, "")
    '
    'ButtonAbort
    '
    Me.ButtonAbort.BackColor = System.Drawing.Color.Silver
    Me.ButtonAbort.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonAbort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonAbort.ImageIndex = 5
    Me.ButtonAbort.ImageList = Me.ImageList16
    Me.ButtonAbort.Location = New System.Drawing.Point(432, 464)
    Me.ButtonAbort.Name = "ButtonAbort"
    Me.ButtonAbort.Size = New System.Drawing.Size(152, 32)
    Me.ButtonAbort.TabIndex = 6
    Me.ButtonAbort.TabStop = False
    Me.ButtonAbort.Text = "Abort"
    Me.ButtonAbort.UseVisualStyleBackColor = False
    '
    'LabelProduct0
    '
    Me.LabelProduct0.AutoEllipsis = True
    Me.LabelProduct0.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct0.Location = New System.Drawing.Point(40, 24)
    Me.LabelProduct0.Name = "LabelProduct0"
    Me.LabelProduct0.Size = New System.Drawing.Size(352, 32)
    Me.LabelProduct0.TabIndex = 0
    Me.LabelProduct0.Text = "Product0"
    Me.LabelProduct0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LabelProduct0.Visible = False
    '
    'LabelProduct1
    '
    Me.LabelProduct1.AutoEllipsis = True
    Me.LabelProduct1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct1.Location = New System.Drawing.Point(40, 56)
    Me.LabelProduct1.Name = "LabelProduct1"
    Me.LabelProduct1.Size = New System.Drawing.Size(352, 32)
    Me.LabelProduct1.TabIndex = 1
    Me.LabelProduct1.Text = "Product1"
    Me.LabelProduct1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LabelProduct1.Visible = False
    '
    'LabelProduct2
    '
    Me.LabelProduct2.AutoEllipsis = True
    Me.LabelProduct2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct2.Location = New System.Drawing.Point(40, 88)
    Me.LabelProduct2.Name = "LabelProduct2"
    Me.LabelProduct2.Size = New System.Drawing.Size(352, 32)
    Me.LabelProduct2.TabIndex = 2
    Me.LabelProduct2.Text = "Product2"
    Me.LabelProduct2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LabelProduct2.Visible = False
    '
    'GroupInstructions
    '
    Me.GroupInstructions.Controls.Add(Me.PictureboxProduct7)
    Me.GroupInstructions.Controls.Add(Me.LabelProduct7)
    Me.GroupInstructions.Controls.Add(Me.PictureboxProduct6)
    Me.GroupInstructions.Controls.Add(Me.LabelProduct6)
    Me.GroupInstructions.Controls.Add(Me.PictureboxProduct5)
    Me.GroupInstructions.Controls.Add(Me.LabelProduct5)
    Me.GroupInstructions.Controls.Add(Me.PictureboxProduct4)
    Me.GroupInstructions.Controls.Add(Me.LabelProduct4)
    Me.GroupInstructions.Controls.Add(Me.LabelBarcode)
    Me.GroupInstructions.Controls.Add(Me.TextboxBarcode)
    Me.GroupInstructions.Controls.Add(Me.PictureboxProduct3)
    Me.GroupInstructions.Controls.Add(Me.PictureboxProduct2)
    Me.GroupInstructions.Controls.Add(Me.PictureboxProduct1)
    Me.GroupInstructions.Controls.Add(Me.PictureboxProduct0)
    Me.GroupInstructions.Controls.Add(Me.LabelProduct3)
    Me.GroupInstructions.Controls.Add(Me.LabelProduct0)
    Me.GroupInstructions.Controls.Add(Me.LabelProduct1)
    Me.GroupInstructions.Controls.Add(Me.LabelProduct2)
    Me.GroupInstructions.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.GroupInstructions.Location = New System.Drawing.Point(8, 8)
    Me.GroupInstructions.Name = "GroupInstructions"
    Me.GroupInstructions.Size = New System.Drawing.Size(408, 312)
    Me.GroupInstructions.TabIndex = 0
    Me.GroupInstructions.TabStop = False
    Me.GroupInstructions.Text = "Please follow the instructions below"
    '
    'PictureboxProduct7
    '
    Me.PictureboxProduct7.Image = CType(resources.GetObject("PictureboxProduct7.Image"), System.Drawing.Image)
    Me.PictureboxProduct7.Location = New System.Drawing.Point(16, 256)
    Me.PictureboxProduct7.Name = "PictureboxProduct7"
    Me.PictureboxProduct7.Size = New System.Drawing.Size(16, 16)
    Me.PictureboxProduct7.TabIndex = 23
    Me.PictureboxProduct7.TabStop = False
    Me.PictureboxProduct7.Visible = False
    '
    'LabelProduct7
    '
    Me.LabelProduct7.AutoEllipsis = True
    Me.LabelProduct7.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct7.Location = New System.Drawing.Point(40, 248)
    Me.LabelProduct7.Name = "LabelProduct7"
    Me.LabelProduct7.Size = New System.Drawing.Size(352, 32)
    Me.LabelProduct7.TabIndex = 22
    Me.LabelProduct7.Text = "Product7"
    Me.LabelProduct7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LabelProduct7.Visible = False
    '
    'PictureboxProduct6
    '
    Me.PictureboxProduct6.Image = CType(resources.GetObject("PictureboxProduct6.Image"), System.Drawing.Image)
    Me.PictureboxProduct6.Location = New System.Drawing.Point(16, 224)
    Me.PictureboxProduct6.Name = "PictureboxProduct6"
    Me.PictureboxProduct6.Size = New System.Drawing.Size(16, 16)
    Me.PictureboxProduct6.TabIndex = 21
    Me.PictureboxProduct6.TabStop = False
    Me.PictureboxProduct6.Visible = False
    '
    'LabelProduct6
    '
    Me.LabelProduct6.AutoEllipsis = True
    Me.LabelProduct6.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct6.Location = New System.Drawing.Point(40, 216)
    Me.LabelProduct6.Name = "LabelProduct6"
    Me.LabelProduct6.Size = New System.Drawing.Size(352, 32)
    Me.LabelProduct6.TabIndex = 20
    Me.LabelProduct6.Text = "Product6"
    Me.LabelProduct6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LabelProduct6.Visible = False
    '
    'PictureboxProduct5
    '
    Me.PictureboxProduct5.Image = CType(resources.GetObject("PictureboxProduct5.Image"), System.Drawing.Image)
    Me.PictureboxProduct5.Location = New System.Drawing.Point(16, 192)
    Me.PictureboxProduct5.Name = "PictureboxProduct5"
    Me.PictureboxProduct5.Size = New System.Drawing.Size(16, 16)
    Me.PictureboxProduct5.TabIndex = 19
    Me.PictureboxProduct5.TabStop = False
    Me.PictureboxProduct5.Visible = False
    '
    'LabelProduct5
    '
    Me.LabelProduct5.AutoEllipsis = True
    Me.LabelProduct5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct5.Location = New System.Drawing.Point(40, 184)
    Me.LabelProduct5.Name = "LabelProduct5"
    Me.LabelProduct5.Size = New System.Drawing.Size(352, 32)
    Me.LabelProduct5.TabIndex = 18
    Me.LabelProduct5.Text = "Product5"
    Me.LabelProduct5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LabelProduct5.Visible = False
    '
    'PictureboxProduct4
    '
    Me.PictureboxProduct4.Image = CType(resources.GetObject("PictureboxProduct4.Image"), System.Drawing.Image)
    Me.PictureboxProduct4.Location = New System.Drawing.Point(16, 160)
    Me.PictureboxProduct4.Name = "PictureboxProduct4"
    Me.PictureboxProduct4.Size = New System.Drawing.Size(16, 16)
    Me.PictureboxProduct4.TabIndex = 17
    Me.PictureboxProduct4.TabStop = False
    Me.PictureboxProduct4.Visible = False
    '
    'LabelProduct4
    '
    Me.LabelProduct4.AutoEllipsis = True
    Me.LabelProduct4.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct4.Location = New System.Drawing.Point(40, 152)
    Me.LabelProduct4.Name = "LabelProduct4"
    Me.LabelProduct4.Size = New System.Drawing.Size(352, 32)
    Me.LabelProduct4.TabIndex = 16
    Me.LabelProduct4.Text = "Product4"
    Me.LabelProduct4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LabelProduct4.Visible = False
    '
    'LabelBarcode
    '
    Me.LabelBarcode.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelBarcode.Location = New System.Drawing.Point(16, 288)
    Me.LabelBarcode.Name = "LabelBarcode"
    Me.LabelBarcode.Size = New System.Drawing.Size(64, 16)
    Me.LabelBarcode.TabIndex = 15
    Me.LabelBarcode.Text = "Barcode: "
    Me.LabelBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'TextboxBarcode
    '
    Me.TextboxBarcode.BorderStyle = System.Windows.Forms.BorderStyle.None
    Me.TextboxBarcode.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.TextboxBarcode.Location = New System.Drawing.Point(80, 288)
    Me.TextboxBarcode.Name = "TextboxBarcode"
    Me.TextboxBarcode.Size = New System.Drawing.Size(312, 16)
    Me.TextboxBarcode.TabIndex = 14
    '
    'PictureboxProduct3
    '
    Me.PictureboxProduct3.Image = CType(resources.GetObject("PictureboxProduct3.Image"), System.Drawing.Image)
    Me.PictureboxProduct3.Location = New System.Drawing.Point(16, 128)
    Me.PictureboxProduct3.Name = "PictureboxProduct3"
    Me.PictureboxProduct3.Size = New System.Drawing.Size(16, 16)
    Me.PictureboxProduct3.TabIndex = 13
    Me.PictureboxProduct3.TabStop = False
    Me.PictureboxProduct3.Visible = False
    '
    'PictureboxProduct2
    '
    Me.PictureboxProduct2.Image = CType(resources.GetObject("PictureboxProduct2.Image"), System.Drawing.Image)
    Me.PictureboxProduct2.Location = New System.Drawing.Point(16, 96)
    Me.PictureboxProduct2.Name = "PictureboxProduct2"
    Me.PictureboxProduct2.Size = New System.Drawing.Size(16, 16)
    Me.PictureboxProduct2.TabIndex = 12
    Me.PictureboxProduct2.TabStop = False
    Me.PictureboxProduct2.Visible = False
    '
    'PictureboxProduct1
    '
    Me.PictureboxProduct1.Image = CType(resources.GetObject("PictureboxProduct1.Image"), System.Drawing.Image)
    Me.PictureboxProduct1.Location = New System.Drawing.Point(16, 64)
    Me.PictureboxProduct1.Name = "PictureboxProduct1"
    Me.PictureboxProduct1.Size = New System.Drawing.Size(16, 16)
    Me.PictureboxProduct1.TabIndex = 11
    Me.PictureboxProduct1.TabStop = False
    Me.PictureboxProduct1.Visible = False
    '
    'PictureboxProduct0
    '
    Me.PictureboxProduct0.Image = CType(resources.GetObject("PictureboxProduct0.Image"), System.Drawing.Image)
    Me.PictureboxProduct0.Location = New System.Drawing.Point(16, 32)
    Me.PictureboxProduct0.Name = "PictureboxProduct0"
    Me.PictureboxProduct0.Size = New System.Drawing.Size(16, 16)
    Me.PictureboxProduct0.TabIndex = 10
    Me.PictureboxProduct0.TabStop = False
    Me.PictureboxProduct0.Visible = False
    '
    'LabelProduct3
    '
    Me.LabelProduct3.AutoEllipsis = True
    Me.LabelProduct3.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct3.Location = New System.Drawing.Point(40, 120)
    Me.LabelProduct3.Name = "LabelProduct3"
    Me.LabelProduct3.Size = New System.Drawing.Size(352, 32)
    Me.LabelProduct3.TabIndex = 3
    Me.LabelProduct3.Text = "Product3"
    Me.LabelProduct3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.LabelProduct3.Visible = False
    '
    'ButtonNoBarcode
    '
    Me.ButtonNoBarcode.BackColor = System.Drawing.Color.Silver
    Me.ButtonNoBarcode.Enabled = False
    Me.ButtonNoBarcode.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonNoBarcode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonNoBarcode.ImageIndex = 4
    Me.ButtonNoBarcode.ImageList = Me.ImageList16
    Me.ButtonNoBarcode.Location = New System.Drawing.Point(432, 418)
    Me.ButtonNoBarcode.Name = "ButtonNoBarcode"
    Me.ButtonNoBarcode.Size = New System.Drawing.Size(152, 32)
    Me.ButtonNoBarcode.TabIndex = 5
    Me.ButtonNoBarcode.TabStop = False
    Me.ButtonNoBarcode.Text = "No Barcode"
    Me.ButtonNoBarcode.UseVisualStyleBackColor = False
    '
    'TimerMain
    '
    Me.TimerMain.Interval = 250
    '
    'ButtonNextProduct
    '
    Me.ButtonNextProduct.BackColor = System.Drawing.Color.Silver
    Me.ButtonNextProduct.Enabled = False
    Me.ButtonNextProduct.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonNextProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonNextProduct.ImageIndex = 3
    Me.ButtonNextProduct.ImageList = Me.ImageList16
    Me.ButtonNextProduct.Location = New System.Drawing.Point(432, 80)
    Me.ButtonNextProduct.Name = "ButtonNextProduct"
    Me.ButtonNextProduct.Size = New System.Drawing.Size(152, 32)
    Me.ButtonNextProduct.TabIndex = 3
    Me.ButtonNextProduct.TabStop = False
    Me.ButtonNextProduct.Text = "Next Product"
    Me.ButtonNextProduct.UseVisualStyleBackColor = False
    '
    'GroupWeigh
    '
    Me.GroupWeigh.Controls.Add(Me.ProgressBars)
    Me.GroupWeigh.Controls.Add(Me.ProgressText)
    Me.GroupWeigh.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.GroupWeigh.Location = New System.Drawing.Point(8, 336)
    Me.GroupWeigh.Name = "GroupWeigh"
    Me.GroupWeigh.Size = New System.Drawing.Size(408, 160)
    Me.GroupWeigh.TabIndex = 1
    Me.GroupWeigh.TabStop = False
    Me.GroupWeigh.Text = "Please weigh the amount below on scale "
    '
    'ProgressBars
    '
    Me.ProgressBars.Location = New System.Drawing.Point(16, 88)
    Me.ProgressBars.Name = "ProgressBars"
    Me.ProgressBars.Size = New System.Drawing.Size(376, 64)
    Me.ProgressBars.TabIndex = 1
    '
    'ProgressText
    '
    Me.ProgressText.ForeColor = System.Drawing.Color.DimGray
    Me.ProgressText.Location = New System.Drawing.Point(12, 24)
    Me.ProgressText.Name = "ProgressText"
    Me.ProgressText.Size = New System.Drawing.Size(380, 64)
    Me.ProgressText.TabIndex = 0
    '
    'ButtonNewContainer
    '
    Me.ButtonNewContainer.BackColor = System.Drawing.Color.Silver
    Me.ButtonNewContainer.Enabled = False
    Me.ButtonNewContainer.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonNewContainer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonNewContainer.ImageIndex = 1
    Me.ButtonNewContainer.ImageList = Me.ImageList16
    Me.ButtonNewContainer.Location = New System.Drawing.Point(432, 288)
    Me.ButtonNewContainer.Name = "ButtonNewContainer"
    Me.ButtonNewContainer.Size = New System.Drawing.Size(152, 32)
    Me.ButtonNewContainer.TabIndex = 4
    Me.ButtonNewContainer.TabStop = False
    Me.ButtonNewContainer.Text = "Partial Weigh"
    Me.ButtonNewContainer.UseVisualStyleBackColor = False
    '
    'ButtonSkipProduct
    '
    Me.ButtonSkipProduct.BackColor = System.Drawing.Color.Silver
    Me.ButtonSkipProduct.Enabled = False
    Me.ButtonSkipProduct.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonSkipProduct.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonSkipProduct.ImageIndex = 6
    Me.ButtonSkipProduct.ImageList = Me.ImageList16
    Me.ButtonSkipProduct.Location = New System.Drawing.Point(432, 334)
    Me.ButtonSkipProduct.Name = "ButtonSkipProduct"
    Me.ButtonSkipProduct.Size = New System.Drawing.Size(152, 32)
    Me.ButtonSkipProduct.TabIndex = 9
    Me.ButtonSkipProduct.TabStop = False
    Me.ButtonSkipProduct.Text = "Skip Product"
    Me.ButtonSkipProduct.UseVisualStyleBackColor = False
    '
    'ButtonPreWeigh
    '
    Me.ButtonPreWeigh.BackColor = System.Drawing.Color.Silver
    Me.ButtonPreWeigh.Enabled = False
    Me.ButtonPreWeigh.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonPreWeigh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonPreWeigh.ImageIndex = 1
    Me.ButtonPreWeigh.ImageList = Me.ImageList16
    Me.ButtonPreWeigh.Location = New System.Drawing.Point(432, 250)
    Me.ButtonPreWeigh.Name = "ButtonPreWeigh"
    Me.ButtonPreWeigh.Size = New System.Drawing.Size(152, 32)
    Me.ButtonPreWeigh.TabIndex = 11
    Me.ButtonPreWeigh.TabStop = False
    Me.ButtonPreWeigh.Text = "Pre Weighed"
    Me.ButtonPreWeigh.UseVisualStyleBackColor = False
    '
    'ButtonLotNumber
    '
    Me.ButtonLotNumber.BackColor = System.Drawing.Color.Silver
    Me.ButtonLotNumber.Enabled = False
    Me.ButtonLotNumber.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonLotNumber.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonLotNumber.ImageIndex = 0
    Me.ButtonLotNumber.ImageList = Me.ImageList16
    Me.ButtonLotNumber.Location = New System.Drawing.Point(432, 380)
    Me.ButtonLotNumber.Name = "ButtonLotNumber"
    Me.ButtonLotNumber.Size = New System.Drawing.Size(152, 32)
    Me.ButtonLotNumber.TabIndex = 12
    Me.ButtonLotNumber.TabStop = False
    Me.ButtonLotNumber.Text = "Lot Number"
    Me.ButtonLotNumber.UseVisualStyleBackColor = False
    '
    'ButtonDemoWeigh
    '
    Me.ButtonDemoWeigh.BackColor = System.Drawing.Color.Silver
    Me.ButtonDemoWeigh.Enabled = False
    Me.ButtonDemoWeigh.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonDemoWeigh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonDemoWeigh.ImageIndex = 0
    Me.ButtonDemoWeigh.ImageList = Me.ImageList16
    Me.ButtonDemoWeigh.Location = New System.Drawing.Point(432, 125)
    Me.ButtonDemoWeigh.Name = "ButtonDemoWeigh"
    Me.ButtonDemoWeigh.Size = New System.Drawing.Size(152, 32)
    Me.ButtonDemoWeigh.TabIndex = 12
    Me.ButtonDemoWeigh.TabStop = False
    Me.ButtonDemoWeigh.Text = "Demo Weigh"
    Me.ButtonDemoWeigh.UseVisualStyleBackColor = False
    '
    'FormWeigh
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 16)
    Me.ClientSize = New System.Drawing.Size(594, 504)
    Me.ControlBox = False
    Me.Controls.Add(Me.ButtonLotNumber)       ' Location (432, 380)
    Me.Controls.Add(Me.ButtonPreWeigh)        ' Location (432, 250)
    Me.Controls.Add(Me.ButtonSkipProduct)     ' Location (432, 334)
    Me.Controls.Add(Me.ButtonNewContainer)    ' Location (432, 288)
    Me.Controls.Add(Me.GroupWeigh)            ' 
    Me.Controls.Add(Me.ButtonNextProduct)     ' Location (432, 80)
    Me.Controls.Add(Me.ButtonNoBarcode)       ' Location (432, 418)
    Me.Controls.Add(Me.GroupInstructions)     ' 
    Me.Controls.Add(Me.ButtonAbort)           ' Location (432, 464)
    Me.Controls.Add(Me.ButtonDone)            ' Location (432, 16)
    Me.Controls.Add(Me.ButtonDemoWeigh)       ' Location (432, 125)
    Me.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "FormWeigh"
    Me.ShowInTaskbar = False
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Adaptive Check Weigh"
    Me.GroupInstructions.ResumeLayout(False)
    Me.GroupInstructions.PerformLayout()
    CType(Me.PictureboxProduct7, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureboxProduct6, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureboxProduct5, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureboxProduct4, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureboxProduct3, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureboxProduct2, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureboxProduct1, System.ComponentModel.ISupportInitialize).EndInit()
    CType(Me.PictureboxProduct0, System.ComponentModel.ISupportInitialize).EndInit()
    Me.GroupWeigh.ResumeLayout(False)
    Me.ResumeLayout(False)

  End Sub

#End Region

End Class