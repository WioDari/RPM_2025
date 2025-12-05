namespace DemoExam.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
        public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
    }
}