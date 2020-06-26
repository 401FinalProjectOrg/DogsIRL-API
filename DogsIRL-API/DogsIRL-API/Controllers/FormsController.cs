using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogsIRL_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DogsIRL_API.Controllers
{
    [Route("[controller]")]
    public class FormsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        public FormsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("reset-password")] // figure out what the url looks like
        public IActionResult ResetPassword(string code, string userEmail)
        {
            ResetPasswordInput model = new ResetPasswordInput { Token = code, Email = userEmail };
            //model.Email = // how to get email from token?
            //model.Token = token;
            return View(model);
        }

        [HttpPost("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(ResetPasswordConfirm));
            }

            bool tokenIsValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", input.Token);
            if (!tokenIsValid)
            {
                return RedirectToAction(nameof(ResetPasswordConfirm));
            }

            bool inputtedPasswordIsSameAsCurrent = await _userManager.CheckPasswordAsync(user, input.Password);
            if (inputtedPasswordIsSameAsCurrent)
            {
                ModelState.AddModelError("Previous Password", "Cannot use the previous password");
                return View(input);
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, input.Token, input.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View(input);
            }
            return RedirectToAction(nameof(ResetPasswordConfirm));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
    }
}