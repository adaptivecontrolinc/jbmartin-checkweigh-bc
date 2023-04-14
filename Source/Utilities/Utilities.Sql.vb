Imports System.Data
Imports System.Globalization.CultureInfo

Namespace Utilities

  Partial Public NotInheritable Class Sql

#Region " Select "

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataRow(ByVal connectionString As String, ByVal selectString As String) As DataRow
      ' Return a data row from the connection string and select string parameters
      '   return nothing if no row found, more than one row found or an exception is thrown
      Try
        Dim table As New DataTable
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = selectString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : adapter.Fill(table) : .Connection.Close()
        End With

        If table.Rows.Count = 1 Then Return table.Rows(0)
      Catch ex As Exception
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataRow(ByVal connectionString As String, ByVal selectString As String, ByVal tableName As String) As DataRow
      ' Return a data row from the connection string and select string parameters and explicitly set the table name
      '   return nothing if no row found, more than one row found or an exception is thrown
      Try
        Dim table As New DataTable(tableName) ' With {.Locale = InvariantCulture} 'TODO Use?
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = selectString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : adapter.Fill(table) : .Connection.Close()
        End With

        If table.Rows.Count = 1 Then Return table.Rows(0)
      Catch ex As Exception
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function


    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataRowWithSchema(ByVal connectionString As String, ByVal selectString As String) As DataRow
      ' Return a data row from the connection string and select string parameters, apply source schema
      '   return nothing if no row found, more than one row found or an exception is thrown
      Try
        Dim table As New DataTable '  With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = selectString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open()
          adapter.FillSchema(table, SchemaType.Source)
          adapter.Fill(table)
          .Connection.Close()
        End With

        If table.Rows.Count = 1 Then Return table.Rows(0)
      Catch ex As Exception
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataRowWithSchema(ByVal connectionString As String, ByVal selectString As String, ByVal tableName As String) As DataRow
      ' Return a data row from connection string and select string parameters and explicitly set the table name, apply source schema
      '   return nothing if no row found, more than one row found or an exception is thrown
      Try
        Dim table As New DataTable(tableName) ' With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = selectString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open()
          adapter.FillSchema(table, SchemaType.Source)
          adapter.Fill(table)
          .Connection.Close()
        End With

        If table.Rows.Count = 1 Then Return table.Rows(0)
      Catch ex As Exception
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function


    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataTable(ByVal connectionString As String, ByVal selectString As String) As DataTable
      Try
        ' Return a populated data table from the connection string and select string parameters
        '   return nothing if no table found or an exception is thrown
        Dim table As New DataTable '  With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = selectString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : adapter.Fill(table) : .Connection.Close()
        End With

        Return table
      Catch ex As Exception
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataTable(ByVal connectionString As String, ByVal selectString As String, ByVal tableName As String) As DataTable
      ' Return a data table from the connection string and select string parameters and explicitly set the tablename
      '   return nothing if no table found or an exception is thrown
      Try
        Dim table As New DataTable(tableName) ' With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = selectString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : adapter.Fill(table) : .Connection.Close()
        End With

        Return table
      Catch ex As Exception
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataTableWithSchema(ByVal connectionString As String, ByVal selectString As String) As DataTable
      ' Return a data table from the connection string and select string parameters, apply the source schema
      '   return nothing if no table found or an exception is thrown
      Try
        Dim table As New DataTable ' With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = selectString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open()
          adapter.FillSchema(table, SchemaType.Source)
          adapter.Fill(table)
          .Connection.Close()
        End With

        Return table
      Catch ex As Exception
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataTableWithSchema(ByVal connectionString As String, ByVal selectString As String, ByVal tableName As String) As DataTable
      ' Return a data table from the connection string and select string parameters and explicitly set the tablename, apply the source schema
      '   return nothing if no table found or an exception is thrown
      Try
        Dim table As New DataTable(tableName) ' With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = selectString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open()
          adapter.FillSchema(table, SchemaType.Source)
          adapter.Fill(table)
          .Connection.Close()
        End With

        Return table
      Catch ex As Exception
        Utilities.Log.LogError(ex, selectString)
      End Try
      Return Nothing
    End Function


    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataTableEmpty(ByVal connectionString As String, ByVal tableName As String) As DataTable
      ' Return an empty data table from the connection string tablename, do NOT apply the source schema (column names / types only)
      '   return nothing if no table found or an exception is thrown
      Try
        Dim table As New DataTable(tableName) ' With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = "SELECT TOP 0 * FROM " & tableName
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : adapter.Fill(table) : .Connection.Close()
        End With

        Return table
      Catch ex As Exception
        Utilities.Log.LogError(ex, tableName)
      End Try
      Return Nothing
    End Function


    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataTableSchema(ByVal connectionString As String, ByVal tableName As String) As DataTable
      ' Return the source schema for the target table
      Try
        Dim table As New DataTable(tableName) ' With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = "SELECT * FROM " & tableName
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : adapter.FillSchema(table, SchemaType.Source) : .Connection.Close()
        End With

        Return table
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function GetDataTableSchema(ByVal connectionString As String, ByVal tableName As String, ByVal primaryKey As String) As DataTable
      Try
        ' Return the source schema for the target table and explicitly set the primary key
        Dim table As New DataTable(tableName) ' With {.Locale = InvariantCulture}
        Dim adapter As New SqlClient.SqlDataAdapter

        adapter.SelectCommand = New SqlClient.SqlCommand
        With adapter.SelectCommand
          .CommandText = "SELECT * FROM " & tableName
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : adapter.FillSchema(table, SchemaType.Source) : .Connection.Close()
        End With

        'Set primary key if it has been passed
        table.PrimaryKey = New DataColumn() {table.Columns(primaryKey)}

        ' Set autoincrement if primary key is "ID" -  a little bit naughty
        '  TODO?: If primaryKey = "ID" Then table.Columns("ID").AutoIncrement = True

        Return table
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return Nothing
    End Function

    Public Shared Sub ResetDBAllowNull(row As DataRow)
      If row Is Nothing Then Exit Sub
      ResetDBAllowNull(row.Table)
    End Sub

    Public Shared Sub ResetDBAllowNull(table As DataTable)
      If table Is Nothing Then Exit Sub
      For Each column As DataColumn In table.Columns
        Try
          column.AllowDBNull = True
        Catch
        End Try
      Next
    End Sub

    Public Shared Sub ResetDBNullReadOnly(row As DataRow)
      If row Is Nothing Then Exit Sub
      ResetDBNullReadOnly(row.Table)
    End Sub

    Public Shared Sub ResetDBNullReadOnly(table As DataTable)
      If table Is Nothing Then Exit Sub
      For Each column As DataColumn In table.Columns
        Try
          column.AllowDBNull = True
          column.ReadOnly = False
        Catch
        End Try
      Next
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SqlSelect(ByVal connectionString As String, ByVal selectString As String) As DataTable
      Return GetDataTable(connectionString, selectString)
    End Function

