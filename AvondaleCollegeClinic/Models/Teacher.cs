using AvondaleCollegeClinic.Areas.Identity.Data; // custom Identity user type
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvondaleCollegeClinic.Models
{
    public class Teacher
    {
        [Key]
        public string TeacherID { get; set; } // primary key (unique teacher ID)

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        // Allow letters and spaces only (keeps names clean)
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string LastName { get; set; }

        // Image upload pattern: the actual uploaded file (NOT stored in DB)
        [Display(Name = "Upload Image")]
        [NotMapped] // EF Core won't create a column for this
        public IFormFile? ImageFile { get; set; }

        // We store only the path/URL to the saved image in the database
        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // A short code the user types in (e.g., used for joining a homeroom)
        [Required]
        [StringLength(3, MinimumLength = 3)]
        // Exactly 3 letters (no numbers/spaces)
        [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Code must be exactly 3 letters.")]
        public string TeacherCode { get; set; } // user-entered code

        // Optional link to the ASP.NET Identity account for this teacher
        public string? IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public AvondaleCollegeClinicUser? AvondaleCollegeClinicUserAccount { get; set; }

        // One teacher can have zero or one homeroom
        public Homeroom? Homeroom { get; set; }
    }
}
