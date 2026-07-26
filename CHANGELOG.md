# Changelog and Project History

This file consolidates the project's former tracking documents (`BUGS.md`,
`BACKLOG.md`, `UI_UX_IMPROVEMENTS.md`) into a single record. Details of the
individual items live in the git history of those files (removed 2026-07-26).

## 2026-07 — Consolidation and hardening

- **Branch consolidation** (see `BRANCH_REVIEW.md`): `dev` promoted to
  `master`; audit-branch build system ported (`CamBamDir` auto-detection,
  AnyCPU, `bin\` output with copy-to-plugins, fail-early DLL check); IDE
  state files untracked; GitHub Actions workflows restored.
- **Calibrated workflow aborts on bad JSON**: a parse failure now stops
  generation before the open document is cleared, instead of producing a
  dipstick with reference text but no graduation lines.
- **Numeric calibration data**: volume/height pairs are now
  `SortedList(Of Decimal, Decimal)` (was string-keyed, which sorted
  `"100"` before `"20"` and compared volumes by string equality).
- **Laser support removed**: the `IsLaser` flag was never settable from any
  form, so the laser document style and the duplicate laser MOP were dead
  weight. Each part now gets a single spindle engrave operation.
- **Forms close after generating** (previously hidden, accumulating in
  `Application.OpenForms` for the CamBam session).
- **G-code header fixed**: emits the actual full volume (was engraving
  `( Full Volume: )` with no value).
- **Exact 90° rotation**: `VERTICAL_TEXT_ROTATION` is `Math.PI / 2`
  (was `1.571`).
- **SWC calculation centralised** on `DipstickModel.CalculateSWC()` /
  `DipstickConstants.SWC_PERCENTAGE`.
- **Cleanups**: accidental `CalForm.nqo-GN.resx` locale resource removed;
  dead `CalibratedDipstickParser.vb` (legacy CSV parser, not compiled)
  deleted; unused copy-offset constants removed.
- **Unit tests added** (`CamBamPlugin.Tests/`) covering the JSON parser,
  calibration data, and model validation — run by GitHub Actions on every
  push/PR (`.github/workflows/tests.yml`).

## 2026-01 — Features (former BACKLOG.md, all completed)

- **Automatic CAD naming from tank type and dimensions** — filename derived
  from JSON tank data (Rectangular: `length_width_height`; Horizontal Flat
  Ends: `diameter_length`; Horizontal Dished Ends:
  `diameter_stLength_dishEndRad_knuckleRad` with optional `ovLength`,
  `tilt`, `dipPoint` variants).
- **Excel template for manual calibration entry** —
  `DipstickCalibrationTemplate.xlsm` + `JSONExport.bas` VBA export +
  `CreateExcelTemplate.ps1`; see `EXCEL_TEMPLATE_README.md`.
- **"Number of Copies" feature removed.**
- **CSV → JSON migration** for calibrated dipstick data import.

## 2025-11 → 2026-01 — Bug fixes (former BUGS.md, bugs 1–13 all fixed)

- Null reference in CSV parser; missing return values in `CreatePart`,
  `CreateCopies`, `UnitConv`; vertical text mutation (rotation accumulating
  across runs); silent error handling; file-selection and numeric input
  validation; magic numbers extracted to `DipstickConstants`; inconsistent
  string validation; vertical input box z-order; shared state in
  `CommonDetails` replaced by instance-based `DipstickModel`.
- Graphics object disposal was investigated and found not to be an issue.

## 2026-01 — UI/UX improvements (former UI_UX_IMPROVEMENTS.md, phases 1–3)

- Tooltips on complex fields, bold required-field labels with asterisks,
  progress indication during generation, JSON file dialog filtering,
  auto-population of form fields from JSON metadata, suggested marked
  increments, improved form layouts.

## Open items

- No open bugs. New issues: use the GitHub issue tracker rather than
  re-creating a bugs file.
