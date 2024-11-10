using MRBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRooms();
        Task<Room> GetRoomsById(int id);
        Task<Room> CreateRoom(Room newRoom);
        Task<bool> RoomExistsAsync(string name);
        Task UpdateRoom(Room roomToBeUpdated, Room room);
        Task DeleteRoom(Room room);
    }
}
