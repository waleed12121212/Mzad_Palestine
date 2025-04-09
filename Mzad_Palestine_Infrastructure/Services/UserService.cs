using Microsoft.AspNetCore.Identity;
using Mzad_Palestine_Core.DTO_s.User;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces.Services;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Username ,
                Email = registerDto.Email ,
                Phone = registerDto.Phone ,
                Role = UserRole.Buyer ,  // Changed from User to Regular
                IsVerified = false ,
                ReputationScore = 0 ,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user , registerDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", " , result.Errors.Select(e => e.Description)));
            }

            return new UserDto
            {
                Id = user.Id ,
                Username = user.UserName ,
                Email = user.Email ,
                Phone = user.Phone ,
                Role = user.Role ,
                IsVerified = user.IsVerified ,
                ReputationScore = user.ReputationScore
            };
        }

        public async Task<UserDto> LoginUserAsync(LoginUserDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }

            var result = await _signInManager.PasswordSignInAsync(user , loginDto.Password , false , false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid email or password");
            }

            return new UserDto
            {
                Id = user.Id ,
                Username = user.UserName ,
                Email = user.Email ,
                Phone = user.Phone ,
                Role = user.Role ,
                IsVerified = user.IsVerified ,
                ReputationScore = user.ReputationScore
            };
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return new UserDto
            {
                Id = user.Id ,
                Username = user.UserName ,
                Email = user.Email ,
                Phone = user.Phone ,
                Role = user.Role ,
                IsVerified = user.IsVerified ,
                ReputationScore = user.ReputationScore
            };
        }

        public Task<IEnumerable<UserDto>> GetAllUsersAsync( )
        {
            throw new NotImplementedException();
        }
    }
}
