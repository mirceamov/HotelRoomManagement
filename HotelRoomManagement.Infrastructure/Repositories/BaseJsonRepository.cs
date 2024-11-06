using HotelRoomManagement.Domain;
using HotelRoomManagement.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManagement.Infrastructure.Repositories
{
    public class BaseJsonRepository
    {
        protected readonly string _filePath;

        public BaseJsonRepository(string filePath)
        {
            _filePath = filePath;
        }

        protected JsonSerializerSettings DefaultSettings { get; } = new JsonSerializerSettings
        {
            DateFormatString = Constants.JsonDateFormat,
            NullValueHandling = NullValueHandling.Ignore
        };

        protected async Task<List<T>> ReadFromJsonFileAsync<T>(string filePath)
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<List<T>>(json, DefaultSettings) ?? new List<T>();
        }
    }
}
