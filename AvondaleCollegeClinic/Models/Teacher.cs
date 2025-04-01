namespace AvondaleCollegeClinic.Models
{
    public class Teacher
    {
        public string TeacherID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<Homeroom> Homerooms { get; set; }
    }
}