#End Region

#Region " Insert "

    '<System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function InsertDataRow(ByVal connectionString As String, ByVal row As DataRow) As Boolean
      Try
        'Must have a valid data row
        If row Is Nothing Then Return False
        Dim table As DataTable = row.Table

        Dim adapter As New SqlClient.SqlDataAdapter
        adapter.InsertCommand = New SqlClient.SqlCommand

        With adapter.InsertCommand
          'Add parameters and build insert string based on table columns
          AddSqlParameters(table, .Parameters)
          BuildSqlInsertString(table, .CommandText)

          'Set values of all parameters from the data row
          Dim autoIncrementColumn As DataColumn = Nothing
          For Each column As DataColumn In table.Columns
            .Parameters("@" & column.ColumnName).Value = row(column.ColumnName)
            If column.AutoIncrement Then autoIncrementColumn = column
          Next

          ' FOR DEBUG - just so we can easily check the string
          Dim sql = .CommandText

          If autoIncrementColumn Is Nothing Then
            .Connection = New SqlClient.SqlConnection(connectionString)
            .Connection.Open() : .ExecuteNonQuery() : .Connection.Close()
          Else
            Dim autoIncrementValue As Integer = -1
            .CommandText = .CommandText & "; SELECT CAST(scope_identity() AS int);"
            .Connection = New SqlClient.SqlConnection(connectionString)
            .Connection.Open() : autoIncrementValue = Integer.Parse(.ExecuteScalar().ToString) : .Connection.Close()

            ' Write autoincrement value into the column if it has not been written
            Dim autoIncrementRowValue = NullToZeroInteger(row(autoIncrementColumn))
            If autoIncrementRowValue <> autoIncrementValue Then
              ' Toggle readonly if set
              If autoIncrementColumn.ReadOnly Then
                autoIncrementColumn.ReadOnly = False
                row(autoIncrementColumn) = autoIncrementValue
                autoIncrementColumn.ReadOnly = True
              Else
                row(autoIncrementColumn) = autoIncrementValue
              End If
            End If
          End If
        End With

        Return True
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function InsertDataTable(ByVal connectionString As String, ByVal dt As DataTable) As Boolean
      Try
        'Must have a valid data table
        If dt Is Nothing Then Return False

        Dim rows As Integer
        Dim da As New SqlClient.SqlDataAdapter
        With da
          .InsertCommand = New SqlClient.SqlCommand

          With .InsertCommand
            AddSqlParameters(dt, .Parameters)
            BuildSqlInsertString(dt, .CommandText)

            .Connection = New SqlClient.SqlConnection(connectionString)
            For Each dr As DataRow In dt.Rows
              For Each dc As DataColumn In dt.Columns
                .Parameters("@" & dc.ColumnName).Value = dr(dc.ColumnName)
              Next
              .Connection.Open() : rows = .ExecuteNonQuery() : .Connection.Close()
            Next
          End With
        End With

        Return True
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function InsertDataTable(ByVal connectionString As String, ByVal table As DataTable, ByVal sqlDelete As String) As Boolean
      'Insert a datatable into the database - this is used to update detail tables using a transactiion
      '  we start the transaction, delete the original records then insert the new records
      '  the transaction will be rolled back if any errors occur

      Dim sql As String = Nothing
      Try
        'Create the connection and declare the command and transaction objects
        Dim connection As New SqlClient.SqlConnection(connectionString)
        Dim deleteCommand As SqlClient.SqlCommand
        Dim insertCommand As SqlClient.SqlCommand
        Dim transaction As SqlClient.SqlTransaction

        'Open the connection start the transaction and make the command and transaction objects on this connection
        connection.Open()
        transaction = connection.BeginTransaction
        deleteCommand = connection.CreateCommand : deleteCommand.Transaction = transaction
        insertCommand = connection.CreateCommand : insertCommand.Transaction = transaction

        Try
          'Delete any existing records
          With deleteCommand
            sql = sqlDelete ' just so we get the sql statement if we throw an error
            .CommandText = sql
            .ExecuteNonQuery()
          End With

          'Insert new or modified records
          With insertCommand
            'Create the parameters we will use for the insert command
            AddSqlParameters(table, .Parameters)

            'Insert all the datarows
            For Each row As DataRow In table.Rows
              If row.RowState <> DataRowState.Deleted Then
                'Set values of all parameters from the data row
                For Each column As DataColumn In table.Columns
                  insertCommand.Parameters("@" & column.ColumnName).Value = row(column.ColumnName)
                Next
                'Make the insert string
                BuildSqlInsertString(table, sql)
                .CommandText = sql & "; SELECT CAST(scope_identity() AS int);"
                row("ID") = Integer.Parse(.ExecuteScalar().ToString)
              End If
            Next
          End With

          'Commit the changes 
          transaction.Commit()
          Return True
        Catch ex As Exception
          Utilities.Log.LogError(ex, sql)
        End Try
        transaction.Rollback()

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function InsertDataTable(ByVal connectionString As String, ByVal table As DataTable, ByVal sqlDelete As String, MaintainID As Boolean) As Boolean
      'Insert a datatable into the database - this is used to update detail tables using a transactiion
      '  we start the transaction, delete the original records then insert the new records
      '  the transaction will be rolled back if any errors occur
      '  if records have a valid IDENTITY set try to keep it the same

      Dim sql As String = Nothing
      Try
        'Create the connection and declare the command and transaction objects
        Dim connection As New SqlClient.SqlConnection(connectionString)
        Dim deleteCommand As SqlClient.SqlCommand
        Dim insertCommand As SqlClient.SqlCommand
        Dim transaction As SqlClient.SqlTransaction

        'Open the connection start the transaction and make the command and transaction objects on this connection
        connection.Open()
        transaction = connection.BeginTransaction
        deleteCommand = connection.CreateCommand : deleteCommand.Transaction = transaction
        insertCommand = connection.CreateCommand : insertCommand.Transaction = transaction

        Try
          'Delete any existing records
          With deleteCommand
            sql = sqlDelete ' just so we get the sql statement if we throw an error
            .CommandText = sql
            .ExecuteNonQuery()
          End With

          'Insert new or modified records
          With insertCommand
            'Create the parameters we will use for the insert command
            AddSqlParameters(table, .Parameters)

            'Insert all the datarows preserving ID where it is set
            For Each row As DataRow In table.Rows
              If row.RowState <> DataRowState.Deleted Then
                'Set values of all parameters from the data row
                For Each dc As DataColumn In table.Columns
                  insertCommand.Parameters("@" & dc.ColumnName).Value = row(dc.ColumnName)
                Next
                'Make the insert string this will be different if ID is null (new record)
                If row.IsNull("ID") Then
                  BuildSqlInsertString(table, sql)
                  sql &= "; SELECT CAST(scope_identity() AS int);"
                  .CommandText = sql
                  row("ID") = Integer.Parse(.ExecuteScalar().ToString)
                Else
                  MakeSqlInsertStringWithID(table, sql)
                  sql = "SET IDENTITY_INSERT " & table.TableName & " ON; " & sql & "; SET IDENTITY_INSERT " & table.TableName & " OFF;"
                  .CommandText = sql
                  .ExecuteNonQuery()
                End If
              End If
            Next
          End With

          'Commit the changes 
          transaction.Commit()
          Return True
        Catch ex As Exception
          Utilities.Log.LogError(ex, sql)
        End Try
        transaction.Rollback()

      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function


    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub BuildSqlInsertString(ByVal table As DataTable, ByRef commandText As String)
      'Make a Sql insert string (using sql parameters) to insert a row into the database
      '  Don't try to insert a value for an AutoIncrement column
      Try
        If table Is Nothing Then Exit Sub

        Dim columns As String = "", values As String = ""
        For Each column As DataColumn In table.Columns

          If Not column.AutoIncrement Then
            If columns.Length = 0 Then
              columns = "[" & column.ColumnName.ToString & "]"
            Else
              columns &= ",[" & column.ColumnName.ToString & "]"
            End If
            If values.Length = 0 Then
              values = "@" & column.ColumnName.ToString
            Else
              values &= ",@" & column.ColumnName.ToString
            End If
          End If
        Next
        commandText = "INSERT INTO " & table.TableName & " (" & columns & ") VALUES(" & values & ")"
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub MakeSqlInsertStringWithID(ByVal dt As DataTable, ByRef commandText As String)
      'Make a Sql insert (using sql parameters) to insert a row into the database
      '  assumes ID is an AutoIncrement field but trys to insert the existing ID value 
      '  this can be used to delete and re-insert records with the original ID values
      Try
        If dt Is Nothing Then Exit Sub

        Dim columns As String = "", values As String = ""
        For Each dc As DataColumn In dt.Columns
          If columns.Length = 0 Then
            columns = "[" & dc.ColumnName.ToString & "]"
          Else
            columns &= ",[" & dc.ColumnName.ToString & "]"
          End If
          If values.Length = 0 Then
            values = "@" & dc.ColumnName.ToString
          Else
            values &= ",@" & dc.ColumnName.ToString
          End If
        Next
        commandText = "INSERT INTO " & dt.TableName & " (" & columns & ") VALUES(" & values & ")"
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SqlInsert(ByVal connectionString As String, ByVal insertString As String) As Integer
      Try
        Dim rows As Integer
        Dim da As New SqlClient.SqlDataAdapter
        da.InsertCommand = New SqlClient.SqlCommand
        With da.InsertCommand
          .CommandText = insertString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : rows = .ExecuteNonQuery() : .Connection.Close()
        End With

        Return rows
      Catch ex As Exception
        Utilities.Log.LogError(ex, insertString)
      End Try
      Return -1
    End Function
