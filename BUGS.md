# Known Bugs in CamBam Plugin

This document lists known bugs found in the codebase, prioritized by severity.

## Critical Bugs (Can cause crashes or data corruption)

### 1. ✅ FIXED: Null Reference in CSV Parser
**File:** `CalForm.vb:57`
**Status:** Fixed in commit b3a5a87
**Issue:** `ReadLine()` returns null at end of file, causing `NullReferenceException`
**Impact:** Application crash when loading calibration files

### 2. ✅ FIXED: Missing Return Value in CreatePart
**File:** `CommonDetails.vb:74-90`
**Status:** Fixed in commit 3957466
**Issue:** Function doesn't return value when part already exists (line 88)
```vb
Public Shared Function CreatePart(...) As CAMPart
    If Not myUI.ActiveView.CADFile.HasPart(...) Then
        ...
        Return myPart
    End If
    ' Missing: Return Nothing or existing part here!
End Function
```
**Impact:** Returns uninitialized value, could cause null reference exceptions
**Fix:** Return Nothing or retrieve and return the existing part

### 3. ✅ FIXED: Missing Return Value in CreateCopies
**File:** `CommonDetails.vb:134-141`
**Status:** Fixed in commit 3957466
**Issue:** Function doesn't return value if `n` is not 1 or 2
```vb
Public Shared Function CreateCopies(n As Integer) As Integer
    Select Case n
        Case 1: Return 0
        Case 2: Return 30
    End Select
    ' Missing default case!
End Function
```
**Impact:** Returns uninitialized value if invalid input
**Fix:** Add default case to return 0 or throw exception

### 4. ✅ FIXED: Missing Return Value in UnitConv
**File:** `UnCalForm.vb:231-240`
**Status:** Fixed in commit 3957466
**Issue:** Function doesn't return value if `SelectedIndex` is invalid
```vb
Private Function UnitConv(x As Single) As Single
    Select Case CboUnits.SelectedIndex
        Case 0: Return x
        Case 1: Return x / 10
        Case 2: Return Round(x / 25.4, 0)
    End Select
    ' Missing default case!
End Function
```
**Impact:** Returns uninitialized value if combo box has unexpected state
**Fix:** Add default case to return x or throw exception

### 5. ✅ FIXED: Vertical Text Mutation Bug
**File:** `CommonDetails.vb:206-238`
**Status:** Fixed
**Issue:** Shared MText objects are mutated by Transform operations, causing rotation/translation to accumulate
```vb
Shared Property FirstLineText As New MText
Shared Property SecondLineText As New MText

Public Shared Sub WriteVerticalInfo(firstLine As MText, secondLine As MText, yLocation As String)
    firstLine.Transform.RotZ(...)  ' Mutates shared object!
    secondLine.Transform.RotZ(...)  ' Mutates shared object!
End Sub
```
**Impact:**
- First run: Text rotated 90° correctly
- Second run: Text rotated 180° (upside down)
- Third run: Text rotated 270°
- Text from previous run persists
**Fix:** Create new MText objects in WriteVerticalInfo instead of mutating shared objects

## High Priority Bugs (Can cause unexpected behavior)

### 6. ✅ FIXED: Silent Error Handling
**File:** `CalForm.vb:69`
**Status:** Fixed in commit 3957466
**Issue:** Empty catch block silently swallows all exceptions
```vb
Catch ex As Exception
    ' Empty - errors are silently ignored!
End Try
```
**Impact:** File parsing errors are hidden from user, returns empty list
**Fix:** Log error and show user-friendly message, or rethrow specific exceptions

### 7. ✅ FIXED: No Input Validation on File Selection
**File:** `CalForm.vb:149-230`
**Status:** Fixed
**Issue:** No validation that selected file is a valid CSV or exists
**Impact:** Could attempt to parse invalid files, leading to confusing errors
**Fix:** Added `ValidateSelectedFile()` function that checks:
- File exists
- Has .csv extension
- Is readable (not locked/permission denied)
- Is not empty
- File dialog now filters to CSV files by default

### 8. ✅ FIXED: No Validation on Numeric Inputs
**File:** `CalForm.vb:238-305`, `UnCalForm.vb:250-265`
**Status:** Fixed
**Issue:** No validation that numeric fields contain valid numbers before parsing
**Impact:** Could cause format exceptions or unexpected behavior
**Fix:**
- CalForm: Added `ValidateNumericInputs()` function that validates all numeric text boxes (Full Volume, Increments, Dipstick Height, Marked Volumes, Wefco Volume)
- UnCalForm: Added `ValidateMarkedIncrements()` function (existing validation for Increments and Height was already present)
- All validation uses `TryParse()` and provides clear user-friendly error messages
- Submit button validation prevents processing invalid data

## Medium Priority Bugs (Code quality issues)

