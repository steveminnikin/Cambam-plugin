Imports System.Windows.Forms
Imports CamBamPlugin.MyPlugin
Imports System.Math
Imports CamBamPlugin.textForm

Public Class CalForm
    Private myfile As String
    Private myList As SortedList(Of String, String)
    Private DipHeight As Single
    Private yPos As Single
    Private fullVol As Single
    Private increments As Single
    Private markedVolIncrement As Single
    Private isFileSelected As Boolean
    Private isRegIncrements As Boolean

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If isFileSelected Then
            CreateLayer(ref)
            CreatePart()
            Process()
            myUI.ActiveView.RefreshView()
            Me.Visible = False
        Else
            MessageBox.Show("You must select a file!")
        End If
    End Sub

    Public Sub Process()
        myList = New SortedList(Of String, String)
        Dim myElements As String()
        Try
            Using sR As New StreamReader(myfile)
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
        For Each i As KeyValuePair(Of String, String) In myList
            Drawline(i.Key, CreateCopies(Number))
            If Not isRegIncrements Then
                If isMultipleOfMarkedInterval(i.Value) Or i.Value = fullVol Or i.Value = increments Then
                    WriteNumber(i.Key, i.Value, CreateCopies(Number))
                End If
            Else
                WriteNumber(i.Key, i.Value, CreateCopies(Number))
            End If


        Next
        WriteUnits("LITRE", DipHeight, CreateCopies(Number))
        If Not ref.Equals("") Then WriteRef(ref, DipHeight, CreateCopies(Number))
        WriteSWC(DipHeight, CreateCopies(Number), "LITRE", Round(fullVol * 0.97))
        If Not clientRef.Equals("") Then WriteClientRef(DipHeight, CreateCopies(Number), clientRef, refText)
        If Not firstLine.Text.Equals("") Then WriteVerticalInfo(firstLine, secondLine, DipHeight + If(Not clientRef = "", 148, 105))
        'WriteAdditionalInfo(DipHeight, CreateCopies(Number), additionalInfo)
    End Sub

    Private Sub Drawline(l As Single, x As Single)
        'sets the source file for incs
        Dim myPoly As New Polyline()
        myPoly.Add(x, l, 0)
        myPoly.Add(x + 20, l, 0)
        'add it to active drawing
        myUI.ActiveView.CADFile.Add(myPoly, myLayer)
    End Sub

    Private Sub WriteNumber(l As Single, n As Single, x As Single)
        Dim NoPos As Single = l + 7
        'add some text
        Dim myCamText As New MText()
        myCamText.Text = n
        myCamText.Font = "1CamBam_Stick_3"
        'adjusts the size of the volume text so htat it always fits on to the dipstick
        myCamText.Height = IIf(n > 99999, "5", "5.5")
        myCamText.Location = 0.5 + x & "," & NoPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText, myLayer)
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
        myUI.ActiveView.CADFile.Add(swcCamText, myLayer)
        'vol text
        volCamText.Text = vol
        volCamText.Font = "1CamBam_Stick_3"
        volCamText.Height = IIf(vol > 99999, "5", "5.5")
        centreText = IIf(vol > 9999, 0.5, 3)
        volCamText.Location = centreText + x & "," & y + 68 & ",0"
        myUI.ActiveView.CADFile.Add(volCamText, myLayer)
        'units text
        unitsCamText.Text = units
        unitsCamText.Font = "1CamBam_Stick_3"
        unitsCamText.Height = "5"
        unitsCamText.Location = 1 + x & "," & y + 60 & ",0"
        myUI.ActiveView.CADFile.Add(unitsCamText, myLayer)

    End Sub
    Private Function isMultipleOfMarkedInterval(inc As Single) As Boolean
        Return (inc Mod markedVolIncrement) = 0
    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click



        OpenFileDialog1.ShowDialog()
        myfile = OpenFileDialog1.FileName
        txtFullVol.Text = TrimFullVolume(myfile)
        txtIncrements.Text = TrimIncrements(myfile)
        txtMarkedVolumes.Text = addSuggestedMarkedIncrements(txtIncrements.Text)

        If Not myfile = "" Then isFileSelected = True

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtRef.TextChanged
        ref = txtRef.Text
    End Sub

    Private Sub NumDips_ValueChanged(sender As Object, e As EventArgs) Handles NumDips.ValueChanged
        Number = NumDips.Value
    End Sub

    Private Sub txtAddInfo_LostFocus(sender As Object, e As EventArgs) Handles txtAddInfo.LostFocus
        firstLine.Text = txtAddInfo.Text

    End Sub

    Private Sub TextBox1_TextChanged_2(sender As Object, e As EventArgs) Handles txtFullVol.TextChanged
        fullVol = txtFullVol.Text
    End Sub

    Private Sub txtDipHeight_LostFocus(sender As Object, e As EventArgs) Handles txtDipHeight.LostFocus
        DipHeight = txtDipHeight.Text
    End Sub

    Private Sub txtClientRef_TextChanged(sender As Object, e As EventArgs) Handles txtClientRef.TextChanged
        clientRef = txtClientRef.Text
    End Sub

    
    Private Sub txtNumInterval_TextChanged(sender As Object, e As EventArgs) Handles txtIncrements.TextChanged
        increments = txtIncrements.Text
    End Sub

    Private Sub txtMarkedVolumes_TextChanged(sender As Object, e As EventArgs) Handles txtMarkedVolumes.TextChanged
        markedVolIncrement = txtMarkedVolumes.Text
    End Sub

    Private Sub chkStriker_CheckedChanged(sender As Object, e As EventArgs) Handles chkStriker.CheckedChanged
        If chkStriker.Checked Then refText = True
    End Sub

    Private Sub txtSecondLine_TextChanged(sender As Object, e As EventArgs) Handles txtSecondLine.TextChanged
        secondLine.Text = txtSecondLine.Text
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        isRegIncrements = CheckBox1.Checked
    End Sub
    Private Function TrimFullVolume(fileName As String) As Integer
        Dim fullVolume As String
        Dim position As Integer

        position = myfile.IndexOf("FV ")
        fullVolume = myfile.Substring(position + 3)
        position = fullVolume.IndexOf("_INCS")
        fullVolume = fullVolume.Remove(position)

        Return fullVolume
    End Function
    Private Function TrimIncrements(fileName As String) As Integer
        Dim increments As String
        Dim position As Integer

        position = myfile.IndexOf("_INCS ")
        increments = myfile.Remove(0, position + 5)

        Return increments
    End Function

    Private Function addSuggestedMarkedIncrements(increments As Integer) As Integer
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
End Class