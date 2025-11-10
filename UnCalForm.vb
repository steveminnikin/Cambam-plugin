Imports System.Windows.Forms
Imports System.Math
Imports System.Drawing
Imports CamBamPlugin.CamBamPlugin.MyPlugin

Namespace CamBamPlugin

Public Class UnCalForm
    Private _dipHeight As Single
    Private _increments As Single
    Private _markedIncrements As Single
    Private commonDetails As CommonDetails
    Private isMarkedIncrement As Boolean
    Private toolTip As New ToolTip()

    Public Sub New()
        InitializeComponent()
        InitializeTooltips()
        ApplyVisualHierarchy()
    End Sub

    Private Sub InitializeTooltips()
        toolTip.AutoPopDelay = 5000
        toolTip.InitialDelay = 500
        toolTip.ReshowDelay = 200
        toolTip.ShowAlways = True

        ' Set tooltips for complex fields
        toolTip.SetToolTip(txtMarkedIncrements, "Display measurement numbers only at these intervals (e.g., every 10mm, 50mm, etc.)")
        toolTip.SetToolTip(chkHalfIncs, "Add shorter tick marks between main measurements for easier reading")
        toolTip.SetToolTip(CboUnits, "Select measurement unit system - all inputs will use this unit")
        toolTip.SetToolTip(txtHeight, "Total height of the dipstick in selected units")
        toolTip.SetToolTip(NumDips, "Number of identical dipsticks to generate side-by-side (1 or 2)")
        toolTip.SetToolTip(txtIncs, "Spacing between measurement marks in selected units")
        toolTip.SetToolTip(txtAddInfo, "Optional text displayed vertically on the dipstick (rotated 90°)")
        toolTip.SetToolTip(txtSecondLine, "Optional second line of vertical text on the dipstick")
    End Sub

    Private Sub ApplyVisualHierarchy()
        ' Make required field labels bold and add asterisk
        If Label3 IsNot Nothing Then
            Label3.Font = New Font(Label3.Font, FontStyle.Bold)
            If Not Label3.Text.Contains("*") Then Label3.Text = Label3.Text.Replace(":", ": *")
        End If
        If txtTop IsNot Nothing Then
            txtTop.Font = New Font(txtTop.Font, FontStyle.Bold)
            If Not txtTop.Text.Contains("*") Then txtTop.Text = txtTop.Text.Replace(":", ": *")
        End If

        ' Make buttons bold
        btnSubmit.Font = New Font(btnSubmit.Font, FontStyle.Bold)

        ' Initialize unit display with default selection (Millimetres)
        CdoUnits_SelectedIndexChanged(Nothing, EventArgs.Empty)

        ' Add note about required fields
        Dim requiredNote As New Label()
        requiredNote.Text = "* Required field"
        requiredNote.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Italic)
        requiredNote.ForeColor = Color.FromArgb(100, 100, 100)
        requiredNote.AutoSize = True
        requiredNote.Location = New Point(12, 318)
        Me.Controls.Add(requiredNote)
    End Sub

    Private Property Increments As Single
        Get
            Return _increments
        End Get
        Set(value As Single)
            Select Case CboUnits.SelectedIndex
                Case 0
                    _increments = value
                Case 1
                    _increments = value * DipstickConstants.MM_PER_CM
                Case 2
                    _increments = value * DipstickConstants.MM_PER_INCH
            End Select
        End Set
    End Property
    Private Property markedIncrement As Single
        Get
            Return _markedIncrements
        End Get
        Set(value As Single)
            Select Case CboUnits.SelectedIndex
                Case 0
                    _markedIncrements = value
                Case 1
                    _markedIncrements = value * DipstickConstants.MM_PER_CM
                Case 2
                    _markedIncrements = value * DipstickConstants.MM_PER_INCH
            End Select
        End Set
    End Property
    Private Property DipHeight() As Single
        Get
            Return _dipHeight
        End Get
        Set(value As Single)
            Select Case CboUnits.SelectedIndex
                Case 0
                    _dipHeight = value
                Case 1
                    _dipHeight = value * DipstickConstants.MM_PER_CM
                Case 2
                    _dipHeight = value * DipstickConstants.MM_PER_INCH
            End Select
        End Set
    End Property

    Private Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        ' Validate marked increments if provided
        If Not ValidateMarkedIncrements() Then
            Return
        End If

        ' Show progress indication
        Me.Cursor = Cursors.WaitCursor
        btnSubmit.Enabled = False
        btnSubmit.Text = "Generating..."
        Application.DoEvents()

        Try
        'clear the current dipstick from the UI and create a fresh template
        myUI.FileNew(True, True, True)
        commonDetails = New CommonDetails(, Me)
        Dim myDoc As New CADFile
        Dim myLayer As Layer
        Dim myPart As CAMPart
        Dim cboUnits As String
        Dim markedIncrement As Single

        cboUnits = GetUnitString(Me.CboUnits.SelectedIndex)
        markedIncrement = If(String.IsNullOrWhiteSpace(Me.txtMarkedIncrements.Text), 0, CSng(Me.txtMarkedIncrements.Text))

        myDoc = commonDetails.CreateCADFile()
        myLayer = commonDetails.CreateLayer(myDoc, commonDetails.Model.Ref)
        myPart = commonDetails.CreatePart(myDoc, commonDetails.Model.Ref)

        DrawLinesAndNumbers(cboUnits, markedIncrement)
        commonDetails.WriteUnits(cboUnits, commonDetails.Model.Height, commonDetails.Model.GetCopyOffset())
        If Not String.IsNullOrWhiteSpace(commonDetails.Model.Ref) Then commonDetails.WriteRef(commonDetails.Model.Ref, commonDetails.Model.Height, commonDetails.Model.GetCopyOffset())
        commonDetails.WriteClientRef(commonDetails.Model.Height, commonDetails.Model.GetCopyOffset(), commonDetails.Model.ClientRef, commonDetails.Model.IncludeStriker)

        ' Calculate vertical text position based on which elements are present
        If Not String.IsNullOrWhiteSpace(commonDetails.FirstLineText.Text) Then
            Dim verticalTextYOffset As Single
            If commonDetails.Model.WefcoVolume > 0 Then
                ' Wefco volume is highest element (at 153), position vertical text above it
                verticalTextYOffset = DipstickConstants.VERTICAL_TEXT_WITH_WEFCO_Y_OFFSET
            ElseIf Not String.IsNullOrWhiteSpace(commonDetails.Model.ClientRef) Then
                ' ClientRef present but no Wefco
                verticalTextYOffset = DipstickConstants.VERTICAL_TEXT_WITH_CLIENTREF_Y_OFFSET
            Else
                ' No ClientRef or Wefco
                verticalTextYOffset = DipstickConstants.VERTICAL_TEXT_BASE_Y_OFFSET
            End If
            commonDetails.WriteVerticalInfo(commonDetails.FirstLineText, commonDetails.SecondLineText, commonDetails.Model.Height + verticalTextYOffset)
        End If

        myUI.ActiveView.RefreshView()
        commonDetails = Nothing
        Me.Hide()
        Catch ex As Exception
            MessageBox.Show("Error generating dipstick: " & ex.Message, "Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Restore UI state
            Me.Cursor = Cursors.Default
            btnSubmit.Enabled = True
            btnSubmit.Text = "&Generate Dipstick"
        End Try
    End Sub
    Private Sub DrawLinesAndNumbers(cboUnits As String, markedIncrement As Single)
        Dim l As Single
        Dim xOffset As Integer = commonDetails.Model.GetCopyOffset()

        Do While l + Increments < DipHeight
            l += Increments
            If chkHalfIncs.Checked Then
                DrawHalfIncs(l - (Increments / 2), xOffset)
            End If
            DrawLine(l, xOffset)
            If isMultipleOfMarkedInterval(UnitConv(l), markedIncrement) Or l = DipHeight Or l = Increments Then
                isMarkedIncrement = True
                WriteNumber(l, UnitConv(l), xOffset)
            Else
                isMarkedIncrement = False
            End If
        Loop
        'adds top line for inches
        'If CboUnits.SelectedIndex = 2 Then
        If chkHalfIncs.Checked Then
            DrawHalfIncs(DipHeight - (Increments / 2), xOffset)
        End If

        DrawLine(DipHeight, xOffset)
        WriteNumber(DipHeight, UnitConv(DipHeight), xOffset)
    End Sub
    Private Sub DrawLine(l As Single, x As Single)
        Dim myPoly As New Polyline()

        myPoly.Add(x, l, 0)
        myPoly.Add(x + DipstickConstants.LINE_LENGTH, l, 0)
        'add it to active drawing
        myUI.ActiveView.CADFile.Add(myPoly)
    End Sub
    Private Sub WriteNumber(l As Single, n As Single, x As Single)
        Dim myCamText As New MText()
        Dim NoPos As Single = l + DipstickConstants.UNCALIBRATED_NUMBER_Y_OFFSET

        myCamText.Text = n
        myCamText.Font = DipstickConstants.FONT_NAME
        myCamText.Height = DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString()
        myCamText.Location = DipstickConstants.NUMBER_X_OFFSET_MEDIUM + x & "," & NoPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText)


    End Sub
    Private Function isMultipleOfMarkedInterval(inc As Single, markedIncrement As Single) As Boolean
        Return (Round(inc, 1) Mod markedIncrement) = 0
    End Function
    Private Sub DrawHalfIncs(incr As Single, x As Single)
        Dim myHalfIncs As New Polyline()
        If isMarkedIncrement Then
            Select Case CboUnits.SelectedIndex
                Case 0
                    myHalfIncs.Add(x + DipstickConstants.HALF_INC_OFFSET_MM, incr, 0)
                    myHalfIncs.Add(x + DipstickConstants.LINE_LENGTH, incr, 0)
                Case 1
                    myHalfIncs.Add(x + DipstickConstants.HALF_INC_OFFSET_CM, incr, 0)
                    myHalfIncs.Add(x + DipstickConstants.LINE_LENGTH, incr, 0)
                Case 2
                    myHalfIncs.Add(x + DipstickConstants.HALF_INC_OFFSET_INCH, incr, 0)
                    myHalfIncs.Add(x + DipstickConstants.LINE_LENGTH, incr, 0)

            End Select
        Else
            myHalfIncs.Add(x + DipstickConstants.HALF_INC_OFFSET_INCH, incr, 0)
            myHalfIncs.Add(x + DipstickConstants.LINE_LENGTH, incr, 0)
        End If
        myUI.ActiveView.CADFile.Add(myHalfIncs)
    End Sub
    Private Sub CdoUnits_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboUnits.SelectedIndexChanged
        Dim unitAbbrev As String = "mm"  ' Default to millimeters

        Select Case CboUnits.SelectedIndex
            Case 0
                unitAbbrev = "mm"
                Label3.Text = "Increment Size (mm):"
                txtTop.Text = "Dipstick Height (mm):"
                Label5.Text = "Display Numbers Every (mm):"
            Case 1
                unitAbbrev = "cm"
                Label3.Text = "Increment Size (cm):"
                txtTop.Text = "Dipstick Height (cm):"
                Label5.Text = "Display Numbers Every (cm):"
            Case 2
                unitAbbrev = "in"
                Label3.Text = "Increment Size (in):"
                txtTop.Text = "Dipstick Height (in):"
                Label5.Text = "Display Numbers Every (in):"
        End Select

        ' Update unit labels (keep for backwards compatibility)
        lblIncs.Text = unitAbbrev
        lblHeight.Text = unitAbbrev
        lblIntervals.Text = unitAbbrev
    End Sub

    Private Sub txtIncs_LostFocus(sender As Object, e As EventArgs) Handles txtIncs.TextChanged
        txtVal.Visible = False
        valInc.Visible = False
        btnSubmit.Enabled = True
        If String.IsNullOrWhiteSpace(txtIncs.Text) Then
            txtVal.Text = "You must enter a value in the Increments box"
            txtVal.Visible = True
            valInc.Visible = True
            btnSubmit.Enabled = False
        Else
            Try
                Increments() = CInt(txtIncs.Text)
            Catch ex As Exception
                txtVal.Text = "You must enter a number in the Increments box"
                txtVal.Visible = True
                valInc.Visible = True
                btnSubmit.Enabled = False
            End Try
        End If

    End Sub

    Private Sub txtHeight_LostFocus(sender As Object, e As EventArgs) Handles txtHeight.TextChanged
        txtVal.Visible = False
        ValHei.Visible = False
        btnSubmit.Enabled = True
        If String.IsNullOrWhiteSpace(txtHeight.Text) Then
            txtVal.Text = "You must enter a value in the Height box"
            txtVal.Visible = True
            ValHei.Visible = True
            btnSubmit.Enabled = False
        Else
            Try
                DipHeight = CSng(txtHeight.Text)
            Catch ex As Exception
                txtVal.Text = "You must enter a number in the Height box"
                txtVal.Visible = True
                ValHei.Visible = True
                btnSubmit.Enabled = False
            End Try

        End If

    End Sub

    Private Function GetUnitString(u As Integer) As String
        Select Case u
            Case 0
                Return "MMs"
            Case 1
                Return "CMs"
            Case Else
                Return "INCH"
        End Select
    End Function

    Private Function UnitConv(x As Single) As Single
        Select Case CboUnits.SelectedIndex
            Case 0
                Return x
            Case 1
                Return x / DipstickConstants.MM_PER_CM
            Case 2
                Return Round(x / DipstickConstants.MM_PER_INCH, 0)
            Case Else
                ' Default to millimeters if invalid selection
                Return x
        End Select
    End Function

    Private Function ValidateMarkedIncrements() As Boolean
        ' Marked increments is optional, but if provided must be valid
        If Not String.IsNullOrWhiteSpace(txtMarkedIncrements.Text) Then
            Dim markedInc As Single
            If Not Single.TryParse(txtMarkedIncrements.Text, markedInc) Then
                MessageBox.Show("Marked Increments must be a valid number.", _
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            ElseIf markedInc < 0 Then
                MessageBox.Show("Marked Increments cannot be negative.", _
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
        End If
        Return True
    End Function


End Class

End Namespace
