namespace SchulFunk_Webprojekt.Model
{
    public static class SurveyData
    {
        public static List<ClassModel> AvailableClasses { get; } = new()
        {
            new ClassModel { ClassName = "5A" },
            new ClassModel { ClassName = "5B" },
            new ClassModel { ClassName = "6A" },
            new ClassModel { ClassName = "6B" },
            new ClassModel { ClassName = "7A" },
            new ClassModel { ClassName = "7B" },
            new ClassModel { ClassName = "8A" },
            new ClassModel { ClassName = "8B" },
            new ClassModel { ClassName = "9A" },
            new ClassModel { ClassName = "9B" },
            new ClassModel { ClassName = "10A" },
            new ClassModel { ClassName = "10B" }
        };

        public static List<SurveyModel> SurveyListe { get; } = new()
        {
            new SurveyModel
            {
                Title = "Gestaltung der Pausenbereiche",
                Description = "Mit dieser Umfrage möchten wir herausfinden, wie die Pausenbereiche verbessert werden können.",
                CreatorName = "Max Mustermann",
                GroupId = 1,
                CreatedAt = DateOnly.FromDateTime(DateTime.Today),
                OnlineUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(14)),
                Classes = new()
                {
                    new ClassModel { ClassName = "10A" },
                    new ClassModel { ClassName = "10B" }
                },
                Questions = new()
                {
                    new QuestionModel
                    {
                        Question = "Was wünschst du dir für die Pausenbereiche?",
                        Options = new()
                        {
                            new AnswerModel { options = "Mehr Sitzmöglichkeiten" },
                            new AnswerModel { options = "Mehr Schattenplätze" },
                            new AnswerModel { options = "Mehr Sportmöglichkeiten" }
                        }
                    },
                    new QuestionModel
                    {
                        Question = "Wie zufrieden bist du aktuell mit den Pausenbereichen?",
                        Options = new()
                        {
                            new AnswerModel { options = "Sehr zufrieden" },
                            new AnswerModel { options = "Geht so" },
                            new AnswerModel { options = "Nicht zufrieden" }
                        }
                    }
                }
            },

            new SurveyModel
            {
                Title = "Schulfest Planung",
                Description = "Mit dieser Umfrage sollen Ideen und Wünsche für das nächste Schulfest gesammelt werden.",
                CreatorName = "Schülervertretung",
                GroupId = 2,
                CreatedAt = DateOnly.FromDateTime(DateTime.Today.AddDays(-6)),
                OnlineUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                Classes = AvailableClasses.ToList(),
                Questions = new()
                {
                    new QuestionModel
                    {
                        Question = "Welche Aktion soll es beim Schulfest geben?",
                        Options = new()
                        {
                            new AnswerModel { options = "Essensstand", NumberOfSelectedAnswer = 12 },
                            new AnswerModel { options = "Sportturnier", NumberOfSelectedAnswer = 8 },
                            new AnswerModel { options = "Musikauftritt", NumberOfSelectedAnswer = 5 }
                        }
                    }
                }
            }
        };
    }
}