' Inherit this user control to maintain a consistent look 
'   Use BaseControl1 if you do not need to pass a parameter into the constructor
Public MustInherit Class BaseControl2 : Inherits UserControl

  Sub New()
    MyBase.New

    AutoScaleMode = AutoScaleMode.None
    BackColor = Defaults.BackColor
    DoubleBuffered = True
    Font = Defaults.DefaultFont
  End Sub

End Class



