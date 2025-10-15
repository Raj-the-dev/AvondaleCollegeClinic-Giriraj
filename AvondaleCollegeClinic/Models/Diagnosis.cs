using AvondaleCollegeClinic.Validation;   // custom validators 
using System.ComponentModel.DataAnnotations;
// using AvondaleCollegeClinic.Validation; // duplicate; safe to remove

namespace AvondaleCollegeClinic.Models
{
    public class Diagnosis
    {
        [Key] // primary key for this table
        public int DiagnosisID { get; set; } // Unique ID

        // Foreign key that points to the related Appointment row.
        // Using an int here means it matches Appointment.AppointmentID.
        [Required]
        [Display(Name = "Appointment")]
        public int AppointmentID { get; set; } // FK to Appointment

        // What the doctor wrote as the diagnosis (kept short-ish).
        [Required]
        [StringLength(300)]
        [Display(Name = "Diagnosis Description")]
        public string Description { get; set; } // Description of diagnosis

        // The date the diagnosis was recorded.
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Diagnosis Date")]
        [NotFuture(ErrorMessage = "Diagnosis date cannot be in the future.")]
        public DateTime DateDiagnosed { get; set; } // When the diagnosis was made

        // ---- Navigation properties (EF Core) ----

        // Single related Appointment object. In many clinic systems,
        // a diagnosis belongs to exactly one appointment (often 1:1).
        // EF will pair this with AppointmentID by convention.
        public Appointment Appointment { get; set; }

        // A diagnosis can have multiple prescriptions (typical 1:many).
        // EF treats this as a collection navigation.
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
