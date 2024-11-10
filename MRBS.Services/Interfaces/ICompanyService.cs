using MRBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> GetAllCompanies();
        Task<Company> GetCompanyById(int id);
        Task<Company> CreateCompany(Company newCompany);
        Task<IEnumerable<Company>> GetCompanyByUserId(int userId);
        Task<bool> CompanyExistsAsync(string email);
        Task<bool> CompanyNameExistsAsync(string name);
        Task UpdateCompany(Company companyToBeUpdated, Company company);
        Task DeleteCompany(Company company);
    }
}
