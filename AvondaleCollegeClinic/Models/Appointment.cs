using Microsoft.CodeAnalysis;

namespace AvondaleCollegeClinic.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public string StudentID { get; set; }
        public int DoctorID { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }

        public Student Student { get; set; }
        public Doctor Doctor { get; set; }
        public Diagnosis Diagnosis { get; set; }
    }
}
