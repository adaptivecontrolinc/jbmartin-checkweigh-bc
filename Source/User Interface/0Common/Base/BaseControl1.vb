' Inherit this user control to maintain a consistent look 
'   Use BaseControl2 if you need to pass a parameter into the constructor (ControlCode for instance)
Public MustInherit Class BaseControl1 : Inherits UserControl

  Sub New()
    MyBase.New

    AutoScaleMode = AutoScaleMode.None
    BackColor = Defaults.BackColor
    DoubleBuffered = True
    Font = Defaults.DefaultFont

    SuspendLayout()
    AddControls()
    ResumeLayout(False)
  End Sub

  ' Only accessible from derived classes
  Protected MustOverride Sub AddControls()

End Class


