using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models
{
    public class UsedPassword

    {
        public UsedPassword()
        {
            CreatedDate = DateTimeOffset.Now;
        }
        
        [Key, Column(Order = 0)]
        public string HashPassword { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        
        [Key, Column(Order = 1)]
        public string UserID { get; set; }
        public virtual ApplicationUser AppUser { get; set; }

    }
}
