﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ControlTextBox
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Me.TextBox1 = New System.Windows.Forms.TextBox
    Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
    Me.SuspendLayout()
    '
    'TextBox1
    '
    Me.TextBox1.BackColor = System.Drawing.SystemColors.Window
    Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.TextBox1.Location = New System.Drawing.Point(0, 0)
    Me.TextBox1.Multiline = True
    Me.TextBox1.Name = "TextBox1"
    Me.TextBox1.ReadOnly = True
    Me.TextBox1.Size = New System.Drawing.Size(150, 150)
    Me.TextBox1.TabIndex = 0
    '
    'ControlTextBox
    '
    Me.Controls.Add(Me.TextBox1)
    Me.Name = "ControlTextBox"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents Text1 As System.Windows.Forms.TextBox
  Friend WithEvents TimerMain As System.Windows.Forms.Timer
  Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
  Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
