using System.ComponentModel.DataAnnotations;

namespace SIMS_2_.Models
{
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        [Required]
        [StringLength(100)]
        public string Department { get; set; }
    }
}
