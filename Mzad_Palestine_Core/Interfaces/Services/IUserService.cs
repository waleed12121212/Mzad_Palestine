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
        Task<UserDto> RegisterAsync(RegisterUserDto dto);
        Task<UserDto> LoginAsync(LoginUserDto dto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync( );
        Task<UserDto?> GetUserByIdAsync(int id);
    }
}
