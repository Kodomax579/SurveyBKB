namespace SchulFunk_Webprojekt.Model
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string Tag { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Date { get; set; } = "";
        public string Artikel { get; set; } = "";
        public byte[] Image { get; set; } = Array.Empty<byte>();
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int CreatedUserID { get; set; }
    }
}
