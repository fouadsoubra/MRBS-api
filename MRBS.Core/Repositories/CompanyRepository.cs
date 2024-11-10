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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(MrbsContext context)
            : base(context)
        { }
        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await MyDbContext.Companies
                .ToListAsync();
        }

        public Task<Company> GetCompaniesByIdAsync(int id)
        {
            return MyDbContext.Companies
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Company>> GetCompanyByUserIdAsync(int userId)
        {
            return await MyDbContext.Companies
                .Where(m => m.Users.Any(u => u.Id == userId))
                .ToListAsync();
        }


        private MrbsContext MyDbContext
        {
            get { return Context as MrbsContext; }
        }
    }
}
