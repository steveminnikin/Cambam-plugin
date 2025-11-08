Imports System
Imports System.Collections.Generic

Namespace CamBamPlugin

''' <summary>
''' Represents calibration data parsed from a CSV file for calibrated dipstick generation.
''' </summary>
Public Class CalibrationData

#Region "Properties"
    ''' <summary>
    ''' Dictionary mapping volume (in liters) to height (in millimeters).
    ''' Key: Volume as string, Value: Height as string
    ''' </summary>
    Public Property VolumeHeightPairs As SortedList(Of String, String)

    ''' <summary>
    ''' Full volume capacity extracted from filename
    ''' </summary>
    Public Property FullVolume As Integer

    ''' <summary>
    ''' Volume increment value extracted from filename
    ''' </summary>
    Public Property Increments As Integer

    ''' <summary>
    ''' Tank dimensions extracted from filename (e.g. 1200x800x600)
    ''' </summary>
    Public Property TankDimensions As String

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
        VolumeHeightPairs = New SortedList(Of String, String)
        FullVolume = 0
        Increments = 0
        TankDimensions = String.Empty
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
    ''' Gets the height for a specific volume
    ''' </summary>
    ''' <param name="volume">Volume to look up</param>
    ''' <returns>Height as string, or Nothing if not found</returns>
    Public Function GetHeight(volume As String) As String
        If VolumeHeightPairs.ContainsKey(volume) Then
            Return VolumeHeightPairs(volume)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the height for the full volume
    ''' </summary>
    ''' <returns>Full volume height as Single</returns>
    Public Function GetFullVolumeHeight() As Single
        Dim fullVolStr As String = FullVolume.ToString()
        Dim heightStr As String = GetHeight(fullVolStr)

        If Not String.IsNullOrEmpty(heightStr) Then
            Dim height As Single
            If Single.TryParse(heightStr, height) Then
                Return height
            End If
        End If

        Return 0
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
