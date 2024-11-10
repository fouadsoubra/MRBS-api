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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MrbsContext _context;
        private CompanyRepository _CompanyRepository;
        private ReservationRepository _ReservationRepository;
        private RoomRepository _RoomRepository;
        private UserRepository _UserRepository;

        public UnitOfWork(MrbsContext context)
        {
            this._context = context;
        }

        public ICompanyRepository Companies => _CompanyRepository = _CompanyRepository ?? new CompanyRepository(_context);
        public IReservationRepository Reservations => _ReservationRepository = _ReservationRepository ?? new ReservationRepository(_context);
        public IRoomRepository Rooms => _RoomRepository = _RoomRepository ?? new RoomRepository(_context);
        public IUserRepository Users => _UserRepository = _UserRepository ?? new UserRepository(_context);



        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> CompanyExistsAsync(string email)
        {
            return await _context.Companies.AnyAsync(u => u.EmailAddress == email);
        }

        public async Task<bool> RoomExistsAsync(string name)
        {
            return await _context.Rooms.AnyAsync(u => u.Name == name);
        }

        public async Task<bool> CompanyNameExistsAsync(string name)
        {
            return await _context.Companies.AnyAsync(u => u.Name == name);
        }


        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
