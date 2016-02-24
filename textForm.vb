Imports CamBamPlugin.MyPlugin

Public Class textForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim FirstLineText As New MText()
        Dim SecondLineText As New MText()

        FirstLineText.Text = TextBox1.Text
        SecondLineText.Text = TextBox2.Text
        FirstLineText.Font = "1CamBam_Stick_3"
        SecondLineText.Font = "1CamBam_Stick_3"
        FirstLineText.Height = "6"
        SecondLineText.Height = "6"
        FirstLineText.Transform.RotZ(1.571)
        SecondLineText.Transform.RotZ(1.571)

        CreateLayer(3)

        If SecondLineText.Text.Equals("") Then
            FirstLineText.Location = "0,-6"
            myDoc.Add(FirstLineText, myLayer)
        Else
            Dim firstLineYCentre As Double
            Dim secondLineCentroid As New Point3F
            Dim secondLineYCentre As Double
            Dim DiffCentres As Double

            FirstLineText.Location = "0,-2"
            SecondLineText.Location = "0,-10"

            firstLineYCentre = FirstLineText.GetCentroid().Y
            secondLineYCentre = SecondLineText.GetCentroid().Y
            DiffCentres = firstLineYCentre - secondLineYCentre
            MsgBox(DiffCentres)

            SecondLineText.Transform.Translate(0, DiffCentres, 0)
            myDoc.Add(FirstLineText, myLayer)
            myDoc.Add(SecondLineText, myLayer)
        End If


        CreatePart()
        myUI.ActiveView.RefreshView()
        Me.Visible = False

    End Sub
End Class