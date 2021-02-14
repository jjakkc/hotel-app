using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
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
using Microsoft.Extensions.DependencyInjection;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDatabaseData _db;

        public MainWindow(IDatabaseData db)
        {
            InitializeComponent();
            _db = db;
        }

        private void SearchBooking_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(lastNameText.Text))
            {
                MessageBox.Show("Enter a valid last name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var bookings = _db.SearchBookings(lastNameText.Text);
            // associates bookings to our list box on the form so we can use Binding to access properties
            bookingList.ItemsSource = bookings;

            lastNameText.Text = "";
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBooking = (BookingFull)((Button)e.Source).DataContext;

            if (selectedBooking.CheckedIn)
            {
                MessageBox.Show("Guest already checked in", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var checkInForm = App.serviceProvider.GetService<CheckInWindow>();

            checkInForm.PopulateCheckInInfo(selectedBooking);

            checkInForm.ShowDialog();
        }
    }
}
