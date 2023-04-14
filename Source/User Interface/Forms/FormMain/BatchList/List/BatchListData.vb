Imports Utilities.Sql

Public Class BatchListData
  Private ReadOnly controlcode As ControlCode

  Property SqlConnection As String
  Property SqlSelect As String
  Property Batches As DataTable
  Property BatchesFiltered As DataTable

  Public Sub New(controlCode As ControlCode)
    MyBase.New

    Me.controlcode = controlCode
  End Sub

  Sub Requery()
    Try
      SqlConnection = controlcode.Settings.SqlConnection
      SqlSelect = GetSqlSelect()

      ' Get dyelots and add some data so we can see how many dyes / chemical have been dispensed
      Dim table = Utilities.Sql.GetDataTable(SqlConnection, SqlSelect, "Dyelots")
      AddCountData(table)
      Batches = table

      ' Apply the row filter if set
      If Not String.IsNullOrEmpty(controlcode.Settings.BatchListRowFilter) Then
        Batches.DefaultView.RowFilter = controlcode.Settings.BatchListRowFilter
      End If

    Catch ex As Exception
      Utilities.Log.LogError(ex, SqlSelect)
    End Try
  End Sub

  Private Function GetSqlSelect() As String
    Dim sql As String = Nothing
    Try
      sql = controlcode.Settings.SqlSelectBatches
      Dim sqlWhere As String = "WHERE ((State=2) OR (State IS NULL))"

      If controlcode.Settings.ScheduledOnly Then sqlWhere = "WHERE ((State=2) OR (State IS NULL)) AND (StartTime Is Not Null)"

      Return sql.Replace("%WHERE", sqlWhere)
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return sql
  End Function

  Private Sub AddCountData(table As DataTable)
    Dim sql As String = Nothing
    Try
      ' TODO - Added 2020-11-02
      If table Is Nothing Then
        Exit Sub
      Else
        AddColumn(table, "StepCount")
        AddColumn(table, "ProductCount")
        AddColumn(table, "DispenseCount")
        AddColumn(table, "DyeCount")
        AddColumn(table, "DyeDispenseCount")
        AddColumn(table, "DyeDispenseDone")
        AddColumn(table, "ChemicalCount")
        AddColumn(table, "ChemicalDispenseCount")
        AddColumn(table, "ChemicalDispenseDone")

        For Each row As DataRow In table.Rows
          Dim dyelotID = Utilities.Sql.NullToZeroInteger(row("ID"))

          sql = "SELECT * FROM DyelotsBulkedRecipe WHERE DyelotID=" & dyelotID.ToString & " AND (StepTypeID=1 OR StepTypeID=2)"
          Dim batchSteps = Utilities.Sql.GetDataTable(SqlConnection, sql)

          If batchSteps IsNot Nothing Then
            row("StepCount") = GetStepCount(batchSteps)
            row("ProductCount") = GetProductCount(batchSteps, 0)

            ' Only fill this in if the count > 0, it scans better with an empty cell 
            Dim dispenseCount = GetDispenseCount(batchSteps, 0)
            If dispenseCount > 0 Then row("DispenseCount") = dispenseCount

            ' Fill in dye count columns
            Dim dyeCount = GetProductCount(batchSteps, 1)
            Dim dyeDispenseCount = GetDispenseCount(batchSteps, 1)
            row("DyeCount") = dyeCount
            If dyeDispenseCount > 0 Then row("DyeDispenseCount") = dyeDispenseCount
            If dyeDispenseCount >= dyeCount Then
              row("DyeDispenseDone") = 1
            Else
              row("DyeDispenseDone") = 0
            End If

            ' Fill in chemical count columns
            Dim chemicalCount = GetProductCount(batchSteps, 2)
            Dim chemicalDispenseCount = GetDispenseCount(batchSteps, 2)
            row("ChemicalCount") = chemicalCount
            If chemicalDispenseCount > 0 Then row("ChemicalDispenseCount") = chemicalDispenseCount
            If chemicalDispenseCount >= chemicalCount Then
              row("ChemicalDispenseDone") = 1
            Else
              row("ChemicalDispenseDone") = 0
            End If
          End If
        Next row
      End If


    Catch ex As Exception
      Utilities.Log.LogError(ex, sql)
    End Try
  End Sub

  Private Function GetStepCount(batchSteps As DataTable) As Integer
    ' Return the highest step number
    Try
      If batchSteps Is Nothing Then Return 0

      Dim stepNumberMax As Integer = 0
      For Each row As DataRow In batchSteps.Rows
        Dim stepNumber = Utilities.Sql.NullToZeroInteger(row("StepNumber"))
        If stepNumber > stepNumberMax Then stepNumberMax = stepNumber
      Next
      Return stepNumberMax
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return 0
  End Function

  Private Function GetProductCount(batchSteps As DataTable, stepTypeID As Integer) As Integer
    ' Return the total number of steps
    Try
      If batchSteps Is Nothing Then Return 0

      ' Count the number of dye and chemical steps
      Dim dyeCount As Integer = 0
      Dim chemicalCount As Integer = 0
      For Each row As DataRow In batchSteps.Rows
        Dim rowStepTypeID = Utilities.Sql.NullToZeroInteger(row("StepTypeID"))
        If rowStepTypeID = 1 Then dyeCount += 1
        If rowStepTypeID = 2 Then chemicalCount += 1
      Next

      ' Return the count we want
      Select Case stepTypeID
        Case 1 : Return dyeCount
        Case 2 : Return chemicalCount
        Case Else
          Return dyeCount + chemicalCount
      End Select

    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return 0
  End Function

  Private Function GetDispenseCount(batchsteps As DataTable, stepTypeID As Integer) As Integer
    ' Return the total number of products dispensed
    Try
      If batchsteps Is Nothing Then Return 0

      ' Count the number of dye and chemical steps that have values filled in for dispensing
      Dim dyeCount As Integer = 0
      Dim chemicalCount As Integer = 0
      For Each row As DataRow In batchsteps.Rows
        Dim rowStepTypeID = Utilities.Sql.NullToZeroInteger(row("StepTypeID"))
        If Not row.IsNull("DispenseGrams") Then
          If rowStepTypeID = 1 Then dyeCount += 1
          If rowStepTypeID = 2 Then chemicalCount += 1
        End If
      Next

      ' Return the count we want
      Select Case stepTypeID
        Case 1 : Return dyeCount
        Case 2 : Return chemicalCount
        Case Else
          Return dyeCount + chemicalCount
      End Select

    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return 0
  End Function

  Private Sub AddColumn(table As DataTable, columnName As String)
    If Not table.Columns.Contains(columnName) Then table.Columns.Add(columnName, GetType(Integer))

  End Sub

End Class
