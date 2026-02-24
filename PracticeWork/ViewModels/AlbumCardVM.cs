using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using PracticeWork.Context;

namespace PracticeWork.ViewModels
{
    class AlbumCardVM : BaseVM
    {
        public class AlbumItem
        {
            public int? ID { get; set; }
            public string? Title { get; set; }
            public string? Autor { get; set; }
            public string? totalTracks { get; set; }
            public BitmapImage? albumImage { get; set; }
        }

        private ObservableCollection<AlbumItem> _data;
        public ObservableCollection<AlbumItem> data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        public AlbumCardVM()
        {
            LoadCards();
        }

        public void LoadCards()
        {
            var context = new PostgresContext();
            data = new ObservableCollection<AlbumItem>(context.Albums
                .Include(a => a.ArtistNavigation).Select(a => new AlbumItem
                {
                    ID = a.AlbumId,
                    Title = a.AlbumName,
                    Autor = a.ArtistNavigation.ArtistName,
                    totalTracks = context.AlbumTracks.Where(at => at.Talbum == a.AlbumId).Count().ToString(),
                    albumImage = new BitmapImage(new Uri(a.CoverPath))
                }).OrderBy(a => a.ID).ToList());
        }
    }
}
