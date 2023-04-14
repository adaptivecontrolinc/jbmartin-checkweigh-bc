Partial Public Class Utilities
  Public NotInheritable Class Schema

    Property SqlConnection As String

    Property TableList As DataTable

    Property ColumnList As DataTable

    Property IndexList As DataTable

    Sub New(sqlConnection As String)
      Me.SqlConnection = sqlConnection

      TableList = GetTableList()
      ColumnList = GetColumnList()
      IndexList = GetIndexList()
    End Sub

    Private Function GetTableList() As DataTable
      Dim sql As String = Nothing
      Try
        ' Get a list of all the tables in the database so we can test to see if a specific table already exists
        sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME"
        Return Utilities.SQL.GetDataTable(SqlConnection, sql)

      Catch ex As Exception
        Utilities.LogError(ex, sql)
      End Try
      Return Nothing
    End Function

    Private Function GetColumnList() As DataTable
      Dim sql As String = Nothing
      Try
        ' Get a list of all the columns in the database so we can see if a specific column already exists
        sql = "SELECT TABLE_NAME,COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS ORDER BY TABLE_NAME,COLUMN_NAME"
        Return Utilities.SQL.GetDataTable(SqlConnection, sql)
      Catch ex As Exception
        Utilities.LogError(ex, sql)
      End Try
      Return Nothing
    End Function

    Private Function GetIndexList() As DataTable
      Dim sql As String = Nothing
      Try
        sql = "SELECT Name FROM sys.indexes WHERE Name Like 'PK%' OR Name Like 'IX%'"
        Return Utilities.SQL.GetDataTable(SqlConnection, sql)
      Catch ex As Exception
        Utilities.LogError(ex, sql)
      End Try
      Return Nothing
    End Function

    Sub CheckTable(sqlColumnList As String, tableName As String)
      Try
        ' Check to see if the table exists and either add it or update it
        If TableExists(tableName) Then
          UpdateTable(sqlColumnList, tableName)
        Else
          AddTable(sqlColumnList, tableName)
        End If
      Catch ex As Exception
        Utilities.LogError(ex, sqlColumnList)
      End Try
    End Sub

    Sub AddTable(sqlColumnList As String, tableName As String)
      Dim sql As String = Nothing
      Try
        ' Create the table
        sql = "CREATE TABLE [" & tableName & "] (" & sqlColumnList & ")"
        Utilities.SQL.SqlExecute(SqlConnection, sql)

        ' Set default value for created if column is defined
        If sqlColumnList.Contains("[Created] [datetime]") Then
          Dim constraintName = "DF_" & tableName & "_Created"
          sql = "ALTER TABLE [" & tableName & "] ADD CONSTRAINT [" & constraintName & "] DEFAULT (getutcdate()) FOR [Created]"
          Utilities.SQL.SqlExecute(SqlConnection, sql)
        End If

      Catch ex As Exception
        Utilities.LogError(ex, sqlColumnList)
      End Try
    End Sub

    Sub UpdateTable(sqlColumnList As String, tableName As String)
      Dim sql As String = Nothing
      Try
        ' Convert table definition into an array of column definitions
        Dim sqlColumns = GetColumnArray(sqlColumnList)

        ' Log this ??
        If sqlColumns Is Nothing OrElse sqlColumns.Length < 2 Then Exit Sub

        ' Loop through the array and add each column - last row is the primary key definition
        For i As Integer = 0 To sqlColumns.GetUpperBound(0) - 1
          AddColumn(sqlColumns(i), tableName)
        Next
      Catch ex As Exception
        Utilities.LogError(ex)
      End Try
    End Sub

    Sub AddColumn(sqlColumn As String, tableName As String)
      Dim sql As String = Nothing
      Try
        Dim columnName = GetColumnName(sqlColumn)
        If ColumnExists(tableName, columnName) Then Exit Sub

        sql = "ALTER TABLE " & tableName & " ADD " & sqlColumn
        Utilities.SQL.SqlExecute(SqlConnection, sql)
      Catch ex As Exception
        Utilities.LogError(ex, sql)
      End Try
    End Sub

    Private Function TableExists(tableName As String) As Boolean
      Try
        Dim searchString = "TABLE_NAME='" & tableName & "'"
        Dim rows = TableList.Select(searchString)
        If rows IsNot Nothing AndAlso rows.Length > 0 Then Return True
      Catch ex As Exception
        Utilities.LogError(ex, tableName)
      End Try
      Return False
    End Function

    Private Function ColumnExists(tableName As String, columnName As String) As Boolean
      Try
        Dim searchString = "TABLE_NAME='" & tableName & "' AND COLUMN_NAME='" & columnName & "'"
        Dim rows = ColumnList.Select(searchString)
        If rows IsNot Nothing AndAlso rows.Length > 0 Then Return True
      Catch ex As Exception
        Utilities.LogError(ex, tableName & " " & columnName)
      End Try
      Return False
    End Function


    ' Return the column definitions as an array (one element per column)
    Private Function GetColumnArray(sqlTable As String) As String()
      ' Replace "NULL," with "NULL;" so we can use string.split on ";" to create the column array
      Dim newString = sqlTable.Replace("NULL,", "NULL;")
      Return newString.Split(";".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
    End Function

    ' Extract the column name from the sql (must be wrapped with [])
    Private Function GetColumnName(sqlColumn As String) As String
      'Return just the column name from the column definition
      Dim index1 = sqlColumn.IndexOf("[".ToCharArray)
      Dim index2 = sqlColumn.IndexOf("]".ToCharArray)

      ' Start after "[" and don't included "]"
      Return sqlColumn.Substring(index1 + 1, index2 - index1 - 1)
    End Function


    Sub AddIndex(name As String, table As String, columns As String)
      Dim sql As String = Nothing
      Try
        If Not IndexExists(name) Then
          sql = "CREATE INDEX " & name & " ON " & table & " (" & columns & ")"
          Utilities.SQL.SqlExecute(SqlConnection, sql)
        End If

      Catch ex As Exception
        Utilities.LogError(ex, sql)
      End Try
    End Sub

    Sub AddUniqueIndex(name As String, table As String, columns As String)
      Dim sql As String = Nothing
      Try
        If Not IndexExists(name) Then
          sql = "CREATE UNIQUE INDEX " & name & " ON " & table & " (" & columns & ")"
          Utilities.SQL.SqlExecute(SqlConnection, sql)
        End If

      Catch ex As Exception
        Utilities.LogError(ex, sql)
      End Try
    End Sub

    Sub DropIndex(name As String, table As String)
      Dim sql As String = Nothing
      Try
        If IndexExists(name) Then
          sql = "DROP INDEX " & name & " ON " & table
          Utilities.SQL.SqlExecute(SqlConnection, sql)
        End If
      Catch ex As Exception
        Utilities.LogError(ex, sql)
      End Try
    End Sub

    Private Function IndexExists(name As String) As Boolean
      Try
        If IndexList Is Nothing OrElse IndexList.Rows.Count <= 0 Then Return False

        For Each row As DataRow In IndexList.Rows
          If row("Name").ToString.Equals(name, StringComparison.InvariantCultureIgnoreCase) Then Return True
        Next

      Catch ex As Exception
        Utilities.LogError(ex)
      End Try
      Return False
    End Function

  End Class

End Class