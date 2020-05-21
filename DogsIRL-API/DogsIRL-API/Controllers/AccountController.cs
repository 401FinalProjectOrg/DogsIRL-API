using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DogsIRL_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DogsIRL_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController
    {

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _email;


        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender email,  SignInManager<ApplicationUser> signIn)
        {
            _userManager = userManager;
            _signInManager = signIn;
            _email = email;
        }

        [HttpPost("login")]
        public async Task<ApplicationUser> SignIn(SignInInput signInInput)
        {
            var result = await _signInManager.PasswordSignInAsync(signInInput.Username, signInInput.Password, isPersistent: false, false);
            
            
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInInput.Username);
                return user;
            }
            return null;
        }

        [HttpPost("logout")]
        public async Task<JsonResult> Logout(string username)
        {
            await _signInManager.SignOutAsync();
            JsonResult result = new JsonResult($"{username} successfully logged out.");
            return result;
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task CreateAccount(RegisterInput registerInput)
        {
            var user = new ApplicationUser
            {
                UserName = registerInput.Username,
                Email = registerInput.Email
            };
            var result = await _userManager.CreateAsync(user, registerInput.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                SendWelcomeEmail(user);
            }
        }

        private protected async void SendWelcomeEmail(ApplicationUser user)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<h1> Welcome, {user.UserName}, to DogsIRL! </h1>");
            sb.AppendLine("<p>To get started: enter the app and create a profile card for your pup by tapping the create button!</p>");
            await _email.SendEmailAsync($"{user.Email}", "Dogs IRL Registration Complete", sb.ToString());
        }
        [HttpPost("Logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        
    }
}