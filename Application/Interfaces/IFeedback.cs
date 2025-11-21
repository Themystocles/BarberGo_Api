using Domain.Entities;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFeedback
    {
        Task<List<FeedbackDto>> ShowFeedbackByBarberId(int barberId);
        Task<bool> HasCommentAsync(int userId, int barberId);

        Task<Feedback> CreateFeedback(Feedback feedback);

        Task<Feedback> GetFeedbackByIdAsync(int id); 

        Task<Feedback> UpdateFeedbackAsync(Feedback feedback);


      




    }
}
