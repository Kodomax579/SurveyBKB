namespace SurveyService.Data
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<AnswerModel> Options { get; set; } = new();
    }
}
