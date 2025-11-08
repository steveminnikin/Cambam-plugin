Imports CamBamPlugin.CamBamPlugin.MyPlugin

Namespace CamBamPlugin

Public Class CommonDetails
    ' NEW: Instance-based model replacing shared state
    Public Property Model As New DipstickModel
    Public Property FirstLineText As New MText
    Public Property SecondLineText As New MText

    ' OLD: Shared properties (deprecated, will be removed after migration)
    Shared Property ClientRef As String = ""
    Shared Property RefText As Boolean
    Shared Property Copies As Single
    Shared Property FullVol As String
    Shared Property DipHeight As String
    Shared Property Increments As String
    Shared Property tankDetails As String
    Shared Property MarkedVolIncrement As String
    Shared Property OldFirstLineText As New MText
    Shared Property OldSecondLineText As New MText
    Shared Property Ref As String
    Shared Property RefSecondLine As String
    Shared Property Laser As Boolean

    Public Sub New(Optional calForm As CalForm = Nothing, Optional unCalForm As UnCalForm = Nothing, Optional textForm As textForm = Nothing)
        If Not IsNothing(calForm) Then
            ' Populate NEW instance model
            Model.ClientRef = calForm.txtClientRef.Text
            Model.IncludeStriker = calForm.chkStriker.Checked
            Model.Copies = CInt(calForm.NumDips.Value)
            Model.Ref = calForm.txtRef.Text
            Model.IsCalibrated = True

            ' Parse numeric values safely
            Single.TryParse(calForm.txtDipHeight.Text, Model.Height)
            Single.TryParse(calForm.txtIncrements.Text, Model.Increments)
            Decimal.TryParse(calForm.txtFullVol.Text, Model.FullVolume)

            ' Text objects
            FirstLineText.Text = calForm.txtAddInfo.Text
            SecondLineText.Text = calForm.txtSecondLine.Text

            ' OLD: Also populate shared properties for backwards compatibility during migration
            ClientRef = calForm.txtClientRef.Text
            RefText = calForm.chkStriker.Checked
            Copies = calForm.NumDips.Value
            MarkedVolIncrement = calForm.txtMarkedVolumes.Text
            Increments = calForm.txtIncrements.Text
            DipHeight = calForm.txtDipHeight.Text
            FullVol = calForm.txtFullVol.Text
            OldFirstLineText.Text = calForm.txtAddInfo.Text
            OldSecondLineText.Text = calForm.txtSecondLine.Text
            Ref = calForm.txtRef.Text

        ElseIf Not IsNothing(unCalForm) Then
            ' Populate NEW instance model
            Model.Ref = unCalForm.txtRef.Text
            Model.Copies = CInt(unCalForm.NumDips.Value)
            Model.IsCalibrated = False

            ' Parse numeric values safely
            Single.TryParse(unCalForm.txtIncs.Text, Model.Increments)
            Single.TryParse(unCalForm.txtHeight.Text, Model.Height)

            ' Text objects
            FirstLineText.Text = unCalForm.txtAddInfo.Text
            SecondLineText.Text = unCalForm.txtSecondLine.Text

            ' OLD: Also populate shared properties for backwards compatibility
            Ref = unCalForm.txtRef.Text
            Increments = unCalForm.txtIncs.Text
            DipHeight = unCalForm.txtHeight.Text
            Copies = unCalForm.NumDips.Value
            OldFirstLineText.Text = unCalForm.txtAddInfo.Text
            OldSecondLineText.Text = unCalForm.txtSecondLine.Text

        Else
            ' Populate NEW instance model
            Model.Ref = textForm.txtOurRef.Text
            Model.ClientRef = textForm.txtClientRef.Text
            Model.IncludeStriker = textForm.chkRef.Checked
            Model.TankRef = textForm.txtTankLetter.Text

            ' Parse numeric value safely
            Single.TryParse(textForm.txtFullVolHeight.Text, Model.Height)

            ' Text objects
            FirstLineText.Text = textForm.txtFirstVertical.Text
            SecondLineText.Text = textForm.txtSecondVertical.Text

            ' OLD: Also populate shared properties for backwards compatibility
            Ref = textForm.txtOurRef.Text
            RefSecondLine = textForm.txtTankLetter.Text
            RefText = textForm.chkRef.Checked
            ClientRef = textForm.txtClientRef.Text
            DipHeight = textForm.txtFullVolHeight.Text
            OldFirstLineText.Text = textForm.txtFirstVertical.Text
            OldSecondLineText.Text = textForm.txtSecondVertical.Text
        End If

    End Sub

    Public Function CreateCADFile() As CADFile
        Dim myDoc = New CADFile
        Dim myArcCentreMode As CamBam.CAM.ArcCenterModes

        myDoc = myUI.ActiveView.CADFile
        myArcCentreMode = ArcCenterModes.IncrementalFromP1
        myDoc.MachiningOptions.ArcCenterMode = myArcCentreMode
        If Model.IsLaser Then
            myDoc.MachiningOptions.Style = "laserEngrave"
            myDoc.MachiningOptions.PostProcessor = "Laser"
        End If

        Return myDoc
    End Function

    Public Function CreateLayer(myDoc As CADFile, Optional n As String = "") As Layer
        Dim myLayer = New Layer With {
            .Name = n & " Dipstick CAD",
            .Visible = True
        }
        myDoc.Layers.Add(myLayer)

        Return myLayer
    End Function

    Public Function CreatePart(myDoc As CADFile, Optional ref As String = "") As CAMPart
        If Not myUI.ActiveView.CADFile.HasPart(DipstickConstants.PART_NAME) Then
            Dim myPart As New CAMPart
            Dim spindleEngravingOp As MOPEngrave
            Dim laserEngraveOp As MOPEngrave

            spindleEngravingOp = CreateEngraving(ref, False)
            laserEngraveOp = CreateEngraving(ref, True)

            myPart = myDoc.CreatePart(DipstickConstants.PART_NAME)
            myPart.MachineOps.Add(spindleEngravingOp)
            myPart.MachineOps.Add(laserEngraveOp)

            Return myPart
        Else
            ' Part already exists - this should rarely happen since callers create fresh documents
            ' Return Nothing as defensive measure (original bug was returning uninitialized value)
            Return Nothing
        End If
    End Function

    Public Function CreateEngraving(ref As String, laser As Boolean) As MOPEngrave
            Dim myFeedRate As CamBam.Values.CBValue(Of Double),
            myDepthInc As CamBam.Values.CBValue(Of Double),
            myTarget As CamBam.Values.CBValue(Of Double),
            myClearance As CamBam.Values.CBValue(Of Double),
            myToolDiameter As CamBam.Values.CBValue(Of Double),
            myToolNumber As CamBam.Values.CBValue(Of Integer),
            myVelocityMode As CamBam.Values.CBValue(Of VelocityModes),
            myCustomHeader As New CamBam.Values.CBValue(Of String)

            myVelocityMode.SetValue(VelocityModes.ExactStop)
            If laser Then
                myFeedRate.SetValue(DipstickConstants.LASER_FEED_RATE)
                myDepthInc.SetValue(DipstickConstants.LASER_DEPTH_INCREMENT)
                myTarget.SetValue(-DipstickConstants.LASER_DEPTH_INCREMENT)
                myClearance.SetValue(DipstickConstants.LASER_DEPTH_INCREMENT)
            Else
                myFeedRate.SetValue(DipstickConstants.SPINDLE_FEED_RATE)
                myDepthInc.SetValue(DipstickConstants.SPINDLE_DEPTH_INCREMENT)
                myTarget.SetValue(-DipstickConstants.SPINDLE_DEPTH_INCREMENT)
                myClearance.SetValue(0.4)
            End If
            myToolDiameter.SetValue(DipstickConstants.TOOL_DIAMETER)
            myToolNumber.SetValue(DipstickConstants.TOOL_NUMBER)
            myCustomHeader.SetValue("( Full Volume: " & ")")

            Dim myEngrave = New CamBam.CAM.MOPEngrave()
            With myEngrave
                .Name = IIf(laser, Model.TankDimensions + " Laser", Model.TankDimensions + " Spindle")
                .CutFeedrate = myFeedRate
                .DepthIncrement = myDepthInc
                .TargetDepth = myTarget
                .ClearancePlane = myClearance
                .ToolDiameter = myToolDiameter
                .ToolNumber = myToolNumber
                '.StartPoint = myStartPoint
                .VelocityMode = myVelocityMode
                .CustomMOPHeader = myCustomHeader
                .Style = IIf(laser, "laserEngrave", "Engrave")

            End With
            Return myEngrave
    End Function

    Public Function CreateCopies(n As Integer) As Integer
        Select Case n
            Case 1
                Return DipstickConstants.SINGLE_COPY_X_OFFSET
            Case 2
                Return DipstickConstants.DUAL_COPY_X_OFFSET
            Case Else
                ' Default to single copy if invalid input
                Return DipstickConstants.SINGLE_COPY_X_OFFSET
        End Select
    End Function

    Public Sub WriteRef(ref As String, fullVolHeight As Single, x As Single)
        Dim refYPos As Single = fullVolHeight + DipstickConstants.REF_Y_OFFSET
        Dim myCamText As New MText With {
            .Text = ref,
            .Font = DipstickConstants.FONT_NAME,
            .Height = DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString(),
            .Location = 1.5 + x & "," & refYPos & ",0"
        }
        myUI.ActiveView.CADFile.Add(myCamText)
    End Sub

    Public Sub WriteUnits(u As String, h As Single, x As Single)
        Dim myCamText As New MText()
        Dim UniPos As Single = h + DipstickConstants.UNITS_Y_OFFSET
        myCamText.Text = u
        myCamText.Font = DipstickConstants.FONT_NAME
        myCamText.Height = "5"
        myCamText.Location = 1 + x & "," & UniPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText)
    End Sub

    Public Sub WriteClientRef(yPos As Single, xPos As Single, text As String, ref As Boolean)
        Dim secondLineText As New MText()
        Dim UniPos As Single = yPos + DipstickConstants.CLIENT_REF_Y_OFFSET

        If ref Then
            Dim refCamText As New MText()
            Dim refPos As Single = yPos + 113

            refCamText.Text = "REF"
            refCamText.Font = DipstickConstants.FONT_NAME
            refCamText.Height = DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString()
            refCamText.Location = 3 + xPos & "," & refPos & ",0"
            myUI.ActiveView.CADFile.Add(refCamText)
        End If

        If Not String.IsNullOrWhiteSpace(text) Then
            Dim myCamText As New MText With {
                .Text = text,
                .Font = DipstickConstants.FONT_NAME,
                .Height = DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString(),
                .Location = 1 + xPos & "," & UniPos & ",0"
            }
            myUI.ActiveView.CADFile.Add(myCamText)
        End If

        If Not String.IsNullOrWhiteSpace(Model.TankRef) Then
            secondLineText.Text = Model.TankRef
            secondLineText.Font = DipstickConstants.FONT_NAME
            secondLineText.Height = DipstickConstants.DEFAULT_TEXT_HEIGHT.ToString()
            secondLineText.Location = 8 + xPos & "," & UniPos - 8 & ",0"
            myUI.ActiveView.CADFile.Add(secondLineText)
        End If
    End Sub

    Public Sub WriteVerticalInfo(firstLine As MText, secondLine As MText, yLocation As String)
        ' Create NEW MText objects to avoid mutating the shared FirstLineText/SecondLineText objects
        ' This prevents rotation and translation from accumulating across multiple runs
        Dim firstLineCopy As New MText With {
            .Text = firstLine.Text,
            .Font = DipstickConstants.FONT_NAME,
            .Height = "6"
        }
        firstLineCopy.Transform.RotZ(DipstickConstants.VERTICAL_TEXT_ROTATION)

        If String.IsNullOrWhiteSpace(secondLine.Text) Then
            firstLineCopy.Location = yLocation & ",-6"
            myUI.ActiveView.CADFile.Add(firstLineCopy)
        Else
            Dim secondLineCopy As New MText With {
                .Text = secondLine.Text,
                .Font = DipstickConstants.FONT_NAME,
                .Height = "6"
            }
            secondLineCopy.Transform.RotZ(DipstickConstants.VERTICAL_TEXT_ROTATION)

            Dim firstLineYCentre As Double
            Dim secondLineCentroid As New Point3F
            Dim secondLineYCentre As Double
            Dim DiffCentres As Double

            firstLineCopy.Location = yLocation & ",-2"
            secondLineCopy.Location = yLocation & ",-10"

            firstLineYCentre = firstLineCopy.GetCentroid().Y
            secondLineYCentre = secondLineCopy.GetCentroid().Y
            DiffCentres = firstLineYCentre - secondLineYCentre

            secondLineCopy.Transform.Translate(0, DiffCentres, 0)
            myUI.ActiveView.CADFile.Add(firstLineCopy)
            myUI.ActiveView.CADFile.Add(secondLineCopy)
        End If
    End Sub

    Public Function IsDivisible(x As Integer, y As Integer) As Boolean
        Return (x Mod y) = 0
    End Function
End Class

End Namespace

