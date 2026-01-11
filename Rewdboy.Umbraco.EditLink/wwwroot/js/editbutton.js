(function () {
    // Hämta konfiguration från <script>-taggen som laddar filen
    var scriptEl = document.currentScript;
    var pingUrl = scriptEl && scriptEl.getAttribute("data-rewdboy-ping-url")
        ? scriptEl.getAttribute("data-rewdboy-ping-url")
        : "/umbraco/backoffice/Rewdboy/EditLinkAuth/Ping";

    var cacheKey = "rewdboy_editlink_is_backoffice_authed_v1";
    console.log("Rewdboy Edit Link 1.1.3 : Checking backoffice auth...");)

    function showButtons() {
        document.querySelectorAll(".rewdboy-edit-btn[data-rewdboy-edit]").forEach(function (el) {
            el.classList.remove("rewdboy-edit-hidden");
        });
    }

    // Om vi redan vet att användaren är authed i denna tab -> visa direkt
    try {
        var cached = sessionStorage.getItem(cacheKey);
        if (cached === "1") {
            showButtons();
            return;
        }
    } catch (e) { /* ignore */ }

    fetch(pingUrl, { credentials: "include" })
        .then(function (r) {
            if (!r.ok) throw new Error("not-auth");
            try { sessionStorage.setItem(cacheKey, "1"); } catch (e) { /* ignore */ }
            showButtons();
        })
        .catch(function () {
            try { sessionStorage.setItem(cacheKey, "0"); } catch (e) { /* ignore */ }
            // Låt knapparna vara dolda
        });
})();
