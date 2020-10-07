using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
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
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DogsIRL_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
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
        public async Task<IActionResult> SignIn(SignInInput signInInput)
        {
            var result = await _signInManager.PasswordSignInAsync(signInInput.Username, signInInput.Password, isPersistent: false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInInput.Username);
                var identityRoles = await _userManager.GetRolesAsync(user);
                var jwtToken = CreateToken(user, identityRoles.ToList());
                return Ok(new 
                {
                    username = user.UserName,
                    jwt = new JwtSecurityTokenHandler().WriteToken(jwtToken), 
                    expiration = jwtToken.ValidTo
                });
            }
            return BadRequest("Invalid login attempt");
        }

        [HttpPost("logout")]
        public async Task<JsonResult> Logout(string username)
        {
            await _signInManager.SignOutAsync();
            JsonResult result = new JsonResult($"{username} successfully logged out.");
            return result;
        }


        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount(RegisterInput registerInput)
        {
            var user = new ApplicationUser
            {
                UserName = registerInput.Username,
                Email = registerInput.Email
            };
            var result = await _userManager.CreateAsync(user, registerInput.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            else
            {
                SendAccountConfirmationEmail(user);
                return Ok();
            }
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
            string confirmationUrl = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext.Response.HttpContext, "email-confirmation", "Account", pathBase: "/api");
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
        public async Task ForgotPassword(EmailInput input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                return;
            }

            string resetCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, "reset-password", "Forms", new { userEmail = user.Email, code = resetCode });
            await _email.SendEmailAsync(user.Email, "Reset Password", $"A request was made to reset your password. To do so, click <a href={callbackUrl}>here</a>. If you did not make this request, ignore this message. If you are receiving multiple messages about resetting your password that you did not request, contact the DogsIRL team at help@dogs-irl.com");
        }

        private JwtSecurityToken CreateToken(ApplicationUser user, List<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id),
                new Claim("UserEmail", user.Email),
                new Claim("UserName", user.UserName)
            };
            foreach(var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = AuthenticateToken(authClaims);

            return token;
        }

        private JwtSecurityToken AuthenticateToken(List<Claim> claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthKey"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["AuthIssuer"],
                expires: DateTime.Now.AddDays(1),
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }

    }
}