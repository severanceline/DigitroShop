using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Queries.GetRequestPayForUser
{
    public interface IGetRequestPayForUser
    {
        ResultDto<ResultGetRequestPayForUserDto> Execute(long requestPayId);
    }
    public class GetRequestPayForUser : IGetRequestPayForUser
    {
        private readonly IDataBaseContext _context;
        public GetRequestPayForUser(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultGetRequestPayForUserDto> Execute(long requestPayId)
        {
            var requestPay = _context.RequestPays.Find(requestPayId);
            if (requestPay == null)
            {
                return new ResultDto<ResultGetRequestPayForUserDto>
                {
                    Data = new ResultGetRequestPayForUserDto(),
                    IsSuccess =false,
                };
            }
            return new ResultDto<ResultGetRequestPayForUserDto>
            {
                Data = new ResultGetRequestPayForUserDto()
                {
                    Amount = requestPay.Amount,
                    PayTime = requestPay.PayTime,
                    RefId = requestPay.RefId,
                    Athority = requestPay.Athority,
                    Id = requestPay.Id,
                },
                IsSuccess = true,
            };
        }
    }
    public class ResultGetRequestPayForUserDto
    {
        public int Amount { get; set; }
        public DateTime? PayTime { get; set; }
        public string  Athority { get; set; }
        public string  RefId { get; set; }
        public long  Id { get; set; }
    }
}
