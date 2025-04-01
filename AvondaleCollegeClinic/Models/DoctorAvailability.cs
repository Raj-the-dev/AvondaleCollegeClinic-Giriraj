namespace AvondaleCollegeClinic.Models
{
    public class DoctorAvailability
    {
        public int DoctorAvailabilityID { get; set; }
        public int DoctorID { get; set; }
        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Doctor Doctor { get; set; }
    }
}
