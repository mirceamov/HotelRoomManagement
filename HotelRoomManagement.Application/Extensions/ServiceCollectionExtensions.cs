using HotelRoomManagement.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManagement.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterQueryHandlers(this ServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
                    .Select(i => new { InterfaceType = i, ImplementationType = t }))
                .ToList();

            // Register each handler with its corresponding IQueryHandler interface
            foreach (var handlerType in handlerTypes)
            {
                services.AddTransient(handlerType.InterfaceType, handlerType.ImplementationType);
            }

            return services;
        }
    }
}
