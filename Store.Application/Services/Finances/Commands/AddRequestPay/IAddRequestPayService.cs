using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Finances;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Commands.AddRequestPay
{
    public interface IAddRequestPayService
    {
        ResultDto<ResultRequestPayDto> Execute(int Amount,long UserId,long cartId);
    }
    public class AddRequestPayService : IAddRequestPayService
    {
        private readonly IDataBaseContext _context;
        public AddRequestPayService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultRequestPayDto> Execute(int Amount, long UserId, long cartId)
        {
            var user = _context.Users.Find(UserId);
            var cart = _context.Carts.Find(cartId);
            RequestPay requestPay = new RequestPay()
            {
                Amount = Amount,
                Guid = Guid.NewGuid(),
                IsPayed = false,
                User = user,
                Cart = cart,

            };
            _context.RequestPays.Add(requestPay);
            _context.SaveChanges();

            return new ResultDto<ResultRequestPayDto>
            {
                Data = new ResultRequestPayDto()
                {
                    guid = requestPay.Guid,
                    Email = user.Email,
                },
                IsSuccess = true,
            };
        }
    }
    public class ResultRequestPayDto
    {
        public Guid guid { get; set; }
        public string Email { get; set; }
    } 
}
