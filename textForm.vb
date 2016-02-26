Imports CamBamPlugin.MyPlugin

Public Class textForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim FirstLineText As New MText()
        Dim SecondLineText As New MText()
        Dim ystartPoint As String = "0"

        FirstLineText.Text = TextBox1.Text
        SecondLineText.Text = TextBox2.Text
        WriteVerticalInfo(FirstLineText, SecondLineText, ystartPoint)
        CreatePart()
        myUI.ActiveView.RefreshView()
        Me.Visible = False
    End Sub
    Public Shared Sub WriteVerticalInfo(firstLine As MText, secondLine As MText, yLocation As String)


        firstLine.Font = "1CamBam_Stick_3"
        secondLine.Font = "1CamBam_Stick_3"
        firstLine.Height = "6"
        secondLine.Height = "6"
        firstLine.Transform.RotZ(1.571)
        secondLine.Transform.RotZ(1.571)

        CreateLayer(3)

        If secondLine.Text.Equals("") Then
            firstLine.Location = yLocation & ",-6"
            myDoc.Add(firstLine, myLayer)
        Else
            Dim firstLineYCentre As Double
            Dim secondLineCentroid As New Point3F
            Dim secondLineYCentre As Double
            Dim DiffCentres As Double

            firstLine.Location = yLocation & ",-2"
            secondLine.Location = yLocation & ",-10"

            firstLineYCentre = firstLine.GetCentroid().Y
            secondLineYCentre = secondLine.GetCentroid().Y
            DiffCentres = firstLineYCentre - secondLineYCentre

            secondLine.Transform.Translate(0, DiffCentres, 0)
            myDoc.Add(firstLine, myLayer)
            myDoc.Add(secondLine, myLayer)
        End If




    End Sub
End Class