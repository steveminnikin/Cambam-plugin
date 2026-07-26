Imports System
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CamBamPlugin

Namespace CamBamPlugin.Tests

    <TestClass>
    Public Class CalibrationDataTests

        Private Function DataWithPairs() As CalibrationData
            Dim data As New CalibrationData With {
                .FullVolume = 2000,
                .Increments = 100
            }
            data.VolumeHeightPairs.Add(9.5D, 100D)
            data.VolumeHeightPairs.Add(45D, 500D)
            data.VolumeHeightPairs.Add(180.5D, 2000D)
            Return data
        End Function

        <TestMethod>
        Public Sub GetHeightForVolume_KnownVolume_ReturnsHeight()
            Assert.AreEqual(45D, DataWithPairs().GetHeightForVolume(500D))
        End Sub

        <TestMethod>
        Public Sub GetHeightForVolume_UnknownVolume_ReturnsZero()
            Assert.AreEqual(0D, DataWithPairs().GetHeightForVolume(999D))
        End Sub

        <TestMethod>
        Public Sub GetFullVolumeHeight_ReturnsHeightOfFullVolume()
            Assert.AreEqual(180.5F, DataWithPairs().GetFullVolumeHeight())
        End Sub

        <TestMethod>
        Public Sub IsValid_WithPairsAndMetadata_ReturnsTrue()
            Assert.IsTrue(DataWithPairs().IsValid())
        End Sub

        <TestMethod>
        Public Sub IsValid_NoPairs_ReturnsFalse()
            Dim data As New CalibrationData With {
                .FullVolume = 2000,
                .Increments = 100
            }
            Assert.IsFalse(data.IsValid())
        End Sub

        <TestMethod>
        Public Sub IsValid_ZeroFullVolume_ReturnsFalse()
            Dim data As CalibrationData = DataWithPairs()
            data.FullVolume = 0
            Assert.IsFalse(data.IsValid())
        End Sub

        <TestMethod>
        Public Sub GetPointCount_ReturnsPairCount()
            Assert.AreEqual(3, DataWithPairs().GetPointCount())
        End Sub

    End Class

End Namespace
