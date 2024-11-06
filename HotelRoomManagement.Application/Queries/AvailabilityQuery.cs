using HotelRoomManagement.Domain.Interfaces;

namespace HotelRoomManagement.Application.Queries
{
    public class AvailabilityQuery : IQuery<AvailabilityQuery, int>
    {
        public string HotelId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RoomType { get; set; } = string.Empty;
    }
}
