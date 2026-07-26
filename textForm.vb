Imports CamBamPlugin.CamBamPlugin.MyPlugin
Imports System.Windows.Forms
Imports System.Drawing

Namespace CamBamPlugin

Public Class textForm
    Private commonDetails As CommonDetails
    Property AddTank As Boolean
    Property TankNumber As String
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
        toolTip.SetToolTip(txtFullVolHeight, "Height where full volume marking appears (in millimeters) - Required")
        toolTip.SetToolTip(chkRef, "Add 'REF' text marker on the dipstick")
        toolTip.SetToolTip(chkTank, "Include tank identification number on the dipstick")
        toolTip.SetToolTip(txtFirstVertical, "Text displayed vertically along the dipstick edge")
        toolTip.SetToolTip(txtSecondVertical, "Second line of text displayed vertically along the dipstick edge")
        toolTip.SetToolTip(txtTankNumber, "Tank identification number to display")
    End Sub

    Private Sub ApplyVisualHierarchy()
        ' Make required field labels bold and add asterisk
        If lblFullVolume IsNot Nothing Then
            lblFullVolume.Font = New Font(lblFullVolume.Font, FontStyle.Bold)
            If Not lblFullVolume.Text.Contains("*") Then lblFullVolume.Text &= " *"
        End If

        ' Make GroupBox header bold
        If GroupBox1 IsNot Nothing Then GroupBox1.Font = New Font(GroupBox1.Font, FontStyle.Bold)

        ' Make buttons bold
        Button1.Font = New Font(Button1.Font, FontStyle.Bold)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrWhiteSpace(txtFullVolHeight.Text) Then
            MsgBox("You must enter a FV Height!")
        Else
            ' Show progress indication
            Me.Cursor = Cursors.WaitCursor
            Button1.Enabled = False
            Button1.Text = "Generating..."
            Application.DoEvents()

            Try
            Dim myDoc As New CADFile
            Dim myLayer As Layer
            Dim myPart As CAMPart

            commonDetails = New CommonDetails(,, Me)
            AddTank = chkTank.Checked
            TankNumber = txtTankNumber.Text


            myUI.FileNew(True, True, True)

            myDoc = commonDetails.CreateCADFile()
            myLayer = commonDetails.CreateLayer(myDoc, commonDetails.Model.Ref)
            myPart = commonDetails.CreatePart(myDoc, commonDetails.Model.Ref)

            commonDetails.WriteRef(commonDetails.Model.Ref, commonDetails.Model.Height, 0)
            commonDetails.WriteClientRef(commonDetails.Model.Height, 0, commonDetails.Model.ClientRef, commonDetails.Model.IncludeStriker)
            If AddTank Then WriteTank(TankNumber, commonDetails.Model.Height, 0)

            ' Calculate vertical text position based on which elements are present
            If Not String.IsNullOrWhiteSpace(commonDetails.FirstLineText.Text) Then
                Dim verticalTextYOffset As Single
                If Not String.IsNullOrWhiteSpace(commonDetails.Model.ClientRef) Then
                    ' ClientRef present (text-only form doesn't support WefcoVolume)
                    verticalTextYOffset = DipstickConstants.VERTICAL_TEXT_WITH_CLIENTREF_Y_OFFSET
                Else
                    ' No ClientRef
                    verticalTextYOffset = DipstickConstants.VERTICAL_TEXT_BASE_Y_OFFSET
                End If
                commonDetails.WriteVerticalInfo(commonDetails.FirstLineText, commonDetails.SecondLineText, commonDetails.Model.Height + verticalTextYOffset)
            End If




            myUI.ActiveView.RefreshView()
            Me.ResetText()
            commonDetails = Nothing
            Me.Close()
            Catch ex As Exception
                MessageBox.Show("Error generating dipstick: " & ex.Message, "Generation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                ' Restore UI state (skipped when the form closed itself after success)
                If Not Me.IsDisposed Then
                    Me.Cursor = Cursors.Default
                    Button1.Enabled = True
                    Button1.Text = "&Generate Dipstick"
                End If
            End Try
        End If
    End Sub

    Private Shared Sub WriteTank(tankNumber As String, DipHeight As Single, x As Single)
        Dim refYPos As Single = DipHeight + DipstickConstants.TEXT_ONLY_REF_Y_OFFSET
        Dim tankText As New MText()
        Dim numberText As New MText()

        tankText.Text = "TANK"
        tankText.Font = DipstickConstants.FONT_NAME
        tankText.Height = DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString()
        tankText.Location = DipstickConstants.NUMBER_X_OFFSET_MEDIUM + x & "," & refYPos & ",0"

        numberText.Text = tankNumber
        numberText.Font = DipstickConstants.FONT_NAME
        numberText.Height = DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString()
        numberText.Location = DipstickConstants.TANK_NUMBER_X_OFFSET + x & "," & refYPos - DipstickConstants.TANK_NUMBER_Y_OFFSET & ",0"

        myUI.ActiveView.CADFile.Add(tankText)
        myUI.ActiveView.CADFile.Add(numberText)


    End Sub


End Class

End Namespace