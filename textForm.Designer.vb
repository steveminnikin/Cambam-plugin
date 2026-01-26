Namespace CamBamPlugin

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class textForm
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
        Me.txtFirstVertical = New System.Windows.Forms.TextBox()
        Me.txtSecondVertical = New System.Windows.Forms.TextBox()
        Me.lblOurRef = New System.Windows.Forms.Label()
        Me.txtOurRef = New System.Windows.Forms.TextBox()
        Me.chkRef = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtClientRef = New System.Windows.Forms.TextBox()
        Me.lblTheirRef = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFullVolHeight = New System.Windows.Forms.TextBox()
        Me.lblFullVolume = New System.Windows.Forms.Label()
        Me.txtTankLetter = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtTankNumber = New System.Windows.Forms.TextBox()
        Me.chkTank = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(260, 280)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(145, 28)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "&Generate Dipstick"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtFirstVertical
        '
        Me.txtFirstVertical.Location = New System.Drawing.Point(157, 34)
        Me.txtFirstVertical.Name = "txtFirstVertical"
        Me.txtFirstVertical.Size = New System.Drawing.Size(200, 20)
        Me.txtFirstVertical.TabIndex = 8
        '
        'txtSecondVertical
        '
        Me.txtSecondVertical.Location = New System.Drawing.Point(157, 60)
        Me.txtSecondVertical.Name = "txtSecondVertical"
        Me.txtSecondVertical.Size = New System.Drawing.Size(200, 20)
        Me.txtSecondVertical.TabIndex = 9
        '
        'lblOurRef
        '
        Me.lblOurRef.AutoSize = True
        Me.lblOurRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblOurRef.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblOurRef.Location = New System.Drawing.Point(20, 58)
        Me.lblOurRef.Name = "lblOurRef"
        Me.lblOurRef.Size = New System.Drawing.Size(95, 16)
        Me.lblOurRef.TabIndex = 5
        Me.lblOurRef.Text = "Our Reference"
        '
        'txtOurRef
        '
        Me.txtOurRef.BackColor = System.Drawing.Color.White
        Me.txtOurRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtOurRef.Location = New System.Drawing.Point(169, 55)
        Me.txtOurRef.Name = "txtOurRef"
        Me.txtOurRef.Size = New System.Drawing.Size(90, 22)
        Me.txtOurRef.TabIndex = 2
        '
        'chkRef
        '
        Me.chkRef.AutoSize = True
        Me.chkRef.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkRef.Location = New System.Drawing.Point(270, 93)
        Me.chkRef.Name = "chkRef"
        Me.chkRef.Size = New System.Drawing.Size(120, 17)
        Me.chkRef.TabIndex = 4
        Me.chkRef.Text = "Show &REF Marker"
        Me.chkRef.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label4.Location = New System.Drawing.Point(268, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 16)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = ""
        Me.Label4.Visible = False
        '
        'txtClientRef
        '
        Me.txtClientRef.BackColor = System.Drawing.Color.White
        Me.txtClientRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtClientRef.Location = New System.Drawing.Point(169, 90)
        Me.txtClientRef.Name = "txtClientRef"
        Me.txtClientRef.Size = New System.Drawing.Size(90, 22)
        Me.txtClientRef.TabIndex = 3
        '
        'lblTheirRef
        '
        Me.lblTheirRef.AutoSize = True
        Me.lblTheirRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblTheirRef.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblTheirRef.Location = New System.Drawing.Point(20, 93)
        Me.lblTheirRef.Name = "lblTheirRef"
        Me.lblTheirRef.Size = New System.Drawing.Size(117, 16)
        Me.lblTheirRef.TabIndex = 9
        Me.lblTheirRef.Text = "Client Reference:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label11.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label11.Location = New System.Drawing.Point(34, 60)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(83, 16)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "Second Line"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label6.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label6.Location = New System.Drawing.Point(34, 34)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(157, 16)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "First Line:"
        '
        'txtFullVolHeight
        '
        Me.txtFullVolHeight.BackColor = System.Drawing.Color.White
        Me.txtFullVolHeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtFullVolHeight.Location = New System.Drawing.Point(169, 20)
        Me.txtFullVolHeight.Name = "txtFullVolHeight"
        Me.txtFullVolHeight.Size = New System.Drawing.Size(90, 22)
        Me.txtFullVolHeight.TabIndex = 1
        '
        'lblFullVolume
        '
        Me.lblFullVolume.AutoSize = True
        Me.lblFullVolume.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblFullVolume.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblFullVolume.Location = New System.Drawing.Point(20, 23)
        Me.lblFullVolume.Name = "lblFullVolume"
        Me.lblFullVolume.Size = New System.Drawing.Size(120, 16)
        Me.lblFullVolume.TabIndex = 24
        Me.lblFullVolume.Text = "Full Volume Height"
        '
        'txtTankLetter
        '
        Me.txtTankLetter.BackColor = System.Drawing.Color.White
        Me.txtTankLetter.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtTankLetter.Location = New System.Drawing.Point(264, 125)
        Me.txtTankLetter.Name = "txtTankLetter"
        Me.txtTankLetter.Size = New System.Drawing.Size(29, 22)
        Me.txtTankLetter.TabIndex = 5
        Me.txtTankLetter.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtFirstVertical)
        Me.GroupBox1.Controls.Add(Me.txtSecondVertical)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 165)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(395, 100)
        Me.GroupBox1.TabIndex = 26
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Vertical Text (Optional)"
        '
        'txtTankNumber
        '
        Me.txtTankNumber.BackColor = System.Drawing.Color.White
        Me.txtTankNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtTankNumber.Location = New System.Drawing.Point(169, 125)
        Me.txtTankNumber.Name = "txtTankNumber"
        Me.txtTankNumber.Size = New System.Drawing.Size(50, 22)
        Me.txtTankNumber.TabIndex = 7
        '
        'chkTank
        '
        Me.chkTank.AutoSize = True
        Me.chkTank.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.chkTank.Location = New System.Drawing.Point(20, 128)
        Me.chkTank.Name = "chkTank"
        Me.chkTank.Size = New System.Drawing.Size(145, 20)
        Me.chkTank.TabIndex = 6
        Me.chkTank.Text = "Include &Tank Number"
        Me.chkTank.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(20, 128)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(150, 16)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = ""
        Me.Label1.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic)
        Me.Label2.ForeColor = System.Drawing.Color.Gray
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label2.Location = New System.Drawing.Point(262, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "* Required field"
        '
        'textForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(420, 320)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtTankNumber)
        Me.Controls.Add(Me.chkTank)
        Me.Controls.Add(Me.txtFullVolHeight)
        Me.Controls.Add(Me.lblFullVolume)
        Me.Controls.Add(Me.chkRef)
        Me.Controls.Add(Me.txtClientRef)
        Me.Controls.Add(Me.lblTheirRef)
        Me.Controls.Add(Me.lblOurRef)
        Me.Controls.Add(Me.txtOurRef)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "textForm"
        Me.Text = "Text-Only Dipstick Generator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents txtFirstVertical As Windows.Forms.TextBox
    Friend WithEvents txtSecondVertical As Windows.Forms.TextBox
    Friend WithEvents lblOurRef As Windows.Forms.Label
    Friend WithEvents txtOurRef As Windows.Forms.TextBox
    Friend WithEvents chkRef As Windows.Forms.CheckBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents txtClientRef As Windows.Forms.TextBox
    Friend WithEvents lblTheirRef As Windows.Forms.Label
    Friend WithEvents Label11 As Windows.Forms.Label
    Friend WithEvents Label6 As Windows.Forms.Label
    Public WithEvents txtFullVolHeight As Windows.Forms.TextBox
    Public WithEvents lblFullVolume As Windows.Forms.Label
    Friend WithEvents txtTankLetter As Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents txtTankNumber As Windows.Forms.TextBox
    Friend WithEvents chkTank As Windows.Forms.CheckBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Public WithEvents Label2 As Windows.Forms.Label
End Class

End Namespace
