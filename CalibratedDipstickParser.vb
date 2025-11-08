Imports System
Imports System.IO
Imports System.Collections.Generic

''' <summary>
''' Parses calibration CSV files and extracts metadata from filenames.
''' Replaces the parsing logic scattered in CalForm.
''' </summary>
Public Class CalibratedDipstickParser

#Region "Public Methods"
    ''' <summary>
    ''' Parses a calibration file and returns CalibrationData
    ''' </summary>
    ''' <param name="filePath">Full path to the CSV file</param>
    ''' <param name="useRegularIncrements">True if using regular increments, False for volume-based</param>
    ''' <returns>CalibrationData object with parsed information</returns>
    Public Function Parse(filePath As String, useRegularIncrements As Boolean) As CalibrationData
        If String.IsNullOrWhiteSpace(filePath) Then
            Throw New ArgumentException("File path cannot be empty", NameOf(filePath))
        End If

        If Not File.Exists(filePath) Then
            Throw New FileNotFoundException("Calibration file not found", filePath)
        End If

        Dim data As New CalibrationData()
        data.FilePath = filePath
        data.FileName = Path.GetFileName(filePath)

        ' Parse filename for metadata
        Try
            data.FullVolume = ExtractFullVolume(filePath)
            data.Increments = ExtractIncrements(filePath)
            data.TankDimensions = ExtractTankDimensions(filePath)
        Catch ex As Exception
            Throw New FormatException("Failed to parse filename. Expected format: 'FV XXX_INCS YYY_(dimensions).csv'", ex)
        End Try

        ' Parse CSV file for volume/height pairs
        Try
            data.VolumeHeightPairs = ReadVolumeHeightPairs(filePath, useRegularIncrements)
        Catch ex As Exception
            Throw New FormatException("Failed to parse CSV file. Expected format: 'height,volume' per line", ex)
        End Try

        Return data
    End Function

    ''' <summary>
    ''' Suggests marked volume increments based on calibration increments
    ''' </summary>
    ''' <param name="increments">Increment value from calibration data</param>
    ''' <returns>Suggested marked interval, or 0 if no suggestion</returns>
    Public Function SuggestMarkedIncrements(increments As Integer) As Integer
        Select Case increments
            Case 25
                Return 100
            Case 50
                Return 200
            Case 100
                Return 500
            Case 200, 250
                Return 1000
            Case 400
                Return 800
            Case 500
                Return 2000
            Case 750
                Return 3000
            Case 1000
                Return 5000
            Case Else
                Return 0
        End Select
    End Function

#End Region

#Region "Private Parsing Methods"
    ''' <summary>
    ''' Extracts full volume from filename
    ''' Expected format: "...FV XXX_INCS..."
    ''' </summary>
    Private Function ExtractFullVolume(filePath As String) As Integer
        Dim fileName As String = Path.GetFileName(filePath)
        Dim position As Integer = fileName.IndexOf("FV ")

        If position = -1 Then
            Throw New FormatException("Filename does not contain 'FV ' marker")
        End If

        Dim fullVolStr As String = fileName.Substring(position + 3)
        position = fullVolStr.IndexOf("_INCS")

        If position = -1 Then
            Throw New FormatException("Filename does not contain '_INCS' marker")
        End If

        fullVolStr = fullVolStr.Remove(position)

        Dim fullVolume As Integer
        If Not Integer.TryParse(fullVolStr, fullVolume) Then
            Throw New FormatException($"Could not parse full volume: '{fullVolStr}'")
        End If

        Return fullVolume
    End Function

    ''' <summary>
    ''' Extracts increments from filename
    ''' Expected format: "..._INCS YYY_(..."
    ''' </summary>
    Private Function ExtractIncrements(filePath As String) As Integer
        Dim fileName As String = Path.GetFileName(filePath)
        Dim startPosition As Integer = fileName.IndexOf("_INCS ")

        If startPosition = -1 Then
            Throw New FormatException("Filename does not contain '_INCS ' marker")
        End If

        Dim incrementsPlusDetails As String = fileName.Remove(0, startPosition + 6)
        Dim endPosition As Integer = incrementsPlusDetails.IndexOf("_(")

        If endPosition = -1 Then
            Throw New FormatException("Filename does not contain '_(' after increments")
        End If

        Dim incrementsStr As String = incrementsPlusDetails.Remove(endPosition)

        Dim increments As Integer
        If Not Integer.TryParse(incrementsStr, increments) Then
            Throw New FormatException($"Could not parse increments: '{incrementsStr}'")
        End If

        Return increments
    End Function

    ''' <summary>
    ''' Extracts tank dimensions from filename
    ''' Expected format: "...(1200x800x600)..."
    ''' </summary>
    Private Function ExtractTankDimensions(filePath As String) As String
        Dim fileName As String = Path.GetFileName(filePath)
        Dim startPosition As Integer = fileName.IndexOf("(")

        If startPosition = -1 Then
            Return String.Empty ' Tank dimensions are optional
        End If

        Dim endPosition As Integer = fileName.IndexOf(")")

        If endPosition = -1 Or endPosition <= startPosition Then
            Return String.Empty
        End If

        Dim length As Integer = endPosition - startPosition - 1
        Dim dimensions As String = fileName.Substring(startPosition + 1, length)

        Return dimensions
    End Function

    ''' <summary>
    ''' Reads volume/height pairs from CSV file
    ''' </summary>
    ''' <param name="filePath">Path to CSV file</param>
    ''' <param name="useRegularIncrements">If True, swap column order (height,volume)</param>
    ''' <returns>SortedList with volume as key and height as value</returns>
    Private Function ReadVolumeHeightPairs(filePath As String, useRegularIncrements As Boolean) As SortedList(Of String, String)
        Dim pairs As New SortedList(Of String, String)

        Using reader As New StreamReader(filePath)
            Dim line As String
            Dim lineNumber As Integer = 0

            Do
                line = reader.ReadLine()
                If line Is Nothing Then Exit Do

                lineNumber += 1
                line = line.Trim()

                ' Skip empty lines
                If String.IsNullOrWhiteSpace(line) Then
                    Continue Do
                End If

                Dim elements As String() = line.Split(","c)

                If elements.Length < 2 Then
                    Throw New FormatException($"Line {lineNumber} does not contain two comma-separated values")
                End If

                ' Trim whitespace from elements
                For i As Integer = 0 To elements.Length - 1
                    elements(i) = elements(i).Trim()
                Next

                Try
                    If useRegularIncrements Then
                        ' File format: height,volume
                        ' Add as: volume -> height
                        pairs.Add(elements(1), elements(0))
                    Else
                        ' File format: volume,height
                        ' Add as: volume -> height
                        pairs.Add(elements(0), elements(1))
                    End If
                Catch ex As ArgumentException
                    ' Duplicate key - skip or throw?
                    ' For now, skip duplicates to match original behavior
                    Continue Do
                End Try

            Loop
        End Using

        If pairs.Count = 0 Then
            Throw New FormatException("CSV file contains no valid data")
        End If

        Return pairs
    End Function

#End Region

End Class
