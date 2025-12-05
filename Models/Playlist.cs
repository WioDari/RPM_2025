namespace DemoExam.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreate { get; set; }
        public string Likes { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public virtual ICollection<Track> Tracks { get; set; } = new List<Track>(); 
    }
}