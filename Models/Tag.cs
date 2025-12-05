namespace DemoExam.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}