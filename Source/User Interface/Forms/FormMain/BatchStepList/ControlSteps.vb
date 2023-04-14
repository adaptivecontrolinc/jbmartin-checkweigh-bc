Public Class ControlSteps : Inherits UserControl
  Private controlCode As ControlCode

  Public Event StepSelected(sender As Object, batchRow As DataRow, stepRow As DataRow)
  Public Event ButtonClick(sender As Object, button As Button)

  Private textBoxBatch As TextBox
  Private stepList As StepList
  Private groupBoxSteps As GroupBox

  Private stepButtons As StepButtons
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

    groupBoxSteps = New GroupBox
    With groupBoxSteps
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Padding = New Padding(12, 8, 8, 8) ' not really necessary cos we're anchoring but a good spacing reminder

      .Location = New Point(x, y)
      .Size = New Size(width, height)

      .Name = "Steps"
      .Text = "Select a batch step to weigh"
    End With
    Controls.Add(groupBoxSteps)

    x = 12 : y = 21
    width = groupBoxSteps.Width - 24

    textBoxBatch = New TextBox
    With textBoxBatch
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
      .Location = New Point(x, y)
      .Width = width

      .Font = New Font("Tahoma", 14)

      .ReadOnly = True
      .TextAlign = HorizontalAlignment.Center
    End With
    groupBoxSteps.Controls.Add(textBoxBatch)

    x = 12
    y = textBoxBatch.Top + textBoxBatch.Height + 8
    width = groupBoxSteps.Width - (x * 2)
    height = groupBoxSteps.Height - y - 9

    stepList = New StepList(controlCode)
    With stepList
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Location = New Point(x, y)
      .Size = New Size(width, height)
    End With
    groupBoxSteps.Controls.Add(stepList)
  End Sub

  Private Sub AddButtons()
    stepButtons = New StepButtons(controlCode)
    With stepButtons
      .Dock = DockStyle.Fill
      AddHandler .ButtonClick, AddressOf Buttons_ButtonClick
    End With

    Dim x = defaultWidth - marginLeft - buttonsWidth
    Dim y = marginTop

    Dim width = buttonsWidth
    Dim height = defaultHeight - marginTop - marginBottom

    groupBoxButtons = New GroupBox
    With groupBoxButtons
      .Anchor = AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Padding = New Padding(8)

      .Location = New Point(x, y)
      .Size = New Size(width, height)

      .Name = "Buttons"
      .Text = "Actions"
      .Controls.Add(stepButtons)
    End With
    Controls.Add(groupBoxButtons)
  End Sub

  Private Sub Buttons_ButtonClick(sender As Object, button As Button)
    Select Case button.Name
      Case "Main" : SelectStepFromList()
      Case "Grid" : CustomizeGrid()
      Case Else
        RaiseEvent ButtonClick(sender, button)  ' pass it along
    End Select
  End Sub

  Private Sub SelectStepFromList()
    Dim batchRow = stepList.Data.BatchRow
    Dim stepRow = stepList.GetSelectedDataRow
    If batchRow Is Nothing OrElse stepRow Is Nothing Then Exit Sub

    RaiseEvent StepSelected(Me, batchRow, stepRow)
  End Sub
  Private Sub CustomizeGrid()
    stepList.CustomizeGrid()
  End Sub

  Sub Requery()
    stepList.Requery()
  End Sub

  Sub Connect(batchRow As DataRow)
    stepList.Connect(batchRow)

    Dim machine = batchRow("Machine").ToString
    Dim dyelot = batchRow("Dyelot").ToString
    Dim redye = Utilities.Sql.NullToZeroInteger(batchRow("ReDye"))
    '  Dim stepNumber = batchRow("StepNumber").ToString

    If redye > 0 Then dyelot = dyelot & "@" & redye.ToString

    textBoxBatch.Text = machine & " -> " & dyelot

    ' Insert Batch
    '    controlCode.Manager.AddBatch(dyelot, "CW1")  ' Moved to FormWeigh.Start
  End Sub

End Class
