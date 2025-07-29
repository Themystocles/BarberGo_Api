using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.DTOs
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string AppUserName { get; set; }
        public string BarberName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
