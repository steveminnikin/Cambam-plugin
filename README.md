# CamBam Dipstick Plugin

A CamBam plugin written in Visual Basic .NET that generates dipstick CAD files with engraving toolpaths. This plugin integrates into CamBam (CAD/CAM software) and provides three workflows for creating dipstick designs.

## Features

- **Calibrated Dipsticks**: Uses calibration data from CSV files to create accurate volume markings
- **Uncalibrated Dipsticks**: Creates dipsticks with regular increments without calibration data
- **Text-only Dipsticks**: Generates reference text without measurement markings
- Supports both laser and spindle engraving operations
- Multiple unit systems (mm, cm, inches)
- Configurable text positioning and formatting

## Prerequisites

- **Visual Studio 2012 or later** (with VB.NET support)
- **.NET Framework 4.8** (or compatible version)
- **CamBam Plus 1.0** installed at `C:\Program Files (x86)\CamBam plus 1.0\`
- **Platform**: x86 (32-bit)

## Building the Project

### Using Visual Studio
1. Open `CamBamPlugin.sln` in Visual Studio
2. Select **Debug** or **Release** configuration
3. Build the solution (Ctrl+Shift+B)

### Using MSBuild (Command Line)
```bash
msbuild CamBamPlugin.vbproj /p:Configuration=Debug
```

### Build Output
- **Debug**: Outputs DLL directly to `C:\Program Files (x86)\CamBam plus 1.0\plugins\`
- **Release**: Outputs to `bin\Release\`

## Debugging in Visual Studio

### Quick Start (F5 Debugging)

The project is already configured to launch CamBam when you press F5. Here's what happens:

1. **Build**: Visual Studio builds the plugin DLL
2. **Deploy**: The DLL is copied to CamBam's plugins folder
3. **Launch**: CamBam.exe starts automatically
4. **Debug**: Visual Studio attaches to the CamBam process

### Step-by-Step Debugging Instructions

1. **Set Breakpoints**
   - Open the file you want to debug (e.g., `CalForm.vb`, `CommonDetails.vb`)
   - Click in the left margin to set breakpoints on desired lines

2. **Start Debugging**
   - Press **F5** or click **Debug > Start Debugging**
   - CamBam will launch automatically
   - Visual Studio will show "Debugging" in the title bar

3. **Trigger the Plugin**
   - In CamBam, go to **Plugins > Dipsticks**
   - Select the dipstick type you want to test:
     - **Calibrated Dipstick** (CalForm)
     - **UnCalibrated Dipstick** (UnCalForm)
     - **Text Dipstick** (textForm)

4. **Debug**
   - When your breakpoint is hit, execution will pause
   - Use F10 (Step Over), F11 (Step Into), F5 (Continue)
   - Inspect variables in the Locals/Watch windows

5. **Stop Debugging**
   - Close CamBam, or press Shift+F5 in Visual Studio

### Important Notes

- **Close CamBam before rebuilding**: If CamBam is running, the DLL file is locked and the build will fail with a "file copy" error
- **CamBam must be installed**: The debug configuration expects CamBam at `C:\Program Files (x86)\CamBam plus 1.0\CamBam.exe`
- **Plugin loads on startup**: CamBam loads plugins from the `plugins\` folder automatically

### Troubleshooting Debug Issues

| Issue | Solution |
|-------|----------|
| Build fails with "cannot copy file" | Close CamBam completely and rebuild |
| CamBam doesn't launch | Check that CamBam is installed at the expected path |
| Breakpoints not hit | Ensure you're building in Debug mode (not Release) |
| Plugin menu doesn't appear | Check that DLL was copied to plugins folder |

### Manual Attach (Alternative Method)

If F5 debugging doesn't work:

1. Start CamBam manually
2. In Visual Studio: **Debug > Attach to Process**
3. Find `CamBam.exe` in the process list
4. Click **Attach**
5. Use the plugin in CamBam to trigger breakpoints

## Project Structure

```
CamBamPlugin/
├── Forms (UI Layer)
│   ├── CalForm.vb              # Calibrated dipstick form
│   ├── UnCalForm.vb            # Uncalibrated dipstick form
│   ├── textForm.vb             # Text-only dipstick form
│   └── *.Designer.vb           # Form designer files
│
├── Business Logic
│   ├── CommonDetails.vb        # Shared CAD creation utilities
│   ├── DipstickConstants.vb    # Constants and configuration
│   ├── DipstickModel.vb        # Data model
│   ├── CalibrationData.vb      # Calibration data model
│   └── CalibratedDipstickParser.vb  # CSV parsing logic
│
├── Plugin Entry Point
│   └── MyPlugin.vb             # CamBam plugin initialization
│
├── Documentation
│   ├── README.md               # This file
│   ├── CLAUDE.md               # Detailed technical documentation
│   └── BUGS.md                 # Known bugs and fixes
│
└── Project Files
    ├── CamBamPlugin.sln        # Visual Studio solution
    └── CamBamPlugin.vbproj     # Project file
```

## Development Workflow

### Making Changes

1. **Checkout dev branch**: `git checkout dev`
2. **Make your changes** in Visual Studio
3. **Build and test**: Press F5 to debug in CamBam
4. **Commit changes**: `git commit -m "Description"`

### Common Development Tasks

#### Adding a New Constant
1. Open `DipstickConstants.vb`
2. Add your constant with descriptive name
3. Use it throughout the codebase instead of magic numbers

#### Modifying CAD Generation
1. Edit methods in `CommonDetails.vb`
2. Common methods:
   - `WriteRef()` - Reference text
   - `WriteUnits()` - Unit labels
   - `WriteVerticalInfo()` - Vertical text
   - `CreateEngraving()` - Engraving operations

#### Updating Forms
1. Open the `.vb` file in code view
2. Or double-click in Solution Explorer to open designer
3. Make changes and test with F5

## CSV File Format (Calibrated Dipsticks)

The calibrated dipstick workflow expects CSV files with specific naming:

**Filename Format:**
```
[description]_FV [volume]_INCS [increment]_([dimensions])_other.csv
```

**Example:**
```
Tank123_FV 5000_INCS 25_(1200x800x600)_calibrated.csv
```

**CSV Content:**
```
height1,volume1
height2,volume2
height3,volume3
...
```

## Configuration

### Engraving Settings

Edit `DipstickConstants.vb` to modify:

- **Laser Engraving**: Feed rate, depth increment
- **Spindle Engraving**: Feed rate, depth increment
- **Tool Settings**: Diameter, tool number
- **Text Positioning**: Y-offsets for various text elements
- **Font**: Default is "1CamBam_Stick_3"

### Debug Paths

If your CamBam installation is in a different location, edit `CamBamPlugin.vbproj`:

```xml
<StartProgram>C:\Your\Path\To\CamBam.exe</StartProgram>
<OutputPath>C:\Your\Path\To\plugins\</OutputPath>
```

## External References

The plugin references CamBam DLLs:
- `CamBam.CAD.dll` - CAD object creation
- `CamBam.Geom.dll` - Geometry operations

These are expected at: `C:\Program Files (x86)\CamBam plus 1.0\`

## Additional Documentation

- **[CLAUDE.md](CLAUDE.md)** - Detailed technical documentation for AI assistance
- **[BUGS.md](BUGS.md)** - Known bugs, fixes, and improvement suggestions

## Version History

- **Current (dev branch)**: Refactored with constants, data models, improved error handling
- **Previous**: Original implementation with hardcoded values

## License

[Add your license information here]

## Support

For issues or questions, refer to the documentation files or check the git history for recent changes.
