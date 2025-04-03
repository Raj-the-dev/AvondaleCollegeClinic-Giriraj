namespace AvondaleCollegeClinic.Models
{
    public class MedicalRecord
    {
        public int MedicalRecordID { get; set; }
        public string StudentID { get; set; }
        public int DoctorID { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }

        public Student Student { get; set; }
        public Doctor Doctor { get; set; }
        public ICollection<Labtest> LabTests { get; set; }
    }
}
