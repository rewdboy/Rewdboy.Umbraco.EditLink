# Changelog


## [2.2.0] – 2026-09-08
### Added
- Support for **Umbraco 18**.
- The package now multi-targets `net9.0` (Umbraco 16) and `net10.0` (Umbraco 17 and 18).

### Changed
- Umbraco dependency ranges are now per target framework: `[16.0.0, 17.0.0)` for `net9.0` and `[17.0.0, 19.0.0)` for `net10.0`.


## [2.1.2] – 2026-01-31
### Fixed
- Fixed an issue in Umbraco 17 where the TagHelper did not accept `ContentModel` as input.
- The edit button now supports both `IPublishedContent` and `ContentModel` in Razor views.


## [2.1.1] - 2026-01-29
### Fixed
- Fixed OpenIddict version conflicts when used with Umbraco 17.


## [2.1.0] – 2025-01-13
### Added
- Configurable button position via `corner` attribute (`top-right`, `top-left`, `bottom-right`, `bottom-left`)
- Short-hand aliases for `corner` (`tr`, `tl`, `br`, `bl`)
- Optional `offset` attribute to control distance from viewport edges
- Improved CSS structure with a dedicated container element for positioning

### Changed
- Button positioning is now handled exclusively by the container element instead of the button itself
- CSS updated to prevent positional conflicts when switching corners

---

## [2.0.0] – 2025-01-11
### ⚠️ Breaking
- Requires .NET 9 and Umbraco 16+
- Removed all JavaScript
- Switched to OpenIddict-based authentication

### Added
- Server-side TagHelper rendering
- OpenIddict integration for reliable auth

### Removed
- editbutton.js
- UMB_UCONTEXT-based detection
