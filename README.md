# FVer-csharp

C# Implementation of the FVer (Friendly Version) versionining scheme.

# FVer Specification (Draft)

**FVer** (Friendly Version) is a clean, minimal, and human-readable versioning format designed as an alternative to Semantic Versioning (SemVer). It emphasizes simplicity and only includes as much detail as necessary, while still supporting robust comparison and branching logic.

---

## Format

```
[prefix]baselineRevision[.release][-postfix]
```

### Components

| Component  | Description                                                                 |
| ---------- | --------------------------------------------------------------------------- |
| `prefix`   | Optional. Defines a separate branch of versions (e.g., `preview`, `alpha`). |
| `baseline` | Required. Integer >= 0, **at least 2 digits** (e.g., `03`, `10`, `104`).    |
| `revision` | Required. Alphabetic string (e.g., `a`, `b`, ..., `z`, `aa`, `ab`, ...).    |
| `.release` | Optional. Integer >= 0. Indicates hotfix or post-release updates.           |
| `-postfix` | Optional. Describes pre-release or variant versions. Ordered semantically.  |

---

## Examples

- `03a` — baseline 3, first revision
- `05d.1` — baseline 5, revision d, hotfix 1
- `preview04c` — preview branch, baseline 4, revision c
- `06f-alpha` — baseline 6, revision f, postfix "alpha" (a prerelease)
- `06f-alpha.1` — an incremental update of the `alpha` prerelease

---

## Version Comparison Rules

FVer uses the following precedence rules for version comparison:

1. **Prefix**

   - Prefixes indicate separate version branches.
   - Versions with no prefix (mainline) are considered higher than any prefixed versions.
   - No specific ordering is defined between different prefixes. By default, they are ordered ordinally.

2. **Baseline**

   - Compared numerically (e.g., `03 < 10 < 104`).

3. **Revision**

   - Compared alphabetically.
   - Alphabetic sequence is treated as base-26 (e.g., `a` = 0, `z` = 25, `aa` = 26).

4. **Release**

   - Compared numerically.
   - Only shown and considered if non-zero.

5. **Postfix** (Pre-release variant)

   - Compared using SemVer-style ordering:
     - Identifiers separated by dots are compared in sequence.
     - Numeric identifiers are compared numerically.
     - Alphanumeric identifiers are compared lexically.
     - Example order:
       ```
       03b-alpha < 03b-alpha.1 < 03b-alpha.beta < 03b-beta < 03b-beta.2 < 03b-beta.11 < 03b-rc.1 < 03b
       ```
   - Absence of a postfix (i.e., a stable version) is considered higher than any postfix.

---

## Notes

- Baseline must be at least two digits for formatting consistency (e.g., `03` instead of `3`).
- Revisions are case-insensitive but standardized to lowercase.
- Prefix and postfix can use any identifier but must avoid characters that conflict with delimiters (`.`, `-`).

---

## Recommended Practices

- Increment the **baseline** for non-revision-breaking changes or significant feature additions.
- Use **revisions** (`a`, `b`, etc.) for iterative changes or patches.
- Add a **release** number (`.1`, `.2`, etc.) only when needed (e.g., emergency fix).
- Use **prefixes** to maintain separate development tracks (e.g., `preview`, `dev`, `alpha`).
- Use **postfixes** to publish pre-release stages of a given version (e.g., `-rc.1`).

---

## Parser & Comparisons

Implementations should:

- Normalize to lowercase.
- Support conversion between revision strings (e.g., `aa`) and their integer representation.
- Use dot-segmented comparison for postfix parts following SemVer pre-release logic.
- Handle both prefixed and postfix variants correctly.

A formal grammar and code samples (C#, Python, Rust) can be provided in future versions of this spec.
