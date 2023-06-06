using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using DBTable = CyberClub.DB.Table;

namespace CyberClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private bool isMenuOpen;

        public bool IsMenuOpen
        {
            get { return isMenuOpen; }
            set
            {
                isMenuOpen = value;
                OnPropertyChanged(nameof(IsMenuOpen));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public static User User { get; set; }
        public MainPage(User user)
        {
            InitializeComponent();
            User = user;
            DataContext = user;
            
            this.DataContext = this;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ToggleMenu(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = !IsMenuOpen;
            menuPanel.Visibility = IsMenuOpen ? Visibility.Visible : Visibility.Collapsed;
        }

        private void NavigateToNewsPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new NewsPage(User));
        }

        private void NavigateToEventsPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new EventsPage(User));
        }

        private void NavigateToBookingPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new BookingPage(User));
        }

        private void NavigateToTariffPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new TariffPage(User));
        }

        private void NavigateToChatPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new ChatPage(User));
        }
        private void NavigateToAboutPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new MainPage(User));
        }
        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage(User));
        }
    }
}