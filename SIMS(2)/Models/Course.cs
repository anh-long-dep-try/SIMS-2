using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIMS_2_.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; }

        public string Description { get; set; } // Optional field.

        [Required]
        public int Credits { get; set; }

        // Foreign Key to Faculty.
        [ForeignKey("Faculty")]
        public int FacultyId { get; set; }

        // Navigation property.
        Faculty Faculty { get; }
    }
}