#End Region

#Region " Update "
    ' Remember to include table name in data row...

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function UpdateDataRow(ByVal connectionString As String, ByVal row As DataRow) As Boolean
      Return UpdateDataRow(connectionString, row, "ID")
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function UpdateDataRow(ByVal connectionString As String, ByVal row As DataRow, primaryKey As String) As Boolean
      'Write changes in a data row back to the database
      '  return false if write fails or an exception is thrown
      Try
        'Make sure the data row is not empty
        If row Is Nothing Then Return False
        Dim table = row.Table

        Dim adapter As New SqlClient.SqlDataAdapter
        adapter.UpdateCommand = New SqlClient.SqlCommand

        With adapter.UpdateCommand
          ' Make the sql update statement
          .CommandText = GetSqlUpdateCommandText(table, primaryKey)

          'Add parameters and build insert string based on table schema
          AddSqlParameters(table, .Parameters)

          'Set values of all parameters from the data row
          For Each dc As DataColumn In table.Columns
            .Parameters("@" & dc.ColumnName).Value = row(dc.ColumnName)
          Next

          Dim test = .CommandText

          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : .ExecuteNonQuery() : .Connection.Close()
        End With

        Return True
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function


    Public Shared Function GetSqlUpdateCommandText(table As DataTable, primaryKey As String) As String
      Dim commandText As String = Nothing
      Try
        'Make sure we have a DataTable to work with
        If table Is Nothing Then Return Nothing

        'Make the sql column update string
        Dim sqlColumns As String = Nothing
        For Each column As DataColumn In table.Columns
          If Not column.ColumnName.Equals(primaryKey, StringComparison.InvariantCultureIgnoreCase) Then
            If sqlColumns Is Nothing Then
              sqlColumns = "[" & column.ColumnName.ToString & "]=@" & column.ColumnName.ToString
            Else
              sqlColumns &= ",[" & column.ColumnName.ToString & "]=@" & column.ColumnName.ToString
            End If
          End If
        Next
        commandText = "UPDATE [" & table.TableName & "] SET " & sqlColumns & " WHERE [" & primaryKey & "]=@" & primaryKey

        Return commandText
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SqlUpdate(ByVal connectionString As String, ByVal updateString As String) As Integer
      Try
        Dim rows As Integer
        Dim da As New SqlClient.SqlDataAdapter
        da.UpdateCommand = New SqlClient.SqlCommand
        With da.UpdateCommand
          .CommandText = updateString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : rows = .ExecuteNonQuery() : .Connection.Close()
        End With

        Return rows
      Catch ex As Exception
        Utilities.Log.LogError(ex, updateString)
      End Try
      Return -1
    End Function

#End Region

#Region " Delete "

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SqlDelete(ByVal connectionString As String, ByVal deleteString As String) As Integer
      Try
        Dim rows As Integer
        Dim da As New SqlClient.SqlDataAdapter
        da.DeleteCommand = New SqlClient.SqlCommand
        With da.DeleteCommand
          .CommandText = deleteString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : rows = .ExecuteNonQuery() : .Connection.Close()
        End With

        Return rows
      Catch ex As Exception
        Utilities.Log.LogError(ex, deleteString)
      End Try
      Return -1
    End Function

