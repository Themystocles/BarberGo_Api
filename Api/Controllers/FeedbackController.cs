using Application.Services;
using Domain.Entities;
using Domain.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        [HttpGet("get/{barberId}")]  // padrão camelCase e nome consistente
        public async Task<IActionResult> GetFeedbacksByBarberIdAsync(int barberId)
        {
            var feedbacks = await _feedbackServices.ShowFeedbackByBarberId(barberId);

            return Ok(feedbacks);
        }
    }
}
