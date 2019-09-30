using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssignmentFIT5032.Models
{
    public class RoomBookingModel
    {
        public Room RM { get; set; }
        public List<Room> Rooms { get; set; }
        public Booking ApplyBooking { get; set; }
    }
}