# CamBam Plugin — Code Audit

> **Historical note (2026-07-26):** this audit was written against the legacy
> `master` code (`7339ef7`), before the `dev` refactor was consolidated. Many
> of the source-level findings below were since fixed on `dev` (see `BUGS.md`);
> the build-system recommendations have been ported into `CamBamPlugin.vbproj`.
> Kept as a historical record.

**Date:** 2026-07-02
**Scope:** All VB.NET source, project files, and repository configuration on `main` (7339ef7).

## What the plugin does

A CamBam 0.9.8 plugin (VB.NET, .NET 3.5 WinForms) that adds a **Dipsticks** menu with three
generators — *Calibrated* (from a volume/height CSV file), *Uncalibrated* (regular increments),
and *Text Only*. Each form clears the active CamBam document, draws graduation lines and MText
labels, and creates a machining part with spindle and laser engrave operations.

---

## 1. Correctness bugs (highest priority)

### 1.1 Shared mutable state in `CommonDetails` leaks between forms and between runs
All fields in `CommonDetails` are `Shared` (static) but are populated by an *instance*
constructor called from whichever form was submitted. Values set by one form persist and are
consumed by the others:

- `UnCalForm.BtnSubmit_Click` calls `WriteClientRef(..., ClientRef, RefText)` but the uncal
  branch of the constructor never sets `ClientRef`/`RefText` — so an uncalibrated dipstick will
  engrave the **client ref left over from a previous calibrated/text run** (`UnCalForm.vb:79`).
- `RefSecondLine` is only set by `textForm`. `WriteClientRef` adds it whenever it is non-Nothing
  (`CommonDetails.vb:187`), so once a Text-Only dipstick has been made, its tank letter is
  engraved on **every subsequent dipstick** in the same CamBam session.
- `FirstLineText`/`SecondLineText` are single shared `MText` *instances* reused across runs.
  `WriteVerticalInfo` applies `Transform.RotZ(1.571)` to them on every submit
  (`CommonDetails.vb:201`), so rotations can accumulate across runs, and the same entity object
  is re-added to a new `CADFile` after `FileNew` — undefined behaviour in the CamBam object model.

**Fix:** make `CommonDetails` a normal instance class (no `Shared` fields), construct it fresh
per submit, and create new `MText` objects per run. This one change removes a whole category of
"wrong text on the stick" defects.

### 1.2 CSV reading loop exits only via a swallowed exception
`CalForm.CreateVolumeHeightPairsFromFile` (`CalForm.vb:48-67`):

- `Loop While Not myList.Equals(Nothing)` is always true; the loop actually terminates when
  `sR.ReadLine()` returns `Nothing` at EOF and `.Split` throws a `NullReferenceException`, which
  the empty `Catch` silently swallows.
- Any *real* error is also swallowed: a duplicate height/volume in the file makes
  `SortedList.Add` throw mid-file, and the function silently returns a **partial list** — a
  dipstick with missing graduation lines and no warning. For a measurement instrument this is
  the most dangerous defect in the codebase.
- A blank line mid-file truncates the data the same way.

**Fix:** loop on `Do While Not sR.EndOfStream` (or `ReadLine() IsNot Nothing`), validate each
line has two numeric fields, and surface any failure to the user instead of catching-and-ignoring.

### 1.3 `SortedList(Of String, String)` sorts numerically-keyed data lexicographically
Heights/volumes are stored as strings, so ordering is `"100" < "20" < "9"`. Drawing order mostly
doesn't matter visually, but any future logic that relies on order (e.g. "last line = full
volume") will be wrong. Parse to `Double` keys.

### 1.4 `CreateCopies` silently returns 0 for 3+ copies
`CommonDetails.vb:132-139` handles only `Case 1` and `Case 2`; the `NumDips` control allows up
to 100 (WinForms default `Maximum`). Any value ≥ 3 falls out of the `Select` and returns the
implicit default `0`, i.e. behaves like 1 copy. Also note nothing ever *loops* over copies — the
value is only used as an X offset. Either implement a real copy loop or clamp
`NumDips.Maximum = 2` and add a `Case Else`.

### 1.5 The G-code header never includes the volume
`CommonDetails.vb:113`: `myCustomHeader.SetValue("( Full Volume: " & ")")` emits
`( Full Volume: )` — the value was never concatenated in. Should be
`"( Full Volume: " & FullVol & " )"`.

