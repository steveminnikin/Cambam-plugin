﻿Imports CamBamPlugin.MyPlugin
Imports CamBamPlugin.CommonDetails

Public Class textForm
    Private commonDetails As CommonDetails
    Property AddTank As Boolean
    Property TankNumber As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtFullVolHeight.Text.Equals("") Then
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

            myDoc = CreateCADFile()
            myLayer = CreateLayer(myDoc, Ref)
            myPart = CreatePart(myDoc, Ref)

            WriteRef(Ref, DipHeight, 0)
            WriteClientRef(DipHeight, 0, ClientRef, RefText)
            If AddTank Then WriteTank(TankNumber, DipHeight, 0)
            WriteVerticalInfo(FirstLineText, SecondLineText, ystartPoint)




            myUI.ActiveView.RefreshView()
            Me.ResetText()
            commonDetails = Nothing
            Me.Hide()
        End If
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