Imports System
Imports System.Collections.Generic

Namespace CamBamPlugin

''' <summary>
''' Represents calibration data parsed from a JSON file for calibrated dipstick generation.
''' </summary>
Public Class CalibrationData

#Region "Properties"
    ''' <summary>
    ''' Calibration points sorted numerically by height.
    ''' Key: Height in millimeters, Value: Volume in litres
    ''' </summary>
    Public Property VolumeHeightPairs As SortedList(Of Decimal, Decimal)

    ''' <summary>
    ''' Full volume capacity extracted from filename
    ''' </summary>
    Public Property FullVolume As Integer

    ''' <summary>
    ''' Volume increment value extracted from filename
    ''' </summary>
    Public Property Increments As Integer

    ''' <summary>
    ''' Tank dimensions formatted for filename based on tank type.
    ''' - Rectangular: "length_width_height" (e.g., "1235_2545_1555")
    ''' - Horizontal Flat Ends: "diameter_length" (e.g., "2488_2999")
    ''' - Horizontal Dished Ends: "diameter_stLength_dishEndRad_knuckleRad" (e.g., "2488_2999_2500_70")
    ''' </summary>
    Public Property TankDimensions As String

    ''' <summary>
    ''' Tank type from JSON (e.g., "Rectangular", "Horizontal Flat Ends", "Horizontal Dished Ends")
    ''' </summary>
    Public Property TankType As String

    ''' <summary>
    ''' Top height value from JSON results (maximum dipstick height in mm)
    ''' </summary>
    Public Property TopHeight As Single

    ''' <summary>
    ''' Original filename (without path)
    ''' </summary>
    Public Property FileName As String

    ''' <summary>
    ''' Full file path
    ''' </summary>
    Public Property FilePath As String

#End Region

#Region "Constructor"
    ''' <summary>
    ''' Initializes a new instance of CalibrationData
    ''' </summary>
    Public Sub New()
        VolumeHeightPairs = New SortedList(Of Decimal, Decimal)
        FullVolume = 0
        Increments = 0
        TankDimensions = String.Empty
        TankType = String.Empty
        TopHeight = 0
        FileName = String.Empty
        FilePath = String.Empty
    End Sub

#End Region

#Region "Helper Methods"
    ''' <summary>
    ''' Checks if calibration data is valid
    ''' </summary>
    ''' <returns>True if data is valid</returns>
    Public Function IsValid() As Boolean
        If VolumeHeightPairs Is Nothing OrElse VolumeHeightPairs.Count = 0 Then
            Return False
        End If

        If FullVolume <= 0 Then
            Return False
        End If

        If Increments <= 0 Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Gets the height at which a specific volume is marked
    ''' </summary>
    ''' <param name="volume">Volume in litres to look up</param>
    ''' <returns>Height in millimeters, or 0 if the volume has no calibration point</returns>
    Public Function GetHeightForVolume(volume As Decimal) As Decimal
        Dim index As Integer = VolumeHeightPairs.IndexOfValue(volume)
        If index >= 0 Then
            Return VolumeHeightPairs.Keys(index)
        End If
        Return 0
    End Function

    ''' <summary>
    ''' Gets the height for the full volume
    ''' </summary>
    ''' <returns>Full volume height as Single</returns>
    Public Function GetFullVolumeHeight() As Single
        Return CSng(GetHeightForVolume(FullVolume))
    End Function

    ''' <summary>
    ''' Gets the number of calibration points
    ''' </summary>
    ''' <returns>Count of volume/height pairs</returns>
    Public Function GetPointCount() As Integer
        Return VolumeHeightPairs.Count
    End Function

#End Region

End Class

End Namespace
