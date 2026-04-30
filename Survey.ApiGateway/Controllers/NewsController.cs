using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Models;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpGet("/GetAllNews")]
        public IActionResult GetAllNews()
        {
            // Implement your logic to get all news here
            return Ok();
        }

        [HttpGet("/GetNewsById/{id}")]
        public IActionResult GetNewsById(int id)
        {
            // Implement your logic to get news by id here
            return Ok();
        }

        [HttpPost("/CreateNews")]
        public IActionResult CreateNews([FromBody] NewsModel news)
        {
            // Implement your logic to create news here
            return Ok();
        }

        [HttpPut("/UpdateNews/{id}")]
        public IActionResult UpdateNews(int id, [FromBody] NewsModel news)
        {
            // Implement your logic to update news here
            return Ok();
        }

        [HttpDelete("/DeleteNews/{id}")]
        public IActionResult DeleteNews(int id)
        {
            // Implement your logic to delete news here
            return Ok();
        }
    }
}