### 1.6 `Laser` is dead code that can never activate
`CommonDetails.Laser` is declared but never assigned (the checkbox was removed in commit
894076e). The laser branch of `CreateCADFile` (`CommonDetails.vb:56-59`) is unreachable, yet
`CreatePart` still **always adds both a spindle and a laser MOP** to every part. Decide: either
reinstate a UI toggle and gate the MOPs on it, or delete the laser path entirely.

### 1.7 Unvalidated numeric conversions can crash the CamBam host
With `Option Strict Off`, strings are implicitly converted at runtime:

- `CalForm`: empty `txtDipHeight`/`txtFullVol` → `InvalidCastException` on submit
  (`Round(FullVol * 0.97)`, `WriteUnits(..., DipHeight, ...)`); the exception is unhandled and
  propagates into CamBam.
- `textForm`: non-numeric `txtFullVolHeight` crashes the same way (only an empty check exists).
- `CalForm.TrimFullVolume/TrimIncrements/TrimTankDimensionsFromFileName` assume the filename
  contains `"FV "`, `"_INCS"`, `"("`, `")"`. `IndexOf` returning -1 gives wrong substrings or
  `ArgumentOutOfRangeException`. A user picking any other CSV crashes `Button1_Click`.

`UnCalForm` has per-field validation; port that pattern (or better, `Decimal.TryParse`) to the
other two forms, and parse filenames with a validated regex plus a friendly error message.

### 1.8 Possible duplicate top line in `UnCalForm`
`DrawLinesAndNumbers` (`UnCalForm.vb:86-111`) accumulates `l += Increments` in `Single` and then
unconditionally draws a final line at `DipHeight`. With increments that don't divide the height
exactly (or float drift, e.g. inch mode ×25.4), the last loop line can land at/near `DipHeight`,
producing two overlapping polylines — which the machine will engrave twice in the same groove.
Compute line positions as `i * Increments` from an integer counter instead of accumulating, and
skip the final line when the loop already produced it.

### 1.9 Identifier shadowing landmines
- `WriteClientRef` declares a local `secondLineText` that (case-insensitively) shadows the
  shared property `SecondLineText`; lines 190/192 *look* like they touch the shared vertical-info
  object but actually hit the local. It works today by accident; the next edit will break it.
- `UnCalForm.BtnSubmit_Click` declares local `markedIncrement` and `cboUnits` that shadow the
  class property `markedIncrement` and control `CboUnits` — so the unit-converting property
  setter is never used for marked increments.

Rename the locals (or delete the redundant properties).

### 1.10 Misc correctness
- `RotZ(1.571)` is 90.01°, not 90° — use `Math.PI / 2` (`CommonDetails.vb:201,210`).
- `CreatePart` has no `Return` when the part already exists → returns `Nothing`
  (`CommonDetails.vb:72-88`); harmless today only because `FileNew` always clears the document.
- `txtIncs_LostFocus`/`txtHeight_LostFocus` are named LostFocus but `Handles ... TextChanged`,
  so unit conversion is re-applied on every keystroke; changing the units combo *after* typing
  does not re-convert already-entered values.
- `WriteVerticalInfo` dereferences `secondLine.Text` without a null check.
- `Copies` is `Single`; it's a count — should be `Integer`.
- `IsDivisible` (`CommonDetails.vb:231`) is unused — delete.

---

## 2. Design & maintainability

1. **Turn `Option Strict On`** (project currently `Off`, with ten `NoWarn` codes suppressing the
   implicit-conversion warnings). This will surface most of §1.7 at compile time. Fix the
   resulting errors by parsing inputs once, at the form boundary, into typed fields.
2. **Extract the drawing logic from the forms.** `CalForm`/`UnCalForm`/`textForm` each mix UI,
   parsing, and geometry. A `DipstickBuilder` class taking a plain options object
   (ref, client ref, height, increments, units, copies…) would make the geometry unit-testable
   without CamBam and would kill the shared-state constructor pattern
   (`New CommonDetails(,, Me)`).
3. **Deduplicate the MText helpers.** `WriteRef`, `WriteUnits`, `WriteSWC`, `WriteWefcoRef`,
   `WriteNumber`, `WriteTank` all repeat the same 5 lines. One factory
   (`AddText(text, x, y, height)`) plus named constants for the font (`1CamBam_Stick_3`), text
   heights, and Y offsets (40, 60, 68, 76, 105, 113, 145, 153, 188…) would document the stick
   layout in one place.
