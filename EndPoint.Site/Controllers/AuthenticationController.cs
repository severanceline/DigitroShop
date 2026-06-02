using EndPoint.Site.Models.ViewModels.AuthenticationViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore.Internal;
using Store.Application.Services.Users.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.RegisterUser.Validators;
using Store.Application.Services.Users.Queries.LoginUser;
using Store.Application.Services.Users.Queries.LoginUser.Validators;
using Store.Common.Dto;
using Store.Common.Roles;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EndPoint.Site.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IRegisterUserService _registerUserService;
        private readonly ILoginUserService _loginUserService;

        public AuthenticationController(IRegisterUserService registerUserService,
             ILoginUserService loginUserService)
        {
            _registerUserService = registerUserService;
            _loginUserService = loginUserService;
        }

        [HttpGet]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(UserLoginRequest request)
        {
            var validator = new UserLoginRequestValidator();
            var result = await validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                string errorMessage = string.Join(" <br> ", result.Errors.Select(e => e.ErrorMessage));

                return Json(new ResultDto { IsSuccess = false, Message = errorMessage });
            }
            var resultLogin = _loginUserService.Execute(request);
            if(resultLogin.IsSuccess == true)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,resultLogin.Data.UserId.ToString()),
                    new Claim(ClaimTypes.Email, request.Email),
                    new Claim(ClaimTypes.Name, resultLogin.Data.FullName),
                };

                foreach(var item in resultLogin.Data.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(5),
                };
                HttpContext.SignInAsync(principal, properties);
            }
            return Json(resultLogin);
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(RequestRegisterUserDto request)
        {
            request.Roles = new List<RolesInRegisterUserDto>()
            {
                new RolesInRegisterUserDto { Id = UserRoles.CustomerNumber }
            };
            var validator = new RequestRegisterUserDtoValidator();
            var result = await validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                string errorMessage = string.Join(" <br> ", result.Errors.Select(e => e.ErrorMessage));

                return Json(new ResultDto { IsSuccess = false, Message = errorMessage });
            }
            var signeupResult = _registerUserService.Execute(request);

            if (signeupResult.IsSuccess == true)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                    new Claim(ClaimTypes.Email, request.Email),
                    new Claim(ClaimTypes.Name, request.FullName),
                    new Claim(ClaimTypes.Role, UserRoles.Customer),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(5),
                };

                HttpContext.SignInAsync(principal, properties);
            }
            return Json(signeupResult);
        }
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
