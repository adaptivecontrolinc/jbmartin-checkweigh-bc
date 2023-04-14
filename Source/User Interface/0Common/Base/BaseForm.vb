' Inherit this form to maintain a consistent look 

Public MustInherit Class BaseForm : Inherits Form

  Public Sub New()
    MyBase.New

    AutoScaleMode = AutoScaleMode.None
    BackColor = Defaults.BackColor
    DoubleBuffered = True
    Font = Defaults.DefaultFont

    ControlBox = True
    FormBorderStyle = FormBorderStyle.FixedDialog
    MaximizeBox = False
    MinimizeBox = False
    ShowIcon = False
    ShowInTaskbar = False
    SizeGripStyle = SizeGripStyle.Hide
    StartPosition = FormStartPosition.CenterParent

    SuspendLayout()
    AddControls()
    ResumeLayout(False)
  End Sub

  ' Only accessible from derived classes
  Protected MustOverride Sub AddControls()

End Class
