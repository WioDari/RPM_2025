using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("TrackArtists")]
    public class TrackArtist
    {
        [Key]
        [Column(Order = 0)]
        public int TrackID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ArtistID { get; set; }

        [ForeignKey("TrackID")]
        public virtual Track Track { get; set; } = null!;

        [ForeignKey("ArtistID")]
        public virtual Artist Artist { get; set; } = null!;
    }
}
