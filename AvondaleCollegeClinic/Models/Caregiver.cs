using AvondaleCollegeClinic.Areas.Identity.Data; // brings in your Identity user class (AvondaleCollegeClinicUser)
using AvondaleCollegeClinic.Validation;          // custom validators (NotFuture, AgeRange)
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleCollegeClinic.Models
{
    // Same idea as the previous enum: a fixed list of allowed relationships.
    public enum RelationshipType
    {
        Parent,
        Guardian,
        Sibling,
        Other
    }

    public class Caregiver
    {
        [Key]                 // primary key (EF uses this as the unique ID)
        [StringLength(9)]     // NEW: keep IDs exactly/at most 9 characters (e.g., school-issued IDs)
        public string CaregiverID { get; set; } // Unique ID

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        // ^ and $ = start/end of the string, so the whole thing must match.
        // [A-Za-z\s]+ = one or more letters or spaces.
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        // Same validation as FirstName for clean, human-readable names.
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string LastName { get; set; }


        // ImageFile is the *uploaded file* from the form. It should NOT be stored in the database.
        // That's why we mark it [NotMapped] so EF Core ignores it.
        [Display(Name = "Upload Image")]
        [NotMapped] // NEW: tells EF not to create a column for this property
        public IFormFile? ImageFile { get; set; }

        // After saving the file somewhere (e.g., /wwwroot/uploads/...), we store the *path* in the DB.
        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        [Required]
        [DataType(DataType.Date)] // UI hint: date-only picker (no time)
        [Display(Name = "Date of Birth")]
        // NEW: Custom validators about time and age.
        // NotFuture -> DOB can't be after today.
        [NotFuture(ErrorMessage = "Date of birth cannot be in the future.")]
        // AgeRange(18) -> must be at least 18 years old (you wrote the logic in your Validation folder).
        [AgeRange(18, ErrorMessage = "Caregivers must be at least 18 years old.")]
        public DateTime DOB { get; set; }

        [Required]
        [EmailAddress] // built-in email format check (basic but helpful)
        public string Email { get; set; }

        [Required]
        [Phone] // basic phone pattern (loose)
        [RegularExpression(@"^02\d{1}[- ]?\d{3}[- ]?\d{4}$",
            ErrorMessage = "Please enter a valid NZ mobile number (e.g., 021-123-4567).")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Relationship to Student")]
        public RelationshipType Relationship { get; set; } // enum like before

        // This is the foreign key string that points to the Identity user table (AspNetUsers).
        // If the caregiver has a login, we connect them here.
        public string? IdentityUserId { get; set; }

        // [ForeignKey] explicitly tells EF that IdentityUserId is the FK for this navigation property.
        // This sets up a relationship to your custom Identity user class.
        [ForeignKey("IdentityUserId")]
        public AvondaleCollegeClinicUser? AvondaleCollegeClinicUserAccount { get; set; }

        public ICollection<Student> Students { get; set; } // Linked students
    }
}
