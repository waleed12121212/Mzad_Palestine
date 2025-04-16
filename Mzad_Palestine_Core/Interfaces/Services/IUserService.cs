using Mzad_Palestine_Core.DTO_s.User;
using Mzad_Palestine_Core.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(RegisterUserDto dto);
        Task<UserDto> LoginUserAsync(LoginUserDto dto);
        Task<IEnumerable<UserDetailsDto>> GetAllUsersAsync();
        Task<UserDetailsDto> GetUserByIdAsync(int id);
        Task<UserDetailsDto> GetCurrentUserAsync();
        Task<UserDetailsDto> UpdateProfileAsync(int userId, UserProfileDto dto);
        Task<string> UploadProfilePictureAsync(int userId, IFormFile file);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangeUserRoleAsync(int userId, string newRole);
    }
}
