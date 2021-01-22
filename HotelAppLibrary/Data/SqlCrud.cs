using HotelAppLibrary.Models;
using HotelAppLibrary.Databases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace HotelAppLibrary.Data
{
    public class SqlCrud
    {
        private readonly ISQLDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlCrud(ISQLDataAccess db)
        {
            _db = db;
        }
        public List<Room> GetAvailableRooms()
        {
            string sql = "SELECT Id, RoomNumber, RoomTypeId " +
                "FROM dbo.Rooms r" +
                "INNER JOIN dbo.Bookings b ON b.RoomId = r.Id" +
                "WHERE b.StartDate < GetDate() OR b.EndDate >= GetDate()";
            List<Room> output = _db.LoadData<Room, dynamic>(sql, new { }, "Default");

            foreach(var room in output)
            {
                // get room type info
                sql = "SELECT rt.* FROM dbo.RoomTypes rt" +
                    "INNER JOIN dbo.Rooms r ON r.RoomTypeId = rt.Id" +
                    "WHERE r.Id = @Id";
                room.RoomInfo = _db.LoadData<RoomType, dynamic>(sql, new { Id = room.RoomTypeId }, "Default").FirstOrDefault();
            }

            return output;
        }

        public List<RoomType> GetAvailableRoomTypes(DateTime startDate, DateTime endDate)
        {
            return _db.LoadData<RoomType, dynamic>("dbo.spRoomTypes_GetAvailableTypes",
                                            new { startDate = startDate, endDate = endDate },
                                            connectionStringName,
                                            true);
        }
    }
}
