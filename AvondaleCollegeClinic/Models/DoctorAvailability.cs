using System.ComponentModel.DataAnnotations;

namespace AvondaleCollegeClinic.Models
{
    public class DoctorAvailability
    {
        [Key]
        public int DoctorAvailabilityID { get; set; } // Unique ID

        [Required]
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; } // Foreign Key to Doctor

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Available Date")]
        public DateTime AvailableDate { get; set; } // Date the doctor is available

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; } // Time availability starts

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; } // Time availability ends

        public Doctor Doctor { get; set; } // Link to Doctor model
    }
}
