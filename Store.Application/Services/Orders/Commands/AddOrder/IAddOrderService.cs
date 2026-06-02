using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Commands.AddOrder
{
    public interface IAddOrderService
    {
        ResultDto Execute(long requestPayId);
    }
    public class AddOrderService : IAddOrderService
    {
        private readonly IDataBaseContext _context;
        public AddOrderService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long requestPayId)
        {
            var requestPay = _context.RequestPays
                .Include(p => p.User)
                .Include(p => p.Cart)
                .ThenInclude(p => p.CartItems)
                .Where(p => p.Id == requestPayId).FirstOrDefault();

            requestPay.Cart.Finished = true;

            Order order = new Order()
            {
                User = requestPay.User,
                RequestPay = requestPay,
                OrderState = State.Processing,
            };

            _context.Orders.Add(order);

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach( var item in requestPay.Cart.CartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                OrderDetail orderDetail = new OrderDetail()
                {
                    Order = order,
                    Product = item.Product,
                    Price = product.Price,
                    Count = item.Count,
                };
                orderDetails.Add(orderDetail);
            }

            _context.OrderDetails.AddRange(orderDetails);
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
            };
        }
    }
}
