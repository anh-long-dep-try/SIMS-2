using System.ComponentModel.DataAnnotations;

namespace SIMS_2_.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string PersonalInfo { get; set; } // Optional field.

        [Required]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
