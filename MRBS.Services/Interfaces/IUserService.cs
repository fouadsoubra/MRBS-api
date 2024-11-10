using Microsoft.EntityFrameworkCore;
using MRBS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUsersById(int id);
        Task<User> CreateUser(User newUser);
        Task<bool> UserExistsAsync(string email);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByTokenAsync(string token);
        Task<User> GetUserByPasswordResetTokenAsync(string token);
        Task UpdateUser(User UserToBeUpdated, User user);
        Task SaveChangesAsync();
        Task DeleteUser(User user);

    }
}
