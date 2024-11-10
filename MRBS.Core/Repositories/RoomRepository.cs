using Microsoft.EntityFrameworkCore;
using MRBS.Core.Interfaces;
using MRBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Core.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(MrbsContext context)
            : base(context)
        { }
        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await MyDbContext.Rooms
                .ToListAsync();
        }

        public Task<Room> GetRoomsByIdAsync(int id)
        {
            return MyDbContext.Rooms
                .SingleOrDefaultAsync(a => a.Id == id);
        }


        private MrbsContext MyDbContext
        {
            get { return Context as MrbsContext; }
        }
    }
}
