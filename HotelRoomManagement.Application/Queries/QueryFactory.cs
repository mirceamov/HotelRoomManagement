
using HotelRoomManagement.Application.Queries.Availability;
using HotelRoomManagement.Application.Queries.RoomTypes;
using HotelRoomManagement.Domain;

namespace HotelRoomManagement.Application.Queries
{
    public static class QueryFactory
    {
        public static T CreateQuery<T>(string[] parameters) where T : new()
        {
            if (parameters.Length < 3)
                throw new ArgumentException("Insufficient parameters provided.");

            var hotelId = parameters[0].Trim();
            var dates = parameters[1].Trim().Split('-');
            var startDate = DateTime.ParseExact(dates[0], Constants.JsonDateFormat, null);
            var endDate = dates.Length > 1 ? DateTime.ParseExact(dates[1], Constants.JsonDateFormat, null) : startDate;

            var query = new T();

            SetProperty(query, nameof(AvailabilityQuery.HotelId), hotelId);
            SetProperty(query, nameof(AvailabilityQuery.StartDate), startDate);
            SetProperty(query, nameof(AvailabilityQuery.EndDate), endDate);

            MapSpecificProperties(query, parameters);

            return query;
        }

        private static void SetProperty<T>(T query, string propertyName, object value)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(query, value);
            }
        }

        private static void MapSpecificProperties<T>(T query, string[] parameters)
        {
            if (query is AvailabilityQuery availabilityQuery)
            {
                availabilityQuery.RoomType = parameters[2].Trim();
            }
            else if (query is RoomTypesQuery roomTypesQuery)
            {
                roomTypesQuery.Guests = int.Parse(parameters[2].Trim());
            }
        }
    }
}
