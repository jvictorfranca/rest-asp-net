using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestASPNet.Model
{
    [Table("books")]
    public class Book : Base.BaseEntity
    {
        [Required]
        [Column("title", TypeName ="varchar(80)")]
        [MaxLength(80)]
        public string Title { get; set; }

        [Required]
        [Column("author", TypeName = "varchar(80)")]
        [MaxLength(80)]
        public string Author { get; set; }

        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column("launch_date", TypeName = "datetime(6)")]
        public DateTime LaunchDate { get; set; }
    }
}
