using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Queries.UpdateRequstPay
{
    public interface IUpdateRequestPayService
    {
        ResultDto Execute(RequestUpdateRequestPayDto request);
    }
    public class UpdateRequestPayService : IUpdateRequestPayService
    {
        private readonly IDataBaseContext _context;
        public UpdateRequestPayService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestUpdateRequestPayDto request)
        {
            var requestPay = _context.RequestPays.Where(p => p.Guid == request.Guid).FirstOrDefault();
            if(requestPay == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                };
            }
            requestPay.RefId = request.RefId;
            requestPay.Athority = request.Authority;
            requestPay.IsPayed = true;
            requestPay.PayTime = DateTime.Now;
            return new ResultDto()
            {
                IsSuccess = true,
            };
        }
    }
    public class RequestUpdateRequestPayDto
    {
        public Guid Guid { get; set; }
        public string RefId { get; set; }
        public string Authority { get; set; }
    }
}
