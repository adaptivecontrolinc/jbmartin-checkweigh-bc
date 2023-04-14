
Public Interface ISqlControl
  Property SqlAutoFill As Boolean

  Property SqlColumnName As String

  Property SqlValue As Object

  Property SqlValueType As ESqlValueType

End Interface

Public Enum ESqlValueType
  [String]
  [Integer]
  [Double]
  [Date]
  [Boolean]
End Enum

Public Enum ELabelPosition As Integer
  Top
  Left
End Enum