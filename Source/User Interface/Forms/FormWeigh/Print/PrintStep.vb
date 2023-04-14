Imports System.Drawing.Printing
Imports Utilities.Sql

Public Class PrintStep
  Private controlCode As ControlCode

  Property Document As System.Drawing.Printing.PrintDocument

  Property HeaderFont As New Font("Tahoma", 11)
  Property DetailFont As New Font("Tahoma", 11)

  Property DocumentWidth As Integer
  Property HeaderHeight As Integer
  Property DetailHeight As Integer

  Private sqlConnection As String

  Private pageMarginLeft As Integer = 8                     ' Page margins
  Private pageMarginRight As Integer = 8                    ' 
  Private pageMarginTop As Integer = 8                      ' 

  Private dyelot As String
  Private redye As Integer
  Private stepNumber As Integer

  Private batchRow As DataRow
  Private batchSteps As DataTable

  Private alignLeft As New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
  Private alignCenter As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
  Private alignRight As New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}

  Public Sub New(controlcode As ControlCode, dyelot As String, redye As Integer, stepNumber As Integer)
    Me.controlCode = controlcode
    sqlConnection = controlcode.Settings.SqlConnection

    Me.dyelot = dyelot
    Me.redye = redye
    Me.stepNumber = stepNumber

    Me.batchRow = GetBatchRow(dyelot, redye)
    Me.batchSteps = GetBatchSteps(batchRow, stepNumber)

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

  Private Function GetBatchSteps(batchRow As DataRow, stepNumber As Integer) As DataTable
    Dim sql As String = Nothing
    Try
      Dim id = Utilities.Sql.NullToZeroInteger(batchRow("ID"))
      sql = "SELECT * FROM DyelotsBulkedRecipe WHERE DyelotID=" & id.ToString & " AND StepNumber=" & stepNumber.ToString & " ORDER BY DisplayOrder"
      Return Utilities.Sql.GetDataTable(sqlConnection, sql)
    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
    Return Nothing
  End Function

  Public Sub PrintAPage(ByVal sender As Object, ByVal args As PrintPageEventArgs)
    ' Calculate document width
    DocumentWidth = args.PageBounds.Width - (pageMarginLeft + pageMarginRight)

    Using g As Graphics = args.Graphics
      PrintHeader(g, DocumentWidth, batchRow, batchSteps)
      PrintDetail(g, DocumentWidth, batchRow, batchSteps)
    End Using
  End Sub

  Private Sub PrintHeader(g As Graphics, documentWidth As Integer, batchRow As DataRow, batchSteps As DataTable)
    Dim machine = batchRow("Machine").ToString
    Dim batch = batchRow("Dyelot").ToString
    Dim redye = Utilities.Sql.NullToZeroInteger(batchRow("ReDye"))
    If redye > 0 Then batch &= "@" & redye.ToString

    Dim headerText = machine & ": " & batch & ", Step " & stepNumber.ToString

    HeaderHeight = HeaderFont.Height + 4
    Dim headerRect As New RectangleF(pageMarginLeft, pageMarginTop, documentWidth, HeaderHeight)

    Using brush As New SolidBrush(Color.Black)
      g.DrawString(headerText, HeaderFont, brush, headerRect, alignLeft)
    End Using

    Dim startX = pageMarginLeft
    Dim endX = startX + documentWidth
    Dim posY = CInt(headerRect.Y + headerRect.Height)

    Using pen As New Pen(Color.Black, 1)
      g.DrawLine(pen, startX, posY, endX, posY)
    End Using
  End Sub

  Private Sub PrintDetail(g As Graphics, documentWidth As Integer, batchRow As DataRow, batchSteps As DataTable)
    Dim rowNumber As Integer
    For Each row As DataRow In batchSteps.Rows
      Dim stepTypeID = Utilities.Sql.NullToZeroInteger(row("StepTypeID"))
      If stepTypeID = 1 OrElse stepTypeID = 2 Then
        rowNumber += 1
        PrintDetailRow(g, documentWidth, row, rowNumber)
      End If
    Next
  End Sub

  Private Sub PrintDetailRow(g As Graphics, documentWidth As Integer, batchRow As DataRow, rowNumber As Integer)
    Try
      Dim stepCode = batchRow("StepCode").ToString
      Dim stepDescription = batchRow("StepDescription").ToString
      Dim grams = Utilities.Sql.NullToZeroDouble(batchRow("DispenseGrams"))

      Dim stepText = stepCode & " " & stepDescription
      Dim dispenseText = grams.ToString("#0") & " grams"

      Dim startY = pageMarginTop + HeaderHeight

      Dim rowWidth = documentWidth
      Dim rowHeight As Integer = DetailFont.Height + 4
      Dim rowX As Integer = pageMarginLeft
      Dim rowY As Integer = startY + ((rowNumber - 1) * rowHeight)

      Dim detailRect As New RectangleF(rowX, rowY, rowWidth, rowHeight)

      Using brush As New SolidBrush(Color.Black)
        g.DrawString(stepText, DetailFont, brush, detailRect, alignLeft)
        g.DrawString(dispenseText, DetailFont, brush, detailRect, alignRight)
      End Using

      DetailHeight = (rowNumber * rowHeight)
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

End Class
