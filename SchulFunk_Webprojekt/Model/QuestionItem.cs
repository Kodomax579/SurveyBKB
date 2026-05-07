namespace SchulFunk_Webprojekt.Model
{
    public class QuestionModel
    {

        public string Question { get; set; } = string.Empty;

        public List<AnswerModel> Options { get; set; } = new();

    }
}
