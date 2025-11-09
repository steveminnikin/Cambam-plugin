Imports System

Namespace CamBamPlugin

''' <summary>
''' Represents the configuration data for generating a dipstick.
''' Replaces the scattered shared properties in CommonDetails.
''' </summary>
Public Class DipstickModel

#Region "Properties"
    ''' <summary>
    ''' Reference text displayed on the dipstick
    ''' </summary>
    Public Property Ref As String = String.Empty

    ''' <summary>
    ''' Client reference text
    ''' </summary>
    Public Property ClientRef As String = String.Empty

    ''' <summary>
    ''' Tank reference text
    ''' </summary>
    Public Property TankRef As String = String.Empty

    ''' <summary>
    ''' Tank dimensions extracted from filename or user input
    ''' </summary>
    Public Property TankDimensions As String = String.Empty

    ''' <summary>
    ''' Full volume capacity in liters
    ''' </summary>
    Public Property FullVolume As Decimal

    ''' <summary>
    ''' Height of the dipstick in millimeters
    ''' </summary>
    Public Property Height As Single

    ''' <summary>
    ''' Full volume height (for calibrated dipsticks)
    ''' </summary>
    Public Property FullVolHeight As Single

    ''' <summary>
    ''' Increment value (volume or distance depending on dipstick type)
    ''' </summary>
    Public Property Increments As Single

    ''' <summary>
    ''' Marked volume increment - volume interval at which to display numbers (for calibrated dipsticks)
    ''' </summary>
    Public Property MarkedVolIncrement As Integer

    ''' <summary>
    ''' Number of dipstick copies to generate (1 or 2)
    ''' </summary>
    Public Property Copies As Integer = 1

    ''' <summary>
    ''' Whether to include striker in the design
    ''' </summary>
    Public Property IncludeStriker As Boolean = False

    ''' <summary>
    ''' Whether to include tank in the design
    ''' </summary>
    Public Property IncludeTank As Boolean = False

    ''' <summary>
    ''' Engraving type: True for laser, False for spindle
    ''' </summary>
    Public Property IsLaser As Boolean = False

    ''' <summary>
    ''' Whether this is a calibrated dipstick (uses calibration data)
    ''' </summary>
    Public Property IsCalibrated As Boolean = False

    ''' <summary>
    ''' Whether to show half-increment marks (uncalibrated only)
    ''' </summary>
    Public Property ShowHalfIncrements As Boolean = False

    ''' <summary>
    ''' Unit system for uncalibrated dipsticks (0=mm, 1=cm, 2=inches)
    ''' </summary>
    Public Property UnitSystem As Integer = 0

#End Region

#Region "Validation"
    ''' <summary>
    ''' Validates the model data
    ''' </summary>
    ''' <returns>True if valid, False otherwise</returns>
    Public Function IsValid() As Boolean
        ' Basic validation rules
        If Height <= 0 Then Return False
        If Increments <= 0 Then Return False
        If Copies < 1 Or Copies > 2 Then Return False
        If IsCalibrated AndAlso FullVolume <= 0 Then Return False

        Return True
    End Function

    ''' <summary>
    ''' Gets validation error message if model is invalid
    ''' </summary>
    ''' <returns>Error message or empty string if valid</returns>
    Public Function GetValidationError() As String
        If Height <= 0 Then Return "Height must be greater than zero"
        If Increments <= 0 Then Return "Increments must be greater than zero"
        If Copies < 1 Or Copies > 2 Then Return "Copies must be 1 or 2"
        If IsCalibrated AndAlso FullVolume <= 0 Then Return "Full volume must be greater than zero for calibrated dipsticks"

        Return String.Empty
    End Function

#End Region

#Region "Helper Methods"
    ''' <summary>
    ''' Calculates Safe Working Capacity (97% of full volume)
    ''' </summary>
    ''' <returns>SWC value</returns>
    Public Function CalculateSWC() As Decimal
        Return FullVolume * DipstickConstants.SWC_PERCENTAGE
    End Function

    ''' <summary>
    ''' Gets the X-offset for dipstick copies
    ''' </summary>
    ''' <returns>X-offset value</returns>
    Public Function GetCopyOffset() As Single
        If Copies = 1 Then
            Return DipstickConstants.SINGLE_COPY_X_OFFSET
        Else
            Return DipstickConstants.DUAL_COPY_X_OFFSET
        End If
    End Function

#End Region

End Class

End Namespace
