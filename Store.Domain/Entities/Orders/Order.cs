using Store.Domain.Entities.Commons;
using Store.Domain.Entities.Finances;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public virtual User User  { get; set; }
        public long UserId { get; set; }
        public virtual RequestPay RequestPay { get; set; }
        public long RequestPayId { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public State OrderState { get; set; }
    }
    public enum State
    {
        Processing = 1,
        Delivered = 2,      
        Canceled = 3    
    }
}
