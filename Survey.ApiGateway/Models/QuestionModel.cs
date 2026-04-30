namespace Survey.ApiGateway.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
    }
}
