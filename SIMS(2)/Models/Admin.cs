using System.ComponentModel.DataAnnotations;

namespace SIMS_2_.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

    
    }
}
