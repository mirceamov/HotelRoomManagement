using HotelRoomManagement.Application.Queries;
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
            var availabilityQuery = CreateAvailabilityQuery(parameters);

            var availability = await _availabilityQueryHandler.HandleAsync(availabilityQuery);

            Console.WriteLine($"Availability: {availability}");
        }

        private AvailabilityQuery CreateAvailabilityQuery(string[] parameters)
        {
            var hotelId = parameters[0].Trim();
            var dates = parameters[1].Trim().Split('-');
            var startDate = DateTime.ParseExact(dates[0], Constants.JsonDateFormat, null);
            var endDate = dates.Length > 1 ? DateTime.ParseExact(dates[1], Constants.JsonDateFormat, null) : startDate;
            var roomType = parameters[2].Trim();

            return new AvailabilityQuery
            {
                HotelId = hotelId,
                StartDate = startDate,
                EndDate = endDate,
                RoomType = roomType
            };
        }
    }
}
