using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DogsIRL_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DogsIRL_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailSender _email;
        private IConfiguration _configuration;
        private LinkGenerator _linkGenerator;
        private IHttpContextAccessor _httpContextAccessor;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailSender email,  SignInManager<ApplicationUser> signIn, IConfiguration configuration, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signIn;
            _email = email;
            _configuration = configuration;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
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


        [HttpPost("register")]
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
            string confirmationUrl = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext.Request.HttpContext, "email-confirmation", "Account", pathBase: "/api");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var builder = new UriBuilder(confirmationUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["email"] = user.Email;
            query["token"] = token;
            builder.Query = query.ToString();
            string url = builder.ToString();
            await _email.SendEmailAsync(user.Email,
               "Dogs IRL Email Confirmation", $"Welcome to Dogs IRL! Please confirm your account by clicking <a href=" + url + ">here</a>");
        }

        [HttpGet("email-confirmation")]
        public async Task<string> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "Error";

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? $"Email for {user.UserName} confirmed!" : "Error during email validation.";
        }

        [HttpPost("forgot-password")]
        public async Task<bool> ForgotPassword(EmailInput input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return false;
            }

            string resetCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, "reset-password", "Account", new { userEmail = user.Email, code = resetCode }, pathBase: "/api");
            await _email.SendEmailAsync(user.Email, "Reset Password", $"A request was made to reset your password. To do so, click <a href={callbackUrl}>here</a>. If you did not make this request, ignore this message. If you are receiving multiple messages about resetting your password that you did not request, contact the DogsIRL team at help@dogs-irl.com");
            return true;
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword()
        {
            return View();
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