#End Region

#Region " Save Row / Table Changes "

    ' Save the table row changes back to the data base using the row state to determine whether to insert, update or delete
    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SaveDataTable(ByVal connectionString As String, ByVal table As System.Data.DataTable) As Boolean
      Return SaveDataTable(connectionString, table, "ID")
    End Function

    ' Save the table row changes back to the data base using the row state to determine whether to insert, update or delete
    '<System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SaveDataTable(ByVal connectionString As String, ByVal table As System.Data.DataTable, primaryKey As String) As Boolean
      If table Is Nothing Then Return False

      Dim tableName As String = table.TableName
      Try
        For Each row As System.Data.DataRow In table.Rows
          SaveDataRow(connectionString, row, primaryKey, tableName)
        Next
        table.AcceptChanges()
        Return True
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    ' Save the row changes back to the data base using the row state to determine whether to insert, update or delete
    Public Shared Function SaveDataRow(connectionString As String, row As System.Data.DataRow) As Boolean
      If row Is Nothing Then Return False
      Return SaveDataRow(connectionString, row, "ID", row.Table.TableName)
    End Function

    Public Shared Function SaveDataRow(connectionString As String, row As System.Data.DataRow, primaryKey As String) As Boolean
      If row Is Nothing Then Return False
      Return SaveDataRow(connectionString, row, primaryKey, row.Table.TableName)
    End Function

    Public Shared Function SaveDataRow(connectionString As String, row As System.Data.DataRow, primaryKey As String, tableName As String) As Boolean
      If row Is Nothing Then Return False
      Try
        Select Case row.RowState
          Case DataRowState.Deleted
            Dim sql = "DELETE FROM " & tableName & " WHERE [" & primaryKey & "]=" & row(primaryKey, DataRowVersion.Original).ToString
            SqlDelete(connectionString, sql)

          Case DataRowState.Added
            InsertDataRow(connectionString, row)
            row.AcceptChanges()

          Case DataRowState.Modified
            UpdateDataRow(connectionString, row, primaryKey)
            row.AcceptChanges()
        End Select

        Return True
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    Public Shared Function TableHasChanges(table As DataTable) As Boolean
      If table Is Nothing Then Return False
      For Each row As DataRow In table.Rows
        If row.RowState <> DataRowState.Unchanged Then Return True
      Next
      Return False
    End Function

#End Region

#Region " Sync Table "

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SyncTableData(ByVal connectionString As String, ByVal dtNewData As System.Data.DataTable, ByVal sqlSelectOldData As String) As Boolean
      Try
        'Get the old data from the database
        Dim dtOldData As System.Data.DataTable = GetDataTable(connectionString, sqlSelectOldData, dtNewData.TableName)

        'Sync the new data
        Return SyncTableData(connectionString, dtNewData, dtOldData)
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SyncTableData(ByVal connectionString As String, ByVal dtNewData As System.Data.DataTable, ByVal dtOldData As System.Data.DataTable) As Boolean
      Try
        'Loop through the new data - insert any new records, update any existing records
        For Each dr As System.Data.DataRow In dtNewData.Rows
          If dr.RowState <> DataRowState.Deleted Then
            If dr.IsNull("ID") Then
              InsertDataRow(connectionString, dr)
            Else
              UpdateDataRow(connectionString, dr)
            End If
          End If
        Next

        'Finally loop through dtOldData and delete any rows that do not exist in dtNewData from the database
        Dim idOld As Integer, idNew As Integer
        For Each drOld As System.Data.DataRow In dtOldData.Rows
          If Integer.TryParse(drOld("ID").ToString, idOld) Then
            Dim deleteRow As Boolean = True
            For Each drNew As System.Data.DataRow In dtNewData.Rows
              If drNew.RowState <> DataRowState.Deleted Then
                If Integer.TryParse(drNew("ID").ToString, idNew) Then
                  'Row found - don't delete
                  If idOld = idNew Then
                    deleteRow = False
                    Exit For
                  End If
                End If
              End If
            Next
            If deleteRow Then
              Dim sql As String = "DELETE FROM " & dtOldData.TableName & " WHERE ID=" & drOld("ID").ToString
              SqlDelete(connectionString, sql)
            End If
          End If
        Next
        Return True
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function


#End Region

#Region " Copy "

    ' Clear existing table and copy new data in
    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function CopyTableData(sourceTable As System.Data.DataTable, targetTable As System.Data.DataTable) As Boolean
      Try
        ' Clear existing data
        targetTable.Clear()

        ' Copy all rows from source table to target table
        For Each row As System.Data.DataRow In sourceTable.Rows
          Try
            Dim newRow As System.Data.DataRow = targetTable.NewRow
            If CopyRowData(row, newRow) Then
              targetTable.Rows.Add(newRow)
            End If
          Catch ex As Exception
            'Don't let one error stop the copy
          End Try
        Next
        Return True
      Catch ex As Exception
        'Ignore errors
      End Try
      Return False
    End Function

    ' Overwrite matching data from one table to another
    '  Add or clear rows if tables have a different number of rows
    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function OverwriteTableData(sourceTable As System.Data.DataTable, targetTable As System.Data.DataTable) As Boolean
      Try
        If sourceTable.Rows.Count >= targetTable.Rows.Count Then
          For rowIndex As Integer = 0 To sourceTable.Rows.Count - 1
            If rowIndex < targetTable.Rows.Count Then
              CopyRowData(sourceTable.Rows(rowIndex), targetTable.Rows(rowIndex))
            Else
              Dim newRow = targetTable.NewRow
              CopyRowData(sourceTable.Rows(rowIndex), newRow)
              targetTable.Rows.Add(newRow)
            End If
          Next
        Else
          For rowIndex As Integer = 0 To targetTable.Rows.Count - 1
            If rowIndex < sourceTable.Rows.Count Then
              CopyRowData(sourceTable.Rows(rowIndex), targetTable.Rows(rowIndex))
            Else
              For colIndex As Integer = 0 To targetTable.Columns.Count - 1
                targetTable.Rows(rowIndex)(colIndex) = DBNull.Value
              Next
            End If
          Next
        End If

        Return True
      Catch ex As Exception
        'Ignore errors
      End Try
      Return False
    End Function

    'Copy matching data from source to target (name and data type must match)
    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function CopyRowData(sourceRow As System.Data.DataRow, targetRow As System.Data.DataRow) As Boolean
      Try
        ' Clear existing data - these rows may not be from the same table
        For Each column As System.Data.DataColumn In targetRow.Table.Columns
          If column.AllowDBNull Then targetRow(column.ColumnName) = DBNull.Value
        Next

        ' Copy matching data from source row to target row
        For Each column As System.Data.DataColumn In sourceRow.Table.Columns
          Try
            Dim columnName As String = column.ColumnName
            If targetRow.Table.Columns.Contains(columnName) Then
              If column.DataType Is targetRow.Table.Columns(columnName).DataType Then
                targetRow(columnName) = sourceRow(columnName)
              End If
            End If
          Catch ex As Exception
            'Don't let one error stop the copy
          End Try
        Next
        Return True
      Catch ex As Exception
        'Ignore errors
      End Try
      Return False
    End Function

    'Copy matching data from source to target (name and data type must match)
    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function CopyRowData(sourceRow As System.Data.DataRow, targetRow As System.Data.DataRow, skipColumn As String) As Boolean
      Try
        ' Clear existing data - these rows may not be from the same table
        For Each targetColumn As System.Data.DataColumn In targetRow.Table.Columns
          If targetColumn.ColumnName.ToLower <> skipColumn.ToLower Then
            If targetColumn.AllowDBNull Then targetRow(targetColumn.ColumnName) = DBNull.Value
          End If
        Next

        ' Copy matching data from source row to target row
        For Each sourceColumn As System.Data.DataColumn In sourceRow.Table.Columns
          Try
            Dim columnName As String = sourceColumn.ColumnName
            If columnName.ToLower <> skipColumn.ToLower Then
              If targetRow.Table.Columns.Contains(columnName) Then
                Dim targetColumn = targetRow.Table.Columns(columnName)
                If sourceColumn.DataType Is targetColumn.DataType Then
                  targetRow(columnName) = sourceRow(columnName)
                End If
              End If
            End If
          Catch ex As Exception
            'Don't let one error stop the copy
          End Try
        Next
        Return True
      Catch ex As Exception
        'Ignore errors
      End Try
      Return False
    End Function
