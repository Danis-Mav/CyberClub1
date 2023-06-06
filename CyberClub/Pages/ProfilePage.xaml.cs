using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace CyberClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        int count =0;
        public static User User { get; set; }
        public ProfilePage(User user)
        {
            InitializeComponent();
            User = user;
            this.DataContext = this;
        }
        private void PhotoClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Filter = "*.jpg|*.jpg|*.png|*.png"
            };
            if (openFile.ShowDialog().GetValueOrDefault())
            {
                User.Img = File.ReadAllBytes(openFile.FileName);
                img.Source = new BitmapImage(new Uri(openFile.FileName));
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            tbFullName.Visibility = Visibility.Hidden;
            tbLogin.Visibility = Visibility.Hidden;
            tbPassword.Visibility = Visibility.Hidden;
            Photo.Visibility = Visibility.Hidden;
            Save.Visibility = Visibility.Hidden;
            

            Nickname.Visibility = Visibility.Visible;
            Login.Visibility = Visibility.Visible;
            Password.Visibility = Visibility.Visible;
            Edit.Visibility = Visibility.Visible;
            //Show.Visibility = Visibility.Visible;
            User.Nickname = tbFullName.Text;
            User.Login = tbLogin.Text;
            User.Password = tbPassword.Text;
            var existingLogin = DBConnection.connection.User.FirstOrDefault(u => u.Login == User.Login);
            if (existingLogin != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return; // Остановка регистрации
            }
            // Проверка существования записи с таким же никнеймом
            var existingNickname = DBConnection.connection.User.FirstOrDefault(u => u.Nickname == User.Nickname);
            if (existingNickname != null)
            {
                MessageBox.Show("Пользователь с таким никнеймом уже существует");
                return; // Остановка регистрации
            }
            DBConnection.connection.SaveChanges();
            NavigationService.Navigate(new ProfilePage(User));
            
        }
        private void EditClick(object sender, RoutedEventArgs e)
        {
            tbFullName.Visibility = Visibility.Visible;
            tbLogin.Visibility = Visibility.Visible;
            tbPassword.Visibility = Visibility.Visible;
            Save.Visibility = Visibility.Visible;
            Photo.Visibility = Visibility.Visible;

            Nickname.Visibility = Visibility.Hidden;
            Login.Visibility = Visibility.Hidden;
            Password.Visibility = Visibility.Hidden;
            Edit.Visibility = Visibility.Hidden;
            tbBalance.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(User));
        }

        private void BalanceClick(object sender, RoutedEventArgs e)
        {
            count++;
            if ((count % 2) == 1)
            {
                tbBalance.Visibility = Visibility.Visible;
            }
            else
            {
                if (int.TryParse(tbBalance.Text, out int balance))
                {
                    User.Balance = balance;
                    DBConnection.connection.SaveChanges();
                    NavigationService.Navigate(new ProfilePage(User));
                }
                else
                {
                    // Обработка некорректного значения баланса
                }
                tbBalance.Visibility = Visibility.Hidden;
            }
        }
    }
}
