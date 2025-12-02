using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PracticeWork.ViewModels
{
    class AlbumCard : BaseVM
    {
        private ImageSource _albumcover;
        public ImageSource albumcover
        {
            get => _albumcover;
            set
            {
                _albumcover = value;
                OnPropertyChanged();
            }
        }
        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }
        private string _artist;
        public string artist
        {
            get => _artist;
            set
            {
                _artist = value;
                OnPropertyChanged();
            }
        }
    }
}
