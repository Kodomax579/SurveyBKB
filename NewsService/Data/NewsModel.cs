namespace NewsService.Data
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string PreviewText { get; set; } = string.Empty;
        public string MainText { get; set; } = string.Empty;
        public byte[] Image { get; set; } = Array.Empty<byte>();
        public DateTime CreatedAt { get; set; }
        public int CreatedUserId { get; set; }
    }
}
