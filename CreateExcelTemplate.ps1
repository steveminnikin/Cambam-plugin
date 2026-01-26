$excel = New-Object -ComObject Excel.Application
$excel.Visible = $false
$excel.DisplayAlerts = $false

# Create new workbook
$workbook = $excel.Workbooks.Add()

# Rename first sheet to 'Calibration Data'
$dataSheet = $workbook.Sheets.Item(1)
$dataSheet.Name = 'Calibration Data'

# Add headers for metadata section
$dataSheet.Cells.Item(1, 1) = 'DIPSTICK CALIBRATION TEMPLATE'
$dataSheet.Cells.Item(1, 1).Font.Bold = $true
$dataSheet.Cells.Item(1, 1).Font.Size = 14

$dataSheet.Cells.Item(2, 1) = 'Enter your calibration data below, then click the Export JSON button'
$dataSheet.Cells.Item(2, 1).Font.Italic = $true

# Tank dimensions
$dataSheet.Cells.Item(4, 1) = 'Tank Dimensions (Optional)'
$dataSheet.Cells.Item(4, 1).Font.Bold = $true
$dataSheet.Cells.Item(5, 1) = 'Length (mm):'
$dataSheet.Cells.Item(5, 2) = ''
$dataSheet.Cells.Item(6, 1) = 'Width (mm):'
$dataSheet.Cells.Item(6, 2) = ''
$dataSheet.Cells.Item(7, 1) = 'Height (mm):'
$dataSheet.Cells.Item(7, 2) = ''

# Calculation settings
$dataSheet.Cells.Item(9, 1) = 'Calculation Settings'
$dataSheet.Cells.Item(9, 1).Font.Bold = $true
$dataSheet.Cells.Item(10, 1) = 'Increments (L):'
$dataSheet.Cells.Item(10, 2) = 100
$dataSheet.Cells.Item(10, 3) = '* Required'
$dataSheet.Cells.Item(10, 3).Font.Italic = $true
$dataSheet.Cells.Item(10, 3).Font.Color = 255

# Results
$dataSheet.Cells.Item(12, 1) = 'Results'
$dataSheet.Cells.Item(12, 1).Font.Bold = $true
$dataSheet.Cells.Item(13, 1) = 'Full Volume (L):'
$dataSheet.Cells.Item(13, 2) = ''
$dataSheet.Cells.Item(13, 3) = '* Required'
$dataSheet.Cells.Item(13, 3).Font.Italic = $true
$dataSheet.Cells.Item(13, 3).Font.Color = 255
$dataSheet.Cells.Item(14, 1) = 'Top Height (mm):'
$dataSheet.Cells.Item(14, 2) = ''
$dataSheet.Cells.Item(14, 3) = '* Required'
$dataSheet.Cells.Item(14, 3).Font.Italic = $true
$dataSheet.Cells.Item(14, 3).Font.Color = 255

# Volume/Height data section
$dataSheet.Cells.Item(16, 1) = 'Volume/Height Data'
$dataSheet.Cells.Item(16, 1).Font.Bold = $true
$dataSheet.Cells.Item(16, 3) = '* At least one row required'
$dataSheet.Cells.Item(16, 3).Font.Italic = $true
$dataSheet.Cells.Item(16, 3).Font.Color = 255

$dataSheet.Cells.Item(17, 1) = 'Volume (L)'
$dataSheet.Cells.Item(17, 1).Font.Bold = $true
$dataSheet.Cells.Item(17, 1).Interior.Color = 0xD9D9D9
$dataSheet.Cells.Item(17, 2) = 'Height (mm)'
$dataSheet.Cells.Item(17, 2).Font.Bold = $true
$dataSheet.Cells.Item(17, 2).Interior.Color = 0xD9D9D9

# Add example data rows (empty)
for ($i = 18; $i -le 200; $i++) {
    $dataSheet.Cells.Item($i, 1) = ''
    $dataSheet.Cells.Item($i, 2) = ''
}

# Set column widths
$dataSheet.Columns.Item(1).ColumnWidth = 22
$dataSheet.Columns.Item(2).ColumnWidth = 15
$dataSheet.Columns.Item(3).ColumnWidth = 18

# Add borders to input cells
$range = $dataSheet.Range('B5:B7')
$range.Borders.LineStyle = 1
$range.Interior.Color = 0xFFFFFF

$range = $dataSheet.Range('B10')
$range.Borders.LineStyle = 1
$range.Interior.Color = 0xFFFFFF

$range = $dataSheet.Range('B13:B14')
$range.Borders.LineStyle = 1
$range.Interior.Color = 0xFFFFFF

$range = $dataSheet.Range('A18:B200')
$range.Borders.LineStyle = 1
$range.Interior.Color = 0xFFFFFF

# Delete extra sheets
while ($workbook.Sheets.Count -gt 1) {
    $workbook.Sheets.Item($workbook.Sheets.Count).Delete()
}

Write-Host 'Workbook structure created'

# Add VBA module
$vbaCode = @'
Option Explicit

