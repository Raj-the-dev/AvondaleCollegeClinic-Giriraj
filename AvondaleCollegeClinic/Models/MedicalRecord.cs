using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class MedicalRecord
    {
        [Key]
        public int MedicalRecordID { get; set; } // Unique ID 

        [Required]
        [Display(Name = "Student")]
        public string StudentID { get; set; } // FK to Student

        [Required]
        [Display(Name = "Doctor")]
        public string DoctorID { get; set; } // FK to Doctor

        [Required]
        [StringLength(500)]
        [Display(Name = "Doctor's Notes")]
        public string Notes { get; set; } // Summary of the visit or notes

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Record Date")]
        public DateTime Date { get; set; } // Date of entry

        public Student Student { get; set; }
        public Doctor Doctor { get; set; }
        public ICollection<Labtest> Labtests { get; set; } // Linked lab test reports
    }
}
