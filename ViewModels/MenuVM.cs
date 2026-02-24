using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace PracticeWork.ViewModels
{
    internal class MenuVM
    {
        public ICommand ExitCommand { get; set; }
        public MenuVM()
        {
            ExitCommand = new RelayCommand(Exit);
        }

        public void Exit()
        {
            foreach (Window w in App.Current.Windows)
            {
                w.Close();
            }
        }
    }
}
