using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class Student
    {
        [Key]
        public string StudentID { get; set; } // Unique student ID

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Photo URL")]
        public string Photo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Homeroom")]
        public string HomeroomID { get; set; }

        [Required]
        [Display(Name = "Caregiver")]
        public int CaregiverID { get; set; }

        public Homeroom Homeroom { get; set; }
        public Caregiver Caregiver { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
