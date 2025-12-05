namespace DemoExam.Models
{
    public class TagPlaylist
    {
        public int TagId { get; set; }
        public int PlaylistId { get; set; }

        public Tag Tag { get; set; }
        public Playlist Playlist { get; set; }
    }
}