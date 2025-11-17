using Microsoft.EntityFrameworkCore;
using SpotApp_wpf.Context;
using SpotApp_wpf.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotApp_wpf.ViewModels
{
    internal class AlbumViewModel : BaseViewModel
    {


        public class AlbTemplate
        {
            public string title { get; set; }
            public string author { get; set; }
            public string tracksCount { get; set; }
        };
        public ObservableCollection<AlbTemplate> albums { get; set; }

        private void LoadAlbums()
        {
            var context = new SpotifyContext();
            /*albums = new ObservableCollection<AlbTemplate>(context.Artists
                .Include(a => a.Albums)
                .Select( a => new AlbTemplate
                {
                    title = a.Albums.,
                    author = a.Artist
                }));*/
        }
    }
}
