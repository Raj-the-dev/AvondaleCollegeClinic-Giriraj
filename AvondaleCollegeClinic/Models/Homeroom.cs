using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public enum YearLevel
    {
        _9 = 9,
        _10 = 10,
        _11 = 11,
        _12 = 12,
        _13 = 13
    }

    public enum Block
    {
        A,
        B,
        C,
        D,
        E,
        F
    }

    public enum ClassNumber
    {
        _1 = 1, _2, _3, _4, _5, _6, _7, _8, _9, _10,
        _11, _12, _13, _14, _15, _16, _17, _18, _19, _20
    }

    public class Homeroom
    {
        [Key]
        public string HomeroomID { get; set; } // Unique ID

        [Required]
        [Display(Name = "Year Level")]
        public YearLevel YearLevel { get; set; } // Enum for year level

        [Required]
        [Display(Name = "Teacher")]
        public string TeacherID { get; set; } // FK to teacher

        [Required]
        public Block Block { get; set; } // Enum for block A–F

        [Required]
        [Display(Name = "Class")]
        public ClassNumber ClassNumber { get; set; } // Enum for class 1–20

        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
