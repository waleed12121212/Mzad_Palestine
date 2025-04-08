using Mzad_Palestine_Core.DTO_s.User;
using Mzad_Palestine_Core.Interfaces.Services;
using Mzad_Palestine_Core.Interfaces;
using Mzad_Palestine_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository) => _repository = repository;

        public async Task<UserDto> RegisterUserAsync(RegisterUserDto dto)
        {
            var user = new User { Username = dto.Username , Email = dto.Email , Password = dto.Password , Phone = dto.Phone , Role = dto.Role };
            var created = await _repository.AddAsync(user);
            return new UserDto(created.Id , created.Username , created.Email , created.Phone , created.Role , created.IsVerified);
        }

        public async Task<UserDto?> LoginUserAsync(LoginUserDto dto)
        {
            var user = await _repository.GetByEmailAsync(dto.Email);
            if (user == null || user.Password != dto.Password) return null;
            return new UserDto(user.Id , user.Username , user.Email , user.Phone , user.Role , user.IsVerified);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync( )
        {
            var users = await _repository.GetAllAsync();
            var list = new List<UserDto>();
            foreach (var user in users)
                list.Add(new UserDto(user.Id , user.Username , user.Email , user.Phone , user.Role , user.IsVerified));
            return list;
        }
    }
}
