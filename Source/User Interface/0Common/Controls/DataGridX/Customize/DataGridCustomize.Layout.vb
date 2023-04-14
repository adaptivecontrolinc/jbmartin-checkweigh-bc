Partial Public Class DataGridCustomize

  Private panels As DataGridCustomizePanels

  Private WithEvents listSource As LabelListBox
  Private WithEvents listTarget As LabelListBox

  Private WithEvents buttonAdd As Button
  Private WithEvents buttonRemove As Button

  Private WithEvents buttonAddAll As Button
  Private WithEvents buttonRemoveAll As Button

  Private WithEvents buttonProperties As Button

  Private WithEvents buttonMoveUp As Button
  Private WithEvents buttonMoveDown As Button

  Private WithEvents textBoxName As SqlTextBoxOld

  Private WithEvents checkboxLockLayout As CheckBox
  Private WithEvents checkBoxLockSort As CheckBox
  Private WithEvents checkBoxLockFilter As CheckBox

  Private Sub AddControls()
    SuspendLayout()

    panels = New DataGridCustomizePanels
    With panels
      .Dock = DockStyle.Fill
    End With
    Controls.Add(panels)

    AddControlsLeftPanel()
    AddControlsMiddlePanel()
    AddControlsRightPanel()

    ResumeLayout(False)
  End Sub

  Private Sub AddControlsLeftPanel()
    Dim posX = 2, posY = 2

    listSource = New LabelListBox("Available Columns:")
    With listSource
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom
      .Location = New Point(posX, posY)
      .Size = New Size(panels.PanelLeft.Width - 4, panels.PanelLeft.Height - 4)

      .ListBox.SelectionMode = SelectionMode.One
      AddHandler .ListBox.DoubleClick, AddressOf listSource_DoubleClick
    End With
    panels.PanelLeft.Controls.Add(listSource)
  End Sub

  Private Sub AddControlsMiddlePanel()
    Dim posX = 2, posY = 22

    buttonAdd = AddButtonCenter("buttonAdd", "Add", posX, posY) : posY = buttonAdd.Top + buttonAdd.Height + 4
    buttonRemove = AddButtonCenter("buttonRemove", "Remove", posX, posY) : posY = buttonRemove.Top + buttonRemove.Height + 16

    buttonAddAll = AddButtonCenter("buttonAddAll", "Add All", posX, posY) : posY = buttonAddAll.Top + buttonAddAll.Height + 4
    buttonRemoveAll = AddButtonCenter("buttonRemoveAll", "Remove All", posX, posY) : posY = buttonRemoveAll.Top + buttonRemoveAll.Height + 16

    buttonProperties = AddButtonCenter("buttonProperties", "Properties", posX, posY)
  End Sub

  Private Sub AddControlsRightPanel()
    Dim posX = 2, posY = 2

    listTarget = New LabelListBox("Selected Columns:")
    With listTarget
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom

      .Location = New Point(posX, posY)
      .Size = New Size(panels.PanelRight.Width - 4, panels.PanelRight.Height - 120)

      .ListBox.SelectionMode = SelectionMode.One
      AddHandler .ListBox.DoubleClick, AddressOf listTarget_DoubleClick
    End With
    panels.PanelRight.Controls.Add(listTarget)


    posY = listTarget.Top + listTarget.Height

    buttonMoveUp = New Button
    With buttonMoveUp
      .Anchor = AnchorStyles.Left Or AnchorStyles.Bottom
      .Text = "Move Up"

      .Size = New Size(100, 28)
      .Location = New Point(posX - 1, posY)
    End With
    panels.PanelRight.Controls.Add(buttonMoveUp)

    posX = listTarget.Left + listTarget.Width - 100

    buttonMoveDown = New Button
    With buttonMoveDown
      .Anchor = AnchorStyles.Right Or AnchorStyles.Bottom
      .Text = "Move Down"

      .Size = New Size(100, 28)
      .Location = New Point(posX + 1, posY)
    End With
    panels.PanelRight.Controls.Add(buttonMoveDown)

    posX = listTarget.Left
    posY = buttonMoveDown.Top + buttonMoveDown.Height + 20

    checkboxLockLayout = AddCheckBoxRight("checkBoxLayout", "Lock Column Layout", listTarget.Width, posX, posY) : posY = checkboxLockLayout.Top + 18
    checkBoxLockSort = AddCheckBoxRight("checkBoxSort", "Lock Sort Order", listTarget.Width, posX, posY) : posY = checkBoxLockSort.Top + 18
    checkBoxLockFilter = AddCheckBoxRight("checkBoxFilter", "Lock Filter", listTarget.Width, posX, posY)
  End Sub



  Private Function AddButtonCenter(name As String, text As String, x As Integer, y As Integer) As Button
    Dim buttonWidth = panels.PanelMiddle.Width - 4
    Dim buttonHeight = 28

    Dim newButton = New Button
    With newButton
      .Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
      .BackColor = SystemColors.Control
      .Name = name
      .Text = text
      .Size = New Size(buttonWidth, buttonHeight)
      .Location = New Point(x, y)
    End With
    panels.PanelMiddle.Controls.Add(newButton)
    Return newButton
  End Function

  Private Function AddCheckBoxRight(name As String, text As String, width As Integer, x As Integer, y As Integer) As CheckBox
    Dim newCheckbox = New CheckBox
    With newCheckbox
      .Anchor = AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
      .Name = name
      .Text = text

      .CheckAlign = ContentAlignment.MiddleLeft
      .TextAlign = ContentAlignment.MiddleLeft

      .Size = New Size(listTarget.Width, 18)
      .Location = New Point(x, y)
    End With
    panels.PanelRight.Controls.Add(newCheckbox)
    Return newCheckbox
  End Function
End Class
