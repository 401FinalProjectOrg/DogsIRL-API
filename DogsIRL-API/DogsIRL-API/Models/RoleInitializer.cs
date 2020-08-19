using DogsIRL_API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models
{
    public class RoleInitializer
    {
        private static readonly List<IdentityRole> Roles = new List<IdentityRole>
        {
            new IdentityRole{Name=ApplicationRoles.Admin, NormalizedName=ApplicationRoles.Admin.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString()}
        };
        public static void SeedData(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            using(var dbcontext = new AccountDbContext(serviceProvider.GetRequiredService<DbContextOptions<AccountDbContext>>()))
            {
                dbcontext.Database.EnsureCreated();
                AddRoles(dbcontext);
                SeedUsers(userManager, config);
            }
        }

        public static void AddRoles(AccountDbContext dbContext)
        {
            if (dbContext.Roles.Any()) return;
            foreach(var role in Roles)
            {
                dbContext.Roles.Add(role);
                dbContext.SaveChanges();
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            if(userManager.FindByEmailAsync( config["AdminEmail"]).Result == null)
            {
                ApplicationUser adminUser = new ApplicationUser
                {
                    UserName = config["AdminUsername"],
                    Email = config["AdminEmail"],
                };
                IdentityResult result = userManager.CreateAsync(adminUser, config["AdminPassword"]).Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, ApplicationRoles.Admin).Wait();
                }
            }
        }
    }
}
