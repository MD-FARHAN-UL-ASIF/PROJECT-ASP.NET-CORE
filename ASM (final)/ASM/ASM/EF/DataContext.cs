using ASM.Models;
using Microsoft.EntityFrameworkCore;
using ASM.Models.DTOs;

namespace ASM.EF
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<ASM.Models.DTOs.DoctorDTO>? DoctorDTO { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ASM.Models.DTOs.AppointmentDTO>? AppointmentDTO { get; set; }
        public DbSet<Ambulance> Ambulances { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
