using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Orders.Commands.ChangeOrderState
{
    public interface IChangeOrderState
    {
        ResultDto Execute(long OrderId,State OrderState);
    }
    public class ChangeOrderState : IChangeOrderState
    {
        private readonly IDataBaseContext _context;
        public ChangeOrderState(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long OrderId, State OrderState)
        {
            var Order = _context.Orders.Find(OrderId);

            if(Order == null)
            {
                return new ResultDto
                {
                    IsSuccess = false
                };
            }

            Order.OrderState = OrderState;
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true
            };
        }
    }
}
