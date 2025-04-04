using System.ComponentModel.DataAnnotations;

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
        public int DoctorID { get; set; } // Unique identifier for the doctor

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

        [Display(Name = "Photo URL")]
        public string Photo { get; set; } // URL or file path to the doctor's photo

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
        public string Phone { get; set; } // Contact number for the doctor

        public ICollection<DoctorAvailability> Availabilities { get; set; } // Linked availability slots
        public ICollection<Appointment> Appointments { get; set; } // Appointments assigned
        public ICollection<MedicalRecord> MedicalRecords { get; set; } // Medical records created by this doctor
    }
}
