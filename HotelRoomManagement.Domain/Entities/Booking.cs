using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManagement.Domain.Entities
{
    public class Booking
    {
        public string HotelId { get; set; } = string.Empty;
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public string RoomRate { get; set; } = string.Empty;
    }
}
