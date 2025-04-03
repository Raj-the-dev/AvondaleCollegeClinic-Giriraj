namespace AvondaleCollegeClinic.Models
{
    public class Labtest
    {
        public int LabtestID { get; set; }
        public int RecordID { get; set; }
        public string TestType { get; set; }
        public string File { get; set; }
        public string ProtectedPDF { get; set; } //PDF File
        public DateTime ResultDate { get; set; }

        public MedicalRecord MedicalRecord { get; set; }
    }
}
