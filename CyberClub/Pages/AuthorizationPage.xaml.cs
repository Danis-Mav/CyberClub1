using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CyberClub.DB;

namespace CyberClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public static ObservableCollection<User> users { get; set; }
        public static User User { get; set; }
        public AuthorizationPage()
        {
            InitializeComponent();
        }
        private void AuthoClick(object sender, RoutedEventArgs e)
        {
            users = new ObservableCollection<User>(DBConnection.connection.User.ToList());
            var User = users.Where(a => a.Login == tb_Login.Text && a.Password == pb_Password.Password).FirstOrDefault();
            if (User != null)
            {
                MessageBox.Show("Добро пожаловать, " + User.Nickname);
                if (User.IdRole == 2)
                {
                    NavigationService.Navigate(new MainPage(User));
                }
                else
                {
                    //   NavigationService.Navigate(new PageAdmin());
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }

        private void RegClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
