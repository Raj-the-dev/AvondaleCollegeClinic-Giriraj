using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    // Year levels stored as enum. Names start with "_" because C# identifiers can't start with a number.
    public enum YearLevel
    {
        _9 = 9,
        _10 = 10,
        _11 = 11,
        _12 = 12,
        _13 = 13
    }

    // School blocks A–F (fixed set avoids typos like "block a").
    public enum Block
    {
        A, B, C, D, E, F
    }

    // Class numbers 1–20 as an enum (same underscore trick as YearLevel).
    public enum ClassNumber
    {
        _1 = 1, _2, _3, _4, _5, _6, _7, _8, _9, _10,
        _11, _12, _13, _14, _15, _16, _17, _18, _19, _20
    }

    public class Homeroom
    {
        [Key]
        public string HomeroomID { get; set; } // e.g., a code like "12A-03" (primary key)

        [Required]
        [Display(Name = "Year Level")]
        public YearLevel YearLevel { get; set; } // 9–13 (enum keeps inputs valid)

        [Required]
        [Display(Name = "Teacher")]
        public string TeacherID { get; set; } // FK to Teacher (stores the teacher's ID)

        [Required]
        public Block Block { get; set; } // A–F block

        [Required]
        [Display(Name = "Class")]
        public ClassNumber ClassNumber { get; set; } // 1–20 (enum)

        // Navigation properties (EF Core will use TeacherID to link Teacher)
        public Teacher Teacher { get; set; }               // the assigned teacher
        public ICollection<Student> Students { get; set; } // students in this homeroom
    }
}
