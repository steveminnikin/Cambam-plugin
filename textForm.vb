Imports CamBamPlugin.MyPlugin
Imports CamBamPlugin.CommonDetails

Public Class textForm
    Private commonDetails As CommonDetails

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        commonDetails = New CommonDetails(,, Me)
        Dim ystartPoint As String = "0"

        myUI.FileNew(True, True, True)
        Dim myDoc As New CADFile
        Dim myLayer As Layer
        Dim myPart As CAMPart

        myDoc = CreateCADFile()
        myLayer = CreateLayer(myDoc, ref)
        myPart = CreatePart(myDoc, ref)

        WriteVerticalInfo(FirstLineText, SecondLineText, ystartPoint)

        myUI.ActiveView.RefreshView()
        Me.ResetText()
        commonDetails = Nothing
        Me.Hide()
    End Sub

End Class