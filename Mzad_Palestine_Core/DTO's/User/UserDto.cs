using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mzad_Palestine_Core.Enums;

namespace Mzad_Palestine_Core.DTO_s.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }
        public bool IsVerified { get; set; }
        public int ReputationScore { get; set; }
    }
}
