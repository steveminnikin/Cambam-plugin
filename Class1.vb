
Imports CamBam.ThisApplication
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports System.IO

Public Class MyPlugin
    Public Shared myDoc As CADFile
    Public Shared myLayer As CAD.Layer
    Public Shared myPart As CAM.CAMPart
    Public Shared myEngrave As CAM.MOPEngrave
    Public Shared myUI As CamBamUI
    Public Shared ref As Integer
    Public Shared Number As Single
    'Public Shared StrikerAmount As Integer
    Public Shared PrimIDs As List(Of Integer)
    Public Shared clientRef As String = ""
    Public Shared yPos As Single
    Public Shared additionalInfo As String = ""
    Public Shared refText As Boolean
    Public Shared numInterval As Integer

    Public Shared isMarkedIncrement As Boolean


    Public Shared Sub InitPlugin(ByRef ui As CamBamUI)
        myUI = ui
        'create new plugin menu item
        Dim mi As New ToolStripMenuItem()
        mi.Text = "Dipsticks"
        ui.Menus.mnuPlugins.DropDownItems.Add(mi)

        'create submenu items
        Dim uncal, cal As New ToolStripMenuItem
        uncal.Text = "Uncalibrated"
        cal.Text = "Calibrated"
        'add submenu items to Dipsticks menu item
        mi.DropDownItems.Add(uncal)
        mi.DropDownItems.Add(cal)
        AddHandler uncal.Click, AddressOf uncal_clicked
        AddHandler cal.Click, AddressOf cal_clicked
    End Sub
    Public Shared Sub uncal_clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim myUncalForm As New UnCalForm
        myUncalForm.Show()
    End Sub
    Public Shared Sub cal_clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim myCalForm As New CalForm
        myCalForm.Show()
    End Sub
    Public Shared Sub CreateLayer(n As Single)
        myDoc = myUI.ActiveView.CADFile
        myLayer = New CamBam.CAD.Layer
        myLayer.Name = n & "_Dipstick"
        myLayer.Visible = True
        myDoc.Layers.Add(myLayer)
    End Sub
    Public Shared Sub CreatePart()
        If Not myUI.ActiveView.CADFile.HasPart("Dipsticks") Then
            Dim myFeedRate As CamBam.Values.CBValue(Of Double)
            Dim myDepthInc As CamBam.Values.CBValue(Of Double)
            Dim myTarget As CamBam.Values.CBValue(Of Double)
            Dim myClearance As CamBam.Values.CBValue(Of Double)
            Dim myToolDiameter As CamBam.Values.CBValue(Of Double)
            Dim myToolNumber As CamBam.Values.CBValue(Of Integer)
            Dim myArcCentreMode As CamBam.CAM.ArcCenterModes
            'Dim myStartPoint As CamBam.Values.CBValue(Of Point3F)
            Dim myVelocityMode As CamBam.Values.CBValue(Of VelocityModes)
            myVelocityMode.SetValue(VelocityModes.ExactStop)
            'myStartPoint.SetValue({0, 0, 0})
            myFeedRate.SetValue(450.0)
            myDepthInc.SetValue(0.45)
            myTarget.SetValue(-0.45)
            myClearance.SetValue(0.4)
            myToolDiameter.SetValue(1.0)
            myToolNumber.SetValue(10)
            myArcCentreMode = ArcCenterModes.IncrementalFromP1
            myDoc.MachiningOptions.ArcCenterMode = myArcCentreMode
            myPart = myDoc.CreatePart("PartName")
            myEngrave = New CamBam.CAM.MOPEngrave()
            myPart.MachineOps.Add(myEngrave)
            With myEngrave
                .Name = "ENgraveName"
                .CutFeedrate = myFeedRate
                .DepthIncrement = myDepthInc
                .TargetDepth = myTarget
                .ClearancePlane = myClearance
                .ToolDiameter = myToolDiameter
                .ToolNumber = myToolNumber
                '.StartPoint = myStartPoint
                .VelocityMode = myVelocityMode

            End With


        End If

    End Sub
    Public Shared Function CreateCopies(n As Integer) As Integer
        Select Case n
            Case 1
                Return 0
            Case 2
                Return 30
                'Case 3
                '    Return 100
                'Case Else
                '    Return 150
        End Select
    End Function
    Public Shared Sub WriteRef(r As Integer, p As Single, x As Single)
        Dim refXPos As Single = p + 40
        Dim myCamText As New MText()
        myCamText.Text = r
        myCamText.Font = "1CamBam_Stick_3"
        myCamText.Height = "5.5"
        myCamText.Location = 0.5 + x & "," & refXPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText, myLayer)
    End Sub
    Public Shared Sub WriteUnits(u As String, h As Single, x As Single)
        Dim myCamText As New MText()
        Dim UniPos As Single = h + 14
        myCamText.Text = u
        myCamText.Font = "1CamBam_Stick_3"
        myCamText.Height = "5"
        myCamText.Location = 1 + x & "," & UniPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText, myLayer)
    End Sub
    Public Shared Sub WriteClientRef(yPos As Single, xPos As Single, text As String, ref As Boolean)
        If ref Then
            Dim refCamText As New MText()
            Dim refPos As Single = yPos + 113
            refCamText.Text = "REF"
            refCamText.Font = "1CamBam_Stick_3"
            refCamText.Height = "5.5"
            refCamText.Location = 3 + xPos & "," & refPos & ",0"
            myUI.ActiveView.CADFile.Add(refCamText, myLayer)
        End If
        Dim myCamText As New MText()
        Dim UniPos As Single = yPos + 105
        myCamText.Text = text
        myCamText.Font = "1CamBam_Stick_3"
        myCamText.Height = "5.5"
        myCamText.Location = 1 + xPos & "," & UniPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText, myLayer)
    End Sub
    Public Shared Sub WriteAdditionalInfo(yPos As Single, xPos As Single, text As String)
        Dim myCamText As New MText()
        Dim UniPos As Single = yPos + If(Not clientRef = "", 148, 105)
        myCamText.Text = text
        myCamText.Font = "1CamBam_Stick_3"
        myCamText.Height = "5.5"
        myCamText.Location = 11 + xPos & "," & UniPos & ",0"
        myUI.ActiveView.CADFile.Add(myCamText, myLayer)
    End Sub
    
    Public Shared Function isDivisible(x As Integer, y As Integer) As Boolean
        Return (x Mod y) = 0
    End Function



End Class