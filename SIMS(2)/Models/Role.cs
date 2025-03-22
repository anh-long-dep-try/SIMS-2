﻿using System.ComponentModel.DataAnnotations;

namespace SIMS_2_.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
    }
}
