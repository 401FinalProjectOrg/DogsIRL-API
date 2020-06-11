using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogsIRL_API.Data;
using DogsIRL_API.Models;
using DogsIRL_API.Models.Interfaces;
using DogsIRL_API.Models.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DogsIRL_API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup()
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddControllers();
            services.AddTransient<IPetCardsManager, PetCardsService>();
            services.AddTransient<IEmailSender, EmailSender>();

            // Install-package Microsoft.EntityFrameworkCore.SqlServer
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddDbContext<AccountDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("UserConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<AccountDbContext>();

            string key = Configuration["AuthKey"]; //this should be same which is used while creating token      
            var issuer = "https://dogsirl-api.azurewebsites.net";  //this should be same which is used while creating token  
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                         options.TokenValidationParameters = new TokenValidationParameters
                          {
                                  ValidateIssuer = true,
                                  ValidateAudience = true,
                                  ValidateIssuerSigningKey = true,
                                  ValidIssuer = issuer,
                                  ValidAudience = issuer,
                                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                                  // check the userid in the claims of the token
                          };

                          options.Events = new JwtBearerEvents
                          {
                              OnAuthenticationFailed = context =>
                              {
                                  if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                  {
                                      context.Response.Headers.Add("Token-Expired", "true");
                                  }
                                  return Task.CompletedTask;
                              }
                          };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
