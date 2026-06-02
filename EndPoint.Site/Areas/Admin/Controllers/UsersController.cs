using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Internal;
using Store.Application.Services.Users.Commands.ActiveUser;
using Store.Application.Services.Users.Commands.EditUser;
using Store.Application.Services.Users.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.RegisterUser.Validators;
using Store.Application.Services.Users.Commands.RemoveUser;
using Store.Application.Services.Users.Queries.GetRoles;
using Store.Application.Services.Users.Queries.GetUsers;
using Store.Common.Dto;
using Store.Common.Roles;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    //[Authorize(Policy = "Admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IGetUsersService _getUsersService;
        private readonly IGetRolesService _getRolesService;
        private readonly IRegisterUserService _registerUserService;
        private readonly IRemoveUserService _removeUserService;
        private readonly IAvtiveUserService _avtiveUserService;
        private readonly IEditUserService _editUserService;

        public UsersController(
             IGetUsersService getUsersService,
             IGetRolesService getRolesService,
             IRegisterUserService registerUserService,
             IRemoveUserService removeUserService,
             IAvtiveUserService avtiveUserService,
            IEditUserService editUserService)
        {
            _getUsersService = getUsersService;
            _getRolesService = getRolesService;
            _registerUserService = registerUserService;
            _removeUserService = removeUserService;
            _avtiveUserService = avtiveUserService;
            _editUserService = editUserService;
        }
        public IActionResult Index(string SearchKey,int Page=1 ,int PageSize = 20)
        {
            return View(_getUsersService.Execute(new RequestGetUserDto{Page=Page , SearchKey=SearchKey , PageSize=PageSize}));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_getRolesService.Execute().Data, "Id" , "Name");
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestRegisterUserDto request)
        {
            var validator = new RequestRegisterUserDtoValidator();
            var result = await validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                string errorMessage = string.Join(" <br> ", result.Errors.Select(e => e.ErrorMessage));

                return Json(new ResultDto { IsSuccess = false, Message = errorMessage });
            }
            var serviceResult = _registerUserService.Execute(request);
            return Json(serviceResult);
        }
        [HttpPost]
        public IActionResult Delete(string email, long userId)
        {
            var request = new RequestRemoveUserDto
            {
                UserId = userId,
                Email = email
            };
            return Json(_removeUserService.Execute(request));
        }

        [HttpPost]
        public IActionResult UserSatusChange(long userId)
        {

            return Json(_avtiveUserService.Execute(userId));
        }

        [HttpPost]
        public IActionResult EditeUser(long userId, string Email, string FullName)
        {
            var request = new RequestEditUserDto
            {
                Email = Email,
                FullName = FullName,
                Id = userId
            };
            return Json(_editUserService.Execute(request));
        }
    }
}