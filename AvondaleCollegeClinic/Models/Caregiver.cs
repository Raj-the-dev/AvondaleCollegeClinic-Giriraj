using System.ComponentModel.DataAnnotations;

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
        public int CaregiverID { get; set; } // Unique ID

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } // First name

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } // Last name

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; } // Date of birth

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Email address

        [Required]
        [Phone]
        public string Phone { get; set; } // Phone number

        [Required]
        [Display(Name = "Relationship to Student")]
        public RelationshipType Relationship { get; set; } // Enum for relationship

        public ICollection<Student> Students { get; set; } // Linked students
    }
}