Public Sub ExportToJSON()
    Dim ws As Worksheet
    Dim filePath As String
    Dim fileNum As Integer
    Dim json As String
    Dim i As Long
    Dim lastRow As Long
    Dim volume As Variant
    Dim height As Variant
    Dim incrementData As String
    Dim isFirst As Boolean

    Set ws = ThisWorkbook.Sheets("Calibration Data")

    ' Validate required fields
    If IsEmpty(ws.Range("B10").Value) Or ws.Range("B10").Value = "" Then
        MsgBox "Increments (L) is required!", vbExclamation, "Validation Error"
        Exit Sub
    End If

    If IsEmpty(ws.Range("B13").Value) Or ws.Range("B13").Value = "" Then
        MsgBox "Full Volume (L) is required!", vbExclamation, "Validation Error"
        Exit Sub
    End If

    If IsEmpty(ws.Range("B14").Value) Or ws.Range("B14").Value = "" Then
        MsgBox "Top Height (mm) is required!", vbExclamation, "Validation Error"
        Exit Sub
    End If

    ' Check for at least one volume/height pair
    If IsEmpty(ws.Range("A18").Value) Or ws.Range("A18").Value = "" Then
        MsgBox "At least one Volume/Height data row is required!", vbExclamation, "Validation Error"
        Exit Sub
    End If

    ' Get save file path
    filePath = Application.GetSaveAsFilename( _
        InitialFileName:="calibration_data.json", _
        FileFilter:="JSON Files (*.json), *.json", _
        Title:="Export Calibration Data as JSON")

    If filePath = "False" Then Exit Sub

    ' Build JSON string
    json = "{" & vbCrLf

    ' Tank dimensions (optional)
    json = json & "  ""tank"": {" & vbCrLf
    json = json & "    ""dimensions"": {" & vbCrLf

    If Not IsEmpty(ws.Range("B5").Value) And ws.Range("B5").Value <> "" Then
        json = json & "      ""length"": " & ws.Range("B5").Value & "," & vbCrLf
    Else
        json = json & "      ""length"": null," & vbCrLf
    End If

    If Not IsEmpty(ws.Range("B6").Value) And ws.Range("B6").Value <> "" Then
        json = json & "      ""width"": " & ws.Range("B6").Value & "," & vbCrLf
    Else
        json = json & "      ""width"": null," & vbCrLf
    End If

    If Not IsEmpty(ws.Range("B7").Value) And ws.Range("B7").Value <> "" Then
        json = json & "      ""height"": " & ws.Range("B7").Value & vbCrLf
    Else
        json = json & "      ""height"": null" & vbCrLf
    End If

    json = json & "    }" & vbCrLf
    json = json & "  }," & vbCrLf

    ' Calculation settings
    json = json & "  ""calculation"": {" & vbCrLf
    json = json & "    ""increments"": " & ws.Range("B10").Value & vbCrLf
    json = json & "  }," & vbCrLf

    ' Results
    json = json & "  ""results"": {" & vbCrLf
    json = json & "    ""fullVolume"": " & ws.Range("B13").Value & "," & vbCrLf
    json = json & "    ""topHeight"": " & ws.Range("B14").Value & vbCrLf
    json = json & "  }," & vbCrLf

    ' Increment data array
    json = json & "  ""incrementData"": [" & vbCrLf

    ' Find last row with data
    lastRow = ws.Cells(ws.Rows.Count, 1).End(-4162).Row ' xlUp = -4162
    If lastRow < 18 Then lastRow = 18

    isFirst = True
    For i = 18 To lastRow
        volume = ws.Cells(i, 1).Value
        height = ws.Cells(i, 2).Value

        If Not IsEmpty(volume) And volume <> "" And Not IsEmpty(height) And height <> "" Then
            If Not isFirst Then
                json = json & "," & vbCrLf
            End If
            json = json & "    { ""volume"": " & volume & ", ""height"": " & height & " }"
            isFirst = False
        End If
    Next i

    json = json & vbCrLf & "  ]" & vbCrLf
    json = json & "}" & vbCrLf

    ' Write to file
    fileNum = FreeFile
    Open filePath For Output As #fileNum
    Print #fileNum, json
    Close #fileNum

    MsgBox "JSON file exported successfully!" & vbCrLf & vbCrLf & filePath, vbInformation, "Export Complete"
End Sub
'@

# Access VBA project and add module
try {
    $vbModule = $workbook.VBProject.VBComponents.Add(1) # 1 = vbext_ct_StdModule
    $vbModule.Name = "JSONExport"
    $vbModule.CodeModule.AddFromString($vbaCode)
    Write-Host 'VBA module added'
} catch {
    Write-Host "Warning: Could not add VBA module automatically. Error: $_"
    Write-Host "You may need to enable 'Trust access to the VBA project object model' in Excel Trust Center settings."
}

# Add a button to the sheet
try {
    $button = $dataSheet.Buttons.Add(280, 10, 120, 30)
    $button.Caption = "Export to JSON"
    $button.OnAction = "ExportToJSON"
    Write-Host 'Button added'
} catch {
    Write-Host "Warning: Could not add button. You can run the macro manually from Developer tab."
}

# Save as macro-enabled workbook
$savePath = 'C:\Users\steve\OneDrive\Code\Cambam-plugin\DipstickCalibrationTemplate.xlsm'
$workbook.SaveAs($savePath, 52)  # 52 = xlOpenXMLWorkbookMacroEnabled

Write-Host "Saved to: $savePath"

# Clean up
$workbook.Close($false)
$excel.Quit()
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($dataSheet) | Out-Null
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($workbook) | Out-Null
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($excel) | Out-Null
[System.GC]::Collect()
[System.GC]::WaitForPendingFinalizers()

Write-Host "Excel template created successfully!"
