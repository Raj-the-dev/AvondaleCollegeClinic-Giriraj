namespace AvondaleCollegeClinic.Models
{
    public class Caregiver
    {
        public int CaregiverID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Relationship { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
