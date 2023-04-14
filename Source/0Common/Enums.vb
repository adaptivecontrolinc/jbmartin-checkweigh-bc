Public Module Enums

  Public Enum EUnits
    Metric
    Imperial
  End Enum

  Public Enum EBalanceUnits
    [Grams]
    [Kilograms]
    [Ounces]
    [Pounds]
  End Enum

  Public Enum ERecipeStepType As Integer
    [None] = 0
    [Dye] = 1
    [Chemical] = 2
    [Message] = 3
    [Treatment] = 4
    [Comment] = 5
  End Enum

  Private eRecipeStepTypeTable_ As DataTable
  Public ReadOnly Property ERecipeStepTypeTable As DataTable
    Get
      If eRecipeStepTypeTable_ Is Nothing Then eRecipeStepTypeTable_ = GetEnumDataTable(GetType(ERecipeStepType))
      Return eRecipeStepTypeTable_
    End Get
  End Property

  Public Enum ERecipeUnits As Integer
    [None] = 0
    [Percent] = 1
    [GramsPerLiter] = 2
    [Grams] = 3
    [Kilograms] = 4
    [Liters] = 5
    [Pounds] = 6
    [Gallons] = 7
    [OuncesPerGallon] = 11
    [PoundsPerGallon] = 12
  End Enum

  Private eRecipeUnitsTable_ As DataTable
  Public ReadOnly Property ERecipeUnitsTable As DataTable
    Get
      If eRecipeUnitsTable_ Is Nothing Then eRecipeUnitsTable_ = GetEnumDataTable(GetType(ERecipeUnits))
      Return eRecipeUnitsTable_
    End Get
  End Property

  Function GetEnumDataTable(enumType As Type) As DataTable
    Dim table = New DataTable
    FillEnumDataTable(enumType, table)
    Return table
  End Function

  Sub FillEnumDataTable(enumType As Type, enumTable As DataTable)
    ' Assumes the values are integers 
    enumTable.Columns.Add("ID", GetType(Integer))
    enumTable.Columns.Add("Code", GetType(String))

    Dim names As Array = System.Enum.GetNames(enumType)
    Dim values As Array = System.Enum.GetValues(enumType)

    For i As Integer = 0 To names.GetUpperBound(0)
      Dim newRow = enumTable.NewRow
      newRow("ID") = values.GetValue(i)
      newRow("Code") = names.GetValue(i)
      enumTable.Rows.Add(newRow)
    Next
  End Sub


  Public Enum ProductState
    Setup
    ScanUser
    ScanCode
    ScanLotNumber
    SelectScale
    Tare
    Weigh
    NewContainer
    SkipProduct
    Done
    Off
  End Enum

  Public Enum ProductType
    Null = 0
    Liquid = 2
    Powder = 3
    Active = 4
    Obsolete = 5
  End Enum

  Public Enum DispenseState
    Null = 0
    Ready = 101
    Busy = 102
    Auto = 201
    Scheduled = 202
    Complete = 301
    Manual = 302
    ManualCopy = 304
    Skipped = 308
    [Error] = 309
  End Enum

End Module