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
using CyberClub.Pages;
using CyberClub.DB;

namespace CyberClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
            this.DataContext = this;

        }

        private void RegClick(object sender, RoutedEventArgs e)
        {
            var a = new User();
            a.Nickname = tb_FullName.Text;
            a.Login = tb_Login.Text;
            a.Password = pb_Password.Password;
            a.IdRole = 2;
            a.IdTariff = 1;
            a.Balance = 0;
            var existingLogin = DBConnection.connection.User.FirstOrDefault(u => u.Login == a.Login);
            if (existingLogin != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return; // Остановка регистрации
            }
            // Проверка существования записи с таким же никнеймом
            var existingNickname = DBConnection.connection.User.FirstOrDefault(u => u.Nickname == a.Nickname);
            if (existingNickname != null)
            {
                MessageBox.Show("Пользователь с таким никнеймом уже существует");
                return; // Остановка регистрации
            }
            DBConnection.connection.User.Add(a);
            DBConnection.connection.SaveChanges();
            MessageBox.Show("Вы успешно зарегестрированы " + a.Nickname);
            NavigationService.GoBack();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void tbPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
