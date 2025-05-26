using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace QAPortal.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int Reputation { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}