using Music__.Commands;
using Music__.Properties;
using Music__.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Music__.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ICommand ExitCommand { get; }




        public MainWindowViewModel()
        {
            ExitCommand = new RelayCommand(OnExit);
        }


        public void OnExit()
        {
            Application.Current.Properties["CurrentUser"] = null;
            Settings.Default.userid = 0;
            Settings.Default.Save();
            new LoginWindow().Show();
            foreach (Window w in Application.Current.Windows)
            {
                if (w is not LoginWindow)
                {
                    w.Close();
                }
            }
        }
    }
}
