using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class Teacher
    {
        [Key]
        public string TeacherID { get; set; } // Unique ID

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } 

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Teacher Code")]
        public string TeacherCode { get; set; } // User-entered code

        public ICollection<Homeroom> Homerooms { get; set; } // link to teacher homeroom
    }
}
