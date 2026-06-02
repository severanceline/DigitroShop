using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Commands.EditUser
{
    public interface IEditUserService
    {
        ResultDto Execute(RequestEditUserDto request); 
    }
    public class EditUserService : IEditUserService
    {
        private readonly IDataBaseContext _context;
        public EditUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestEditUserDto request)
        {
            var user = _context.Users.Where(p => p.Id == request.Id).FirstOrDefault();
            if(user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد."
                };
            }
            else
            {
                user.Email = request.Email;
                user.FullName = request.FullName;
                _context.SaveChanges();
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "کاربر با موفقیت ویرایش شد"
                };
            }
        }
    }
    public class RequestEditUserDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
