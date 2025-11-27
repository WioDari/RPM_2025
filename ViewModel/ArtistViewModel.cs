using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicWpf.ViewModel
{
    public class ArtistViewModel : BaseViewModel
    {
        private ObservableCollection<ArtistVM> _artist;
        public ObservableCollection<ArtistVM> atrist
        {
            get => _artist;
            set
            {
                _artist = value;
                OnPropertyChanged();
            }
        }
        public ArtistViewModel()
        {
            LoadArtist();
        }

        public class ArtistVM
        {
            
        }

        private void LoadArtist()
        {

        }


    }
}
