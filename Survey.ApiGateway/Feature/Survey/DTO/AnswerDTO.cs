namespace Survey.ApiGateway.Feature.Survey.DTO
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public string Answers { get; set; } = string.Empty;
        public int NumberOfSelectedAnswer { get; set; }
        public int QuestionModelId { get; set; }
    }
}
