using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Musick.Context;
using Musick.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Musick.ViewModel
{
     public partial class AlbumsPageViewModel : ObservableObject
    {

        public AlbumsPageViewModel() 
        {
            LoadAlbums();
        }



        public class AlbumPageViewList()
        {
            public int ID { get; set; }
            public string AlbumName { get; set; }

            public string ArtistName { get; set; } 

            public string CountTraks { get; set; } 

            public string Photo {  get; set; }
        }


        
        public ObservableCollection<AlbumPageViewList> albumsList {  get; set; }

        public ObservableCollection<AlbumPageViewList> albumsListt { get; set; }



        public void LoadAlbums()
        {
            MusicContext context = new MusicContext();

            albumsList = new ObservableCollection<AlbumPageViewList>(context.Albums
               .Include(x => x.Artist)
               .Select(x=> new AlbumPageViewList
               {
                   ID = x.AlbumId,
                   AlbumName = x.AlbumName,
                   ArtistName = x.Artist.ArtistName,
                  


               }).OrderBy(x=>x.ID).ToList());

            
           
            foreach (var a in albumsList)
            {
                a.CountTraks =  $"Треков:\n      {context.Tracks.Count(x => x.AlbumId == a.ID).ToString()}";
                if (File.Exists($"C:\\Users\\DODATKINS\\Desktop\\Musick\\Musick\\Resources\\covers\\{a.AlbumName}.jpg") == true)
                {
                    a.Photo = $"/Resources/covers/{a.AlbumName}.jpg";
                }
                else
                {
                    a.Photo = "/Resources/Flow.png";
                }
                
            }
            
        }




    }
}
