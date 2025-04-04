using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class Labtest
    {
        [Key]
        public int LabtestID { get; set; } // report ID

        [Required]
        [Display(Name = "Medical Record")]
        public int RecordID { get; set; } // FK to MedicalRecord

        [Required]
        [StringLength(100)]
        [Display(Name = "Test Type")]
        public string TestType { get; set; } // E.g., Blood Test, X-Ray

        [Required]
        [Display(Name = "File Name")]
        public string File { get; set; } // File of report

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Result Date")]
        public DateOnly ResultDate { get; set; } // When test results were received

        public MedicalRecord MedicalRecord { get; set; }
    }
}
