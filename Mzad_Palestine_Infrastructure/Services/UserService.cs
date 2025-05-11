using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Mzad_Palestine_Core.DTO_s.User;
using Mzad_Palestine_Core.DTOs;
using Mzad_Palestine_Core.Models;
using Mzad_Palestine_Core.Enums;
using Mzad_Palestine_Core.Interfaces.Services;
using System.Security.Claims;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                Role = UserRole.Buyer,
                IsVerified = false,
                ReputationScore = 0,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "User");

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                IsVerified = user.IsVerified,
                ReputationScore = user.ReputationScore
            };
        }

        public async Task<UserDto> LoginUserAsync(LoginUserDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("البريد الإلكتروني أو كلمة المرور غير صحيحة");
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                throw new Exception("البريد الإلكتروني أو كلمة المرور غير صحيحة");
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                IsVerified = user.IsVerified,
                ReputationScore = user.ReputationScore
            };
        }

        public async Task<UserDetailsDto> GetUserByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                throw new Exception("المستخدم غير موجود");

            var roles = await _userManager.GetRolesAsync(user);
            
            return new UserDetailsDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.Phone,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Bio = user.Bio,
                ProfilePicture = user.ProfilePicture,
                Role = roles.FirstOrDefault() ?? "User",
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        public async Task<IEnumerable<UserDetailsDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<UserDetailsDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDetailsDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.Phone,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    Bio = user.Bio,
                    ProfilePicture = user.ProfilePicture,
                    Role = roles.FirstOrDefault() ?? "User",
                    CreatedAt = user.CreatedAt,
                    IsActive = user.IsActive
                });
            }

            return userDtos;
        }

        public async Task<UserDetailsDto> GetCurrentUserAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new Exception("المستخدم غير مسجل الدخول");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("المستخدم غير موجود");

            var roles = await _userManager.GetRolesAsync(user);
            
            return new UserDetailsDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.Phone,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                Bio = user.Bio,
                ProfilePicture = user.ProfilePicture,
                Role = roles.FirstOrDefault() ?? "User",
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        public async Task<UserDetailsDto> UpdateProfileAsync(int userId, UserProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("المستخدم غير موجود");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Phone = dto.PhoneNumber;
            user.Email = dto.Email;
            user.Address = dto.Address;
            user.DateOfBirth = dto.DateOfBirth;
            user.Bio = dto.Bio;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("فشل تحديث الملف الشخصي");

            return await GetUserByIdAsync(userId);
        }

        public async Task<string> UploadProfilePictureAsync(int userId, IFormFile file)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("المستخدم غير موجود");

            if (file == null || file.Length == 0)
                throw new Exception("لم يتم اختيار صورة");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("نوع الملف غير مدعوم. يرجى تحميل صورة بصيغة JPG أو PNG أو GIF");

            if (file.Length > 10485760)
                throw new Exception("حجم الملف كبير جداً. الحد الأقصى هو 10MB");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var uploadsFolder = Path.Combine(baseDirectory, "wwwroot", "uploads", "profile-pictures");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            if (!string.IsNullOrEmpty(user.ProfilePicture))
            {
                var oldFilePath = Path.Combine(baseDirectory, "wwwroot", user.ProfilePicture.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            user.ProfilePicture = $"/uploads/profile-pictures/{fileName}";
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("فشل تحديث الصورة الشخصية");

            return user.ProfilePicture;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangeUserRoleAsync(int userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            
            var result = await _userManager.AddToRoleAsync(user, newRole);
            return result.Succeeded;
        }
    }
}
