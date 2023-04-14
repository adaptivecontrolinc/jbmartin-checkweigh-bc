Imports Utilities.Sql

Public Class StepListData
  Private controlCode As ControlCode

  Property SqlConnection As String
  Property SqlSelect As String

  Property BatchID As Integer
  Property BatchRow As DataRow
  Property BatchSteps As DataTable

  Public Sub New(controlcode As ControlCode)
    Me.controlCode = controlcode
  End Sub

  Sub Connect(batchRow As DataRow)
    Me.BatchRow = batchRow
    Me.BatchID = Utilities.Sql.NullToZeroInteger(batchRow("ID"))
    Requery()
  End Sub

  Sub Requery()
    Try
      SqlConnection = controlCode.Settings.SqlConnection
      SqlSelect = GetSqlSelect()

      Dim table = Utilities.Sql.GetDataTable(SqlConnection, SqlSelect)
      BatchSteps = table
    Catch ex As Exception
      Utilities.Log.LogError(ex, SqlSelect)
    End Try
  End Sub

  Private Function GetSqlSelect() As String
    Dim sql As String = Nothing
    Try
      sql = controlCode.Settings.SqlSelectBatchSteps
      Dim sqlWhere As String = "WHERE (DyelotID=" & BatchID.ToString & ") AND ((StepTypeID=1) OR (StepTypeID=2))"

      If controlCode.Settings.DyesOnly Then sqlWhere = "WHERE (DyelotID=" & BatchID.ToString & ") AND (StepTypeID=1)"
      If controlCode.Settings.ChemsOnly Then sqlWhere = "WHERE (DyelotID=" & BatchID.ToString & ") AND (StepTypeID=2)"

      If controlCode.Settings.CheckWeighEnableOnly Then sqlWhere &= " AND (CheckWeighEnable=1)"
      If controlCode.Settings.ExcludeWeighed Then sqlWhere &= " AND (DispenseGrams Is Null)"

      Return sql.Replace("%WHERE", sqlWhere)
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return sql
  End Function

End Class
