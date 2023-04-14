' Inherit this popup form to maintain a consistent look 

Public MustInherit Class BaseFormPopup : Inherits Form

  Public Sub New()
    MyBase.New

    AutoScaleMode = AutoScaleMode.None
    BackColor = Defaults.BackColor
    DoubleBuffered = True
    Font = Defaults.DefaultFont

    ControlBox = False
    MaximizeBox = False
    MinimizeBox = False
    ShowIcon = False
    ShowInTaskbar = False
    SizeGripStyle = SizeGripStyle.Hide
    StartPosition = FormStartPosition.Manual

    SuspendLayout()
    AddControls()
    ResumeLayout(False)
  End Sub

  ' Only accessible from derived classes
  Protected MustOverride Sub AddControls()

End Class
