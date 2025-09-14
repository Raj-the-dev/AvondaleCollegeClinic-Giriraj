using AvondaleCollegeClinic.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleCollegeClinic.Models
{
    public enum RelationshipType
    {
        Parent,
        Guardian,
        Sibling,
        Other
    }

    public class Caregiver
    {
        [Key]
        [StringLength(9)]
        public string CaregiverID { get; set; } // Unique ID

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string FirstName { get; set; } // First name

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string LastName { get; set; } // Last name

        [Display(Name = "Upload Image")]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; } // Date of birth

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Email address

        [Required]
        [Phone]
        [RegularExpression(@"^02\d{1}[- ]?\d{3}[- ]?\d{4}$",
    ErrorMessage = "Please enter a valid NZ mobile number (e.g., 021-123-4567).")]
        public string Phone { get; set; } // Phone number

        [Required]
        [Display(Name = "Relationship to Student")]
        public RelationshipType Relationship { get; set; } // Enum for relationship
        public string? IdentityUserId { get; set; }

        [ForeignKey("IdentityUserId")]
        public AvondaleCollegeClinicUser? AvondaleCollegeClinicUserAccount { get; set; }

        public ICollection<Student> Students { get; set; } // Linked students
    }
}
