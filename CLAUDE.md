# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a CamBam plugin written in Visual Basic .NET that generates dipstick CAD files with engraving toolpaths. The plugin integrates into CamBam (a CAD/CAM software) and provides three workflows for creating dipstick designs:

1. **Calibrated dipsticks** - Uses calibration data from JSON files to create accurate volume markings
2. **Uncalibrated dipsticks** - Creates dipsticks with regular increments without calibration data
3. **Text-only dipsticks** - Generates reference text without measurement markings

## Build and Development

### Building the Project
```bash
# Build using MSBuild (Visual Studio required)
msbuild CamBamPlugin.vbproj /p:Configuration=Debug
```

The Debug configuration outputs directly to: `C:\Program Files (x86)\CamBam plus 0.9.8\plugins\`

### Dependencies
- **Target Framework:** .NET Framework 4.8
- **Platform:** x86 (32-bit)
- **External References:**
  - `CamBam.CAD.dll` - Located in CamBam installation directory
  - `CamBam.Geom.dll` - Located in CamBam installation directory
  - System assemblies (Windows.Forms, Drawing, Web.Extensions, etc.)

### Solution Structure
- `CamBamPlugin.sln` - Visual Studio solution file
- `CamBamPlugin.vbproj` - Project file (VS2012+ compatible)

## Architecture

### Plugin Entry Point
`MyPlugin.vb` contains the `InitPlugin` method which CamBam calls on plugin load. This method:
- Receives the CamBam UI reference (`CamBamUI`)
- Creates a "Dipsticks" menu with three submenu items
- Wires up event handlers to show the appropriate form

### Core Components

**Forms (UI Layer):**
- `CalForm.vb` - Calibrated dipstick generation with JSON file input
- `UnCalForm.vb` - Uncalibrated dipstick with manual increment settings
- `textForm.vb` - Text-only dipstick generation

**Business Logic:**
- `CommonDetails.vb` - Shared functionality and CAD object creation
  - Factory methods for creating CADFile, Layer, CAMPart, and MOPEngrave objects
  - Text rendering methods (`WriteRef`, `WriteUnits`, `WriteClientRef`, `WriteVerticalInfo`)
  - Engraving operation setup for both laser and spindle engraving
- `JSONCalibrationParser.vb` - Parses JSON calibration files
  - Extracts tank metadata (dimensions, full volume, increments)
  - Reads volume/height pairs from incrementData array
  - Returns SortedList for dipstick generation

### Data Flow

1. User selects a dipstick type from the CamBam Plugins menu
2. Appropriate form displays and collects user input
3. On submit, form instantiates `CommonDetails` with form data
4. Form calls `myUI.FileNew()` to create a fresh CAD document
5. Form creates CADFile, Layer, and CAMPart using `CommonDetails` factory methods
6. Form generates geometry (polylines, text) and adds to active CAD document
7. Form calls `myUI.ActiveView.RefreshView()` to update CamBam display

### Key Design Patterns

**Calibrated Workflow (`CalForm`):**
- Reads JSON files with volume/height pairs and tank metadata
- Extracts tank details, full volume, and increments from JSON structure
- Supports both regular increments and volume-based markings
- Marks only specified volume intervals (e.g., every 100L)
- JSON format includes: exportVersion, tank dimensions, calculation settings, results, and incrementData array

**Uncalibrated Workflow (`UnCalForm`):**
- Generates evenly-spaced lines at user-defined increments
- Supports multiple unit systems (mm, cm, inches) with automatic conversion
- Optional half-increment markings
- Unit conversion properties handle display vs. internal units

**Shared CAM Configuration:**
- Engraving operations configured for both laser and spindle
- Laser: shallow depth (0.01mm), 500mm/min feed rate
- Spindle: 0.45mm depth, 500mm/min feed rate
- Uses "1CamBam_Stick_3" font for all text
- Velocity mode set to ExactStop for precise engraving

### Text Positioning System

The codebase uses a coordinate-based positioning system where Y-coordinates are calculated relative to dipstick height:
- Main reference text: `fullVolHeight + 40`
- Units text: `height + 14`
- SWC text: `height + 76`
- Client reference: `height + 105`
- Vertical info text is rotated 90 degrees (RotZ(1.571)) and centered

### Important Constants and Conventions

- Font: "1CamBam_Stick_3" (CamBam stick font for engraving)
- Default text heights: 5.5mm (5mm for large numbers >99999)
- Tool number: 10
- Tool diameter: 1.0mm
- X-offset for copies: 0 (single) or 30 (dual)
- SWC (Safe Working Capacity): 97% of full volume

## Common Development Patterns

### Adding New CAD Objects
All CAD objects are added through `myUI.ActiveView.CADFile.Add()`. The pattern is:
1. Create object (MText, Polyline, etc.)
2. Configure properties
3. Add to CADFile via UI reference

### Creating Engraving Operations
Use `CommonDetails.CreateEngraving(ref, isLaser)` which returns a configured `MOPEngrave` object with appropriate parameters for laser or spindle engraving.

### Handling Multiple Dipstick Copies
`CommonDetails.CreateCopies(n)` returns X-offset: 0 for single, 30 for dual copies.
