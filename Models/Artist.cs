namespace DemoExam.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string YearsActive { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Album> Albums { get; set; } = new List<Album>();
        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
        public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
    }
}