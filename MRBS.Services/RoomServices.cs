using MRBS.Core.Interfaces;
using MRBS.Core.Models;
using MRBS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Room> CreateRoom(Room newRoom)
        {
            await _unitOfWork.Rooms.AddAsync(newRoom);
            await _unitOfWork.CommitAsync();
            return newRoom;
        }

        public async Task DeleteRoom(Room room)
        {
            _unitOfWork.Rooms.Remove(room);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _unitOfWork.Rooms
                .GetAllRoomsAsync();
        }

        public async Task<Room> GetRoomsById(int id)
        {
            return await _unitOfWork.Rooms
                .GetRoomsByIdAsync(id);
        }

        public async Task<bool> RoomExistsAsync(string name)
        {
            return await _unitOfWork.RoomExistsAsync(name);
        }


        public async Task UpdateRoom(Room roomToBeUpdated, Room room)
        {
            roomToBeUpdated.Capacity = room.Capacity;
            roomToBeUpdated.RoomDescription = room.RoomDescription;
            roomToBeUpdated.Name = room.Name;
            roomToBeUpdated.Location = room.Location;

            await _unitOfWork.CommitAsync();
        }
    }
}
