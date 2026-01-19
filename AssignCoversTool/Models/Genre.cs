using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("Genres")]
    public class Genre
    {
        [Key]
        [Column("GenreID")]
        public int GenreID { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("GenreName")]
        public string GenreName { get; set; } = string.Empty;

        public virtual ICollection<AlbumGenre> AlbumGenres { get; set; } = new List<AlbumGenre>();
    }
}
