<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CalForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CalForm))
        Me.Button1 = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.txtRef = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NumDips = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtClientRef = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkStriker = New System.Windows.Forms.CheckBox()
        Me.txtAddInfo = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtFullVol = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtDipHeight = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtIncrements = New System.Windows.Forms.TextBox()
        Me.txtMarkedVolumes = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtSecondLine = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.txtWefco = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        CType(Me.NumDips, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.DarkGray
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'btnSubmit
        '
        Me.btnSubmit.BackColor = System.Drawing.Color.DarkGray
        resources.ApplyResources(Me.btnSubmit, "btnSubmit")
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.UseVisualStyleBackColor = False
        '
        'txtRef
        '
        Me.txtRef.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtRef, "txtRef")
        Me.txtRef.Name = "txtRef"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'NumDips
        '
        Me.NumDips.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.NumDips, "NumDips")
        Me.NumDips.Name = "NumDips"
        Me.NumDips.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'txtClientRef
        '
        Me.txtClientRef.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtClientRef, "txtClientRef")
        Me.txtClientRef.Name = "txtClientRef"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'chkStriker
        '
        resources.ApplyResources(Me.chkStriker, "chkStriker")
        Me.chkStriker.Name = "chkStriker"
        Me.chkStriker.UseVisualStyleBackColor = True
        '
        'txtAddInfo
        '
        Me.txtAddInfo.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtAddInfo, "txtAddInfo")
        Me.txtAddInfo.Name = "txtAddInfo"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'txtFullVol
        '
        Me.txtFullVol.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtFullVol, "txtFullVol")
        Me.txtFullVol.Name = "txtFullVol"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'txtDipHeight
        '
        Me.txtDipHeight.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtDipHeight, "txtDipHeight")
        Me.txtDipHeight.Name = "txtDipHeight"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'txtIncrements
        '
        Me.txtIncrements.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtIncrements, "txtIncrements")
        Me.txtIncrements.Name = "txtIncrements"
        '
        'txtMarkedVolumes
        '
        Me.txtMarkedVolumes.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtMarkedVolumes, "txtMarkedVolumes")
        Me.txtMarkedVolumes.Name = "txtMarkedVolumes"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'txtSecondLine
        '
        Me.txtSecondLine.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtSecondLine, "txtSecondLine")
        Me.txtSecondLine.Name = "txtSecondLine"
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'CheckBox1
        '
        resources.ApplyResources(Me.CheckBox1, "CheckBox1")
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'txtWefco
        '
        Me.txtWefco.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtWefco, "txtWefco")
        Me.txtWefco.Name = "txtWefco"
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'CalForm
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.txtWefco)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.txtSecondLine)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtMarkedVolumes)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtIncrements)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtDipHeight)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtFullVol)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtAddInfo)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.chkStriker)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtClientRef)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.NumDips)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtRef)
        Me.Controls.Add(Me.btnSubmit)
        Me.Controls.Add(Me.Button1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CalForm"
        CType(Me.NumDips, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnSubmit As System.Windows.Forms.Button
    Friend WithEvents txtRef As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents NumDips As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents txtClientRef As Windows.Forms.TextBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents chkStriker As Windows.Forms.CheckBox
    Friend WithEvents txtAddInfo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Public WithEvents txtFullVol As System.Windows.Forms.TextBox
    Public WithEvents Label7 As System.Windows.Forms.Label
    Public WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents txtDipHeight As System.Windows.Forms.TextBox
    Public WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Public WithEvents txtIncrements As System.Windows.Forms.TextBox
    Public WithEvents txtMarkedVolumes As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtSecondLine As Windows.Forms.TextBox
    Friend WithEvents Label11 As Windows.Forms.Label
    Friend WithEvents CheckBox1 As Windows.Forms.CheckBox
    Public WithEvents txtWefco As Windows.Forms.TextBox
    Friend WithEvents Label12 As Windows.Forms.Label
    Friend WithEvents Label13 As Windows.Forms.Label
End Class
