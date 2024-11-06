using HotelRoomManagement.Application.Queries;
using HotelRoomManagement.Application.Queries.Availability;
using HotelRoomManagement.Domain;
using HotelRoomManagement.Domain.Interfaces;

namespace HotelRoomManagement.CommadStrategies
{
    public class AvailabilityStrategy : ICommandStrategy
    {
        private readonly IQueryHandler<AvailabilityQuery, int> _availabilityQueryHandler;
        public string CommandName => "Availability";

        public AvailabilityStrategy(IQueryHandler<AvailabilityQuery, int> availabilityQueryHandler)
        {
            _availabilityQueryHandler = availabilityQueryHandler;
        }

        public async Task ExecuteAsync(string[] parameters)
        {
            var availabilityQuery = QueryFactory.CreateQuery<AvailabilityQuery>(parameters);

            var availability = await _availabilityQueryHandler.HandleAsync(availabilityQuery);

            Console.WriteLine($"Availability: {availability}");
        }
    }
}
