using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DogsIRL_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DogsIRL_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController
    {

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _email;
        private IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender email,  SignInManager<ApplicationUser> signIn, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signIn;
            _email = email;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<string> SignIn(SignInInput signInInput)
        {
            var result = await _signInManager.PasswordSignInAsync(signInInput.Username, signInInput.Password, isPersistent: false, false);

            if (result.Succeeded)
            {
                string JwtToken = GetToken(signInInput.Username);
                return JwtToken;
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


        [HttpPost("Register")]
        public async Task CreateAccount(RegisterInput registerInput)
        {
            var user = new ApplicationUser
            {
                UserName = registerInput.Username,
                Email = registerInput.Email
            };
            var result = await _userManager.CreateAsync(user, registerInput.Password);



            if (!result.Succeeded)
            {
                return;
            }
            
            SendAccountConfirmationEmail(user);
        }

        private protected async void SendWelcomeEmail(ApplicationUser user)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<h1> Welcome, {user.UserName}, to DogsIRL! </h1>");
            sb.AppendLine("<p>To get started: enter the app and create a profile card for your pup by tapping the create button!</p>");
            await _email.SendEmailAsync($"{user.Email}", "Dogs IRL Registration Complete", sb.ToString());
        }

        private protected async void SendAccountConfirmationEmail(ApplicationUser user)
        {
            string confirmationUrl = @"https://dogsirl-api.azurewebsites.net/api/account/email-confirmation";
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var builder = new UriBuilder(confirmationUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["email"] = user.Email;
            query["token"] = token;
            builder.Query = query.ToString();
            string url = builder.ToString();
            await _email.SendEmailAsync(user.Email,
               "Dogs IRL Email Confirmation", $"Welcome to Dogs IRL! Please confirm your account by clicking <a href={url}>here</a>");
        }

        [HttpGet("email-confirmation")]
        public async Task<string> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "Error";

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? $"{nameof(ConfirmEmail)} confirmed!" : "Error during email validation.";
        }

        [HttpPost("Logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        // Code for JWT token creation taken from https://www.c-sharpcorner.com/article/asp-net-core-web-api-creating-and-validating-jwt-json-web-token/ 5/20/2020
        private protected string GetToken(string username)
        {
            string key = _configuration["AuthKey"]; // Secret key
            var issuer = "https://dogsirl-api.azurewebsites.net";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("username", username));

            var token = new JwtSecurityToken(issuer,
                issuer, // audience same as issuer in our case
                permClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}