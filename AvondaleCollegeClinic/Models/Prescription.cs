using AvondaleCollegeClinic.Validation;
using System.ComponentModel.DataAnnotations;
using AvondaleCollegeClinic.Validation;

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
        [NotFuture(ErrorMessage = "Start date cannot be in the future.")]
        [NotBefore(nameof(StartDate))] // no-op here but keeps symmetry with EndDate rule below
        public DateTime StartDate { get; set; } // Medication start

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [NotBefore(nameof(StartDate), ErrorMessage = "End date cannot be earlier than start date.")]
        public DateTime EndDate { get; set; } // Medication end

        public Diagnosis Diagnosis { get; set; } // Linked diagnosis
    }
}
