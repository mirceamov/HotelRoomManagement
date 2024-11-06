using HotelRoomManagement.Domain.Interfaces;
using HotelRoomManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using HotelRoomManagement.Application.Extensions;
using HotelRoomManagement.CommadStrategies;

namespace HotelRoomManagement.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAppDependencies(this ServiceCollection serviceCollection, string[] args)
        {
            serviceCollection.RegisterCommandStrategies();
            serviceCollection.RegisterQueryHandlers();
            RegisterRepositories(serviceCollection, args);
        }

        private static ServiceCollection RegisterCommandStrategies(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICommandStrategy, AvailabilityStrategy>();
            serviceCollection.AddSingleton<ICommandStrategy, RoomTypesStrategy>();

            return serviceCollection;
        }

        private static ServiceCollection RegisterRepositories(this ServiceCollection serviceCollection, string[] args)
        {
            var hotelFilePath = args[1];
            var bookingFilePath = args[3];

            serviceCollection.AddSingleton<IHotelRepository>(_ => new HotelRepository(hotelFilePath));
            serviceCollection.AddSingleton<IBookingRepository>(_ => new BookingRepository(bookingFilePath));

            return serviceCollection;
        }
    }
}
