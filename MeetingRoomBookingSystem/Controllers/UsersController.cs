using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using MeetingRoomBookingSystem.Resources;
using MeetingRoomBookingSystem.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRBS.Core.Models;
using MRBS.Services;
using MRBS.Services.Interfaces;
using SQLitePCL;
using System.Text;


namespace MeetingRoomBookingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this._mapper = mapper;
            this._userService = userService;
        }

        // GET: api/Companies
        [HttpGet("getAllUsers")]
        public async Task<ActionResult<IEnumerable<UserResource>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            var userResources = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

            return Ok(userResources);
        }

        // GET: api/Companies/5
        [HttpGet("getUserById")]
        public async Task<ActionResult<UserResource>> GetUserById(int id)
        {
            var user = await _userService.GetUsersById(id);
            var userResource = _mapper.Map<User, UserResource>(user);

            return Ok(userResource);
        }


        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("editUser")]
        public async Task<ActionResult<UserResource>> UpdateUser(int id, [FromBody] SaveUserResource saveUserResource)
        {
            var validator = new SaveUserResourceValidator();
            var validationResult = await validator.ValidateAsync(saveUserResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var userToBeUpdated = await _userService.GetUsersById(id);

            if (userToBeUpdated == null)
                return NotFound();

            var user = _mapper.Map<SaveUserResource, User>(saveUserResource);

            await _userService.UpdateUser(userToBeUpdated, user);

            var updatedUser = await _userService.GetUsersById(id);

            var updatedUserResource = _mapper.Map<User, UserResource>(updatedUser);

            return Ok(updatedUserResource);
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       /* [HttpPost("")]
        public async Task<ActionResult<UserResource>> CreateUser([FromBody] SaveUserResource saveUserResource)
        {
            var validator = new SaveUserResourceValidator();
            var validationResult = await validator.ValidateAsync(saveUserResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // this needs refining, but for demo it is ok

            var userToCreate = _mapper.Map<SaveUserResource, User>(saveUserResource);

            var newUser = await _userService.CreateUser(userToCreate);

            var user = await _userService.GetUsersById(newUser.Id);

            var userResource = _mapper.Map<User, UserResource>(user);

            return Ok(userResource);
        }*/

        // DELETE: api/Companies/5
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUsersById(id);

            await _userService.DeleteUser(user);

            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(SaveUserCredentialResource request)
        {
            if (await _userService.UserExistsAsync(request.Email))
            {
                return BadRequest("User already exists.");
            }
            if (await _userService.PhoneNumberExistsAsync(request.phoneNumber))
            {
                return BadRequest("This phone number has already been taken.");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

            var user = _mapper.Map<SaveUserCredentialResource, User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.VerificationToken = CreateRandomToken();

            await _userService.CreateUser(user);

            var userResource = _mapper.Map<User, UserResource>(user);

            return Ok(userResource);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            if(user == null)
            {
                return BadRequest("Wrong email or password.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong email or password.");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("Not Verified!");
            }
            return Ok(new { Message = $"Welcome back, {user.Name}!", UserName = user.Name , userRole = user.Role });
            //return Ok($"Welcome back, {user.Name}! :)");
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _userService.GetUserByTokenAsync(token);
            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            user.VerifiedAt = DateTime.Now;
            user.VerificationToken = null; // this line was added to remove the "maybe" conflict of verification
            await _userService.SaveChangesAsync();

            return Ok($"User Verified!");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("User not found! ");
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _userService.SaveChangesAsync();

            return Ok($"You may now reset your password!");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userService.GetUserByPasswordResetTokenAsync(request.Token);
            if (user == null || user.ResetTokenExpires<DateTime.Now)
            {
                return BadRequest("Invalid token.");
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _userService.SaveChangesAsync();

            return Ok($"User Verified!");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash,  byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(8));
        }

    }
}

