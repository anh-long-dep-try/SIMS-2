using System.ComponentModel.DataAnnotations;

namespace SIMS_2_.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(100)] // Match the database schema (nvarchar(100))
        public string RoleName { get; set; }
    }
}