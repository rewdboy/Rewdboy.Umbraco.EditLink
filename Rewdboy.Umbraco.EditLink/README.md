
# Umbraco Quick Edit Button

A lightweight and non-intrusive package for **Umbraco 14+** that adds a floating "Edit Page" button to your website's frontend. 

## How it works

When you are logged into the Umbraco backoffice and browse the website,
an edit button is shown on the front-end. Clicking it opens the current
page directly in edit mode.

![Edit link button on the front-end](docs/images/editbutton_example.png)


## ⚠️ Breaking changes (v2.0.0)

This release introduces major improvements and requires attention before upgrading.

### 🔹 .NET 9 & Umbraco 16+
- The package now targets **.NET 9**.
- Only **Umbraco 16+** is supported.
- Earlier Umbraco versions (≤15) are no longer compatible.

### 🔹 JavaScript removed
- All client-side JavaScript has been removed.
- The edit button is now rendered and controlled **entirely server-side**.
- `editbutton.js` and the `rewdboy-edit-hidden` CSS class are no longer used.

### 🔹 New authentication mechanism
- Visibility of the edit button is now based on a **custom authentication cookie created via OpenIddict server events**.
- The package no longer relies on Umbraco cookies such as `UMB_UCONTEXT`.

### 🔹 TagHelper registration required
Because the edit button is implemented as a Razor TagHelper from an external NuGet package, it must be registered in `_ViewImports.cshtml`:

```razor
@addTagHelper *, Rewdboy.Umbraco.EditLink
```

### Why these changes?
They align the package with Umbraco 16’s OpenID Connect authentication model, remove client-side hacks, and significantly improve reliability and maintainability.

## Features

* **Secure:** Only visible to users logged into the Umbraco Backoffice.
* **Lightweight:** Zero dependencies other than the Umbraco core.
* **Modern:** Built specifically for Umbraco 14+ using the new Backoffice URL structure (GUID-based).
* **Customizable:** Easily styled to match your brand.

## Installation

### 1. Install via NuGet
Run the following command in your terminal:
```bash
dotnet add package Rewdboy.Umbraco.EditLink
```

### 2. Register Tag Helpers
Add the following line to your ~/Views/_ViewImports.cshtml file:

```
@addTagHelper *, Rewdboy.Umbraco.EditLink
```

## Usage
Place the Tag Helper inside your main layout file (e.g., _Layout.cshtml), ideally right after the opening `<body>``` tag:
```
<umbraco-edit-button model="Model" />
```

## Customization
The button is rendered with the CSS **class .edit-page-btn**. You can override the default styling in your site's main CSS file.

Example (changing to Umbraco Orange):
```
.edit-page-btn {
    background-color: #f5c12e !important;
    border-color: #d6a41d;
}
```

## Compatibility
- Umbraco 16 (v16.x)
- Umbraco 17 (v17.x)


## License
This project is licensed under the MIT License.