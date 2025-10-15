// These "using" lines tell the file which namespaces (libraries) we want to use.
using AvondaleCollegeClinic.Validation;   // <-- custom validation attributes live here (NotPast, WithinNextDays, NotWeekend)
using System.ComponentModel.DataAnnotations; // <-- built-in Data Annotations (Required, Display, StringLength, etc.)

namespace AvondaleCollegeClinic.Models
{
    // An enum is a list of named options. Under the hood, this is an int (0..n).
    // This makes our "Status" field strongly typed and easy to read.
    public enum AppointmentStatus
    {
        Pending,    // 0 - appointment created, waiting for confirmation
        Confirmed,  // 1 - approved/locked in
        Cancelled,  // 2 - cancelled by student or clinic
        Completed   // 3 - the appointment already happened
    }

    // This class represents a row in the "Appointments" table (if using EF Core).
    public class Appointment
    {
        [Key] // Marks the primary key (unique ID). EF Core uses this as the identity for the row.
        public int AppointmentID { get; set; } // Unique ID

        [Required]                    // Field must be provided (no null/empty).
        [Display(Name = "Student")]   // Friendly label for forms/views.
        public string StudentID { get; set; } // FK to Student (we store the student's ID here as text)

        [Required]
        [Display(Name = "Doctor")]
        public string DoctorID { get; set; } // FK to Doctor (doctor's ID as text)

        // The actual date and time for the appointment.
        // Below are a mix of built-in and custom validation rules:
        [Required]                           // must be set
        [DataType(DataType.DateTime)]        // hints UI scaffolding to use a date-time control
        [Display(Name = "Appointment Date & Time")] // friendly label for UI
        [NotPast(ErrorMessage = "Appointments cannot be in the past.")] // CUSTOM RULE: date must be >= now
        [WithinNextDays(30, ErrorMessage = "Appointments can only be up to 30 days ahead.")] // CUSTOM RULE: limit to next 30 days
        [NotWeekend(ErrorMessage = "No weekend appointments.")] // CUSTOM RULE: Saturday/Sunday not allowed
        public DateTime AppointmentDateTime { get; set; }

        // The current state of the appointment, using our enum above.
        [Required]
        [Display(Name = "Status")]
        public AppointmentStatus Status { get; set; } // Appointment status (enum)

        // Short description of why the student booked the appointment.
        [Required]                 // must be filled in
        [StringLength(200)]        // keep the text short (max 200 chars)
        [Display(Name = "Reason for Visit")]
        public string Reason { get; set; } // Reason for scheduling appointment

        // These are object references that let us jump from Appointment to related records.
        // EF Core uses naming conventions (StudentID -> Student) to understand relationships.
        // They are NOT stored as separate columns; they help with loading related data.

        public Student Student { get; set; } // Linked student model relationship
        public Doctor Doctor { get; set; }   // Linked doctor model relationship
        public Diagnosis Diagnosis { get; set; }  // Link to related diagnosis relationship
    }
}
