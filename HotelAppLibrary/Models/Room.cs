using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAppLibrary.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public RoomType RoomInfo { get; set; }
    }
}
