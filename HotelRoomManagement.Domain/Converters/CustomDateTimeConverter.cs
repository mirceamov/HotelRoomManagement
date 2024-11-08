﻿using Newtonsoft.Json;

namespace HotelRoomManagement.Domain.Converters
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var dateString = (string)reader.Value;
            return DateTime.ParseExact(dateString, Constants.JsonDateFormat, null);
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(Constants.JsonDateFormat));
        }
    }
}
