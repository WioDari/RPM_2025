using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("PlaylistTracks")]
    public class PlaylistTrack
    {
        [Key]
        [Column(Order = 0)]
        public int PlaylistID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int TrackID { get; set; }

        [Column("DateAdded")]
        public DateTime? DateAdded { get; set; }

        [ForeignKey("PlaylistID")]
        public virtual Playlist Playlist { get; set; } = null!;

        [ForeignKey("TrackID")]
        public virtual Track Track { get; set; } = null!;
    }
}
