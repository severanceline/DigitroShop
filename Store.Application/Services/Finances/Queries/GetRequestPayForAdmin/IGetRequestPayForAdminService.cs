using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Queries.GetRequestPayForAdmin
{
    public interface IGetRequestPayForAdminService
    {
        ResultDto<ResultGetRequestPayForAdminDto> Execute(long requestPayId);
    }
    public class GetRequestPayForAdminService : IGetRequestPayForAdminService
    {
        private readonly IDataBaseContext _context;
        public GetRequestPayForAdminService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultGetRequestPayForAdminDto> Execute(long requestPayId)
        {
            var requestPay = _context.RequestPays.Find(requestPayId);
            if (requestPay == null)
            {
                return new ResultDto<ResultGetRequestPayForAdminDto>
                {
                    Data = new ResultGetRequestPayForAdminDto(),
                    IsSuccess = false,
                };
            }
            return new ResultDto<ResultGetRequestPayForAdminDto>
            {
                Data = new ResultGetRequestPayForAdminDto()
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
    public class ResultGetRequestPayForAdminDto
    {
        public int Amount { get; set; }
        public DateTime? PayTime { get; set; }
        public string Athority { get; set; }
        public string RefId { get; set; }
        public long Id { get; set; }
    }
}
