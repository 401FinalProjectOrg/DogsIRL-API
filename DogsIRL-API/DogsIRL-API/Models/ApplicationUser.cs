using System;
using Microsoft.AspNetCore.Identity;

namespace DogsIRL_API.Models
{
    public class ApplicationUser : IdentityUser
    {
    }

    public static class ApplicationRoles
    {
        public const string Member = "Member";
        public const string Admin = "Admin";
    }
}
