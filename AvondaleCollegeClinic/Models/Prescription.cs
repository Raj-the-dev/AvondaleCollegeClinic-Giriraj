namespace AvondaleCollegeClinic.Models
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int DiagnosisID { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Diagnosis Diagnosis { get; set; }
    }
}