4. **Machining parameters are hard-coded** (feed 500, depth 0.45, tool 10, laser style names) in
   `CreateEngraving`. Move them to a settings file/`My.Settings` so shop-floor changes don't
   require a recompile.
5. **Form lifecycle:** every menu click news up a form and `Show()`s it non-modally; submit
   `Hide()`s it. Hidden instances accumulate for the CamBam session and users can open several
   at once. Use `ShowDialog()` + `Dispose()`, or keep one instance per form type.
6. Prefer `If(cond, a, b)` over `IIf(...)` (IIf evaluates both arms and is un-typed), and
   string interpolation over `1.5 + x & "," & refYPos & ",0"` location strings.
7. Empty `Catch ex As Exception` blocks (`CalForm.vb:63`) — at minimum show/log the error.

---

## 3. Repository & build hygiene

1. **Build artifacts and IDE state are committed** despite the `.gitignore`: `obj/Debug/**`
   (including `CamBamPlugin.dll` and `.pdb`), `CamBamPlugin.v12.suo`, `CamBamPlugin.v14.suo`,
   `CamBamPlugin.sln.ide/**` (ESE database files), `*.vspscc`, `*.vssscc`. They were added
   before the ignore file; remove with `git rm -r --cached obj "*.suo" CamBamPlugin.sln.ide ...`.
2. **`CalForm.nqo-GN.resx`** — an N'Ko (Guinea) localized resource, almost certainly an
   accidental `Language` property flip in the WinForms designer. It generates a pointless
   satellite assembly (`nqo-GN/CamBamPlugin.resources.dll`). Delete the file and its
   `<EmbeddedResource>` entry.
3. **Machine-specific paths in the project file** (`CamBamPlugin.vbproj:25,53,56`): the Debug
   `OutputPath` climbs eight directories into `Program Files (x86)\CamBam plus 0.9.8\plugins\`
   (requires elevation and only works from one checkout location), and the CamBam reference
   `HintPath`s do the same. Introduce a `<CamBamDir>` MSBuild property (overridable via
   environment), reference `$(CamBamDir)\CamBam.CAD.dll`, build to `bin\`, and copy to the
   plugins folder in an `AfterBuild` target.
4. **Legacy source-control bindings**: the `Scc*` properties (`SAK`) and `.vspscc`/`.vssscc`
   files are VSS/TFS leftovers — remove.
5. **No README or LICENSE.** Add a short README: what the plugin does, expected CSV/filename
   format (`... FV <vol>_INCS <inc>_(<dims>)...`), CamBam version, build steps, install path.
6. `AssemblyInfo.vb` still says "Copyright © 2014" with empty company/description; bump
   `AssemblyVersion` when behaviour changes so the shop can tell which build is installed.
7. **.NET 3.5 target** is correct for CamBam 0.9.8 and should stay unless you move to
   CamBam 1.0 (which is .NET 4.x) — document this constraint rather than "upgrading" it.
8. CI note: a compile check in GitHub Actions is only possible if stub/reference copies of
   `CamBam.CAD.dll`/`CamBam.Geom.dll` are available to the build; otherwise keep CI to linting.

---

## 4. Suggested order of work

| Priority | Item | Refs |
|---|---|---|
| P1 | De-static `CommonDetails`; new MText per run | §1.1, §1.9 |
| P1 | Rewrite CSV loop with real EOF + error reporting | §1.2 |
| P1 | Validate all numeric/filename inputs before use | §1.7 |
| P1 | Fix `CreateCopies` default + clamp `NumDips` | §1.4 |
| P2 | Fix G-code header; resolve dead `Laser` path | §1.5, §1.6 |
| P2 | Non-accumulating increment loop in `UnCalForm` | §1.8 |
| P2 | `Option Strict On`; remove `NoWarn`; fix types | §2.1 |
| P2 | Untrack build artifacts; delete nqo-GN resx & Scc bindings | §3.1–3.4 |
| P3 | Extract `DipstickBuilder`; dedupe text helpers; settings for machining params | §2.2–2.4 |
| P3 | Form lifecycle (`ShowDialog`/`Dispose`) | §2.5 |
| P3 | README, LICENSE, versioning, portable build paths | §3.3, §3.5–3.6 |
