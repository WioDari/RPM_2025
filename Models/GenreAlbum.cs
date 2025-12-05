using Microsoft.EntityFrameworkCore;
namespace DemoExam.Models
{
    public class GenreAlbum
    {
        public int GenreId { get; set; }
        public int AlbumId { get; set; }
        public Genre Genre { get; set; }
        public Album Album { get; set; }
    }
}