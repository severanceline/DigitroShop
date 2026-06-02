using Microsoft.AspNetCore.Identity;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Common.HashPassword;
using Store.Domain.Entities.Users;

namespace Store.Application.Services.Users.Commands.RegisterUser
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDataBaseContext _context;
        public RegisterUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<RegisterUserServiceDto> Execute(RequestRegisterUserDto request)
        {
            try
            {
                User user = new User()
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    Password = HashPassword.Execute(request.Password),
                    IsActive = true
                };

                List<UserInRole> userInRoles = new List<UserInRole>();
                foreach (var item in request.Roles)
                {
                    //برای هر کدوم از اون رول ها که تو لیست رولز هستند بیا و
                    //برو و اون آیدی ای که تو اون لیست رول ها هست رو بگرد تو 
                    //کانتکس قسمت رول و اون رو بریز تومتغیز رولز
                    var roles = _context.Roles.Find(item.Id);
                    //و یعد برو به پوشه ی  یوزر این رول در دیتا بیس اسن رو اضافه کن 
                    //با مشخصاتی که در حال حاضر با ریکوعستمون داریم
                    //اول یه لیستی درست میکنیم از یوزذ این رول و مقدار دهیش میکنیم 
                    //و بعدش اینجا به ازای رول هایی که دارد براش سطر تو جدول یوزر این رول ایجاد میکنیم
                    userInRoles.Add(new UserInRole
                    {
                        role = roles,
                        RoleId = roles.Id,
                        user = user,
                        UserId = user.Id
                    });
                    //حالا تازه میگیم یوزری که میخوای ثبت نام کنی 
                    //یوزراین رولزت یشه همون لیستس که ماساختیم
                }

                user.UserInRoles = userInRoles;
                _context.Users.Add(user);
                _context.SaveChanges();

                return new ResultDto<RegisterUserServiceDto>()
                {
                    Data = new RegisterUserServiceDto
                    {
                        UserId = user.Id
                    },
                    IsSuccess = true,
                    Message = "ثبت نام کاربر انجام شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<RegisterUserServiceDto>()
                {
                    Data = new RegisterUserServiceDto()
                    {
                        UserId = 0,
                    },
                    IsSuccess = false,
                    Message = "ثبت نام انجام نشد !"
                };
            }
        }
    }
}
