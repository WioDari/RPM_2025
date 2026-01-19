using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("Albums")]
    public class Album
    {
        [Key]
        [Column("AlbumID")]
        public int AlbumID { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("AlbumTitle")]
        public string AlbumTitle { get; set; } = string.Empty;

        [Column("ArtistID")]
        public int? ArtistID { get; set; }

        [Column("ReleaseYear")]
        public int? ReleaseYear { get; set; }

        [MaxLength(500)]
        [Column("CoverPath")]
        public string? CoverPath { get; set; }

        [Column("CoverBinary", TypeName = "LONGBLOB")]
        public byte[]? CoverBinary { get; set; }

        [Column("TotalDuration")]
        public int TotalDuration { get; set; } = 0;

        [ForeignKey("ArtistID")]
        public virtual Artist? Artist { get; set; }

        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
        public virtual ICollection<AlbumGenre> AlbumGenres { get; set; } = new List<AlbumGenre>();
    }
}
