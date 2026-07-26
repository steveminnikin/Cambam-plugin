Attribute VB_Name = "JSONExport"
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
