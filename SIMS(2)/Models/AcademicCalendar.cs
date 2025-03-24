using System.ComponentModel.DataAnnotations;

namespace SIMS_2_.Models
{
    public class AcademicCalendar
    {
        [Key]
        public int CalendarId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Semester { get; set; }
    }
}