using SportHub.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SportHub.Domain.ViewModel
{
    public class NavigationItemForSave
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Type { get; set; }
        public int? ParentsItemId { get; set; }
        public List<NavigationItemForSave>? Children { get; set; }
    }
}