#End Region

#Region " Utilities "

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SqlExecute(ByVal connectionString As String, ByVal sqlString As String) As Boolean
      Try
        Dim da As New SqlClient.SqlDataAdapter
        da.UpdateCommand = New SqlClient.SqlCommand
        With da.UpdateCommand
          .CommandText = sqlString
          .Connection = New SqlClient.SqlConnection(connectionString)
          .Connection.Open() : .ExecuteNonQuery() : .Connection.Close()
        End With

        Return True
      Catch ex As Exception
        Utilities.Log.LogError(ex, sqlString)
      End Try
      Return False
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub AddSqlParameters(ByVal table As DataTable, ByVal sqlParameters As SqlClient.SqlParameterCollection)
      Try
        If table Is Nothing OrElse sqlParameters Is Nothing Then Exit Sub

        With sqlParameters
          .Clear()
          For Each column As DataColumn In table.Columns
            'Create a parameter for every column in the table
            Dim parameter As New SqlClient.SqlParameter
            parameter.ParameterName = "@" & column.ColumnName
            parameter.SqlDbType = GetSqlDataType(column)
            .Add(parameter)
          Next
        End With
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SqlString(ByVal value As String) As String
      Try
        If value Is Nothing OrElse (value.Length = 0) Then Return "Null"

        'Wrap single quotes round string or return "Null" if string is empty
        Dim returnString As String = Nothing

        'Look for single quotes (') and double up
        For i As Integer = 0 To value.Length - 1
          Select Case value.Substring(i, 1)
            Case "'" : returnString &= value.Substring(i, 1) & "'"
            Case Else : returnString &= value.Substring(i, 1)
          End Select
        Next i
        Return "'" & returnString & "'"
      Catch ex As Exception
        Return "Null"
      End Try
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SqlDateString(ByVal value As String) As String
      Try
        If value = Nothing Then
          Return "Null"
        Else
          Dim tryDate As Date
          If Date.TryParse(value, tryDate) Then
            Return SqlDateString(tryDate)
          Else
            Return "Null"
          End If
        End If
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return "Null"
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function SqlDateString(ByVal value As Date) As String
      Try
        If value = Nothing Then
          Return "Null"
        Else
          Return SqlString(value.ToString(InvariantCulture))
        End If
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return "Null"
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function EmptyStringToNull(ByVal dr As DataRow, ByVal column As String) As String
      Try
        If dr.IsNull(column) Then Return "Null"
        If dr(column).ToString.Length <= 0 Then Return "Null"
        Return SqlString(dr(column).ToString)
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return "Null"
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub EmptyStringToNull(ByRef dataCell As Object, ByVal text As String)
      'Set dataCell to text if there's actually something in text, else set it to Null
      '  Useful when updating data from a textbox and we want an empty text box to be saved as Null
      If text.Length > 0 Then
        dataCell = text
      Else
        dataCell = System.DBNull.Value
      End If
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function EmptyDateToNull([date] As Date) As Object
      If [date] = Nothing Then Return DBNull.Value
      Return [date]
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToNothing(ByVal value As String) As String
      Try
        'Check to see if it's null
        If String.IsNullOrEmpty(value) Then Return Nothing

        'String isn't null so just return the string
        Return value
      Catch ex As Exception
        'No code
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToFalseBoolean(ByVal value As String) As Boolean
      Try
        'Check to see if it's null
        If String.IsNullOrEmpty(value) Then Return False

        'See if we can parse it
        Dim tryBoolean As Boolean
        If Boolean.TryParse(value, tryBoolean) Then Return tryBoolean
      Catch ex As Exception
        'No code
      End Try
      Return False
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToFalseBoolean(ByVal value As Object) As Boolean
      Try
        'Make sure we have something
        If value Is Nothing Then Return False

        'If this is a boolean then cast it and return it
        If TypeOf value Is Boolean Then Return DirectCast(value, Boolean)

        'If this is an integer return true if it is not equal to 0
        If TypeOf value Is Integer Then Return (DirectCast(value, Integer) <> 0)

        'See if we can parse it
        Dim tryBoolean As Boolean
        If Boolean.TryParse(value.ToString, tryBoolean) Then Return tryBoolean
      Catch ex As Exception
        'No code
      End Try
      Return False
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToZeroInteger(ByVal value As String) As Integer
      Try
        'Check to see if it's null
        If String.IsNullOrEmpty(value) Then Return 0

        'See if we can parse it
        Dim tryInteger As Integer
        If Integer.TryParse(value, tryInteger) Then Return tryInteger
      Catch ex As Exception
        'No code
      End Try
      Return 0
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToZeroInteger(ByVal value As Object) As Integer
      Try
        'Make sure we have something
        If value Is Nothing OrElse TypeOf value Is DBNull Then Return 0

        'If this is an integer then just return the value
        If TypeOf value Is Integer Then Return DirectCast(value, Integer)

        'See if we can parse it
        Dim tryInteger As Integer
        If Integer.TryParse(value.ToString, tryInteger) Then Return tryInteger
      Catch ex As Exception
        'No code
      End Try
      Return 0
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToZeroDouble(ByVal value As String) As Double
      Try
        'Check to see if it's null
        If String.IsNullOrEmpty(value) Then Return 0

        'See if we can parse it
        Dim tryDouble As Double
        If Double.TryParse(value, tryDouble) Then Return tryDouble
      Catch ex As Exception
        'No code
      End Try
      Return 0
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToZeroDouble(ByVal value As Object) As Double
      Try
        'Make sure we have something
        If value Is Nothing OrElse TypeOf value Is DBNull Then Return 0

        'If this is a double then cast it and return it
        If TypeOf value Is Double Then Return DirectCast(value, Double)

        'See if we can parse it
        Dim tryDouble As Double
        If Double.TryParse(value.ToString, tryDouble) Then Return tryDouble
      Catch ex As Exception
        'No code
      End Try
      Return 0
    End Function

    Public Shared Function NullToEmptyString(ByVal value As Object) As String
      If value Is Nothing OrElse TypeOf value Is DBNull Then Return String.Empty
      Dim str As String = TryCast(value, String) : If str IsNot Nothing Then Return str
      Return CType(value, String)
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToNothingString(ByVal value As Object) As String
      Try
        'If this is a string return the string
        If TypeOf value Is String Then Return DirectCast(value, String)
      Catch ex As Exception
        'No code
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToNothingDateString(ByVal value As Object) As String
      Try
        'If this is not a date then return nothing
        If Not (TypeOf value Is Date) Then Return Nothing

        Dim valueDate = DirectCast(value, Date)
        Return valueDate.ToString
      Catch ex As Exception
        'No code
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToNothingDateString(ByVal value As Object, convertToLocal As Boolean) As String
      Try
        'If this is not a date then return nothing
        If Not (TypeOf value Is Date) Then Return Nothing

        Dim valueDate = DirectCast(value, Date)
        If convertToLocal Then
          Return valueDate.ToLocalTime.ToString
        Else
          Return valueDate.ToString
        End If
      Catch ex As Exception
        'No code
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function NullToNothingDateString(ByVal value As Object, convertToLocal As Boolean, dateFormat As String) As String
      Try
        'If this is not a date then return nothing
        If Not (TypeOf value Is Date) Then Return Nothing

        Dim valueDate = DirectCast(value, Date)
        If convertToLocal Then
          Return valueDate.ToLocalTime.ToString(dateFormat)
        Else
          Return valueDate.ToString(dateFormat)
        End If
      Catch ex As Exception
        'No code
      End Try
      Return Nothing
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function TrueFalseToInteger(value As Boolean) As Integer
      If value = True Then Return -1
      Return 0
    End Function

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Function TrueFalseToInteger(value As Boolean, trueValue As Integer) As Integer
      If value = True Then Return trueValue
      Return 0
    End Function

    Public Shared Function GetInteger(row As DataRow, columnName As String) As Integer
      If row Is Nothing Then Return -1
      If Not row.Table.Columns.Contains(columnName) Then Return -1
      If row.IsNull(columnName) Then Return 0

      Dim value As Integer
      If Integer.TryParse(row(columnName).ToString, value) Then
        Return value
      Else
        Return -1
      End If
    End Function

    Public Shared Function GetDouble(row As DataRow, columnName As String) As Double
      If row Is Nothing Then Return -1
      If Not row.Table.Columns.Contains(columnName) Then Return -1
      If row.IsNull(columnName) Then Return 0

      Dim value As Double
      If Double.TryParse(row(columnName).ToString, value) Then
        Return value
      Else
        Return -1
      End If
    End Function

    Public Shared Function GetDouble(row As DataRow, columnName As String, digits As Integer) As Double
      If row Is Nothing Then Return -1
      If Not row.Table.Columns.Contains(columnName) Then Return -1
      If row.IsNull(columnName) Then Return 0

      Dim value As Double
      If Double.TryParse(row(columnName).ToString, value) Then
        Return Math.Round(value, digits)
      Else
        Return -1
      End If
    End Function

    Public Shared Function GetString(row As DataRow, columnName As String) As String
      If row Is Nothing Then Return Nothing
      If Not row.Table.Columns.Contains(columnName) Then Return Nothing
      If row.IsNull(columnName) Then Return Nothing

      Return row(columnName).ToString
    End Function

