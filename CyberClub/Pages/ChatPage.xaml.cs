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
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
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
        public static ObservableCollection<Message> Mes { get; set; }

        public ChatPage(User user)
        {
            InitializeComponent();
            Mes = new ObservableCollection<Message>(DBConnection.connection.Message.ToList());
            DataContext = this;
            User = user;
            Messages = new ObservableCollection<Message>();
            LoadMessages();

        }

        private ObservableCollection<Message> _messages;

        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        private void LoadMessages()
        {
            // Загрузка сообщений из базы данных или другого источника данных
            // Пример:
            Mes = new ObservableCollection<Message>(DBConnection.connection.Message);
            lvMessages.ItemsSource = Mes;
            lvMessages.Items.Refresh();

            ScrollToEnd();
            }
        private void ScrollToEnd()
        {
            var scrollViewer = lvMessages.Parent as ScrollViewer;
            scrollViewer?.ScrollToEnd();
        }
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string messageText = txtMessage.Text;
            if (string.IsNullOrEmpty(messageText))
            {
                return;
            }
            // Создание нового сообщения
            var newMessage = new Message
            {
                Text = txtMessage.Text,
                IdUser = User.Id,
                DateTime = DateTime.Now,
                IdChat = 1,
                // Установка других свойств сообщения, например, IdChat и IdUser
            };

            // Сохранение сообщения в базе данных или другом источнике данных
            // Пример:
            DBConnection.connection.Message.Add(newMessage);
            DBConnection.connection.SaveChanges();

            // Добавление нового сообщения в коллекцию и обновление списка сообщений
            Messages.Add(newMessage);

            // Очистка текстового поля для ввода сообщений
            txtMessage.Text = string.Empty;

            LoadMessages();

            lvMessages.ScrollIntoView(lvMessages.Items[lvMessages.Items.Count - 1]);
            lvMessages.UpdateLayout();
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
