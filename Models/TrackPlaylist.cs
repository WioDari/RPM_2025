namespace DemoExam.Models
{
    public class TrackPlaylist
    {
        public int TrackId { get; set; }
        public int PlaylistId { get; set; }

        public Track Track { get; set; }
        public Playlist Playlist { get; set; }
    }
}