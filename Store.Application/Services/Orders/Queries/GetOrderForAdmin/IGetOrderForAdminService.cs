using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Queries.GetOrderForAdmin
{
    public interface IGetOrderForAdminService
    {
        ResultDto<ResultGetOrderForAdminDto> Execute(State orderState,int Page, int PageSize);
    }
    public class GetOrderForAdminService : IGetOrderForAdminService
    {
        private readonly IDataBaseContext _context;
        public GetOrderForAdminService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultGetOrderForAdminDto> Execute(State orderState, int Page, int PageSize)
        {
            int totalRow = 0;
            var Orders = _context.Orders
                .Include(p => p.RequestPay)
                .Include(p => p.User)
                .Include(p => p.OrderDetails)
                .ThenInclude(p =>p.Product)
                .Where(p => p.OrderState == orderState).AsQueryable();

            var order = Orders.ToPaged(Page, PageSize, out totalRow);

            return new ResultDto<ResultGetOrderForAdminDto>
            {
                Data = new ResultGetOrderForAdminDto()
                {
                    TotalRow = totalRow,
                    OrderInfo = order.Select(p => new OrderInfoDto
                    {
                        Amount = p.RequestPay.Amount,
                        OrderId = p.Id,
                        UserName = p.User.FullName,
                        UserId = p.User.Id,
                        ProductCount = p.OrderDetails.Sum(od => od.Count),
                        RequestPayId = p.RequestPayId,
                        OrderState = p.OrderState,
                        OrderDetails = p.OrderDetails.Select(or => new OrderDetail
                        {
                            ProductName = or.Product.Name,
                            Price = or.Price,
                            Count = or.Count
                        }).ToList()
                    }).ToList(),
                },
                IsSuccess = true,
            };       
        }   
    }
    public class ResultGetOrderForAdminDto
    {
        public int TotalRow { get; set; }
        public List<OrderInfoDto> OrderInfo { get; set; }

    }
    public class OrderInfoDto
    {
        public int Amount { get; set; }
        public long OrderId { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public int ProductCount { get; set; }
        public long RequestPayId { get; set; }
        public State OrderState { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
    public class OrderDetail
    {
        public long OrderId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}
