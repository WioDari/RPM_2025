using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("PlaylistTags")]
    public class PlaylistTag
    {
        [Key]
        [Column(Order = 0)]
        public int PlaylistID { get; set; }

        [Key]
        [Column(Order = 1)]
        [MaxLength(100)]
        public string TagName { get; set; } = string.Empty;

        [ForeignKey("PlaylistID")]
        public virtual Playlist Playlist { get; set; } = null!;
    }
}