#End Region

#Region " Convert Dates "

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub ConvertToLocalTime(table As System.Data.DataTable)
      Try
        For Each row As System.Data.DataRow In table.Rows
          For Each column As System.Data.DataColumn In table.Columns
            If column.DataType Is GetType(Date) Then
              If Not row.IsNull(column) Then row(column) = DirectCast(row(column), Date).ToLocalTime
            End If
          Next
        Next
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub ConvertToLocalTime(row As System.Data.DataRow)
      Try
        For Each column As System.Data.DataColumn In row.Table.Columns
          If column.DataType Is GetType(Date) Then
            If Not row.IsNull(column) Then row(column) = DirectCast(row(column), Date).ToLocalTime
          End If
        Next
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub ConvertToLocalTime(table As System.Data.DataTable, columnName As String)
      Try
        Dim tryDate As Date
        For Each row As System.Data.DataRow In table.Rows
          If Date.TryParse(row(columnName).ToString, tryDate) Then
            If Not row.IsNull(columnName) Then row(columnName) = DirectCast(row(columnName), Date).ToLocalTime
          End If
        Next
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub ConvertToLocalTime(row As System.Data.DataRow, columnName As String)
      Try
        Dim tryDate As Date
        If Date.TryParse(row(columnName).ToString, tryDate) Then
          If Not row.IsNull(columnName) Then row(columnName) = DirectCast(row(columnName), Date).ToLocalTime
        End If
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub ConvertToUniversalTime(table As System.Data.DataTable)
      Try
        For Each row As System.Data.DataRow In table.Rows
          For Each column As System.Data.DataColumn In table.Columns
            If column.DataType Is GetType(Date) Then
              If Not row.IsNull(column) Then row(column) = DirectCast(row(column), Date).ToUniversalTime
            End If
          Next
        Next
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub ConvertToUniversalTime(row As System.Data.DataRow)
      Try
        For Each column As System.Data.DataColumn In row.Table.Columns
          If column.DataType Is GetType(Date) Then
            If Not row.IsNull(column) Then row(column) = DirectCast(row(column), Date).ToUniversalTime
          End If
        Next
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub ConvertToUniversalTime(table As System.Data.DataTable, columnName As String)
      Try
        Dim tryDate As Date
        For Each row As System.Data.DataRow In table.Rows
          If Date.TryParse(row(columnName).ToString, tryDate) Then
            If Not row.IsNull(columnName) Then row(columnName) = DirectCast(row(columnName), Date).ToUniversalTime
          End If
        Next
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Public Shared Sub ConvertToUniversalTime(row As System.Data.DataRow, columnName As String)
      Try
        Dim tryDate As Date
        If Date.TryParse(row(columnName).ToString, tryDate) Then
          If Not row.IsNull(columnName) Then row(columnName) = DirectCast(row(columnName), Date).ToUniversalTime
        End If
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
    End Sub

