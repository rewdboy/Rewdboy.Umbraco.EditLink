# Umbraco Quick Edit Button

A lightweight and non-intrusive package for **Umbraco 16** that adds a floating "Edit Page" button to your website's frontend. 

The button is only rendered for authenticated Backoffice users, providing a seamless shortcut directly to the current page's editor in the Umbraco Backoffice.

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

### 2. Register Tag Helpers
Add the following line to your ~/Views/_ViewImports.cshtml file:

Razor CSHTML

@addTagHelper *, Rewdboy.Umbraco.EditLink


## Usage
Place the Tag Helper inside your main layout file (e.g., _Layout.cshtml), ideally right after the opening <body> tag:

Razor CSHTML

<umbraco-edit-button model="Model" />


## Customization
The button is rendered with the CSS class .edit-page-btn. You can override the default styling in your site's main CSS file.

Example (changing to Umbraco Orange):

CSS

.edit-page-btn {
    background-color: #f5c12e !important;
    border-color: #d6a41d;
}

## Compatibility
- Umbraco 14 (v14.x)
- Umbraco 15 (v15.x)
- Umbraco 16 (v16.x)
- Umbraco 17 (v17.x)


## License
This project is licensed under the MIT License.