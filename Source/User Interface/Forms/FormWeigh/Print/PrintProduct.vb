Imports System.Drawing.Printing
Imports Utilities.Sql

Public Class PrintProduct
  Private controlCode As ControlCode

  Property Document As System.Drawing.Printing.PrintDocument

  Property Font As New Font("Tahoma", 11)

  Property BarcodeFont As New Font(controlCode.Settings.BarcodeFontName, controlCode.Settings.BarcodeFontSize)

  Property DocumentWidth As Integer                          ' Document width from the printer
  Property PageMarginLeft As Integer = 8                     ' Page margins we are going to use
  Property PageMarginRight As Integer = 8                    ' 
  Property PageMarginTop As Integer = 8                      ' 


  Private sqlConnection As String

  Private dyelot As String
  Private redye As Integer
  Private stepNumber As Integer
  Private stepID As Integer

  Private batchRow As DataRow
  Private batchStep As DataRow

  Private alignLeft As New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
  Private alignCenter As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
  Private alignRight As New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}

  Public Sub New(controlcode As ControlCode, dyelot As String, redye As Integer, stepNumber As Integer, stepID As Integer)


    Me.controlCode = controlcode
    sqlConnection = controlcode.Settings.SqlConnection

    Me.dyelot = dyelot
    Me.redye = redye
    Me.stepNumber = stepNumber
    Me.stepID = stepID

    Me.batchRow = GetBatchRow(dyelot, redye)
    Me.batchStep = GetBatchStep(stepID)

    Document = New PrintDocument
    Document.DefaultPageSettings.Landscape = False
    AddHandler Document.PrintPage, AddressOf PrintAPage
  End Sub

  Private Function GetBatchRow(dyelot As String, redye As Integer) As DataRow
    Dim sql As String = Nothing
    Try
      sql = "SELECT * FROM Dyelots WHERE Dyelot=" & Utilities.Sql.SqlString(dyelot) & " AND ReDye=" & redye.ToString
      Return Utilities.Sql.GetDataRow(sqlConnection, sql)
    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
    Return Nothing
  End Function

  Private Function GetBatchStep(stepID As Integer) As DataRow
    Dim sql As String = Nothing
    Try
      sql = "SELECT * FROM DyelotsBulkedRecipe WHERE ID=" & stepID.ToString
      Return Utilities.Sql.GetDataRow(sqlConnection, sql)
    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
    Return Nothing
  End Function

  Public Sub PrintAPage(ByVal sender As Object, ByVal args As PrintPageEventArgs)
    DocumentWidth = args.PageBounds.Width - (PageMarginLeft + PageMarginRight)
    Using g As Graphics = args.Graphics
      PrintDetail(g)
    End Using
  End Sub

  Private Sub PrintDetail(g As Graphics)
    Dim machine = batchRow("Machine").ToString
    Dim batch = GetBatchText(batchRow)
    Dim stepNumber = batchStep("StepNumber").ToString
    Dim productCode = GetProductCode(batchStep)
    Dim product = GetProductText(batchStep)
    Dim dispenseAmount = GetDispenseAmount(batchStep)
    Dim dispenseBy = GetDispenseBy(batchStep)

    ' Set height in print Sub
    Dim printRect As New RectangleF(PageMarginLeft, PageMarginTop, DocumentWidth, 0)

    Using brush As New SolidBrush(Color.Black)
      PrintBarcode(g, brush, printRect, "*" & productCode & "*")
      PrintRow(g, brush, printRect, machine)
      PrintRow(g, brush, printRect, "Batch:  " & batch)
      PrintRow(g, brush, printRect, "Step:  " & stepNumber)
      PrintRow(g, brush, printRect, "Product:  " & product)
      PrintRow(g, brush, printRect, "Dispensed:  " & dispenseAmount)
      PrintRow(g, brush, printRect, "Dispense By:  " & dispenseBy)
      PrintRow(g, brush, printRect, "Dispense Time:  " & Date.Now.ToString)
    End Using
  End Sub

  Private Sub PrintBarcode(ByRef g As Graphics, ByRef brush As SolidBrush, ByRef printRect As RectangleF, printText As String)
    printRect.Height = BarcodeFont.Height + 8
    g.DrawString(printText, BarcodeFont, brush, printRect, alignCenter)
    Dim newY = printRect.Y + printRect.Height
    printRect.Y = newY
  End Sub

  Private Sub PrintRow(ByRef g As Graphics, ByRef brush As SolidBrush, ByRef printRect As RectangleF, printText As String)
    printRect.Height = Font.Height + 4
    g.DrawString(printText, Font, brush, printRect, alignLeft)
    Dim newY = printRect.Y + printRect.Height
    printRect.Y = newY
  End Sub

  Private Function GetBatchText(batchRow As DataRow) As String
    Dim batch = batchRow("Dyelot").ToString
    Dim redye = Utilities.Sql.NullToZeroInteger(batchRow("ReDye"))

    If redye = 0 Then
      Return batch
    Else
      Return batch & "@" & redye.ToString
    End If
  End Function

  Private Function GetProductCode(batchStep As DataRow) As String
    Dim stepCode = batchStep("StepCode").ToString
    Return stepCode
  End Function

  Private Function GetProductText(batchStep As DataRow) As String
    Dim stepCode = batchStep("StepCode").ToString
    Dim stepDescription = batchStep("StepDescription").ToString

    Return stepCode & " " & stepDescription
  End Function

  Private Function GetDispenseAmount(batchStep As DataRow) As String
    Dim grams = Utilities.Sql.NullToZeroDouble(batchStep("DispenseGrams"))

    Return grams.ToString("#0.00") & " grams"
  End Function

  Private Function GetDispenseBy(batchStep As DataRow) As String
    If batchStep.Table.Columns.Contains("DispenseBy") Then Return batchStep("DispenseBy").ToString
    Return Nothing
  End Function

End Class
