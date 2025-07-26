using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<List<Feedback>> ShowFeedbackByBarberId(int barberid)
        {
            if(barberid == 0)
            {
                throw new ArgumentException("O id não pode ser zero aqui");
            }
            if(barberid == null)
            {
                throw new ArgumentNullException("O id é nulo ou inválido");
            }
            var feedback = await _feedback.ShowFeedbackByBarberId(barberid);
            if(feedback == null)
            {
                throw new ArgumentNullException("Ainda não existe Feedback para o barbeiro selecionado.");
            }

            return feedback;

        }

    }
}
