using SIMS_2_.Models;
using System.Collections.Generic;

namespace SIMS_2_.Models
{
    public class StudentPageViewModel
    {
        public Student Student { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Course> AvailableCourses { get; set; }//the course that the student is not enrolled in (dropdown)
        public List<Enrollment> Schedule { get; set; } //display the course schedule (fetch from enrollment) 
    }
}