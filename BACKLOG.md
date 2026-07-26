# Feature Backlog

**Created:** 2026-01-25
**Last Updated:** 2026-01-26

This document tracks planned features and enhancements for the CamBam Dipstick Plugin.

---

## Pending Features

*No pending features at this time.*

---

## Completed Features

### 1. Automatic CAD Object Naming Based on Tank Dimensions
**Status:** Completed (2026-01-26)
**Priority:** High
**Description:** Automatically name CAD files based on tank shape and dimensions for easy identification. The filename is generated based on tank type:

**Naming Conventions:**
| Tank Type | Format | Example |
|-----------|--------|---------|
| Rectangular | length_width_height | `1235_2545_1555.nc` |
| Horizontal Flat Ends | diameter_length | `2488_2999.nc` |
| Horizontal Dished Ends | diameter_stLength_dishEndRad_knuckleRad | `2488_2999_2500_70.nc` |
| (with tilt/dipPoint) | ...add _tilt_dipPoint if present | `2488_2999_2500_70_5_center.nc` |
| (with ovLength) | diameter_stLength_ovLength | `2488_2999_3500.nc` |

**Implementation:**
- `JSONCalibrationParser.vb` - Updated `ExtractTankDimensions()` to detect tank type and extract appropriate dimension fields
- `JSONCalibrationParser.vb` - Added `ExtractTankType()` helper method
- `JSONCalibrationParser.vb` - Added `HasValidDimension()` and `AddDimensionIfValid()` helper methods
- `CalibrationData.vb` - Added `TankType` property

**JSON Fields Used:**
- **Rectangular:** `length`, `width`, `height`
- **Horizontal Flat Ends:** `flatDiameter`, `flatLength`
- **Horizontal Dished Ends:** `dishDiameter`, `stLength`, `dishEndRad`, `knuckleRad`, `ovLength` (optional alternative), `tilt` (optional), `dipPoint` (optional)

---

### 2. Excel Document for Manual Calibration Data Entry
**Status:** Completed (2026-01-26)
**Priority:** High
**Description:** Created an Excel spreadsheet/template that allows users to manually enter height and volume pairs, which outputs the required JSON file format for the plugin to read.

**Implementation:**
- `DipstickCalibrationTemplate.xlsm` - Excel template with structured data entry sections
- `JSONExport.bas` - VBA module for exporting to JSON format
- `CreateExcelTemplate.ps1` - PowerShell script for template generation
- `EXCEL_TEMPLATE_README.md` - Setup and usage instructions

**Features Delivered:**
- Excel template with columns for volume (L) and height (mm)
- Validation of required fields before export
- VBA macro exports properly formatted JSON
- Includes tank metadata fields (dimensions, full volume, increments)
- Clear instructions for importing VBA module and using template

---

## Notes

- See `BUGS.md` for bug tracking
- See `UI_UX_IMPROVEMENTS.md` for UI/UX specific improvements
