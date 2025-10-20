using AvondaleCollegeClinic.Areas.Identity.Data; // Identity user type
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleCollegeClinic.Models
{
    public enum SpecializationType
    {
        General,
        Pediatric,
        MentalHealth,
        SportsMedicine,
        Dermatology
    }

    public class Doctor
    {
        [Key]
        [StringLength(12)]
        [Display(Name = "Doctor ID")]
        public string DoctorID { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Image upload pattern: file from the form is NOT stored in DB
        [Display(Name = "Upload Image")]
        [NotMapped]                       // EF ignores this (no column)
        public IFormFile? ImageFile { get; set; }

        // We store only the path/URL to the saved image in the DB
        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        [Required]
        [Display(Name = "Specialization")]
        public SpecializationType Specialization { get; set; } // enum: fixed set of expertise

        [Required]
        [EmailAddress]
        [StringLength(50)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }



        // Optional link to ASP.NET Identity account for this doctor
        public string? IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public AvondaleCollegeClinicUser? AvondaleCollegeClinicUserAccount { get; set; }

        // Work pattern (simple booleans for days available)
        public bool WorksMon { get; set; } = true;
        public bool WorksTue { get; set; } = true;
        public bool WorksWed { get; set; } = true;
        public bool WorksThu { get; set; } = true;
        public bool WorksFri { get; set; } = true;
        public bool WorksSat { get; set; } = false;
        public bool WorksSun { get; set; } = false;

        // Daily clinic window (local time). Used to build bookable slots.
        [DataType(DataType.Time)]
        public TimeSpan DailyStartTime { get; set; } = new TimeSpan(9, 0, 0);
        [DataType(DataType.Time)]
        public TimeSpan DailyEndTime { get; set; } = new TimeSpan(17, 0, 0);

        // Length of each appointment slot (minutes)
        public int SlotMinutes { get; set; } = 30;

        // Navigation: what this doctor owns/relates to
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
