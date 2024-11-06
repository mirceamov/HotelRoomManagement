using HotelRoomManagement.CommadStrategies;
using HotelRoomManagement.Extensions;
using Microsoft.Extensions.DependencyInjection;

/*
// print args 
foreach (var arg in args)
{
    Console.WriteLine(arg);
}
*/

args.ValidateAppArgs();
args.ValidateFiles();

// DI setup
var services = new ServiceCollection();
services.RegisterAppDependencies(args);
var serviceProvider = services.BuildServiceProvider();

while (true)
{
    Console.Write("Enter command: ");
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) break;

    var commandArgs = input.Split('(', ')');
    var command = commandArgs[0];
    var parameters = commandArgs[1].Split(',');

    try
    {
        var commandStrategy = serviceProvider
            .GetServices<ICommandStrategy>()
            .SingleOrDefault(c => c.CommandName.Equals(command, StringComparison.InvariantCultureIgnoreCase));

        if (commandStrategy is null)
        {
            Console.WriteLine("Unknown command");
            continue;
        }

        await commandStrategy.ExecuteAsync(parameters);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}