Imports System.ComponentModel

Public Class IO : Inherits MarshalByRefObject

  Property Balance As BalanceBase
  Property BalanceList As New List(Of BalanceBase) ' List of all balances 

  Public Sub New(ByVal controlCode As ControlCode)

    ' Load balance setup from the balance xml files (balanceXX.xml) 'balanceserial or balancedemo or balancetcp
    Dim balanceXmlLoader = New BalanceXml(controlCode)
    balanceXmlLoader.LoadFromXml(BalanceList)

    ' Set the active balance to balance 0 initially
    Balance = BalanceList(0)

    ' Start comms for all balances
    For i As Integer = 0 To BalanceList.Count - 1
      BalanceList(i).Start()
    Next i

  End Sub


End Class
