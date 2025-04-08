using BarberGo.Entities;
using BarberGo.Data;
using Microsoft.EntityFrameworkCore;

namespace BarberGo.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Haircut> Haircuts { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<WeeklySchedule> weeklySchedules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
       .HasOne(a => a.Client)
       .WithMany()
       .HasForeignKey(a => a.ClientId)
       .OnDelete(DeleteBehavior.NoAction);

         modelBuilder.Entity<Appointment>()
        .HasOne(a => a.Barber)
           .WithMany()
                .HasForeignKey(a => a.BarberId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Haircut)
                .WithMany()
                .HasForeignKey(a => a.HaircutId)
                .OnDelete(DeleteBehavior.Cascade);



        }
    }
}