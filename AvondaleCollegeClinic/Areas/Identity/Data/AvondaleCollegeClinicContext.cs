using AvondaleCollegeClinic.Areas.Identity.Data;
using AvondaleCollegeClinic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AvondaleCollegeClinic.Areas.Identity.Data;

public class AvondaleCollegeClinicContext : IdentityDbContext<AvondaleCollegeClinicUser>
{
    public AvondaleCollegeClinicContext(DbContextOptions<AvondaleCollegeClinicContext> options)
        : base(options)
    { }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Labtest> LabTests { get; set; }
        public DbSet<Homeroom> Homerooms { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Caregiver> Caregivers { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        // Composite or custom relationships if needed
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Caregiver)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.CaregiverID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Homeroom)
            .WithMany(h => h.Students)
            .HasForeignKey(s => s.HomeroomID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Student)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.StudentID);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorID);

        modelBuilder.Entity<Diagnosis>()
            .HasOne(d => d.Appointment)
            .WithOne(a => a.Diagnosis)
            .HasForeignKey<Diagnosis>(d => d.AppointmentID);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Diagnosis)
            .WithMany(d => d.Prescriptions)
            .HasForeignKey(p => p.DiagnosisID);

        modelBuilder.Entity<DoctorAvailability>()
            .HasOne(d => d.Doctor)
            .WithMany(a => a.Availabilities)
            .HasForeignKey(d => d.DoctorID);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Student)
            .WithMany(s => s.MedicalRecords)
            .HasForeignKey(m => m.StudentID);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Doctor)
            .WithMany(d => d.MedicalRecords)
            .HasForeignKey(m => m.DoctorID);

        modelBuilder.Entity<Labtest>()
            .HasOne(l => l.MedicalRecord)
            .WithMany(r => r.Labtests)
            .HasForeignKey(l => l.RecordID);

        modelBuilder.Entity<Homeroom>()
            .HasOne(h => h.Teacher)
            .WithMany(t => t.Homerooms)
            .HasForeignKey(h => h.TeacherID);

        base.OnModelCreating(modelBuilder);
    }
}
