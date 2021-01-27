using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;

namespace HotelAppLibrary.Data
{
    public interface IDatabaseData
    {
        void BookGuest(string firstName, string lastName, int roomTypeId, DateTime startDate, DateTime endDate);
        void CheckInGuest(int bookingId);
        List<RoomType> GetAvailableRoomTypes(DateTime startDate, DateTime endDate);
        List<BookingFull> SearchBookings(string lastName);
    }
}