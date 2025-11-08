Namespace CamBamPlugin

''' <summary>
''' Constants used throughout the dipstick generation plugin.
''' Centralizes all magic numbers for easier maintenance.
''' </summary>
Public Class DipstickConstants

#Region "CAM Configuration"
    ''' <summary>
    ''' CamBam stick font used for all engraving text
    ''' </summary>
    Public Const FONT_NAME As String = "1CamBam_Stick_3"

    ''' <summary>
    ''' Tool number used for engraving operations
    ''' </summary>
    Public Const TOOL_NUMBER As Integer = 10

    ''' <summary>
    ''' Tool diameter in millimeters
    ''' </summary>
    Public Const TOOL_DIAMETER As Double = 1.0

    ''' <summary>
    ''' Name of the CAM part created in CamBam
    ''' </summary>
    Public Const PART_NAME As String = "Dipstick Machine Part"

#End Region

#Region "Engraving Settings"
    ''' <summary>
    ''' Feed rate for laser engraving in mm/min
    ''' </summary>
    Public Const LASER_FEED_RATE As Double = 500.0

    ''' <summary>
    ''' Feed rate for spindle engraving in mm/min
    ''' </summary>
    Public Const SPINDLE_FEED_RATE As Double = 500.0

    ''' <summary>
    ''' Depth increment for laser engraving in mm
    ''' </summary>
    Public Const LASER_DEPTH_INCREMENT As Double = 0.01

    ''' <summary>
    ''' Depth increment for spindle engraving in mm
    ''' </summary>
    Public Const SPINDLE_DEPTH_INCREMENT As Double = 0.45

#End Region

#Region "Text Positioning"
    ''' <summary>
    ''' Y-axis offset above full volume height for reference text
    ''' </summary>
    Public Const REF_Y_OFFSET As Single = 40

    ''' <summary>
    ''' Y-axis offset above dipstick height for units text
    ''' </summary>
    Public Const UNITS_Y_OFFSET As Single = 14

    ''' <summary>
    ''' Y-axis offset above dipstick height for SWC text
    ''' </summary>
    Public Const SWC_Y_OFFSET As Single = 76

    ''' <summary>
    ''' Y-axis offset above dipstick height for client reference text
    ''' </summary>
    Public Const CLIENT_REF_Y_OFFSET As Single = 105

    ''' <summary>
    ''' Default text height in millimeters
    ''' </summary>
    Public Const DEFAULT_TEXT_HEIGHT As Single = 5.5

    ''' <summary>
    ''' Text height for large numbers (greater than 99999)
    ''' </summary>
    Public Const LARGE_NUMBER_TEXT_HEIGHT As Single = 5.0

    ''' <summary>
    ''' Rotation angle for vertical text (90 degrees in radians)
    ''' </summary>
    Public Const VERTICAL_TEXT_ROTATION As Double = 1.571

#End Region

#Region "Geometry Settings"
    ''' <summary>
    ''' Length of measurement line markings in millimeters
    ''' </summary>
    Public Const LINE_LENGTH As Single = 20

    ''' <summary>
    ''' Length of half-increment line markings in millimeters
    ''' </summary>
    Public Const HALF_LINE_LENGTH As Single = 15

    ''' <summary>
    ''' X-offset for single dipstick copy
    ''' </summary>
    Public Const SINGLE_COPY_X_OFFSET As Single = 0

    ''' <summary>
    ''' X-offset for dual dipstick copies
    ''' </summary>
    Public Const DUAL_COPY_X_OFFSET As Single = 30

#End Region

#Region "Unit Conversions"
    ''' <summary>
    ''' Millimeters per centimeter
    ''' </summary>
    Public Const MM_PER_CM As Single = 10.0

    ''' <summary>
    ''' Millimeters per inch
    ''' </summary>
    Public Const MM_PER_INCH As Single = 25.4

#End Region

#Region "Number Positioning"
    ''' <summary>
    ''' X-offset for numbers when volume is less than or equal to 9999
    ''' </summary>
    Public Const NUMBER_X_OFFSET_SMALL As Single = 3

    ''' <summary>
    ''' X-offset for numbers when volume is greater than 9999 but less than or equal to 99999
    ''' </summary>
    Public Const NUMBER_X_OFFSET_MEDIUM As Single = 0.5

    ''' <summary>
    ''' X-offset for numbers when volume is greater than 99999
    ''' </summary>
    Public Const NUMBER_X_OFFSET_LARGE As Single = -2

    ''' <summary>
    ''' Threshold for medium-sized numbers
    ''' </summary>
    Public Const MEDIUM_NUMBER_THRESHOLD As Integer = 9999

    ''' <summary>
    ''' Threshold for large numbers
    ''' </summary>
    Public Const LARGE_NUMBER_THRESHOLD As Integer = 99999

#End Region

#Region "Safe Working Capacity"
    ''' <summary>
    ''' Safe Working Capacity as percentage of full volume (0.97 or 97 percent)
    ''' </summary>
    Public Const SWC_PERCENTAGE As Double = 0.97

#End Region

#Region "Half-Increment X-Offsets (Unit-Specific)"
    ''' <summary>
    ''' X-offset for half-increment marks in millimeters
    ''' </summary>
    Public Const HALF_INC_OFFSET_MM As Single = 15

    ''' <summary>
    ''' X-offset for half-increment marks in centimeters
    ''' </summary>
    Public Const HALF_INC_OFFSET_CM As Single = 12

    ''' <summary>
    ''' X-offset for half-increment marks in inches
    ''' </summary>
    Public Const HALF_INC_OFFSET_INCH As Single = 10

#End Region

End Class

End Namespace
