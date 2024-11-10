using MRBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository Companies { get; }
        IReservationRepository Reservations { get; }
        IRoomRepository Rooms { get; }
        IUserRepository Users { get; }
        Task<bool> UserExistsAsync(string email);
        Task<bool> CompanyExistsAsync(string email);
        Task<bool> RoomExistsAsync(string name);
        Task<bool> CompanyNameExistsAsync(string name);
        Task<bool> PhoneNumberExistsAsync(string email);
        Task<int> CommitAsync();
    }
}
