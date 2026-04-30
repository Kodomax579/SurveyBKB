using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Survey.ApiGateway.Models;

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        [HttpGet("/GetAllSurveys")]
        public IActionResult GetAllSurveys()
        {
            return Ok("Get all surveys");
        }

        [HttpGet("/GetSurveyById/{id}")]
        public IActionResult GetSurveyById(int id)
        {
            return Ok($"Get survey by id: {id}");
        }

        [HttpGet("/GetAllSurveysForGroup/{GroupId}")]
        public IActionResult GetAllSurveysForGroup(int GroupId)
        {
            return Ok($"Get all surveys for group with id: {GroupId}");
        }

        [HttpPost("/CreateSurvey")]
        public IActionResult CreateSurvey([FromBody] SurveyModel model)
        {
            return Ok("Create survey");
        }

        [HttpPut("/UpdateSurvey")]
        public IActionResult UpdateSurvey(int id, [FromBody] SurveyModel model)
        {
            return Ok($"Update survey with id: {id}");
        }

        [HttpDelete("/DeleteSurvey/{id}")]
        public IActionResult DeleteSurvey(int id)
        {
            return Ok($"Delete survey with id: {id}");
        }
    }
}
