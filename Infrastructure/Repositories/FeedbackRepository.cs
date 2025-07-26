using Domain.Entities;
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

      

        public async Task<List<Feedback>> ShowFeedbackByBarberId(int barberId)
        {
            var feedback = await _context.Feedback
                .Where(f => f.BarberId == barberId)
                .ToListAsync();

            return feedback;
            
        }
    }
}
