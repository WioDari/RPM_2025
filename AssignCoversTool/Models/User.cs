using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPlus.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserID")]
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("UserLogin")]
        public string UserLogin { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("UserPassword")]
        public string UserPassword { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("FullName")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("Role")]
        public string Role { get; set; } = "User";

        [MaxLength(50)]
        [Column("Subscription")]
        public string? Subscription { get; set; }

        [Column("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [Column("LastLogin")]
        public DateTime? LastLogin { get; set; }

        [Column("IsBlocked")]
        public bool IsBlocked { get; set; } = false;

        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public virtual ICollection<PlaylistSubscription> PlaylistSubscriptions { get; set; } = new List<PlaylistSubscription>();
    }
}
