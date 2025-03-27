using SIMS_2_.Models;
using System.Collections.Generic;

namespace SIMS_2_.Models
{
    public class StudentPageViewModel
    {
        public Student Student { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Course> AvailableCourses { get; set; }
        public List<Enrollment> Schedule { get; set; } // Using Enrollment for now to derive schedule
    }
}