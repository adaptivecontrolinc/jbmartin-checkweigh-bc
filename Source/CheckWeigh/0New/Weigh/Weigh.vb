' NOTE
'  Use grams internally but use the balance display units for all UI

Public Class Weigh
  Private ReadOnly controlCode As ControlCode

  Public Sub New(ByVal controlCode As ControlCode)
    Me.controlCode = controlCode
  End Sub

  Property Items As List(Of WeighItem)            ' List of all weigh items in this step
  Property Item As WeighItem                      ' Current weigh item
  Property Balance As BalanceBase                 ' Balance to use to weigh the current weigh item
  Public Enum EState
    Idle               ' inactive
    Start              ' load the weigh list
    User               ' log the user
    Barcode            ' check the barcode
    Balance            ' select the balance to use
    Tare               ' tare the scale
    Weigh              ' weigh the item
    Done               ' complete - update the database
  End Enum
  Property State As EState
  Property Timer As New Timer
  Property Message As String

  Public Sub Start()
    Try

    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Public Sub Run()
    Try
      Select Case State
        Case EState.Idle
          Message = "Idle"

        Case EState.Start
          Message = "Start"

        Case EState.User
          Message = "Scan or enter operator code"

        Case EState.Barcode
          Message = "Scan or enter the product code"
          If Timer.Finished Then
            If CheckBarcode() Then
              State = EState.Balance
              Timer.Seconds = 2
            End If
          End If

        Case EState.Balance
          Message = "Selecting scale to use to weigh this product"
          If Timer.Finished Then
            SetBalance()
            Item.BalanceName = Balance.Name  ' Log the balance name
            State = EState.Tare
            Timer.Seconds = 2
          End If

        Case EState.Tare
          Message = "Place an empty container on the scale and Tare the scale"
          If Timer.Finished Then
            If CheckTare() Then
              State = EState.Weigh
              Timer.Seconds = 2
            End If
          End If

        Case EState.Weigh
          Message = "Add product to the container until the progress bar is green"
          If Timer.Finished Then
            If CheckWeight() Then
              State = EState.Done
              Timer.Seconds = 2
            End If
          End If

        Case EState.Done
          Message = Nothing

      End Select

    Catch ex As Exception
      Utilities.Log.LogError(ex)
    End Try
  End Sub

  Function CheckBarcode() As Boolean
    If Item Is Nothing Then Return False
    Return Item.CheckBarcode
  End Function

  Sub SkipBarcode()
    If State = EState.Barcode Then State = EState.Balance
  End Sub

  Sub SetBalance()
    If State <> EState.Balance Then Exit Sub
    controlCode.IO.Balance = controlCode.IO.BalanceList(0)
    ' TODO If more than one scale is defined pick scale based on product type or target grams + tolerance
  End Sub

  Function CheckTare() As Boolean
    If State <> EState.Tare Then Return False
    Return Balance.Tared
  End Function

  Sub SkipTare()
    If State = EState.Tare Then State = EState.Weigh
  End Sub

  Function CheckWeight() As Boolean
    If Item Is Nothing OrElse Balance Is Nothing Then Return False
    If State = EState.Weigh Then

    End If
    Return False
  End Function

  Sub SkipWeigh()
    If State = EState.Weigh Then

    End If
  End Sub

  Sub SetPreWeighedGrams(grams As Double)
    If Item Is Nothing Then Exit Sub
    If State <> EState.Weigh Then Exit Sub
    Item.GramsPreWeighed = grams
  End Sub

  Sub SetNewContainerGrams(grams As Double)
    If Item Is Nothing Then Exit Sub
    If State = EState.Weigh Then
      Item.GramsNewContainer = grams
      State = EState.Tare
      Timer.Seconds = 2
    End If
  End Sub

  Sub Abort()
  End Sub

End Class
