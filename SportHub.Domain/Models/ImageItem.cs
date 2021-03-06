using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SportHub.Domain.Models
{
    public class ImageItem
    {
        [Key]
        public int Id { get; set; }
        public string ImageLink { get; set; }
        [Required]
        [MaxLength(70)]
        [Column(TypeName = "varchar(70)")]
        public string Alt { get; set; }
        [Required]
        [MaxLength(60)]
        [Column(TypeName = "varchar(60)")]
        public string PhotoTitle { get; set; }
        [Required]
        [MaxLength(150)]
        [Column(TypeName = "varchar(150)")]
        public string ShortDescription { get; set; }
        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Author { get; set; }

    }
}
