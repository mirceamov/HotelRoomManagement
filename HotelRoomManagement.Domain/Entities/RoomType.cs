using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManagement.Domain.Entities
{
    public class RoomType
    {
        public string Code { get; set; } = string.Empty;

        public int Size { get; set; }

        public string Description { get; set; } = string.Empty;

        public List<string> Amenities { get; set; } = new List<string>();

        public List<string> Features { get; set; } = new List<string>();
    }
}
