using AutoMapper;
using Domain.Entities;
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
        private readonly IMapper _mapper;

        public FeedbackServices(IFeedback feedback,  IMapper mapper)
        {
            _feedback = feedback;
            _mapper = mapper;
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

        public async Task<Feedback> Createfeedback(FeedbackDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("A Entidade Feedback não pode ser nula aqui.");
            }
            
            var feedback = _mapper.Map<Feedback>(dto);

            var feedbackCriado = await _feedback.CreateFeedback(feedback);


            return feedbackCriado;

        }
    }
}
