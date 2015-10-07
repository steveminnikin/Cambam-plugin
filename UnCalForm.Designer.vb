<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnCalForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnCalForm))
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CboUnits = New System.Windows.Forms.ComboBox()
        Me.NumDips = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtIncs = New System.Windows.Forms.TextBox()
        Me.lblIncs = New System.Windows.Forms.Label()
        Me.txtVal = New System.Windows.Forms.Label()
        Me.txtTop = New System.Windows.Forms.Label()
        Me.txtHeight = New System.Windows.Forms.TextBox()
        Me.lblHeight = New System.Windows.Forms.Label()
        Me.valInc = New System.Windows.Forms.Label()
        Me.ValHei = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtRef = New System.Windows.Forms.TextBox()
        Me.chkHalfIncs = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtMarkedIncrements = New System.Windows.Forms.TextBox()
        Me.lblIntervals = New System.Windows.Forms.Label()
        CType(Me.NumDips, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnSubmit
        '
        Me.btnSubmit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSubmit.Location = New System.Drawing.Point(249, 293)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(75, 23)
        Me.btnSubmit.TabIndex = 7
        Me.btnSubmit.Text = "Submit"
        Me.btnSubmit.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(35, 227)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Dipstick Position"
        '
        'CboUnits
        '
        Me.CboUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboUnits.ForeColor = System.Drawing.SystemColors.WindowText
        Me.CboUnits.FormattingEnabled = True
        Me.CboUnits.Items.AddRange(New Object() {"Millimetres", "Centimetres", "Inches"})
        Me.CboUnits.Location = New System.Drawing.Point(204, 71)
        Me.CboUnits.Name = "CboUnits"
        Me.CboUnits.Size = New System.Drawing.Size(97, 24)
        Me.CboUnits.TabIndex = 1
        Me.CboUnits.Text = "Millimetres"
        '
        'NumDips
        '
        Me.NumDips.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NumDips.Location = New System.Drawing.Point(203, 220)
        Me.NumDips.Name = "NumDips"
        Me.NumDips.Size = New System.Drawing.Size(50, 22)
        Me.NumDips.TabIndex = 5
        Me.NumDips.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(35, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Units"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(35, 106)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Increments"
        '
        'txtIncs
        '
        Me.txtIncs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIncs.Location = New System.Drawing.Point(203, 103)
        Me.txtIncs.Name = "txtIncs"
        Me.txtIncs.Size = New System.Drawing.Size(62, 22)
        Me.txtIncs.TabIndex = 2
        '
        'lblIncs
        '
        Me.lblIncs.AutoSize = True
        Me.lblIncs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIncs.Location = New System.Drawing.Point(264, 109)
        Me.lblIncs.Name = "lblIncs"
        Me.lblIncs.Size = New System.Drawing.Size(37, 16)
        Me.lblIncs.TabIndex = 7
        Me.lblIncs.Text = "MMs"
        '
        'txtVal
        '
        Me.txtVal.AutoSize = True
        Me.txtVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVal.ForeColor = System.Drawing.Color.Red
        Me.txtVal.Location = New System.Drawing.Point(33, 290)
        Me.txtVal.Name = "txtVal"
        Me.txtVal.Size = New System.Drawing.Size(40, 16)
        Me.txtVal.TabIndex = 8
        Me.txtVal.Text = "txtVal"
        Me.txtVal.Visible = False
        '
        'txtTop
        '
        Me.txtTop.AutoSize = True
        Me.txtTop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTop.Location = New System.Drawing.Point(35, 190)
        Me.txtTop.Name = "txtTop"
        Me.txtTop.Size = New System.Drawing.Size(81, 16)
        Me.txtTop.TabIndex = 9
        Me.txtTop.Text = "Tank Height"
        '
        'txtHeight
        '
        Me.txtHeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeight.Location = New System.Drawing.Point(203, 182)
        Me.txtHeight.Name = "txtHeight"
        Me.txtHeight.Size = New System.Drawing.Size(62, 22)
        Me.txtHeight.TabIndex = 4
        '
        'lblHeight
        '
        Me.lblHeight.AutoSize = True
        Me.lblHeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeight.Location = New System.Drawing.Point(265, 190)
        Me.lblHeight.Name = "lblHeight"
        Me.lblHeight.Size = New System.Drawing.Size(37, 16)
        Me.lblHeight.TabIndex = 11
        Me.lblHeight.Text = "MMs"
        '
        'valInc
        '
        Me.valInc.AutoSize = True
        Me.valInc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.valInc.ForeColor = System.Drawing.Color.Red
        Me.valInc.Location = New System.Drawing.Point(298, 103)
        Me.valInc.Name = "valInc"
        Me.valInc.Size = New System.Drawing.Size(13, 16)
        Me.valInc.TabIndex = 12
        Me.valInc.Text = "*"
        Me.valInc.Visible = False
        '
        'ValHei
        '
        Me.ValHei.AutoSize = True
        Me.ValHei.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ValHei.ForeColor = System.Drawing.Color.Red
        Me.ValHei.Location = New System.Drawing.Point(298, 185)
        Me.ValHei.Name = "ValHei"
        Me.ValHei.Size = New System.Drawing.Size(13, 16)
        Me.ValHei.TabIndex = 13
        Me.ValHei.Text = "*"
        Me.ValHei.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(35, 39)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 16)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Our Reference"
        '
        'txtRef
        '
        Me.txtRef.Location = New System.Drawing.Point(203, 36)
        Me.txtRef.Name = "txtRef"
        Me.txtRef.Size = New System.Drawing.Size(81, 20)
        Me.txtRef.TabIndex = 0
        '
        'chkHalfIncs
        '
        Me.chkHalfIncs.AutoSize = True
        Me.chkHalfIncs.Location = New System.Drawing.Point(204, 263)
        Me.chkHalfIncs.Name = "chkHalfIncs"
        Me.chkHalfIncs.Size = New System.Drawing.Size(122, 17)
        Me.chkHalfIncs.TabIndex = 6
        Me.chkHalfIncs.Text = "Add Half Increments"
        Me.chkHalfIncs.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(35, 149)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(92, 16)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Marked Every"
        '
        'txtMarkedIncrements
        '
        Me.txtMarkedIncrements.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMarkedIncrements.Location = New System.Drawing.Point(204, 146)
        Me.txtMarkedIncrements.Name = "txtMarkedIncrements"
        Me.txtMarkedIncrements.Size = New System.Drawing.Size(62, 22)
        Me.txtMarkedIncrements.TabIndex = 3
        '
        'lblIntervals
        '
        Me.lblIntervals.AutoSize = True
        Me.lblIntervals.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIntervals.Location = New System.Drawing.Point(265, 152)
        Me.lblIntervals.Name = "lblIntervals"
        Me.lblIntervals.Size = New System.Drawing.Size(37, 16)
        Me.lblIntervals.TabIndex = 18
        Me.lblIntervals.Text = "MMs"
        '
        'UnCalForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(349, 381)
        Me.Controls.Add(Me.lblIntervals)
        Me.Controls.Add(Me.txtMarkedIncrements)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.chkHalfIncs)
        Me.Controls.Add(Me.txtRef)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ValHei)
        Me.Controls.Add(Me.valInc)
        Me.Controls.Add(Me.lblHeight)
        Me.Controls.Add(Me.txtHeight)
        Me.Controls.Add(Me.txtTop)
        Me.Controls.Add(Me.txtVal)
        Me.Controls.Add(Me.lblIncs)
        Me.Controls.Add(Me.txtIncs)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.NumDips)
        Me.Controls.Add(Me.CboUnits)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSubmit)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "UnCalForm"
        Me.Text = "Uncalibrated Dipstick"
        CType(Me.NumDips, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSubmit As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CboUnits As System.Windows.Forms.ComboBox
    Friend WithEvents NumDips As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtIncs As System.Windows.Forms.TextBox
    Friend WithEvents lblIncs As System.Windows.Forms.Label
    Friend WithEvents txtVal As System.Windows.Forms.Label
    Friend WithEvents txtTop As System.Windows.Forms.Label
    Friend WithEvents txtHeight As System.Windows.Forms.TextBox
    Friend WithEvents lblHeight As System.Windows.Forms.Label
    Friend WithEvents valInc As System.Windows.Forms.Label
    Friend WithEvents ValHei As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtRef As System.Windows.Forms.TextBox
    Friend WithEvents chkHalfIncs As Windows.Forms.CheckBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents txtMarkedIncrements As Windows.Forms.TextBox
    Friend WithEvents lblIntervals As System.Windows.Forms.Label
End Class
