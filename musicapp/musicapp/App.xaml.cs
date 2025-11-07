using System.Windows;

namespace musicapp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
        }
    }
}