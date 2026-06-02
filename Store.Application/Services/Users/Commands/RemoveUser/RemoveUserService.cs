using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;

namespace Store.Application.Services.Users.Commands.RemoveUser
{
    public class RemoveUserService : IRemoveUserService
    {
        private readonly IDataBaseContext _context;
        public RemoveUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<RemoveUserDto> Execute(RequestRemoveUserDto request)
        {
            var user = _context.Users.Where(p => p.Id == request.UserId || p.Email == request.Email).FirstOrDefault();
            if (user == null)
            {
                return new ResultDto<RemoveUserDto>
                {
                    Data = new RemoveUserDto
                    {
                        UserId = 0
                    },
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }
            else
            {
                user.RemovedTime = DateTime.Now;
                user.IsRemoved = true;
                _context.SaveChanges();
                return new ResultDto<RemoveUserDto>
                {
                    Data = new RemoveUserDto
                    {
                        UserId = user.Id,
                    },
                    IsSuccess = true,
                    Message = "کاربر با موفقیت حذف شد"
                };
            }
        }
    }
}
