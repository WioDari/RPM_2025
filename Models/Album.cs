namespace DemoExam.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string AlbumTitle { get; set; }
        public int ArtistId { get; set; }
        public int ReleaseYear { get; set; }
        public string CoverPath { get; set; }
        public byte[] CoverBinary { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>(); // ← Many-to-Many
    }
}