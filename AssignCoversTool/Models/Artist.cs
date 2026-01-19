using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("Artists")]
    public class Artist
    {
        [Key]
        [Column("ArtistID")]
        public int ArtistID { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("ArtistName")]
        public string ArtistName { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("Country")]
        public string? Country { get; set; }

        [MaxLength(50)]
        [Column("YearsActive")]
        public string? YearsActive { get; set; }

        [Column("Description", TypeName = "TEXT")]
        public string? Description { get; set; }

        [MaxLength(500)]
        [Column("PhotoPath")]
        public string? PhotoPath { get; set; }

        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
        public virtual ICollection<TrackArtist> TrackArtists { get; set; } = new List<TrackArtist>();
    }
}
