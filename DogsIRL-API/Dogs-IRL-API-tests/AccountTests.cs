using DogsIRL_API.Controllers;
using DogsIRL_API.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net.Http;
using System.Web.Http;
using Xunit;

namespace Dogs_IRL_API_tests
{
    public class AccountTests
    {
        HttpConfiguration _config;

        public AccountTests()
        {
            _config = new HttpConfiguration();
            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            _config.Routes.MapHttpRoute(name: "Default", routeTemplate: "api/{controller}/{action}/{id}", defaults: new { id = RouteParameter.Optional });
        }

        /// <summary>
        /// Tests that going to the login route executes the SignIn method
        /// </summary>
        [Fact]
        public void AccountControllerSignInIsCorrect()
        {
            string testRoute = "https://localhost:44317/api/account/login/";
            SignInInput testInput = new SignInInput
            {
                Username = "Test",
                Password = "Test123!"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, testRoute);

            RouteTester routeTester = new RouteTester(_config, request);

            Assert.Equal(typeof(AccountController), routeTester.GetControllerType());
            Assert.Equal(ReflectionHelpers.GetMethodName((AccountController p) => p.SignIn(testInput)), routeTester.GetActionName());
        }

        [Fact]
        public void TestCanCreateAccount()
        {
            RegisterInput testInput = new RegisterInput
            {
                Username = "testuser",
                Email = "test@test.com",
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            };
            
        }
    }
}
