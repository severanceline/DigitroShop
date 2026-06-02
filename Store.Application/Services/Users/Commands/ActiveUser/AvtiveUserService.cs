using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;

namespace Store.Application.Services.Users.Commands.ActiveUser
{
    public class AvtiveUserService : IAvtiveUserService
    {
        private readonly IDataBaseContext _context;
        public AvtiveUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultActiveDto>  Execute(long UserId)
        {
            var user = _context.Users.Where(p => p.Id == UserId).FirstOrDefault();
            if (user == null)
            {
                return new ResultDto<ResultActiveDto>
                {
                    Data = new ResultActiveDto
                    {
                        UserStage = ""
                    },
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }
            else
            {
                user.IsActive = !user.IsActive;
                _context.SaveChanges();
                string userstate = user.IsActive == true ? "فعال" : "غیر فعال";
                return new ResultDto<ResultActiveDto>
                {
                    Data = new ResultActiveDto
                    {
                        UserStage = userstate,
                    },
                    IsSuccess = true,
                    Message = $"کاربر با موفقیت {userstate} شد!",
                };
            }
        }
    }
}
