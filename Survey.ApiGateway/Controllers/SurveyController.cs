using Contracts.Protos;
using Google.Protobuf;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Models;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController(Contracts.Protos.Survey.SurveyClient grpcClient) : ControllerBase
    {
        [HttpGet("/GetAllSurveys")]
        public async Task<IActionResult> GetAllSurveys()
        {
            var request = new GetAllSurveysRequest();
            var response = await grpcClient.GetAllSurveysAsync(request);

            // Wir bereiten eine leere Liste für das Ergebnis vor
            var models = new List<SurveyModel>();

            // Wir gehen jede gRPC-Nachricht einzeln durch
            foreach (var proto in response.Surveys)
            {
                var model = new SurveyModel
                {
                    Title = proto.Title,
                    GroupId = proto.GroupId,

                    CreatedAt = DateOnly.FromDateTime(proto.CreatedAt.ToDateTime().ToLocalTime()),
                    OnlineUntil = DateOnly.FromDateTime(proto.OnlineUntil.ToDateTime().ToLocalTime()),

                    Classes = proto.Classes.ToList(),
                    UserIDs = proto.UserIDs.ToList(),

                    Questions = new List<QuestionModel>()
                };

                foreach (var q in proto.Questions)
                {
                    var questionModel = new QuestionModel
                    {
                        Question = q.Question,
                        Options = new List<AnswerModel>()
                    };

                    foreach (var o in q.Options)
                    {
                        questionModel.Options.Add(new AnswerModel
                        {
                            options = o.Options,
                            NumberOfSelectedAnswer = o.NumberOfSelectedAnswer
                        });
                    }

                    model.Questions.Add(questionModel);
                }

                models.Add(model);
            }

            return Ok(models);
        }

        [HttpPost("/CreateSurvey")]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyModel survey)
        {
            var protoSurvey = new SurveyMessage
            {
                Title = survey.Title,
                GroupId = survey.GroupId,
                CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(survey.CreatedAt.ToDateTime(TimeOnly.MinValue).ToUniversalTime()),
                OnlineUntil = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(survey.OnlineUntil.ToDateTime(TimeOnly.MinValue).ToUniversalTime())
            };

            if (survey.Classes != null)
            {
                protoSurvey.Classes.AddRange(survey.Classes);
            }

            if (survey.UserIDs != null)
            {
                protoSurvey.UserIDs.AddRange(survey.UserIDs);
            }

            if (survey.Questions != null)
            {
                foreach (var q in survey.Questions)
                {
                    var protoQuestion = new QuestionMessage { Question = q.Question };

                    if (q.Options != null)
                    {
                        foreach (var o in q.Options)
                        {
                            protoQuestion.Options.Add(new AnswerMessage
                            {
                                Options = o.options,
                                NumberOfSelectedAnswer = o.NumberOfSelectedAnswer
                            });
                        }
                    }
                    protoSurvey.Questions.Add(protoQuestion);
                }
            }

            var grpcRequest = new CreateSurveyRequest { Survey = protoSurvey };

            var response = await grpcClient.CreateSurveyAsync(grpcRequest);

            if (!response.Success)
            {
                return BadRequest(false);
            }

            return Ok(response.Success);
        }

        [HttpPut("/UpdateSurveyAnswer")]
        public async Task<IActionResult> UpdateAnswerSelction(int answerId, int userId)
        {
            var grpcRequest = new UpdateSurveyAnswerRequest { AnswerId = answerId, UserId = userId };
            var response = await grpcClient.UpdateSurveyAnswerAsync(grpcRequest);

            if(!response.Success)
            {
                return BadRequest(false);
            }
            return Ok(response.Success);
        }

        [HttpDelete("/DeleteSurvey/{id}")]
        public async Task<IActionResult> DeleteSurvey(int id)
        {
            var grpcRequest = new DeleteSurveyRequest { Id = id };
            var response = await grpcClient.DeleteSurveyAsync(grpcRequest);

            if(!response.Success)
            {
                return BadRequest(false);
            }
            return Ok(response.Success);
        }
    }
}
