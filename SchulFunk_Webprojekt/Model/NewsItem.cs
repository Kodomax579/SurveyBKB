namespace SchulFunk_Webprojekt.Model
{
    public class NewsItem
    {
        public string Tag { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Date { get; set; } = "";
        public string Artikel { get; set; } = "";
        public string IconPath { get; set; } = "lib/images/Icon-News.png";
        public byte[] Image { get; set; } = Array.Empty<byte>();
    }
}
