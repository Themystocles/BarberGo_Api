using Application.Services;
using AutoMapper;
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
        [HttpGet("get/{barberId}")]  
        public async Task<IActionResult> GetFeedbacksByBarberIdAsync(int barberId)
        {
            var feedbacks = await _feedbackServices.ShowFeedbackByBarberId(barberId);

            return Ok(feedbacks);
        }
        [AllowAnonymous]
        [HttpPost("create-feedback")]
        public virtual async Task<ActionResult<Feedback>> CreateEntity(FeedbackDto dto)
        {
            var createdFeedback = await _feedbackServices.Createfeedback(dto);

            return Created("", createdFeedback);
        }


    }
}
