using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spotify_wpf.Models;

namespace Spotify_wpf.ViewModels
{
    public class AddTrackViewModel :BaseViewModel
    {
        public ObservableCollection<string> artists { get; set; }
        private ObservableCollection<Artist> _allArtists;


    }
}
