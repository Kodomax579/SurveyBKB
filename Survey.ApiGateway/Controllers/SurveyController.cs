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
            if (userId <= 0)
            {
                return BadRequest("Ungültige Benutzer-ID.");
            }

            var success = await surveyService.IncrementAnswerCount(answerId, userId);

            if (!success)
            {
                // Could be because the answer was not found or the user already voted
                return Conflict("Benutzer hat bereits abgestimmt oder Fehler beim Speichern der Abstimmung.");
            }

            // Find the survey that contains the answer and send the full survey to clients
            var surveyContaining = (await surveyService.GetAllSurveys()).FirstOrDefault(s => s.Questions.SelectMany(q => q.Options).Any(o => o.Id == answerId));
            if (surveyContaining != null)
            {
                var updatedSurvey = await surveyService.GetSurveyById(surveyContaining.Id);
                if (updatedSurvey != null)
                {
                    await hubContext.Clients.All.SendAsync("SurveyVoteUpdated", updatedSurvey);
                }
            }

            return Ok(true);
        }

        [HttpPut("{id}/end")]
        [Authorize]
        public async Task<IActionResult> EndSurveyEarly(int id)
        {
            var existingSurvey = await surveyService.GetSurveyById(id);
            if (existingSurvey == null)
            {
                return NotFound("Umfrage nicht gefunden.");
            }

            // Service-Aufruf zum Ändern des Datums
            var success = await surveyService.EndSurveyEarly(id);

            if (!success)
            {
                return BadRequest("Umfrage konnte nicht beendet werden.");
            }

            // Aktualisiertes Objekt holen, um es an alle Clients zu senden
            var updatedSurvey = await surveyService.GetSurveyById(id);

            // SignalR: Alle Clients informieren, dass diese Umfrage nun aktualisiert (beendet) wurde
            await hubContext.Clients.All.SendAsync("SurveyVoteUpdated", updatedSurvey);

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

            return Ok(id);
        }
    }
}