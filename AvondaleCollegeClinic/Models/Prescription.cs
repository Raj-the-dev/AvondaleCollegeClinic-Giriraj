using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionID { get; set; } // Unique ID

        [Required]
        [Display(Name = "Diagnosis")]
        public int DiagnosisID { get; set; } // FK to Diagnosis

        [Required]
        [StringLength(100)]
        public string Medication { get; set; } // Name of medication

        [Required]
        [StringLength(100)]
        public string Dosage { get; set; } // Dosage details (e.g., "2x daily")

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } // Medication start

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } // Medication end

        public Diagnosis Diagnosis { get; set; } // Linked diagnosis
    }
}
