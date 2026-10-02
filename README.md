# Umbraco Quick Edit Button

A lightweight, non-intrusive package for **Umbraco 16+** that adds a floating **Edit Page** button on the frontend.

When a user is signed in to the Umbraco backoffice, the button links directly to the current document in edit mode.

![Edit link button on the front-end](https://raw.githubusercontent.com/rewdboy/Rewdboy.Umbraco.EditLink/master/docs/images/editbutton_example.png)

Available on [NuGet](https://www.nuget.org/packages/Rewdboy.Umbraco.EditLink) and the [Umbraco Marketplace](https://marketplace.umbraco.com/package/rewdboy.umbraco.editlink).

## Latest changes

- **2.2.3:** Dynamic paths are now used for both the backoffice edit URL and default CSS URL (works with custom backoffice path and virtual directories / `PathBase`).
- **2.2.2:** Improved accessibility (`aria-label`, screen-reader text, `:focus-visible`) and async TagHelper auth flow.

See full release notes in [CHANGELOG.md](CHANGELOG.md).

## Features

- Visible only to authenticated backoffice users
- No client-side JavaScript
- Opens the current page directly in Umbraco edit mode
- Configurable button corner and offset
- Optional automatic CSS injection

## Requirements

| Umbraco | .NET | Package target |
|---------|------|----------------|
| 16.x    | 9    | `net9.0`       |
| 17.x    | 10   | `net10.0`      |
| 18.x    | 10   | `net10.0`      |

> This package does **not** support Umbraco 14/15.

## Installation

Install via NuGet:

`dotnet add package Rewdboy.Umbraco.EditLink`

Register the TagHelper in `~/Views/_ViewImports.cshtml`:

```razor
@addTagHelper *, Rewdboy.Umbraco.EditLink
```

## Usage

Place the TagHelper in your layout (for example `_Layout.cshtml`):

`<umbraco-edit-button model="Model" />`

The `model` input supports:
- `IPublishedContent`
- `ContentModel`
- custom models exposing a readable `Content` property of type `IPublishedContent`

### Attributes

- `corner`: `top-right` (default), `top-left`, `bottom-right`, `bottom-left` (also supports `tr`, `tl`, `br`, `bl`)
- `offset`: optional pixel offset from the selected corner (negative values are clamped to `0`)
- `inject-css`: `true` by default, set to `false` to disable automatic stylesheet injection
- `css-url`: optional custom stylesheet URL
- `title`: button title / aria-label text (default: `Edit page`)

Examples:

`<umbraco-edit-button model="Model" corner="tl" offset="24" />`

`<umbraco-edit-button model="Model" inject-css="false" />`

`<umbraco-edit-button model="Model" css-url="/css/my-editbutton.css" />`

## How it works / Security

The package uses OpenIddict server events to keep a small, dedicated cookie (`REWDBOY_EDITLINK`) in sync:

1. On token generation, the package signs in a minimal principal on its own cookie scheme.
2. On token revocation, it signs out that cookie.
3. The TagHelper calls `AuthenticateAsync` on that scheme before rendering.
4. The button is never rendered in preview mode.

The edit URL still points to Umbraco backoffice and therefore requires normal Umbraco authorization.

## Custom styling

Default stylesheet path (when `inject-css="true"` and `css-url` is not set):

`/_content/Rewdboy.Umbraco.EditLink/css/editbutton.css`

This URL is generated as an absolute URL via Umbraco hosting APIs, so it works with custom app `PathBase`.

Available CSS classes:

- `.rewdboy-editlink-container`
- `.edit-page-btn`
- `.rewdboy-corner-top-right`
- `.rewdboy-corner-top-left`
- `.rewdboy-corner-bottom-right`
- `.rewdboy-corner-bottom-left`
- `.rewdboy-visually-hidden`

## Troubleshooting

- **Button not visible:** verify `@addTagHelper *, Rewdboy.Umbraco.EditLink` exists in `_ViewImports.cshtml`.
- **Logged in but no button:** sign out and sign back in to backoffice so the edit-link cookie is refreshed.
- **Preview mode:** the button is intentionally hidden.
- **Custom backoffice path / virtual directory:** supported in 2.2.3+.

## License

MIT
