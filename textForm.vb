Imports CamBamPlugin.MyPlugin
Imports CamBamPlugin.CommonDetails

Public Class textForm
    Private commonDetails As CommonDetails
    Property addTank As Boolean
    Property tankNumber As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim ystartPoint As String = "0"
        Dim myDoc As New CADFile
        Dim myLayer As Layer
        Dim myPart As CAMPart

        commonDetails = New CommonDetails(,, Me)
        addTank = chkTank.Checked
        tankNumber = txtTankNumber.Text


        myUI.FileNew(True, True, True)

        myDoc = CreateCADFile()
        myLayer = CreateLayer(myDoc, ref)
        myPart = CreatePart(myDoc, ref)

        WriteRef(ref, DipHeight, 0)
        WriteClientRef(DipHeight, 0, ClientRef, RefText)
        If addTank Then WriteTank(tankNumber, DipHeight, 0)
        WriteVerticalInfo(FirstLineText, SecondLineText, ystartPoint)




        myUI.ActiveView.RefreshView()
        Me.ResetText()
        commonDetails = Nothing
        Me.Hide()
    End Sub

    Private Shared Sub WriteTank(tankNumber As String, DipHeight As Single, x As Single)
        Dim refYPos As Single = DipHeight + 188
        Dim tankText As New MText()
        Dim numberText As New MText()

        tankText.Text = "TANK"
        tankText.Font = "1CamBam_Stick_3"
        tankText.Height = "5.5"
        tankText.Location = 0.5 + x & "," & refYPos & ",0"

        numberText.Text = tankNumber
        numberText.Font = "1CamBam_Stick_3"
        numberText.Height = "5.5"
        numberText.Location = 8 + x & "," & refYPos - 8 & ",0"

        myUI.ActiveView.CADFile.Add(tankText)
        myUI.ActiveView.CADFile.Add(numberText)


    End Sub
End Class