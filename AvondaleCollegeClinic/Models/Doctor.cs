namespace AvondaleCollegeClinic.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<DoctorAvailability> Availabilities { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
