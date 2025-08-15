using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateFeedbackDto
    {
        public int AppUserId { get; set; }
        public int BarberId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
