# Dipstick Calibration Template

This Excel template allows you to manually enter calibration data (volume/height pairs) and export it as a JSON file that can be read by the CamBam Dipstick Plugin.

## Files

- `DipstickCalibrationTemplate.xlsm` - Excel template with data entry sections
- `JSONExport.bas` - VBA module for JSON export functionality

## Setup Instructions

### Importing the VBA Module

1. Open `DipstickCalibrationTemplate.xlsm` in Excel
2. Press `Alt + F11` to open the VBA Editor
3. In the VBA Editor, go to **File > Import File...**
4. Select `JSONExport.bas` and click Open
5. Close the VBA Editor
6. Save the workbook

### Adding the Export Button (Optional)

1. Go to **Developer** tab (enable it in File > Options > Customize Ribbon if not visible)
2. Click **Insert** > **Button (Form Control)**
3. Draw a button on the sheet
4. In the "Assign Macro" dialog, select `ExportToJSON`
5. Right-click the button and select "Edit Text" to rename it "Export to JSON"

## Using the Template

### Required Fields

- **Increments (L)**: The volume increment for markings (e.g., 100 for marks every 100 litres)
- **Full Volume (L)**: The total tank capacity
- **Top Height (mm)**: The dipstick height at full volume
- **Volume/Height Data**: At least one row of volume/height pairs

### Optional Fields

- **Tank Dimensions**: Length, Width, Height in mm (for reference)

### Volume/Height Data

Enter your calibration data in the table starting at row 18:
- Column A: Volume (L)
- Column B: Height (mm)

Example:
| Volume (L) | Height (mm) |
|------------|-------------|
| 0          | 0           |
| 100        | 45          |
| 200        | 89          |
| 300        | 132         |
| ...        | ...         |

### Exporting to JSON

1. Fill in all required fields
2. Run the `ExportToJSON` macro (via button or Alt+F8)
3. Choose a save location for the JSON file
4. Use the exported JSON file with the CamBam Dipstick Plugin

## JSON Output Format

The exported JSON will have this structure:

```json
{
  "tank": {
    "dimensions": {
      "length": 1000,
      "width": 500,
      "height": 800
    }
  },
  "calculation": {
    "increments": 100
  },
  "results": {
    "fullVolume": 5000,
    "topHeight": 750
  },
  "incrementData": [
    { "volume": 0, "height": 0 },
    { "volume": 100, "height": 45 },
    { "volume": 200, "height": 89 }
  ]
}
```

## Troubleshooting

### Macros are disabled
Go to File > Options > Trust Center > Trust Center Settings > Macro Settings and select "Enable all macros" or "Disable all macros with notification".

### VBA module won't import
Ensure you're importing into a macro-enabled workbook (.xlsm) and that macros are enabled.
