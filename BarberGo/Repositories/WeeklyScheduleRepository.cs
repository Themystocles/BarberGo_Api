﻿using BarberGo.Data;
using BarberGo.Entities;
using BarberGo.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberGo.Repositories
{
    public class WeeklyScheduleRepository : IWeeklySchedule
    {
        private readonly DataContext _context;

        public WeeklyScheduleRepository(DataContext context)
        {
            _context = context;
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
    }
}
