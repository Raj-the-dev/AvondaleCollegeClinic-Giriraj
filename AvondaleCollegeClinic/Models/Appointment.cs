using AvondaleCollegeClinic.Validation;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using AvondaleCollegeClinic.Validation;

namespace AvondaleCollegeClinic.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; } // Unique ID

        [Required]
        [Display(Name = "Student")]
        public string StudentID { get; set; } // FK to Student

        [Required]
        [Display(Name = "Doctor")]
        public string DoctorID { get; set; } // FK to Doctor

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date & Time")]
        [NotPast(ErrorMessage = "Appointments cannot be in the past.")]
        [WithinNextDays(30, ErrorMessage = "Appointments can only be up to 30 days ahead.")]
        [NotWeekend(ErrorMessage = "No weekend appointments.")]
        public DateTime AppointmentDateTime { get; set; }

        [Required]
        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; } // Appointment status (enum)

        [Required]
        [StringLength(200)]
        [Display(Name = "Reason for Visit")]
        public string Reason { get; set; } // Reason for scheduling appointment

        public Student Student { get; set; } // Linked student model
        public Doctor Doctor { get; set; } // Linked doctor model
        public Diagnosis Diagnosis { get; set; }  //link to diagnosis
    }
}
