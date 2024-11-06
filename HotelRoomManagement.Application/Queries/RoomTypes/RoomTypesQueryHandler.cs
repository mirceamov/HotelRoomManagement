using HotelRoomManagement.Domain.Entities;
using HotelRoomManagement.Domain.Interfaces;

namespace HotelRoomManagement.Application.Queries.RoomTypes
{
    public class RoomTypesQueryHandler : IQueryHandler<RoomTypesQuery, string>
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;

        public RoomTypesQueryHandler(IHotelRepository hotelRepository, IBookingRepository bookingRepository)
        {
            _hotelRepository = hotelRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<string> HandleAsync(RoomTypesQuery query)
        {
            var hotel = await _hotelRepository.GetHotelAsync(query.HotelId);
            var hotelBookings = await _bookingRepository.GetHotelBookingsAsync(query.HotelId);

            // Step 1: Guard Clause - Check if there's enough capacity
            int availableCapacity = CalculateAvailableCapacity(hotel, hotelBookings, query.StartDate, query.EndDate);

            if (availableCapacity < query.Guests)
            {
                throw new Exception("Not enough rooms available to accommodate all guests.");
            }

            // Step 2: Allocate Rooms
            var allocatedRooms = AllocateRooms(hotel, hotelBookings, query.StartDate, query.EndDate, query.Guests);

            return $"{query.HotelId}: " + string.Join(", ", allocatedRooms);
        }

        private static int CalculateAvailableCapacity(Hotel hotel, List<Booking> bookings, DateTime startDate, DateTime endDate)
        {
            var bookedRoomTypeCounts = GetBookedRoomTypeCounts(bookings, startDate, endDate);

            int bookedCapacity = bookedRoomTypeCounts.Sum(kvp =>
                kvp.Value * hotel.RoomTypes.First(rt => rt.Code == kvp.Key).Size);

            int totalHotelCapacity = hotel.Rooms
                .Sum(r => hotel.RoomTypes.First(rt => rt.Code == r.RoomType).Size);

            return totalHotelCapacity - bookedCapacity;
        }

        private static List<string> AllocateRooms(Hotel hotel, List<Booking> bookings, DateTime startDate, DateTime endDate, int guests)
        {
            // Step 1: Calculate unavailable room counts per room type based on bookings
            var bookedRoomTypeCounts = GetBookedRoomTypeCounts(bookings, startDate, endDate);

            // Step 2: Calculate available room counts for each room type
            var availableRoomCounts = hotel.RoomTypes
                .ToDictionary(
                    rt => rt.Code,
                    rt => hotel.Rooms.Count(r => r.RoomType == rt.Code) - bookedRoomTypeCounts.GetValueOrDefault(rt.Code, 0)
                );

            // Step 3: Allocate rooms optimally
            var requiredRooms = new List<string>();
            int remainingGuests = guests;

            // Order room types by size (largest first for more efficient packing)
            var roomTypes = hotel.RoomTypes
                .Where(rt => availableRoomCounts.ContainsKey(rt.Code) && availableRoomCounts[rt.Code] > 0)
                .OrderByDescending(rt => rt.Size).ToList();

            foreach (var roomType in roomTypes)
            {
                int availableRoomsOfType = availableRoomCounts[roomType.Code];
                int roomsNeeded = remainingGuests / roomType.Size;

                // Limit rooms needed to available count
                if (roomsNeeded > availableRoomsOfType)
                {
                    roomsNeeded = availableRoomsOfType;
                }

                // Deduct guests according to the rooms allocated
                remainingGuests -= roomsNeeded * roomType.Size;

                // Add allocated rooms to the list
                for (int i = 0; i < roomsNeeded; i++)
                {
                    requiredRooms.Add(roomType.Code);
                }

                // Update the available count after allocation
                availableRoomCounts[roomType.Code] -= roomsNeeded;
            }

            // Handle any remaining guests with a partially filled room
            if (remainingGuests > 0)
            {
                // Find the smallest available room that can accommodate the partial allocation
                var smallestRoomType = roomTypes.First(rt => availableRoomCounts[rt.Code] > 0);

                // Partially allocate the room and mark it with "!"
                requiredRooms.Add(smallestRoomType.Code + "!");
            }

            return requiredRooms;
        }

        private static Dictionary<string, int> GetBookedRoomTypeCounts(List<Booking> bookings, DateTime startDate, DateTime endDate)
        {
            return bookings
                .Where(b => b.Arrival < endDate && b.Departure > startDate)
                .GroupBy(b => b.RoomType)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
