using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Queries.GetRequestPay
{
    public interface IGetRequestPayService
    {
        ResultDto<ResultGetRequestPayDto> Execute(Guid guid);
    }
    public class GetRequestPayService : IGetRequestPayService
    {
        private readonly IDataBaseContext _context;
        public GetRequestPayService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultGetRequestPayDto> Execute(Guid guid)
        {
            var requestPay = _context.RequestPays.Where(p => p.Guid == guid).FirstOrDefault();
            if(requestPay == null )
            {
                return new ResultDto<ResultGetRequestPayDto>
                {
                    Data = new ResultGetRequestPayDto(),
                    IsSuccess = false,
                };
            }
            return new ResultDto<ResultGetRequestPayDto>
            {
                Data = new ResultGetRequestPayDto()
                {
                    Amount = requestPay.Amount,
                    Id = requestPay.Id,
                },
                IsSuccess = true,
            };
        }
    }
    public class ResultGetRequestPayDto
    {
        public  int  Amount { get; set; }
        public long Id { get; set; }
    }
}
