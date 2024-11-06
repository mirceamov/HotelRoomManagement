using HotelRoomManagement.Domain.Interfaces;

namespace HotelRoomManagement.Application.Queries
{
    public class RoomTypesQuery : IQuery<RoomTypesQuery, string>
    {
        public string HotelId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Guests { get; set; }
    }
}
