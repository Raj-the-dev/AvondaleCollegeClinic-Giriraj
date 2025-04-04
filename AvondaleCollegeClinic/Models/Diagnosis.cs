using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class Diagnosis
    {
        [Key]
        public int DiagnosisID { get; set; } // Unique ID

        [Required]
        [Display(Name = "Appointment")]
        public int AppointmentID { get; set; } // FK to Appointment

        [Required]
        [StringLength(300)]
        [Display(Name = "Diagnosis Description")]
        public string Description { get; set; } // Description of diagnosis

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Diagnosis Date")]
        public DateTime DateDiagnosed { get; set; } // When the diagnosis was made

        public Appointment Appointment { get; set; } // Navigation to appointment
        public ICollection<Prescription> Prescriptions { get; set; } // link to prescriptions
    }
}
