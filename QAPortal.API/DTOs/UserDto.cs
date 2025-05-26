using System.ComponentModel.DataAnnotations;

namespace QAPortal.API.DTOs
{
    public class UserDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
} 