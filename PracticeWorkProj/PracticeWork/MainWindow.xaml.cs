using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PracticeWork.Context;
using PracticeWork.Models;

namespace PracticeWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loginbox.Text = "antonina.pushmenkova";
            passwordbox.Text = "it3cy55";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var context = new PostgresContext();
            User user = context.Users.FirstOrDefault(u => u.UserLogin == loginbox.Text && u.UserPassword == passwordbox.Text)!;
            if (user != null)
            {
                App.Current.Properties["CurrentUser"] = user;
                var mainmenu = new MainMenu();
                mainmenu.Show();
                foreach (Window w in App.Current.Windows)
                {
                    if (w is not MainMenu)
                    {
                        w.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Введены неправильные данные для входа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}