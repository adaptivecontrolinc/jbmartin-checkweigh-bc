' Custom check box or the data grid
'   Displays a simple check box    
Partial Public Class DataGridX

  Class XColumnCheckBox
    Inherits DataGridViewColumn

    Sub New()
      MyBase.New(New XCellCheckBox())
      Me.ReadOnly = True
    End Sub

    Overrides Property CellTemplate() As DataGridViewCell
      Get
        Return MyBase.CellTemplate
      End Get
      Set(ByVal value As DataGridViewCell)
        ' Ensure that the cell used for the template is the correct type
        If (value IsNot Nothing) AndAlso Not value.GetType().IsAssignableFrom(GetType(XCellCheckBox)) Then
          Throw New InvalidCastException("Must be a Check Box Integer Cell")
        End If
        MyBase.CellTemplate = value
      End Set
    End Property
  End Class

  Class XCellCheckBox
    Inherits DataGridViewCheckBoxCell

    ' Make sure we always return 0 or -1
    Protected Overrides Function GetValue(rowIndex As Integer) As Object
      Dim value = MyBase.GetValue(rowIndex)
      If TypeOf value Is Integer Then
        Dim valueInt = DirectCast(value, Integer)
        If valueInt = 1 OrElse valueInt = -1 Then Return -1
      End If
      Return 0
    End Function

  End Class


End Class
