using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManagement.Extensions
{
    public static class ArgsExtensions
    {
        public static void ValidateAppArgs(this string[] args)
        {
            if (args.Length != 4 || args[0] != "--hotels" || args[2] != "--bookings")
            {
                Console.WriteLine("Usage: HotelRoomManagement --hotels hotels.json --bookings bookings.json");
                throw new Exception("Error: Invalid app arguments.");
            }
        }

        public static void ValidateFiles(this string[] args)
        {
            var hotelFilePath = args[1];
            var bookingFilePath = args[3];

            if (!File.Exists(hotelFilePath) || !File.Exists(bookingFilePath))
            {
                Console.WriteLine("Error: One or both of the provided file paths do not exist.");
                throw new Exception("Error: Invalid app file paths.");
            }
        }
    }
}
