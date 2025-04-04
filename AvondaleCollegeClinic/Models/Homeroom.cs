using NuGet.DependencyResolver;
using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public enum HomeroomClass
    {
        A,
        B,
        C,
        D,
        E
    }
    public class Homeroom
    {
        [Key]
        public string HomeroomID { get; set; } // Unique ID

        [Required]
        [Range(1, 13, ErrorMessage = "Year level must be between 1 and 13")]
        [Display(Name = "Year Level")]
        public int YearLevel { get; set; } // Grade or year level

        [Required]
        [Display(Name = "Teacher")]
        public string TeacherID { get; set; } // FK to teacher

        [Required]
        [Display(Name = "Class")]
        public HomeroomClass Class { get; set; } // Enum for class section

        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
