using MRBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Core.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company> GetCompaniesByIdAsync(int id);
        Task<IEnumerable<Company>> GetCompanyByUserIdAsync(int userId);
    }
}
