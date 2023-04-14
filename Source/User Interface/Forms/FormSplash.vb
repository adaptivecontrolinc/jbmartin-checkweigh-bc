Public Class FormSplash
  Inherits System.Windows.Forms.Form

  Private Sub InitializeForm()
    ButtonOK.Visible = True
    LabelCompany.Text = Application.CompanyName
    LabelProduct.Text = Application.ProductName
    LabelVersion.Text = "Version " & Application.ProductVersion.ToString
    LabelCopyright.Text = "Copyright © Adaptive Incorporated, all rights reserved."
    LabelWarning.Text = "Warning: This computer program is protected by copyright law and international treaties. Unauthorised reproduction or distribution of this program, or any portion of it, may result in severe civil and criminal penalties, and will be prosecuted to the maximum extent possible under law."
  End Sub

  Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
    Me.Close()
  End Sub

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call
    InitializeForm()
  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents LabelCopyright As System.Windows.Forms.Label
  Friend WithEvents LabelWarning As System.Windows.Forms.Label
  Friend WithEvents ButtonOK As System.Windows.Forms.Button
  Friend WithEvents LabelCompany As System.Windows.Forms.Label
  Friend WithEvents LabelProduct As System.Windows.Forms.Label
  Friend WithEvents LabelVersion As System.Windows.Forms.Label
  Friend WithEvents TimerMain As System.Windows.Forms.Timer
  '<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
  Friend WithEvents PictureBox As System.Windows.Forms.PictureBox

  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSplash))
    Me.PictureBox = New System.Windows.Forms.PictureBox()
    Me.LabelCompany = New System.Windows.Forms.Label()
    Me.LabelProduct = New System.Windows.Forms.Label()
    Me.LabelVersion = New System.Windows.Forms.Label()
    Me.LabelCopyright = New System.Windows.Forms.Label()
    Me.LabelWarning = New System.Windows.Forms.Label()
    Me.ButtonOK = New System.Windows.Forms.Button()
    Me.TimerMain = New System.Windows.Forms.Timer(Me.components)
    CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'PictureBox
    '
    Me.PictureBox.Image = CType(resources.GetObject("PictureBox.Image"), System.Drawing.Image)
    Me.PictureBox.Location = New System.Drawing.Point(8, 8)
    Me.PictureBox.Name = "PictureBox"
    Me.PictureBox.Size = New System.Drawing.Size(224, 272)
    Me.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
    Me.PictureBox.TabIndex = 0
    Me.PictureBox.TabStop = False
    '
    'LabelCompany
    '
    Me.LabelCompany.Font = New System.Drawing.Font("Tahoma", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelCompany.Location = New System.Drawing.Point(232, 8)
    Me.LabelCompany.Name = "LabelCompany"
    Me.LabelCompany.Size = New System.Drawing.Size(384, 32)
    Me.LabelCompany.TabIndex = 1
    Me.LabelCompany.Text = "Adaptive Incorporated"
    Me.LabelCompany.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'LabelProduct
    '
    Me.LabelProduct.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelProduct.Location = New System.Drawing.Point(248, 48)
    Me.LabelProduct.Name = "LabelProduct"
    Me.LabelProduct.Size = New System.Drawing.Size(368, 28)
    Me.LabelProduct.TabIndex = 2
    Me.LabelProduct.Text = "Check Weigh System"
    Me.LabelProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'LabelVersion
    '
    Me.LabelVersion.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelVersion.Location = New System.Drawing.Point(252, 80)
    Me.LabelVersion.Name = "LabelVersion"
    Me.LabelVersion.Size = New System.Drawing.Size(364, 24)
    Me.LabelVersion.TabIndex = 3
    Me.LabelVersion.Text = "Version X Build Y"
    Me.LabelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'LabelCopyright
    '
    Me.LabelCopyright.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelCopyright.Location = New System.Drawing.Point(248, 120)
    Me.LabelCopyright.Name = "LabelCopyright"
    Me.LabelCopyright.Size = New System.Drawing.Size(368, 23)
    Me.LabelCopyright.TabIndex = 4
    Me.LabelCopyright.Text = "Copyright © 2001-2017 Adaptive Incorporated. All rights reserved."
    Me.LabelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    '
    'LabelWarning
    '
    Me.LabelWarning.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelWarning.Location = New System.Drawing.Point(248, 160)
    Me.LabelWarning.Name = "LabelWarning"
    Me.LabelWarning.Size = New System.Drawing.Size(368, 88)
    Me.LabelWarning.TabIndex = 5
    Me.LabelWarning.Text = resources.GetString("LabelWarning.Text")
    '
    'ButtonOK
    '
    Me.ButtonOK.BackColor = System.Drawing.Color.Khaki
    Me.ButtonOK.Location = New System.Drawing.Point(520, 252)
    Me.ButtonOK.Name = "ButtonOK"
    Me.ButtonOK.Size = New System.Drawing.Size(96, 24)
    Me.ButtonOK.TabIndex = 6
    Me.ButtonOK.Text = "OK"
    Me.ButtonOK.UseVisualStyleBackColor = False
    '
    'TimerMain
    '
    Me.TimerMain.Interval = 1000
    '
    'FormSplash
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.BackColor = System.Drawing.Color.LightGoldenrodYellow
    Me.ClientSize = New System.Drawing.Size(622, 282)
    Me.ControlBox = False
    Me.Controls.Add(Me.ButtonOK)
    Me.Controls.Add(Me.LabelWarning)
    Me.Controls.Add(Me.LabelCopyright)
    Me.Controls.Add(Me.LabelVersion)
    Me.Controls.Add(Me.LabelProduct)
    Me.Controls.Add(Me.LabelCompany)
    Me.Controls.Add(Me.PictureBox)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "FormSplash"
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.TopMost = True
    CType(Me.PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

#End Region

End Class
