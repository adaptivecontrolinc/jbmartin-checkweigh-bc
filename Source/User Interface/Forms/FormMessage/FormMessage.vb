Public Class FormMessage : Inherits Form

  Private ReadOnly defaultWidth As Integer = 300
  Private ReadOnly defaultHeight As Integer = 150

  Private LabelMessage As Label
  Private WithEvents ButtonOK As Button

  Sub New()
    ClientSize = New Size(defaultWidth, defaultHeight)
    Text = "Balance selection"

    ControlBox = False
    FormBorderStyle = FormBorderStyle.FixedSingle
    MaximizeBox = False
    MinimizeBox = False
    ShowInTaskbar = False
    SizeGripStyle = Windows.Forms.SizeGripStyle.Hide
    StartPosition = FormStartPosition.CenterParent

    AddControls()
  End Sub

  Private Sub AddControls()
    SuspendLayout()

    LabelMessage = New Label
    With LabelMessage
      .Location = New Point(20, 20)
      .Size = New Size(260, 50)

      .Image = My.Resources.Scale32x32

      .ImageAlign = ContentAlignment.TopLeft

      .Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      .TextAlign = ContentAlignment.TopCenter
    End With
    Controls.Add(LabelMessage)

    ButtonOK = New Button
    With ButtonOK
      .Location = New Point(50, 96)
      .Size = New Size(200, 40)

      .BackColor = System.Drawing.Color.Silver
      .FlatStyle = FlatStyle.Standard
      .Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

      .TabIndex = 0
      .Text = "OK"
    End With
    Controls.Add(ButtonOK)

    ResumeLayout(False)
  End Sub

  Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
    Me.Close()
  End Sub

  Public Sub Connect(balance As BalanceBase)
    LabelMessage.Text = "Use balance: " & balance.Name
    LabelMessage.Refresh()
  End Sub

  Public Sub Connect(balance As BalanceBase, product As ProductClass)
    LabelMessage.Text = "Use balance: " & balance.Name
  End Sub

End Class


