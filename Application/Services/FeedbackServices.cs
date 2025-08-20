using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Domain.Interfaces;

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

        public async Task<Feedback> Createfeedback(CreateFeedbackDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException("A Entidade Feedback não pode ser nula aqui.");
            }
            bool jaComentou = await _feedback.HasCommentAsync(dto.AppUserId, dto.BarberId);

            if (jaComentou)
                throw new InvalidOperationException("O usuário já comentou para este barbeiro. Edite seu comentário se precisar");

            var feedback = _mapper.Map<Feedback>(dto);

            var feedbackCriado = await _feedback.CreateFeedback(feedback);


            return feedbackCriado;

        }
        public async Task<Feedback> GetFeedback(int id)
        {
            if (_feedback == null)
                throw new NullReferenceException("O feedback par ao id passado não foi encontrado ou não existe");

           var feedback = await _feedback.GetFeedbackByIdAsync(id);

            return feedback;

        }
        public async Task<Feedback> UpdateFeedback(int id, FeedbackDto dto)
        {
            if (id == null)
            {
                throw new ArgumentNullException("O Id não pode ser nulo aqui.");
            }
            if (id <= 0)
            {
                throw new ArgumentException("O id é inválido", nameof(id));
            }

            var feedback = await  _feedback.GetFeedbackByIdAsync(id);

            if(feedback == null)
            {
                throw new NullReferenceException("O feedback par ao id passado não foi encontrado ou não existe");
            }

            _mapper.Map(dto, feedback);

            await _feedback.UpdateFeedbackAsync(feedback);

            return feedback;





        }
    }
}
