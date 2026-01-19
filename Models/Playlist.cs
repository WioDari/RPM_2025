using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MusicPlus.Models
{
    [Table("Playlists")]
    public class Playlist
    {
        [Key]
        [Column("PlaylistID")]
        public int PlaylistID { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("PlaylistName")]
        public string PlaylistName { get; set; } = string.Empty;

        [Column("UserID")]
        public int? UserID { get; set; }

        [Column("DateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column("Likes")]
        public int Likes { get; set; } = 0;

        [Column("Description", TypeName = "TEXT")]
        public string? Description { get; set; }

        [NotMapped]
        public int TotalDuration => PlaylistTracks?.Sum(pt => pt.Track?.Duration ?? 0) ?? 0;

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }

        public virtual ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
        public virtual ICollection<PlaylistTag> PlaylistTags { get; set; } = new List<PlaylistTag>();
        public virtual ICollection<PlaylistSubscription> PlaylistSubscriptions { get; set; } = new List<PlaylistSubscription>();
    }
}
