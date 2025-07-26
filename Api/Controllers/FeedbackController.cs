using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : GenericRepositoryController<Feedback>
    {
        private readonly FeedbackServices _feedbackServices;

        public FeedbackController(GenericRepositoryServices<Feedback> genericRepositoryServices, FeedbackServices feedbackServices)
            : base(genericRepositoryServices)
        {
            _feedbackServices = feedbackServices;
            
        }
        [AllowAnonymous]
        [HttpGet("get/{Barberid}")]
        public async Task <IActionResult> GetFeedbacksByBarberIdAsync(int id)
        {
            var feedbacks = await _feedbackServices.ShowFeedbackByBarberId(id);

            return Ok(feedbacks);
        }



    }
}
