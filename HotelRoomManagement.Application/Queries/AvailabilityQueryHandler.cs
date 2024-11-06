using HotelRoomManagement.Domain.Entities;
using HotelRoomManagement.Domain.Interfaces;

namespace HotelRoomManagement.Application.Queries
{
    public class AvailabilityQueryHandler : IQueryHandler<AvailabilityQuery, int>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;

        public AvailabilityQueryHandler(IHotelRepository hotelRepository, IBookingRepository bookingRepository)
        {
            _hotelRepository = hotelRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<int> HandleAsync(AvailabilityQuery query)
        {
            var hotel = await _hotelRepository.GetHotelAsync(query.HotelId);
            var hotelBookings = await _bookingRepository.GetHotelBookingsAsync(query.HotelId);

            var roomCount = hotel.Rooms.Count(r => r.RoomType == query.RoomType);
            var bookedCount = hotelBookings.Count(b =>
                b.RoomType == query.RoomType &&
                b.Arrival <= query.EndDate &&
                b.Departure > query.StartDate);

            return roomCount - bookedCount;
        }
    }
}
