using Microsoft.EntityFrameworkCore;
using System;

namespace WPF_Proj.Classes
{
    public class DBContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<AppointmentStories> AppointmentStories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=localhost\\SQLEXPRESS;Initial Catalog=db_wpf;Integrated Security=True;Trust Server Certificate=True",
                    options => options.EnableRetryOnFailure()
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Doctors
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MiddleName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Specialization).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(50);
            });

            // Patients
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patients");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MiddleName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Birthday).IsRequired();
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(50);
            });

            // AppointmentStories
            modelBuilder.Entity<AppointmentStories>(entity =>
            {
                entity.ToTable("AppointmentStories");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Patient_id).IsRequired();
                entity.Property(e => e.Doctor_id).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Diagnosis).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Recomendations).HasMaxLength(1000);
            });
        }
    }
}