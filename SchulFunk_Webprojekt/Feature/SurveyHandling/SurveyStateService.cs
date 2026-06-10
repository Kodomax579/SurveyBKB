using SchulFunk_Webprojekt.Feature.NewsHandling.Model;
using SchulFunk_Webprojekt.Feature.SurveyHandling.Model;

namespace SchulFunk_Webprojekt.Feature.SurveyHandling
{
    public class SurveyStateService
    {
        public SurveyStateService(SignalRHub.SignalRHub signalRHub)
        {
            signalRHub.OnSurveyCreated += AddSurveyItem;
            signalRHub.OnSurveyDeleted += DeleteSurveyItem;
            signalRHub.OnSurveyVoteUpdate += UpdateSurveyFromHub;
        }

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

        public void DeleteSurveyItem(int id)
        {
            var news = surveyItems.FirstOrDefault(n => n.Id == id);

            if (news == null)
            {
                return;
            }

            surveyItems.Remove(news);
            NotifyStateChanged();
        }

        public void AddSurveyItem(SurveyModel SurveyItem)
        {
            surveyItems.Add(SurveyItem);
            NotifyStateChanged();
        }

        public void UpdateSurveyFromHub(SurveyModel updatedSurvey)
        {
            if (updatedSurvey == null)
            {
                return;
            }

            var i = surveyItems.FindIndex(n => n.Id == updatedSurvey.Id);

            if (i == -1)
            {
                // If survey not present, add it
                surveyItems.Add(updatedSurvey);
            }
            else
            {
                surveyItems[i] = updatedSurvey;
            }

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
