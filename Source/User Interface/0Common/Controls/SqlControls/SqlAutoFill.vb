Module SqlAutoFill

  Public Function FillControls(controls As System.Windows.Forms.Control.ControlCollection, row As System.Data.DataRow) As Boolean
    Try
      For Each control In controls
        Dim sqlControl = TryCast(control, ISqlControl)
        If sqlControl IsNot Nothing AndAlso sqlControl.SqlAutoFill Then
          If Not String.IsNullOrEmpty(sqlControl.SqlColumnName) Then
            If row.Table.Columns.Contains(sqlControl.SqlColumnName) Then
              sqlControl.SqlValue = row(sqlControl.SqlColumnName)
            End If
          End If
        End If
      Next
      Return True
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return False
  End Function

  Public Function FillDataRow(controls As System.Windows.Forms.Control.ControlCollection, row As System.Data.DataRow) As Boolean
    Try
      For Each control In controls
        Dim sqlControl = TryCast(control, ISqlControl)
        If sqlControl IsNot Nothing AndAlso sqlControl.SqlAutoFill Then
          If Not String.IsNullOrEmpty(sqlControl.SqlColumnName) Then
            If row.Table.Columns.Contains(sqlControl.SqlColumnName) Then
              row(sqlControl.SqlColumnName) = sqlControl.SqlValue
            End If
          End If
        End If
      Next
      Return True
    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
    Return False
  End Function


End Module
