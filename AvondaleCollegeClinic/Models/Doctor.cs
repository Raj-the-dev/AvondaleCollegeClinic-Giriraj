using AvondaleCollegeClinic.Areas.Identity.Data;
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
        public string FirstName { get; set; } // Doctor's first name

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } // Doctor's last name


        // NEW: same pattern as Student
        [Display(Name = "Upload Image")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        [Required]
        [Display(Name = "Specialization")]
        public SpecializationType Specialization { get; set; } // Doctor's area of expertise (enum)

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } // Doctor's email address

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^02\d{1}[- ]?\d{3}[- ]?\d{4}$",
    ErrorMessage = "Please enter a valid NZ mobile number (e.g., 021-123-4567).")]
        public string Phone { get; set; } // Contact number for the doctor
        public string? IdentityUserId { get; set; }

        [ForeignKey("IdentityUserId")]
        public AvondaleCollegeClinicUser? AvondaleCollegeClinicUserAccount { get; set; }

        public bool WorksMon { get; set; } = true;
        public bool WorksTue { get; set; } = true;
        public bool WorksWed { get; set; } = true;
        public bool WorksThu { get; set; } = true;
        public bool WorksFri { get; set; } = true;
        public bool WorksSat { get; set; } = false;
        public bool WorksSun { get; set; } = false;

        // Daily window (local office time)
        [DataType(DataType.Time)]
        public TimeSpan DailyStartTime { get; set; } = new TimeSpan(9, 0, 0);

        [DataType(DataType.Time)]
        public TimeSpan DailyEndTime { get; set; } = new TimeSpan(17, 0, 0);

        // Slot size in minutes (e.g., 30)
        public int SlotMinutes { get; set; } = 30;

        public ICollection<Appointment> Appointments { get; set; } // Appointments assigned
        public ICollection<MedicalRecord> MedicalRecords { get; set; } // Medical records created by this doctor
    }
}
