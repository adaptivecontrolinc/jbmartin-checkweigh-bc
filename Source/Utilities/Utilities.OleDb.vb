Namespace Utilities

  Public NotInheritable Class OleDB

    Public Shared Function GetDataRow(ByVal connectionString As String, ByVal tableName As String, ByVal id As Integer) As DataRow
      Try
        Dim dt As New System.Data.DataTable
        Dim da As New System.Data.OleDb.OleDbDataAdapter

        da.SelectCommand = New System.Data.OleDb.OleDbCommand
        With da.SelectCommand
          .CommandText = "SELECT * FROM " & tableName & " WHERE ID=" & id.ToString
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : da.Fill(dt) : .Connection.Close()
          If dt.Rows.Count = 1 Then Return dt.Rows(0)
        End With

      Catch Ex As Exception
        Debug.Print("GetDataRow " & tableName & " " & id)
        Debug.Print(Ex.Message)
        Utilities.Log.LogError(Ex)
      End Try
      Return Nothing
    End Function

    Public Shared Function GetDataRow(ByVal connectionString As String, ByVal selectString As String) As DataRow
      Try
        Dim dt As New System.Data.DataTable
        Dim da As New System.Data.OleDb.OleDbDataAdapter

        da.SelectCommand = New System.Data.OleDb.OleDbCommand
        With da.SelectCommand
          .CommandText = selectString
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : da.Fill(dt) : .Connection.Close()
          If dt.Rows.Count = 1 Then Return dt.Rows(0)
        End With

      Catch ex As Exception
        Debug.Print("GetDataRow: " & selectString)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function

    Public Shared Function GetDataTable(ByVal connectionString As String, ByVal selectString As String) As System.Data.DataTable
      Try
        Dim dt As New System.Data.DataTable
        Dim da As New System.Data.OleDb.OleDbDataAdapter

        da.SelectCommand = New System.Data.OleDb.OleDbCommand
        With da.SelectCommand
          .CommandText = selectString
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : da.Fill(dt) : .Connection.Close()
          Return dt
        End With

      Catch ex As Exception
        Debug.Print("GetDataTable: " & selectString)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function

    Public Shared Function GetDataTable(ByVal connectionString As String, ByVal selectString As String, ByVal tableName As String) As System.Data.DataTable
      Try
        Dim dt As New System.Data.DataTable(tableName)
        Dim da As New System.Data.OleDb.OleDbDataAdapter

        da.SelectCommand = New System.Data.OleDb.OleDbCommand
        With da.SelectCommand
          .CommandText = selectString
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : da.Fill(dt) : .Connection.Close()
          Return dt
        End With

      Catch ex As Exception
        Debug.Print("GetDataTable: " & selectString)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function

    Public Shared Function GetDataTableSchema(ByVal connectionString As String, ByVal tableName As String) As System.Data.DataTable
      Try
        'Get the Schema for the target table
        '  this will give us the structure of table and we can use DataTable.NewRow
        '  doing this here and passing a reference to reduce the number of calls to the database
        Dim dt As New System.Data.DataTable(tableName)
        Dim da As New System.Data.OleDb.OleDbDataAdapter

        da.SelectCommand = New System.Data.OleDb.OleDbCommand
        With da.SelectCommand
          .CommandText = "SELECT * FROM " & tableName
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : da.FillSchema(dt, SchemaType.Source) : .Connection.Close()
        End With

        'Set the AutoIncrement properties - this is not imported with the schema so we must manually set it
        dt.Columns("ID").AutoIncrement = True
        dt.Columns("ID").AutoIncrementSeed = 1
        dt.Columns("ID").AutoIncrementStep = 1

        Return dt
      Catch ex As Exception
        Debug.Print("GetDataTableSchema: " & tableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
      Return Nothing
    End Function

    Public Shared Function GetDataTableSchema(ByVal connectionString As String, ByVal tableName As String, ByVal primaryKey As String) As System.Data.DataTable
      Try
        'Get the Schema for the target table
        '  this will give us the structure of table and we can use DataTable.NewRow
        '  doing this here and passing a reference to reduce the number of calls to the database
        Dim dt As New System.Data.DataTable(tableName)
        Dim da As New System.Data.OleDb.OleDbDataAdapter

        da.SelectCommand = New System.Data.OleDb.OleDbCommand
        With da.SelectCommand
          .CommandText = "SELECT * FROM " & tableName
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : da.FillSchema(dt, SchemaType.Source) : .Connection.Close()
        End With

        'Set primary key if it has been passed
        dt.PrimaryKey = New DataColumn() {dt.Columns(primaryKey)}

        'Set the AutoIncrement properties - this is not imported with the schema so we must manually set it
        dt.Columns("ID").AutoIncrement = True
        dt.Columns("ID").AutoIncrementSeed = 1
        dt.Columns("ID").AutoIncrementStep = 1

        Return dt
      Catch ex As Exception
        Debug.Print("GetDataTableSchema: " & tableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
      Return Nothing
    End Function

    Public Shared Function InsertDataRow(ByVal connectionString As String, ByVal dr As System.Data.DataRow) As Boolean
      Try
        'Must have a valid data row
        If dr Is Nothing Then Return False

        Dim dt As System.Data.DataTable = dr.Table
        Dim da As New System.Data.OleDb.OleDbDataAdapter

        da.InsertCommand = New System.Data.OleDb.OleDbCommand
        With da.InsertCommand

          'Make parameters and insert string based on table structure
          MakeParameters(dt, .Parameters)
          MakeInsertString(dt, .CommandText)

          'Set values of all parameters from the data row
          For Each dc As System.Data.DataColumn In dt.Columns
            .Parameters("@" & dc.ColumnName).Value = dr(dc.ColumnName)
          Next
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : .ExecuteNonQuery() : .Connection.Close()

          'Dim autoIncrement As Integer = -1
          '.CommandText = .CommandText '& "; SELECT CAST(scope_identity() AS int);"
          '.Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          '.Connection.Open() : autoIncrement = Integer.Parse(.ExecuteScalar().ToString) : .Connection.Close()
          'dr("ID") = autoIncrement
        End With

        Return True
      Catch ex As Exception
        Debug.Print("InsertDataRow: " & dr.Table.TableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    Public Shared Function InsertDataTable(ByVal connectionString As String, ByVal dt As System.Data.DataTable) As Boolean
      Try
        'Must have a valid data table
        If dt Is Nothing Then Return False

        Dim rows As Integer
        Dim da As New System.Data.OleDb.OleDbDataAdapter
        With da
          .InsertCommand = New System.Data.OleDb.OleDbCommand

          With .InsertCommand
            MakeParameters(dt, .Parameters)
            MakeInsertString(dt, .CommandText)

            .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
            For Each dr As System.Data.DataRow In dt.Rows
              For Each dc As System.Data.DataColumn In dt.Columns
                .Parameters("@" & dc.ColumnName).Value = dr(dc.ColumnName)
              Next
              .Connection.Open() : rows = .ExecuteNonQuery() : .Connection.Close()
            Next
          End With
        End With

        Return True
      Catch ex As Exception
        Debug.Print("InsertDataRow: " & dt.TableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function


    Public Shared Sub MakeInsertString(ByVal dt As System.Data.DataTable, ByRef commandText As String)
      'Make a SQL insert (using sql parameters) to insert a row into the database
      '  assumes ID is an AutoIncrement field and will be filled in by the database
      Try
        If dt Is Nothing Then Exit Sub

        Dim columns As String = Nothing, values As String = Nothing
        For Each dc As System.Data.DataColumn In dt.Columns
          If columns = Nothing Then
            columns = "[" & dc.ColumnName.ToString & "]"
          Else
            columns &= ",[" & dc.ColumnName.ToString & "]"
          End If
          If values = Nothing Then
            values = "@" & dc.ColumnName.ToString
          Else
            values &= ",@" & dc.ColumnName.ToString
          End If
        Next
        commandText = "INSERT INTO " & dt.TableName & " (" & columns & ") VALUES(" & values & ")"
      Catch ex As Exception
        Debug.Print("MakeInsertString: " & dt.TableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    Public Shared Sub MakeInsertStringWithID(ByVal dt As System.Data.DataTable, ByRef commandText As String)
      'Make a SQL insert (using sql parameters) to insert a row into the database
      '  assumes ID is an AutoIncrement field but trys to insert the existing ID value 
      '  this can be used to delete and re-insert records with the original ID values
      Try
        If dt Is Nothing Then Exit Sub

        Dim columns As String = Nothing, values As String = Nothing
        For Each dc As System.Data.DataColumn In dt.Columns
          If columns = Nothing Then
            columns = "[" & dc.ColumnName.ToString & "]"
          Else
            columns &= ",[" & dc.ColumnName.ToString & "]"
          End If
          If values = Nothing Then
            values = "@" & dc.ColumnName.ToString
          Else
            values &= ",@" & dc.ColumnName.ToString
          End If
        Next
        commandText = "INSERT INTO " & dt.TableName & " (" & columns & ") VALUES(" & values & ")"
      Catch ex As Exception
        Debug.Print("MakeInsertString: " & dt.TableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    Public Shared Function UpdateDataRow(ByVal connectionString As String, ByVal dr As System.Data.DataRow) As Boolean
      Try

        If dr Is Nothing Then Return False
        Dim dt As System.Data.DataTable = dr.Table

        Dim da As New System.Data.OleDb.OleDbDataAdapter
        da.UpdateCommand = New System.Data.OleDb.OleDbCommand

        With da.UpdateCommand
          MakeParameters(dt, .Parameters)
          MakeUpdateString(dt, .CommandText)

          For Each dc As System.Data.DataColumn In dt.Columns
            .Parameters("@" & dc.ColumnName).Value = dr(dc.ColumnName)
          Next

          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open()
          Dim rows As Integer = .ExecuteNonQuery()
          .Connection.Close()
        End With

        Return True
      Catch ex As Exception
        Debug.Print("UpdateDataRow: " & dr.Table.TableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    Public Shared Sub MakeUpdateString(ByVal dt As System.Data.DataTable, ByRef commandText As String)
      Try
        If dt Is Nothing Then Exit Sub

        Dim updateString As String = Nothing
        For Each dc As System.Data.DataColumn In dt.Columns
          If updateString = Nothing Then
            updateString = "[" & dt.TableName & "].[" & dc.ColumnName.ToString & "]=@" & dc.ColumnName.ToString
          Else
            updateString &= ",[" & dt.TableName & "].[" & dc.ColumnName.ToString & "]=@" & dc.ColumnName.ToString
          End If
        Next
        commandText = "UPDATE " & dt.TableName & " SET " & updateString & " WHERE " & dt.TableName & ".ID=@ID"
      Catch ex As Exception
        Debug.Print("MakeUpdateString: " & dt.TableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    Public Shared Function DeleteDataRow(ByVal connectionString As String, ByVal dr As System.Data.DataRow) As Boolean
      Try
        If dr.IsNull("ID") Then Return True

        Dim sql As String = "DELETE FROM " & dr.Table.TableName & " WHERE ID=" & dr("ID").ToString
        If SqlDelete(connectionString, sql) >= 0 Then Return True

      Catch ex As Exception
        Debug.Print("DeleteDataRow: " & dr.Table.TableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    Public Shared Sub MakeParameters(ByVal dt As System.Data.DataTable, ByRef pc As System.Data.OleDb.OleDbParameterCollection)
      Try
        If dt Is Nothing Then Exit Sub

        pc.Clear()
        For Each dc As System.Data.DataColumn In dt.Columns
          'Create a parameter for every column in the table
          Dim p As New System.Data.OleDb.OleDbParameter
          p.ParameterName = "@" & dc.ColumnName
          If dc.DataType.Name = "Int32" Then p.OleDbType = Data.OleDb.OleDbType.Integer
          If dc.DataType.Name = "String" Then p.OleDbType = Data.OleDb.OleDbType.VarChar
          If dc.DataType.Name = "DateTime" Then p.OleDbType = Data.OleDb.OleDbType.Date
          If dc.DataType.Name = "Double" Then p.OleDbType = Data.OleDb.OleDbType.Double
          If dc.DataType.Name = "Decimal" Then p.OleDbType = Data.OleDb.OleDbType.Decimal
          'Special case - can't figue out how to distinguish string from text...
          'TODO - check, it might not matter NVarChar can be 4000 characters... 
          '     - could create both types and check the length of the string during the update ?
          'If dc.ColumnName.ToLower.Contains("comment") Then p.OleDbType = Data.OleDb.OleDbType.LongVarChar
          'If dc.ColumnName.ToLower.Contains("notes") Then p.OleDbType = Data.OleDb.OleDbType.LongVarChar
          pc.Add(p)
        Next
      Catch ex As Exception
        Debug.Print("MakeParameters: " & dt.TableName)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    Public Shared Function SqlDelete(ByVal connectionString As String, ByVal deleteString As String) As Integer
      Try
        Dim rows As Integer
        Dim da As New System.Data.OleDb.OleDbDataAdapter
        da.DeleteCommand = New System.Data.OleDb.OleDbCommand
        With da.DeleteCommand
          .CommandText = deleteString
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : rows = .ExecuteNonQuery() : .Connection.Close()
        End With

        Return rows
      Catch ex As Exception
        Debug.Print("SQLDelete: " & deleteString)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex, deleteString)
      End Try
      Return -1
    End Function

    Public Shared Function SqlUpdate(ByVal connectionString As String, ByVal updateString As String) As Integer
      Try
        Dim rows As Integer
        Dim da As New System.Data.OleDb.OleDbDataAdapter
        da.UpdateCommand = New System.Data.OleDb.OleDbCommand
        With da.UpdateCommand
          .CommandText = updateString
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : rows = .ExecuteNonQuery() : .Connection.Close()
        End With

        Return rows
      Catch ex As Exception
        Debug.Print("SQLUpdate: " & updateString)
        Debug.Print(ex.Message)
        Utilities.Log.LogError(ex, updateString)
      End Try
      Return -1
    End Function

    Public Shared Function SqlInsert(ByVal connectionString As String, ByVal insertString As String) As Integer
      Try
        Dim rows As Integer
        Dim da As New System.Data.OleDb.OleDbDataAdapter
        da.InsertCommand = New System.Data.OleDb.OleDbCommand
        With da.InsertCommand
          .CommandText = insertString
          .Connection = New System.Data.OleDb.OleDbConnection(connectionString)
          .Connection.Open() : rows = .ExecuteNonQuery() : .Connection.Close()
        End With

        Return rows
      Catch ex As Exception
        Utilities.Log.LogError(ex, insertString)
      End Try
      Return -1
    End Function

  End Class

End Namespace
