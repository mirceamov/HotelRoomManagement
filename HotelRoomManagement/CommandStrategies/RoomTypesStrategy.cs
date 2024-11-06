using HotelRoomManagement.Application.Queries.RoomTypes;
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
            var roomTypesQuery = QueryFactory.CreateQuery<RoomTypesQuery>(parameters);

            var roomTypes = await _roomTypesQueryHandler.HandleAsync(roomTypesQuery);
            Console.WriteLine(roomTypes);
        }
    }
}
