using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models
{
    public class RegisterResponse
    {
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
