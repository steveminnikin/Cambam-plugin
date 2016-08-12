
Imports System.Windows.Forms
Imports CamBamPlugin.CommonDetails

Public Class MyPlugin
    Public Shared myUI As CamBamUI

    Public Shared Sub InitPlugin(ByRef ui As CamBamUI)
        myUI = ui
        'create new plugin menu item
        Dim mi As New ToolStripMenuItem()
        mi.Text = "Dipsticks"
        ui.Menus.mnuPlugins.DropDownItems.Add(mi)

        'create submenu items
        Dim uncal, cal, text As New ToolStripMenuItem
        uncal.Text = "Uncalibrated"
        cal.Text = "Calibrated"
        text.Text = "Text Only"
        'add submenu items to Dipsticks menu item
        mi.DropDownItems.Add(uncal)
        mi.DropDownItems.Add(cal)
        mi.DropDownItems.Add(text)
        AddHandler uncal.Click, AddressOf uncal_clicked
        AddHandler cal.Click, AddressOf cal_clicked
        'AddHandler text.Click, AddressOf text_clicked
    End Sub
    Public Shared Sub uncal_clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim myUncalForm As New UnCalForm
        myUncalForm.Show()
    End Sub
    Public Shared Sub cal_clicked(ByVal sender As Object, ByVal e As EventArgs)
        Dim myCalForm As New CalForm
        myCalForm.Show()
    End Sub
    'Public Shared Sub text_clicked(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim mytextForm As New textForm
    '    mytextForm.Show()
    'End Sub
End Class