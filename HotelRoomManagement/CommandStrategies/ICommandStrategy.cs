using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelRoomManagement.CommadStrategies
{
    public interface ICommandStrategy
    {
        public string CommandName { get; }

        public Task ExecuteAsync(string[] parameters);
    }
}
