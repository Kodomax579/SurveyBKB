using SchulFunk_Webprojekt.Feature.NewsHandling.Model;
using SchulFunk_Webprojekt.Feature.SurveyHandling.Model;

namespace SchulFunk_Webprojekt.Feature.SurveyHandling
{
    public class SurveyStateService
    {
        private List<SurveyModel> surveyItems = new();

        public event Action OnChange;

        public List<SurveyModel> GetAllSurveys()
        {
            return surveyItems;
        }

        public void SetSurvey(List<SurveyModel?> surveyItem)
        {
            if (surveyItem == null)
            {
                return;
            }
            surveyItems = surveyItem;
            NotifyStateChanged();
        }

        public bool UpdateSurveyItem(SurveyModel updatedSurvey)
        {
            var i = surveyItems.FindIndex(n => n.Id == updatedSurvey.Id);

            if (i == -1)
            {
                return false;
            }

            surveyItems[i] = updatedSurvey;
            NotifyStateChanged();
            return true;
        }

        public bool DeleteSurveyItem(int id)
        {
            var news = surveyItems.FirstOrDefault(n => n.Id == id);

            if (news == null)
            {
                return false;
            }

            surveyItems.Remove(news);
            NotifyStateChanged();
            return true;
        }

        public void AddSurveyItem(SurveyModel SurveyItem)
        {
            surveyItems.Add(SurveyItem);
            NotifyStateChanged();
        }

        public void UpdateAnswerVote(int answerId)
        {
            var answer = surveyItems
                .SelectMany(s => s.Questions)
                .SelectMany(s => s.AnswerModels)
                .FirstOrDefault(answer => answer.Id == answerId);

            if (answer == null)
            {
                return;
            }

            answer.NumberOfSelectedAnswer++;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
