Public Class ControlBatches : Inherits UserControl
  Private controlCode As ControlCode

  Public Event BatchSelected(sender As Object, batchRow As DataRow)
  Public Event ButtonClick(sender As Object, button As Button)

  Private textboxSearch As TextBox
  Private buttonClear As Button
  Private batchList As BatchList
  Private groupBoxBatches As GroupBox

  Private batchButtons As BatchButtons
  Private groupBoxButtons As GroupBox

  Private marginLeft As Integer = 8
  Private marginRight As Integer = 8

  Private marginTop As Integer = 8
  Private marginBottom As Integer = 8

  Private defaultWidth As Integer = 1024
  Private defaultHeight As Integer = 768

  Private controlSpacing As Integer = 4
  Private buttonsWidth As Integer = 200

  Sub New(ByVal controlCode As ControlCode)
    Me.controlCode = controlCode

    DoubleBuffered = True
    Me.Size = New Size(defaultWidth, defaultHeight)
    AddControls()
    ActiveControl = textboxSearch
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    AddList()
    AddButtons()

    ResumeLayout(False)
  End Sub

  Private Sub AddList()
    Dim x, y, width, height As Integer

    x = marginLeft
    y = marginTop
    width = defaultWidth - marginLeft - marginRight - controlSpacing - buttonsWidth
    height = defaultHeight - marginTop - marginBottom

    groupBoxBatches = New GroupBox
    With groupBoxBatches
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Padding = New Padding(12, 8, 8, 8) ' not really necessary cos we're anchoring but a good spacing reminder

      .Location = New Point(x, y)
      .Size = New Size(width, height)

      .Name = "Batches"
      .Text = "Select or scan a batch to weigh"
    End With
    Controls.Add(groupBoxBatches)

    x = 12 : y = 21
    width = groupBoxBatches.Width - 24 - 128

    textboxSearch = New TextBox
    With textboxSearch
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
      .Location = New Point(x, y)
      .Width = width

      .Font = New Font("Tahoma", 14)
      AddHandler .KeyDown, AddressOf TextBoxSearch_KeyDown
    End With
    groupBoxBatches.Controls.Add(textboxSearch)


    x = groupBoxBatches.Width - 12 - 120
    y = 20
    buttonClear = New Button
    With buttonClear
      .Anchor = AnchorStyles.Top Or AnchorStyles.Right
      .Location = New Point(x, y)
      .Size = New Size(120, textboxSearch.Height)

      .Text = "Clear..."

      AddHandler .Click, AddressOf ButtonClear_Click
    End With
    groupBoxBatches.Controls.Add(buttonClear)


    x = 12
    y = textboxSearch.Top + textboxSearch.Height + 8
    width = groupBoxBatches.Width - (x * 2)
    height = groupBoxBatches.Height - y - 9

    batchList = New BatchList(Me.controlCode)
    With batchList
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Location = New Point(x, y)
      .Size = New Size(width, height)
    End With
    groupBoxBatches.Controls.Add(batchList)

  End Sub

  Private Sub AddButtons()
    Dim x, y, width, height As Integer

    batchButtons = New BatchButtons(Me.controlCode)
    With batchButtons
      .Dock = DockStyle.Fill

      AddHandler .ButtonClick, AddressOf Buttons_ButtonClick
    End With

    x = defaultWidth - marginLeft - buttonsWidth
    y = marginTop
    width = buttonsWidth
    height = defaultHeight - marginTop - marginBottom

    groupBoxButtons = New GroupBox
    With groupBoxButtons
      .Anchor = AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Padding = New Padding(8)

      .Location = New Point(x, y)
      .Size = New Size(width, height)

      .Name = "Buttons"
      .Text = "Actions"
      .Controls.Add(batchButtons)
    End With
    Controls.Add(groupBoxButtons)
  End Sub

  Private Sub Buttons_ButtonClick(sender As Object, button As Button)
    Select Case button.Name
      Case "Main" : SelectBatchFromList()
      Case "Grid" : CustomizeGrid()
      Case Else
        RaiseEvent ButtonClick(sender, button)  ' pass it along
    End Select
    textboxSearch.Select()
  End Sub

  Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
    textboxSearch.Text = Nothing
    textboxSearch.Select()
  End Sub

  Private Sub TextBoxSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs)
    If e.KeyCode = Keys.Enter Then SelectBatchFromSearch()
  End Sub

  Private Sub SelectBatchFromSearch()
    Try
      Dim batch = textboxSearch.Text
      Dim values = batch.Split(Settings.BarcodeSeparator.ToCharArray, StringSplitOptions.RemoveEmptyEntries)
      If values Is Nothing OrElse values.Length <= 0 Then
        MessageBox.Show("Batch " & batch & " split to nothing.", "Adaptive", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Exit Sub
      End If

      Dim strDyelot As String = Nothing
      Dim strRedye As String = Nothing
      Dim redye As Integer = 0

      Select Case values.Length
        Case 1
          strDyelot = values(0).Trim
        Case 2
          strDyelot = values(0).Trim
          strRedye = values(1).Trim
      End Select

      If Not String.IsNullOrEmpty(strDyelot) Then

        Dim tryRedye As Integer
        If Integer.TryParse(strRedye, tryRedye) Then
          redye = tryRedye
        End If

        If batchList.SetSelectedRow(strDyelot, redye) Then
          SelectBatchFromList()
        Else
          MessageBox.Show("Batch " & strDyelot & "@" & redye.ToString & " not found", "Adaptive", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

      End If

    Catch ex As Exception
      Utilities.Log.LogError(ex.ToString, textboxSearch.Text)
    End Try
  End Sub

  Private Sub SelectBatchFromList()
    Dim batchRow = batchList.GetSelectedDataRow
    If batchRow IsNot Nothing Then
      RaiseEvent BatchSelected(Me, batchRow)
    End If
    textboxSearch.Select()
  End Sub

  Private Sub CustomizeGrid()
    batchList.CustomizeGrid()
  End Sub

  Sub Requery()
    batchList.Requery()
    textboxSearch.Select()
  End Sub

End Class

