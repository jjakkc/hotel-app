using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for CheckInWindow.xaml
    /// </summary>
    public partial class CheckInWindow : Window
    {
        private readonly IDatabaseData _db;
        private BookingFull _data = null;

        public CheckInWindow(IDatabaseData db)
        {
            InitializeComponent();
            _db = db;
        }

        public void PopulateCheckInInfo(BookingFull data)
        {
            _data = data;

            firstNameText.Text = _data.FirstName;
            lastNameText.Text = _data.LastName;
            roomTypeText.Text = _data.Title;
            roomNumText.Text = _data.RoomNumber;
            descriptionText.Text = _data.Description;
            startDateText.Text = _data.StartDate.ToShortDateString();
            endDateText.Text = _data.EndDate.ToShortDateString();
            costText.Text = string.Format("{0:C}", _data.TotalCost);
        }

        private void ConfirmCheckInButton_Click(object sender, RoutedEventArgs e)
        {
            _db.CheckInGuest(_data.Id);

            MessageBox.Show("Guest is now checked in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
    }
}
