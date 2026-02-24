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
    class ArtistVM : BaseVM
    {
        public class ArtistItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public string Activeyears { get; set; }
            public string Description { get; set; }
            public BitmapImage ArtistPhoto { get; set; }
        }

        private ObservableCollection<ArtistItem> _data;

        public ObservableCollection<ArtistItem> data
        {
            get => _data;
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        public ArtistVM()
        {
            LoadArtists();
        }

        private void LoadArtists()
        {
            var context = new PostgresContext();
            data = new ObservableCollection<ArtistItem>(context.Artists.Include(a => a.ArtistCountryNavigation)
                .Select(ar => new ArtistItem
                {
                    ID = ar.ArtistId,
                    Name = ar.ArtistName,
                    Country = (context.Countries.FirstOrDefault(z => z.CountryId == ar.ArtistCountry)).CountryName,
                    Activeyears = ar.ActiveYears,
                    Description = ar.ArtistDescription,
                    ArtistPhoto = new BitmapImage(new Uri(ar.PhotoPath))
                }).OrderBy(v => v.ID).ToList());
        }
    }
}
