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

namespace CyberClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для EventsPage.xaml
    /// </summary>
    public partial class EventsPage : Page
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
        public static ObservableCollection<Event> Events { get; set; }
        public EventsPage(User user)
        {
            InitializeComponent();
            User = user;
            Events = new ObservableCollection<Event>(DBConnection.connection.Event.Where(x => x.IsDeleted != true).ToList());
            this.DataContext = this;
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Button registerButton = (Button)sender;
            Event selectedEvent = (Event)registerButton.DataContext;

            // Проверка наличия свободных мест
            if (selectedEvent.MaxUser != null && selectedEvent.UserEvent.Count >= selectedEvent.MaxUser)
            {
                MessageBox.Show("Извините, мест больше нет.");
                return;
            }

            // Проверка, участвует ли пользователь уже в данном событии
            if (selectedEvent.UserEvent.Any(ue => ue.IdUser == User.Id && ue.IdEvent == selectedEvent.Id))
            {
                MessageBox.Show("Вы уже участвуете в данном событии.");
                return;
            }

            // Создание новой записи UserEvent и сохранение в базе данных
            UserEvent userEvent = new UserEvent
            {
                IdEvent = selectedEvent.Id,
                IdUser = User.Id
            };

            // Ваш код для сохранения userEvent в базе данных или в других местах

            // Добавление userEvent к коллекции UserEvent события
            selectedEvent.UserEvent.Add(userEvent);

            MessageBox.Show("Вы успешно зарегистрированы на событие.");

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
