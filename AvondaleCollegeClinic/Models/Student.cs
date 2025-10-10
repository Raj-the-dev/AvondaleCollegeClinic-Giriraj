using AvondaleCollegeClinic.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleCollegeClinic.Models
{

    public class Student
    {
        [Key]
        public string StudentID { get; set; } // Unique student ID

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string LastName { get; set; }

        [Display(Name = "Upload Image")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required, Display(Name = "Homeroom")]
        public string HomeroomID { get; set; }
        public Homeroom Homeroom { get; set; }

        public ICollection<Caregiver> Caregivers { get; set; } = new List<Caregiver>();

        public string? IdentityUserId { get; set; }
        public AvondaleCollegeClinicUser? AvondaleCollegeClinicUserAccount { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
