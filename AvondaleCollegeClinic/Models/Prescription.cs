using AvondaleCollegeClinic.Validation;   // custom validators (NotFuture, NotBefore)
// using AvondaleCollegeClinic.Validation; // duplicate; safe to remove
using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionID { get; set; } // primary key

        [Required]
        [Display(Name = "Diagnosis")]
        public int DiagnosisID { get; set; } // FK to Diagnosis (pairs with navigation below)

        [Required]
        [StringLength(100)]
        public string Medication { get; set; } // e.g., "Amoxicillin"

        [Required]
        [StringLength(100)]
        public string Dosage { get; set; } // e.g., "500mg, 2x daily"

        // Start of the medication course; must not be in the future.
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [NotFuture(ErrorMessage = "Start date cannot be in the future.")]
        [NotBefore(nameof(StartDate))] // note: effectively a no-op; kept for symmetry with EndDate
        public DateTime StartDate { get; set; }

        // End of the course; must be on/after StartDate.
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [NotBefore(nameof(StartDate), ErrorMessage = "End date cannot be earlier than start date.")]
        public DateTime EndDate { get; set; }

        // Navigation property to the owning diagnosis (1 diagnosis : many prescriptions).
        public Diagnosis Diagnosis { get; set; }
    }
}
