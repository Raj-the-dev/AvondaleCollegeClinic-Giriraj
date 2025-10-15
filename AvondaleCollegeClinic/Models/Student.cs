using AvondaleCollegeClinic.Areas.Identity.Data; // your custom Identity user type
using AvondaleCollegeClinic.Validation;          // custom validators: NotFuture, AgeRange
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using AvondaleCollegeClinic.Validation;       // duplicate; safe to remove

namespace AvondaleCollegeClinic.Models
{
    public class Student
    {
        // Identity / Basic Info 

        [Key]                                       // primary key for Students table
        public string StudentID { get; set; }       // e.g., school-issued ID string

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        // Only letters/spaces allowed keeps data clean 
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string LastName { get; set; }

        //  Profile Image (standard upload pattern) 
        // ImageFile the uploaded file from the form
        [Display(Name = "Upload Image")]
        [NotMapped]                                  // EF ignores this property: no column created
        public IFormFile? ImageFile { get; set; }

        // ImagePath where we saved the file e.g., "/uploads/students/123.jpg"
        // We store just the path/URL in the DB.
        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        //  Dates & Validation 
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [NotFuture(ErrorMessage = "Date of birth cannot be in the future.")] // custom: blocks future dates
        [AgeRange(12, 21, ErrorMessage = "Students must be between 12 and 21 years old.")] // custom: inclusive age limits
        public DateTime DOB { get; set; }

        // Contact
        [Required]
        [EmailAddress]                               // built-in email format check
        public string Email { get; set; }

        // Homeroomrequired 1-to-many one homeroom has many students
        [Required, Display(Name = "Homeroom")]
        public string HomeroomID { get; set; }       // FK column
        public Homeroom Homeroom { get; set; }       // navigation: EF links via HomeroomID


        // A student can have multiple caregivers; a caregiver can belong to multiple students.
        public ICollection<Caregiver> Caregivers { get; set; } = new List<Caregiver>();

        // If students can log in, this connects the Student row to the Identity user row.
        public string? IdentityUserId { get; set; }
        public AvondaleCollegeClinicUser? AvondaleCollegeClinicUserAccount { get; set; }

        public ICollection<Appointment> Appointments { get; set; }   // all appointments for this student
        public ICollection<MedicalRecord> MedicalRecords { get; set; } // all medical records for this student
    }
}
