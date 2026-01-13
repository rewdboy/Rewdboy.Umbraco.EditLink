# Changelog

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
