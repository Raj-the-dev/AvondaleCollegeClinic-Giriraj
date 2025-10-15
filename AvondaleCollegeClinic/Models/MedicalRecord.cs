using AvondaleCollegeClinic.Validation;   // custom validators 
// using AvondaleCollegeClinic.Validation; // duplicate; safe to remove
using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class MedicalRecord
    {
        [Key]
        public int MedicalRecordID { get; set; } // primary key

        [Required]
        [Display(Name = "Student")]
        public string StudentID { get; set; } // FK to Student 

        [Required]
        [Display(Name = "Doctor")]
        public string DoctorID { get; set; } // FK to Doctor 

        // Brief summary of the visit; capped to 500 chars to keep it readable.
        [Required]
        [StringLength(500)]
        [Display(Name = "Doctor's Notes")]
        public string Notes { get; set; }

        // Calendar date the record was created; cannot be in the future.
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Record Date")]
        [NotFuture(ErrorMessage = "Record date cannot be in the future.")]
        public DateTime Date { get; set; }

        // ---- Navigation properties ----
        public Student Student { get; set; }         // StudentID
        public Doctor Doctor { get; set; }           //  DoctorID
        public ICollection<Labtest> Labtests { get; set; } // zero or more lab test PDFs tied to this visit
    }
}
