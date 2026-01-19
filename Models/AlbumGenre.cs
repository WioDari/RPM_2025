using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("AlbumGenres")]
    public class AlbumGenre
    {
        [Key]
        [Column(Order = 0)]
        public int AlbumID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int GenreID { get; set; }

        [ForeignKey("AlbumID")]
        public virtual Album Album { get; set; } = null!;

        [ForeignKey("GenreID")]
        public virtual Genre Genre { get; set; } = null!;
    }
}
