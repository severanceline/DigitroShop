using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Common.HashPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Queries.LoginUser
{
    public interface ILoginUserService
    {
        ResultDto<UserLoginResultDto> Execute(UserLoginRequest request);
    }
    public class LoginUserService: ILoginUserService
    {
        private readonly IDataBaseContext _context;
        public LoginUserService(IDataBaseContext context)
        {
            _context = context;      
        }
        public ResultDto<UserLoginResultDto> Execute(UserLoginRequest request)
        {
            var user = _context.Users
                .Include(p => p.UserInRoles)
                .ThenInclude(p => p.role)
                .Where(p => p.Email.Equals(request.Email)
            && p.IsActive == true)
            .FirstOrDefault();

            if (user == null)
            {
                return new ResultDto<UserLoginResultDto>
                {
                    Data = new UserLoginResultDto
                    {
                        UserId = 0,
                        FullName = ""
                    },
                    IsSuccess = false,
                    Message = "با این ایمیل ثبت نامی انجام نشده."
                };
            }
            else
            {
                string OldPassword = user.Password;
                string NewPassword = request.Password;
                bool IsPasswordCurrect = HashPassword.Verify(NewPassword, OldPassword);
                if(IsPasswordCurrect)
                {
                    List<string> roles = new List<string>();
                    foreach (var item in user.UserInRoles)
                    {
                        roles.Add(item.role.Name);
                    }

                    return new ResultDto<UserLoginResultDto>
                    {
                        Data = new UserLoginResultDto
                        {
                            UserId = user.Id,
                            FullName = user.FullName,
                            Roles = roles
                        },
                        IsSuccess = true,
                        Message = "ورود با موقیت انجام شد."
                    };
                }
                else
                {
                    return new ResultDto<UserLoginResultDto>
                    {
                        Data = new UserLoginResultDto
                        {
                            UserId = 0,
                            FullName = ""
                        },
                        IsSuccess = false,
                        Message = "رمز عبور اشتباه است."
                    };
                }
            }
        }
    }
    public class UserLoginResultDto
    {
        public long  UserId { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }

    }
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
