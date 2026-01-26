Namespace CamBamPlugin

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CalForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.txtRef = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtClientRef = New System.Windows.Forms.TextBox()
        Me.chkStriker = New System.Windows.Forms.CheckBox()
        Me.txtAddInfo = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFullVol = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtDipHeight = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtIncrements = New System.Windows.Forms.TextBox()
        Me.txtMarkedVolumes = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtSecondLine = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.chkRegIncs = New System.Windows.Forms.CheckBox()
        Me.txtWefco = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Button1.Location = New System.Drawing.Point(340, 18)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(105, 26)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "&Browse..."
        Me.Button1.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'btnSubmit
        '
        Me.btnSubmit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnSubmit.Location = New System.Drawing.Point(300, 330)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(145, 28)
        Me.btnSubmit.TabIndex = 15
        Me.btnSubmit.Text = "&Generate Dipstick"
        Me.btnSubmit.UseVisualStyleBackColor = True
        '
        'txtRef
        '
        Me.txtRef.BackColor = System.Drawing.Color.White
        Me.txtRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtRef.Location = New System.Drawing.Point(140, 20)
        Me.txtRef.Name = "txtRef"
        Me.txtRef.Size = New System.Drawing.Size(90, 22)
        Me.txtRef.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(20, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Our Reference:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label3.Location = New System.Drawing.Point(20, 175)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Client Reference:"
        '
        'txtClientRef
        '
        Me.txtClientRef.BackColor = System.Drawing.Color.White
        Me.txtClientRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtClientRef.Location = New System.Drawing.Point(140, 172)
        Me.txtClientRef.Name = "txtClientRef"
        Me.txtClientRef.Size = New System.Drawing.Size(90, 22)
        Me.txtClientRef.TabIndex = 10
        '
        'chkStriker
        '
        Me.chkStriker.AutoSize = True
        Me.chkStriker.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.chkStriker.Location = New System.Drawing.Point(250, 174)
        Me.chkStriker.Name = "chkStriker"
        Me.chkStriker.Size = New System.Drawing.Size(135, 20)
        Me.chkStriker.TabIndex = 11
        Me.chkStriker.Text = "Show &REF Marker"
        Me.chkStriker.UseVisualStyleBackColor = True
        '
        'txtAddInfo
        '
        Me.txtAddInfo.BackColor = System.Drawing.Color.White
        Me.txtAddInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtAddInfo.Location = New System.Drawing.Point(100, 25)
        Me.txtAddInfo.Name = "txtAddInfo"
        Me.txtAddInfo.Size = New System.Drawing.Size(200, 22)
        Me.txtAddInfo.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label6.Location = New System.Drawing.Point(15, 28)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(65, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "First Line:"
        '
        'txtFullVol
        '
        Me.txtFullVol.BackColor = System.Drawing.Color.White
        Me.txtFullVol.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtFullVol.Location = New System.Drawing.Point(140, 58)
        Me.txtFullVol.Name = "txtFullVol"
        Me.txtFullVol.Size = New System.Drawing.Size(80, 22)
        Me.txtFullVol.TabIndex = 2
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label7.Location = New System.Drawing.Point(20, 61)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(113, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Full Volume (L):"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label5.Location = New System.Drawing.Point(240, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(130, 16)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Dipstick Height (mm):"
        '
        'txtDipHeight
        '
        Me.txtDipHeight.BackColor = System.Drawing.Color.White
        Me.txtDipHeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtDipHeight.Location = New System.Drawing.Point(375, 58)
        Me.txtDipHeight.Name = "txtDipHeight"
        Me.txtDipHeight.Size = New System.Drawing.Size(70, 22)
        Me.txtDipHeight.TabIndex = 3
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label8.Location = New System.Drawing.Point(20, 99)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(108, 16)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Increments (mm):"
        '
        'txtIncrements
        '
        Me.txtIncrements.BackColor = System.Drawing.Color.White
        Me.txtIncrements.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtIncrements.Location = New System.Drawing.Point(140, 96)
        Me.txtIncrements.Name = "txtIncrements"
        Me.txtIncrements.Size = New System.Drawing.Size(80, 22)
        Me.txtIncrements.TabIndex = 4
        '
        'txtMarkedVolumes
        '
        Me.txtMarkedVolumes.BackColor = System.Drawing.Color.White
        Me.txtMarkedVolumes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtMarkedVolumes.Location = New System.Drawing.Point(140, 134)
        Me.txtMarkedVolumes.Name = "txtMarkedVolumes"
        Me.txtMarkedVolumes.Size = New System.Drawing.Size(80, 22)
        Me.txtMarkedVolumes.TabIndex = 6
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label10.Location = New System.Drawing.Point(20, 137)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(116, 16)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Mark Numbers (L):"
        '
        'txtSecondLine
        '
        Me.txtSecondLine.BackColor = System.Drawing.Color.White
        Me.txtSecondLine.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtSecondLine.Location = New System.Drawing.Point(100, 53)
        Me.txtSecondLine.Name = "txtSecondLine"
        Me.txtSecondLine.Size = New System.Drawing.Size(200, 22)
        Me.txtSecondLine.TabIndex = 13
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label11.Location = New System.Drawing.Point(15, 56)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 16)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Second Line:"
        '
        'chkRegIncs
        '
        Me.chkRegIncs.AutoSize = True
        Me.chkRegIncs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.chkRegIncs.Location = New System.Drawing.Point(240, 98)
        Me.chkRegIncs.Name = "chkRegIncs"
        Me.chkRegIncs.Size = New System.Drawing.Size(145, 20)
        Me.chkRegIncs.TabIndex = 5
        Me.chkRegIncs.Text = "Regular &Increments"
        Me.chkRegIncs.UseVisualStyleBackColor = True
        '
        'txtWefco
        '
        Me.txtWefco.BackColor = System.Drawing.Color.White
        Me.txtWefco.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtWefco.Location = New System.Drawing.Point(375, 134)
        Me.txtWefco.Name = "txtWefco"
        Me.txtWefco.Size = New System.Drawing.Size(70, 22)
        Me.txtWefco.TabIndex = 7
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label12.Location = New System.Drawing.Point(240, 137)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(130, 16)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Wefco Volume (000s):"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtAddInfo)
        Me.GroupBox1.Controls.Add(Me.txtSecondLine)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 210)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(433, 105)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Vertical Text (Optional)"
        '
        'CalForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 370)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtWefco)
        Me.Controls.Add(Me.chkRegIncs)
        Me.Controls.Add(Me.txtMarkedVolumes)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtIncrements)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtDipHeight)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtFullVol)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.chkStriker)
        Me.Controls.Add(Me.txtClientRef)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtRef)
        Me.Controls.Add(Me.btnSubmit)
        Me.Controls.Add(Me.Button1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CalForm"
        Me.Text = "Calibrated Dipstick Generator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnSubmit As System.Windows.Forms.Button
    Friend WithEvents txtRef As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtClientRef As System.Windows.Forms.TextBox
    Friend WithEvents chkStriker As System.Windows.Forms.CheckBox
    Friend WithEvents txtAddInfo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents txtFullVol As System.Windows.Forms.TextBox
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents txtDipHeight As System.Windows.Forms.TextBox
    Public WithEvents Label8 As System.Windows.Forms.Label
    Public WithEvents txtIncrements As System.Windows.Forms.TextBox
    Public WithEvents txtMarkedVolumes As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSecondLine As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents chkRegIncs As System.Windows.Forms.CheckBox
    Public WithEvents txtWefco As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class

End Namespace
