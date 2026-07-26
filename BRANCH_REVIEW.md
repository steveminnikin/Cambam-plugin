# Branch Review and Consolidation Plan

*Review date: 2026-07-26*

## Current branches

| Branch | Last commit | Status |
|---|---|---|
| `master` (default) | 2025-11-08 | Legacy 2019 code (.NET 3.5, x86) plus the merged GitHub Actions workflows (PR #2). 34 commits behind `dev`. |
| `dev` | 2026-01-26 | The main development line: 34 commits of refactoring, bug fixes and features on top of the fork point. |
| `feature/json-calibration-import` | 2025-11-11 | Fully contained in `dev` (0 commits ahead). Obsolete. |
| `claude/fix-text-input-bug-3ZB22` | 2026-01-25 | Superseded — every feature on it was re-implemented on `dev`. |
| `claude/cambam-plugin-audit-1aaa59` | 2026-07-26 | Independent hardening of the *legacy* master code. Valuable build/tooling work, but its source fixes target the old code structure. |

## What each line contains

### `dev` — the canonical code line

`dev` diverged from `master` at commit `399faab` (2019) and carries the whole
modernisation effort:

- Namespaces added throughout; code extracted into `DipstickModel`,
  `DipstickConstants`, `CalibrationData`, `CalibratedDipstickParser`,
  `JSONCalibrationParser`.
- Bugs #2–#12 fixed (crashes, validation, shared-state elimination via the
  instance-based model).
- CSV → JSON migration for calibrated dipsticks; Excel template
  (`DipstickCalibrationTemplate.xlsm`, `JSONExport.bas`,
  `CreateExcelTemplate.ps1`).
- UI/UX improvement phases 1–3; vertical-text z-order fix; automatic CAD
  file naming; "Number of Copies" feature removed.
- Targets .NET Framework 4.8.

Gaps on `dev`:

- Missing the GitHub Actions workflows (they only exist on `master` and the
  branches cut from it).
- Still tracks IDE state files that should not be in git:
  `CamBamPlugin.v12.suo`, `CamBamPlugin.v14.suo`, `CamBamPlugin.sln.ide/*`,
  `*.vspscc`, `*.vssscc`, `.claude/settings.local.json`.
- `CamBamPlugin.vbproj` Debug `OutputPath` is a fragile relative path into
  `Program Files (x86)\CamBam plus 1.0\plugins\` (breaks on any other
  checkout location and needs elevated rights), CamBam DLL `HintPath`s are
  machine-specific relative paths, and `PlatformTarget` is pinned to `x86`,
  which prevents the plugin loading into 64-bit CamBam 1.0.

### `claude/cambam-plugin-audit-1aaa59` — hardening of the legacy code

Branched from current `master`, so it never saw the `dev` refactor. Its 14
commits fall into three groups:

1. **Repo hygiene** — removed `obj/`, `bin/`, IDE state and legacy
   source-control bindings from git; better `.gitignore`; `AUDIT.md` report.
2. **Source fixes on the old code** — shared-state reset, input validation,
   safe file reading, copy-offset fix, G-code header fix, modal dialog
   disposal, exact 90° rotation. Most of these were fixed *differently and
   more thoroughly* on `dev` (bugs #2–#12), and they patch files `dev` has
   since restructured, so they cannot be merged mechanically.
3. **Build modernisation (unique, valuable)** — `CamBamDir` as a single
   overridable MSBuild property with automatic probing of the standard
   CamBam 1.0/0.9.8 install paths, `AnyCPU` target, output to `bin\` with a
   best-effort post-build copy into CamBam's `plugins` folder, and a
   fail-early error when the CamBam DLLs cannot be found.

### `claude/fix-text-input-bug-3ZB22` — superseded

Branched from `master`, merged `dev` in, then added three things that were
each re-implemented on `dev` afterwards:

| fix branch | dev equivalent |
|---|---|
| `f670a70` z-order fix | `ffa29b5` z-order fix + form layout improvements |
| `80af3e8` CAD naming | `a089987` CAD naming by tank type and dimensions |
| `58f8a87` `DipstickCalibrationGenerator.bas` (VBA) | `d90d8df` `.xlsm` template + `JSONExport.bas` + docs |

Nothing on it is worth keeping that `dev` doesn't already have in a newer
form.

### `feature/json-calibration-import` — merged

Every commit is an ancestor of `dev`. Delete.

## Recommended way ahead

**Adopt `dev` as the single line of truth, port the audit branch's build and
hygiene work onto it, then fast-track it into `master`.**

Do **not** attempt a git merge of the audit branch into `dev` — they share a
2019 merge-base and both rewrote the same files (`CalForm.vb`,
`CommonDetails.vb`, `UnCalForm.vb`, `MyPlugin.vb`, `CamBamPlugin.vbproj`), so
a merge would be one large conflict resolved by hand anyway. Porting the
specific improvements is smaller and safer.

### Step 1 — port onto `dev` (one working branch, one PR)

1. **Project file**: take the audit branch's `CamBamPlugin.vbproj` approach —
   `CamBamDir` property with install-path probing, `$(CamBamDir)` HintPaths,
   `AnyCPU`, `bin\` output, post-build copy to `plugins`, fail-early check —
   while keeping `dev`'s compile item list (models/parsers) and its
   `System.Web.Extensions` reference (needed by the JSON parser).
2. **Repo hygiene**: `git rm --cached` the IDE files still tracked on `dev`
   (`*.suo`, `CamBamPlugin.sln.ide/`, `*.vspscc`, `*.vssscc`,
   `.claude/settings.local.json`) and add the audit branch's `.gitignore`
   entries (`*.sln.ide/`).
3. **Workflows**: restore `.github/workflows/claude.yml` and
   `claude-code-review.yml` from `master`.
4. **Cherry-check the audit source fixes** against `dev`'s code: most are
   already covered by bugs #2–#12; the ones worth verifying individually are
   modal dialog disposal (`ShowDialog` + `Dispose`), the exact 90° rotation
   constant (vs `1.571`), and the null-safe FullVol check. The copy-offset
   fix is moot (`dev` removed the copies feature).
5. Optionally carry `AUDIT.md` over as a historical record.

### Step 2 — verify

Build the plugin from the consolidated branch and smoke-test all three
workflows (calibrated JSON, uncalibrated, text-only) in CamBam before
promoting. `BUGS.md`/`BACKLOG.md` indicate testing was still in progress on
`dev`.

### Step 3 — promote and clean up

1. Merge the consolidated branch into `dev`, then open a PR `dev` → `master`
   so `master` finally reflects reality (it is currently 34+ commits stale).
2. Delete `feature/json-calibration-import` (fully merged).
3. Delete `claude/fix-text-input-bug-3ZB22` (superseded).
4. Delete `claude/cambam-plugin-audit-1aaa59` once its build changes are
   ported.

End state: `master` = released/current, `dev` = integration branch,
short-lived feature branches cut from `dev`.
