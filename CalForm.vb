Imports System.Windows.Forms
Imports System.Math
Imports System.Drawing
Imports CamBamPlugin.CamBamPlugin.MyPlugin

Namespace CamBamPlugin

    Public Class CalForm
        Private myFile As String
        Private isFileSelected As Boolean
        Private isRegIncrements As Boolean
        Private commonDetails As CommonDetails
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
            toolTip.SetToolTip(txtMarkedVolumes, "Display volume numbers only at these intervals (e.g., 100 = show 100L, 200L, 300L, etc.)")
            toolTip.SetToolTip(txtWefco, "Enter Wefco volume in thousands (e.g., enter 5 for 5000 litres)")
            toolTip.SetToolTip(chkRegIncs, "Use evenly-spaced increments regardless of calibration data from JSON file")
            toolTip.SetToolTip(txtFullVol, "Total tank capacity in litres")
            toolTip.SetToolTip(txtDipHeight, "Maximum dipstick measurement height in millimeters")
            toolTip.SetToolTip(txtIncrements, "Spacing between measurement marks in millimeters")
            toolTip.SetToolTip(txtAddInfo, "Optional text displayed vertically on the dipstick (rotated 90°)")
            toolTip.SetToolTip(txtSecondLine, "Optional second line of vertical text on the dipstick")
            toolTip.SetToolTip(Button1, "Select a JSON calibration file containing volume/height data")
        End Sub

        Private Sub ApplyVisualHierarchy()
            ' Make required field labels bold and add asterisk
            If Label7 IsNot Nothing Then
                Label7.Font = New Font(Label7.Font, FontStyle.Bold)
                If Not Label7.Text.EndsWith("*") Then Label7.Text &= " *"
            End If
            If Label5 IsNot Nothing Then
                Label5.Font = New Font(Label5.Font, FontStyle.Bold)
                If Not Label5.Text.EndsWith("*") Then Label5.Text &= " *"
            End If
            If Label8 IsNot Nothing Then
                Label8.Font = New Font(Label8.Font, FontStyle.Bold)
                If Not Label8.Text.EndsWith("*") Then Label8.Text &= " *"
            End If

            ' Make buttons bold
            btnSubmit.Font = New Font(btnSubmit.Font, FontStyle.Bold)
            Button1.Font = New Font(Button1.Font, FontStyle.Bold)

            ' Add note about required fields at top of form
            Dim requiredNote As New Label()
            requiredNote.Text = "* Required field"
            requiredNote.Font = New Font("Microsoft Sans Serif", 8, FontStyle.Italic)
            requiredNote.ForeColor = Color.FromArgb(100, 100, 100)
            requiredNote.AutoSize = True
            requiredNote.Location = New Point(12, 260)
            Me.Controls.Add(requiredNote)
        End Sub

        Private Sub BtnSubmit_Click(sender As Object, E As EventArgs) Handles btnSubmit.Click

            If Not isFileSelected Then
                MessageBox.Show("You must select a file!", "File Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Validate all numeric inputs before processing
            If Not ValidateNumericInputs() Then
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
                commonDetails = New CommonDetails(Me)
                Dim myDoc As New CADFile
                Dim myLayer As Layer
                Dim myPart As CAMPart
                Dim myList As SortedList(Of String, String)

                isRegIncrements = chkRegIncs.Checked
                myDoc = commonDetails.CreateCADFile()
                myLayer = commonDetails.CreateLayer(myDoc, commonDetails.Model.Ref)
                myPart = commonDetails.CreatePart(myDoc, commonDetails.Model.Ref)

                myList = CreateVolumeHeightPairsFromFile(myFile, commonDetails.Model.Ref)
                DrawLinesAndNumbers(myList, commonDetails.Model.Ref)
                commonDetails.WriteUnits("LITRE", commonDetails.Model.Height, 0)
                If Not String.IsNullOrWhiteSpace(commonDetails.Model.Ref) Then commonDetails.WriteRef(commonDetails.Model.Ref, commonDetails.Model.Height, 0)
                WriteSWC(commonDetails.Model.Height, 0, "LITRE", Round(commonDetails.Model.FullVolume * 0.97))
                commonDetails.WriteClientRef(commonDetails.Model.Height, 0, commonDetails.Model.ClientRef, commonDetails.Model.IncludeStriker)
                If commonDetails.Model.WefcoVolume > 0 Then WriteWefcoRef(commonDetails.Model.WefcoVolume.ToString(), commonDetails.Model.Height, 0)

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
                Me.ResetText()
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

        Private Function CreateVolumeHeightPairsFromFile(myFile As String, ref As String) As SortedList(Of String, String)
            Dim myList As New SortedList(Of String, String)

            Try
                Dim jsonParser As New JSONCalibrationParser()
                myList = jsonParser.ReadVolumeHeightPairs(myFile, isRegIncrements)
            Catch ex As IO.FileNotFoundException
                MessageBox.Show("File not found: " & myFile, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As IO.IOException
                MessageBox.Show("Error reading file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As FormatException
                MessageBox.Show("Error parsing JSON file: " & ex.Message, "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MessageBox.Show("Unexpected error parsing JSON file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            Return myList
        End Function
        Private Sub DrawLinesAndNumbers(myList As SortedList(Of String, String), ref As String)
            Dim xOffset As Integer = 0
            For Each i As KeyValuePair(Of String, String) In myList
                Drawline(i.Key, xOffset)
                If Not isRegIncrements Then
                    If IsMultipleOfMarkedInterval(i.Value) Or i.Value = commonDetails.Model.FullVolume.ToString() Or i.Value = commonDetails.Model.Increments.ToString() Then
                        WriteNumber(i, xOffset)
                    End If
                Else
                    WriteNumber(i, xOffset)
                End If
            Next
        End Sub

        Private Sub Drawline(l As Single, x As Single)
            'sets the source file for incs
            Dim myPoly As New Polyline()
            myPoly.Add(x, l, 0)
            myPoly.Add(x + DipstickConstants.LINE_LENGTH, l, 0)
            'add it to active drawing
            myUI.ActiveView.CADFile.Add(myPoly)
        End Sub

        Private Sub WriteNumber(i As KeyValuePair(Of String, String), x As Single)
            Dim NoPos As Single = i.Key + DipstickConstants.CALIBRATED_NUMBER_Y_OFFSET
            'add some text
            'adjusts the size of the volume text so htat it always fits on to the dipstick
            Dim myCamText As New MText With {
            .Text = i.Value,
            .Font = DipstickConstants.FONT_NAME,
            .Height = IIf(i.Value > DipstickConstants.LARGE_NUMBER_THRESHOLD, DipstickConstants.LARGE_NUMBER_TEXT_HEIGHT.ToString(), DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString()),
            .Location = DipstickConstants.NUMBER_X_OFFSET_MEDIUM + x & "," & NoPos & ",0"
        }
            myUI.ActiveView.CADFile.Add(myCamText)
        End Sub

        Private Sub WriteSWC(y As Single, x As Single, units As String, vol As Single)
            Dim swcCamText As New MText()
            Dim volCamText As New MText()
            Dim unitsCamText As New MText()
            Dim centreText As Single
            'swc text
            swcCamText.Text = "SWC"
            swcCamText.Font = DipstickConstants.FONT_NAME
            swcCamText.Height = DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString()
            swcCamText.Location = DipstickConstants.SWC_TEXT_X_OFFSET + x & "," & y + DipstickConstants.SWC_Y_OFFSET & ",0"
            myUI.ActiveView.CADFile.Add(swcCamText)
            'vol text
            volCamText.Text = vol
            volCamText.Font = DipstickConstants.FONT_NAME
            volCamText.Height = IIf(vol > DipstickConstants.LARGE_NUMBER_THRESHOLD, DipstickConstants.LARGE_NUMBER_TEXT_HEIGHT.ToString(), DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString())
            centreText = IIf(vol > DipstickConstants.MEDIUM_NUMBER_THRESHOLD, DipstickConstants.NUMBER_X_OFFSET_MEDIUM, DipstickConstants.NUMBER_X_OFFSET_SMALL)
            volCamText.Location = centreText + x & "," & y + DipstickConstants.SWC_VOLUME_Y_OFFSET & ",0"
            myUI.ActiveView.CADFile.Add(volCamText)
            'units text
            unitsCamText.Text = units
            unitsCamText.Font = DipstickConstants.FONT_NAME
            unitsCamText.Height = DipstickConstants.LARGE_NUMBER_TEXT_HEIGHT.ToString()
            unitsCamText.Location = DipstickConstants.UNITS_TEXT_X_OFFSET + x & "," & y + DipstickConstants.SWC_UNITS_Y_OFFSET & ",0"
            myUI.ActiveView.CADFile.Add(unitsCamText)

        End Sub
        Private Function IsMultipleOfMarkedInterval(inc As Single) As Boolean
            Return (inc Mod commonDetails.Model.MarkedVolIncrement) = 0
        End Function


        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

            ' Configure file dialog to only show JSON files
            OpenFileDialog1.Filter = "JSON Calibration Files (*.json)|*.json|All Files (*.*)|*.*"
            OpenFileDialog1.FilterIndex = 1
            OpenFileDialog1.Title = "Select JSON Calibration File"

            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                myFile = OpenFileDialog1.FileName

                ' Validate the selected file
                If Not ValidateSelectedFile(myFile) Then
                    isFileSelected = False
                    Return
                End If

                Try
                    isFileSelected = True

                    ' Extract metadata from JSON file and populate form fields
                    Dim parser As New JSONCalibrationParser()
                    Dim jsonData As CalibrationData = parser.Parse(myFile, False)

                    txtFullVol.Text = jsonData.FullVolume.ToString()
                    txtIncrements.Text = jsonData.Increments.ToString()
                    txtDipHeight.Text = jsonData.TopHeight.ToString()
                    txtMarkedVolumes.Text = AddSuggestedMarkedIncrements(jsonData.Increments)

                Catch ex As Exception
                    MessageBox.Show("Error parsing JSON calibration file: " & ex.Message, _
                        "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    isFileSelected = False
                End Try
            End If

        End Sub

    Private Function ValidateSelectedFile(filePath As String) As Boolean
        ' Check if file path is empty
        If String.IsNullOrWhiteSpace(filePath) Then
            MessageBox.Show("No file was selected.", "File Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' Check if file exists
        If Not System.IO.File.Exists(filePath) Then
            MessageBox.Show("The selected file does not exist:" & vbCrLf & filePath, _
                "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        ' Check if file has .json extension
        Dim extension As String = System.IO.Path.GetExtension(filePath).ToLower()
        If extension <> ".json" Then
            MessageBox.Show("The selected file is not a JSON calibration file." & vbCrLf & vbCrLf & _
                "Please select a file with .json extension." & vbCrLf & _
                "Selected: " & extension, _
                "Invalid File Type", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' Check if file is readable
        Try
            Using reader As New System.IO.StreamReader(filePath)
                ' Try to read first line to verify file is accessible
                Dim firstLine As String = reader.ReadLine()
                If firstLine Is Nothing Then
                    MessageBox.Show("The selected CSV file is empty.", _
                        "Empty File", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If
            End Using
        Catch ex As System.IO.IOException
            MessageBox.Show("Cannot read the selected file. It may be in use by another program." & vbCrLf & vbCrLf & _
                "Error: " & ex.Message, _
                "File Access Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Catch ex As UnauthorizedAccessException
            MessageBox.Show("Access denied. You don't have permission to read this file." & vbCrLf & vbCrLf & _
                "File: " & filePath, _
                "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

        ' All validations passed
        Return True
    End Function

    Private Function ValidateNumericInputs() As Boolean
        Dim errorMessages As New System.Text.StringBuilder()

        ' Validate Full Volume
        If String.IsNullOrWhiteSpace(txtFullVol.Text) Then
            errorMessages.AppendLine("• Full Volume is required")
        Else
            Dim fullVol As Integer
            If Not Integer.TryParse(txtFullVol.Text, fullVol) Then
                errorMessages.AppendLine("• Full Volume must be a valid integer")
            ElseIf fullVol <= 0 Then
                errorMessages.AppendLine("• Full Volume must be greater than 0")
            End If
        End If

        ' Validate Increments
        If String.IsNullOrWhiteSpace(txtIncrements.Text) Then
            errorMessages.AppendLine("• Increments is required")
        Else
            Dim increments As Integer
            If Not Integer.TryParse(txtIncrements.Text, increments) Then
                errorMessages.AppendLine("• Increments must be a valid integer")
            ElseIf increments <= 0 Then
                errorMessages.AppendLine("• Increments must be greater than 0")
            End If
        End If

        ' Validate Dipstick Height
        If String.IsNullOrWhiteSpace(txtDipHeight.Text) Then
            errorMessages.AppendLine("• Dipstick Height is required")
        Else
            Dim dipHeight As Single
            If Not Single.TryParse(txtDipHeight.Text, dipHeight) Then
                errorMessages.AppendLine("• Dipstick Height must be a valid number")
            ElseIf dipHeight <= 0 Then
                errorMessages.AppendLine("• Dipstick Height must be greater than 0")
            End If
        End If

        ' Validate Marked Volumes (optional field, but if provided must be valid)
        If Not String.IsNullOrWhiteSpace(txtMarkedVolumes.Text) Then
            Dim markedVol As Integer
            If Not Integer.TryParse(txtMarkedVolumes.Text, markedVol) Then
                errorMessages.AppendLine("• Marked Volumes must be a valid integer")
            ElseIf markedVol < 0 Then
                errorMessages.AppendLine("• Marked Volumes cannot be negative")
            End If
        End If

        ' Validate Wefco Volume (optional field, but if provided must be valid)
        If Not String.IsNullOrWhiteSpace(txtWefco.Text) Then
            Dim wefcoVol As Integer
            If Not Integer.TryParse(txtWefco.Text, wefcoVol) Then
                errorMessages.AppendLine("• Wefco Volume must be a valid integer")
            ElseIf wefcoVol < 0 Then
                errorMessages.AppendLine("• Wefco Volume cannot be negative")
            End If
        End If

        ' If there are any validation errors, show them and return False
        If errorMessages.Length > 0 Then
            MessageBox.Show("Please correct the following errors:" & vbCrLf & vbCrLf & errorMessages.ToString(), _
                "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

    Private Function AddSuggestedMarkedIncrements(increments As Integer) As Integer
        Select Case increments
            Case 25
                Return 100
            Case 50
                Return 200
            Case 100
                Return 500
            Case 200, 250
                Return 1000
            Case 400
                Return 800
            Case 500
                Return 2000
            Case 750
                Return 3000
            Case 1000
                Return 5000
            Case Else
                Return 0
        End Select
    End Function

    Private Sub WriteWefcoRef(volume As String, ypos As Single, x As Integer)
        Dim volCamText As New MText()
        Dim unitsCamText As New MText()
        Dim centreText As Single

        volCamText.Text = volume
        volCamText.Font = DipstickConstants.FONT_NAME
        volCamText.Height = IIf(volume > DipstickConstants.LARGE_NUMBER_THRESHOLD, DipstickConstants.LARGE_NUMBER_TEXT_HEIGHT.ToString(), DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString())
        centreText = IIf(volume > DipstickConstants.MEDIUM_NUMBER_THRESHOLD, DipstickConstants.NUMBER_X_OFFSET_MEDIUM, DipstickConstants.NUMBER_X_OFFSET_SMALL)
        volCamText.Location = centreText + x & "," & ypos + DipstickConstants.WEFCO_VOLUME_Y_OFFSET & ",0"
        myUI.ActiveView.CADFile.Add(volCamText)
        'units text
        unitsCamText.Text = "LITRE"
        unitsCamText.Font = DipstickConstants.FONT_NAME
        unitsCamText.Height = DipstickConstants.LARGE_NUMBER_TEXT_HEIGHT.ToString()
        unitsCamText.Location = DipstickConstants.UNITS_TEXT_X_OFFSET + x & "," & ypos + DipstickConstants.WEFCO_UNITS_Y_OFFSET & ",0"
        myUI.ActiveView.CADFile.Add(unitsCamText)
    End Sub


End Class

End Namespace