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
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Company> CreateCompany(Company newCompany)
        {
            await _unitOfWork.Companies.AddAsync(newCompany);
            await _unitOfWork.CommitAsync();
            return newCompany;
        }

        public async Task DeleteCompany(Company company)
        {
            _unitOfWork.Companies.Remove(company);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await _unitOfWork.Companies
                .GetAllCompaniesAsync();
        }

        public async Task<Company> GetCompanyById(int id)
        {
            return await _unitOfWork.Companies
                .GetCompaniesByIdAsync(id);
        }

        public async Task<IEnumerable<Company>> GetCompanyByUserId(int userId)
        {
            return await _unitOfWork.Companies
                .GetCompanyByUserIdAsync(userId);
        }

        public async Task<bool> CompanyExistsAsync(string email)
        {
            return await _unitOfWork.CompanyExistsAsync(email);
        }

        public async Task<bool> CompanyNameExistsAsync(string name)
        {
            return await _unitOfWork.CompanyNameExistsAsync(name);
        }

        public async Task UpdateCompany(Company companyToBeUpdated, Company company)
        {
            companyToBeUpdated.Name = company.Name;
            companyToBeUpdated.EmailAddress = company.EmailAddress;
            companyToBeUpdated.Description = company.Description;
            companyToBeUpdated.Active = company.Active;
            companyToBeUpdated.Users = company.Users;

            await _unitOfWork.CommitAsync();
        }

    }
}
