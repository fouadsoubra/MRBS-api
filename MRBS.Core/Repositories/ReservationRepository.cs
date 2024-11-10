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
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(MrbsContext context)
            : base(context)
        { }
        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await MyDbContext.Reservations
                .ToListAsync();
        }

        public Task<Reservation> GetReservationsByIdAsync(int id)
        {
            return MyDbContext.Reservations
                .SingleOrDefaultAsync(a => a.Id == id);
        }


        private MrbsContext MyDbContext
        {
            get { return Context as MrbsContext; }
        }
    }
}
