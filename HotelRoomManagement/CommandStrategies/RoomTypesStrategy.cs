using HotelRoomManagement.Application.Queries;
using HotelRoomManagement.Domain.Interfaces;

namespace HotelRoomManagement.CommadStrategies
{
    public class RoomTypesStrategy : ICommandStrategy
    {
        private readonly IQueryHandler<RoomTypesQuery, string> _roomTypesQueryHandler;
        public string CommandName => "RoomTypes";

        public RoomTypesStrategy(IQueryHandler<RoomTypesQuery, string> roomTypesQueryHandler)
        {
            _roomTypesQueryHandler = roomTypesQueryHandler;
        }

        public async Task ExecuteAsync(string[] parameters)
        {
            var roomTypesQuery = CreateRoomTypesQuery(parameters);

            var roomTypes = await _roomTypesQueryHandler.HandleAsync(roomTypesQuery);
            Console.WriteLine(roomTypes);
        }

        private RoomTypesQuery CreateRoomTypesQuery(string[] parameters)
        {
            var hotelId = parameters[0].Trim();
            var dates = parameters[1].Trim().Split('-');
            var startDate = DateTime.ParseExact(dates[0], "yyyyMMdd", null);
            var endDate = dates.Length > 1 ? DateTime.ParseExact(dates[1], "yyyyMMdd", null) : startDate;
            var guests = int.Parse(parameters[2].Trim());

            return new RoomTypesQuery
            {
                HotelId = hotelId,
                StartDate = startDate,
                EndDate = endDate,
                Guests = guests
            };
        }
    }
}
