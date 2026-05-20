using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Survey.ApiGateway.Feature.Survey;
using Survey.ApiGateway.Feature.Survey.Models; // Deine Models

namespace Survey.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController(
        IHubContext<RealtimeHub.RealtimeHub> hubContext,
        SurveyService surveyService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllSurveys()
        {
            return Ok(await surveyService.GetAllSurveys());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSurveyById(int id)
        {
            var survey = await surveyService.GetSurveyById(id);
            if (survey == null) return NotFound();

            return Ok(survey);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyModel survey)
        {
            if (survey == null) return BadRequest();

            var createdSurvey = await surveyService.CreateSurvey(survey);

            if (createdSurvey == null)
            {
                return BadRequest(false);
            }

            await hubContext.Clients.All.SendAsync("ReceiveNewSurvey", createdSurvey);

            return Ok(createdSurvey);
        }

        [HttpPut("vote/{answerId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAnswerSelection(int answerId, [FromQuery] int userId)
        {
            var success = await surveyService.IncrementAnswerCount(answerId);

            if (!success)
            {
                return BadRequest("Fehler beim Speichern der Abstimmung.");
            }

            await hubContext.Clients.All.SendAsync("SurveyVoteUpdated", answerId);

            return Ok(true);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSurvey(int id)
        {
            var existingSurvey = await surveyService.GetSurveyById(id);
            if (existingSurvey == null)
            {
                return NotFound();
            }

            await surveyService.DeleteSurvey(id);

            await hubContext.Clients.All.SendAsync("SurveyDeleted", id);

            return Ok(true);
        }
    }
}