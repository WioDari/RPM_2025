using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotApp_wpf.ViewModels
{
    public class AlbumsAddditionViewModel : BaseViewModel
    {
        private string _title { get; set; }
        public string title
        {
            get => _title;
            set
            {
                _title = value;
            }
        }
        private string _dateCreate { get; set; }
        public string dateCreate
        {
            get => _dateCreate;
            set
            {
                _dateCreate = value;
            }
        }
        private List<string> _authors { get; set; }
        public AlbumsAddditionViewModel()
        {

        }
    }
}
