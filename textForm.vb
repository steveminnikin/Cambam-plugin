Imports CamBamPlugin.CamBamPlugin.MyPlugin

Namespace CamBamPlugin

Public Class textForm
    Private commonDetails As CommonDetails
    Property AddTank As Boolean
    Property TankNumber As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrWhiteSpace(txtFullVolHeight.Text) Then
            MsgBox("You must enter a FV Height!")
        Else
            Dim ystartPoint As String = "0"
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
            commonDetails.WriteVerticalInfo(commonDetails.FirstLineText, commonDetails.SecondLineText, ystartPoint)




            myUI.ActiveView.RefreshView()
            Me.ResetText()
            commonDetails = Nothing
            Me.Hide()
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