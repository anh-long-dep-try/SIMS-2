using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIMS_2_.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string PersonalInfo { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}