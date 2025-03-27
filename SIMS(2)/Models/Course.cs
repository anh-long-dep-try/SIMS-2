using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS_2_.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; }

        [Required]
        [StringLength(10)]
        public string CourseCode { get; set; }

        [Required]
        public int Credits { get; set; }

        [Required]
        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }
        public Faculty? Faculty { get; set; }

        [Required]
        [ForeignKey("AcademicCalendar")]
        public int CalendarId { get; set; }
        public AcademicCalendar? AcademicCalendar { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}