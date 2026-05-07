using Contracts.Protos;
using Grpc.Core;
using SurveyService.Feature;

namespace SurveyService.Services
{
    public class GrpcSurveyService(Feature.SurveyService surveyService) : Survey.SurveyBase
    {
        public override async Task<GetAllSurveysResponse> GetAllSurveys(GetAllSurveysRequest request, ServerCallContext context)
        {
            var listOfSurveys = await surveyService.GetAllSurveysAsync();

            var response = new GetAllSurveysResponse();

            foreach (var entity in listOfSurveys)
            {
                response.Surveys.Add(entity.ToProto());
            }

            return response;
        }

        public override async Task<CreateSurveyResponse> CreateSurvey(CreateSurveyRequest request, ServerCallContext context)
        {
            var survey = request.Survey.ToModel();

            var response = await surveyService.CreateSurveyAsync(survey);

            return new CreateSurveyResponse{ Success = response };
        }

        public override async Task<UpdateSurveyAnswerResponse> UpdateSurveyAnswer(UpdateSurveyAnswerRequest request, ServerCallContext context)
        {
            var response = await surveyService.UpdateAnswerSelectionAsync(request.UserId, request.AnswerId);
            return new UpdateSurveyAnswerResponse { Success = response };
        }

        public override Task<DeleteSurveyResponse> DeleteSurvey(DeleteSurveyRequest request, ServerCallContext context)
        {
            return base.DeleteSurvey(request, context);
        }
    }
}
