# Known Bugs in CamBam Plugin

This document lists known bugs found in the codebase, prioritized by severity.

## Critical Bugs (Can cause crashes or data corruption)

### 1. ✅ FIXED: Null Reference in CSV Parser
**File:** `CalForm.vb:57`
**Status:** Fixed in commit b3a5a87
**Issue:** `ReadLine()` returns null at end of file, causing `NullReferenceException`
**Impact:** Application crash when loading calibration files

### 2. Missing Return Value in CreatePart
**File:** `CommonDetails.vb:74-90`
**Status:** OPEN
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

### 3. Missing Return Value in CreateCopies
**File:** `CommonDetails.vb:134-141`
**Status:** OPEN
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

### 4. Missing Return Value in UnitConv
**File:** `UnCalForm.vb:231-240`
**Status:** OPEN
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

## High Priority Bugs (Can cause unexpected behavior)

### 5. Silent Error Handling
**File:** `CalForm.vb:69`
**Status:** OPEN
**Issue:** Empty catch block silently swallows all exceptions
```vb
Catch ex As Exception
    ' Empty - errors are silently ignored!
End Try
```
**Impact:** File parsing errors are hidden from user, returns empty list
**Fix:** Log error and show user-friendly message, or rethrow specific exceptions

### 6. No Input Validation on File Selection
**File:** `CalForm.vb:139-147`
**Status:** OPEN
**Issue:** No validation that selected file is a valid CSV or exists
**Impact:** Could attempt to parse invalid files, leading to confusing errors
**Fix:** Validate file exists, has .csv extension, is readable

### 7. No Validation on Numeric Inputs
**File:** `CalForm.vb`, `UnCalForm.vb`
**Status:** OPEN
**Issue:** No validation that numeric fields contain valid numbers before parsing
**Impact:** Could cause format exceptions or unexpected behavior
**Fix:** Add input validation with user-friendly error messages

## Medium Priority Bugs (Code quality issues)

### 8. Hardcoded Magic Numbers in Text Positioning
**File:** `CalForm.vb`, `UnCalForm.vb`, `textForm.vb`
**Status:** PARTIALLY FIXED
**Issue:** Many hardcoded values still exist in forms (not using DipstickConstants)
**Impact:** Inconsistent positioning, hard to maintain
**Fix:** Migrate remaining magic numbers to DipstickConstants

### 9. Inconsistent String Validation
**File:** Multiple files
**Status:** OPEN
**Issue:** Mix of `String.Equals("")`, `= ""`, and no validation
**Impact:** Inconsistent behavior, potential bugs
**Fix:** Standardize on `String.IsNullOrWhiteSpace()`

### 10. No Disposal of Graphics Objects
**File:** `CalForm.vb`, `UnCalForm.vb`, `textForm.vb`
**Status:** OPEN
**Issue:** Creating `MText` and `Polyline` objects without ensuring disposal
**Impact:** Potential memory leaks in long-running sessions
**Fix:** Ensure proper disposal or verify CamBam API handles it

## Low Priority (Future Improvements)

### 11. Shared State in CommonDetails
**File:** `CommonDetails.vb`
**Status:** OPEN (by design, but not ideal)
**Issue:** All properties are Shared, creating global mutable state
**Impact:** Could cause issues if multiple forms are opened simultaneously
**Fix:** Refactor to use DipstickModel instance instead of shared properties

### 12. Lack of Unit Tests
**File:** All files
**Status:** OPEN
**Issue:** No automated tests for business logic
**Impact:** Bugs may be introduced during refactoring
**Fix:** Extract testable logic and add unit tests

---

## Bug Fix Priority Order

1. **Critical first:** Fix missing return statements (bugs 2-4)
2. **High priority:** Improve error handling (bug 5)
3. **Medium priority:** Add input validation (bugs 6-7)
4. **Low priority:** Code quality improvements (bugs 8-12)
