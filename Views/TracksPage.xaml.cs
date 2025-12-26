using SpotApp_wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotApp_wpf.Views
{
    /// <summary>
    /// Логика взаимодействия для TracksPage.xaml
    /// </summary>
    public partial class TracksPage : Page
    {
        public TracksPage()
        {
            InitializeComponent();
            DataContext = new TracksViewModel();
        }
    }
}
