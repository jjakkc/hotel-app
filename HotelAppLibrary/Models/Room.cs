﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAppLibrary.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
    }
}
