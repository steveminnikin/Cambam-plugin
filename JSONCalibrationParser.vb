Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.Web.Script.Serialization

Namespace CamBamPlugin

    ''' <summary>
    ''' Parses JSON calibration files exported from the tank calculator.
    ''' </summary>
    Public Class JSONCalibrationParser

        #Region "Public Methods"

        ''' <summary>
        ''' Parses a JSON calibration file and returns CalibrationData
        ''' </summary>
        ''' <param name="filePath">Full path to the JSON file</param>
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

            Try
                ' Read and parse JSON file
                Dim jsonContent As String = File.ReadAllText(filePath)
                Dim serializer As New JavaScriptSerializer()
                Dim jsonData As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(jsonContent)

                ' Extract metadata from JSON structure
                data.FullVolume = ExtractFullVolume(jsonData)
                data.Increments = ExtractIncrements(jsonData)
                data.TankType = ExtractTankType(jsonData)
                data.TankDimensions = ExtractTankDimensions(jsonData)
                data.TopHeight = ExtractTopHeight(jsonData)

                ' Parse increment data for volume/height pairs
                data.VolumeHeightPairs = ReadVolumeHeightPairs(jsonData, useRegularIncrements)

            Catch ex As Exception When TypeOf ex Is FileNotFoundException Or TypeOf ex Is ArgumentException
                ' Re-throw these specific exceptions
                Throw
            Catch ex As Exception
                Throw New FormatException("Failed to parse JSON calibration file. " & ex.Message, ex)
            End Try

            Return data
        End Function

        ''' <summary>
        ''' Reads volume/height pairs from JSON data
        ''' </summary>
        ''' <param name="filePath">Path to JSON file</param>
        ''' <param name="useRegularIncrements">If True, generates regular increments</param>
        ''' <returns>SortedList keyed by height (mm) with volume (litres) as value, sorted numerically</returns>
        Public Function ReadVolumeHeightPairs(filePath As String, useRegularIncrements As Boolean) As SortedList(Of Decimal, Decimal)
            If Not File.Exists(filePath) Then
                Throw New FileNotFoundException("JSON file not found", filePath)
            End If

            Dim jsonContent As String = File.ReadAllText(filePath)
            Dim serializer As New JavaScriptSerializer()
            Dim jsonData As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(jsonContent)

            Return ReadVolumeHeightPairs(jsonData, useRegularIncrements)
        End Function

#End Region

#Region "Private Parsing Methods"

        ''' <summary>
        ''' Extracts full volume from JSON results section
        ''' </summary>
        Private Function ExtractFullVolume(jsonData As Dictionary(Of String, Object)) As Integer
            If Not jsonData.ContainsKey("results") Then
                Throw New FormatException("JSON does not contain 'results' section")
            End If

            Dim results As Dictionary(Of String, Object) = CType(jsonData("results"), Dictionary(Of String, Object))

            If Not results.ContainsKey("fullVolume") Then
                Throw New FormatException("JSON results section does not contain 'fullVolume'")
            End If

            ' Handle both integer and double types from JSON deserialization
            Dim fullVolumeObj As Object = results("fullVolume")
            Return Convert.ToInt32(fullVolumeObj)
        End Function

        ''' <summary>
        ''' Extracts increments from JSON calculation section
        ''' </summary>
        Private Function ExtractIncrements(jsonData As Dictionary(Of String, Object)) As Integer
            If Not jsonData.ContainsKey("calculation") Then
                Throw New FormatException("JSON does not contain 'calculation' section")
            End If

            Dim calculation As Dictionary(Of String, Object) = CType(jsonData("calculation"), Dictionary(Of String, Object))

            If Not calculation.ContainsKey("increments") Then
                Throw New FormatException("JSON calculation section does not contain 'increments'")
            End If

            Dim incrementsObj As Object = calculation("increments")
            Return Convert.ToInt32(incrementsObj)
        End Function

        ''' <summary>
        ''' Extracts top height from JSON results section
        ''' </summary>
        Private Function ExtractTopHeight(jsonData As Dictionary(Of String, Object)) As Single
            If Not jsonData.ContainsKey("results") Then
                Throw New FormatException("JSON does not contain 'results' section")
            End If

            Dim results As Dictionary(Of String, Object) = CType(jsonData("results"), Dictionary(Of String, Object))

            If Not results.ContainsKey("topHeight") Then
                Throw New FormatException("JSON results section does not contain 'topHeight'")
            End If

            ' Handle both integer and double types from JSON deserialization
            Dim topHeightObj As Object = results("topHeight")
            Return Convert.ToSingle(topHeightObj)
        End Function

        ''' <summary>
        ''' Extracts tank type from JSON tank section
        ''' Returns tank type string like "Rectangular", "Horizontal Flat Ends", "Horizontal Dished Ends"
        ''' </summary>
        Private Function ExtractTankType(jsonData As Dictionary(Of String, Object)) As String
            Try
                If Not jsonData.ContainsKey("tank") Then
                    Return String.Empty
                End If

                Dim tank As Dictionary(Of String, Object) = CType(jsonData("tank"), Dictionary(Of String, Object))

                If Not tank.ContainsKey("type") Then
                    Return String.Empty
                End If

                Return Convert.ToString(tank("type"))
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Extracts tank dimensions from JSON tank section based on tank type.
        ''' Returns formatted string with underscore separators for use as filename.
        ''' - Rectangular: length_width_height (e.g., "1235_2545_1555")
        ''' - Horizontal Flat Ends: diameter_length (e.g., "2488_2999")
        ''' - Horizontal Dished Ends: diameter_stLength_dishEndRad_knuckleRad (e.g., "2488_2999_2500_70")
        '''   OR diameter_stLength_ovLength if ovLength is provided instead
        '''   Optional _tilt_dipPoint appended if those values are present
        ''' </summary>
        Private Function ExtractTankDimensions(jsonData As Dictionary(Of String, Object)) As String
            Try
                If Not jsonData.ContainsKey("tank") Then
                    Return String.Empty
                End If

                Dim tank As Dictionary(Of String, Object) = CType(jsonData("tank"), Dictionary(Of String, Object))

                If Not tank.ContainsKey("dimensions") Then
                    Return String.Empty
                End If

                Dim dimensions As Dictionary(Of String, Object) = CType(tank("dimensions"), Dictionary(Of String, Object))

                ' Get tank type to determine which dimensions to use
                Dim tankType As String = String.Empty
                If tank.ContainsKey("type") Then
                    tankType = Convert.ToString(tank("type"))
                End If

                Dim dimList As New List(Of String)()

                Select Case tankType
                    Case "Rectangular"
                        ' Rectangular: length_width_height
                        AddDimensionIfValid(dimList, dimensions, "length")
                        AddDimensionIfValid(dimList, dimensions, "width")
                        AddDimensionIfValid(dimList, dimensions, "height")

                    Case "Horizontal Flat Ends"
                        ' Horizontal Flat Ends: flatDiameter_flatLength
                        AddDimensionIfValid(dimList, dimensions, "flatDiameter")
                        AddDimensionIfValid(dimList, dimensions, "flatLength")

                    Case "Horizontal Dished Ends"
                        ' Horizontal Dished Ends: dishDiameter_stLength_dishEndRad_knuckleRad
                        ' OR dishDiameter_stLength_ovLength (if ovLength provided instead of dishEndRad/knuckleRad)
                        AddDimensionIfValid(dimList, dimensions, "dishDiameter")
                        AddDimensionIfValid(dimList, dimensions, "stLength")

                        ' Check if ovLength is provided (alternative to dishEndRad/knuckleRad)
                        If HasValidDimension(dimensions, "ovLength") Then
                            AddDimensionIfValid(dimList, dimensions, "ovLength")
                        Else
                            ' Use dishEndRad and knuckleRad
                            AddDimensionIfValid(dimList, dimensions, "dishEndRad")
                            AddDimensionIfValid(dimList, dimensions, "knuckleRad")
                        End If

                        ' Add optional tilt and dipPoint if present
                        AddDimensionIfValid(dimList, dimensions, "tilt")
                        AddDimensionIfValid(dimList, dimensions, "dipPoint")

                    Case Else
                        ' Fallback: try rectangular dimensions for backwards compatibility
                        AddDimensionIfValid(dimList, dimensions, "length")
                        AddDimensionIfValid(dimList, dimensions, "width")
                        AddDimensionIfValid(dimList, dimensions, "height")
                End Select

                If dimList.Count > 0 Then
                    Return String.Join("_", dimList.ToArray())
                End If

                Return String.Empty
            Catch ex As Exception
                ' Tank dimensions are optional, so return empty on any error
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Helper function to check if a dimension field exists and has a valid (non-null) value
        ''' </summary>
        Private Function HasValidDimension(dimensions As Dictionary(Of String, Object), fieldName As String) As Boolean
            If Not dimensions.ContainsKey(fieldName) Then
                Return False
            End If

            Dim value As Object = dimensions(fieldName)
            If value Is Nothing Then
                Return False
            End If

            ' Check if it's a numeric value (not zero for optional fields like tilt)
            Try
                Dim numValue As Double = Convert.ToDouble(value)
                Return True
            Catch
                ' For string values like dipPoint, check if not empty
                Dim strValue As String = Convert.ToString(value)
                Return Not String.IsNullOrWhiteSpace(strValue)
            End Try
        End Function

        ''' <summary>
        ''' Helper function to add a dimension value to the list if it exists and is valid
        ''' </summary>
        Private Sub AddDimensionIfValid(dimList As List(Of String), dimensions As Dictionary(Of String, Object), fieldName As String)
            If Not HasValidDimension(dimensions, fieldName) Then
                Return
            End If

            Dim value As Object = dimensions(fieldName)

            ' Try to convert to integer for numeric values
            Try
                Dim numValue As Double = Convert.ToDouble(value)
                ' Use integer if it's a whole number, otherwise keep decimal
                If numValue = Math.Floor(numValue) Then
                    dimList.Add(Convert.ToInt32(numValue).ToString())
                Else
                    dimList.Add(numValue.ToString())
                End If
            Catch
                ' For string values (like dipPoint), add as-is
                dimList.Add(Convert.ToString(value))
            End Try
        End Sub

        ''' <summary>
        ''' Reads volume/height pairs from parsed JSON data
        ''' </summary>
        ''' <param name="jsonData">Parsed JSON data dictionary</param>
        ''' <param name="useRegularIncrements">If True, generates regular increments</param>
        ''' <returns>SortedList keyed by height (mm) with volume (litres) as value, sorted numerically</returns>
        Private Function ReadVolumeHeightPairs(jsonData As Dictionary(Of String, Object), useRegularIncrements As Boolean) As SortedList(Of Decimal, Decimal)
            Dim pairs As New SortedList(Of Decimal, Decimal)()

            If Not jsonData.ContainsKey("incrementData") Then
                Throw New FormatException("JSON does not contain 'incrementData' array")
            End If

            ' JavaScriptSerializer deserializes JSON arrays as ArrayList, not Object()
            Dim incrementData As ArrayList = CType(jsonData("incrementData"), ArrayList)

            If incrementData.Count = 0 Then
                Throw New FormatException("JSON incrementData array is empty")
            End If

            For Each item As Object In incrementData
                Dim dataPoint As Dictionary(Of String, Object) = CType(item, Dictionary(Of String, Object))

                If Not dataPoint.ContainsKey("volume") OrElse Not dataPoint.ContainsKey("height") Then
                    Continue For ' Skip invalid entries
                End If

                ' Extract volume and height as numbers so entries sort numerically
                ' (string keys sorted "100" before "20"); volumes are engraved as whole litres
                Dim volume As Decimal = Convert.ToInt32(dataPoint("volume"))
                Dim height As Decimal = Convert.ToDecimal(dataPoint("height"))

                ' Add to sorted list: Key = height, Value = volume
                ' This matches the format expected by CalForm
                Try
                    pairs.Add(height, volume)
                Catch ex As ArgumentException
                    ' Skip duplicate heights
                    Continue For
                End Try
            Next

            If pairs.Count = 0 Then
                Throw New FormatException("No valid volume/height pairs found in JSON")
            End If

            Return pairs
        End Function

        #End Region

    End Class

End Namespace
