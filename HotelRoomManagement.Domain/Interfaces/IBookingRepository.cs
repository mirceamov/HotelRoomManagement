using HotelRoomManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManagement.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllBookingsAsync();

        Task<List<Booking>> GetHotelBookingsAsync(string hotelId);
    }
}
