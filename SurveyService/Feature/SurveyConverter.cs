using Contracts.Protos;
using Google.Protobuf.WellKnownTypes;
using SurveyService.Data;
using System.Linq;

namespace SurveyService.Feature
{
    public static class SurveyConverter
    {
        public static SurveyMessage ToProto(this SurveyModel model)
        {
            if (model == null) return new SurveyMessage();

            var message = new SurveyMessage
            {
                Title = model.Title,
                GroupId = model.GroupId,
                Email = model.CreatedEmail,
                CreatedAt = Timestamp.FromDateTime(model.CreatedAt.ToDateTime(TimeOnly.MinValue).ToUniversalTime()),
                OnlineUntil = Timestamp.FromDateTime(model.OnlineUntil.ToDateTime(TimeOnly.MinValue).ToUniversalTime())
            };

            if (model.Classes != null)
            {
                message.Classes.AddRange(model.Classes);
            }

            if (model.UserIDs != null)
            {
                message.UserIDs.AddRange(model.UserIDs);
            }

            if (model.Questions != null)
            {
                message.Questions.AddRange(model.Questions.Select(q =>
                {
                    var protoQuestion = new QuestionMessage { Question = q.Question };

                    if (q.Options != null)
                    {
                        protoQuestion.Options.AddRange(q.Options.Select(o => new AnswerMessage
                        {
                            Options = o.Answer,
                            NumberOfSelectedAnswer = o.NumberOfSelectedAnswer
                        }));
                    }
                    return protoQuestion;
                }));
            }

            return message;
        }

        public static SurveyModel ToModel(this SurveyMessage proto)
        {
            if (proto == null) return new SurveyModel();

            return new SurveyModel
            {
                Id = proto.Id,
                Title = proto.Title,
                GroupId = proto.GroupId,
                CreatedAt = DateOnly.FromDateTime(proto.CreatedAt.ToDateTime().ToLocalTime()),
                OnlineUntil = DateOnly.FromDateTime(proto.OnlineUntil.ToDateTime().ToLocalTime()),
                CreatedEmail = proto.Email,
                Classes = proto.Classes.ToList(),

                UserIDs = proto.UserIDs.ToList(),

                Questions = proto.Questions.Select(q => new QuestionModel
                {
                    Question = q.Question,
                    Options = q.Options.Select(o => new AnswerModel
                    {
                        Answer = o.Options,
                        NumberOfSelectedAnswer = o.NumberOfSelectedAnswer
                    }).ToList()
                }).ToList()
            };
        }
    }
}