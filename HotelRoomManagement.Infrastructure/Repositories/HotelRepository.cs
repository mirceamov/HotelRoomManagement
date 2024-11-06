using HotelRoomManagement.Domain.Entities;
using HotelRoomManagement.Domain.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HotelRoomManagement.Infrastructure.Repositories
{
    public class HotelRepository : BaseJsonRepository, IHotelRepository
    {
        private const string JSON_PROP_ID = "id";

        public HotelRepository(string hotelFilePath) : base(hotelFilePath)
        { }

        public async Task<Hotel> GetHotelAsync(string hotelId)
        {
            using var reader = new StreamReader(_filePath);
            using var jsonReader = new JsonTextReader(reader);
            var json = await JToken.LoadAsync(jsonReader);
            var hotel = json
                .FirstOrDefault(h => h[JSON_PROP_ID]?.Value<string>() == hotelId)?
                .ToObject<Hotel>();

            return hotel ?? throw new ArgumentException($"Hotel {hotelId} doesn't exist.");
        }

        public async Task<List<Hotel>> GetAllHotelsAsync()
        {
            return await ReadFromJsonFileAsync<Hotel>(_filePath);
        }
    }
}