using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using Store.Domain.Entities.Carts;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finances.Queries.GetAllRequestPayForAdmin
{
    public interface IGetAllRequestPayForAdminSevice
    {
        ResultDto<ResultGetAllRequestPayDto> Execute(int page, int pageSize);
    }
    public class GetAllRequestPayForAdminSevice : IGetAllRequestPayForAdminSevice
    {
        private readonly IDataBaseContext _context;
        public GetAllRequestPayForAdminSevice(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultGetAllRequestPayDto> Execute(int page, int pageSize)
        {
            int totalRows;
            var requestPays = _context.RequestPays
                .Include(p => p.User).ToPaged(page, pageSize, out totalRows)
                .Select(p => new RequestPayDetailDto()
                {
                    UserId = p.UserId,
                    UserName = p.User.FullName,
                    CartId = p.CartId,
                    Amount = p.Amount,
                    IsPayed = p.IsPayed,
                    PayTime = p.PayTime,
                    Athority = p.Athority,
                    RefId = p.RefId,
                }).ToList();
            return new ResultDto<ResultGetAllRequestPayDto>
            {
                Data = new ResultGetAllRequestPayDto()
                {
                    TotalRows = totalRows,
                    RequestPayDetails = requestPays
                },
                IsSuccess = true,
            };
        }
    }
    public class ResultGetAllRequestPayDto
    {
        public int TotalRows { get; set; }
        public List<RequestPayDetailDto> RequestPayDetails { get; set; }
    }
    public class RequestPayDetailDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long CartId { get; set; }
        public int Amount { get; set; }
        public bool IsPayed { get; set; }
        public DateTime? PayTime { get; set; }
        public string Athority { get; set; } = "";
        public string RefId { get; set; } = "";
    }
}
