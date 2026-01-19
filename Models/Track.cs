using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("Tracks")]
    public class Track
    {
        [Key]
        [Column("TrackID")]
        public int TrackID { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("TrackName")]
        public string TrackName { get; set; } = string.Empty;

        [Column("Duration")]
        public int Duration { get; set; }

        [Column("ReleaseDate")]
        public DateTime? ReleaseDate { get; set; }

        [Column("Bitrate")]
        public int? Bitrate { get; set; }

        [MaxLength(500)]
        [Column("FilePath")]
        public string? FilePath { get; set; }

        [MaxLength(500)]
        [Column("AlbumCoverPath")]
        public string? AlbumCoverPath { get; set; }

        [Column("AlbumCoverBinary", TypeName = "LONGBLOB")]
        public byte[]? AlbumCoverBinary { get; set; }

        [Column("Rating", TypeName = "DECIMAL(2,1)")]
        public decimal? Rating { get; set; }

        [Column("PlayCount")]
        public int PlayCount { get; set; } = 0;

        [Column("AlbumID")]
        public int? AlbumID { get; set; }

        [ForeignKey("AlbumID")]
        public virtual Album? Album { get; set; }

        public virtual ICollection<TrackArtist> TrackArtists { get; set; } = new List<TrackArtist>();
        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
    }
}
