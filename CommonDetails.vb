Imports CamBamPlugin.MyPlugin

Public Class CommonDetails
    Shared Property ClientRef As String = ""
    Shared Property RefText As Boolean
    Shared Property Copies As Single
    Shared Property FullVol As String
    Shared Property DipHeight As String
    Shared Property Increments As String
    Shared Property markedVolIncrement As String
    Shared Property FirstLineText As New MText
    Shared Property SecondLineText As New MText
    Shared Property ref As String

    Public Sub New(Optional calForm As CalForm = Nothing, Optional unCalForm As UnCalForm = Nothing, Optional textForm As textForm = Nothing)
        If Not IsNothing(calForm) Then
            ClientRef = calForm.txtClientRef.Text
            RefText = calForm.chkStriker.Checked
            Copies = calForm.NumDips.Value
            markedVolIncrement = calForm.txtMarkedVolumes.Text
            Increments = calForm.txtIncrements.Text
            DipHeight = calForm.txtDipHeight.Text
            FullVol = calForm.txtFullVol.Text
            FirstLineText.Text = calForm.txtAddInfo.Text
            SecondLineText.Text = calForm.txtSecondLine.Text
            ref = calForm.txtRef.Text
        ElseIf Not IsNothing(unCalForm) Then
            ref = unCalForm.txtRef.Text
            Increments = unCalForm.txtIncs.Text
            DipHeight = unCalForm.txtHeight.Text
            Copies = unCalForm.NumDips.Value
            FirstLineText.Text = unCalForm.txtAddInfo.Text
            SecondLineText.Text = unCalForm.txtSecondLine.Text
        Else
            FirstLineText.Text = textForm.TextBox1.Text
            SecondLineText.Text = textForm.TextBox2.Text
        End If

    End Sub

    Public Shared Function CreateCADFile()
        Dim myDoc = New CADFile
        Dim myArcCentreMode As CamBam.CAM.ArcCenterModes

        myDoc = myUI.ActiveView.CADFile
        myArcCentreMode = ArcCenterModes.IncrementalFromP1
        myDoc.MachiningOptions.ArcCenterMode = myArcCentreMode

        Return myDoc
    End Function
    Public Shared Function CreateLayer(myDoc As CADFile, Optional n As String = "") As Layer
        Dim myLayer = New Layer
        myLayer.Name = n & " Dipstick Layer"
        myLayer.Visible = True
        myDoc.Layers.Add(myLayer)

        Return myLayer
    End Function
    Public Shared Function CreatePart(myDoc As CADFile, Optional ref As String = "") As CAMPart
        If Not myUI.ActiveView.CADFile.HasPart("Dipstick Machine Part") Then
            Dim myPart As New CAMPart
            Dim myEngravingOp As New MOPEngrave()

            myEngravingOp = CreateEngraving(ref)

            myPart = myDoc.CreatePart("Dipstick Machine Part")
            myPart.MachineOps.Add(myEngravingOp)
            Return myPart
        End If

    End Function
    Public Shared Function CreateEngraving(ref As String) As MOPEngrave
        Dim myFeedRate As CamBam.Values.CBValue(Of Double),
            myDepthInc As CamBam.Values.CBValue(Of Double),
            myTarget As CamBam.Values.CBValue(Of Double),
            myClearance As CamBam.Values.CBValue(Of Double),
            myToolDiameter As CamBam.Values.CBValue(Of Double),
            myToolNumber As CamBam.Values.CBValue(Of Integer),
            myVelocityMode As CamBam.Values.CBValue(Of VelocityModes),
            myCustomHeader As New CamBam.Values.CBValue(Of String)

        myVelocityMode.SetValue(VelocityModes.ExactStop)
        myFeedRate.SetValue(500.0)
        myDepthInc.SetValue(0.45)
        myTarget.SetValue(-0.45)
        myClearance.SetValue(0.4)
        myToolDiameter.SetValue(1.0)
        myToolNumber.SetValue(10)
        myCustomHeader.SetValue("( Full Volume: " & ")")

        Dim myEngrave = New CamBam.CAM.MOPEngrave()
        With myEngrave
            .Name = ref & " Engraving Op"
            .CutFeedrate = myFeedRate
            .DepthIncrement = myDepthInc
            .TargetDepth = myTarget
            .ClearancePlane = myClearance
            .ToolDiameter = myToolDiameter
            .ToolNumber = myToolNumber
            '.StartPoint = myStartPoint
            .VelocityMode = myVelocityMode
            .CustomMOPHeader = myCustomHeader

        End With
        Return myEngrave
    End Function
    Public Shared Function CreateCopies(n As Integer) As Integer
        Select Case n
            Case 1
                Return 0
            Case 2
                Return 30
        End Select
    End Function
    Public Shared Function WriteRef(r As String, p As Single, x As Single) As Layer
        Dim refXPos As Single = p + 40
        Dim myCamText As New MText()
        myCamText.Text = r
        myCamText.Font = "1CamBam_Stick_3"
        myCamText.Height = "5.5"
        myCamText.Location = 0.5 + x & "," & refXPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText)
    End Function
    Public Shared Function WriteUnits(u As String, h As Single, x As Single) As Layer
        Dim myCamText As New MText()
        Dim UniPos As Single = h + 14
        myCamText.Text = u
        myCamText.Font = "1CamBam_Stick_3"
        myCamText.Height = "5"
        myCamText.Location = 1 + x & "," & UniPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText)
    End Function
    Public Shared Sub WriteClientRef(yPos As Single, xPos As Single, text As String, ref As Boolean)
        If ref Then
            Dim refCamText As New MText()
            Dim refPos As Single = yPos + 113
            refCamText.Text = "REF"
            refCamText.Font = "1CamBam_Stick_3"
            refCamText.Height = "5.5"
            refCamText.Location = 3 + xPos & "," & refPos & ",0"
            myUI.ActiveView.CADFile.Add(refCamText)
        End If
        If Not text.Equals("") Then
            Dim myCamText As New MText()
            Dim UniPos As Single = yPos + 105
            myCamText.Text = text
            myCamText.Font = "1CamBam_Stick_3"
            myCamText.Height = "5.5"
            myCamText.Location = 1 + xPos & "," & UniPos & ",0"
            myUI.ActiveView.CADFile.Add(myCamText)
        End If
    End Sub

    Public Shared Sub WriteVerticalInfo(firstLine As MText, secondLine As MText, yLocation As String)

        firstLine.Font = "1CamBam_Stick_3"
        firstLine.Height = "6"
        firstLine.Transform.RotZ(1.571)

        If secondLine.Text.Equals("") Then
            firstLine.Location = yLocation & ",-6"
            myUI.ActiveView.CADFile.Add(firstLine)

        Else
            secondLine.Font = "1CamBam_Stick_3"
            secondLine.Height = "6"
            secondLine.Transform.RotZ(1.571)

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
            myUI.ActiveView.CADFile.Add(firstLine)
            myUI.ActiveView.CADFile.Add(secondLine)

        End If
    End Sub

    Public Shared Function isDivisible(x As Integer, y As Integer) As Boolean
        Return (x Mod y) = 0
    End Function
End Class

