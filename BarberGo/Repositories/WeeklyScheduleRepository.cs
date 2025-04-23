using AutoMapper;
using BarberGo.Data;
using BarberGo.Entities;
using BarberGo.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BarberGo.Repositories
{
    public class WeeklyScheduleRepository : IWeeklySchedule
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public WeeklyScheduleRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DateTime>> GetAvailableSlotsAsync(DateTime date, int? barberId = null)
        {
            var dayOfWeek = date.DayOfWeek;

            var schedules = await _context.weeklySchedules
                .Where(w => w.DayOfWeek == dayOfWeek && (barberId == null || w.BarberId == barberId))
                .ToListAsync();
           
            var appointments = await _context.Appointments
                .Where(a => a.DateTime.Date == date.Date && (barberId == null || a.BarberId == barberId))
                .Select(a => a.DateTime.TimeOfDay)
                .ToListAsync();

            var availableSlots = new List<DateTime>();

            foreach (var schedule in schedules)
            {
                var currentTime = schedule.StartTime;
                while (currentTime + TimeSpan.FromMinutes(schedule.IntervalMinutes) <= schedule.EndTime)
                {
                    if (!appointments.Contains(currentTime))
                    {
                        availableSlots.Add(date.Date + currentTime);
                    }

                    currentTime = currentTime.Add(TimeSpan.FromMinutes(schedule.IntervalMinutes));
                }
            }

            return availableSlots;
        }

        public async Task<WeeklySchedule> CreateNewSchedule(WeeklySchedule schedule)
        {
            var schedules = _context.weeklySchedules
                .Where(w => w.DayOfWeek == schedule.DayOfWeek)
                 .Where(w => schedule.StartTime < w.EndTime && schedule.EndTime > w.StartTime);
            
            if (schedules.Any())
            {
                throw new ArgumentException("Existem conflitos de horarios nesse dia");
            }

            _context.Add(schedule);
            _context.SaveChanges();

          


            return schedule;

        }

        public async Task<WeeklySchedule> UpdateScheduleAsync(WeeklySchedule schedule)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }
            var conflict = _context.weeklySchedules
                .Where(w => w.DayOfWeek == schedule.DayOfWeek && w.Id != schedule.Id)
                 .Where(w => schedule.StartTime < w.EndTime && schedule.EndTime > w.StartTime);
            if (conflict.Any())
            {
                throw new ArgumentException("Existem conflitos de horarios nesse dia");
            }

            var scheduleExist = await _context.weeklySchedules.FindAsync(schedule.Id);
            if (scheduleExist == null)
            { 
                throw new ArgumentNullException(nameof(scheduleExist));
            }
            _mapper.Map(schedule, scheduleExist);

             _context.Update(scheduleExist);
              await _context.SaveChangesAsync();


            return scheduleExist;




        }
    }
}
