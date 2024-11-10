using Microsoft.EntityFrameworkCore;
using MRBS.Core.Interfaces;
using MRBS.Core.Models;
using MRBS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MRBS.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> CreateUser(User newUser)
        {
            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            return newUser;
        }

        public async Task DeleteUser(User user)
        {
            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _unitOfWork.Users
                .GetAllUsersAsync();
        }

        public async Task<User> GetUsersById(int id)
        {
            return await _unitOfWork.Users
                .GetUsersByIdAsync(id);
        }


        public async Task UpdateUser(User userToBeUpdated, User user)
        {
            userToBeUpdated.Name = user.Name;
            userToBeUpdated.Email = user.Email;
          //  userToBeUpdated.DateOfBirth = user.DateOfBirth;
            userToBeUpdated.PhoneNumber = user.PhoneNumber;
            userToBeUpdated.Role = user.Role;
          //  userToBeUpdated.CompanyId = user.CompanyId;


            await _unitOfWork.CommitAsync();
        }


        public async Task<bool> UserExistsAsync(string email)
        {
            return await _unitOfWork.UserExistsAsync(email);
        }
        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return await _unitOfWork.PhoneNumberExistsAsync(phoneNumber);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.Users
                .SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByTokenAsync(string token)
        {
            return await _unitOfWork.Users
                .SingleOrDefaultAsync(u => u.VerificationToken == token);
        }
        public async Task SaveChangesAsync()
        {
            await _unitOfWork.CommitAsync(); 
        }
        public async Task<User> GetUserByPasswordResetTokenAsync(string token)
        {
            return await _unitOfWork.Users
                .SingleOrDefaultAsync(u => u.PasswordResetToken == token);
        }
    }
}
