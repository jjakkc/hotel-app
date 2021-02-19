using HotelAppLibrary.Databases;
using HotelAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelAppLibrary.Data
{
    public class SqliteData : IDatabaseData
    {
        private const string connectionStringName = "SqliteDb";
        private readonly ISqliteDataAccess _db;

        public SqliteData(ISqliteDataAccess db)
        {
            _db = db;
        }

        public void BookGuest(string firstName, string lastName, int roomTypeId, DateTime startDate, DateTime endDate)
        {
            string sqlStatement = "INSERT INTO Guests (FirstName, LastName) " +
                                  "SELECT @firstName as FirstName, @lastName as LastName " +
                                  "WHERE not exists (" +
                                        "SELECT 1 " +
                                        "FROM Guests " +
                                        "WHERE FirstName = @firstName and LastName = @lastName); " +
                                  "SELECT Id, FirstName, LastName " +
                                  "FROM Guests " +
                                  "WHERE FirstName = @firstName and LastName = @lastName";

            Guest guest = _db.LoadData<Guest, dynamic>(sqlStatement,
                                                       new { firstName, lastName },
                                                       connectionStringName).First();

            // Get room type for later calculating total cost
            RoomType roomType = _db.LoadData<RoomType, dynamic>("select * from RoomTypes where Id = @Id",
                                                                new { Id = roomTypeId },
                                                                connectionStringName).First();

            sqlStatement = "SELECT rt.Id, rt.Title, rt.Description, rt.Price " +
                           "FROM RoomTypes rt " +
                           "INNER JOIN Rooms r ON rt.Id = r.RoomTypeId " +
                           "WHERE r.RoomTypeId = @Id " +
                           "AND " +
                           "r.Id not in (" +
                                 "SELECT b.RoomId " +
                                 "FROM Bookings b " +
                                 "WHERE (@startDate <= b.EndDate and @endDate >= b.StartDate)) " +
                           "GROUP BY rt.Id, rt.Title, rt.Description, rt.Price;"; ;

            // get available rooms
            List<Room> availableRooms = _db.LoadData<Room, dynamic>(sqlStatement,
                                                                    new { startDate, endDate, Id = roomTypeId },
                                                                    connectionStringName);

            TimeSpan timeStaying = endDate.Date.Subtract(startDate.Date);

            sqlStatement = "INSERT INTO Bookings (RoomId, GuestId, StartDate, EndDate, CheckedIn, TotalCost) " +
                           "VALUES (@roomId, @guestId, @startDate, @endDate, @checkedIn, @totalCost);";

            // Book room
            _db.SaveData(sqlStatement,
                         new
                         {
                             roomId = availableRooms.First().Id,
                             guestId = guest.Id,
                             startDate = startDate,
                             endDate = endDate,
                             checkedIn = false,
                             totalCost = timeStaying.Days * roomType.Price
                         },
                         connectionStringName);
        }

        public void CheckInGuest(int bookingId)
        {
            string sqlStatement = "UPDATE Bookings SET CheckedIn = 1 WHERE Id = @Id";
            _db.SaveData(sqlStatement, new { Id = bookingId }, connectionStringName);
        }

        public List<RoomType> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            string sqlStatement = "SELECT rt.Id, rt.Title, rt.Description, rt.Price " +
                                  "FROM RoomTypes rt " +
                                  "INNER JOIN Rooms r ON rt.Id = r.RoomTypeId " +
                                  "WHERE r.Id not in (" +
                                      "SELECT b.RoomId " +
                                      "FROM Bookings b " +
                                      "WHERE (@StartDate <= b.EndDate and @EndDate >= b.StartDate)) " +
                                  "GROUP BY rt.Id, rt.Title, rt.Description, rt.Price;";

            var rooms = _db.LoadData<RoomType, dynamic>(sqlStatement,
                                                        new { StartDate = startDate, EndDate = endDate },
                                                        connectionStringName);

            // converting price to currency value due to accommodating for sqlite int value type
            rooms.ForEach(r => r.Price = r.Price / 100);

            return rooms;
        }

        public RoomType GetRoomTypeById(int id)
        {
            string sqlStatement = "SELECT Id, Title, Description, Price " +
                                  "FROM RoomTypes " +
                                  "WHERE Id = @Id;";

            var room = _db.LoadData<RoomType, dynamic>(sqlStatement, new { Id = id }, connectionStringName).First();

            room.Price = room.Price / 100;

            return room;
        }

        public List<BookingFull> SearchBookings(string lastName)
        {
            string sqlStatement = "SELECT b.Id, b.RoomId, b.GuestId, b.StartDate, b.EndDate, b.CheckedIn, b.TotalCost, " +
                                         "g.FirstName, g.LastName, " +
                                         "r.RoomNumber, r.RoomTypeId, " +
                                         "rt.Title, rt.Description, rt.Price " +
                                  "FROM Bookings b " +
                                  "INNER JOIN Guests g ON b.GuestId = g.Id " +
                                  "INNER JOIN Rooms r ON b.RoomId = r.Id " +
                                  "INNER JOIN RoomTypes rt ON r.RoomTypeId = rt.Id " +
                                  "WHERE g.LastName = @lastName and b.StartDate >= @startDate;";



            var bookings =  _db.LoadData<BookingFull, dynamic>(sqlStatement,
                                                      new { lastName, startDate = DateTime.Now.Date },
                                                      connectionStringName);

            // convert int price to currency
            bookings.ForEach(b => {
                b.Price = b.Price / 100;
                b.TotalCost = b.TotalCost / 100;
            });

            return bookings;
        }
    }
}
