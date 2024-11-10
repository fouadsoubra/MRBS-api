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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MrbsContext context)
            : base(context)
        { }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await MyDbContext.Users
        //        .Include(m => m.Company)  //to include information sbout company
                .ToListAsync();
        }

        public Task<User> GetUsersByIdAsync(int id)
        {
            return MyDbContext.Users
                .SingleOrDefaultAsync(a => a.Id == id);
        }


        private MrbsContext MyDbContext
        {
            get { return Context as MrbsContext; }
        }
        

    }
}
