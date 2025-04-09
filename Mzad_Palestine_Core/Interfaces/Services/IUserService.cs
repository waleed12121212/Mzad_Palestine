using Mzad_Palestine_Core.DTO_s.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzad_Palestine_Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(RegisterUserDto dto);
        Task<UserDto> LoginUserAsync(LoginUserDto dto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync( );
        Task<UserDto?> GetUserByIdAsync(int id);
    }
}
