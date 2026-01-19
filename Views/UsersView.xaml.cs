using System.Windows.Controls;
using MusicPlus.ViewModels;

namespace MusicPlus.Views
{
    public partial class UsersView : UserControl
    {
        private readonly UsersViewModel _viewModel;

        public UsersView()
        {
            InitializeComponent();
            _viewModel = new UsersViewModel();
            DataContext = _viewModel;
        }
    }
}
