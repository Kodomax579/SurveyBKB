window.schulFunkTheme = (function () {
    const storageKey = "schulfunk-theme";

    function apply(theme) {
        const selectedTheme = theme === "light" ? "light" : "dark";

        document.documentElement.setAttribute("data-theme", selectedTheme);
        document.documentElement.style.colorScheme = selectedTheme;
    }

    function get() {
        return localStorage.getItem(storageKey) || "dark";
    }

    function set(theme) {
        const selectedTheme = theme === "light" ? "light" : "dark";

        localStorage.setItem(storageKey, selectedTheme);
        apply(selectedTheme);
    }

    apply(get());

    return {
        get,
        set,
        apply
    };
})();