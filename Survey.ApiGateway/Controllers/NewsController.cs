using Contracts.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Models;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController(News.NewsClient grpcClient) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllNews()
        {
            var grpcRequest = new GetAllNewsRequest();
            var grpcResponse = await grpcClient.GetAllNewsAsync(grpcRequest);



            var models = grpcResponse.News.Adapt<List<NewsModel>>();

            return Ok(models);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetNewsById(int id)
        {
            try
            {
                var grpcRequest = new GetNewsByIdRequest { Id = id };
                var grpcResponse = await grpcClient.GetNewsByIdAsync(grpcRequest);

                var model = grpcResponse.News.Adapt<NewsModel>();

                return Ok(model);
            }
            catch (RpcException ex)
            {
                return NotFound(new { Message = ex.Status.Detail });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNews([FromBody] NewsModel news)
        {
            var protoNews = news.Adapt<NewsMessage>();

            var grpcRequest = new CreateNewsRequest { News = protoNews };
            var grpcResponse = await grpcClient.CreateNewsAsync(grpcRequest);

            if (grpcResponse.Success)
            {
                return Ok(true);
            }

            return BadRequest(false);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] NewsModel news)
        {
            news.Id = id;

            var protoNews = news.Adapt<NewsMessage>();
            var grpcRequest = new UpdateNewsRequest { News = protoNews };

            var grpcResponse = await grpcClient.UpdateNewsAsync(grpcRequest);

            if (grpcResponse.Success)
            {
                return Ok(true);
            }

            return BadRequest(false);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var grpcRequest = new DeleteNewsRequest { Id = id };
            var grpcResponse = await grpcClient.DeleteNewsAsync(grpcRequest);

            if (grpcResponse.Success)
            {
                return Ok(true);
            }

            return BadRequest(false);
        }
    }
}