### 9. ✅ FIXED: Hardcoded Magic Numbers in Text Positioning
**File:** `CalForm.vb`, `UnCalForm.vb`, `textForm.vb`, `DipstickConstants.vb`
**Status:** Fixed
**Issue:** Many hardcoded values still exist in forms (not using DipstickConstants)
**Impact:** Inconsistent positioning, hard to maintain
**Fix:**
- Added new constants to `DipstickConstants.vb`:
  - SWC_VOLUME_Y_OFFSET, SWC_UNITS_Y_OFFSET
  - WEFCO_VOLUME_Y_OFFSET, WEFCO_UNITS_Y_OFFSET
  - CALIBRATED_NUMBER_Y_OFFSET, UNCALIBRATED_NUMBER_Y_OFFSET
  - TEXT_ONLY_REF_Y_OFFSET
  - SWC_TEXT_X_OFFSET, UNITS_TEXT_X_OFFSET
  - TANK_NUMBER_X_OFFSET, TANK_NUMBER_Y_OFFSET
- Replaced all hardcoded magic numbers in forms with appropriate constants
- All font names now use DipstickConstants.FONT_NAME
- All text heights use DipstickConstants constants
- Unit conversions use MM_PER_CM and MM_PER_INCH constants

### 10. ✅ FIXED: Inconsistent String Validation
**File:** `CalForm.vb`, `UnCalForm.vb`, `textForm.vb`, `CommonDetails.vb`
**Status:** Fixed
**Issue:** Mix of `String.Equals("")`, `= ""`, and no validation
**Impact:** Inconsistent behavior, potential bugs
**Fix:** Standardized all string empty checks to use `String.IsNullOrWhiteSpace()`
- CalForm.vb: Updated Ref and FirstLineText checks
- UnCalForm.vb: Updated Ref, text field validation checks
- textForm.vb: Updated txtFullVolHeight validation
- CommonDetails.vb: Updated text and RefSecondLine checks in WriteClientRef and WriteVerticalInfo

### 11. ✅ INVESTIGATED: Graphics Object Disposal
**File:** `CalForm.vb`, `UnCalForm.vb`, `textForm.vb`, `CommonDetails.vb`
**Status:** No Action Required (Verified)
**Issue:** Creating `MText` and `Polyline` objects without ensuring disposal
**Impact:** None - CamBam API manages object lifecycle

**Investigation Findings:**

1. **Object Lifecycle Pattern:**
   - Objects created: `Dim myPoly As New Polyline()` / `Dim myCamText As New MText()`
   - Objects configured with properties
   - Objects added to CADFile: `myUI.ActiveView.CADFile.Add(myPoly)`
   - Objects never referenced again after addition

2. **Evidence that disposal is NOT needed:**
   - Existing code uses `Using` statements for `StreamReader` (which implements IDisposable)
   - No `Using` statements ever used for `MText` or `Polyline`
   - No `Dispose()` calls on these objects anywhere in codebase
   - Code has been functioning correctly in production

3. **CAD/CAM Document Pattern:**
   - Standard pattern: Document/CADFile takes ownership of objects when added
   - CADFile is responsible for object lifecycle management
   - Objects must remain alive as long as the document exists
   - CADFile handles disposal when document is closed

4. **Why manual disposal would be problematic:**
   - Can't dispose immediately after `Add()` - objects must stay alive
   - No event/callback to know when CADFile is done with objects
   - Would break the document ownership model

**Conclusion:**
This is NOT a bug. The CamBam API follows the standard document-based application pattern where the `CADFile` owns and manages the lifecycle of all CAD objects added to it. The objects (`MText`, `Polyline`) either don't implement `IDisposable`, or their disposal is handled internally by the CamBam framework when the document is closed or cleared.

**No changes required.**

## Low Priority (Future Improvements)

### 12. Shared State in CommonDetails
**File:** `CommonDetails.vb`
**Status:** OPEN (by design, but not ideal)
**Issue:** All properties are Shared, creating global mutable state
**Impact:** Could cause issues if multiple forms are opened simultaneously
**Fix:** Refactor to use DipstickModel instance instead of shared properties

### 13. Lack of Unit Tests
**File:** All files
**Status:** OPEN
**Issue:** No automated tests for business logic
**Impact:** Bugs may be introduced during refactoring
**Fix:** Extract testable logic and add unit tests

---

## Bug Fix Priority Order

1. **Critical first:** Fix missing return statements (bugs 2-4) ✅ DONE
2. **High priority:** Fix vertical text mutation bug (bug 5) ✅ DONE
3. **High priority:** Improve error handling (bug 6) ✅ DONE
4. **High priority:** Add file validation (bug 7) ✅ DONE
5. **High priority:** Add input validation (bug 8) ✅ DONE
6. **Medium priority:** Migrate magic numbers to constants (bug 9) ✅ DONE
7. **Medium priority:** Standardize string validation (bug 10) ✅ DONE
8. **Low priority:** Code quality improvements (bugs 11-13)
