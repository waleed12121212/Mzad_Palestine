using System;

namespace Mzad_Palestine_Core.DTOs
{
    public class UserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Bio { get; set; }
    }

    public class UserDetailsDto : UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class ChangeUserRoleDto
    {
        public string NewRole { get; set; }
    }
}