#End Region

#Region " Table Configuration "

    Public Shared Function AddTable(connectionString As String, tableName As String, columnString As String) As Boolean
      Dim sql As String = Nothing
      Try
        sql = "CREATE TABLE " & tableName & "([ID] [int] IDENTITY(1,1) NOT NULL," & columnString & " CONSTRAINT [PK_" & tableName & "] PRIMARY KEY CLUSTERED ([ID] ASC))"
        Return SqlExecute(connectionString, sql)

      Catch ex As Exception
        Utilities.Log.LogError(ex, sql)
      End Try
      Return False
    End Function

    Public Shared Function UpdateTable(connectionString As String, tableName As String, columnString As String) As Boolean
      Dim sql As String = Nothing
      Try
        ' TODO - Add Code
      Catch ex As Exception
        Utilities.Log.LogError(ex, sql)
      End Try
      Return False
    End Function

    Public Shared Function TableExists(connectionString As String, tableName As String) As Boolean
      Dim sql As String = Nothing
      Try
        ' Get the catalog view with all the table names
        sql = "SELECT Name FROM sys.tables ORDER BY Name"
        Dim sysTables = GetDataTable(connectionString, sql)

        Return TableExists(tableName, sysTables)
      Catch ex As Exception
        Utilities.Log.LogError(ex, sql)
      End Try
      Return False
    End Function

    Public Shared Function TableExists(tableName As String, sysTables As System.Data.DataTable) As Boolean
      Try
        If sysTables Is Nothing Then Return False

        For Each row As System.Data.DataRow In sysTables.Rows
          If row("Name").ToString.Equals(tableName, StringComparison.OrdinalIgnoreCase) Then Return True
        Next
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

    Public Shared Function AlterTableAddColumns(connectionString As String, tableName As String, columnSqlList As String) As Boolean
      Dim sql As String = Nothing
      Try
        ' Get the table structure
        Dim table = GetDataTableSchema(connectionString, tableName)

        ' Odd but just in case
        If table Is Nothing Then Return False

        ' Split the column list into an array 
        Dim columnSqlArray = columnSqlList.Split(",".ToCharArray, System.StringSplitOptions.RemoveEmptyEntries)

        ' Make sure we have some data to work with
        If columnSqlArray Is Nothing OrElse columnSqlArray.Length <= 0 Then Return False

        ' Loop through the list and check to see if any of the columns are already in the table before add the column to the
        '   alter table sql string
        For i = 0 To columnSqlArray.GetUpperBound(0)
          If Not ColumnExists(columnSqlArray(i), table) Then
            If sql Is Nothing Then
              sql = columnSqlArray(i)
            Else
              sql = sql & "," & columnSqlArray(i)
            End If
          End If
        Next

        ' Make sure we still have columns to add
        If sql Is Nothing Then Return False

        ' Execute the alter table statment
        sql = "ALTER TABLE " & tableName & " ADD " & sql
        Utilities.Sql.SqlExecute(connectionString, sql)
      Catch ex As Exception
        Utilities.Log.LogError(ex, sql)
      End Try
      Return False
    End Function

    Private Shared Function ColumnExists(columnSql As String, table As System.Data.DataTable) As Boolean
      Try
        ' Assumes "[]" around column name
        Dim endOfNameIndex = columnSql.IndexOf("]")
        Dim columnName = columnSql.Substring(1, endOfNameIndex - 1)  ' [1]

        For Each column As System.Data.DataColumn In table.Columns
          If column.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase) Then Return True
        Next
      Catch ex As Exception
        Utilities.Log.LogError(ex)
      End Try
      Return False
    End Function

#End Region

#Region " Compare "

    Public Enum EDataCompare
      Same
      Different
      [Error]
    End Enum

    ' Compare the data and structure of two tables to see if the data is different
    '   Use this to see if data has been changed (take a copy of original table to compare)
    Public Shared Function CompareTables(table1 As DataTable, table2 As DataTable) As EDataCompare
      Try
        ' Some easy ones
        If table1 Is Nothing AndAlso table2 Is Nothing Then Return EDataCompare.Same
        If table1 Is Nothing AndAlso table2 IsNot Nothing Then Return EDataCompare.Different
        If table2 Is Nothing AndAlso table1 IsNot Nothing Then Return EDataCompare.Different
        If table1.Columns.Count <> table2.Columns.Count Then Return EDataCompare.Different
        If table1.Rows.Count <> table2.Rows.Count Then Return EDataCompare.Different

        If table1 Is table2 Then Return EDataCompare.Same   'TODO return a specific enum for this case ??
        If table1.Rows.Count = 0 Then Return EDataCompare.Same

        'Compare column names and data types
        For i As Integer = 0 To table1.Columns.Count - 1
          If table1.Columns(i).ColumnName <> table2.Columns(i).ColumnName Then Return EDataCompare.Different
          If Not table1.Columns(i).DataType Is table2.Columns(i).DataType Then Return EDataCompare.Different
        Next

        'Compare each row
        For i As Integer = 0 To table1.Rows.Count - 1
          Dim result = CompareRows(table1.Rows(i), table2.Rows(i))
          If result <> EDataCompare.Same Then Return result
        Next

        ' Must be the same
        Return EDataCompare.Same
      Catch ex As Exception
        ' Ignore for now
      End Try
      Return EDataCompare.Error
    End Function


    Public Shared Function CompareRows(row1 As DataRow, row2 As DataRow) As EDataCompare
      Try
        For i As Integer = 0 To row1.Table.Columns.Count - 1
          If row1(i).ToString <> row2(i).ToString Then Return EDataCompare.Different
        Next

        ' Must be the same
        Return EDataCompare.Same
      Catch ex As Exception
        ' Ignore for now
      End Try
      Return EDataCompare.Error
    End Function


