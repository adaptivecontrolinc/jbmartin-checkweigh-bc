Public Class FormNewDyeDrop
  Inherits System.Windows.Forms.Form

  Private Sub Setup()
    SetLanguage()
  End Sub

  Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
    'If Main.Balance.Tared Then 
    Me.Hide()
  End Sub

#Region "Language "
  Private LabelMessageText As String

  Private Sub SetLanguage()
    LabelMessageText = "This recipe has more than one dye drop. Please follow the instructions below: " & vbCrLf & vbCrLf & "  1. Remove all containers from all balances " & vbCrLf & "  2. Place a new empty container on the balance " & vbCrLf & "  3. Press Tare or Zero on the balance " & vbCrLf & "  4. Press OK to continue"
    LabelMessage.Text = LabelMessageText

  End Sub
#End Region

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call
    Setup()
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
  Friend WithEvents ImageList32 As System.Windows.Forms.ImageList
  Friend WithEvents PictureBoxMessage As System.Windows.Forms.PictureBox
  Friend WithEvents LabelMessage As System.Windows.Forms.Label
  Friend WithEvents ButtonOK As System.Windows.Forms.Button
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormNewDyeDrop))
    Me.PictureBoxMessage = New System.Windows.Forms.PictureBox()
    Me.LabelMessage = New System.Windows.Forms.Label()
    Me.ButtonOK = New System.Windows.Forms.Button()
    Me.ImageList32 = New System.Windows.Forms.ImageList(Me.components)
    CType(Me.PictureBoxMessage, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'PictureBoxMessage
    '
    Me.PictureBoxMessage.Image = CType(resources.GetObject("PictureBoxMessage.Image"), System.Drawing.Image)
    Me.PictureBoxMessage.Location = New System.Drawing.Point(8, 8)
    Me.PictureBoxMessage.Name = "PictureBoxMessage"
    Me.PictureBoxMessage.Size = New System.Drawing.Size(32, 32)
    Me.PictureBoxMessage.TabIndex = 0
    Me.PictureBoxMessage.TabStop = False
    '
    'LabelMessage
    '
    Me.LabelMessage.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.LabelMessage.Location = New System.Drawing.Point(56, 16)
    Me.LabelMessage.Name = "LabelMessage"
    Me.LabelMessage.Size = New System.Drawing.Size(336, 112)
    Me.LabelMessage.TabIndex = 1
    Me.LabelMessage.Text = "Message"
    '
    'ButtonOK
    '
    Me.ButtonOK.BackColor = System.Drawing.Color.Silver
    Me.ButtonOK.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.ButtonOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
    Me.ButtonOK.Location = New System.Drawing.Point(120, 136)
    Me.ButtonOK.Name = "ButtonOK"
    Me.ButtonOK.Size = New System.Drawing.Size(152, 32)
    Me.ButtonOK.TabIndex = 7
    Me.ButtonOK.TabStop = False
    Me.ButtonOK.Text = "OK"
    Me.ButtonOK.UseVisualStyleBackColor = False
    '
    'ImageList32
    '
    Me.ImageList32.ImageStream = CType(resources.GetObject("ImageList32.ImageStream"), System.Windows.Forms.ImageListStreamer)
    Me.ImageList32.TransparentColor = System.Drawing.Color.Transparent
    Me.ImageList32.Images.SetKeyName(0, "")
    '
    'FormNewDyeDrop
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(394, 182)
    Me.ControlBox = False
    Me.Controls.Add(Me.ButtonOK)
    Me.Controls.Add(Me.LabelMessage)
    Me.Controls.Add(Me.PictureBoxMessage)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
    Me.HelpButton = True
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "FormNewDyeDrop"
    Me.ShowInTaskbar = False
    Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "New Dye Drop"
    CType(Me.PictureBoxMessage, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

#End Region

End Class
