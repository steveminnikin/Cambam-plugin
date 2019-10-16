Imports System.Windows.Forms
Imports CamBamPlugin.MyPlugin
Imports System.Math
Imports CamBamPlugin.CommonDetails

Public Class UnCalForm
    Private _dipHeight As Single
    Private _increments As Single
    Private _markedIncrements As Single
    Private commonDetails As CommonDetails
    Private isMarkedIncrement As Boolean
    Private Property Increments As Single
        Get
            Return _increments
        End Get
        Set(value As Single)
            Select Case CboUnits.SelectedIndex
                Case 0
                    _increments = value
                Case 1
                    _increments = value * 10
                Case 2
                    _increments = value * 25.4
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
                    _markedIncrements = value * 10
                Case 2
                    _markedIncrements = value * 25.4
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
                    _dipHeight = value * 10
                Case 2
                    _dipHeight = value * 25.4
            End Select
        End Set
    End Property

    Private Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        'clear the current dipstick from the UI and create a fresh template
        myUI.FileNew(True, True, True)
        commonDetails = New CommonDetails(, Me)
        Dim myDoc As New CADFile
        Dim myLayer As Layer
        Dim myPart As CAMPart
        Dim cboUnits As String
        Dim markedIncrement As Single

        cboUnits = GetUnitString(Me.CboUnits.SelectedIndex)
        markedIncrement = Me.txtMarkedIncrements.Text
        'If chkLaser.Checked Then
        '    CommonDetails.Laser = True
        'End If

        myDoc = CreateCADFile()
        myLayer = CreateLayer(myDoc, Ref)
        myPart = CreatePart(myDoc, Ref)

        DrawLinesAndNumbers(cboUnits, markedIncrement)
        WriteUnits(cboUnits, DipHeight, CreateCopies(Copies))
        If Not Ref.Equals("") Then WriteRef(Ref, DipHeight, CreateCopies(Copies))
        WriteClientRef(DipHeight, CreateCopies(Copies), ClientRef, RefText)
        ' If Not FirstLineText.Text.Equals("") Then WriteVerticalInfo(FirstLineText, SecondLineText, DipHeight + If(Not ClientRef = "", 148, 105))

        myUI.ActiveView.RefreshView()
        commonDetails = Nothing
        Me.Hide()
    End Sub
    Private Sub DrawLinesAndNumbers(cboUnits As String, markedIncrement As Single)

        Dim l As Single

        Do While l + Increments < DipHeight
            l += Increments
            If chkHalfIncs.Checked Then
                DrawHalfIncs(l - (Increments / 2), CreateCopies(Copies))
            End If
            DrawLine(l, CreateCopies(Copies))
            If isMultipleOfMarkedInterval(UnitConv(l), markedIncrement) Or l = DipHeight Or l = Increments Then
                isMarkedIncrement = True
                WriteNumber(l, UnitConv(l), CreateCopies(Copies))
            Else
                isMarkedIncrement = False
            End If
        Loop
        'adds top line for inches
        'If CboUnits.SelectedIndex = 2 Then
        If chkHalfIncs.Checked Then
            DrawHalfIncs(DipHeight - (Increments / 2), CreateCopies(Copies))
        End If

        DrawLine(DipHeight, CreateCopies(Copies))
        WriteNumber(DipHeight, UnitConv(DipHeight), CreateCopies(Copies))
    End Sub
    Private Sub DrawLine(l As Single, x As Single)
        Dim myPoly As New Polyline()

        myPoly.Add(x, l, 0)
        myPoly.Add(x + 20, l, 0)
        'add it to active drawing
        myUI.ActiveView.CADFile.Add(myPoly)
    End Sub
    Private Sub WriteNumber(l As Single, n As Single, x As Single)
        Dim myCamText As New MText()
        Dim NoPos As Single = l + 6.5

        myCamText.Text = n
        myCamText.Font = "1CamBam_Stick_3"
        myCamText.Height = "5.5"
        myCamText.Location = 0.5 + x & "," & NoPos & ",0"
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
                    myHalfIncs.Add(x + 15, incr, 0)
                    myHalfIncs.Add(x + 20, incr, 0)
                Case 1
                    myHalfIncs.Add(x + 12, incr, 0)
                    myHalfIncs.Add(x + 20, incr, 0)
                Case 2
                    myHalfIncs.Add(x + 10, incr, 0)
                    myHalfIncs.Add(x + 20, incr, 0)

            End Select
        Else
            myHalfIncs.Add(x + 10, incr, 0)
            myHalfIncs.Add(x + 20, incr, 0)
        End If
        myUI.ActiveView.CADFile.Add(myHalfIncs)
    End Sub
    Private Sub CdoUnits_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboUnits.SelectedIndexChanged
        Select Case CboUnits.SelectedIndex
            Case 0
                lblIncs.Text = "MMs"
                lblHeight.Text = "MMs"
                lblIntervals.Text = "MMs"
            Case 1
                lblIncs.Text = "CMs"
                lblHeight.Text = "CMs"
                lblIntervals.Text = "CMs"
            Case 2
                lblIncs.Text = "In"
                lblHeight.Text = "In"
                lblIntervals.Text = "In"
        End Select
    End Sub

    Private Sub txtIncs_LostFocus(sender As Object, e As EventArgs) Handles txtIncs.TextChanged
        txtVal.Visible = False
        valInc.Visible = False
        btnSubmit.Enabled = True
        If txtIncs.Text = "" Then
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
        If txtHeight.Text = "" Then
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
                Return x / 10
            Case 2
                Return Round(x / 25.4, 0)
        End Select
    End Function


End Class
