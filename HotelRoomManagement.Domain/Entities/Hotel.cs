namespace HotelRoomManagement.Domain.Entities
{
    public class Hotel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<RoomType> RoomTypes { get; set; } = new List<RoomType>();
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}