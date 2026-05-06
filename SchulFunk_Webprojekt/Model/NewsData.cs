namespace SchulFunk_Webprojekt.Model
{
    public static class NewsData
    {
        public static List<NewsItem> NewsListe { get; } = new()
        {
            new NewsItem
            {
                Id = 1,
                Tag = "Projekte",
                Title = "Neue Projektwoche geplant",
                Description = "Bald startet eine neue Projektwoche. Weitere Informationen folgen über SchulFunk.",
                Date = "Heute",
                Artikel = "Die neue Projektwoche startet bald. Schülerinnen und Schüler können zwischen verschiedenen Projekten wählen. Weitere Informationen zur Anmeldung und zum Ablauf werden rechtzeitig bekannt gegeben.",
                IconPath = "lib/images/Icon-News.png"
            },
            new NewsItem
            {
                Id = 2,
                Tag = "Essen",
                Title = "Neue Cafeteria-Öffnungszeiten",
                Description = "Ab nächster Woche gelten neue Öffnungszeiten für die Cafeteria.",
                Date = "Heute",
                Artikel = "Ab nächster Woche gelten neue Öffnungszeiten für die Cafeteria. Bitte achtet auf die Aushänge im Eingangsbereich.",
                IconPath = "lib/images/Icon-News.png"
            },
            new NewsItem
            {
                Id = 3,
                Tag = "Info",
                Title = "Informationen zur Projektpräsentation",
                Description = "Die Präsentationen der aktuellen Projekte finden am Freitag statt.",
                Date = "Diese Woche",
                Artikel = "Die Präsentationen der aktuellen Projekte finden am Freitag in den jeweiligen Klassenräumen statt.",
                IconPath = "lib/images/Icon-News.png"
            }
        };
    }
}