Imports System
Imports System.Collections.Generic
Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CamBamPlugin

Namespace CamBamPlugin.Tests

    <TestClass>
    Public Class JSONCalibrationParserTests

        Private tempDir As String
        Private parser As JSONCalibrationParser

        <TestInitialize>
        Public Sub Setup()
            tempDir = Path.Combine(Path.GetTempPath(), "DipstickTests_" & Guid.NewGuid().ToString("N"))
            Directory.CreateDirectory(tempDir)
            parser = New JSONCalibrationParser()
        End Sub

        <TestCleanup>
        Public Sub Cleanup()
            If Directory.Exists(tempDir) Then Directory.Delete(tempDir, True)
        End Sub

        <TestMethod>
        Public Sub Parse_ValidFile_ExtractsMetadata()
            Dim file As String = JsonTestFiles.WriteJson(tempDir)

            Dim data As CalibrationData = parser.Parse(file, False)

            Assert.AreEqual(2000, data.FullVolume)
            Assert.AreEqual(100, data.Increments)
            Assert.AreEqual(180.5F, data.TopHeight)
            Assert.AreEqual("Rectangular", data.TankType)
            Assert.AreEqual("1235_2545_1555", data.TankDimensions)
            Assert.AreEqual(4, data.VolumeHeightPairs.Count)
        End Sub

        <TestMethod>
        Public Sub ReadVolumeHeightPairs_SortsNumericallyByHeight()
            ' Regression test: string keys sorted "100" before "45" and "9.5"
            Dim file As String = JsonTestFiles.WriteJson(tempDir)

            Dim pairs As SortedList(Of Decimal, Decimal) = parser.ReadVolumeHeightPairs(file, False)

            Dim expectedHeights() As Decimal = {9.5D, 45D, 100D, 180.5D}
            Dim expectedVolumes() As Decimal = {100D, 500D, 1000D, 2000D}
            CollectionAssert.AreEqual(expectedHeights, New List(Of Decimal)(pairs.Keys))
            CollectionAssert.AreEqual(expectedVolumes, New List(Of Decimal)(pairs.Values))
        End Sub

        <TestMethod>
        Public Sub ReadVolumeHeightPairs_SkipsDuplicateHeights()
            Dim file As String = JsonTestFiles.WriteJson(tempDir,
                incrementDataSection:="[" &
                    "{ ""volume"": 100, ""height"": 10 }," &
                    "{ ""volume"": 200, ""height"": 10 }," &
                    "{ ""volume"": 300, ""height"": 30 }" &
                    "]")

            Dim pairs As SortedList(Of Decimal, Decimal) = parser.ReadVolumeHeightPairs(file, False)

            Assert.AreEqual(2, pairs.Count)
            Assert.AreEqual(100D, pairs(10D))
            Assert.AreEqual(300D, pairs(30D))
        End Sub

        <TestMethod>
        Public Sub ReadVolumeHeightPairs_SkipsEntriesMissingVolumeOrHeight()
            Dim file As String = JsonTestFiles.WriteJson(tempDir,
                incrementDataSection:="[" &
                    "{ ""volume"": 100 }," &
                    "{ ""height"": 20 }," &
                    "{ ""volume"": 300, ""height"": 30 }" &
                    "]")

            Dim pairs As SortedList(Of Decimal, Decimal) = parser.ReadVolumeHeightPairs(file, False)

            Assert.AreEqual(1, pairs.Count)
            Assert.AreEqual(300D, pairs(30D))
        End Sub

        <TestMethod>
        Public Sub ReadVolumeHeightPairs_MissingIncrementData_Throws()
            Dim file As String = JsonTestFiles.WriteJson(tempDir, incrementDataSection:="")

            Assert.ThrowsException(Of FormatException)(
                Sub() parser.ReadVolumeHeightPairs(file, False))
        End Sub

        <TestMethod>
        Public Sub ReadVolumeHeightPairs_EmptyIncrementData_Throws()
            Dim file As String = JsonTestFiles.WriteJson(tempDir, incrementDataSection:="[]")

            Assert.ThrowsException(Of FormatException)(
                Sub() parser.ReadVolumeHeightPairs(file, False))
        End Sub

        <TestMethod>
        Public Sub Parse_MissingFullVolume_Throws()
            Dim file As String = JsonTestFiles.WriteJson(tempDir,
                resultsSection:="{ ""topHeight"": 180.5 }")

            Assert.ThrowsException(Of FormatException)(
                Sub() parser.Parse(file, False))
        End Sub

        <TestMethod>
        Public Sub Parse_MissingFile_Throws()
            Assert.ThrowsException(Of FileNotFoundException)(
                Sub() parser.Parse(Path.Combine(tempDir, "missing.json"), False))
        End Sub

        <TestMethod>
        Public Sub Parse_HorizontalFlatEnds_BuildsDimensionString()
            Dim file As String = JsonTestFiles.WriteJson(tempDir,
                tankSection:="{ ""type"": ""Horizontal Flat Ends"", ""dimensions"": { ""flatDiameter"": 2488, ""flatLength"": 2999 } }")

            Dim data As CalibrationData = parser.Parse(file, False)

            Assert.AreEqual("Horizontal Flat Ends", data.TankType)
            Assert.AreEqual("2488_2999", data.TankDimensions)
        End Sub

        <TestMethod>
        Public Sub Parse_DishedEndsWithRadii_BuildsDimensionString()
            Dim file As String = JsonTestFiles.WriteJson(tempDir,
                tankSection:="{ ""type"": ""Horizontal Dished Ends"", ""dimensions"": { ""dishDiameter"": 2488, ""stLength"": 2999, ""dishEndRad"": 2500, ""knuckleRad"": 70 } }")

            Dim data As CalibrationData = parser.Parse(file, False)

            Assert.AreEqual("2488_2999_2500_70", data.TankDimensions)
        End Sub

        <TestMethod>
        Public Sub Parse_DishedEndsWithOvLength_PrefersOvLength()
            Dim file As String = JsonTestFiles.WriteJson(tempDir,
                tankSection:="{ ""type"": ""Horizontal Dished Ends"", ""dimensions"": { ""dishDiameter"": 2488, ""stLength"": 2999, ""ovLength"": 3500, ""dishEndRad"": 2500, ""knuckleRad"": 70 } }")

            Dim data As CalibrationData = parser.Parse(file, False)

            Assert.AreEqual("2488_2999_3500", data.TankDimensions)
        End Sub

        <TestMethod>
        Public Sub Parse_MissingTankSection_LeavesDimensionsEmpty()
            Dim file As String = JsonTestFiles.WriteJson(tempDir, tankSection:="")

            Dim data As CalibrationData = parser.Parse(file, False)

            Assert.AreEqual(String.Empty, data.TankType)
            Assert.AreEqual(String.Empty, data.TankDimensions)
        End Sub

    End Class

End Namespace
