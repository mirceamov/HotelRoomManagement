using HotelRoomManagement.Domain.Entities;
using HotelRoomManagement.Domain.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HotelRoomManagement.Infrastructure.Repositories
{
    public class BookingRepository : BaseJsonRepository, IBookingRepository
    {
        private const string JSON_PROP_HOTELID = "hotelId";

        public BookingRepository(string filePath) : base(filePath)
        {
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            var bookings = await ReadFromJsonFileAsync<Booking>(_filePath);
            return await Task.FromResult(bookings);
        }

        public async Task<List<Booking>> GetHotelBookingsAsync(string hotelId)
        {
            var filteredBookings = new List<Booking>();

            using (var reader = new StreamReader(_filePath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                if (await jsonReader.ReadAsync() && jsonReader.TokenType == JsonToken.StartArray)
                {
                    while (await jsonReader.ReadAsync())
                    {
                        if (jsonReader.TokenType == JsonToken.StartObject)
                        {
                            var bookingToken = await JToken.ReadFromAsync(jsonReader);

                            if (bookingToken[JSON_PROP_HOTELID]?.Value<string>() == hotelId)
                            {
                                var booking = bookingToken.ToObject<Booking>();
                                if (booking != null)
                                {
                                    filteredBookings.Add(booking);
                                }
                            }
                        }
                        else if (jsonReader.TokenType == JsonToken.EndArray)
                        {
                            break;
                        }
                    }
                }
            }

            return filteredBookings;
        }

    }
}
