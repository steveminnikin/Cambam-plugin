# Feature Backlog

**Created:** 2026-01-25
**Last Updated:** 2026-01-26

This document tracks planned features and enhancements for the CamBam Dipstick Plugin.

---

## Pending Features

### 1. Automatic CAD Object Naming Based on Tank Dimensions
**Status:** Not Started
**Priority:** TBD
**Description:** Automatically name/label CAD objects based on the tank dimensions (e.g., height, volume, diameter) to improve organization and identification in CamBam.

**Potential Implementation:**
- Extract tank dimensions from JSON calibration data or user input
- Generate descriptive names like "Dipstick_1500mm_5000L" or similar
- Apply to CADFile, Layer, or Part names

---

## Completed Features

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
