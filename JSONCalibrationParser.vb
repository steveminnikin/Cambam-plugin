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
                data.TankDimensions = ExtractTankDimensions(jsonData)

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
        ''' <returns>SortedList with height as key and volume as value</returns>
        Public Function ReadVolumeHeightPairs(filePath As String, useRegularIncrements As Boolean) As SortedList(Of String, String)
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
        ''' Extracts tank dimensions from JSON tank section
        ''' Returns formatted string like "5415x1542x1545"
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

                ' Build dimension string based on available properties
                Dim dimList As New List(Of String)()

                If dimensions.ContainsKey("length") AndAlso dimensions("length") IsNot Nothing Then
                    dimList.Add(Convert.ToInt32(dimensions("length")).ToString())
                End If

                If dimensions.ContainsKey("width") AndAlso dimensions("width") IsNot Nothing Then
                    dimList.Add(Convert.ToInt32(dimensions("width")).ToString())
                End If

                If dimensions.ContainsKey("height") AndAlso dimensions("height") IsNot Nothing Then
                    dimList.Add(Convert.ToInt32(dimensions("height")).ToString())
                End If

                If dimList.Count > 0 Then
                    Return String.Join("x", dimList.ToArray())
                End If

                Return String.Empty
            Catch ex As Exception
                ' Tank dimensions are optional, so return empty on any error
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Reads volume/height pairs from parsed JSON data
        ''' </summary>
        ''' <param name="jsonData">Parsed JSON data dictionary</param>
        ''' <param name="useRegularIncrements">If True, generates regular increments</param>
        ''' <returns>SortedList with height as key and volume as value</returns>
        Private Function ReadVolumeHeightPairs(jsonData As Dictionary(Of String, Object), useRegularIncrements As Boolean) As SortedList(Of String, String)
            Dim pairs As New SortedList(Of String, String)()

            If Not jsonData.ContainsKey("incrementData") Then
                Throw New FormatException("JSON does not contain 'incrementData' array")
            End If

            Dim incrementData As Object() = CType(jsonData("incrementData"), Object())

            If incrementData.Length = 0 Then
                Throw New FormatException("JSON incrementData array is empty")
            End If

            For Each item As Object In incrementData
                Dim dataPoint As Dictionary(Of String, Object) = CType(item, Dictionary(Of String, Object))

                If Not dataPoint.ContainsKey("volume") OrElse Not dataPoint.ContainsKey("height") Then
                    Continue For ' Skip invalid entries
                End If

                ' Extract volume and height
                Dim volume As String = Convert.ToInt32(dataPoint("volume")).ToString()
                Dim height As String = Convert.ToDouble(dataPoint("height")).ToString()

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
