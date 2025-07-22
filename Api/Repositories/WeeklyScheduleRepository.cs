using AutoMapper;
using Persistence.Data;
using Domain.Entities;
using Domain.Entities.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
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

        public async Task<List<DateTime>> GetAvailableSlotsAsync(DateTime date, int? barberId)
        {
            var dayOfWeek = date.DayOfWeek;

            var schedules = await _context.weeklySchedules
                .Where(w => w.DayOfWeek == dayOfWeek && w.BarberId == barberId)
                .ToListAsync();

            date = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);

            var appointments = await _context.Appointments
                .Where(a => a.DateTime.Date == date.Date &&  a.BarberId == barberId)
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
                .Where(w => w.DayOfWeek == schedule.DayOfWeek && w.BarberId == schedule.BarberId)
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
                throw new ArgumentNullException(nameof(schedule));

            var conflict = await _context.weeklySchedules
                .Where(w => w.DayOfWeek == schedule.DayOfWeek
                            && w.BarberId == schedule.BarberId
                            && w.Id != schedule.Id)
                .Where(w => w.StartTime < schedule.EndTime && w.EndTime > schedule.StartTime)
                .AnyAsync();

            if (conflict)
            {
                throw new ArgumentException("Existem conflitos de horários nesse dia");
            }

            var scheduleExist = await _context.weeklySchedules.FindAsync(schedule.Id);
            if (scheduleExist == null)
                throw new ArgumentNullException(nameof(scheduleExist));

            // Atualizar manualmente os campos
            scheduleExist.StartTime = schedule.StartTime;
            scheduleExist.EndTime = schedule.EndTime;
            scheduleExist.IntervalMinutes = schedule.IntervalMinutes;
            scheduleExist.DayOfWeek = schedule.DayOfWeek;
            // atualize aqui o que for permitido

            _context.Update(scheduleExist);
            await _context.SaveChangesAsync();

            return scheduleExist;
        }

        public async Task<List<BarberDto>> GetUserForType()
        {
            var barbers = await _context.AppUsers
                .Where(t => t.Type == TipoUsuario.Administrator && t.IsMaster == false)
                .Select(t => new BarberDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    phone = t.Phone,
                    ProfilePictureUrl = t.ProfilePictureUrl
                    
                    
                })
                .ToListAsync();


            return barbers;
        }

        public async Task<List<WeeklySchedule>> GetWeeklyScheduleByBarberId(int BarberId)
        {
            var Weekly = await _context.weeklySchedules
                .Where(b => b.BarberId == BarberId)
                .ToListAsync();

            return Weekly;
        }
    }
}
