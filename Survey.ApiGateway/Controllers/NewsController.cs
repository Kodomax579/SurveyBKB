using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Survey.ApiGateway.Feature.News;
using Survey.ApiGateway.Feature.News.Models;
using Survey.ApiGateway.RealtimeHub;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController(IHubContext<RealtimeHub.RealtimeHub> realtimeHub, NewsService newsService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllNews()
        {
            return Ok(await newsService.GetAllNews());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetNewsById(int id)
        {
            return Ok(await newsService.GetNewsById(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNews([FromForm] NewsModel news, IFormFile? image)
        {
            if (image != null)
            {
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                news.ImageLink = $"/uploads/{fileName}";
            }
            var result = await newsService.CreateNews(news);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] NewsModel news)
        {
            if(news == null)
            {
                return BadRequest();
            }

            var result = await newsService.UpdateNews(id, news);

            if (result == null)
            {
                return BadRequest(false);
            }

            await realtimeHub.Clients.All.SendAsync("NewsUpdated", result);

            return Ok(true);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNews(int id)
        {
            if(!(await newsService.DeleteNews(id)))
            {
                return NotFound();
            }

            await realtimeHub.Clients.All.SendAsync("NewsDeleted", id);
            return Ok(true);
        }
    }
}
