using Domain.Entities;
using Domain.Entities.DTOs;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FeedbackRepository : IFeedback
    {
        private readonly DataContext _context;

        public FeedbackRepository(DataContext context)
        {       
            _context = context;
           
        }
        public async Task<List<FeedbackDto>> ShowFeedbackByBarberId(int barberId)
        {
            var feedbackDtos = await _context.Feedback
        .Where(f => f.BarberId == barberId)
        .Select(f => new FeedbackDto
        {
            Id = f.Id,
            AppUserId = f.AppUserId,
            BarberId = f.BarberId,
            Rating = f.Rating,
            Comment = f.Comment
        })
        .ToListAsync();

            return feedbackDtos;

        }
        public async Task<Feedback> CreateFeedback(Feedback feedback)
        {
            _context.Feedback.Add(feedback);
           await _context.SaveChangesAsync();

            return feedback;
        }

    }

}
