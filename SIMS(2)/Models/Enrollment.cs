using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS_2_.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        public string Grade { get; set; }
    }
}