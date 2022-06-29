﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportHub.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(64)]
        [Column(TypeName = "char(64)")]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(320)]
        [Column(TypeName = "varchar(320)")]
        [EmailAddress]
        public string Email { get; set; }
        public virtual ICollection<UserRole> Roles { get; set; }
    }
}
