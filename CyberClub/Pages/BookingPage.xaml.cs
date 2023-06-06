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
using Table = CyberClub.DB.Table;

namespace CyberClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для BookingPage.xaml
    /// </summary>
    public partial class BookingPage : Page, INotifyPropertyChanged
    {
        private bool isMenuOpen;
        int count;
        public ObservableCollection<Table> Table { get; set; }
        public bool IsMenuOpen
        {
            get { return isMenuOpen; }
            set
            {
                isMenuOpen = value;
                OnPropertyChanged(nameof(IsMenuOpen));
            }
        }

        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BookingPage(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            ShowTable();
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
            NavigationService.Navigate(new NewsPage(CurrentUser));
        }

        private void NavigateToEventsPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new EventsPage(CurrentUser));
        }

        private void NavigateToBookingPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new BookingPage(CurrentUser));
        }

        private void NavigateToTariffPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new TariffPage(CurrentUser));
        }

        private void NavigateToChatPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new ChatPage(CurrentUser));
        }

        private void NavigateToAboutPage(object sender, RoutedEventArgs e)
        {
            IsMenuOpen = false;
            NavigationService.Navigate(new MainPage(CurrentUser));
        }

        private void NavigateToProfilePage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage(CurrentUser));
        }
        private void ShowTable()
        {

            Table = new ObservableCollection<Table>(DBConnection.connection.Table.Where(x => x.IsDeleted == false).ToList());
            DataContext = this;
            LViewTable.ItemsSource = Table;
            LViewTable.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }
        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (LViewTable.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                foreach (var item in LViewTable.Items)
                {
                    var listViewItem = LViewTable.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                    var table = (Table)item;
                    if ((table.IdUser != null) && table.IdUser != CurrentUser.Id)
                    {
                        listViewItem.Background = Brushes.Gray;
                    }
                    if (table.IdUser == CurrentUser.Id && table.IdUser != null)
                    {
                        listViewItem.Background = Brushes.Red;
                    }
                    if (table.IdUser == null)
                    {
                        listViewItem.Background = Brushes.Aquamarine;
                    }
                    if (((table.IdUser != null) && table.IdUser != CurrentUser.Id) && table.IdType == 2)
                    {
                        listViewItem.Background = Brushes.Purple;
                    }

                }
            }
        }
        private void BookTables()
        {
            int selectedCount = LViewTable.SelectedItems.Count;
            if (selectedCount > 5 )
            {
                MessageBox.Show("Максимальное количество выбранных мест - 5");
                return;
            }


            foreach (var table in LViewTable.SelectedItems)
            {
                var selectedTable = (Table)table;
                if (selectedTable.IsBooked == true || selectedTable.IdUser != null)
                {
                MessageBox.Show("Данный стол уже забронирован!");
                 return;
                }
                selectedTable.IdUser = CurrentUser.Id;
                selectedTable.IsBooked = true;
            }

            DBConnection.connection.SaveChanges();

            foreach (var table in LViewTable.Items)
            {
                var listItem = (Table)table;
                if (LViewTable.SelectedItems.Contains(listItem))
                {
                    listItem.IsBooked = true;
                }
                else
                {
                    listItem.IsBooked = false;
                }
            }

            DBConnection.connection.SaveChanges();

            MessageBox.Show("Места успешно забронированы!");
        }

        private void BookTablesButton_Click(object sender, RoutedEventArgs e)
        {
            BookTables();
        }
        private void ShowBookedTablesButton_Click(object sender, RoutedEventArgs e)
        {
            count++;
            if ((count % 2) == 1)
            {
                RemoveBooking.Visibility = Visibility.Visible;
                BookButton.Visibility = Visibility.Hidden;
                var bookedTables = Table.Where(t => t.IdUser == CurrentUser.Id /*&& t.IsBooked.GetValueOrDefault()*/).ToList();
                LViewTable.ItemsSource = bookedTables;
                ShowBookedTablesButton.Content = "Показать все столы";
            }
            else
            {
                RemoveBooking.Visibility = Visibility.Hidden;
                BookButton.Visibility = Visibility.Visible;
                ShowTable();
                ShowBookedTablesButton.Content = "Показать забронированные места";
            }
        }

        private void RemoveBooking_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in LViewTable.SelectedItems)
            {
                var selectedTable = (Table)item;
                selectedTable.IsBooked = false;
                selectedTable.IdUser = null;
            }

            DBConnection.connection.SaveChanges();
            var bookedTables = Table.Where(t => t.IdUser == CurrentUser.Id && t.IsBooked.GetValueOrDefault()).ToList();
            LViewTable.ItemsSource = bookedTables;

            MessageBox.Show("Бронирование успешно удалено!");
        }
    }
}
