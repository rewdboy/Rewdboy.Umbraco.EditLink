# Umbraco Quick Edit Button 
A lightweight and non-intrusive package for **Umbraco 16+** that adds a floating **Edit Page** button to the website frontend.

When logged into the Umbraco backoffice, the button appears on the site and lets you open the current page directly in edit mode.

![Edit link button on the front-end](https://raw.githubusercontent.com/rewdboy/Rewdboy.Umbraco.EditLink/master/docs/images/editbutton_example.png)

## ⚠️ Breaking changes (v2.0.0)  
- Requires **.NET 9**  
- - Supports **Umbraco 16+ only**  
- - All client-side JavaScript has been removed 
- - Authentication is now handled via **OpenIddict server events**  
- - The package no longer relies on `UMB_UCONTEXT`  


### Required setup 
Register the TagHelper in `_ViewImports.cshtml`:

```razor
@addTagHelper *, Rewdboy.Umbraco.EditLink`` 
```

## Features

 🔐 Visible only to authenticated backoffice users  
 🪶 Lightweight (no client-side JavaScript)
🎯 Opens the current page directly in edit mode
🎨 Fully customizable via CSS
    
----------

## Installation

Install via NuGet:

`dotnet add package Rewdboy.Umbraco.EditLink` 

Register the TagHelper:
Add the following line to your ~/Views/_ViewImports.cshtml file:

`@addTagHelper *, Rewdboy.Umbraco.EditLink` 

----------

## Usage

Place the TagHelper in your layout (e.g. `_Layout.cshtml`):

`<umbraco-edit-button model="Model" />` 

### Button position

Choose placement using the `corner` attribute  
(default is top-right):

`<umbraco-edit-button model="Model" corner="tr" />
<umbraco-edit-button model="Model" corner="tl" />
<umbraco-edit-button model="Model" corner="br" />
<umbraco-edit-button model="Model" corner="bl" />` 

Optional offset (in pixels):

`<umbraco-edit-button model="Model" corner="tl" offset="24" />` 

----------

## Custom styling

By default, the package injects its own stylesheet.

Disable automatic CSS injection if you want full control:

`<umbraco-edit-button model="Model" inject-css="false" />` 

Available CSS classes:

-   `.rewdboy-editlink-container`
    
-   `.edit-page-btn`
    
-   `.rewdboy-corner-top-right`
    
-   `.rewdboy-corner-top-left`
    
-   `.rewdboy-corner-bottom-right`
    
-   `.rewdboy-corner-bottom-left`
    

You can also override the stylesheet URL:

`<umbraco-edit-button model="Model" css-url="/css/my-editbutton.css" />` 

----------

## Compatibility

-   Umbraco 16.x
    
-   Umbraco 17.x
    

----------

## License

MIT