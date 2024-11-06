using HotelRoomManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManagement.Domain.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel> GetHotelAsync(string hotelId);
        Task<List<Hotel>> GetAllHotelsAsync();
    }
}
