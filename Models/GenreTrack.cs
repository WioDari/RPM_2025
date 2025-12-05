namespace DemoExam.Models
{
    public class GenreTrack
    {
        public int GenreId { get; set; }
        public int TrackId { get; set; }

        public Genre Genre { get; set; }
        public Track Track { get; set; }
    }
}