#End Region

#Region " Map DataType to SqlDbType and SqlDbType to DataType "

    ' Map framework DataType to SqlDbType
    Public Shared DataTypeMap As New Dictionary(Of Type, SqlDbType) From {
      {GetType(Boolean), SqlDbType.Bit},
      {GetType(Byte), SqlDbType.TinyInt},
      {GetType(Int16), SqlDbType.SmallInt},
      {GetType(Int32), SqlDbType.Int},
      {GetType(Int64), SqlDbType.BigInt},
      {GetType(DateTime), SqlDbType.DateTime},
      {GetType(TimeSpan), SqlDbType.Time},
      {GetType(DateTimeOffset), SqlDbType.DateTimeOffset},
      {GetType(Single), SqlDbType.Float},
      {GetType(Double), SqlDbType.Float},
      {GetType(Decimal), SqlDbType.Decimal},
      {GetType(Byte()), SqlDbType.Image},
      {GetType(String), SqlDbType.NVarChar},
      {GetType(Guid), SqlDbType.UniqueIdentifier},
      {GetType(Object), SqlDbType.Variant}
    }

    Private Shared SqlDbTypeMap As New Dictionary(Of SqlDbType, Type) From {
      {SqlDbType.BigInt, GetType(Int64)},
      {SqlDbType.Binary, GetType(Byte())},
      {SqlDbType.Bit, GetType(Boolean)},
      {SqlDbType.Char, GetType(String)},
      {SqlDbType.Date, GetType(DateTime)},
      {SqlDbType.DateTime, GetType(DateTime)},
      {SqlDbType.DateTime2, GetType(DateTime)},
      {SqlDbType.DateTimeOffset, GetType(DateTimeOffset)},
      {SqlDbType.Decimal, GetType(Decimal)},
      {SqlDbType.Float, GetType(Double)},
      {SqlDbType.Image, GetType(Byte())},
      {SqlDbType.Int, GetType(Int32)},
      {SqlDbType.Money, GetType(Decimal)},
      {SqlDbType.NChar, GetType(String)},
      {SqlDbType.NText, GetType(String)},
      {SqlDbType.NVarChar, GetType(String)},
      {SqlDbType.Real, GetType(String)},
      {SqlDbType.SmallDateTime, GetType(DateTime)},
      {SqlDbType.SmallInt, GetType(Int16)},
      {SqlDbType.SmallMoney, GetType(Decimal)},
      {SqlDbType.Text, GetType(String)},
      {SqlDbType.Time, GetType(TimeSpan)},
      {SqlDbType.Timestamp, GetType(Byte())},
      {SqlDbType.TinyInt, GetType(Byte)},
      {SqlDbType.UniqueIdentifier, GetType(Guid)},
      {SqlDbType.VarBinary, GetType(Byte())},
      {SqlDbType.VarChar, GetType(String)},
      {SqlDbType.Variant, GetType(Object)},
      {SqlDbType.Xml, GetType(String)}
    }

    Public Shared Function GetSqlDataType(column As DataColumn) As SqlDbType
      If column Is Nothing Then Return Nothing
      Return DataTypeMap(column.DataType)
    End Function

    'Private Shared Sub FillSqlTypeMaps()
    '  DataTypeMap = New Dictionary(Of Type, SqlDbType)
    '  With DataTypeMap
    '    .Add(GetType(Boolean), SqlDbType.Bit)

    '    .Add(GetType(Byte), SqlDbType.TinyInt)
    '    .Add(GetType(Int16), SqlDbType.SmallInt)
    '    .Add(GetType(Int32), SqlDbType.Int)
    '    .Add(GetType(Int64), SqlDbType.BigInt)

    '    .Add(GetType(Single), SqlDbType.Float)
    '    .Add(GetType(Double), SqlDbType.Float)
    '    .Add(GetType(Decimal), SqlDbType.Decimal)

    '    .Add(GetType(String), SqlDbType.NVarChar)

    '    .Add(GetType(DateTime), SqlDbType.DateTime)
    '    .Add(GetType(TimeSpan), SqlDbType.Time)
    '    .Add(GetType(DateTimeOffset), SqlDbType.DateTimeOffset)

    '    .Add(GetType(Guid), SqlDbType.UniqueIdentifier)
    '    .Add(GetType(Object), SqlDbType.Variant)

    '    .Add(GetType(Byte()), SqlDbType.Image)
    '  End With

    '  SqlDbTypeMap = New Dictionary(Of SqlDbType, Type)
    '  With SqlDbTypeMap
    '    .Add(SqlDbType.BigInt, GetType(Int64))
    '    .Add(SqlDbType.Binary, GetType(Byte()))
    '    .Add(SqlDbType.Bit, GetType(Boolean))
    '    .Add(SqlDbType.Char, GetType(String))
    '    .Add(SqlDbType.Date, GetType(DateTime))
    '    .Add(SqlDbType.DateTime, GetType(DateTime))
    '    .Add(SqlDbType.DateTime2, GetType(DateTime))
    '    .Add(SqlDbType.DateTimeOffset, GetType(DateTimeOffset))
    '    .Add(SqlDbType.Decimal, GetType(Decimal))
    '    .Add(SqlDbType.Float, GetType(Double))
    '    .Add(SqlDbType.Image, GetType(Byte()))
    '    .Add(SqlDbType.Int, GetType(Int32))
    '    .Add(SqlDbType.Money, GetType(Decimal))
    '    .Add(SqlDbType.NChar, GetType(String))
    '    .Add(SqlDbType.NText, GetType(String))
    '    .Add(SqlDbType.NVarChar, GetType(String))
    '    .Add(SqlDbType.Real, GetType(String))
    '    .Add(SqlDbType.SmallDateTime, GetType(DateTime))
    '    .Add(SqlDbType.SmallInt, GetType(Int16))
    '    .Add(SqlDbType.SmallMoney, GetType(Decimal))
    '    .Add(SqlDbType.Text, GetType(String))
    '    .Add(SqlDbType.Time, GetType(TimeSpan))
    '    .Add(SqlDbType.Timestamp, GetType(Byte()))
    '    .Add(SqlDbType.TinyInt, GetType(Byte))
    '    .Add(SqlDbType.UniqueIdentifier, GetType(Guid))
    '    .Add(SqlDbType.VarBinary, GetType(Byte()))
    '    .Add(SqlDbType.VarChar, GetType(String))
    '    .Add(SqlDbType.Variant, GetType(Object))
    '    .Add(SqlDbType.Xml, GetType(String))
    '  End With
    'End Sub

#End Region

  End Class

End Namespace