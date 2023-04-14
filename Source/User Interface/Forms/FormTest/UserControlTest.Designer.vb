<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UserControlTest
  Inherits System.Windows.Forms.UserControl

  'UserControl overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()>
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
  <System.Diagnostics.DebuggerStepThrough()>
  Private Sub InitializeComponent()
    Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Label2 = New System.Windows.Forms.Label()
    Me.ImageLabel1 = New ImageLabel()
    Me.FlowLayoutPanel1.SuspendLayout()
    Me.SuspendLayout()
    '
    'FlowLayoutPanel1
    '
    Me.FlowLayoutPanel1.Controls.Add(Me.Label1)
    Me.FlowLayoutPanel1.Controls.Add(Me.Label2)
    Me.FlowLayoutPanel1.Controls.Add(Me.ImageLabel1)
    Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
    Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
    Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
    Me.FlowLayoutPanel1.Size = New System.Drawing.Size(842, 713)
    Me.FlowLayoutPanel1.TabIndex = 0
    Me.FlowLayoutPanel1.WrapContents = False
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(3, 0)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(39, 13)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Label1"
    '
    'Label2
    '
    Me.Label2.Image = My.Resources.Resources.Adjustment16x16
    Me.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.Label2.Location = New System.Drawing.Point(3, 13)
    Me.Label2.Name = "Label2"
    Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
    Me.Label2.Size = New System.Drawing.Size(570, 23)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "        Label2"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'ImageLabel1
    '
    Me.ImageLabel1.Image = My.Resources.Resources.Adjustment16x16
    Me.ImageLabel1.Location = New System.Drawing.Point(3, 36)
    Me.ImageLabel1.Name = "ImageLabel1"
    Me.ImageLabel1.Size = New System.Drawing.Size(400, 24)
    Me.ImageLabel1.TabIndex = 2
    Me.ImageLabel1.Text = "ImageLabel1"
    '
    'UserControlTest
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.Controls.Add(Me.FlowLayoutPanel1)
    Me.Name = "UserControlTest"
    Me.Size = New System.Drawing.Size(842, 713)
    Me.FlowLayoutPanel1.ResumeLayout(False)
    Me.FlowLayoutPanel1.PerformLayout()
    Me.ResumeLayout(False)

  End Sub

  Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
  Friend WithEvents Label1 As Label
  Friend WithEvents Label2 As Label
  Friend WithEvents ImageLabel1 As ImageLabel
End Class
