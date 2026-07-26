Imports System
Imports System.IO
Imports System.Text

Namespace CamBamPlugin.Tests

    ''' <summary>
    ''' Builds JSON calibration files on disk for parser tests.
    ''' </summary>
    Public Class JsonTestFiles

        ''' <summary>
        ''' Writes a JSON calibration file and returns its path.
        ''' Pass Nothing for a section to omit it entirely.
        ''' </summary>
        Public Shared Function WriteJson(directory As String,
                                         Optional tankSection As String = Nothing,
                                         Optional calculationSection As String = "{ ""increments"": 100 }",
                                         Optional resultsSection As String = "{ ""fullVolume"": 2000, ""topHeight"": 180.5 }",
                                         Optional incrementDataSection As String = Nothing) As String
            If tankSection Is Nothing Then
                tankSection = "{ ""type"": ""Rectangular"", ""dimensions"": { ""length"": 1235, ""width"": 2545, ""height"": 1555 } }"
            End If
            If incrementDataSection Is Nothing Then
                ' Heights deliberately out of lexicographic order: as strings,
                ' "100" would sort before "45" and "9.5"
                incrementDataSection = "[" &
                    "{ ""volume"": 2000, ""height"": 180.5 }," &
                    "{ ""volume"": 100, ""height"": 9.5 }," &
                    "{ ""volume"": 1000, ""height"": 100 }," &
                    "{ ""volume"": 500, ""height"": 45 }" &
                    "]"
            End If

            Dim json As New StringBuilder()
            json.Append("{")
            json.Append("""exportVersion"": ""1.0"",")
            If tankSection <> "" Then json.Append("""tank"": ").Append(tankSection).Append(",")
            If calculationSection <> "" Then json.Append("""calculation"": ").Append(calculationSection).Append(",")
            If resultsSection <> "" Then json.Append("""results"": ").Append(resultsSection).Append(",")
            If incrementDataSection <> "" Then json.Append("""incrementData"": ").Append(incrementDataSection).Append(",")
            ' Remove trailing comma
            If json(json.Length - 1) = ","c Then json.Length -= 1
            json.Append("}")

            Dim path As String = IO.Path.Combine(directory, Guid.NewGuid().ToString("N") & ".json")
            File.WriteAllText(path, json.ToString())
            Return path
        End Function

    End Class

End Namespace
