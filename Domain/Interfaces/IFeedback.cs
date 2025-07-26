using Domain.Entities;
using Domain.Entities.DTOs;
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

      
        
    }
}
