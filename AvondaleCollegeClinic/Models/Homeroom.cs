using NuGet.DependencyResolver;

namespace AvondaleCollegeClinic.Models
{
    public class Homeroom
    {
        public string HomeroomID { get; set; }
        public int YearLevel { get; set; }
        public string TeacherID { get; set; }
        public string Class { get; set; }

        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
