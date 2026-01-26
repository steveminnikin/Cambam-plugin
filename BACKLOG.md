# Feature Backlog

**Created:** 2026-01-25
**Last Updated:** 2026-01-25

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

### 2. Excel Document for Manual Calibration Data Entry
**Status:** Not Started
**Priority:** TBD
**Description:** Create an Excel spreadsheet/template that allows users to manually enter height and volume pairs, which then outputs the required JSON file format for the plugin to read.

**Requirements:**
- Excel template with columns for height (mm) and volume (L)
- Validation of entered data
- Export functionality to generate properly formatted JSON
- Include tank metadata fields (dimensions, full volume, increments)

**Potential Approaches:**
- VBA macro in Excel to export JSON
- Standalone converter utility
- Excel formula-based JSON generation

---

## Completed Features

*(None yet)*

---

## Notes

- See `BUGS.md` for bug tracking
- See `UI_UX_IMPROVEMENTS.md` for UI/UX specific improvements
