using Contracts.Protos;
using Grpc.Core;
using NewsService.Feature;

namespace NewsService.Services
{
    public class GrpcsNewsService(Feature.NewsService newsService) : News.NewsBase
    {
        public override async Task<GetAllNewsResponse> GetAllNews(GetAllNewsRequest request, ServerCallContext context)
        {
            var newsEntities = await newsService.GetAllNewsAsync();

            var response = new GetAllNewsResponse();

            foreach (var entity in newsEntities)
            {
                response.News.Add(entity.ToProto());
            }

            return response;
        }

        public override async Task<GetNewsByIdResponse> GetNewsById(GetNewsByIdRequest request, ServerCallContext context)
        {
            var entity = await newsService.GetNewsByIdAsync(request.Id);

            if (entity == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"News mit ID {request.Id} nicht gefunden."));
            }

            return new GetNewsByIdResponse
            {
                News = entity.ToProto()
            };
        }

        public override async Task<CreateNewsResponse> CreateNews(CreateNewsRequest request, ServerCallContext context)
        {
            var entity = request.News.ToModel();

            var success = await newsService.CreateNewsAsync(entity);

            return new CreateNewsResponse { Success = success };
        }

        public override async Task<UpdateNewsResponse> UpdateNews(UpdateNewsRequest request, ServerCallContext context)
        {
            var entity = request.News.ToModel();
            var success = await newsService.UpdateNewsAsync(entity);

            return new UpdateNewsResponse { Success = success };
        }

        public override async Task<DeleteNewsResponse> DeleteNews(DeleteNewsRequest request, ServerCallContext context)
        {
            var success = await newsService.DeleteNewsAsync(request.Id);

            return new DeleteNewsResponse { Success = success };
        }
    }
}
