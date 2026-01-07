# Umbraco Quick Edit Button


Ett smidigt litet paket för Umbraco 16 som lägger till en diskret redigeringsknapp på din webbplats frontend. Knappen visas endast för inloggade redaktörer och tar dig direkt till den aktuella sidans redigeringsläge i Backoffice.

## Installation

1. install NuGet-package:
   ```bash
   dotnet add package Rewdboy.Umbraco.EditLink
   ```


Add this to your viewimport.cshtml file to use the EditLink helper in your views

@addTagHelper *, Rewdboy.Umbraco.EditLink

Then you can use the EditLink helper like this in your layoutfile. Place it right after the <body> tag:
<umbraco-edit-button model="Model" />


Knappen har klassen .edit-page-btn. Om du vill anpassa utseendet kan du lägga till egen CSS i din huvudfil, till exempel:

CSS

.edit-page-btn {
    background-color: #f5c12e !important; /* Umbraco Orange */
}