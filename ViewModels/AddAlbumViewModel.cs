using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_wpf.ViewModels
{
    class AddAlbumViewModel :BaseViewModel
    {
        private string _namealbum {  get; set; }
        public string Namealbum
        {
            get => _namealbum;
            set
            {
                _namealbum = value;
                OnPropertyChanged();
            }
        }



       

    }
}
