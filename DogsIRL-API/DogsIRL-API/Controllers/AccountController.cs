using System;
using System.Text;
using System.Threading.Tasks;
using DogsIRL_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<string> SignIn(SignInInput signInInput)
        {
            var result = await _signInManager.PasswordSignInAsync(signInInput.Username, signInInput.Password, isPersistent: false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInInput.Username);
                return user.UserName;
            }
            return null;
        }

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
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<h1> Welcome to DogsIRL! </h1>");
                sb.AppendLine("<p> Create a profile card for your pup by tapping the create button! </p>");
                await _email.SendEmailAsync($"{user.Email}", "Registration Complete", sb.ToString());
            }
            
        }
        [HttpPost("Logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}