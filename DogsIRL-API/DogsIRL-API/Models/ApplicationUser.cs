using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DogsIRL_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateCreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public static class ApplicationRoles
    {
        public const string Member = "Member";
        public const string Admin = "Admin";
    }
}
