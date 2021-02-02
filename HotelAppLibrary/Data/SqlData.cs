using HotelAppLibrary.Models;
using HotelAppLibrary.Databases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HotelAppLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISQLDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISQLDataAccess db)
        {
            _db = db;
        }

        public List<RoomType> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            return _db.LoadData<RoomType, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                            new { startDate = startDate, endDate = endDate },
                                            connectionStringName,
                                            true);
        }

        public RoomType GetRoomTypeById(int id)
        {
            return _db.LoadData<RoomType, dynamic>("dbo.spRoomTypes_GetRoomById", new { Id = id }, connectionStringName, true).FirstOrDefault();
        }

        public void BookGuest(string firstName, string lastName, int roomTypeId, DateTime startDate, DateTime endDate)
        {
            // Insert if new -> get guest
            Guest guest = _db.LoadData<Guest, dynamic>("dbo.spGuests_Insert",
                                                       new { firstName, lastName },
                                                       connectionStringName,
                                                       true).First();

            // Get room type for later calculating total cost
            RoomType roomType = _db.LoadData<RoomType, dynamic>("select * from dbo.RoomTypes where Id = @Id",
                                                                new { Id = roomTypeId },
                                                                connectionStringName).First();

            // get available rooms
            List<Room> availableRooms = _db.LoadData<Room, dynamic>("dbo.spRooms_GetAvailableRooms",
                                                                    new { startDate, endDate, roomTypeId },
                                                                    connectionStringName,
                                                                    true);

            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            // Book room
            _db.SaveData("dbo.spBookings_Insert",
                         new
                         {
                             roomId = availableRooms.First().Id,
                             guestId = guest.Id,
                             startDate = startDate,
                             endDate = endDate,
                             checkedIn = false,
                             totalCost = timeStaying.Days * roomType.Price
                         },
                         connectionStringName,
                         true);
        }

        public List<BookingFull> SearchBookings(string lastName)
        {
            return _db.LoadData<BookingFull, dynamic>("dbo.spBookings_Search",
                                                  new { lastName, startDate = DateTime.Now.Date },
                                                  connectionStringName,
                                                  true);
        }

        public void CheckInGuest(int bookingId)
        {
            _db.SaveData("dbo.spBookings_CheckIn", new { Id = bookingId }, connectionStringName, true);
        }

        public void CheckInGuest(string lastName, int roomId)
        {
            int guestId = _db.LoadData<Guest, dynamic>("select * from dbo.Guests where LastName = @lastName;", new { lastName }, connectionStringName).First().Id;

            string sql = "UPDATE dbo.Bookings set CheckedIn = 1 WHERE GuestId = @guestId and RoomId = @roomId";

            _db.SaveData(sql, new { guestId, roomId }, connectionStringName);
        }

    }
}
