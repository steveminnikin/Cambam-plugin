Imports CamBam
Imports CamBam.CAD
Imports CamBam.UI
Imports CamBam.Util
Imports CamBam.ThisApplication
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports CamBamPlugin.MyPlugin
Imports System.Math

Public Class UnCalForm
    Private inc As Single
    Private hei As Single
    Private markedInc
    Private Property Increments As Single
        Get
            Return inc
        End Get
        Set(value As Single)
            Select Case CboUnits.SelectedIndex
                Case 0
                    inc = value
                Case 1
                    inc = value * 10
                Case 2
                    inc = value * 25.4
            End Select
        End Set
    End Property
    Private Property markedIncrement As Single
        Get
            Return markedInc
        End Get
        Set(value As Single)
            Select Case CboUnits.SelectedIndex
                Case 0
                    markedInc = value
                Case 1
                    markedInc = value * 10
                Case 2
                    markedInc = value * 25.4
            End Select
        End Set
    End Property
    Private Property DipHeight() As Single
        Get
            Return hei
        End Get
        Set(value As Single)
            Select Case CboUnits.SelectedIndex
                Case 0
                    hei = value
                Case 1
                    hei = value * 10
                Case 2
                    hei = value * 25.4
            End Select
        End Set
    End Property

    Private Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        CreateLayer(ref)
        'CreatePart()
        Dim l As Single
        Dim unit As String = GetUnitString(CboUnits)
        Do While l + Increments < DipHeight
            l += Increments
            If chkHalfIncs.Checked Then
                DrawHalfIncs(l - (Increments / 2), CreateCopies(Number))
            End If
            DrawLine(l, CreateCopies(Number))
            If isMultipleOfMarkedInterval(l) Or l = DipHeight Or l = Increments Then
                isMarkedIncrement = True
                WriteNumber(l, UnitConv(l), CreateCopies(Number))
            Else
                isMarkedIncrement = False
            End If
        Loop
        'adds top line for inches
        'If CboUnits.SelectedIndex = 2 Then
        If chkHalfIncs.Checked Then
            DrawHalfIncs(DipHeight - (Increments / 2), CreateCopies(Number))
        End If

        DrawLine(DipHeight, CreateCopies(Number))
        WriteNumber(DipHeight, UnitConv(DipHeight), CreateCopies(Number))
        'End If
        WriteUnits(unit, DipHeight, CreateCopies(Number))
        WriteRef(ref, DipHeight, CreateCopies(Number))
        'myUI.ActiveView.SelectAllVisibleGeometry()
        CreatePart()
        'CAMUtils.GenerateGCodeOutput(myUI.ActiveView)
        myUI.ActiveView.RefreshView()
        Me.Visible = False
    End Sub
    Private Sub DrawLine(l As Single, x As Single)
        Dim myPoly As New Polyline()

        myPoly.Add(x, l, 0)
        myPoly.Add(x + 20, l, 0)
        'add it to active drawing
        myDoc.Add(myPoly, myLayer)
    End Sub
    Private Sub WriteNumber(l As Single, n As Single, x As Single)
        Dim myCamText As New MText()
        Dim NoPos As Single = l + 6.5

        myCamText.Text = n
        myCamText.Font = "1CamBam_Stick_3"
        myCamText.Height = "5.5"
        myCamText.Location = 0.5 + x & "," & NoPos & ",0"
        myDoc.Add(myCamText, myLayer)


    End Sub
    Private Function isMultipleOfMarkedInterval(inc As Single) As Boolean
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
        myDoc.Add(myHalfIncs, myLayer)
    End Sub
    Private Sub CdoUnits_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboUnits.SelectedIndexChanged
        Select Case CboUnits.SelectedIndex
            Case 0
                lblIncs.Text = "MMs"
                lblHeight.Text = "MMs"
                lblintervals.Text = "MMs"
            Case 1
                lblIncs.Text = "CMs"
                lblHeight.Text = "CMs"
                lblintervals.Text = "CMs"
            Case 2
                lblIncs.Text = "In"
                lblHeight.Text = "In"
                lblintervals.Text = "In"
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

    Private Function GetUnitString(u As ComboBox) As String
        Select Case u.SelectedIndex
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

    Public Sub NumDips_ValueChanged(sender As Object, e As EventArgs) Handles NumDips.ValueChanged
        Number = NumDips.Value
    End Sub

    Private Sub txtRef_TextChanged(sender As Object, e As EventArgs) Handles txtRef.TextChanged
        ref = txtRef.Text
    End Sub

    Private Sub chkHalfIncs_CheckedChanged(sender As Object, e As EventArgs) Handles chkHalfIncs.CheckedChanged

    End Sub

    Private Sub txtMarkedIncrements_LostFocus(sender As Object, e As EventArgs) Handles txtMarkedIncrements.LostFocus
        markedIncrement = txtMarkedIncrements.Text
    End Sub

End Class
