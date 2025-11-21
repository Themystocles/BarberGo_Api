using Domain.Entities;
using Application.DTOs;
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
        .Include(a => a.AppUser)
        .Include(b => b.Barber)
        .Select(f => new FeedbackDto
        {
            Id = f.Id,
            AppUserName = f.AppUser.Name,
            BarberName = f.Barber.Name,
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
        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _context.Feedback.FindAsync(id);

            return feedback;

        }
        public async Task<Feedback> UpdateFeedbackAsync(Feedback feedback)
        {
            _context.Feedback.Update(feedback);
            await _context.SaveChangesAsync();

            return feedback;
        }

        public async Task<bool> HasCommentAsync(int userId, int barberId)
        {
            return await _context.Feedback
            .AnyAsync(f => f.AppUserId == userId && f.BarberId == barberId);
        }

        
    }

}
