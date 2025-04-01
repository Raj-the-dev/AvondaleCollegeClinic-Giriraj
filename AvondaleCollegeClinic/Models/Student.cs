namespace AvondaleCollegeClinic.Models
{
    public class Student
    {
        public string StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string HomeroomID { get; set; }
        public int CaregiverID { get; set; }

        public Homeroom Homeroom { get; set; }
        public Caregiver Caregiver { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
