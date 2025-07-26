using Domain.Entities.DTOs;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FeedbackServices
    {
        private readonly IFeedback _feedback;

        public FeedbackServices(IFeedback feedback)
        {
            _feedback = feedback;
        }

        public async Task<List<FeedbackDto>> ShowFeedbackByBarberId(int barberId)
        {
            if (barberId <= 0)
            {
                throw new ArgumentException("O id não pode ser zero ou negativo.");
            }

            var feedbacks = await _feedback.ShowFeedbackByBarberId(barberId);

            if (feedbacks == null || feedbacks.Count == 0)
            {
                throw new ArgumentNullException("Ainda não existe Feedback para o barbeiro selecionado.");
            }

            return feedbacks;
        }
    }
}
