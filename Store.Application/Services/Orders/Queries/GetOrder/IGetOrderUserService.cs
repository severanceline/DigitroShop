using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Queries.GetOrder
{
    public interface IGetOrderUserService
    {
        ResultDto<List<ResultGetOrderUserDto>> Execute(long UserId);
    }
    public class GetOrderUserService : IGetOrderUserService
    {
        private readonly IDataBaseContext _context;
        public GetOrderUserService (IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<ResultGetOrderUserDto>> Execute(long UserId)
        {
            var orders = _context.Orders
                .Include(r => r.RequestPay)
                .Include(o => o.OrderDetails)
                .ThenInclude(p => p.Product)
                .Where(o => o.UserId == UserId)
                .Select(p => new ResultGetOrderUserDto()
                {
                    OrderId = p.Id,
                    RequestPayId = p.RequestPayId,
                    OrderState = p.OrderState,
                    Price = p.RequestPay.Amount,
                    InsertTime = p.InsertTime,
                    ProductDetail = p.OrderDetails.Select(d => new ResultOrderDetailUserDto()
                    {
                        ProductId = d.ProductId,
                        ProductName = d.Product.Name,
                        ProductPrice = d.Price,
                        Count = d.Count,
                    }).ToList()
                }).ToList();
            return new ResultDto<List<ResultGetOrderUserDto>>
            {
                Data = orders,  
                IsSuccess = true,
            };
        }
    }
    public class ResultGetOrderUserDto
    {
        public long OrderId { get; set; }
        public long RequestPayId { get; set; }
        public State OrderState { get; set; }
        public int Price { get; set; }
        public DateTime InsertTime { get; set; }
        public List<ResultOrderDetailUserDto> ProductDetail { get; set; }
    }
    public class ResultOrderDetailUserDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int Count { get; set; }
    }
}
