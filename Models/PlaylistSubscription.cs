using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("PlaylistSubscriptions")]
    public class PlaylistSubscription
    {
        [Key]
        [Column(Order = 0)]
        public int UserID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int PlaylistID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("PlaylistID")]
        public virtual Playlist Playlist { get; set; } = null!;
    }
}
