namespace AvondaleCollegeClinic.Models
{
    public class Diagnosis
    {
        public int DiagnosisID { get; set; }
        public int AppointmentID { get; set; }
        public string Description { get; set; }
        public DateTime DateDiagnosed { get; set; }

        public Appointment Appointment { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
