Imports System.Windows.Forms
Imports CamBamPlugin.MyPlugin
Imports System.Math
Imports CamBamPlugin.CommonDetails

Public Class CalForm
    Private myFile As String
    Private isFileSelected As Boolean
    Private isRegIncrements As Boolean
    Private commonDetails As CommonDetails
    Private WefcoVol As String

    Private Sub BtnSubmit_Click(sender As Object, E As EventArgs) Handles btnSubmit.Click

        If isFileSelected Then
            'clear the current dipstick from the UI and create a fresh template
            myUI.FileNew(True, True, True)
            commonDetails = New CommonDetails(Me)
            Dim myDoc As New CADFile
            Dim myLayer As Layer
            Dim myPart As CAMPart
            Dim myList As SortedList(Of String, String)

            WefcoVol = txtWefco.Text & "000"
            isRegIncrements = chkRegIncs.Checked
            myDoc = CreateCADFile()
            myLayer = CreateLayer(myDoc, Ref)
            myPart = CreatePart(myDoc, Ref)

            myList = CreateVolumeHeightPairsFromFile(myFile, Ref)
            DrawLinesAndNumbers(myList, Ref)
            WriteUnits("LITRE", DipHeight, CreateCopies(Copies))
            If Not Ref.Equals("") Then WriteRef(Ref, DipHeight, CreateCopies(Copies))
            WriteSWC(DipHeight, CreateCopies(Copies), "LITRE", Round(FullVol * 0.97))
            WriteClientRef(DipHeight, CreateCopies(Copies), ClientRef, RefText)
            If Not WefcoVol.Equals("000") Then WriteWefcoRef(WefcoVol, DipHeight, CreateCopies(Copies))
            If Not FirstLineText.Text.Equals("") Then WriteVerticalInfo(FirstLineText, SecondLineText, DipHeight + If(Not ClientRef = "", 148, 105))

            myUI.ActiveView.RefreshView()
            Me.ResetText()
            commonDetails = Nothing
            Me.Hide()
        Else
            MessageBox.Show("You must select a file!")
        End If
    End Sub

    Private Function CreateVolumeHeightPairsFromFile(myFile As String, ref As String) As SortedList(Of String, String)
        Dim myList As New SortedList(Of String, String)
        Dim myElements As String()

        Try
            Using sR As New StreamReader(myFile)
                Do
                    myElements = sR.ReadLine.Split(",")
                    If isRegIncrements Then
                        myList.Add(myElements(1), myElements(0))
                    Else
                        myList.Add(myElements(0), myElements(1))
                    End If
                Loop While Not myList.Equals(Nothing)
            End Using
        Catch ex As Exception
            MessageBox.Show("End of File")
        End Try
        Return myList
    End Function
    Private Sub DrawLinesAndNumbers(myList As SortedList(Of String, String), ref As String)
        For Each i As KeyValuePair(Of String, String) In myList
            Drawline(i.Key, CreateCopies(CommonDetails.Copies))
            If Not isRegIncrements Then
                If IsMultipleOfMarkedInterval(i.Value) Or i.Value = FullVol Or i.Value = Increments Then
                    WriteNumber(i, CreateCopies(CommonDetails.Copies))
                End If
            Else
                WriteNumber(i, CreateCopies(CommonDetails.Copies))
            End If
        Next
    End Sub

    Private Sub Drawline(l As Single, x As Single)
        'sets the source file for incs
        Dim myPoly As New Polyline()
        myPoly.Add(x, l, 0)
        myPoly.Add(x + 20, l, 0)
        'add it to active drawing
        myUI.ActiveView.CADFile.Add(myPoly)
    End Sub

    Private Sub WriteNumber(i As KeyValuePair(Of String, String), x As Single)
        Dim NoPos As Single = i.Key + 7
        'add some text
        'adjusts the size of the volume text so htat it always fits on to the dipstick
        Dim myCamText As New MText With {
            .Text = i.Value,
            .Font = "1CamBam_Stick_3",
            .Height = IIf(i.Value > 99999, "5", "5.5"),
            .Location = 0.5 + x & "," & NoPos & ",0"
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
        swcCamText.Font = "1CamBam_Stick_3"
        swcCamText.Height = "5.5"
        swcCamText.Location = 3 + x & "," & y + 76 & ",0"
        myUI.ActiveView.CADFile.Add(swcCamText)
        'vol text
        volCamText.Text = vol
        volCamText.Font = "1CamBam_Stick_3"
        volCamText.Height = IIf(vol > 99999, "5", "5.5")
        centreText = IIf(vol > 9999, 0.5, 3)
        volCamText.Location = centreText + x & "," & y + 68 & ",0"
        myUI.ActiveView.CADFile.Add(volCamText)
        'units text
        unitsCamText.Text = units
        unitsCamText.Font = "1CamBam_Stick_3"
        unitsCamText.Height = "5"
        unitsCamText.Location = 1 + x & "," & y + 60 & ",0"
        myUI.ActiveView.CADFile.Add(unitsCamText)

    End Sub
    Private Function IsMultipleOfMarkedInterval(inc As Single) As Boolean
        Return (inc Mod MarkedVolIncrement) = 0
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click


        OpenFileDialog1.ShowDialog()
        myFile = OpenFileDialog1.FileName
        If Not myFile = "" Then
            isFileSelected = True
            txtFullVol.Text = TrimFullVolume(myFile)
            txtIncrements.Text = TrimIncrements(myFile)
            tankDetails = TrimTankDimensionsFromFileName(myFile)
            txtMarkedVolumes.Text = AddSuggestedMarkedIncrements(txtIncrements.Text)
            'txtFullVol.Text = TrimFullHeight(myFile)
        End If

    End Sub
    Private Function TrimFullVolume(myFile As String) As Integer
        Dim fullVolume As String
        Dim position As Integer

        position = myFile.IndexOf("FV ")
        fullVolume = myFile.Substring(position + 3)
        position = fullVolume.IndexOf("_INCS")
        fullVolume = fullVolume.Remove(position)

        Return fullVolume
    End Function
    Private Function TrimIncrements(myFile As String) As Integer
        Dim increments As String
        Dim incrementsPlusDetails As String
        Dim startPosition As Integer
        Dim endPosition As Integer

        startPosition = myFile.IndexOf("_INCS ")
        incrementsPlusDetails = myFile.Remove(0, startPosition + 5)

        endPosition = incrementsPlusDetails.IndexOf("_(")
        increments = incrementsPlusDetails.Remove(endPosition)

        Return increments
    End Function
    Function TrimTankDimensionsFromFileName(myFile As String) As String
        Dim tankDimensions As String
        Dim startPosition As Integer
        Dim endPosition As Integer
        Dim length As Integer

        startPosition = myFile.IndexOf("(")
        endPosition = myFile.IndexOf(")") - 1
        length = endPosition - startPosition
        tankDimensions = myFile.Substring(startPosition + 1, length)


        Return tankDimensions
    End Function

    'Private Function TrimFullHeight(myFile As String) As Integer
    '    Dim fullMark As String
    '    Dim position As Integer

    '    position = myFile.IndexOf("_@ ")
    '    fullMark = myFile.Remove(0, position + 5)

    '    Return fullMark
    'End Function

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
        volCamText.Font = "1CamBam_Stick_3"
        volCamText.Height = IIf(volume > 99999, "5", "5.5")
        centreText = IIf(volume > 9999, 0.5, 3)
        volCamText.Location = centreText + x & "," & ypos + 153 & ",0"
        myUI.ActiveView.CADFile.Add(volCamText)
        'units text
        unitsCamText.Text = "LITRE"
        unitsCamText.Font = "1CamBam_Stick_3"
        unitsCamText.Height = "5"
        unitsCamText.Location = 1 + x & "," & ypos + 145 & ",0"
        myUI.ActiveView.CADFile.Add(unitsCamText)
    End Sub


End Class