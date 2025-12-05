namespace DemoExam.Models
{
    public class Track
    {
        public int Id { get; set; }
        public string TrackName { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Bitrate { get; set; }
        public string FilePath { get; set; }
        public string AlbumCoverPath { get; set; }
        public int Rating { get; set; }
        public string Playcost { get; set; }
        public int? AlbumId { get; set; }
        public virtual Album? Album { get; set; }
        public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}