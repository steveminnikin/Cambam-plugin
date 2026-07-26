Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CamBamPlugin

Namespace CamBamPlugin.Tests

    <TestClass>
    Public Class DipstickModelTests

        Private Function ValidModel() As DipstickModel
            Return New DipstickModel With {
                .Height = 1000,
                .Increments = 25,
                .FullVolume = 2000,
                .IsCalibrated = True
            }
        End Function

        <TestMethod>
        Public Sub IsValid_ValidModel_ReturnsTrue()
            Assert.IsTrue(ValidModel().IsValid())
            Assert.AreEqual(String.Empty, ValidModel().GetValidationError())
        End Sub

        <TestMethod>
        Public Sub IsValid_ZeroHeight_ReturnsFalse()
            Dim model As DipstickModel = ValidModel()
            model.Height = 0

            Assert.IsFalse(model.IsValid())
            StringAssert.Contains(model.GetValidationError(), "Height")
        End Sub

        <TestMethod>
        Public Sub IsValid_ZeroIncrements_ReturnsFalse()
            Dim model As DipstickModel = ValidModel()
            model.Increments = 0

            Assert.IsFalse(model.IsValid())
            StringAssert.Contains(model.GetValidationError(), "Increments")
        End Sub

        <TestMethod>
        Public Sub IsValid_CalibratedWithoutFullVolume_ReturnsFalse()
            Dim model As DipstickModel = ValidModel()
            model.FullVolume = 0

            Assert.IsFalse(model.IsValid())
            StringAssert.Contains(model.GetValidationError(), "Full volume")
        End Sub

        <TestMethod>
        Public Sub IsValid_UncalibratedWithoutFullVolume_ReturnsTrue()
            Dim model As DipstickModel = ValidModel()
            model.IsCalibrated = False
            model.FullVolume = 0

            Assert.IsTrue(model.IsValid())
        End Sub

        <TestMethod>
        Public Sub CalculateSWC_Is97PercentOfFullVolume()
            Dim model As DipstickModel = ValidModel()
            model.FullVolume = 1000

            Assert.AreEqual(970D, model.CalculateSWC())
        End Sub

    End Class

End Namespace
